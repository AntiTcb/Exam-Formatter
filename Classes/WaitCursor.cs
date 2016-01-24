#region Header

// Description:
//
// Solution: Exam Formatter
// Project: Exam Formatter
//
// Created: 12/03/2015 11:54 PM
// Last Revised: 01/22/2016 5:28 AM
// Last Revised by: Alex Gravely - Alex

#endregion Header

namespace Exam_Formatter.Classes
{
    #region Using

    using System;
    using System.Windows.Input;

    #endregion Using

    public class WaitCursor : IDisposable {

        #region Private Fields + Properties

        readonly Cursor previousCursor;

        #endregion Private Fields + Properties

        #region Public Constructors

        public WaitCursor() {
            previousCursor = Mouse.OverrideCursor;
            Mouse.OverrideCursor = Cursors.Wait;
        }

        #endregion Public Constructors

        #region IDisposable Members

        public void Dispose() => Mouse.OverrideCursor = previousCursor;

        #endregion IDisposable Members
    }
}