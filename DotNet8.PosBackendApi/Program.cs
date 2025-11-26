var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(
            "https://localhost:7136",
            "http://localhost:5065",
            "https://localhost:7288",
            "http://localhost:5048"
            ) // or AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin();
    });
});


//string projectDirectory = Environment.CurrentDirectory;
//var builderJwtSetting = new ConfigurationBuilder();
//builderJwtSetting.SetBasePath(projectDirectory)
//    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
//IConfiguration configJwtSetting = builderJwtSetting.Build();
//builder.Services.Configure<JwtModel>(configJwtSetting.GetSection("Jwt"));

builder.Services.Configure<JwtModel>(builder.Configuration.GetSection("Jwt"));

#region Register Services

string connectionString = builder.Configuration.GetConnectionString("DbConnection")!;

builder.Services.AddScoped(n => new DapperService(connectionString));
builder.Services.AddAppDbContextService(connectionString);
builder.Services.AddServices();

#endregion

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();