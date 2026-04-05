using ALPackage;
using System.Collections.Generic;

namespace GOE
{
    public static partial class GCommon
    {
        //根据owen淫威潜规则PER
        public static bool IsPer(string _value)
        {
            return _value.Contains("_PER");
        }

        /// <summary>
        /// 将字符串列表转换为WinMsgType列表
        /// </summary>
        /// <param name="_emunList"></param>
        /// <returns></returns>
        public static List<WinMsgType> tryEnumParseToWinMsgTypeList(List<string> _emunList)
        {
            List<WinMsgType> winMsgTypeList = new List<WinMsgType>();
            if (_emunList != null)
            {
                WinMsgType temp = 0;
                for (int i = 0; i < _emunList.Count; i++)
                {
                    bool isParse = ALCommon.TryEnumParse(typeof(WinMsgType), _emunList[i], out temp);
                    if (!isParse || 0 == temp)
                        continue;

                    winMsgTypeList.Add(temp);
                }
            }
            return winMsgTypeList;
        }
    }
}