using System;
using System.Collections.Generic;
using ALPackage;
using Common.GuildDungeonEnum;
using NPEnum;

namespace GOE
{
    [Serializable]
    public class GuildDungeonMonsterHP
    {
        public EGuildDungeon_MonsterType monsterType; // 怪物类型
        public long monsterHp; // 怪物血量


        #region 自动导出

         /************
        * 读取字符串
        **/
        public static GuildDungeonMonsterHP readFromStr(string _str)
        {
            if (string.IsNullOrEmpty(_str))
            {
                Debug.LogError("GuildDungeonMonsterHP readFromStr _str is null");
                return null;
            }
            
            //拆分字符串后进行读取
            string[] strs = _str.Split(new string[] { ":","-" }, StringSplitOptions.RemoveEmptyEntries);
            if (strs.Length < 2)
            {
                Debug.LogError($"GuildDungeonMonsterHP readFromStr strs.Length < 2");
                return null;
            }

            GuildDungeonMonsterHP ret = new GuildDungeonMonsterHP();

            ret.monsterType = (EGuildDungeon_MonsterType)ALCommon.EnumParse(typeof(EGuildDungeon_MonsterType), strs[0]);
            ret.monsterHp = ALCommon.ParseLong(strs[1]);

            return ret;
        }

        /************
         * 读取队列
         **/
        public static List<GuildDungeonMonsterHP> readList(string _str)
        {
            List<GuildDungeonMonsterHP> list = new List<GuildDungeonMonsterHP>();
            if (string.IsNullOrEmpty(_str))
                return list;

            string[] strs = _str.Split(new string[] { "|", ";" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < strs.Length; i++)
            {
                GuildDungeonMonsterHP newSkillInfo = GuildDungeonMonsterHP.readFromStr(strs[i]);
                if(null == newSkillInfo)
                    continue;

                list.Add(newSkillInfo);
            }
            return list;
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}", monsterType, monsterHp);
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
                Debug.LogError($"GuildDungeonMonsterHP ParseFromString strs.Length < 2");
                return;
            }
            monsterType = (EGuildDungeon_MonsterType)ALCommon.EnumParse(typeof(EGuildDungeon_MonsterType), strs[0]);
            monsterHp = ALCommon.ParseLong(strs[1]);
        }

        /// <summary>
        /// 为自动导出写的
        /// </summary>
        public static List<GuildDungeonMonsterHP> MakeListFromString(string _str)
        {
            return readList(_str);
        }


        #endregion
       
    }
}