#region Header

// Alex Gravely - Alex
//
// Exam Formatter - Exam Formatter
// CategoryGrid.xaml.cs - 30//11//2015 1:25 AM

#endregion Header

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MahApps.Metro.Controls;

namespace Exam_Formatter.Controls {

	/// <summary>
	///     Interaction logic for CategoryGrid.xaml
	/// </summary>
	public partial class CategoryGrid {

		#region Public Constructors

		public CategoryGrid()
		{
			InitializeComponent();
		}

		#endregion Public Constructors

	    void StyleCorrectCheckbox(object Sender, RoutedEventArgs E) {
	        var cb = (CheckBox)Sender;
	        cb.Foreground = Brushes.Green;
	    }

	    void StyleIncorrectCheckbox(object Sender, RoutedEventArgs E) {
	        var cb = (CheckBox) Sender;
	        cb.Foreground = Brushes.Red;
	    }

	    public void SetControlsEnabled(bool AreControlsEnabled) {
            Answer1CB.IsEnabled = AreControlsEnabled;
            Answer2CB.IsEnabled = AreControlsEnabled;
            Answer3CB.IsEnabled = AreControlsEnabled;
            Answer4CB.IsEnabled = AreControlsEnabled;
            Answer5CB.IsEnabled = AreControlsEnabled;

            AnswerOneText.IsEnabled = AreControlsEnabled;
            AnswerTwoText.IsEnabled = AreControlsEnabled;
            AnswerThreeText.IsEnabled = AreControlsEnabled;
            AnswerFourText.IsEnabled = AreControlsEnabled;
            AnswerFiveText.IsEnabled = AreControlsEnabled;

            QuestionTextTB.IsEnabled = AreControlsEnabled;

            MultiSelectRB.IsEnabled = AreControlsEnabled;
            MultiSingleRB.IsEnabled = AreControlsEnabled;
            MultiSingleNoShuffleRB.IsEnabled = AreControlsEnabled;
            TrueFalseRB.IsEnabled = AreControlsEnabled;
        }

	    void TrueFalseRB_OnChecked(object Sender, RoutedEventArgs E) {
            Answer1CB.IsEnabled = false;
            Answer2CB.IsEnabled = false;
            Answer3CB.IsEnabled = false;
            Answer4CB.IsEnabled = false;
            Answer5CB.IsEnabled = false;
            
            AnswerThreeText.IsEnabled = false;
            AnswerFourText.IsEnabled = false;
            AnswerFiveText.IsEnabled = false;

	        TextBoxHelper.SetWatermark(AnswerOneText, "True");
            TextBoxHelper.SetWatermark(AnswerTwoText, "False");
	    }

	    void TrueFalseRB_OnUnchecked(object Sender, RoutedEventArgs E) {
            Answer1CB.IsEnabled = true;
            Answer2CB.IsEnabled = true;
            Answer3CB.IsEnabled = true;
            Answer4CB.IsEnabled = true;
            Answer5CB.IsEnabled = true;

            AnswerThreeText.IsEnabled = true;
            AnswerFourText.IsEnabled = true;
            AnswerFiveText.IsEnabled = true;

            TextBoxHelper.SetWatermark(AnswerOneText, string.Empty);
            TextBoxHelper.SetWatermark(AnswerTwoText, string.Empty);
        }
	}
}