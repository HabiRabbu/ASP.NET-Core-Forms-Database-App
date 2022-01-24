using ISSProjectFINAL;
using ISSProjectFINAL.Data;
using ISSProjectFINAL.Models;
using ISSProjectFINAL.Repos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSqlite<AppDbContext>("Data Source=FormsApp.db");

builder.Services.AddSingleton<FormRepo>();
builder.Services.AddSingleton<UserRepo>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var key = Encoding.ASCII.GetBytes("this is my custom secret key for authentication");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});
builder.Services.AddAuthorization(options =>
{
    //options.FallbackPolicy = new AuthorizationPolicyBuilder()
    //.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
    //.RequireAuthenticatedUser()
    //.Build();
});

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.MapPost("/login", ([FromServices] UserRepo userRepository, User user) =>
{
    var login = userRepository.Get(user.Username, user.Password);
    if (login == null) return Results.NotFound();

    var token = TokenService.GenerateToken(login);
    login.Password = string.Empty;
    return Results.Ok(new
    {
        user = login,
        token
    });
});


app.MapGet("/forms", ([FromServices] FormRepo repository, ClaimsPrincipal user) =>
{
    var userRoles = user.Claims.Where(x => x.Type == ClaimTypes.Role).Select(y => y.Value);
    return repository.GetAll().Where(x => userRoles.Contains(x.Category.Title));
});

app.MapGet("/forms/{id}", ([FromServices] FormRepo repository, long id, ClaimsPrincipal user) =>
{
    var userRoles = user.Claims.Where(x => x.Type == ClaimTypes.Role).Select(y => y.Value);
    var form = repository.GetById(id);
    if (!userRoles.Contains(form.Category.Title)) return Results.Unauthorized();
    return form is not null ? Results.Ok(form) : Results.NotFound();
});

app.MapPost("/forms", ([FromServices] FormRepo repository, Form form, ClaimsPrincipal user) =>
{
    var userRoles = user.Claims.Where(x => x.Type == ClaimTypes.Role).Select(y => y.Value);
    if (!userRoles.Contains(form.Category.Title)) return Results.Unauthorized();

    repository.Create(form);
    return Results.Created($"/forms/{form.Id}", form);
});

app.MapPut("/forms/{id}", ([FromServices] FormRepo repository, long id, Form updatedForm, ClaimsPrincipal user) =>
{
    var userRoles = user.Claims.Where(x => x.Type == ClaimTypes.Role).Select(y => y.Value);

    var form = repository.GetById(id);
    if (form is null) return Results.NotFound();
    if (!userRoles.Contains(form.Category.Title)) return Results.Unauthorized();

    repository.Update(form);
    return Results.Ok(updatedForm);
});

app.MapDelete("/forms/{id}", ([FromServices] FormRepo repository, long id, ClaimsPrincipal user) =>
{
    var userRoles = user.Claims.Where(x => x.Type == ClaimTypes.Role).Select(y => y.Value);

    var form = repository.GetById(id);
    if (!userRoles.Contains(form.Category.Title)) return Results.Unauthorized();

    repository.Delete(id);
    return Results.Ok();
});

app.Run();
