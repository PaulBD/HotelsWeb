using System;
using System.Collections.Generic;
using FuzzyString;

namespace core.places.utilities
{
    public static class Common
    {
        public static bool DoesStringMatch(string source, string target)
        {
            List<FuzzyStringComparisonOptions> options = new List<FuzzyStringComparisonOptions>();

            // Choose which algorithms should weigh in for the comparison
            options.Add(FuzzyStringComparisonOptions.UseOverlapCoefficient);
            options.Add(FuzzyStringComparisonOptions.UseLongestCommonSubsequence);
            options.Add(FuzzyStringComparisonOptions.UseLongestCommonSubstring);
            options.Add(FuzzyStringComparisonOptions.UseRatcliffObershelpSimilarity);

            // Choose the relative strength of the comparison - is it almost exactly equal? or is it just close?
            FuzzyStringComparisonTolerance tolerance = FuzzyStringComparisonTolerance.Strong;

            // Get a boolean determination of approximate equality
            return source.ApproximatelyEquals(target, options, tolerance);
        }
    }
}
