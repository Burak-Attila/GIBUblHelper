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

        group.MapPost("/process-full", ProcessUploadFullAsync)
            .WithName("ProcessUblPayloadFull")
            .WithSummary("UBL envelope / Package / Invoice / ZIP yükler; özet + tam tipli belgeleri döner (InvoiceType, DespatchAdviceType, ...).")
            .DisableAntiforgery();

        group.MapPost("/process-full-raw", ProcessRawFullAsync)
            .WithName("ProcessUblPayloadFullRaw")
            .WithSummary("Raw body olarak UBL/ZIP alır; tam tipli belgeleri döner.")
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
        var (bytes, name, error) = await ReadUploadAsync(file, cancellationToken);
        if (error is not null)
            return TypedResults.BadRequest(error);
        return Execute(bytes!, name, logger, () => processor.Process(bytes!, name));
    }

    private static async Task<Results<Ok<UblProcessingResult>, BadRequest<ProblemDetails>, StatusCodeHttpResult>>
        ProcessRawAsync(
            HttpRequest request,
            IEnvelopeProcessor processor,
            ILogger<UblProcessingResult> logger,
            CancellationToken cancellationToken)
    {
        var (bytes, name, error) = await ReadRawAsync(request, cancellationToken);
        if (error is not null)
            return TypedResults.BadRequest(error);
        return Execute(bytes!, name, logger, () => processor.Process(bytes!, name));
    }

    private static async Task<Results<Ok<UblFullProcessingResult>, BadRequest<ProblemDetails>, StatusCodeHttpResult>>
        ProcessUploadFullAsync(
            IFormFile file,
            IEnvelopeProcessor processor,
            ILogger<UblFullProcessingResult> logger,
            CancellationToken cancellationToken)
    {
        var (bytes, name, error) = await ReadUploadAsync(file, cancellationToken);
        if (error is not null)
            return TypedResults.BadRequest(error);
        return Execute(bytes!, name, logger, () => processor.ProcessFull(bytes!, name));
    }

    private static async Task<Results<Ok<UblFullProcessingResult>, BadRequest<ProblemDetails>, StatusCodeHttpResult>>
        ProcessRawFullAsync(
            HttpRequest request,
            IEnvelopeProcessor processor,
            ILogger<UblFullProcessingResult> logger,
            CancellationToken cancellationToken)
    {
        var (bytes, name, error) = await ReadRawAsync(request, cancellationToken);
        if (error is not null)
            return TypedResults.BadRequest(error);
        return Execute(bytes!, name, logger, () => processor.ProcessFull(bytes!, name));
    }

    private static async Task<(byte[]? Bytes, string? FileName, ProblemDetails? Error)>
        ReadUploadAsync(IFormFile file, CancellationToken cancellationToken)
    {
        if (file is null || file.Length == 0)
            return (null, null, new ProblemDetails
            {
                Title = "Boş dosya",
                Detail = "Yüklenen dosya bulunamadı veya boş."
            });

        await using var stream = file.OpenReadStream();
        using var ms = new MemoryStream(capacity: (int)Math.Min(file.Length, int.MaxValue));
        await stream.CopyToAsync(ms, cancellationToken);
        return (ms.ToArray(), file.FileName, null);
    }

    private static async Task<(byte[]? Bytes, string? FileName, ProblemDetails? Error)>
        ReadRawAsync(HttpRequest request, CancellationToken cancellationToken)
    {
        using var ms = new MemoryStream();
        await request.Body.CopyToAsync(ms, cancellationToken);
        if (ms.Length == 0)
            return (null, null, new ProblemDetails
            {
                Title = "Boş body",
                Detail = "İstek gövdesi boş."
            });

        var fileName = request.Headers.TryGetValue("X-File-Name", out var n) ? n.ToString() : null;
        return (ms.ToArray(), fileName, null);
    }

    private static Results<Ok<T>, BadRequest<ProblemDetails>, StatusCodeHttpResult>
        Execute<T>(byte[] bytes, string? fileName, ILogger logger, Func<T> action) where T : class
    {
        try
        {
            return TypedResults.Ok(action());
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
