// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using App.Metrics.Internal;
using App.Metrics.Serialization.Interfaces;

namespace App.Metrics.Serialization
{
    [AppMetricsExcludeFromCodeCoverage]
    public sealed class NoOpMetricDataSerializer : IMetricDataSerializer
    {
        public T Deserialize<T>(string json)
        {
            return default(T);
        }

        public string Serialize<T>(T value)
        {
            return string.Empty;
        }
    }
}