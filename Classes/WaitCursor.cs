#region Header
// Alex Gravely - Alex
// 
// Exam Formatter - Exam Formatter
// WaitCursor.cs - 03//12//2015 11:54 PM
#endregion

using System;
using System.Windows.Input;

namespace Exam_Formatter.Classes {
	public class WaitCursor : IDisposable {
		Cursor _previousCursor;

		public WaitCursor() {
			_previousCursor = Mouse.OverrideCursor;
			Mouse.OverrideCursor = Cursors.Wait;
		}

		#region IDisposable Members

		public void Dispose() { Mouse.OverrideCursor = _previousCursor; }

		#endregion
	}
}