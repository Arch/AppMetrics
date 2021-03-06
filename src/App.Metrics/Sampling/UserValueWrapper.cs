﻿// Copyright (c) Allan hardy. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


// Originally Written by Iulian Margarintescu https://github.com/etishor/Metrics.NET and will retain the same license
// Ported/Refactored to .NET Standard Library by Allan Hardy

using System.Collections.Generic;

namespace App.Metrics.Sampling
{
    public struct UserValueWrapper
    {
        public static readonly IComparer<UserValueWrapper> Comparer = new UserValueComparer();
        public static readonly UserValueWrapper Empty = new UserValueWrapper();
        public readonly string UserValue;

        public readonly long Value;

        public UserValueWrapper(long value, string userValue = null)
        {
            Value = value;
            UserValue = userValue;
        }

        public static bool operator ==(UserValueWrapper left, UserValueWrapper right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(UserValueWrapper left, UserValueWrapper right)
        {
            return !left.Equals(right);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is UserValueWrapper && Equals((UserValueWrapper)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((UserValue?.GetHashCode() ?? 0) * 397) ^ Value.GetHashCode();
            }
        }

        public bool Equals(UserValueWrapper other)
        {
            return string.Equals(UserValue, other.UserValue) && Value == other.Value;
        }

        private class UserValueComparer : IComparer<UserValueWrapper>
        {
            public int Compare(UserValueWrapper x, UserValueWrapper y)
            {
                return Comparer<long>.Default.Compare(x.Value, y.Value);
            }
        }
    }
}