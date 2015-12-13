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
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

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

	    bool _enableControls;

	    public bool EnableControls
	    {
	        get
	        {
	            return _enableControls;
	        }
	        set
	        {
	           CategoryGrid.SetControlsEnabled(value);
               _enableControls = value;
            }
	    }

	    #endregion Private Fields + Properties

		#region Public Constructors

		public MainWindow() {
			exam = new Exam();
			InitializeComponent();
            EnableControls = false;
            CategoryTabs.SelectionChanged -= CategoryTabs_OnSelectionChanged;
			QuestionTabs.SelectionChanged -= QuestionTabs_OnSelectionChanged;
			C1.IsSelected = true;
			Q1.IsSelected = true;
			SetEventHandlers();
			LoadedQuestion = exam.Categories[0].Questions[0];
			CurrentCategory = C1;
			CurrentQuestion = Q1;
		}

		void SetEventHandlers() {
			CategoryTabs.SelectionChanged += CategoryTabs_OnSelectionChanged;
			QuestionTabs.SelectionChanged += QuestionTabs_OnSelectionChanged;

			CategoryGrid.MultiSelectRB.Checked += delegate { LoadedQuestion.QuestionType = QuestionType.MultiSelect; };
			CategoryGrid.MultiSingleRB.Checked += delegate { LoadedQuestion.QuestionType = QuestionType.MultiSingle; };
			CategoryGrid.TrueFalseRB.Checked += delegate { LoadedQuestion.QuestionType = QuestionType.TrueFalse; };
			CategoryGrid.MultiSingleNoShuffleRB.Checked +=
				delegate { LoadedQuestion.QuestionType = QuestionType.MultiSingleNoShuffle; };
			CategoryGrid.Answer1CB.Checked += delegate {
												if (CategoryGrid.Answer1CB.IsChecked == null || LoadedQuestion.QuestionType == QuestionType.MultiSelect ||
													!(bool) CategoryGrid.Answer1CB.IsChecked) return;
												CategoryGrid.Answer2CB.IsChecked = false;
												CategoryGrid.Answer3CB.IsChecked = false;
												CategoryGrid.Answer4CB.IsChecked = false;
												CategoryGrid.Answer5CB.IsChecked = false;
											};
			CategoryGrid.Answer2CB.Checked += delegate {
												if (CategoryGrid.Answer2CB.IsChecked == null || LoadedQuestion.QuestionType == QuestionType.MultiSelect ||
													!(bool) CategoryGrid.Answer2CB.IsChecked)
													return;
												CategoryGrid.Answer1CB.IsChecked = false;
												CategoryGrid.Answer3CB.IsChecked = false;
												CategoryGrid.Answer4CB.IsChecked = false;
												CategoryGrid.Answer5CB.IsChecked = false;
											};

			CategoryGrid.Answer3CB.Checked += delegate {
												if (CategoryGrid.Answer3CB.IsChecked == null || LoadedQuestion.QuestionType == QuestionType.MultiSelect ||
													!(bool) CategoryGrid.Answer3CB.IsChecked)
													return;
												CategoryGrid.Answer1CB.IsChecked = false;
												CategoryGrid.Answer2CB.IsChecked = false;
												CategoryGrid.Answer4CB.IsChecked = false;
												CategoryGrid.Answer5CB.IsChecked = false;
											};

			CategoryGrid.Answer4CB.Checked += delegate {
												if (CategoryGrid.Answer4CB.IsChecked == null || LoadedQuestion.QuestionType == QuestionType.MultiSelect ||
													!(bool) CategoryGrid.Answer4CB.IsChecked)
													return;
												CategoryGrid.Answer1CB.IsChecked = false;
												CategoryGrid.Answer2CB.IsChecked = false;
												CategoryGrid.Answer3CB.IsChecked = false;
												CategoryGrid.Answer5CB.IsChecked = false;
											};

			CategoryGrid.Answer5CB.Checked += delegate {
												if (CategoryGrid.Answer5CB.IsChecked == null || LoadedQuestion.QuestionType == QuestionType.MultiSelect ||
													!(bool) CategoryGrid.Answer5CB.IsChecked)
													return;
												CategoryGrid.Answer1CB.IsChecked = false;
												CategoryGrid.Answer2CB.IsChecked = false;
												CategoryGrid.Answer3CB.IsChecked = false;
												CategoryGrid.Answer4CB.IsChecked = false;
											};
		    NewExamFlyout.IsCreatingExam += delegate {
		                                        exam.Name = NewExamFlyout.ExamNameTextbox.Text;
		                                        EnableControls = true;
		                                    };
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
		    QuestionTabs.SelectedIndex = 0;
		}

		void LoadCorrectAnswers() {
			var Answers = LoadedQuestion.GetCorrectAnswerString();
			switch (LoadedQuestion.QuestionType)
			{
				case QuestionType.MultiSingle:
				case QuestionType.MultiSingleNoShuffle:
					switch (Answers.IndexOf("1", StringComparison.Ordinal))
					{
						case 0:
							CategoryGrid.Answer1CB.IsChecked = true;
							break;
						case 1:
							CategoryGrid.Answer2CB.IsChecked = true;
							break;
						case 2:
							CategoryGrid.Answer3CB.IsChecked = true;
							break;
						case 3:
							CategoryGrid.Answer4CB.IsChecked = true;
							break;
						case 4:
							CategoryGrid.Answer5CB.IsChecked = true;
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}
					break;

				case QuestionType.MultiSelect:
					CategoryGrid.Answer1CB.IsChecked = Answers[0].ToString() == "1";
					CategoryGrid.Answer2CB.IsChecked = Answers[1].ToString() == "1";
					CategoryGrid.Answer3CB.IsChecked = Answers[2].ToString() == "1";
					CategoryGrid.Answer4CB.IsChecked = Answers[3].ToString() == "1";
					CategoryGrid.Answer5CB.IsChecked = Answers[4].ToString() == "1";
					break;

				case QuestionType.TrueFalse:
					if (Answers.IndexOf("1", StringComparison.Ordinal) == 0) { CategoryGrid.Answer1CB.IsChecked = true; }
					else { CategoryGrid.Answer2CB.IsChecked = true;}
					break;

				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		void SaveCorrectAnswers() {
			var FinalAnswer = "";
			if (CategoryGrid.Answer1CB.IsChecked != null && (bool) CategoryGrid.Answer1CB.IsChecked) { FinalAnswer = string.Concat(FinalAnswer, "A"); }
			if (CategoryGrid.Answer2CB.IsChecked != null && (bool)CategoryGrid.Answer2CB.IsChecked) { FinalAnswer = string.Concat(FinalAnswer, "B"); }
			if (CategoryGrid.Answer3CB.IsChecked != null && (bool)CategoryGrid.Answer3CB.IsChecked) { FinalAnswer = string.Concat(FinalAnswer, "C"); }
			if (CategoryGrid.Answer4CB.IsChecked != null && (bool)CategoryGrid.Answer4CB.IsChecked) { FinalAnswer = string.Concat(FinalAnswer, "D"); }
			if (CategoryGrid.Answer5CB.IsChecked != null && (bool)CategoryGrid.Answer5CB.IsChecked) { FinalAnswer = string.Concat(FinalAnswer, "E"); }
			LoadedQuestion.SetCorrectAnswer(FinalAnswer);
		}

		void LoadQuestion() {
			LoadedQuestion = exam.Categories[GetIDNum(CurrentCategory?.Name)].Questions[GetIDNum(CurrentQuestion?.Name)];

			CategoryGrid.QuestionTextTB.Text = LoadedQuestion.Text;
			CategoryGrid.AnswerOneText.Text = LoadedQuestion.A.Text;
			CategoryGrid.AnswerTwoText.Text = LoadedQuestion.B.Text;
			CategoryGrid.AnswerThreeText.Text = LoadedQuestion.C.Text;
			CategoryGrid.AnswerFourText.Text = LoadedQuestion.D.Text;
			CategoryGrid.AnswerFiveText.Text = LoadedQuestion.E.Text;
			SetQuestionType(LoadedQuestion.QuestionType);
			CategoryNameTB.Text = exam.Categories[GetIDNum(CurrentCategory?.Name)].Name;
			LoadCorrectAnswers();
		    EnableControls = true;
		}

		void QuestionTabs_OnSelectionChanged(object O, SelectionChangedEventArgs E) {
			var OldQuestion = (TabItem) E.RemovedItems[0];
			if (OldQuestion != null && CurrentCategory != null) SaveQuestion(CurrentCategory, OldQuestion);

			var TabControl = (MetroAnimatedSingleRowTabControl) O;
			CurrentQuestion = (TabItem) TabControl.SelectedItem;
			if (CurrentCategory != null && CurrentQuestion != null) LoadQuestion();
		}

		void SaveQuestion(IFrameworkInputElement category, IFrameworkInputElement question) {
			LoadedQuestion = exam.Categories[GetIDNum(category?.Name)].Questions[GetIDNum(question?.Name)];

			LoadedQuestion.Text = CategoryGrid.QuestionTextTB.Text;
			LoadedQuestion.A.Text = CategoryGrid.AnswerOneText.Text;
			LoadedQuestion.B.Text = CategoryGrid.AnswerTwoText.Text;
			LoadedQuestion.C.Text = CategoryGrid.AnswerThreeText.Text;
			LoadedQuestion.D.Text = CategoryGrid.AnswerFourText.Text;
			LoadedQuestion.E.Text = CategoryGrid.AnswerFiveText.Text;
			exam.Categories[GetIDNum(category?.Name)].Name = CategoryNameTB.Text;
			SaveCorrectAnswers();
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

				case QuestionType.MultiSingleNoShuffle:
					CategoryGrid.MultiSingleNoShuffleRB.IsChecked = true;
					break;

				default:
					throw new ArgumentOutOfRangeException(nameof(QT), QT, null);
			}
		}

		void ShowCreateExamFlyout(object Sender, RoutedEventArgs E) { NewExamFlyout.IsOpen = true; }

		#endregion Private Methods

		void EmailExam(object Sender, RoutedEventArgs E) { }

		async void OpenFile(object Sender, RoutedEventArgs E) {
			var OFD = new OpenFileDialog {Filter = "Text files (*.txt)|*.txt"};
			if (OFD.ShowDialog() != true) return;
			using (new WaitCursor())
			{
			    exam = new Exam();
				await ExamParser.ReadExamFile(OFD.FileName, exam);
				LoadQuestion();
			}
		}

		async void SaveFile(object Sender, RoutedEventArgs E) {
			var SFD = new SaveFileDialog();
			if (SFD.ShowDialog() != true) return;
			using (new WaitCursor())
			{
				SaveQuestion(CurrentCategory, CurrentQuestion);
				await ExamParser.WriteExamFile(SFD.FileName, exam);
                
			}
		}
	}
}