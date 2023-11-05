using ElasticSearch.API.Extensions;
using ElasticSearch.API.Services;
using ElasticSearch.API.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddElastic(builder.Configuration);

//AddScoped olarak ProductService ekliyoruz.
builder.Services.AddScoped<ProductService>();
//AddScoped olarak ProductRepository ekliyoruz.
builder.Services.AddScoped<ProductRepository>();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
