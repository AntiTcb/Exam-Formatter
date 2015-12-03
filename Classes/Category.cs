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

		public int ID;
		public Question[] Questions;
		public string Name;

		#endregion Public Fields + Properties

		#region Public Constructors

		public Category(int ID) {
			var I = 0;
			Questions = Enumerable.Repeat(0, 3).Select(x => new Question(++I, QuestionType.MultiSingle)).ToArray(); }

		public Category(int ID, Question q) : this(ID) { Questions = new[] { q }; }

		public Category(int ID, string n) : this(ID) { Name = n; }

		public Category(int ID, string n, Question q) : this(ID, n) { Questions = new[] {q}; }

		public Category(int ID, Question[] q) : this(ID) { Questions = q; }

		public Category(int ID, Question[] q, string n) : this(ID, q) { Name = n; }

		#endregion Public Constructors
	}
}