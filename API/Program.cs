using API.Data;
using API.Service.TokenServices;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var conf = builder.Configuration.GetConnectionString("connstring");
builder.Services.AddDbContext<DataContext>(options=>options.UseSqlite(conf));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddCors(options=>options.AddDefaultPolicy(builder=>builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ITokenService, TokenService>();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors();
app.UseAuthorization();
app.MapControllers();
app.Run();
