#region Header

// Alex Gravely - Alex
// 
// Exam Formatter - Exam Formatter
// Question.cs - 30//11//2015 11:13 PM

#endregion

#region Using

using System;
using System.Collections.Generic;
using System.Text;
using Exam_Formatter.Enums;

#endregion

namespace Exam_Formatter.Classes {

	public class Question {
		#region Public Fields + Properties

		public int ID;
		public CorrectAnswer CorrectAnswers { get; protected set; }
		public QuestionType QuestionType { get; set; }
		public Answer A;
		public Answer B;
		public Answer C;
		public Answer D;
		public Answer E;

		public string Text;

		#endregion Public Fields + Properties

		#region Public Constructors

		public Question(int id) { ID = id; }

		public Question(int ID, QuestionType QT) : this(ID) { QuestionType = QT; }

		public Question(int ID, QuestionType QT, string text) : this(ID, QT) { Text = text; }

		public Question(int ID, QuestionType QT, string text, CorrectAnswer CA) : this(ID, QT, text) { CorrectAnswers = CA; }

		#endregion Protected Constructors

		#region Public Methods

		public void SetCorrectAnswer(CorrectAnswer Letter) { CorrectAnswers = Letter; }

		public void SetCorrectAnswer(string Letter) { CorrectAnswers = (CorrectAnswer)Enum.Parse(typeof (CorrectAnswer), Letter); }

		public string GetCorrectAnswerString() {
			switch (CorrectAnswers)
			{
				case CorrectAnswer.A:
				case CorrectAnswer.True:
					return "10000";
				case CorrectAnswer.False:
				case CorrectAnswer.AB:
					return "11000";
				case CorrectAnswer.AC:
					return "10100";
				case CorrectAnswer.AD:
					return "10010";
				case CorrectAnswer.AE:
					return "10001";
				case CorrectAnswer.ABC:
					return "11100";
				case CorrectAnswer.ABD:
					return "11010";
				case CorrectAnswer.ABE:
					return "11001";
				case CorrectAnswer.ACD:
					return "10110";
				case CorrectAnswer.ACE:
					return "10101";
				case CorrectAnswer.ADE:
					return "10011";
				case CorrectAnswer.ABCD:
					return "11110";
				case CorrectAnswer.ABCE:
					return "11101";
				case CorrectAnswer.ABCDE:
					return "11111";
				case CorrectAnswer.B:
					return "01000";
				case CorrectAnswer.BC:
					return "01100";
				case CorrectAnswer.BD:
					return "01010";
				case CorrectAnswer.BE:
					return "01001";
				case CorrectAnswer.BCD:
					return "01110";
				case CorrectAnswer.BCE:
					return "01101";
				case CorrectAnswer.BDE:
					return "01011";
				case CorrectAnswer.BCDE:
					return "01111";
				case CorrectAnswer.C:
					return "00100";
				case CorrectAnswer.CD:
					return "00110";
				case CorrectAnswer.CE:
					return "00101";
				case CorrectAnswer.CDE:
					return "00111";
				case CorrectAnswer.D:
					return "00010";
				case CorrectAnswer.DE:
					return "00011";
				case CorrectAnswer.E:
					return "00001";
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		#region Overrides of Object

		public override string ToString() {
			var SB = new StringBuilder();
			SB.AppendLine(Text);
			switch (QuestionType)
			{
				case QuestionType.MultiSingle:
					SB.AppendLine("multi-single");
					break;
				case QuestionType.MultiSingleNoShuffle:
					SB.AppendLine("multi-single-noshuffle");
					break;
				case QuestionType.MultiSelect:
					SB.AppendLine("multi");
					break;
				case QuestionType.TrueFalse:
					SB.AppendLine("tf");
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
			SB.AppendLine(A.Text);
			SB.AppendLine(B.Text);
			SB.AppendLine(C.Text);
			SB.AppendLine(D.Text);
			SB.AppendLine(E.Text);
			SB.AppendLine(GetCorrectAnswerString());
			return SB.ToString();
		}

		#endregion

		#endregion Public Methods
	}
}