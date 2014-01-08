using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        
        
        private static string toMengelHeurRec(string input)
        {
            return rec(parseInput(input),"");
        }

        private static List<int> Remove(List<int> input)
        {
            input.RemoveAt(0);
            return input;
        }

        private static string Format(int value, char firstChar, char alternateChare, char secondChar, int totalLength, int secTotalLength)
        {
            const string l = "\r\n";
            return
                ("".PadRight(value/5, firstChar).PadRight(totalLength, secondChar)).Replace("".PadRight(3, firstChar),
                        firstChar.ToString() + firstChar.ToString() + alternateChare.ToString())
                    .Replace("".PadRight(3, firstChar),
                        firstChar.ToString() + firstChar.ToString() + alternateChare.ToString())
                    .Replace("".PadRight(3, firstChar),
                        firstChar.ToString() + firstChar.ToString() + alternateChare.ToString())
                    + l +
                ("".PadRight(value%5, firstChar).PadRight(secTotalLength, secondChar));
        }

        //private static string FormatMinutes(int value,char firstChar, char alternateChar)

        private static string rec(List<int> input,string res)
        {
            const string l = "\r\n";
            var value = input.First();
            switch (input.Count()-1)
            {
                case 2:
                {
                    return rec(Remove(input),
                        res + Format(value,'R','R','O',4,4));
                }
                case 1:
                {  
                    return rec(Remove(input),
                        res + l + Format(value,'Y','R','O',11,4)
                        );

                }
                case 0:
                {
                    return (input.First() % 2 == 0 ? "Y" : "O") + l + res + l;
                }

            }
           return "";
        }

        public static string toMengelHeur(this string input)
        {
            return toMengelHeurRec(input);
        }

        private static string toMengelHeurRegular(string input)
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

            while (tmp.Contains("YYY"))
            {
                tmp = tmp.Replace("YYY", "YYR");
            }

            output.AppendLine(tmp);
            output.AppendLine(("".PadRight(minutes % 5, 'Y').PadRight(4, 'O')));

            return output.ToString();
        }

        private static List<int> parseInput(string input)
        {
            var parsed = input.Split(':');
            List<int> values = new List<int>();
            foreach (string s in parsed)
                values.Add(int.Parse(s));
            return values;
        }
    }

    [TestFixture]
    public class TestClass
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
