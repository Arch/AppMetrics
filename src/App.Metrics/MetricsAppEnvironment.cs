﻿// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
#if NET452
using System.Reflection;
#endif
using Microsoft.Extensions.PlatformAbstractions;

namespace App.Metrics
{
    public sealed class MetricsAppEnvironment : IMetricsEnvironment
    {
#if NET452
        public MetricsAppEnvironment(ApplicationEnvironment applicationEnvironment, AssemblyName executingAssemblyName)
        {
            if (applicationEnvironment == null)
            {
                throw new ArgumentNullException(nameof(applicationEnvironment));
            }
            if (executingAssemblyName == null)
            {
                throw new ArgumentNullException(nameof(executingAssemblyName));
            }

            ApplicationName = executingAssemblyName.Name;
            ApplicationVersion = executingAssemblyName.Version.ToString();
            RuntimeFramework = applicationEnvironment.RuntimeFramework.Identifier;
            RuntimeFrameworkVersion = applicationEnvironment.RuntimeFramework.Version.ToString();
        }
#endif

#if !NET452
        public MetricsAppEnvironment(ApplicationEnvironment applicationEnvironment)
        {
            if (applicationEnvironment == null)
            {
                throw new ArgumentNullException(nameof(applicationEnvironment));
            }

            ApplicationName = applicationEnvironment.ApplicationName;
            ApplicationVersion = applicationEnvironment.ApplicationVersion;
            RuntimeFramework = applicationEnvironment.RuntimeFramework.Identifier;
            RuntimeFrameworkVersion = applicationEnvironment.RuntimeFramework.Version.ToString();
        }
#endif

        public string ApplicationName { get; }

        public string ApplicationVersion { get; }

        public string RuntimeFramework { get; }

        public string RuntimeFrameworkVersion { get; }
    }
}