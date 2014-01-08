using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Should;

namespace BerlinClockLib
{
    public static class MengelHeur
    {
        public static string toMengelHeur(this string input)
        {
            var parsed = parseInput(input);
            var output = new StringBuilder();
            int seconds = parsed[2];
            int hours = parsed[0];
            int minutes = parsed[1];

            output.AppendLine(seconds % 2 == 0 ? "Y" : "O");

            output.AppendLine(("".PadRight(hours / 5, 'R')).PadRight(4, 'O'));
            output.AppendLine(("".PadRight(hours % 5, 'R')).PadRight(4, 'O'));

            var tmp = ("".PadRight(minutes / 5, 'Y').PadRight(11, 'O'));
            //var res = "";
            //for (int i = 0; i < tmp.Length; i++)
            //    if ((i + 1) % 3 == 0 && tmp[i] == 'Y')
            //        res += "R";
            //    else
            //    {
            //        res += tmp[i];
            //    }

            while (tmp.Contains("YYY"))
            {
                tmp = tmp.Replace("YYY", "YYR");
            }

            output.AppendLine(tmp);
            output.AppendLine(("".PadRight(minutes % 5, 'Y').PadRight(4, 'O')));

            return output.ToString();
        }

        private static int[] parseInput(string input)
        {
            var parsed = input.Split(':');
            List<int> values = new List<int>();
            foreach (string s in parsed)
                values.Add(int.Parse(s));
            return values.ToArray();
        }
    }

    [TestFixture]
    public class Class1
    {
        [TestCase("00:00:00", "Y")]
        [TestCase("00:00:12", "Y")]
        [TestCase("00:00:01", "O")]
        [TestCase("00:00:59", "O")]
        public void TestSecondsState(string input, string result)
        {
            input.toMengelHeur().ShouldStartWith(result);
        }


        [TestCase("00:00:00", "Y", "OOOO", "OOOO")]
        [TestCase("13:00:00", "Y", "RROO", "RRRO")]
        [TestCase("23:00:00", "Y", "RRRR", "RRRO")]
        [TestCase("24:00:00", "Y", "RRRR", "RRRR")]
        public void TestHoursState(string input, string seconds, string hours1, string hours2)
        {
            input.toMengelHeur().ShouldStartWith(seconds + Environment.NewLine + hours1 + Environment.NewLine + hours2 + Environment.NewLine);
        }

        [TestCase("13:17:01", "O", "RROO", "RRRO", "YYROOOOOOOO", "YYOO")]
        [TestCase("00:00:00", "Y", "OOOO", "OOOO", "OOOOOOOOOOO", "OOOO")]
        [TestCase("23:59:59", "O", "RRRR", "RRRO", "YYRYYRYYRYY", "YYYY")]
        [TestCase("24:00:00", "Y", "RRRR", "RRRR", "OOOOOOOOOOO", "OOOO")]
        public void TestMinutes(string input, string seconds, string hours1, string hours2, string minutes1, string minutes2)
        {
            input.toMengelHeur().ShouldEqual(string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}", Environment.NewLine, seconds, hours1, hours2, minutes1, minutes2));
        }
    }
}
