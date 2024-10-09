using IdentityAutenticacao_INFRA.DbContext;
using IdentityAutenticacao.API.Identity;
using IdentityAutenticacao.API.Services;
using IdentityAutenticacao.API.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ApplicationUser = IdentityAutenticacao_DOMAIN.Models.ApplicationUser;

namespace IdentityAutenticacao.API;

public class Startup
{
      public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        // Configurar o banco de dados (Exemplo com SQL Server)
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

        // Configurar Identity
        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
        
        services.AddScoped<IUserService, UserService>();
        // Configurar autenticação com JWT
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = Configuration["Jwt:Issuer"],
                ValidAudience = Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    System.Text.Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
            };
        });

        // Adicionar serviços para controladores
        services.AddControllers();

        // Adicionar Swagger
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "API de Autenticação",
                Version = "v1",
                Description = "API para autenticação usando Identity e JWT",
            });
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        // Configure o middleware do Swagger
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "API de Autenticação v1");
            c.RoutePrefix = string.Empty; // Para acessar no root
        });

        app.UseRouting();
        
        app.UseAuthentication(); // Necessário para habilitar a autenticação
        app.UseAuthorization();  // Necessário para habilitar a autorização

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}