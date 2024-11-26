using Service.Configs;
using Service.Handlers;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container.

builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
{
    options.InvalidModelStateResponseFactory = InvalidModelStateResponseHandler.GetErrorResponse;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.RegisterServices();
builder.Services.RegisterTypes();
builder.Services.AddExceptionHandler<GlobalErrorHandler>();



var app = builder.Build();

app.UseExceptionHandler(_ => { });
// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
    app.UseSwaggerUI();

//}

//app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
