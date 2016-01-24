#region Header

// Description:
//
// Solution: Exam Formatter
// Project: Exam Formatter
//
// Created: 11/29/2015 9:40 PM
// Last Revised: 01/22/2016 5:29 AM
// Last Revised by: Alex Gravely - Alex

#endregion Header

namespace Exam_Formatter.FlyoutControls
{
    #region Using

    using System.Windows;

    #endregion Using

    public partial class CreateExamFlyout {

        #region Public Delegates + Events

        public event CreatingExamDelegate IsCreatingExam;

        #endregion Public Delegates + Events

        #region Public Constructors

        public CreateExamFlyout() {
            InitializeComponent();
            CreateExamButton.Click += RaiseCreatingExamEvent;
        }

        #endregion Public Constructors

        #region Protected Methods

        protected virtual void OnCreatingExam(RoutedEventArgs e) => IsCreatingExam?.Invoke(e);

        #endregion Protected Methods

        protected void RaiseCreatingExamEvent(object sender, RoutedEventArgs routedEventArgs) {
            OnCreatingExam(routedEventArgs);
            Visibility = Visibility.Hidden;
        }
    }

    /// <summary>
    ///     Interaction logic for NewExamFlyout.xaml
    /// </summary>
    public delegate void CreatingExamDelegate(RoutedEventArgs e);
}