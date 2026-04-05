using System;
using System.Collections.Generic;
using NPEnum;

namespace GOE
{
    [Serializable]
    public class CommonQualityWeight
    {
        public EQuality quality;
        public int weight;
        

        public static CommonQualityWeight[] MakeArrayFromString(string _str)
        {
            List<CommonQualityWeight> list = MakeListFromString(_str);
            if (_str is not { Length: > 0 })
                return Array.Empty<CommonQualityWeight>();
            return list.ToArray();
        }
        public static List<CommonQualityWeight> MakeListFromString(string _str)
        {
            List<CommonQualityWeight> list = new List<CommonQualityWeight>();
            if (_str is not { Length: > 0 })
                return list;

            string[] strArray = _str.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string str in strArray)
            {
                CommonQualityWeight newItem = new CommonQualityWeight();
                newItem.ParseFromString(str);
                list.Add(newItem);
            }

            return list;
        }
        public void ParseFromString(string _str)
        {
            string[] strArray = _str.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
            if (strArray.Length >= 2)
            {
                quality = (EQuality)Enum.Parse(typeof(EQuality), strArray[0]);
                weight = int.Parse(strArray[1]);
            }
        }
    }
}