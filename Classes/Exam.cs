#region Header

// Alex Gravely - Alex
//
// Exam Formatter - Exam Formatter
// Exam.cs - 30//11//2015 1:24 AM

#endregion Header

#region Using

using System.Collections.Generic;
using System.Linq;

#endregion Using

namespace Exam_Formatter.Classes {

	public class Exam {

		public string Name;

		#region Public Fields + Properties

		public Category[] Categories;

		#endregion Public Fields + Properties

		#region Public Constructors

		public Exam() {
			var I = 0;
			Categories = Enumerable.Repeat(0, 20).Select(x => new Category(++I)).ToArray(); }

		public Exam(Category[] cats) { Categories = cats; }

		#endregion Public Constructors

		#region Public Methods

		public int GetCategoryCount() => Categories.Length;

		public int GetQuestionCount() => Categories.Sum(Category => Category.Questions.Length);

		#endregion Public Methods
	}
}