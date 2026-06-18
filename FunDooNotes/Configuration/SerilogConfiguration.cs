using Serilog;

namespace FunDooNotes.Configuration
{
    public static class SerilogConfiguration
    {
        public static void ConfigureSerilog(WebApplicationBuilder builder)
        {
            Log.Logger = new LoggerConfiguration()

                .MinimumLevel.Information()

                .WriteTo.Console()

                .WriteTo.File(
                    "Logs/log-.txt",
                    rollingInterval: RollingInterval.Day)

                .CreateLogger();

            builder.Host.UseSerilog();
        }
    }
}