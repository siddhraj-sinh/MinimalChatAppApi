using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MinimalChatAppApi.Data;
using System.Text;

namespace MinimalChatAppApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<ChatContext>(options =>
  options.UseSqlServer(builder.Configuration.GetConnectionString("ChatContext")));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = "MimnalChatClient",
                    ValidIssuer = "MinimalChatServer",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:Token"]))
                };
            });

            var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var context = services.GetRequiredService<ChatContext>();
                context.Database.EnsureCreated();
                // DbInitializer.Initialize(context);
            }


            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseAuthentication();

            app.MapControllers();

            app.Run();
        }
    }
}