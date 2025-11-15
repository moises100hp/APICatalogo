using APICatalogo.Context;
using APICatalogo.Extensions;
using APICatalogo.Filters;
using APICatalogo.Logging;
using APICatalogo.Repositories;
using APICatalogo.Repository;
using APICatalogo.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
    {
        options.Filters.Add(typeof(ApiLoggingFilter));
    })
    .AddJsonOptions(options =>
    options.JsonSerializerOptions
        .ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Minha API",
        Version = "v1"
    });

    //options.ExampleFilters(); //Ativa os exemplos nas respostas
});

string mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(mySqlConnection,
        ServerVersion.AutoDetect(mySqlConnection)));

builder.Services.AddTransient<IMeuServico, MeuServico>();
builder.Services.AddScoped<ApiLoggingFilter>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


builder.Logging.ClearProviders();

var logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File(
        path: "C:/logs/log-.txt",
        rollingInterval: RollingInterval.Day,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
        retainedFileCountLimit: 31,
        fileSizeLimitBytes: 5242880 //5 MB
        )
    .CreateLogger();


builder.Host.UseSerilog(logger);

//builder.Services.Configure<ApiBehaviorOptions>(options =>
//{
//    options.DisableImplicitFromServicesParameters = true;
//});

builder.Logging.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfiguration
{
    LogLevel = LogLevel.Information
}));

var app = builder.Build();

//app.UseExceptionHandler(errorApp =>
//{
//    errorApp.Run(async context =>
//    {
//        context.Response.ContentType = "application/json";

//        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

//        if (exception != null)
//        {
//            context.Response.StatusCode = exception switch
//            {
//                ArgumentException => StatusCodes.Status400BadRequest,  // Erro de argumento inválido (400)
//                KeyNotFoundException => StatusCodes.Status404NotFound, // Não encontrado (404)
//                UnauthorizedAccessException => StatusCodes.Status401Unauthorized, // Acesso negado (403)
//                _ => StatusCodes.Status500InternalServerError // Erro interno (500)
//            };
//        }

//        //await context.Response.WriteAsJsonAsync(errorResponse);
//    });
//});

app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ConfigureExceptionHandler();
}

//app.UseReDoc(c =>
//{
//    c.RoutePrefix = "redoc";
//    c.DocumentTitle = "Documentação ReDoc";
//    c.SpecUrl("/swagger/v1/swagger.json");// Arquivo JSON da documentação

//    //Configurações visuais

//    c.ExpandResponses("200,201,500"); // Expande as respostas de sucesso
//    c.HideDownloadButton(); // Oculta botão de download do JSON
//    c.RequiredPropsFirst(); // Prioriza campos obrigatórios
//    c.SortPropsAlphabetically(); // Ordena propriedades alfabeticamente
//});

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
