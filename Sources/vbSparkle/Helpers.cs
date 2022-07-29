using System;
using System.Collections.Generic;
using System.Linq;

namespace vbSparkle
{
    internal class Helpers
    {
        internal static string IndentLines(int indent, string code)
        {
            List<string> lines = code.Split( new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).ToList();

            for (int i = 0; i < lines.Count; i++)
            {
                if (!string.IsNullOrWhiteSpace(code))
                    lines[i] = new string(' ', indent) + lines[i];
            }

            if (string.IsNullOrWhiteSpace(lines[lines.Count - 1]))
                lines.RemoveAt(lines.Count - 1);

            return string.Join("\r\n", lines);
        }

        internal static string CommentLines(int indent, string code)
        {
            List<string> lines = code.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).ToList();

            for (int i = 0; i < lines.Count; i++)
            {
                if (!string.IsNullOrWhiteSpace(code))
                    lines[i] = "\'" + new string(' ', indent) + lines[i];
            }

            if (string.IsNullOrWhiteSpace(lines[lines.Count - 1]))
                lines.RemoveAt(lines.Count - 1);

            return string.Join("\r\n", lines);
        }
    }
}