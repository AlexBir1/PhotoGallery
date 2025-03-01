
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using PhotoGalleryAPI.Middlewares.AppBuilderExtensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.EntityFrameworkCore;
using PhotoGalleryAPI.DAL.DBContext;
using PhotoGalleryAPI.DAL.Initializer;
using PhotoGalleryAPI.DAL.Repositories;
using PhotoGalleryAPI.Services.Services;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace PhotoGalleryAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Photo Gallery API", Version = "v1" });

                // Add JWT Authentication support to Swagger
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
            });

            var currentDirectory = Directory.GetCurrentDirectory();

            if (!Directory.Exists(currentDirectory + "\\Photos"))
                Directory.CreateDirectory(currentDirectory + "\\Photos");

            builder.Services.AddDbContext<AppDbContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("StrConnection")!);
            });

            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            builder.Services.AddScoped(typeof(IAlbumService), typeof(AlbumService));
            builder.Services.AddScoped(typeof(IPhotoService), typeof(PhotoService));
            builder.Services.AddScoped(typeof(ILikeService), typeof(LikeService));
            builder.Services.AddScoped(typeof(IPersonService), typeof(PersonService));
            builder.Services.AddScoped(typeof(IRoleService), typeof(RoleService));

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(l =>
            {
                l.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
            });

            builder.Services.AddAutoMapper(typeof(Program).Assembly);

            builder.Services.Configure<ApiBehaviorOptions>(opt =>
            {
                opt.SuppressModelStateInvalidFilter = true;
            });

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<AppDbContext>();
                await DBInitializer.Initialize(context);
            }

            app.UseExceptionMiddleware();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(currentDirectory + "\\Photos"),
                RequestPath = new PathString("/Photos")
            });

            app.UseCors(t => t.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.Run();
        }
    }
}
