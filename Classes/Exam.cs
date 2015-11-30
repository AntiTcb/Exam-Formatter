#region Header

// Alex Gravely - Alex
//
// Exam Formatter - Exam Formatter
// Exam.cs - 30//11//2015 1:24 AM

#endregion Header

#region Using

using System.Linq;

#endregion Using

namespace Exam_Formatter.Classes {

	public class Exam {

		#region Public Fields + Properties

		public Category[] Categories;

		#endregion Public Fields + Properties

		#region Public Constructors

		public Exam()
		{
			Categories = new Category[19];
		}

		public Exam(Category[] cats)
		{
			Categories = cats;
		}

		#endregion Public Constructors

		#region Public Methods

		public int GetCategoryCount() => Categories.Length;

		public int GetQuestionCount() => Categories.Sum(Category => Category.Questions.Length);

		#endregion Public Methods
	}
}