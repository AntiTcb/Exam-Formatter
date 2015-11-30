using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam_Formatter.Classes
{
	public class Category {
		public Question[] Questions;

		public Category() { Questions = new Question[2]; }

		public Category(Question q) : this() { Questions = new[] {q}; }

		public Category(Question[] q) : this () { Questions = q; }
	}
}
