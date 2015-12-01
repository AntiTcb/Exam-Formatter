#region Header

// Alex Gravely - Alex
// 
// Exam Formatter - Exam Formatter
// MainWindow.xaml.cs - 30//11//2015 11:14 PM

#endregion

#region Using

using System;
using System.Windows;
using System.Windows.Controls;
using Exam_Formatter.Classes;
using Exam_Formatter.Enums;
using MahApps.Metro.Controls;

#endregion

namespace Exam_Formatter.Windows {
	/// <summary>
	///     Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow {
		#region Private Fields + Properties

		TabItem CurrentCategory;
		TabItem CurrentQuestion;
		Exam exam;

		#endregion Private Fields + Properties

		#region Public Constructors

		public MainWindow() {
			exam = new Exam();
			InitializeComponent();
			C1.IsSelected = true;
			Q1.IsSelected = true;
		}

		#endregion Public Constructors

		#region Private Methods

		static int GetIDNum(string name) => Convert.ToInt32(name.Remove(0, 1)) - 1;

		void CategoryTabs_OnSelectionChanged(object O, SelectionChangedEventArgs E) {
			var TabControl = (MetroAnimatedSingleRowTabControl) O;
			CurrentCategory = (TabItem) TabControl.SelectedItem;
			if (CurrentCategory != null && CurrentQuestion != null) LoadQuestion();
		}

		void SetQuestionType(QuestionType QT) {
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

		void LoadQuestion() {
			var LoadedQuestion = exam.Categories[GetIDNum(CurrentCategory?.Name)].Questions[GetIDNum(CurrentQuestion?.Name)];
			
			CategoryGrid.QuestionTextTB.Text = LoadedQuestion.Text;
			CategoryGrid.AnswerOneText.Text = LoadedQuestion.Answers["A"].Text;
			CategoryGrid.AnswerTwoText.Text = LoadedQuestion.Answers["B"].Text;
			CategoryGrid.AnswerThreeText.Text = LoadedQuestion.Answers["C"].Text;
			CategoryGrid.AnswerFourText.Text = LoadedQuestion.Answers["D"].Text;
			CategoryGrid.AnswerFiveText.Text = LoadedQuestion.Answers["E"].Text;
			SetQuestionType(LoadedQuestion.QuestionType);
			CategoryNameTB.Text = exam.Categories[GetIDNum(CurrentCategory?.Name)].
		}

		void QuestionTabs_OnSelectionChanged(object O, SelectionChangedEventArgs E) {
			var TabControl = (MetroAnimatedSingleRowTabControl) O;
			CurrentQuestion = (TabItem) TabControl.SelectedItem;
			if (CurrentCategory != null && CurrentQuestion != null) LoadQuestion();
		}

		void ShowCreateExamFlyout(object Sender, RoutedEventArgs E) { CreateExamFlyout.IsOpen = true; }

		#endregion Private Methods

		void OpenFile(object Sender, RoutedEventArgs E) { }
		void EmailExam(object Sender, RoutedEventArgs E) { }
		void SaveFile(object Sender, RoutedEventArgs E) { }
	}
}