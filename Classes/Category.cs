#region Header

// Description:
//
// Solution: Exam Formatter
// Project: Exam Formatter
//
// Created: 11/29/2015 1:46 AM
// Last Revised: 01/22/2016 5:28 AM
// Last Revised by: Alex Gravely - Alex

#endregion Header

namespace Exam_Formatter.Classes
{
    #region Using

    using System.Diagnostics;
    using Enums;
    using System.Linq;
    using System.Text;

    #endregion Using

    public class Category {

        #region Public Fields + Properties

        public int ID;
        public Question[ ] Questions;
        public string Name;

        #endregion Public Fields + Properties

        #region Public Constructors

        public Category(int id) {
            var I = 0;
            ID = id;
            Questions = Enumerable.Repeat(0, 3).Select(x => new Question(++I, QuestionType.MultiSingle)).ToArray();
        }

        public Category(int id, Question q) : this(id)
        {
            Questions = new[] { q };
        }

        public Category(int id, string n) : this(id)
        {
            Name = n;
        }

        public Category(int id, string n, Question q) : this(id, n)
        {
            Questions = new[] { q };
        }

        public Category(int id, Question[] q) : this(id)
        {
            Questions = q;
        }

        public Category(int id, Question[] q, string n) : this(id, q)
        {
            Name = n;
        }

        #region Overrides of Object

        public override string ToString() {
            var sb = new StringBuilder();

            foreach ( var q in Questions )
            {
                sb.AppendLine($"#{ID}.{q.ID}.{Name}".Trim());
                sb.Append(q);
                Debug.WriteLine($"{ID}.{q.ID}".Trim());
            }
            return $"{sb}\n";
        }

        #endregion Overrides of Object

        #endregion Public Constructors
    }
}