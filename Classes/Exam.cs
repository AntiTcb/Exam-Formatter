#region Header

// Description:
//
// Solution: Exam Formatter
// Project: Exam Formatter
//
// Created: 11/29/2015 8:18 PM
// Last Revised: 01/22/2016 5:28 AM
// Last Revised by: Alex Gravely - Alex

#endregion Header

namespace Exam_Formatter.Classes
{
    #region Using

    using System.Linq;

    #endregion Using

    public class Exam {

        #region Public Fields + Properties

        public Category[ ] Categories;
        public string Name;

        #endregion Public Fields + Properties

        #region Public Constructors

        public Exam() {
            var I = 0;
            Categories = Enumerable.Repeat(0, 20).Select(x => new Category(++I)).ToArray();
        }

        public Exam(Category[] cats)
        {
            Categories = cats;
        }

        #endregion Public Constructors

        #region Public Methods

        public int GetCategoryCount() => Categories.Length;

        public int GetQuestionCount() => Categories.Sum(category => category.Questions.Length);

        #endregion Public Methods
    }
}