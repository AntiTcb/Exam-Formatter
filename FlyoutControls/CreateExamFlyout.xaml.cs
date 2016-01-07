#region Header

// Alex Gravely - Alex
//
// Exam Formatter - Exam Formatter
// NewExamFlyout.xaml.cs - 30//11//2015 1:25 AM

#endregion Header

using System;
using System.Windows;

namespace Exam_Formatter.FlyoutControls {

    /// <summary>
    ///     Interaction logic for NewExamFlyout.xaml
    /// </summary>
    public delegate void CreatingExamDelegate(RoutedEventArgs e);
	public partial class CreateExamFlyout {

	    public event CreatingExamDelegate IsCreatingExam;
		#region Public Constructors

	    public CreateExamFlyout()
		{
			InitializeComponent();
	        CreateExamButton.Click += RaiseCreatingExamEvent;
	    }

	    protected void RaiseCreatingExamEvent(object sender, RoutedEventArgs routedEventArgs) {
	        OnCreatingExam(routedEventArgs);
	        Visibility = Visibility.Hidden;
	    }

	    protected virtual void OnCreatingExam(RoutedEventArgs e) => IsCreatingExam?.Invoke(e);

	    #endregion Public Constructors
	}
}