using System;
using System.Collections.Generic;

namespace GOE
{
    [Serializable]
    public class CommonStringList
    {
        public List<string> string_list;


        public static CommonStringList[] MakeArrayFromString(string _str)
        {
            List<CommonStringList> list = MakeListFromString(_str);
            if (_str is not { Length: > 0 })
                return Array.Empty<CommonStringList>();
            return list.ToArray();
        }
        public static List<CommonStringList> MakeListFromString(string _str)
        {
            List<CommonStringList> list = new List<CommonStringList>();
            if (_str is not { Length: > 0 })
                return list;
        
            string[] strArray = _str.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string str in strArray)
            {
                CommonStringList newItem = new CommonStringList();
                newItem.ParseFromString(str);
                list.Add(newItem);
            }

            return list;
        }
        public void ParseFromString(string _str)
        {
            string[] strArray = _str.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            string_list = new List<string>(strArray);
        }
    }
}