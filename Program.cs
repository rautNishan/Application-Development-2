using CourseWork.Common.Helper.EmailService;
using CourseWork.Common.Middlewares.Auth;
using CourseWork.Common.Middlewares.Errors;
using CourseWork.Common.Middlewares.Response;
using CourseWork.Modules.Admin.Repository;
using CourseWork.Modules.Admin.Services;
using CourseWork.Modules.Auth.Services;
using CourseWork.Modules.Blogs.Repository;
using CourseWork.Modules.Blogs.Services;
using CourseWork.Modules.user.repository;
using CourseWork.Modules.User.Services;
using dotenv.net;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MyAppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("AppConnectionString")));



// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => //swaggerGen method takes a configuration action that makes it possible to customize the behavior of the swagger generator
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });


    c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Scheme = "oauth2",
                    Name = "Bearer",
                    In = ParameterLocation.Header,
                },
                new List<string>()
            }
        });

    //Swagger document for Admin APIs
    c.SwaggerDoc("admin", new OpenApiInfo { Title = "Admin API", Version = "v1" });

    // Swagger document for User APIs
    c.SwaggerDoc("user", new OpenApiInfo { Title = "User API", Version = "v1" });



    // Decides which controller action (Api Endpoints) should be included in the swagger documentation
    c.DocInclusionPredicate((docName, apiDesc) =>
    {
        //Check if the api has a group name and the group name matches the name of the swagger document ignoring case
        if (apiDesc.GroupName != null && apiDesc.GroupName.Equals(docName, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }
        return false;
    });
});


// Users Injectable
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<UserService>();

//Auth Injectable
builder.Services.AddScoped<AuthService>();

// Add services to the container.
builder.Services.AddScoped<RoleAuthFilter>();

//Admin Injectable
builder.Services.AddScoped<AdminRepository>();
builder.Services.AddScoped<AdminService>();

//Blogs Injectable
builder.Services.AddScoped<BlogRepository>();
builder.Services.AddScoped<BlogService>();

//Helper Injectable
builder.Services.AddScoped<EmailService>();


// Define CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});


var app = builder.Build();
// Apply CORS middleware
app.UseCors("AllowAnyOrigin");

app.UseMiddleware<ErrorFilter>();
app.UseMiddleware<ResponseInterceptor>();

//Seeding Admin
using (var scope = app.Services.CreateScope())
{
    var adminService = scope.ServiceProvider.GetRequiredService<AdminService>();
    adminService.SeedAdmin().Wait(); // This ensures the method runs and completes before continuing.
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/admin/swagger.json", "Admin API");
        c.SwaggerEndpoint("/swagger/user/swagger.json", "User API");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

DotEnv.Load();
app.Run();
