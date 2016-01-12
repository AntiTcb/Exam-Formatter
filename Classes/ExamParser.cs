#region Header

// Description:
// 
// Solution: Exam Formatter
// Project: Exam Formatter
// 
// Copyright: Copyright (c) 2014-2015 Star City Games (http://www.starcitygames.com)
// 
// Created: 12/08/2015 11:50 PM
// Last Revised: 01/05/2016 11:52 PM
// Last Revised by: Alex Gravely - Alex

#endregion

#region Using

using static System.Text.RegularExpressions.Regex;

#endregion

namespace Exam_Formatter.Classes {
    #region Using

    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Enums;

    #endregion

    [ SuppressMessage("ReSharper", "SwitchStatementMissingSomeCases") ]
    public static class ExamParser {
        #region Public Methods

        const int INDEX_ADJUST = 1;

        const string KDE_POLICY =
            "<u><font color='blue'><a href='http://www.yugioh-card.com/en/gameplay/penalty_guide/KDE%20TCG%20Tournament%20Policy%20v1.4%202013November14.pdf' target='_blank' id='KDEPolicy'>KDE Tournament Policy Guidelines</a></font></u>";

        const string PENALTY_POLICY =
            "<u><font color='blue'><a href='http://www.yugioh-card.com/en/gameplay/penalty_guide/Penalty%20Guidelines%20v1.4%202013November14.pdf' target='_blank' id='Penalties'>Yu-Gi-Oh! Penalty Guidelines</a></font></u>";

        const string YUGIOH_POLICY =
            "<u><font color='blue'><a href='http://www.yugioh-card.com/en/gameplay/penalty_guide/Yu-Gi-Oh!%20Tournament%20Policy%20v1.4%202013November14.pdf' target='_blank' id='YGOPolicy'>Yu-Gi-Oh! Policy Guidelines</a></font></u>";

        public static async Task ReadExamFileAsync(string filePath, Exam exam) {
            string fileDump;
            using ( var reader = new StreamReader(filePath) ) { fileDump = await reader.ReadToEndAsync(); }
            ReadExam(fileDump, exam);
        }

        public static async Task WriteExamFileAsync(string filepath, Exam exam) {
            using ( var sw = new StreamWriter(filepath) )
            {
                await sw.WriteLineAsync($"{exam.Name}\n");
                foreach ( var cat in exam.Categories ) { await sw.WriteLineAsync(ConvertToHtml(cat.ToString())); }
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
                Console.WriteLine
                    ($"Text Category: {splitHeader[ 0 ]}, {splitHeader[ 1 ]}, Accessed Question: {thisQuestion.ID}");
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

        #endregion Public Methods

        #region Private Methods

        public static string ConvertFromHtml(string text) {
            string finalResult;
            try
            {
                var pattern = new[ ] { @"<f.+?'_blank'>", "</a></u></font>", "</a></font></u>", @"<u.+?\YGOPolicy.+?>", @"<u.+?\KDEPolicy.+?>", @"<u.+?\Penalties.+?>" };
                var replacement = new[ ] { "<card>", "</card>", "[POLICY-YGO]", "[POLICY-KDE]", "[POLICY-PENALTY]" };
                var reg = new Regex(pattern[ 0 ]);
                var firstPass = reg.Replace(text, replacement[ 0 ]);
                reg = new Regex(pattern[ 1 ]);
                finalResult = reg.Replace(firstPass, replacement[ 1 ]);
            }
            catch ( Exception e )
            {
                Console.WriteLine(e);
                finalResult = string.Empty;
            }
            return finalResult;
        }

        public static string ConvertToHtml(string text) {
            string finalResult;
            try
            {
                var patterns = new[ ]
                               {
                                   @"<card>(.+?)<", "<card>", "</card>", "[POLICY-YGO]", "[POLICY-KDE]", "[POLICY-PENALTY]"
                               };
                var reg = new Regex(patterns[ 0 ]);
                var cardName = reg.Match(text).Groups[ 1 ].Value.Replace(" ", "+");
                var replacement = new[ ]
                                  {
                                      $@"<font color='blue'><u><a href='http://antitcb.com/cardlookup?CardName={cardName
                                          }' target='_blank'>",
                                      "</a></u></font>",
                                      $"{YUGIOH_POLICY}",
                                      $"{KDE_POLICY}",
                                      $"{PENALTY_POLICY}"
                                  };
                reg = new Regex(patterns[ 1 ]);
                var firstPass = reg.Replace(text, replacement[ 0 ]);
                reg = new Regex(patterns[ 2 ]);
                finalResult = reg.Replace(firstPass, replacement[ 1 ]);
            }
            catch ( Exception e )
            {
                Console.WriteLine(e);
                finalResult = string.Empty;
            }
            return finalResult;
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

        static async Task ParseQuestionAsync(Exam exam, int cat, int q, string question) {
            var currentQuestion = exam.Categories[ cat ].Questions[ q ];
            using ( var sr = new StringReader(question) )
            {
                currentQuestion.Text = ConvertFromHtml(await sr.ReadLineAsync());
                currentQuestion.QuestionType = ParseQuestionType(await sr.ReadLineAsync());

                switch ( currentQuestion.QuestionType )
                {
                    case QuestionType.TrueFalse:
                        currentQuestion.SetCorrectAnswer(ParseAnswers(await sr.ReadLineAsync()));
                        return;

                    default:
                        currentQuestion.A.Text = ConvertFromHtml(await sr.ReadLineAsync());
                        currentQuestion.B.Text = ConvertFromHtml(await sr.ReadLineAsync());
                        currentQuestion.C.Text = ConvertFromHtml(await sr.ReadLineAsync());
                        currentQuestion.D.Text = ConvertFromHtml(await sr.ReadLineAsync());
                        currentQuestion.E.Text = ConvertFromHtml(await sr.ReadLineAsync());
                        currentQuestion.SetCorrectAnswer(await sr.ReadLineAsync());
                        return;
                }
            }
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

    #endregion Private Methods
}