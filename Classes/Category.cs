#region Header

// Alex Gravely - Alex
//
// Exam Formatter - Exam Formatter
// Category.cs - 30//11//2015 1:24 AM

#endregion Header

using System;
using System.Linq;
using System.Text;
using Exam_Formatter.Enums;

namespace Exam_Formatter.Classes {

	public class Category {

		#region Public Fields + Properties

		public int ID;
		public Question[] Questions;
		public string Name;

		#endregion Public Fields + Properties

		#region Public Constructors

		public Category(int id) {
			var I = 0;
			ID = id;
			Questions = Enumerable.Repeat(0, 3).Select(x => new Question(++I, QuestionType.MultiSingle)).ToArray(); }

		public Category(int id, Question q) : this(id) { Questions = new[] { q }; }

		public Category(int id, string n) : this(id) { Name = n; }

		public Category(int id, string n, Question q) : this(id, n) { Questions = new[] {q}; }

		public Category(int id, Question[] q) : this(id) { Questions = q; }

		public Category(int id, Question[] q, string n) : this(id, q) { Name = n; }

		#region Overrides of Object

		public override string ToString() {
			var sb = new StringBuilder();

			foreach (var q in Questions)
			{
				sb.AppendLine($"#{ID}.{q.ID}");
				sb.Append(q);
                Console.WriteLine($"{ID}.{q.ID}");
			}
			return $"{sb}\n";
		}

		#endregion

		#endregion Public Constructors
	}
}