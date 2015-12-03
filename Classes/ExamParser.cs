#region Header
// Alex Gravely - Alex
// 
// Exam Formatter - Exam Formatter
// ExamParser.cs - 02//12//2015 10:27 PM
#endregion

using System.IO;
using System.Threading.Tasks;

namespace Exam_Formatter.Classes {
	public static class ExamParser {

		static async Task ReadExamFile(string filepath, Exam exam) {
			using (var SR = new StreamReader(filepath))
			{
				while (!SR.EndOfStream)
				{
					var Line = await SR.ReadLineAsync();

					switch ()
				}
			}
		}
	}
}