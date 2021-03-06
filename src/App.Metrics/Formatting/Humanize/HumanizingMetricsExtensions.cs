﻿// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Generic;
using App.Metrics.Core;
using App.Metrics.Data;

namespace App.Metrics.Formatting.Humanize
{
    public static class HumanizingMetricsExtensions
    {
        private static readonly IReadOnlyDictionary<Type, string> MetricTypeDisplayNameMapping = new Dictionary<Type, string>
        {
            { typeof(CounterValue), "Counters" },
            { typeof(double), "Gauges" },
            { typeof(MeterValue), "Meters" },
            { typeof(TimerValue), "Timers" },
            { typeof(ApdexValue), "ApdexScores" },
            { typeof(HistogramValue), "Histograms" },
            { typeof(HealthStatus), "Health Checks" },
            { typeof(EnvironmentInfo), "Environment Information" }
        };

        public static string HumanzeEndMetricType(this Type metricType)
        {
            var metricTypeDisplay = MetricTypeDisplayNameMapping[metricType];

            return string.Format("***** End - {0} *****" + Environment.NewLine, metricTypeDisplay);
        }

        public static string HumanzeStartMetricType(this Type metricType, string context = null)
        {
            var metricTypeDisplay = MetricTypeDisplayNameMapping[metricType];

            if (context.IsPresent())
            {
                return string.Format(Environment.NewLine + "***** [{0}] {1} *****" + Environment.NewLine + Environment.NewLine,
                    context, metricTypeDisplay);
            }

            return string.Format(Environment.NewLine + "***** {0} *****" + Environment.NewLine + Environment.NewLine, metricTypeDisplay);
        }

        public static string HumanzizeName<T>(this MetricValueSource<T> valueSource, string context = null)
        {
            if (context.IsPresent())
            {
                return $"\t[{context}] {valueSource.Name}";
            }

            return $"\t{valueSource.Name}";
        }

        public static string Hummanize<T>(this MetricValueSource<T> valueSource)
        {
#if NET452
            var formatProvider = new HumanizeMetricValueFormatProvider<T>();
            var formatter = formatProvider.GetFormat(valueSource.GetType()) as ICustomFormatter;
            return formatter != null 
                ? formatter.Format(string.Empty, valueSource, null) 
                : string.Empty;
#else
            FormattableString metricValueSourceString = $"{valueSource}";
            return metricValueSourceString.ToString(new HumanizeMetricValueFormatProvider<T>());
#endif
        }

        public static string Hummanize(this EnvironmentInfo environmentInfo)
        {
#if NET452
            var formatProvider = new HumanizeEnvironmentInfoFormatter();
            return formatProvider.Format(string.Empty, environmentInfo, null);
#else
            FormattableString environmentInfoString = $"{environmentInfo}";
            return environmentInfoString.ToString(new HumanizeMetricValueFormatProvider<EnvironmentInfo>());
#endif
        }

        public static string Hummanize(this HealthCheck.Result healthCheckResult)
        {
#if NET452
            var formatProvider = new HumanizeHealthCheckResultFormatter();
            return formatProvider.Format(string.Empty, healthCheckResult, null);
#else
            FormattableString healthCheckResultString = $"{healthCheckResult}";
            return healthCheckResultString.ToString(new HumanizeMetricValueFormatProvider<HealthCheck.Result>());
#endif
        }
    }
}