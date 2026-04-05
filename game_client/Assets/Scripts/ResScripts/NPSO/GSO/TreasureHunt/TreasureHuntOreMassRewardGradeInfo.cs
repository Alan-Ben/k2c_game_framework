using System;
using System.Collections.Generic;
using ALPackage;
using NPEnum;

namespace GOE
{
    [System.Serializable]
    public class TreasureHuntOreMassRewardGradeInfo
    {
        public int mass;//质量
        public NPCommonCostItem rewardItem;

        public TreasureHuntOreMassRewardGradeInfo()
        {
            
        }
        
        public TreasureHuntOreMassRewardGradeInfo(int _mass, NPCommonCostItem _rewardItem)
        {
            this.mass = _mass;
            this.rewardItem = _rewardItem;
        }
        
        /************
        * 读取字符串
        **/
        public static TreasureHuntOreMassRewardGradeInfo readFromStr(string _str)
        {
            if (string.IsNullOrEmpty(_str))
            {
                UnityEngine.Debug.LogError("[TreasureHuntOreMassRewardGradeInfo readFromStr] 输入字符串为空");
                return null;
            }
            
            //拆分字符串后进行读取
            string[] strs = _str.Split('|');
            if (strs == null || strs.Length < 1)
            {
                UnityEngine.Debug.LogError($"[TreasureHuntOreMassRewardGradeInfo readFromStr] {_str}配置错误");
                return null;
            }

            if (string.IsNullOrEmpty(strs[0]) || !int.TryParse(strs[0], out int _mass))
            {
                UnityEngine.Debug.LogError($"[TreasureHuntOreMassRewardGradeInfo readFromStr] {_str}配置错误, 第一个字段(质量)配置错误, 应该要配置一个整型值");
                return null;
            }
            
            TreasureHuntOreMassRewardGradeInfo ret = new TreasureHuntOreMassRewardGradeInfo();
            ret.mass = _mass;

            // 阶段的奖励可以没有
            if (strs.Length > 1 && !string.IsNullOrEmpty(strs[1]))
            {
                ret.rewardItem = NPCommonCostItem.readFromStr(strs[1]);
            }
            
            return ret;
        }

        /************
         * 读取队列
         **/
        public static List<TreasureHuntOreMassRewardGradeInfo> readList(string _str)
        {
            List<TreasureHuntOreMassRewardGradeInfo> list = new List<TreasureHuntOreMassRewardGradeInfo>();
            if (null == _str || string.IsNullOrEmpty(_str))
                return list;

            string[] strs = _str.Split(";", StringSplitOptions.RemoveEmptyEntries);
            if (strs != null)
            {
                for (int i = 0; i < strs.Length; i++)
                {
                    TreasureHuntOreMassRewardGradeInfo newInfo = TreasureHuntOreMassRewardGradeInfo.readFromStr(strs[i]);
                    if(null == newInfo)
                        continue;

                    list.Add(newInfo);
                }
            }
            return list;
        }

        public override string ToString()
        {
            return $"{mass}|{rewardItem}";
        }
        
        /// <summary>
        /// 为自动导出写的
        /// </summary>
        public void ParseFromString(string _str)
        {
            //拆分字符串后进行读取
            if (string.IsNullOrEmpty(_str))
            {
                UnityEngine.Debug.LogError("[TreasureHuntOreMassRewardGradeInfo readFromStr] 输入字符串为空");
                return;
            }
            
            //拆分字符串后进行读取
            string[] strs = _str.Split('|');
            if (strs == null || strs.Length < 2)
            {
                UnityEngine.Debug.LogError($"[TreasureHuntOreMassRewardGradeInfo readFromStr] {_str}配置错误, 无法解析出两个字段");
                return;
            }

            if (string.IsNullOrEmpty(strs[0]) || !int.TryParse(strs[0], out mass))
            {
                UnityEngine.Debug.LogError($"[TreasureHuntOreMassRewardGradeInfo readFromStr] {_str}配置错误, 第一个字段(质量)配置错误, 应该要配置一个整型值");
            }

            if (!string.IsNullOrEmpty(strs[1]))
            {
                rewardItem = NPCommonCostItem.readFromStr(strs[1]);
            }
            else
            {
                rewardItem = null;
            }
        }

        /// <summary>
        /// 为自动导出写的
        /// </summary>
        public static List<TreasureHuntOreMassRewardGradeInfo> MakeListFromString(string _str)
        {
            return readList(_str);
        }

        /// <summary>
        /// 为自动导出写的
        /// </summary>
        public static TreasureHuntOreMassRewardGradeInfo[] MakeArrayFromString(string _str)
        {
            return readList(_str).ToArray();
        }
    }
}