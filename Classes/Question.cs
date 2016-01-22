#region Header

// Description:
//
// Solution: Exam Formatter
// Project: Exam Formatter
//
// Created: 11/29/2015 1:29 AM
// Last Revised: 01/12/2016 4:25 AM
// Last Revised by: Alex Gravely - Alex

#endregion Header

namespace Exam_Formatter.Classes
{
    #region Using

    using Enums;
    using System;
    using System.Text;

    #endregion Using

    public class Question {

        #region Public Fields + Properties

        public CorrectAnswer CorrectAnswers { get; protected set; }
        public QuestionType QuestionType { get; set; }
        public string Text { get { return text; } set { text = ExamParser.ConvertFromHtml(value); } }

        public Answer A;
        public Answer B;
        public Answer C;
        public Answer D;
        public Answer E;
        public int ID;
        string text;

        #endregion Public Fields + Properties

        #region Public Constructors

        public Question(int id) {
            ID = id;
            CorrectAnswers = CorrectAnswer.A;
        }

        public Question(int id, QuestionType qt) : this(id)
        {
            QuestionType = qt;
        }

        public Question(int id, QuestionType qt, string text) : this(id, qt)
        {
            Text = text;
        }

        public Question(int id, QuestionType qt, string text, CorrectAnswer ca) : this(id, qt, text) {
            CorrectAnswers = ca;
        }

        #endregion Public Constructors

        #region Public Methods

        public string GetCorrectAnswerString() {
            switch ( CorrectAnswers )
            {
                case CorrectAnswer.A:
                    return "10000";

                case CorrectAnswer.AB:
                    return "11000";

                case CorrectAnswer.AC:
                    return "10100";

                case CorrectAnswer.AD:
                    return "10010";

                case CorrectAnswer.AE:
                    return "10001";

                case CorrectAnswer.ABC:
                    return "11100";

                case CorrectAnswer.ABD:
                    return "11010";

                case CorrectAnswer.ABE:
                    return "11001";

                case CorrectAnswer.ACD:
                    return "10110";

                case CorrectAnswer.ACE:
                    return "10101";

                case CorrectAnswer.ADE:
                    return "10011";

                case CorrectAnswer.ABCD:
                    return "11110";

                case CorrectAnswer.ABCE:
                    return "11101";

                case CorrectAnswer.ABCDE:
                    return "11111";

                case CorrectAnswer.B:
                    return "01000";

                case CorrectAnswer.BC:
                    return "01100";

                case CorrectAnswer.BD:
                    return "01010";

                case CorrectAnswer.BE:
                    return "01001";

                case CorrectAnswer.BCD:
                    return "01110";

                case CorrectAnswer.BCE:
                    return "01101";

                case CorrectAnswer.BDE:
                    return "01011";

                case CorrectAnswer.BCDE:
                    return "01111";

                case CorrectAnswer.C:
                    return "00100";

                case CorrectAnswer.CD:
                    return "00110";

                case CorrectAnswer.CE:
                    return "00101";

                case CorrectAnswer.CDE:
                    return "00111";

                case CorrectAnswer.D:
                    return "00010";

                case CorrectAnswer.DE:
                    return "00011";

                case CorrectAnswer.E:
                    return "00001";

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void SetCorrectAnswer(CorrectAnswer letter) => CorrectAnswers = letter;

        public void SetCorrectAnswer(string letter) {
            if ( letter == string.Empty ) { return; }
            CorrectAnswers = (CorrectAnswer) Enum.Parse(typeof ( CorrectAnswer ), letter);
        }

        #region Overrides of Object

        public override string ToString() {
            Text = ExamParser.ConvertToHtml(Text);
            A.Text = ExamParser.ConvertToHtml(A.Text);
            B.Text = ExamParser.ConvertToHtml(B.Text);
            C.Text = ExamParser.ConvertToHtml(C.Text);
            D.Text = ExamParser.ConvertToHtml(D.Text);
            E.Text = ExamParser.ConvertToHtml(E.Text);

            var sb = new StringBuilder();
            sb.AppendLine(Text);

            switch ( QuestionType )
            {
                case QuestionType.MultiSingle:
                    sb.AppendLine("multi-single");
                    break;

                case QuestionType.MultiSingleNoShuffle:
                    sb.AppendLine("multi-single-noshuffle");
                    break;

                case QuestionType.MultiSelect:
                    sb.AppendLine("multi");
                    break;

                case QuestionType.TrueFalse:
                    sb.AppendLine("tf");
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            if ( QuestionType != QuestionType.TrueFalse )
            {
                sb.AppendLine(A.Text);
                sb.AppendLine(B.Text);
                sb.AppendLine(C.Text);
                sb.AppendLine(D.Text);
                sb.AppendLine(E.Text);
            }

            sb.AppendLine(GetCorrectAnswerString());
            return sb.ToString();
        }

        #endregion Overrides of Object

        #endregion Public Methods
    }
}