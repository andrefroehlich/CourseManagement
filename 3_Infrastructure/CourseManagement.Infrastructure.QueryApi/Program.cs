using CourseManagement.Application.Queries;
using CourseManagement.Application.Services;
using CourseManagement.Infrastructure.Marten;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSingleton<GetAllCoursesQueryHandler>();
builder.Services.AddSingleton<GetCourseByIdQueryHandler>();
builder.Services.AddSingleton<GetCourseHistoryByIdQueryHandler>();
builder.Services.AddSingleton<ICourseReadModelRepository, MartenCourseReadModelRepository>();

builder.SetupMarten();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUi(options =>
    {
        options.DocumentPath = "openapi/v1.json";
        options.DocumentTitle = "Query API Swagger UI";
    });
}

app.UseHttpsRedirection();

app.MapGet("/courses", async (
        GetAllCoursesQueryHandler handler, 
        CancellationToken cancellationToken) 
    => await handler.Handle(new GetAllCoursesQuery(), cancellationToken));

app.MapGet("/courses/{courseId}", async (
        Guid courseId, 
        GetCourseByIdQueryHandler handler, 
        CancellationToken cancellationToken) 
    => await handler.Handle(new GetCourseByIdQuery(courseId), cancellationToken));

app.MapGet("/courses/{courseId}/history", async (
        Guid courseId, 
        GetCourseHistoryByIdQueryHandler handler, 
        CancellationToken cancellationToken) 
    => await handler.Handle(new GetCourseHistoryByIdQuery(courseId), cancellationToken));

app.Run();
