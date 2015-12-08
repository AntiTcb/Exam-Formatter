#region Header

// Alex Gravely - Alex
//
// Exam Formatter - Exam Formatter
// Category.cs - 30//11//2015 1:24 AM

#endregion Header

using System;
using System.Collections.Generic;
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

		public Category(int ID, Question q) : this(ID) { Questions = new[] { q }; }

		public Category(int ID, string n) : this(ID) { Name = n; }

		public Category(int ID, string n, Question q) : this(ID, n) { Questions = new[] {q}; }

		public Category(int ID, Question[] q) : this(ID) { Questions = q; }

		public Category(int ID, Question[] q, string n) : this(ID, q) { Name = n; }

		#region Overrides of Object

		public override string ToString() {
			var SB = new StringBuilder();

			foreach (var Q in Questions)
			{
				SB.AppendLine($"#{ID}.{Q.ID}");
				SB.AppendLine(Q.ToString());
			}
			return $"{SB}\n";
		}

		#endregion

		#endregion Public Constructors
	}
}