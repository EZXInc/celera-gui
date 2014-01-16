using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace EZXWPFLibrary.Utils
{
    public partial class StringUtils
    {
        public static string CamelCaseToLabel(string value)
        {
            return Regex.Replace(value, "([A-Z]{1,2}|[0-9]+)", " $1").TrimStart();
        }
        
        public static string CamelCaseToSentenceWithSmallCase(string value)
        {
            var sb = new StringBuilder();
            var firstWord = true;

            foreach (var match in Regex.Matches(value, "([A-Z][a-z]+)|[0-9]+"))
            {
                if (firstWord)
                {
                    sb.Append(match.ToString());
                    firstWord = false;
                }
                else
                {
                    sb.Append(" ");
                    sb.Append(match.ToString().ToLower());
                }
            }
            return sb.ToString();
        }

        public static string StringListToText(List<string> textList, string separator)
        {
            string text = string.Empty;
            if (textList != null)
            {
                for (int i = 0; i < textList.Count; i++)
                {
                    if (i == 0)
                    {
                        text = textList[i].Trim();
                    }
                    else
                    {
                        text = text + separator + textList[i].Trim();
                    }
                }
            }
            return text;
        }

        public static List<string> StringTextToList(string text, char separator)
        {
            List<string> textList = new List<string>();
            if (!string.IsNullOrEmpty(text))
            {
                textList = text.Split(separator).ToList();
            }
            return textList;
        }

        public static string GetObjectLog(object objectParam)
        {
            if (objectParam == null)
            {
                const string PARAMETER_NAME = "objectToGetStateOf";
                throw new ArgumentException(string.Format("Parameter {0} cannot be null", PARAMETER_NAME), PARAMETER_NAME);
            }
            var builder = new StringBuilder();

            foreach (var property in objectParam.GetType().GetProperties())
            {
                object value = property.GetValue(objectParam, null);

                builder.Append(property.Name)
                .Append(" = ")
                .Append((value ?? "null"))
                .AppendLine();
            }
            return builder.ToString();
        }

    }
}
