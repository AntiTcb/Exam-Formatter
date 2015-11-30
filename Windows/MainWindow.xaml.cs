#region Header

// Alex Gravely - Alex
//
// Exam Formatter - Exam Formatter
// MainWindow.xaml.cs - 30//11//2015 1:26 AM

#endregion Header

#region Using

using Exam_Formatter.Classes;
using Microsoft.VisualBasic;
using System;
using System.Windows;
using System.Windows.Controls;

#endregion Using

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

		static int GetIDNum(string name) => Convert.ToInt32(Strings.Mid(name, 1));

		void CategoryTabs_OnSelectionChanged(object O, SelectionChangedEventArgs E) => CurrentCategory = (TabItem) O;

		void LoadQuestion() {
			var LoadedQuestion = exam.Categories[GetIDNum(CurrentCategory.Name)].Questions[GetIDNum(CurrentQuestion.Name)];
		}

		void QuestionTabs_OnSelectionChanged(object O, SelectionChangedEventArgs E) => CurrentQuestion = (TabItem) O;

		void ShowCreateExamFlyout(object Sender, RoutedEventArgs E)
		{
			CreateExamFlyout.IsOpen = true;
		}

		#endregion Private Methods
	}
}