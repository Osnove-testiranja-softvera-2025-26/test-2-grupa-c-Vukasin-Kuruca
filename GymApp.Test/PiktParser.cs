using GymApp.Models;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GymApp.Test
{
    internal class PiktParser
    {
        public static class PictParser
        {
            private static readonly string PictResultsPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "PiktModels.txt");

            public static IEnumerable GetTestCases()
            {
                string[] lines = File.ReadAllLines(PictResultsPath);


                for (int i = 1; i < lines.Length; i++)
                {
                    string line = lines[i].Trim();
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    string[] parts = line.Split('\t');
                    if (parts.Length < 5)
                        continue;

                    int numberOfMonths = int.Parse(parts[0].Trim());
                    bool groupTrainings = bool.Parse(parts[1].Trim());
                    double monthlyPriceBudget = double.Parse(parts[2].Trim(), System.Globalization.CultureInfo.InvariantCulture);
                    TrainingTime trainingTime = (TrainingTime)Enum.Parse(typeof(TrainingTime), parts[3].Trim());

                    string expectedResultStr = parts[4].Trim();
                    MembershipType? expectedResult = expectedResultStr.Equals("null", StringComparison.OrdinalIgnoreCase)
                        ? (MembershipType?)null
                        : (MembershipType)Enum.Parse(typeof(MembershipType), expectedResultStr);

                    yield return new TestCaseData(numberOfMonths, groupTrainings, monthlyPriceBudget, trainingTime, expectedResult)
                        .SetName($"RecommendMembershipType_Months={numberOfMonths}_Group={groupTrainings}_Budget={monthlyPriceBudget}_Time={trainingTime}_Expected={expectedResultStr}");
                }
            }
        }
    }
}
