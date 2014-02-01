using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BuildCop.Configuration;

namespace BuildCop.Test
{
    public static class Utility
    {
        internal static void CheckStringComparison(StringComparison stringComparison, string p)
        {
            Assert.AreEqual<StringComparison>(stringComparison, (StringComparison)Enum.Parse(typeof(StringComparison), p, true));
        }

        internal static void CheckCompareOption(CompareOption compareOption, string p)
        {
            Assert.AreEqual<CompareOption>(compareOption, (CompareOption)Enum.Parse(typeof(CompareOption), p, true));
        }
    }
}
