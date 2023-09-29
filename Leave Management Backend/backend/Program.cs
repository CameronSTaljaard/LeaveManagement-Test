using backend.Data;
using backend.Data.Concrete;
using backend.Data.Interfaces;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using System.Web.Http;
using Microsoft.AspNetCore.Authorization;
using backend.Enums;
using backend.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthorization();
builder.Services.AddAuthorization(o => o.AddPolicy("AdminsOnly",
                                  b => b.RequireClaim(ClaimTypes.Role, UserType.Admin.ToString())));
builder.Services.AddCors();
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
});



var app = builder.Build();

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider
        .GetRequiredService<DataContext>();

    // Here is the migration executed
    dbContext.Database.Migrate();
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/login", (string username, IUnitOfWork unitOfWork) =>
{
    var User = unitOfWork.UserRepository.Get(u => u.Username == username).FirstOrDefault();

    if (User == null) return Results.Unauthorized();

    string jwtKey = "nMF6aOoUYUtviFUg7oZU";
    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
    var claims = new[]
    {
        new Claim(ClaimTypes.NameIdentifier,User.Username),
        new Claim(ClaimTypes.Role, User.UserType.ToString()),
    };
    var token = new JwtSecurityToken(null, null, claims: claims, expires: DateTime.Now.AddMinutes(15), signingCredentials: credentials);

    return Results.Ok(new JwtSecurityTokenHandler().WriteToken(token));
})
.WithName("Login");


app.MapGet("/getUsers", (IUnitOfWork unitOfWork) =>
{
    var val = unitOfWork.UserRepository.Get(includeProperties: new string[] { "Requests", "Resolvements" });

    return Results.Ok(val);
})
.WithName("GetUsers");

app.MapGet("/getAllRequests", (IUnitOfWork unitOfWork) =>
{
    var val = unitOfWork.RequestRepository.Get(includeProperties: new string[] { "User", "Resolvement" });

    return val;
})
.WithName("GetRequests");

app.MapGet("/getAllRequestsForUser", (int userId, IUnitOfWork unitOfWork) =>
{
    var val = unitOfWork.RequestRepository.Get(
        filter: x => x.UserId == userId,
        includeProperties: new string[] { "Resolvement" }
        );

    return val;
})
.WithName("GetRequestsForUser");

app.MapGet("/getAllResolvements", (IUnitOfWork unitOfWork) =>
{
    var val = unitOfWork.ResolvementRepository.Get();

    return Results.Ok(val);
})
.WithName("GetResolvements");

app.MapPost("/getUser", (int id, IUnitOfWork unitOfWork) =>
{
    var val = unitOfWork.UserRepository.Find(x=>x.Id == id);

    return Results.Ok(val);
})
.WithName("GetUser");

app.MapPost("/getRequest", (int id, IUnitOfWork unitOfWork) =>
{
    var val = unitOfWork.RequestRepository.Find(x=>x.Id == id, includeProperties: new string[] { "User", "Resolvement", "Resolvement.Admin" });

    return Results.Ok(val);
})
.WithName("GetRequest");

app.MapPost("/getResolvement", (int id, IUnitOfWork unitOfWork) =>
{
    var val = unitOfWork.ResolvementRepository.Find(x => x.Id == id, includeProperties: new string[] { "Admin", "Request" });

    return Results.Ok(val);
})
.WithName("GetResolvement");


app.MapPost("/addRequest", (Request request, IUnitOfWork unitOfWork) =>
{
    if (request == null) return Results.BadRequest();
    unitOfWork.RequestRepository.Insert(request);
    unitOfWork.Save();
    return Results.Ok();
}).WithName("AddRequest");

app.MapPost("/addResolvement", (Resolvement resolvement, IUnitOfWork unitOfWork) =>
{
    if (resolvement == null) return Results.BadRequest();

    var request = unitOfWork.RequestRepository.Find(x => x.Id == resolvement.RequestId);
    if (request == null) return Results.BadRequest();

    resolvement.Request = request;
    unitOfWork.ResolvementRepository.Insert(resolvement);
    unitOfWork.Save();
    return Results.Ok();
}).WithName("AddResolvement");


app.Run();
