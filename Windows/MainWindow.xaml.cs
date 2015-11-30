
using System.Windows;
using Exam_Formatter.Classes;
using Exam_Formatter.FlyoutControls;

namespace Exam_Formatter.Windows
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow {
		Exam exam;
		public MainWindow() {
			exam = new Exam();
			InitializeComponent();
		}

		void ShowCreateExamFlyout(object Sender, RoutedEventArgs E) { CreateExamFlyout.IsOpen = true; }
	}
}
