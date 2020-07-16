using System;
using System.Collections.Generic;
using System.Linq;

namespace World_yachts.Model.Logic
{
    public class FormattingList
    {
        public static string GetFormattedList(List<string> list)
        {
            string str = "";

            for (int i = 0; i < list.Count; i++)
            {
                if (i < list.Count - 1)
                {
                    str += $"'{list[i]}', ";
                }
                else
                {
                    str += $"'{list[i]}'";
                }
            }

            return str;
        }

        public static List<string> GetList(string strValues)
        {
            return strValues.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }
    }
}
