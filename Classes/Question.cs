#region Header

// Alex Gravely - Alex
// 
// Exam Formatter - Exam Formatter
// Question.cs - 30//11//2015 11:13 PM

#endregion

#region Using

using System;
using System.Collections.Generic;
using Exam_Formatter.Enums;

#endregion

namespace Exam_Formatter.Classes {

	public class Question {
		#region Public Fields + Properties

		public CorrectAnswer CorrectAnswers { get; protected set; }
		public QuestionType QuestionType { get; protected set; }

		public Answer A;
		public Answer B;
		public Answer C;
		public Answer D;
		public Answer E;
		public Answer True;
		public Answer False;

		public string Text;

		#endregion Public Fields + Properties

		#region Protected Constructors

		public Question() { }

		public Question(QuestionType QT) : this() { QuestionType = QT; }

		public Question(QuestionType QT, string text) : this(QT) { Text = text; }

		public Question(QuestionType QT, string text, CorrectAnswer CA) : this(QT, text) { CorrectAnswers = CA; }

		#endregion Protected Constructors

		#region Public Methods

		public void SetCorrectAnswer(CorrectAnswer Letter) {
			switch (QuestionType)
			{
				case QuestionType.MultiSingle:

					break;
				case QuestionType.MultiSelect:
					break;
				case QuestionType.TrueFalse:
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		#endregion Public Methods
	}
}