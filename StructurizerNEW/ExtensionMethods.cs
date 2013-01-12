using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace StructurizerNEW
{
    public static class ExtensionMethods
    {
        public static double GetSize(this DirectoryInfo directoryInfo)
        {
            return GetFilesRecursive(directoryInfo).Sum(f => f.Length);
        }

        public static string ReadMarkdown(this FileInfo fileInfo)
        {
            using (var sr = new StreamReader(fileInfo.FullName))
            {
                var raw = sr.ReadToEnd();

                // TODO BDM: Move out of here

                raw = raw.Replace("TODO KVL:", "<span class=\"label label-important\">TODO KVL</span> ");
                raw = raw.Replace("TODO BDM:", "<span class=\"label label-important\">TODO BDM</span> ");


                return raw;
            }
        }

        public static IEnumerable<FileInfo> GetFilesRecursive(this DirectoryInfo directoryInfo)
        {
            return GetFilesRecursive(directoryInfo.ToString());
        }

        private static IEnumerable<FileInfo> GetFilesRecursive(string folder)
        {
            return Directory.GetFiles(folder).Select(f => new FileInfo(f)).Union(
                Directory.GetDirectories(folder).SelectMany(GetFilesRecursive));
        }

        public static string ToHumanReadable(this double sizeBytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            var order = 0;
            while (sizeBytes >= 1024 && order + 1 < sizes.Length)
            {
                order++;
                sizeBytes = sizeBytes / 1024;
            }

            // Adjust the format string to your preferences. For example "{0:0.#}{1}" would show a single decimal place, and no space.
            return String.Format("{0:0.##} {1}", sizeBytes, sizes[order]);
        }

        // Source: http://stackoverflow.com/questions/334850/c-sharp-how-to-replace-microsofts-smart-quotes-with-straight-quotation-marks
        public static string ReplaceOfficeCrap(this string buffer)
        {
            if (buffer.IndexOf('\u2013') > -1) buffer = buffer.Replace('\u2013', '-');
            if (buffer.IndexOf('\u2014') > -1) buffer = buffer.Replace('\u2014', '-');
            if (buffer.IndexOf('\u2015') > -1) buffer = buffer.Replace('\u2015', '-');
            if (buffer.IndexOf('\u2017') > -1) buffer = buffer.Replace('\u2017', '_');
            if (buffer.IndexOf('\u2018') > -1) buffer = buffer.Replace('\u2018', '\'');
            if (buffer.IndexOf('\u2019') > -1) buffer = buffer.Replace('\u2019', '\'');
            if (buffer.IndexOf('\u201a') > -1) buffer = buffer.Replace('\u201a', ',');
            if (buffer.IndexOf('\u201b') > -1) buffer = buffer.Replace('\u201b', '\'');
            if (buffer.IndexOf('\u201c') > -1) buffer = buffer.Replace('\u201c', '\"');
            if (buffer.IndexOf('\u201d') > -1) buffer = buffer.Replace('\u201d', '\"');
            if (buffer.IndexOf('\u201e') > -1) buffer = buffer.Replace('\u201e', '\"');
            if (buffer.IndexOf('\u2026') > -1) buffer = buffer.Replace("\u2026", "...");
            if (buffer.IndexOf('\u2032') > -1) buffer = buffer.Replace('\u2032', '\'');
            if (buffer.IndexOf('\u2033') > -1) buffer = buffer.Replace('\u2033', '\"');

            return buffer;
        }
    }

    /// <summary>
    /// Natural sort, see http://stackoverflow.com/questions/1032775/sorting-mixed-numbers-and-strings
    /// </summary>
    public class MixedNumbersAndStringsComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            double xVal, yVal;

            if (ExtractNumber(x, out xVal) && ExtractNumber(y, out yVal))
            {
                return xVal.CompareTo(yVal);
            }

            return string.Compare(x, y);
        }

        private bool ExtractNumber(string s, out double number)
        {
            number = 0;
            var success = false;
            for (int stringIndex = 1; stringIndex < s.Length; stringIndex++)
            {
                var stringPart = s.Substring(0, stringIndex);
                double val;
                if (double.TryParse(stringPart, out val))
                {
                    number = val;
                    success = true;
                }
                else
                {
                    break;
                }
            }

            return success;
        }
    }
}