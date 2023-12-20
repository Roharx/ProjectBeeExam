using BeeProject.Middleware;
using infrastructure;
using infrastructure.Repositories;
using service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
const string frontendRelPath = "../frontend/www/";
builder.Services.AddSpaStaticFiles(conf => conf.RootPath = frontendRelPath);

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddNpgsqlDataSource(Utilities.ProperlyFormattedConnectionString,
        dataSourceBuilder => dataSourceBuilder.EnableParameterLogging());
}

if (builder.Environment.IsProduction())
{
    builder.Services.AddNpgsqlDataSource(Utilities.ProperlyFormattedConnectionString);
}

builder.Services.AddSingleton<ITokenService, TokenService>();
// Repositories
builder.Services.AddSingleton<AccountRepository>();
builder.Services.AddSingleton<AilmentRepository>();
builder.Services.AddSingleton<BeeRepository>();
builder.Services.AddSingleton<FieldRepository>();
builder.Services.AddSingleton<HarvestRepository>();
builder.Services.AddSingleton<HiveRepository>();
builder.Services.AddSingleton<HoneyRepository>();
builder.Services.AddSingleton<InventoryRepository>();
builder.Services.AddSingleton<TaskRepository>();
// Services
builder.Services.AddSingleton<AccountService>();
builder.Services.AddSingleton<AilmentService>();
builder.Services.AddSingleton<BeeService>();
builder.Services.AddSingleton<FieldService>();
builder.Services.AddSingleton<HarvestService>();
builder.Services.AddSingleton<HiveService>();
builder.Services.AddSingleton<HoneyService>();
builder.Services.AddSingleton<InventoryService>();
builder.Services.AddSingleton<TaskService>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(options =>
{
    options.SetIsOriginAllowed(origin => true)
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
});

app.UseSpaStaticFiles();
app.UseSpa(conf =>
{
    conf.Options.SourcePath = frontendRelPath;
});

app.MapControllers();
app.UseMiddleware<GlobalExceptionHandler>();
app.Run();
