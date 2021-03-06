﻿using System;
using System.Linq;
using App.Metrics.Sampling;
using App.Metrics.Sampling.Interfaces;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Facts.Sampling
{
    public class UniformSnapshotTest
    {
        private readonly UniformSnapshot _snapshot = new UniformSnapshot(5, new[] { 5L, 1, 2, 3, 4 });

        [Fact]
        public void UniformSnapshot_BigQuantilesAreTheLastValue()
        {
            _snapshot.GetValue(1.0).Should().BeApproximately(5, 0.1);
        }

        [Fact]
        public void UniformSnapshot_CalculatesAMaxOfZeroForAnEmptySnapshot()
        {
            ISnapshot snapshot = new UniformSnapshot(0, Enumerable.Empty<long>());
            snapshot.Max.Should().Be(0);
        }

        [Fact]
        public void UniformSnapshot_CalculatesAMeanOfZeroForAnEmptySnapshot()
        {
            ISnapshot snapshot = new UniformSnapshot(0, Enumerable.Empty<long>());
            snapshot.Mean.Should().Be(0);
        }

        [Fact]
        public void UniformSnapshot_CalculatesAMinOfZeroForAnEmptySnapshot()
        {
            ISnapshot snapshot = new UniformSnapshot(0, Enumerable.Empty<long>());
            snapshot.Min.Should().Be(0);
        }

        [Fact]
        public void UniformSnapshot_CalculatesAStdDevOfZeroForAnEmptySnapshot()
        {
            ISnapshot snapshot = new UniformSnapshot(0, Enumerable.Empty<long>());
            snapshot.StdDev.Should().Be(0);
        }

        [Fact]
        public void UniformSnapshot_CalculatesAStdDevOfZeroForASingletonSnapshot()
        {
            ISnapshot snapshot = new UniformSnapshot(0, new[] { 1L });
            snapshot.StdDev.Should().Be(0);
        }

        [Fact]
        public void UniformSnapshot_CalculatesTheMaximumValue()
        {
            _snapshot.Max.Should().Be(5);
        }

        [Fact]
        public void UniformSnapshot_CalculatesTheMeanValue()
        {
            _snapshot.Mean.Should().Be(3.0);
        }

        [Fact]
        public void UniformSnapshot_CalculatesTheMinimumValue()
        {
            _snapshot.Min.Should().Be(1);
        }

        [Fact]
        public void UniformSnapshot_CalculatesTheStdDev()
        {
            _snapshot.StdDev.Should().BeApproximately(1.5811, 0.0001);
        }

        [Fact]
        public void UniformSnapshot_HasAMedian()
        {
            _snapshot.Median.Should().BeApproximately(3, 0.1);
        }

        [Fact]
        public void UniformSnapshot_HasAp75()
        {
            _snapshot.Percentile75.Should().BeApproximately(4.5, 0.1);
        }

        [Fact]
        public void UniformSnapshot_HasAp95()
        {
            _snapshot.Percentile95.Should().BeApproximately(5.0, 0.1);
        }

        [Fact]
        public void UniformSnapshot_HasAp98()
        {
            _snapshot.Percentile98.Should().BeApproximately(5.0, 0.1);
        }

        [Fact]
        public void UniformSnapshot_HasAp99()
        {
            _snapshot.Percentile99.Should().BeApproximately(5.0, 0.1);
        }

        [Fact]
        public void UniformSnapshot_HasAp999()
        {
            _snapshot.Percentile999.Should().BeApproximately(5.0, 0.1);
        }

        [Fact]
        public void UniformSnapshot_HasASize()
        {
            _snapshot.Size.Should().Be(5);
        }

        [Fact]
        public void UniformSnapshot_HasValues()
        {
            _snapshot.Values.Should().ContainInOrder(1L, 2L, 3L, 4L, 5L);
        }

        [Fact]
        public void UniformSnapshot_SmallQuantilesAreTheFirstValue()
        {
            _snapshot.GetValue(0.0).Should().BeApproximately(1, 0.1);
        }

        [Fact]
        public void UniformSnapshot_ThrowsOnBadQuantileValue()
        {
            ((Action)(() => _snapshot.GetValue(-0.5))).ShouldThrow<ArgumentException>();
            ((Action)(() => _snapshot.GetValue(1.5))).ShouldThrow<ArgumentException>();
            ((Action)(() => _snapshot.GetValue(double.NaN))).ShouldThrow<ArgumentException>();
        }
    }
}