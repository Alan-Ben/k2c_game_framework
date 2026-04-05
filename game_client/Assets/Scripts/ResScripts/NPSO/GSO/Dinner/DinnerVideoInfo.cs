using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine.Serialization;

namespace GOE
{
    [Serializable]
    public class DinnerVideoInfo
    {
        public int peopleCount; // 宴会人数
        public GVideoClipIndex videoClipIndex; // 视频ui路径ID


        public static int sort(DinnerVideoInfo a, DinnerVideoInfo b)
        {
            return a.peopleCount.CompareTo(b.peopleCount);
        }
        #region 自动导出

         /************
        * 读取字符串
        **/
        public static DinnerVideoInfo readFromStr(string _str)
        {
            if (string.IsNullOrEmpty(_str))
            {
                Debug.LogError("DinnerVideoInfo readFromStr _str is null");
                return null;
            }
            
            //拆分字符串后进行读取
            string[] strs = _str.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
            if (strs.Length < 2)
            {
                Debug.LogError($"DinnerVideoInfo readFromStr strs.Length < 2");
                return null;
            }

            DinnerVideoInfo ret = new DinnerVideoInfo();

            ret.peopleCount = ALCommon.ParseInt(strs[0]);
            ret.videoClipIndex = GVideoClipIndex.readFromStr(strs[1]);

            return ret;
        }

        /************
         * 读取队列
         **/
        public static List<DinnerVideoInfo> readList(string _str)
        {
            List<DinnerVideoInfo> list = new List<DinnerVideoInfo>();
            if (string.IsNullOrEmpty(_str))
                return list;

            string[] strs = _str.Split(new string[] { "|", ";" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < strs.Length; i++)
            {
                DinnerVideoInfo newSkillInfo = DinnerVideoInfo.readFromStr(strs[i]);
                if(null == newSkillInfo)
                    continue;

                list.Add(newSkillInfo);
            }
            return list;
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}", peopleCount, videoClipIndex);
        }
        
        /// <summary>
        /// 为自动导出写的
        /// </summary>
        public void ParseFromString(string _str)
        {
            if(string.IsNullOrEmpty(_str))
                return;
            
            //拆分字符串后进行读取
            string[] strs = _str.Split(new string[] { ":","-" }, StringSplitOptions.RemoveEmptyEntries);

            if (strs.Length < 2)
            {
                Debug.LogError($"DinnerVideoInfo ParseFromString strs.Length < 2");
                return;
            }
            peopleCount = ALCommon.ParseInt(strs[0]);
            videoClipIndex = GVideoClipIndex.readFromStr(strs[1]);
        }

        /// <summary>
        /// 为自动导出写的
        /// </summary>
        public static List<DinnerVideoInfo> MakeListFromString(string _str)
        {
            return readList(_str);
        }


        #endregion
       
    }
}