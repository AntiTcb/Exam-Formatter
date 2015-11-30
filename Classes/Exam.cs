#region Header
// Alex Gravely - Alex
// 
// Exam Formatter - Exam Formatter
// Exam.cs - 29//11//2015 8:18 PM
#endregion

using System.Linq;

namespace Exam_Formatter.Classes {
	public class Exam {
		public Category[] Categories;

		public Exam() { Categories = new Category[19]; }
		public Exam(Category[] cats) { Categories = cats; }

		public int GetCategoryCount() => Categories.Length;
		public int GetQuestionCount() => Categories.Sum(Category => Category.Questions.Length);
	}
}