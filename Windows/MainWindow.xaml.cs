#region Header

// Alex Gravely - Alex
//
// Exam Formatter - Exam Formatter
// MainWindow.xaml.cs - 30//11//2015 11:14 PM

#endregion Header

#region Using

using Exam_Formatter.Classes;
using Exam_Formatter.Enums;
using MahApps.Metro.Controls;
using System;
using System.Windows;
using System.Windows.Controls;

#endregion Using

namespace Exam_Formatter.Windows
{
	/// <summary>
	///     Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		#region Private Fields + Properties

		TabItem CurrentCategory;
		TabItem CurrentQuestion;
		Exam exam;
		Question LoadedQuestion;

		#endregion Private Fields + Properties

		#region Public Constructors

		public MainWindow()
		{
			exam = new Exam();
			InitializeComponent();
			CategoryTabs.SelectionChanged -= CategoryTabs_OnSelectionChanged;
			QuestionTabs.SelectionChanged -= QuestionTabs_OnSelectionChanged;
			C1.IsSelected = true;
			Q1.IsSelected = true;
			CategoryTabs.SelectionChanged += CategoryTabs_OnSelectionChanged;
			QuestionTabs.SelectionChanged += QuestionTabs_OnSelectionChanged;

			CategoryGrid.MultiSelectRB.Checked += delegate { LoadedQuestion.QuestionType = QuestionType.MultiSelect; };
			CategoryGrid.MultiSingleRB.Checked += delegate { LoadedQuestion.QuestionType = QuestionType.MultiSingle; };
			CategoryGrid.TrueFalseRB.Checked += delegate { LoadedQuestion.QuestionType = QuestionType.TrueFalse; };
			LoadedQuestion = exam.Categories[0].Questions[0];
			CurrentCategory = C1;
			CurrentQuestion = Q1;
		}

		#endregion Public Constructors

		#region Private Methods

		static int GetIDNum(string name) => Convert.ToInt32(name.Remove(0, 1)) - 1;

		void CategoryTabs_OnSelectionChanged(object O, SelectionChangedEventArgs E)
		{
			var OldCategory = (TabItem)E.RemovedItems[0];
			if (OldCategory != null && CurrentQuestion != null) SaveQuestion(OldCategory, CurrentQuestion);

			var NewTabControl = (MetroAnimatedSingleRowTabControl)O;
			CurrentCategory = (TabItem)NewTabControl.SelectedItem;
			if (CurrentCategory != null && CurrentQuestion != null) LoadQuestion();
		}

		void LoadQuestion()
		{
			LoadedQuestion = exam.Categories[GetIDNum(CurrentCategory?.Name)].Questions[GetIDNum(CurrentQuestion?.Name)];

			CategoryGrid.QuestionTextTB.Text = LoadedQuestion.Text;
			CategoryGrid.AnswerOneText.Text = LoadedQuestion.A.Text;
			CategoryGrid.AnswerTwoText.Text = LoadedQuestion.B.Text;
			CategoryGrid.AnswerThreeText.Text = LoadedQuestion.C.Text;
			CategoryGrid.AnswerFourText.Text = LoadedQuestion.D.Text;
			CategoryGrid.AnswerFiveText.Text = LoadedQuestion.E.Text;
			SetQuestionType(LoadedQuestion.QuestionType);
			CategoryNameTB.Text = exam.Categories[GetIDNum(CurrentCategory?.Name)].Name;
		}

		void QuestionTabs_OnSelectionChanged(object O, SelectionChangedEventArgs E)
		{
			var OldQuestion = (TabItem)E.RemovedItems[0];
			if (OldQuestion != null && CurrentCategory != null) SaveQuestion(CurrentCategory, OldQuestion);

			var TabControl = (MetroAnimatedSingleRowTabControl)O;
			CurrentQuestion = (TabItem)TabControl.SelectedItem;
			if (CurrentCategory != null && CurrentQuestion != null) LoadQuestion();
		}

		void SaveQuestion(IFrameworkInputElement category, IFrameworkInputElement question)
		{
			LoadedQuestion = exam.Categories[GetIDNum(category?.Name)].Questions[GetIDNum(question?.Name)];

			LoadedQuestion.Text = CategoryGrid.QuestionTextTB.Text;
			LoadedQuestion.A.Text = CategoryGrid.AnswerOneText.Text;
			LoadedQuestion.B.Text = CategoryGrid.AnswerTwoText.Text;
			LoadedQuestion.C.Text = CategoryGrid.AnswerThreeText.Text;
			LoadedQuestion.D.Text = CategoryGrid.AnswerFourText.Text;
			LoadedQuestion.E.Text = CategoryGrid.AnswerFiveText.Text;
			exam.Categories[GetIDNum(category?.Name)].Name = CategoryNameTB.Text;
		}

		void SetQuestionType(QuestionType QT)
		{
			switch (QT)
			{
				case QuestionType.MultiSingle:
					CategoryGrid.MultiSingleRB.IsChecked = true;
					break;

				case QuestionType.MultiSelect:
					CategoryGrid.MultiSelectRB.IsChecked = true;
					break;

				case QuestionType.TrueFalse:
					CategoryGrid.TrueFalseRB.IsChecked = true;
					break;

				default:
					throw new ArgumentOutOfRangeException(nameof(QT), QT, null);
			}
		}

		void ShowCreateExamFlyout(object Sender, RoutedEventArgs E)
		{
			CreateExamFlyout.IsOpen = true;
		}

		#endregion Private Methods

		void EmailExam(object Sender, RoutedEventArgs E)
		{
		}

		void OpenFile(object Sender, RoutedEventArgs E)
		{
		}

		void SaveFile(object Sender, RoutedEventArgs E)
		{
		}
	}
}