using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Metrics.Core;
using App.Metrics.Data;
using App.Metrics.Reporting.Interfaces;

namespace App.Metrics.Facts.Reporting.Helpers
{
    public class TestMetricReporter : IMetricReporter
    {
        private readonly bool _pass;
        private readonly Exception _throwEx;

        public TestMetricReporter(bool pass, TimeSpan reportInterval, Exception throwEx = null)
        {
            _pass = pass;
            _throwEx = throwEx;
            ReportInterval = reportInterval;
        }
        public void Dispose()
        {
        }

        public string Name { get; } = "Test Reporter";

        public TimeSpan ReportInterval { get; }

        public Task<bool> EndAndFlushReportRunAsync(IMetrics metrics)
        {
            if (_throwEx != null)
            {
                throw _throwEx;
            }

            return Task.FromResult(_pass);
        }

        public void ReportEnvironment(EnvironmentInfo environmentInfo)
        {
            
        }

        public void ReportHealth(GlobalMetricTags globalTags, IEnumerable<HealthCheck.Result> healthyChecks, IEnumerable<HealthCheck.Result> degradedChecks, IEnumerable<HealthCheck.Result> unhealthyChecks)
        {
        }

        public void ReportMetric<T>(string context, MetricValueSource<T> valueSource)
        {
        }

        public void StartReportRun(IMetrics metrics)
        {
        }
    }
}