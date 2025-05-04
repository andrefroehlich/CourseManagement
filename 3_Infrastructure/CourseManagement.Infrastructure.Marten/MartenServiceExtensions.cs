using CourseManagement.Application.ReadModels;
using CourseManagement.Infrastructure.Marten.Projections;
using Marten;
using Marten.Events.Projections;
using Microsoft.Extensions.Hosting;
using Weasel.Core;

namespace CourseManagement.Infrastructure.Marten;

public static class MartenServiceExtensions
{
    public static IHostApplicationBuilder SetupMarten(this IHostApplicationBuilder builder)
    {
        builder.AddNpgsqlDataSource("postgres");
        builder.Services.AddMarten(options =>
        {
            options.UseSystemTextJsonForSerialization();
            options.Projections.Add<CourseReadModelProjection>(ProjectionLifecycle.Inline);
            options.Projections.Add<CourseHistoryReadModelProjection>(ProjectionLifecycle.Inline);
            options.Projections.Snapshot<CourseOverviewSelfProjectedReadModel>(SnapshotLifecycle.Inline); // Inline make a new snapshot after each event, model projects itself
            
            if (builder.Environment.IsDevelopment())
            {
                options.AutoCreateSchemaObjects = AutoCreate.All;
            }
        }).UseNpgsqlDataSource();
        
        return builder;
    }
}