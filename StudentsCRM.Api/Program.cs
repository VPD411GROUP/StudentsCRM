using StudentsCRM.Data;
using StudentsCRM.Interfaces;
using StudentsCRM.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<StudentsDbContext>();

builder.Services.AddControllers();

builder.Services.AddScoped<IStudentsRepository, StudentsRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app =  builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();