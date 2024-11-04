using Asp.Versioning.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Api.AppScanner.BarCode.Rotary.EF.DataContext;
;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

#if DEBUG
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
#else
    var connectionString = builder.Configuration.GetConnectionString("ProdutionConnection");
#endif

builder.Services.AddDbContext<AppDbContext>(options => {
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        policyBuilder => policyBuilder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

builder.Services.AddApiVersioning()
                .AddMvc()
                .AddApiExplorer(setup =>
                {
                    setup.GroupNameFormat = "'v'VVV";
                    setup.SubstituteApiVersionInUrl = true;
                });

var app = builder.Build();

// Aplicar migra��es automaticamente no in�cio da aplica��o
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        db.Database.Migrate();
        Console.WriteLine("Migra��es aplicadas com sucesso.");
    }
    catch (Exception ex)
    {
        Console.WriteLine("Erro ao aplicar migra��es: " + ex.Message);
        throw new InvalidOperationException(
            "As migra��es do banco de dados est�o desatualizadas. Verifique se todas as migra��es foram aplicadas corretamente.",
            ex
        );
    }
}

//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    var version = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    foreach (var description in version.ApiVersionDescriptions)
    {
        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", $"WebApi - {description.GroupName.ToUpper()}");
    }
});
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
