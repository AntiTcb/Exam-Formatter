#region Header

// Alex Gravely - Alex
//
// Exam Formatter - Exam Formatter
// Question.cs - 30//11//2015 1:24 AM

#endregion Header

#region Using

using Exam_Formatter.Enums;
using System;
using System.Collections.Generic;

#endregion Using

namespace Exam_Formatter.Classes {

	public class FiveAnswers : TwoAnswers {

		#region Public Constructors

		public FiveAnswers()
		{
			CorrectAnswers = new[] { "A", "B", "C", "D", "E" };
		}

		public FiveAnswers(string text) : this()
		{
			Text = text;
		}

		public FiveAnswers(string text, Dictionary<string, Answer> ANS) : this(text)
		{
			Answers = ANS;
		}

		#endregion Public Constructors

		#region Public Methods

		public override void SetCorrectAnswer(string[] Letter)
		{
		}

		#endregion Public Methods
	}

	public class FourAnswers : TwoAnswers {}

	public class OneAnswer : Question {

		#region Public Constructors

		public OneAnswer(QuestionType QT) {
			if (QT != QuestionType.MultiSingle || QT != QuestionType.TrueFalse)
			{
				throw new ArgumentException("One answer questions must be MultiSingle or TrueFalse.");
			}
			QuestionType = QT;
		}

		public OneAnswer(QuestionType QT, string text) : this(QT)
		{
			Text = text;
		}

		public OneAnswer(QuestionType QT, string text, Dictionary<string, Answer> ANS) : base(QT, text)
		{
			Answers = ANS;
		}

		public OneAnswer(QuestionType QT, string text, Dictionary<string, Answer> ANS, string CA) : base(QT, text, ANS) {
			CorrectAnswers = new[] {CA};
		}

		#endregion Public Constructors

		#region Public Methods

		public override void SetCorrectAnswer(string[] Letter)
		{
			CorrectAnswers = Letter;
		}

		#endregion Public Methods
	}

	public abstract class Question {

		#region Public Fields + Properties

		public string[] CorrectAnswers { get; protected set; }
		public QuestionType QuestionType { get; protected set; }
		public Dictionary<string, Answer> Answers;
		public string Text;

		#endregion Public Fields + Properties

		#region Protected Constructors

		protected Question()
		{
			Answers = new Dictionary<string, Answer>();
		}

		protected Question(QuestionType QT) : this()
		{
			QuestionType = QT;
		}

		protected Question(QuestionType QT, string text) : this(QT)
		{
			Text = text;
		}

		protected Question(QuestionType QT, string text, Dictionary<string, Answer> ANS) : this(QT, text)
		{
			Answers = ANS;
		}

		protected Question(QuestionType QT, string text, Dictionary<string, Answer> ANS, string[] CA) : this(QT, text, ANS) {
			CorrectAnswers = CA;
		}

		#endregion Protected Constructors

		#region Public Methods

		public abstract void SetCorrectAnswer(string[] Letter);

		public virtual bool TryAddAnswer(string Letter, Answer answer) {
			if (Answers.ContainsKey(Letter)) { return false; }
			Answers.Add(Letter, answer);
			return true;
		}

		#endregion Public Methods
	}

	public class ThreeAnswers : TwoAnswers {}

	public class TwoAnswers : Question {

		#region Public Constructors

		public TwoAnswers()
		{
			QuestionType = QuestionType.MultiSelect;
		}

		public TwoAnswers(string text) : this()
		{
			Text = text;
		}

		public TwoAnswers(string text, Dictionary<string, Answer> ANS) : this(text)
		{
			Answers = ANS;
		}

		public TwoAnswers(string text, Dictionary<string, Answer> ANS, string[] CA) : this(text, ANS)
		{
			CorrectAnswers = CA;
		}

		#endregion Public Constructors

		#region Public Methods

		public override void SetCorrectAnswer(string[] Letter)
		{
			CorrectAnswers = Letter;
		}

		#endregion Public Methods
	}
}