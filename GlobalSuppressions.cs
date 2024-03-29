#region Header

// Description:
//
// Solution: Exam Formatter
// Project: Exam Formatter
//
// Created: 01/22/2016 3:03 AM
// Last Revised: 01/22/2016 5:29 AM
// Last Revised by: Alex Gravely - Alex

#endregion Header

#region Using

using System.Diagnostics.CodeAnalysis;

#endregion Using

[assembly:
    SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations", Scope = "member",
        Target = "Exam_Formatter.Classes.Question.#ToString()")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly", Scope = "type",
        Target = "Exam_Formatter.Classes.WaitCursor")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly", Scope = "member",
        Target = "Exam_Formatter.Classes.WaitCursor.#Dispose()")]
[assembly:
    SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Scope = "member",
        Target = "Exam_Formatter.FlyoutControls.CreateExamFlyout.#IsCreatingExam")]
// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.
//
// To add a suppression to this file, right-click the message in the
// Code Analysis results, point to "Suppress Message", and click
// "In Suppression File".
// You do not need to add suppressions to this file manually.