using Core.Ubl.Processing.DependencyInjection;
using Core.Ubl.Processing.Options;
using GIBUblHelper.Api.Endpoints;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

var processingLimits = builder.Configuration
    .GetSection(UblProcessingOptions.SectionName)
    .Get<UblProcessingOptions>() ?? new UblProcessingOptions();

builder.Services.AddUblProcessing(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "GIB UBL Helper API", Version = "v1" });
});

builder.Services.Configure<FormOptions>(form =>
{
    form.MultipartBodyLengthLimit = processingLimits.MaxInputSizeBytes;
    form.ValueLengthLimit = int.MaxValue;
});

builder.Services.Configure<KestrelServerOptions>(kestrel =>
{
    kestrel.Limits.MaxRequestBodySize = processingLimits.MaxInputSizeBytes;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapUblEndpoints();

app.MapGet("/", () => Results.Redirect("/swagger"))
    .ExcludeFromDescription();

app.Run();
