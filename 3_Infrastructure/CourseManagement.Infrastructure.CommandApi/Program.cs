using CourseManagement.Application.Commands;
using CourseManagement.Application.Services;
using CourseManagement.Infrastructure.Marten;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSingleton<RegisterCourseCommandHandler>();
builder.Services.AddSingleton<RegisterParticipantCommandHandler>();
builder.Services.AddSingleton<WithdrawParticipantCommandHandler>();
builder.Services.AddSingleton<IAggregateRepository, MartenAggregateRepository>();

builder.SetupMarten();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUi(options =>
    {
        options.DocumentPath = "openapi/v1.json";
        options.DocumentTitle = "Command API Swagger UI";
    });
}

app.UseHttpsRedirection();

app.MapPost("/courses", async (
        RegisterCourseCommand command, 
        RegisterCourseCommandHandler handler, 
        CancellationToken cancellationToken)
    => await handler.Handle(command, cancellationToken));

app.MapPost("/courses/{courseId}/participant-registrations", async (
    Guid courseId,
    RegisterParticipantCommand command,
    RegisterParticipantCommandHandler handler,
    CancellationToken cancellationToken
) =>
{
    if(courseId != command.CourseId) throw new ArgumentException("Course ID does not match", nameof(courseId));
    await handler.Handle(command, cancellationToken);
});

app.MapPost("/courses/{courseId}/participant-withdrawals", async (
    Guid courseId,
    WithdrawParticipantCommand command,
    WithdrawParticipantCommandHandler handler,
    CancellationToken cancellationToken
) =>
{
    if (courseId != command.CourseId) throw new ArgumentException("Course ID does not match", nameof(courseId));
    await handler.Handle(command, cancellationToken);
});

app.Run();
