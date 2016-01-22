#region Header

// Description:
//
// Solution: Exam Formatter
// Project: Exam Formatter
//
// Created: 12/03/2015 11:54 PM
// Last Revised: 01/12/2016 4:26 AM
// Last Revised by: Alex Gravely - Alex

#endregion Header

namespace Exam_Formatter.Classes
{
    #region Using

    using System;
    using System.Windows.Input;

    #endregion Using

    public class WaitCursor : IDisposable {
        readonly Cursor previousCursor;

        public WaitCursor() {
            previousCursor = Mouse.OverrideCursor;
            Mouse.OverrideCursor = Cursors.Wait;
        }

        #region IDisposable Members

        public void Dispose() => Mouse.OverrideCursor = previousCursor;

        #endregion IDisposable Members
    }
}