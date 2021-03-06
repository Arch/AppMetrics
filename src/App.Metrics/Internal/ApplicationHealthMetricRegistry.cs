using App.Metrics.Core.Options;

namespace App.Metrics.Internal
{
    public static class ApplicationHealthMetricRegistry
    {
        public static readonly string Context = "Health";

        public static CounterOptions DegradedCheckCounter = new CounterOptions
        {
            Context = Context,
            Name = "Degraded",
            MeasurementUnit = Unit.Items,
            ResetOnReporting = true,
            ReportItemPercentages = true,
            ReportSetItems = true
        };

        public static CounterOptions HealthyCheckCounter = new CounterOptions
        {
            Context = Context,
            Name = "Healthy",
            MeasurementUnit = Unit.Items,
            ResetOnReporting = true,
            ReportItemPercentages = true,
            ReportSetItems = true
        };

        public static CounterOptions UnhealthyCheckCounter = new CounterOptions
        {
            Context = Context,
            Name = "UnHealthy",
            MeasurementUnit = Unit.Items,
            ResetOnReporting = true,
            ReportItemPercentages = true,
            ReportSetItems = true
        };
    }
}