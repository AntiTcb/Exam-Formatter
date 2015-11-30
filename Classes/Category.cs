#region Header

// Alex Gravely - Alex
//
// Exam Formatter - Exam Formatter
// Category.cs - 30//11//2015 1:24 AM

#endregion Header

namespace Exam_Formatter.Classes {

	public class Category {

		#region Public Fields + Properties

		public Question[] Questions;

		#endregion Public Fields + Properties

		#region Public Constructors

		public Category()
		{
			Questions = new Question[2];
		}

		public Category(Question q) : this()
		{
			Questions = new[] { q };
		}

		public Category(Question[] q) : this()
		{
			Questions = q;
		}

		#endregion Public Constructors
	}
}