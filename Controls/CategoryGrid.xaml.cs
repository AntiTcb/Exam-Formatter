#region Header

// Description:
//
// Solution: Exam Formatter
// Project: Exam Formatter
//
// Created: 11/29/2015 11:04 PM
// Last Revised: 01/22/2016 5:28 AM
// Last Revised by: Alex Gravely - Alex

#endregion Header

namespace Exam_Formatter.Controls
{
    #region Using

    using System.Diagnostics.CodeAnalysis;
    using MahApps.Metro.Controls;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    #endregion Using

    /// <summary>
    ///     Interaction logic for CategoryGrid.xaml
    /// </summary>
    public partial class CategoryGrid {

        #region Private Fields + Properties

        string answerOneTextHolder;
        string answerTwoTextHolder;

        #endregion Private Fields + Properties

        #region Public Constructors

        public CategoryGrid()
        {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Methods

        [ SuppressMessage("ReSharper", "CyclomaticComplexity") ]
        public void SetControlsEnabled(bool areControlsEnabled) {
            Answer1CheckBox.IsEnabled = areControlsEnabled;
            Answer2CheckBox.IsEnabled = areControlsEnabled;
            Answer3CheckBox.IsEnabled = areControlsEnabled;
            Answer4CheckBox.IsEnabled = areControlsEnabled;
            Answer5CheckBox.IsEnabled = areControlsEnabled;

            AnswerOneTextBox.IsEnabled = areControlsEnabled;
            AnswerTwoTextBox.IsEnabled = areControlsEnabled;
            AnswerThreeTextBox.IsEnabled = areControlsEnabled;
            AnswerFourTextBox.IsEnabled = areControlsEnabled;
            AnswerFiveTextBox.IsEnabled = areControlsEnabled;

            QuestionTextTextBox.IsEnabled = areControlsEnabled;

            MultiSelectRadioButton.IsEnabled = areControlsEnabled;
            MultiSingleRadioButton.IsEnabled = areControlsEnabled;
            MultiSingleNoShuffleRadioButton.IsEnabled = areControlsEnabled;
            TrueFalseRadioButton.IsEnabled = areControlsEnabled;
        }

        #endregion Public Methods

        #region Private Methods

        void StyleCorrectCheckbox(object sender, RoutedEventArgs e) {
            var cb = (CheckBox) sender;
            cb.Foreground = Brushes.Green;
            cb.Content = "Correct";
        }

        void StyleIncorrectCheckbox(object sender, RoutedEventArgs e) {
            var cb = (CheckBox) sender;
            cb.Foreground = Brushes.Red;
            cb.Content = "Incorrect";
        }

        void TrueFalseRB_OnChecked(object sender, RoutedEventArgs e) {
            Answer1CheckBox.IsEnabled = true;
            Answer2CheckBox.IsEnabled = true;
            Answer3CheckBox.IsEnabled = false;
            Answer4CheckBox.IsEnabled = false;
            Answer5CheckBox.IsEnabled = false;

            AnswerThreeTextBox.IsEnabled = false;
            AnswerFourTextBox.IsEnabled = false;
            AnswerFiveTextBox.IsEnabled = false;

            answerOneTextHolder = AnswerOneTextBox.Text;
            answerTwoTextHolder = AnswerTwoTextBox.Text;

            AnswerOneTextBox.Text = string.Empty;
            AnswerTwoTextBox.Text = string.Empty;

            TextBoxHelper.SetWatermark(AnswerOneTextBox, "True");
            TextBoxHelper.SetWatermark(AnswerTwoTextBox, "False");
        }

        void TrueFalseRB_OnUnchecked(object sender, RoutedEventArgs e) {
            Answer1CheckBox.IsEnabled = true;
            Answer2CheckBox.IsEnabled = true;
            Answer3CheckBox.IsEnabled = true;
            Answer4CheckBox.IsEnabled = true;
            Answer5CheckBox.IsEnabled = true;

            AnswerThreeTextBox.IsEnabled = true;
            AnswerFourTextBox.IsEnabled = true;
            AnswerFiveTextBox.IsEnabled = true;

            TextBoxHelper.SetWatermark(AnswerOneTextBox, string.Empty);
            TextBoxHelper.SetWatermark(AnswerTwoTextBox, string.Empty);

            if ( answerOneTextHolder.Length != 0 ) { AnswerOneTextBox.Text = answerOneTextHolder; }
            if ( answerTwoTextHolder.Length != 0 ) { AnswerTwoTextBox.Text = answerTwoTextHolder; }
        }

        #endregion Private Methods
    }
}