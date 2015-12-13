#region Header

// Alex Gravely - Alex
// 
// Exam Formatter - Exam Formatter
// ExamParser.cs - 03//12//2015 11:49 PM

#endregion

#region Using

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Exam_Formatter.Enums;
using Exam_Formatter.Windows;
using static System.Text.RegularExpressions.Regex;

#endregion

namespace Exam_Formatter.Classes {
	public static class ExamParser {
		#region Public Methods

		public static async Task ReadExamFile(string filepath, Exam exam) {
			using (var SR = new StreamReader(filepath))
			{
				var Category = 0;
				var ThisQuestion = new StringBuilder();
				var QuestionDone = false;

				// Exam Name
				exam.Name = await SR.ReadLineAsync();
				await SR.ReadLineAsync();

				while (!SR.EndOfStream)
				{
					if (Category == 20) { return;}
					// Categories
					for (var i = 0 ; i <= 2 ; i += 1)
					{
						// Questions
						while (!QuestionDone)
						{
							var Line = ConvertFromHTML(await SR.ReadLineAsync());
							ThisQuestion.AppendLine(Line);
							// Find the last line of the question
							if (IsMatch(Line, "[0-1]{5}")) { QuestionDone = true; }
						}
						await ParseQuestion(exam, Category, i, ThisQuestion.ToString());
						QuestionDone = false;
						ThisQuestion = new StringBuilder();
					}
					Category += 1;
					await SR.ReadLineAsync();
				}
			}
		}

		public static async Task WriteExamFile(string filepath, Exam exam) {
			using (var SW = new StreamWriter(filepath))
			{
				await SW.WriteLineAsync($"{exam.Name}\n");
				foreach (var Cat in exam.Categories)
				{
					await SW.WriteLineAsync(ConvertToHTML(Cat.ToString()));
				}

			}
		}

		#endregion Public Methods

		#region Private Methods

		[SuppressMessage("ReSharper", "SwitchStatementMissingSomeCases")]
		static CorrectAnswer ParseAnswers(string AnswerLine) {
			switch (new Regex(Escape("1")).Matches(AnswerLine).Count)
			{
				case 1:
					switch (AnswerLine.IndexOf("1", StringComparison.Ordinal))
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
					switch (AnswerLine)
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
					switch (AnswerLine)
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
					switch (AnswerLine)
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

		[SuppressMessage("ReSharper", "SwitchStatementMissingSomeCases")]
		static async Task ParseQuestion(Exam exam, int Cat, int Q, string Question) {
			var CurrentQuestion = exam.Categories[Cat].Questions[Q];
			using (var SR = new StringReader(Question))
			{
				CurrentQuestion.Text = await SR.ReadLineAsync();
				CurrentQuestion.QuestionType = ParseQuestionType(await SR.ReadLineAsync());

				switch (CurrentQuestion.QuestionType)
				{
					case QuestionType.TrueFalse:
						CurrentQuestion.SetCorrectAnswer(ParseAnswers(await SR.ReadLineAsync()));
						return;

					default:
						CurrentQuestion.A.Text = await SR.ReadLineAsync();
						CurrentQuestion.B.Text = await SR.ReadLineAsync();
						CurrentQuestion.C.Text = await SR.ReadLineAsync();
						CurrentQuestion.D.Text = await SR.ReadLineAsync();
						CurrentQuestion.E.Text = await SR.ReadLineAsync();
						CurrentQuestion.SetCorrectAnswer(await SR.ReadLineAsync());
						return;
				}
			}
		}

		static QuestionType ParseQuestionType(string QT) {
			switch (QT)
			{
				case "tf":
					return QuestionType.TrueFalse;

				case "multi":
					return QuestionType.MultiSelect;

				case "multi-single":
					return QuestionType.MultiSingle;

				case "multi-single-noshuffle":
					return QuestionType.MultiSingleNoShuffle;

				default:
					throw new ArgumentOutOfRangeException(nameof(QT), QT, null);
			}
		}

	    public static string ConvertToHTML(string text) {
	        string FinalResult;
            try
	        {
	            var Pattern = new[] {"<card>", "</card>"};
	            var Replacement = new[] {"<font color=\"#0000FF\"><u><a href=\".....\">", "</a></u></font>"};
	            var Reg = new Regex(Pattern[0]);
	            var FirstPass = Reg.Replace(text, Replacement[0]);
	            Reg = new Regex(Pattern[1]);
	            FinalResult = Reg.Replace(FirstPass, Replacement[1]);
	        }
	        catch (Exception e)
	        {
	            Console.WriteLine(e);
	            FinalResult = string.Empty;
	        }
	        return FinalResult;
	    }

	    public static string ConvertFromHTML(string text) {
	        string FinalResult;
	        try
	        {
	            var Pattern = new[] {"<.+'>", "</.+>"};
	            var Replacement = new[] {"<card>", "</card>"};
	            var Reg = new Regex(Pattern[0]);
	            var FirstPass = Reg.Replace(text, Replacement[0]);
	            Reg = new Regex(Pattern[1]);
	            FinalResult = Reg.Replace(FirstPass, Replacement[1]);
	        }
	        catch (Exception e)
	        {
	            Console.WriteLine(e);
	            FinalResult = string.Empty;
	        }
	        return FinalResult;
	    }
        

		#endregion Private Methods
	}
}