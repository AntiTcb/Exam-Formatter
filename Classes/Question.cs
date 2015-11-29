using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam_Formatter.Classes
{
	public abstract class Question
	{
		public string[] CorrectAnswers { get; protected set; }
		public Dictionary<string, Answer> Answers;

		public virtual bool TryAddAnswer(string Letter, Answer answer) {
			if (Answers.ContainsKey(Letter)) { return false; }
			Answers.Add(Letter, answer);
			return true;
		}
		public abstract void SetCorrectAnswer(string[] Letter);
	}

	public class OneAnswer : Question
	{
		public override void SetCorrectAnswer(string[] Letter) { CorrectAnswers = Letter; }
	}
}
