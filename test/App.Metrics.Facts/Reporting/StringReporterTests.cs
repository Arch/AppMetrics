﻿using System;
using System.Collections.Generic;
using System.Linq;
using App.Metrics.Apdex;
using App.Metrics.Core;
using App.Metrics.Data;
using App.Metrics.Facts.Reporting.Helpers;
using App.Metrics.Internal;
using App.Metrics.Internal.Test;
using App.Metrics.Reporting;
using App.Metrics.Sampling;
using App.Metrics.Utils;
using Castle.Core.Internal;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Reporting
{
    public class StringReporterTests
    {
        [Fact]
        public void can_report_apdex()
        {
            var expected = StringReporterSamples.Apdex.ExtractStringReporterSampleFromResourceFile();
            var clock = new TestClock();
            var sr = new StringReporter();
            var reservoir = new ExponentiallyDecayingReservoir(Constants.ReservoirSampling.DefaultSampleSize,
                Constants.ReservoirSampling.DefaultExponentialDecayFactor, clock, new TestTaskScheduler(clock));
            var metric = new ApdexMetric(new ApdexProvider(reservoir, Constants.ReservoirSampling.DefaultApdexTSeconds), clock, true);

            metric.Track(1000);

            sr.ReportMetric("test", new ApdexValueSource("apdex_name", metric, MetricTags.None));

            AssertReportResult(sr.Result, expected);
        }

        [Fact]
        public void can_report_counters()
        {
            var expected = StringReporterSamples.Counters.ExtractStringReporterSampleFromResourceFile();
            var sr = new StringReporter();

            sr.ReportMetric("test", new CounterValueSource("counter_name", new CounterMetric(), Unit.None, MetricTags.None));

            AssertReportResult(sr.Result, expected);
        }

        [Fact]
        public void can_report_environment_info()
        {
            var expected = StringReporterSamples.EnvironmentInfo.ExtractStringReporterSampleFromResourceFile();
            var envInfo = new EnvironmentInfo("assembly", "entry", "host", "time", "machine", "OS", "OS version", "process", "4");
            var sr = new StringReporter();

            sr.ReportEnvironment(envInfo);

            AssertReportResult(sr.Result, expected);
        }

        [Fact]
        public void can_report_gauges()
        {
            var expected = StringReporterSamples.Gauges.ExtractStringReporterSampleFromResourceFile();
            var sr = new StringReporter();

            sr.ReportMetric("test", new GaugeValueSource("gauge_name", new FunctionGauge(() => 2), Unit.None, MetricTags.None));

            AssertReportResult(sr.Result, expected);
        }

        [Fact]
        public void can_report_health_checks()
        {
            var expected = StringReporterSamples.HealthChecks.ExtractStringReporterSampleFromResourceFile();
            var globalTags = new GlobalMetricTags(new Dictionary<string, string> { { "tag_key", "tag_value" } });

            var healthyChecks = new[]
            {
                new HealthCheck.Result("healthy check", HealthCheckResult.Healthy("healthy message"))
            }.AsEnumerable();

            var degradedChecks = new[]
            {
                new HealthCheck.Result("degraded check", HealthCheckResult.Degraded("degraded message"))
            }.AsEnumerable();

            var unhealthyChecks = new[]
            {
                new HealthCheck.Result("unhealthy check", HealthCheckResult.Unhealthy("unhealthy message"))
            }.AsEnumerable();

            var sr = new StringReporter();

            sr.ReportHealth(globalTags, healthyChecks, degradedChecks, unhealthyChecks);

            AssertReportResult(sr.Result, expected);
        }

        [Fact]
        public void can_report_histograms()
        {
            var expected = StringReporterSamples.Histograms.ExtractStringReporterSampleFromResourceFile();
            var sr = new StringReporter();
            var metric = new HistogramMetric(SamplingType.ExponentiallyDecaying, Constants.ReservoirSampling.DefaultSampleSize,
                Constants.ReservoirSampling.DefaultExponentialDecayFactor);

            metric.Update(1000, "value1");
            metric.Update(2000, "value2");

            sr.ReportMetric("test", new HistogramValueSource("histogram_name", metric, Unit.None, MetricTags.None));

            AssertReportResult(sr.Result, expected);
        }

        [Fact]
        public void can_report_meters()
        {
            var expected = StringReporterSamples.Meters.ExtractStringReporterSampleFromResourceFile();
            var clock = new TestClock();
            var sr = new StringReporter();
            var metric = new MeterMetric(clock, new TestTaskScheduler(clock));
            metric.Mark(1);

            sr.ReportMetric("test", new MeterValueSource("meter_name", metric, Unit.None, TimeUnit.Milliseconds, MetricTags.None));

            AssertReportResult(sr.Result, expected);
        }

        [Fact]
        public void can_report_timers()
        {
            var expected = StringReporterSamples.Timers.ExtractStringReporterSampleFromResourceFile();
            var sr = new StringReporter();
            var clock = new TestClock();
            var histogram = new HistogramMetric(SamplingType.ExponentiallyDecaying, Constants.ReservoirSampling.DefaultSampleSize,
                Constants.ReservoirSampling.DefaultExponentialDecayFactor);
            var metric = new TimerMetric(histogram, clock);

            metric.Record(1000, TimeUnit.Milliseconds, "value1");
            metric.Record(2000, TimeUnit.Milliseconds, "value2");

            sr.ReportMetric("test", new TimerValueSource("timer_name", metric, Unit.None, TimeUnit.Milliseconds,
                TimeUnit.Milliseconds, MetricTags.None));

            AssertReportResult(sr.Result, expected);
        }

        [Fact]
        public void reporter_name_is_required()
        {
            Action action = () =>
            {
                var sr = new StringReporter(null);
            };

            action.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void when_disposed_buffer_is_cleared()
        {
            var envInfo = new EnvironmentInfo("assembly", "entry", "host", "time", "machine", "OS", "OS version", "process", "4");
            var sr = new StringReporter();
            sr.ReportEnvironment(envInfo);
            sr.Dispose();

            sr.Result.IsNullOrEmpty();
        }

        private static void AssertReportResult(string result, string expected)
        {
            (string.CompareOrdinal(result.Replace("\r\n", "\n"), expected.Replace("\r\n", "\n")) == 0).Should().BeTrue();
        }
    }
}