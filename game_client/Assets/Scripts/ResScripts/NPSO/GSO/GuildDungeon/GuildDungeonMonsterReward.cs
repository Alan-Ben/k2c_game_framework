using System;
using System.Collections.Generic;
using ALPackage;
using Common.GuildDungeonEnum;
using NPEnum;
using UnityEngine.Serialization;

namespace GOE
{
    [Serializable]
    public class GuildDungeonMonsterReward
    {
        public EGuildDungeon_MonsterType monsterType; // 怪物类型
        public int count; // 数量
        public NPCommonCostItem reward; // 怪物血量


        #region 自动导出

         /************
        * 读取字符串
        **/
        public static GuildDungeonMonsterReward readFromStr(string _str)
        {
            if (string.IsNullOrEmpty(_str))
            {
                Debug.LogError("GuildDungeonMonsterReward readFromStr _str is null");
                return null;
            }
            
            //拆分字符串后进行读取
            string[] strs = _str.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
            if (strs.Length < 3)
            {
                Debug.LogError($"GuildDungeonMonsterReward readFromStr strs.Length < 2");
                return null;
            }

            GuildDungeonMonsterReward ret = new GuildDungeonMonsterReward();
            ret.monsterType = (EGuildDungeon_MonsterType)ALCommon.EnumParse(typeof(EGuildDungeon_MonsterType), strs[0]);
            ret.count = int.Parse(strs[1]);
            ret.reward = NPCommonCostItem.readFromStr(strs[2]);

            return ret;
        }

        /************
         * 读取队列
         **/
        public static List<GuildDungeonMonsterReward> readList(string _str)
        {
            List<GuildDungeonMonsterReward> list = new List<GuildDungeonMonsterReward>();
            if (string.IsNullOrEmpty(_str))
                return list;

            string[] strs = _str.Split(new string[] {  ";" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < strs.Length; i++)
            {
                GuildDungeonMonsterReward newSkillInfo = GuildDungeonMonsterReward.readFromStr(strs[i]);
                if(null == newSkillInfo)
                    continue;

                list.Add(newSkillInfo);
            }
            return list;
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}", monsterType, reward);
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

            if (strs.Length < 3)
            {
                Debug.LogError($"GuildDungeonMonsterReward ParseFromString strs.Length < 2");
                return;
            }
            monsterType = (EGuildDungeon_MonsterType)ALCommon.EnumParse(typeof(EGuildDungeon_MonsterType), strs[0]);
            count = int.Parse(strs[1]);
            reward = NPCommonCostItem.readFromStr(strs[2]);
        }

        /// <summary>
        /// 为自动导出写的
        /// </summary>
        public static List<GuildDungeonMonsterReward> MakeListFromString(string _str)
        {
            return readList(_str);
        }


        #endregion
       
    }
}