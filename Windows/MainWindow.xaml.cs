#region Header

// Description:
// 
// Solution: Exam Formatter
// Project: Exam Formatter
// 
// Created: 01/03/2016 7:36 PM
// Last Revised: 01/24/2016 2:52 AM
// Last Revised by: Alex Gravely - Alex

#endregion

namespace Exam_Formatter.Windows {
    #region Using

    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using Classes;
    using Enums;
    using MahApps.Metro.Controls;
    using MahApps.Metro.Controls.Dialogs;
    using Microsoft.Win32;

    #endregion

    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow {
        #region Public Fields + Properties

        public bool EnableControls
        {
            get { return enableControls; }
            set
            {
                CategoryGrid.SetControlsEnabled(value);
                enableControls = value;
            }
        }

        #endregion Public Fields + Properties

        #region Private Fields + Properties

        const string HELP_MESSAGE =
            "To link to a card, use <card></card> tags. \n Ex: 'This will link to <card>Bujin Yamato</card>.\n\n" +
            "To link to the various policy documents, use [POLICY-KDE], [POLICY-YGO], or [POLICY-PENALTY].\n\n" +
            "You may double click on the Category names to rename them!";

        TabItem currentCategory;
        TabItem currentQuestion;
        bool enableControls;
        Exam exam;
        Question loadedQuestion;

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
            loadedQuestion = exam.Categories[ 0 ].Questions[ 0 ];
            currentCategory = C1;
            currentQuestion = Q1;
        }

        #endregion Public Constructors

        #region Private Methods

        static int GetIdNum(string name) => Convert.ToInt32(name.Remove(0, 1)) - 1;

        void CategoryTabs_OnSelectionChanged(object o, SelectionChangedEventArgs e) {
            var oldCategory = (TabItem) e.RemovedItems[ 0 ];
            if ( oldCategory != null &&
                 currentQuestion != null ) {
                     SaveQuestion(oldCategory, currentQuestion);
                 }

            var newTabControl = (MetroAnimatedSingleRowTabControl) o;
            currentCategory = (TabItem) newTabControl.SelectedItem;
            if ( currentCategory != null &&
                 currentQuestion != null ) {
                     LoadQuestion();
                 }
            QuestionTabs.SelectedIndex = 0;
        }

        [ SuppressMessage("ReSharper", "CyclomaticComplexity") ]
        void CheckForNoSelectedAnswers(object sender, RoutedEventArgs routedEventArgs) {
            var checkBox = (CheckBox) sender;
            if ( Equals(checkBox, CategoryGrid.Answer1CheckBox) )
            {
                if ( CategoryGrid.Answer2CheckBox.IsChecked.GetValueOrDefault() ||
                     CategoryGrid.Answer3CheckBox.IsChecked.GetValueOrDefault() ||
                     CategoryGrid.Answer4CheckBox.IsChecked.GetValueOrDefault() ||
                     CategoryGrid.Answer5CheckBox.IsChecked.GetValueOrDefault() ) {
                         return;
                     }
            }
            else if ( Equals(checkBox, CategoryGrid.Answer2CheckBox) )
            {
                if ( CategoryGrid.Answer1CheckBox.IsChecked.GetValueOrDefault() ||
                     CategoryGrid.Answer3CheckBox.IsChecked.GetValueOrDefault() ||
                     CategoryGrid.Answer4CheckBox.IsChecked.GetValueOrDefault() ||
                     CategoryGrid.Answer5CheckBox.IsChecked.GetValueOrDefault() ) {
                         return;
                     }
            }
            else if ( Equals(checkBox, CategoryGrid.Answer3CheckBox) )
            {
                if ( CategoryGrid.Answer1CheckBox.IsChecked.GetValueOrDefault() ||
                     CategoryGrid.Answer2CheckBox.IsChecked.GetValueOrDefault() ||
                     CategoryGrid.Answer4CheckBox.IsChecked.GetValueOrDefault() ||
                     CategoryGrid.Answer5CheckBox.IsChecked.GetValueOrDefault() ) {
                         return;
                     }
            }
            else if ( Equals(checkBox, CategoryGrid.Answer4CheckBox) )
            {
                if ( CategoryGrid.Answer1CheckBox.IsChecked.GetValueOrDefault() ||
                     CategoryGrid.Answer2CheckBox.IsChecked.GetValueOrDefault() ||
                     CategoryGrid.Answer3CheckBox.IsChecked.GetValueOrDefault() ||
                     CategoryGrid.Answer5CheckBox.IsChecked.GetValueOrDefault() ) {
                         return;
                     }
            }
            else if ( Equals(checkBox, CategoryGrid.Answer5CheckBox) )
            {
                if ( CategoryGrid.Answer1CheckBox.IsChecked.GetValueOrDefault() ||
                     CategoryGrid.Answer2CheckBox.IsChecked.GetValueOrDefault() ||
                     CategoryGrid.Answer3CheckBox.IsChecked.GetValueOrDefault() ||
                     CategoryGrid.Answer4CheckBox.IsChecked.GetValueOrDefault() ) {
                         return;
                     }
            }
            checkBox.IsChecked = false;
        }

        void DonateButton_OnClick(object sender, RoutedEventArgs e) => Process.Start("https://paypal.me/AntiTcb");

        async void EditCategoryNameAsync(object sender, MouseButtonEventArgs e) {
            var categoryLabel = sender as TabItem;
            if ( categoryLabel == null ) { return; }
            var thisCategory = exam.Categories[ GetIdNum(currentCategory.Name) ];
            var output = await this.ShowInputAsync
                                   ("Rename the Category", $"{thisCategory.Name}",
                                    new MetroDialogSettings { DefaultText = nameof(Category) });
            categoryLabel.Header = output == nameof(Category) || output == string.Empty
                                       ? $"{nameof(Category)} {categoryLabel.Name.Substring(1)}"
                                       : output ?? categoryLabel.Name;
            thisCategory.Name = categoryLabel.Header.ToString();
        }

        async void EditExamNameAsync(object sender, RoutedEventArgs e) {
            if ( !EnableControls ) { return; }
            var editName =
                await
                this.ShowInputAsync
                    ("Edit Exam Name", "Name this exam!", new MetroDialogSettings { DefaultText = exam.Name });
            if ( editName != null ) { exam.Name = editName; }
        }

        void LoadCorrectAnswers() {
            var answers = loadedQuestion.GetCorrectAnswerString();
            switch ( loadedQuestion.QuestionType )
            {
                case QuestionType.MultiSingle:
                case QuestionType.MultiSingleNoShuffle:
                    switch ( answers.IndexOf("1", StringComparison.Ordinal) )
                    {
                        case 0:
                            CategoryGrid.Answer1CheckBox.IsChecked = true;
                            break;

                        case 1:
                            CategoryGrid.Answer2CheckBox.IsChecked = true;
                            break;

                        case 2:
                            CategoryGrid.Answer3CheckBox.IsChecked = true;
                            break;

                        case 3:
                            CategoryGrid.Answer4CheckBox.IsChecked = true;
                            break;

                        case 4:
                            CategoryGrid.Answer5CheckBox.IsChecked = true;
                            break;

                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;

                case QuestionType.MultiSelect:
                    CategoryGrid.Answer1CheckBox.IsChecked = answers[ 0 ].ToString() == "1";
                    CategoryGrid.Answer2CheckBox.IsChecked = answers[ 1 ].ToString() == "1";
                    CategoryGrid.Answer3CheckBox.IsChecked = answers[ 2 ].ToString() == "1";
                    CategoryGrid.Answer4CheckBox.IsChecked = answers[ 3 ].ToString() == "1";
                    CategoryGrid.Answer5CheckBox.IsChecked = answers[ 4 ].ToString() == "1";
                    break;

                case QuestionType.TrueFalse:
                    if ( answers.IndexOf("1", StringComparison.Ordinal) == 0 ) {
                        CategoryGrid.Answer1CheckBox.IsChecked = true;
                    }
                    else
                    {
                        CategoryGrid.Answer2CheckBox.IsChecked = true;
                    }
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        void LoadQuestion() {
            loadedQuestion =
                exam.Categories[ GetIdNum(currentCategory?.Name) ].Questions[ GetIdNum(currentQuestion?.Name) ];

            CategoryGrid.QuestionTextTextBox.Text = loadedQuestion.Text;
            CategoryGrid.AnswerOneTextBox.Text = loadedQuestion.A.Text;
            CategoryGrid.AnswerTwoTextBox.Text = loadedQuestion.B.Text;
            CategoryGrid.AnswerThreeTextBox.Text = loadedQuestion.C.Text;
            CategoryGrid.AnswerFourTextBox.Text = loadedQuestion.D.Text;
            CategoryGrid.AnswerFiveTextBox.Text = loadedQuestion.E.Text;
            SetQuestionType(loadedQuestion.QuestionType);
            LoadCorrectAnswers();

            var examNameTitle = string.IsNullOrEmpty(exam.Name) ? "Exam Loaded" : exam.Name.Trim();
            var categoryTitle = string.IsNullOrEmpty(exam.Categories[ GetIdNum(currentCategory?.Name) ].Name)
                                    ? $"Category {GetIdNum(currentCategory?.Name)}"
                                    : exam.Categories[ GetIdNum(currentCategory?.Name) ].Name.Trim();

            Title = $"Exam Formatter - {examNameTitle} - {categoryTitle} - Question {loadedQuestion.ID}";
        }

        async void MainWindow_OnClosing(object sender, CancelEventArgs e) {
            var closeDialog = await this.ShowMessageAsync("Close?", "Are you sure you wish to exit?");
            if ( closeDialog == MessageDialogResult.Affirmative ) { Application.Current.Shutdown(); }
        }

        async void OpenFileAsync(object sender, RoutedEventArgs e) {
            var ofd = new OpenFileDialog { Filter = "Text Files (*.txt)|*.txt" };
            if ( ofd.ShowDialog() != true ) { return; }
            using ( new WaitCursor() )
            {
                exam = new Exam();
                await ExamParser.ReadExamFileAsync(ofd.FileName, exam);
                LoadQuestion();
                EnableControls = true;
                foreach (
                    var tabItem in
                        CategoryTabs.Items.Cast< object >().
                                     Where
                            (tabItem =>
                             !string.IsNullOrEmpty(exam.Categories[ CategoryTabs.Items.IndexOf(tabItem) ].Name)) ) {
                                 ( (TabItem) tabItem ).Header = exam.Categories[ CategoryTabs.Items.IndexOf(tabItem) ].Name.Trim();
                             }
            }
        }

        void QuestionTabs_OnSelectionChanged(object o, SelectionChangedEventArgs e) {
            var oldQuestion = (TabItem) e.RemovedItems[ 0 ];
            if ( oldQuestion != null &&
                 currentCategory != null ) {
                     SaveQuestion(currentCategory, oldQuestion);
                 }

            var tabControl = (MetroAnimatedSingleRowTabControl) o;
            currentQuestion = (TabItem) tabControl.SelectedItem;
            if ( currentCategory != null &&
                 currentQuestion != null ) {
                     LoadQuestion();
                 }
        }

        void SaveCorrectAnswers() {
            var finalAnswer = "";
            if ( CategoryGrid.Answer1CheckBox.IsChecked != null &&
                 (bool) CategoryGrid.Answer1CheckBox.IsChecked ) {
                     finalAnswer = string.Concat(finalAnswer, "A");
                 }
            if ( CategoryGrid.Answer2CheckBox.IsChecked != null &&
                 (bool) CategoryGrid.Answer2CheckBox.IsChecked ) {
                     finalAnswer = string.Concat(finalAnswer, "B");
                 }
            if ( CategoryGrid.Answer3CheckBox.IsChecked != null &&
                 (bool) CategoryGrid.Answer3CheckBox.IsChecked ) {
                     finalAnswer = string.Concat(finalAnswer, "C");
                 }
            if ( CategoryGrid.Answer4CheckBox.IsChecked != null &&
                 (bool) CategoryGrid.Answer4CheckBox.IsChecked ) {
                     finalAnswer = string.Concat(finalAnswer, "D");
                 }
            if ( CategoryGrid.Answer5CheckBox.IsChecked != null &&
                 (bool) CategoryGrid.Answer5CheckBox.IsChecked ) {
                     finalAnswer = string.Concat(finalAnswer, "E");
                 }
            loadedQuestion.SetCorrectAnswer(finalAnswer);
        }

        async void SaveFileAsync(object sender, RoutedEventArgs e) {
            var sfd = new SaveFileDialog
                      {
                          DefaultExt = ".txt",
                          Filter = "Text Files (*.txt) | *.txt"
                      };
            if ( sfd.ShowDialog() != true ) { return; }
            using ( new WaitCursor() )
            {
                var controller = await this.ShowProgressAsync("Saving exam...", "Writing the exam file!");
                controller.SetIndeterminate();
                SaveQuestion(currentCategory, currentQuestion);
                await ExamParser.WriteExamFileAsync(sfd.FileName, exam);
                await controller.CloseAsync();
                await this.ShowMessageAsync("Exam Saved!", $"Exam saved to {sfd.FileName}!");
            }
        }

        void SaveQuestion(IFrameworkInputElement category, IFrameworkInputElement question) {
            loadedQuestion = exam.Categories[ GetIdNum(category?.Name) ].Questions[ GetIdNum(question?.Name) ];

            loadedQuestion.Text = CategoryGrid.QuestionTextTextBox.Text;
            loadedQuestion.A.Text = CategoryGrid.AnswerOneTextBox.Text;
            loadedQuestion.B.Text = CategoryGrid.AnswerTwoTextBox.Text;
            loadedQuestion.C.Text = CategoryGrid.AnswerThreeTextBox.Text;
            loadedQuestion.D.Text = CategoryGrid.AnswerFourTextBox.Text;
            loadedQuestion.E.Text = CategoryGrid.AnswerFiveTextBox.Text;
            SaveCorrectAnswers();
        }

        void SetEventHandlers() {
            CategoryTabs.SelectionChanged += CategoryTabs_OnSelectionChanged;
            QuestionTabs.SelectionChanged += QuestionTabs_OnSelectionChanged;

            foreach ( TabItem tabItem in CategoryTabs.Items ) { tabItem.MouseDoubleClick += EditCategoryNameAsync; }

            CategoryGrid.MultiSelectRadioButton.Checked +=
                delegate { loadedQuestion.QuestionType = QuestionType.MultiSelect; };
            CategoryGrid.MultiSingleRadioButton.Checked +=
                delegate { loadedQuestion.QuestionType = QuestionType.MultiSingle; };
            CategoryGrid.TrueFalseRadioButton.Checked +=
                delegate { loadedQuestion.QuestionType = QuestionType.TrueFalse; };
            CategoryGrid.MultiSingleNoShuffleRadioButton.Checked +=
                delegate { loadedQuestion.QuestionType = QuestionType.MultiSingleNoShuffle; };
            CategoryGrid.Answer1CheckBox.Checked += delegate {
                                                        if ( CategoryGrid.Answer1CheckBox.IsChecked == null ||
                                                             loadedQuestion.QuestionType == QuestionType.MultiSelect ||
                                                             !(bool) CategoryGrid.Answer1CheckBox.IsChecked ) {
                                                                 return;
                                                             }
                                                        CategoryGrid.Answer2CheckBox.IsChecked = false;
                                                        CategoryGrid.Answer3CheckBox.IsChecked = false;
                                                        CategoryGrid.Answer4CheckBox.IsChecked = false;
                                                        CategoryGrid.Answer5CheckBox.IsChecked = false;
                                                    };
            CategoryGrid.Answer2CheckBox.Checked += delegate {
                                                        if ( CategoryGrid.Answer2CheckBox.IsChecked == null ||
                                                             loadedQuestion.QuestionType == QuestionType.MultiSelect ||
                                                             !(bool) CategoryGrid.Answer2CheckBox.IsChecked ) {
                                                                 return;
                                                             }
                                                        CategoryGrid.Answer1CheckBox.IsChecked = false;
                                                        CategoryGrid.Answer3CheckBox.IsChecked = false;
                                                        CategoryGrid.Answer4CheckBox.IsChecked = false;
                                                        CategoryGrid.Answer5CheckBox.IsChecked = false;
                                                    };

            CategoryGrid.Answer3CheckBox.Checked += delegate {
                                                        if ( CategoryGrid.Answer3CheckBox.IsChecked == null ||
                                                             loadedQuestion.QuestionType == QuestionType.MultiSelect ||
                                                             !(bool) CategoryGrid.Answer3CheckBox.IsChecked ) {
                                                                 return;
                                                             }
                                                        CategoryGrid.Answer1CheckBox.IsChecked = false;
                                                        CategoryGrid.Answer2CheckBox.IsChecked = false;
                                                        CategoryGrid.Answer4CheckBox.IsChecked = false;
                                                        CategoryGrid.Answer5CheckBox.IsChecked = false;
                                                    };

            CategoryGrid.Answer4CheckBox.Checked += delegate {
                                                        if ( CategoryGrid.Answer4CheckBox.IsChecked == null ||
                                                             loadedQuestion.QuestionType == QuestionType.MultiSelect ||
                                                             !(bool) CategoryGrid.Answer4CheckBox.IsChecked ) {
                                                                 return;
                                                             }
                                                        CategoryGrid.Answer1CheckBox.IsChecked = false;
                                                        CategoryGrid.Answer2CheckBox.IsChecked = false;
                                                        CategoryGrid.Answer3CheckBox.IsChecked = false;
                                                        CategoryGrid.Answer5CheckBox.IsChecked = false;
                                                    };

            CategoryGrid.Answer5CheckBox.Checked += delegate {
                                                        if ( CategoryGrid.Answer5CheckBox.IsChecked == null ||
                                                             loadedQuestion.QuestionType == QuestionType.MultiSelect ||
                                                             !(bool) CategoryGrid.Answer5CheckBox.IsChecked ) {
                                                                 return;
                                                             }
                                                        CategoryGrid.Answer1CheckBox.IsChecked = false;
                                                        CategoryGrid.Answer2CheckBox.IsChecked = false;
                                                        CategoryGrid.Answer3CheckBox.IsChecked = false;
                                                        CategoryGrid.Answer4CheckBox.IsChecked = false;
                                                    };
            NewExamFlyout.IsCreatingExam += delegate {
                                                exam = new Exam { Name = NewExamFlyout.ExamNameTextbox.Text };
                                                EnableControls = true;

                                                CategoryGrid.Answer1CheckBox.Unchecked += CheckForNoSelectedAnswers;
                                                CategoryGrid.Answer2CheckBox.Unchecked += CheckForNoSelectedAnswers;
                                                CategoryGrid.Answer3CheckBox.Unchecked += CheckForNoSelectedAnswers;
                                                CategoryGrid.Answer4CheckBox.Unchecked += CheckForNoSelectedAnswers;
                                                CategoryGrid.Answer5CheckBox.Unchecked += CheckForNoSelectedAnswers;
                                            };
        }

        void SetQuestionType(QuestionType qt) {
            switch ( qt )
            {
                case QuestionType.MultiSingle:
                    CategoryGrid.MultiSingleRadioButton.IsChecked = true;
                    break;

                case QuestionType.MultiSelect:
                    CategoryGrid.MultiSelectRadioButton.IsChecked = true;
                    break;

                case QuestionType.TrueFalse:
                    CategoryGrid.TrueFalseRadioButton.IsChecked = true;
                    break;

                case QuestionType.MultiSingleNoShuffle:
                    CategoryGrid.MultiSingleNoShuffleRadioButton.IsChecked = true;
                    break;

                default:
                    Debug.WriteLine(qt);
                    throw new ArgumentOutOfRangeException(nameof(qt), qt, null);
            }
        }

        void ShowCreateExamFlyout(object sender, RoutedEventArgs e) => NewExamFlyout.IsOpen = true;

        async void ShowHelpDialogAsync(object sender, RoutedEventArgs e)
            => await this.ShowMessageAsync("Help!", HELP_MESSAGE);

        #endregion Private Methods
    }
}