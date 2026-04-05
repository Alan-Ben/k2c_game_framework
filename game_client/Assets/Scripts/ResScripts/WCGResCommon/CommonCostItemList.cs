using System;
using System.Collections.Generic;

namespace GOE
{
    [Serializable]
    public class CommonCostItemList
    {
        public List<NPCommonCostItem> costItemList;
        
        
        public static CommonCostItemList[] MakeArrayFromString(string _str)
        {
            List<CommonCostItemList> list = MakeListFromString(_str);
            if (_str is not { Length: > 0 })
                return Array.Empty<CommonCostItemList>();
            return list.ToArray();
        }
        public static List<CommonCostItemList> MakeListFromString(string _str)
        {
            List<CommonCostItemList> list = new List<CommonCostItemList>();
            if (_str is not { Length: > 0 })
                return list;

            string[] strArray = _str.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string str in strArray)
            {
                CommonCostItemList newItem = new CommonCostItemList();
                newItem.ParseFromString(str);
                list.Add(newItem);
            }

            return list;
        }
        public void ParseFromString(string _str)
        {
            string[] strArray = _str.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            costItemList = new List<NPCommonCostItem>();
            foreach (string str in strArray)
            {
                NPCommonCostItem item = new NPCommonCostItem();
                item.ParseFromString(str);
                costItemList.Add(item);
            }
        }
    }
}