using Core.Ubl.Processing.Abstractions;
using Core.Ubl.Processing.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace GIBUblHelper.Api.Endpoints;

public static class UblEndpoints
{
    public static IEndpointRouteBuilder MapUblEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/ubl").WithTags("UBL");

        group.MapPost("/process", ProcessUploadAsync)
            .WithName("ProcessUblPayload")
            .WithSummary("UBL envelope / Package / Invoice / ZIP yükler ve özetini döner.")
            .DisableAntiforgery();

        group.MapPost("/process-raw", ProcessRawAsync)
            .WithName("ProcessUblPayloadRaw")
            .WithSummary("UBL/ZIP payload'ı raw body olarak alır ve özet döner.")
            .Accepts<byte[]>("application/octet-stream", "application/xml", "application/zip", "text/xml")
            .DisableAntiforgery();

        return app;
    }

    private static async Task<Results<Ok<UblProcessingResult>, BadRequest<ProblemDetails>, StatusCodeHttpResult>>
        ProcessUploadAsync(
            IFormFile file,
            IEnvelopeProcessor processor,
            ILogger<UblProcessingResult> logger,
            CancellationToken cancellationToken)
    {
        if (file is null || file.Length == 0)
            return TypedResults.BadRequest(new ProblemDetails
            {
                Title = "Boş dosya",
                Detail = "Yüklenen dosya bulunamadı veya boş."
            });

        await using var stream = file.OpenReadStream();
        using var ms = new MemoryStream(capacity: (int)Math.Min(file.Length, int.MaxValue));
        await stream.CopyToAsync(ms, cancellationToken);

        return ExecuteProcess(ms.ToArray(), file.FileName, processor, logger);
    }

    private static async Task<Results<Ok<UblProcessingResult>, BadRequest<ProblemDetails>, StatusCodeHttpResult>>
        ProcessRawAsync(
            HttpRequest request,
            IEnvelopeProcessor processor,
            ILogger<UblProcessingResult> logger,
            CancellationToken cancellationToken)
    {
        using var ms = new MemoryStream();
        await request.Body.CopyToAsync(ms, cancellationToken);
        if (ms.Length == 0)
            return TypedResults.BadRequest(new ProblemDetails
            {
                Title = "Boş body",
                Detail = "İstek gövdesi boş."
            });

        var fileName = request.Headers.TryGetValue("X-File-Name", out var n) ? n.ToString() : null;
        return ExecuteProcess(ms.ToArray(), fileName, processor, logger);
    }

    private static Results<Ok<UblProcessingResult>, BadRequest<ProblemDetails>, StatusCodeHttpResult>
        ExecuteProcess(byte[] bytes, string? fileName, IEnvelopeProcessor processor, ILogger logger)
    {
        try
        {
            var result = processor.Process(bytes, fileName);
            return TypedResults.Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning(ex, "UBL payload reddedildi: {FileName}", fileName);
            return TypedResults.BadRequest(new ProblemDetails
            {
                Title = "Geçersiz payload",
                Detail = ex.Message
            });
        }
        catch (NotSupportedException ex)
        {
            logger.LogWarning(ex, "Desteklenmeyen UBL tipi: {FileName}", fileName);
            return TypedResults.BadRequest(new ProblemDetails
            {
                Title = "Desteklenmeyen belge tipi",
                Detail = ex.Message
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "UBL işleme hatası: {FileName}", fileName);
            return TypedResults.StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
