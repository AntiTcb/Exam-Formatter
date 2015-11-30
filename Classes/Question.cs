using System;
using System.Collections.Generic;
using Exam_Formatter.Enums;

namespace Exam_Formatter.Classes
{
	public abstract class Question {
		public string Text;
		public QuestionType QuestionType { get; protected set; }
		public string[] CorrectAnswers { get; protected set; }
		public Dictionary<string, Answer> Answers;

		protected Question() { Answers = new Dictionary<string, Answer>(); }
		protected Question(QuestionType QT) : this() { QuestionType = QT; }
		protected Question(QuestionType QT, string text) : this(QT) { Text = text; }
		protected Question(QuestionType QT, string text, Dictionary<string, Answer> ANS) : this(QT, text) { Answers = ANS; }
		protected Question(QuestionType QT, string text, Dictionary<string, Answer> ANS, string[] CA) : this(QT, text, ANS) { CorrectAnswers = CA; }

		public virtual bool TryAddAnswer(string Letter, Answer answer) {
			if (Answers.ContainsKey(Letter)) { return false; }
			Answers.Add(Letter, answer);
			return true;
		}
		public abstract void SetCorrectAnswer(string[] Letter);
	}

	
	public class OneAnswer : Question {
		public OneAnswer(QuestionType QT) {
			if (QT != QuestionType.MultiSingle || QT != QuestionType.TrueFalse) { throw new ArgumentException("One answer questions must be MultiSingle or TrueFalse."); }
			QuestionType = QT;
		}
		public OneAnswer(QuestionType QT, string text) : this(QT) { Text = text; } 
		public OneAnswer(QuestionType QT, string text, Dictionary<string, Answer> ANS) : base(QT, text) { Answers = ANS; }
		public OneAnswer(QuestionType QT, string text, Dictionary<string, Answer> ANS, string CA) : base(QT, text, ANS) { CorrectAnswers = new[] {CA}; }

		public override void SetCorrectAnswer(string[] Letter) { CorrectAnswers = Letter; }
	}

	public class TwoAnswers : Question {
		public TwoAnswers() { QuestionType = QuestionType.MultiSelect; }
		public TwoAnswers(string text) : this() { Text = text; }
		public TwoAnswers(string text, Dictionary<string, Answer> ANS) : this(text) { Answers = ANS; }
		public TwoAnswers(string text, Dictionary<string, Answer> ANS, string[] CA) : this(text, ANS) { CorrectAnswers = CA; }
		
		public override void SetCorrectAnswer(string[] Letter) { CorrectAnswers = Letter; }
	}

	public class ThreeAnswers : TwoAnswers {
	}

	public class FourAnswers : TwoAnswers {
	}

	public class FiveAnswers : TwoAnswers {
		public FiveAnswers() { CorrectAnswers = new[] {"A", "B", "C", "D", "E"}; }
		public FiveAnswers(string text) : this() { Text = text; }
		public FiveAnswers(string text, Dictionary<string, Answer> ANS) : this(text) { Answers = ANS; }

		public override void SetCorrectAnswer(string[] Letter) { }
	}

}
