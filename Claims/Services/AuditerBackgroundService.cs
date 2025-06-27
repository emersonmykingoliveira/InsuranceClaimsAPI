using Claims.DbContexts;

namespace Claims.Services
{
    public class AuditerBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public AuditerBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    /*
                    Since the hosted service AuditerBackgroundService is singleton by default,
                    this will resolve the scoped services within a scope
                    */
                    var auditerService = scope.ServiceProvider.GetRequiredService<IAuditerService>();
                    var auditContext = scope.ServiceProvider.GetRequiredService<AuditContext>();

                    if (auditerService.TryDequeue(out var audit))
                    {
                        await auditContext.AddAsync(audit, stoppingToken);
                        await auditContext.SaveChangesAsync(stoppingToken);
                    }
                    else
                    {
                        await Task.Delay(500, stoppingToken);
                    }
                }
            }
        }
    }
}
