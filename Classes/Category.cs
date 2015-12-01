#region Header

// Alex Gravely - Alex
//
// Exam Formatter - Exam Formatter
// Category.cs - 30//11//2015 1:24 AM

#endregion Header

using System.Collections.Generic;
using System.Linq;
using Exam_Formatter.Enums;

namespace Exam_Formatter.Classes {

	public class Category {

		#region Public Fields + Properties

		public Question[] Questions;

		public string Name;

		#endregion Public Fields + Properties

		#region Public Constructors

		public Category() { Questions = Enumerable.Repeat(new OneAnswer(QuestionType.MultiSingle), 3).ToArray(); }

		public Category(Question q) : this()
		{
			Questions = new[] { q };
		}

		public Category(string n) : this() { Name = n; }

		public Category(Question q, string n) : this(n) { Questions = new[] {q}; }

		public Category(Question[] q) : this()
		{
			Questions = q;
		}

		public Category(Question[] q, string n) : this(q) {
			Name = n;
		}

		#endregion Public Constructors
	}
}