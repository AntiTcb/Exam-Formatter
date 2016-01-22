#region Header

// Description:
//
// Solution: Exam Formatter
// Project: Exam Formatter
//
// Created: 01/12/2016 3:28 AM
// Last Revised: 01/12/2016 4:25 AM
// Last Revised by: Alex Gravely - Alex

#endregion Header

#region Using

using static System.Text.RegularExpressions.Regex;

#endregion Using

namespace Exam_Formatter.Classes
{
    #region Using

    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Windows.Controls;
    using Enums;

    #endregion Using

    [ SuppressMessage("ReSharper", "SwitchStatementMissingSomeCases") ]
    public static class ExamParser {
        const int INDEX_ADJUST = 1;

        const string KDE_POLICY =
            "<u><font color='blue'><a href='http://www.yugioh-card.com/en/gameplay/penalty_guide/KDE%20TCG%20Tournament%20Policy%20v1.4%202013November14.pdf' target='_blank' class='KDEPolicy'>KDE Tournament Policy Guidelines</a></font></u>";

        const string PENALTY_POLICY =
            "<u><font color='blue'><a href='http://www.yugioh-card.com/en/gameplay/penalty_guide/Penalty%20Guidelines%20v1.4%202013November14.pdf' target='_blank' class='Penalties'>Yu-Gi-Oh! Penalty Guidelines</a></font></u>";

        const string YUGIOH_POLICY =
            "<u><font color='blue'><a href='http://www.yugioh-card.com/en/gameplay/penalty_guide/Yu-Gi-Oh!%20Tournament%20Policy%20v1.4%202013November14.pdf' target='_blank' class='YGOPolicy'>Yu-Gi-Oh! Policy Guidelines</a></font></u>";

        public static async Task ReadExamFileAsync(string filePath, Exam exam) {
            string fileDump;
            using ( var reader = new StreamReader(filePath) ) { fileDump = await reader.ReadToEndAsync(); }
            ReadExam(fileDump, exam);
        }

        public static async Task WriteExamFileAsync(string filepath, Exam exam) {
            using ( var sw = new StreamWriter(filepath) )
            {
                await sw.WriteLineAsync($"{exam.Name}\n");
                var progress = new ProgressBar();
                progress.Orientation = Orientation.Horizontal;
                progress.IsEnabled = true;
                foreach ( var cat in exam.Categories )
                {
                    await sw.WriteLineAsync(ConvertToHtml(cat.ToString()));
                    progress.Value = ( cat.ID / 20 ) * 100;
                }
            }
        }

        static void ReadExam(string fileDump, Exam exam) {
            var splitExam = fileDump.Split(Convert.ToChar("#"));

            exam.Name = splitExam[ 0 ];

            foreach ( var question in splitExam )
            {
                if ( question == exam.Name ) { continue; }
                var splitQuestion = question.Split(Convert.ToChar("\n"));
                var splitHeader = splitQuestion[ 0 ].Split(Convert.ToChar("."));
                exam.Categories[ Convert.ToInt32(splitHeader[ 0 ]) - INDEX_ADJUST ].Name = splitQuestion[ 2 ];
                var thisQuestion =
                    exam.Categories[ Convert.ToInt32(splitHeader[ 0 ]) - INDEX_ADJUST ].Questions[
                                                                                                  Convert.ToInt32
                                                                                                      (splitHeader[ 1 ]) -
                                                                                                  INDEX_ADJUST ];
                thisQuestion.Text = ConvertFromHtml(splitQuestion[ 1 ]);
                thisQuestion.QuestionType = ParseQuestionType(splitQuestion[ 2 ]);

                switch ( thisQuestion.QuestionType )
                {
                    case QuestionType.TrueFalse:
                        thisQuestion.A.Text = "True";
                        thisQuestion.B.Text = "False";
                        thisQuestion.SetCorrectAnswer(ParseAnswers(splitQuestion[ 5 ]));
                        break;

                    default:
                        thisQuestion.A.Text = ConvertFromHtml(splitQuestion[ 3 ]);
                        thisQuestion.B.Text = ConvertFromHtml(splitQuestion[ 4 ]);
                        thisQuestion.C.Text = ConvertFromHtml(splitQuestion[ 5 ]);
                        thisQuestion.D.Text = ConvertFromHtml(splitQuestion[ 6 ]);
                        thisQuestion.E.Text = ConvertFromHtml(splitQuestion[ 7 ]);
                        thisQuestion.SetCorrectAnswer(ParseAnswers(splitQuestion[ 8 ]));
                        break;
                }
            }
        }

        public static string ConvertFromHtml(string text)
        {
            var finalResult = text;
            try
            {
                var pattern = new[] { @"<f.+?'_blank'>", "</a></u></font>", $"{YUGIOH_POLICY}", $"{KDE_POLICY}", $"{PENALTY_POLICY}" };
                var replacement = new[] { "<card>", "</card>", "[POLICY-YGO]", "[POLICY-KDE]", "[POLICY-PENALTY]" };
                finalResult = Replace(finalResult, pattern[0], replacement[0]);
                finalResult = Replace(finalResult, pattern[1], replacement[1]);
                finalResult = Replace(finalResult, pattern[2], replacement[2]);
                finalResult = Replace(finalResult, pattern[3], replacement[3]);
                finalResult = Replace(finalResult, pattern[4], replacement[4]);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                finalResult = "An error occurred. PM Anti with the text that should be here.";
            }
            return finalResult;
        }

        public static string ConvertToHtml(string text)
        {
            var finalResult = text;
            try
            {
                var patterns = new[] { @"(?: +)?<card>(?:[\\ ]+)?([a-z0-9""][&\-=\p{Lu}\p{Ll}!.:""',#/・ 0-9]*\s*)(?:[!\\""'/_@#$%^&*()]+)?</card>(?: +)?", @"\[policy-ygo\]", @"\WPOLICY-KDE\W", @"\WPOLICY-PENALTY\W" };
                var replacements = new[] { @" <font color='blue'><u><a href='http://antitcb.com/cardlookup?CardName=$1' target='_blank'>$1</a></u></font> ", $"{YUGIOH_POLICY}", $"{KDE_POLICY}", $"{PENALTY_POLICY}" };

                for (var i = 0; i <= patterns.GetUpperBound(0); i++)
                {
                    var regex = new Regex(patterns[i], RegexOptions.IgnoreCase);
                    finalResult = regex.Replace(finalResult, replacements[i]);
                }
            }
            catch (ArgumentNullException ex)
            {

                Console.WriteLine(ex);
                finalResult = "This line was left empty. Please place text here.";
            }
            catch (Exception e)
            {

                Console.WriteLine(e);
                finalResult = "An error occurred. PM Anti with the text that should be here.";
            }

            var replaceDict = Matches(finalResult, @"'_blank'>(.+?)</a></u></font>").Cast<Match>().ToDictionary(match => match.Groups[1].ToString(), match => match.Groups[1].ToString().Replace(" ", "+"));

            return replaceDict.Aggregate(finalResult, (current, cardNameReplace) => Replace(current, $"CardName={cardNameReplace.Key}", $"CardName={cardNameReplace.Value}"));
        }

        static CorrectAnswer ParseAnswers(string answerLine) {
            switch ( new Regex(Escape("1")).Matches(answerLine).Count )
            {
                case 1:
                    switch ( answerLine.IndexOf("1", StringComparison.Ordinal) )
                    {
                        case 0:
                            return CorrectAnswer.A;

                        case 1:
                            return CorrectAnswer.B;

                        case 2:
                            return CorrectAnswer.C;

                        case 3:
                            return CorrectAnswer.D;

                        case 4:
                            return CorrectAnswer.E;
                    }
                    break;

                case 2:
                    switch ( answerLine )
                    {
                        case "11000":
                            return CorrectAnswer.AB;

                        case "10100":
                            return CorrectAnswer.AC;

                        case "10010":
                            return CorrectAnswer.AD;

                        case "10001":
                            return CorrectAnswer.AE;

                        case "01100":
                            return CorrectAnswer.BC;

                        case "01010":
                            return CorrectAnswer.BD;

                        case "01001":
                            return CorrectAnswer.BE;

                        case "00110":
                            return CorrectAnswer.CD;

                        case "00101":
                            return CorrectAnswer.CE;

                        case "00011":
                            return CorrectAnswer.DE;
                    }
                    break;

                case 3:
                    switch ( answerLine )
                    {
                        case "11100":
                            return CorrectAnswer.ABC;

                        case "11010":
                            return CorrectAnswer.ABD;

                        case "11001":
                            return CorrectAnswer.ABE;

                        case "10110":
                            return CorrectAnswer.ACD;

                        case "10101":
                            return CorrectAnswer.ACE;

                        case "10011":
                            return CorrectAnswer.ADE;

                        case "01110":
                            return CorrectAnswer.BCD;

                        case "01101":
                            return CorrectAnswer.BCE;

                        case "01011":
                            return CorrectAnswer.BDE;

                        case "00111":
                            return CorrectAnswer.CDE;
                    }
                    break;

                case 4:
                    switch ( answerLine )
                    {
                        case "11110":
                            return CorrectAnswer.ABCD;

                        case "11101":
                            return CorrectAnswer.ABCE;

                        case "01111":
                            return CorrectAnswer.BCDE;
                    }
                    break;

                case 5:
                    return CorrectAnswer.ABCDE;

                default:
                    throw new ArgumentException("An answer was not properly encoded!");
            }
            throw new ArgumentException("An answer was not properly encoded!");
        }

        static QuestionType ParseQuestionType(string qt) {
            switch ( qt )
            {
                case "tf\r":
                    return QuestionType.TrueFalse;

                case "multi\r":
                    return QuestionType.MultiSelect;

                case "multi-single\r":
                    return QuestionType.MultiSingle;

                case "multi-single-noshuffle\r":
                    return QuestionType.MultiSingleNoShuffle;

                default:
                    throw new ArgumentOutOfRangeException(nameof(qt), qt, null);
            }
        }
    }
}