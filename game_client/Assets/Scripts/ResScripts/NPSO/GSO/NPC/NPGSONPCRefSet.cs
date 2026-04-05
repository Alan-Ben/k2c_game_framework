using ALPackage;
using SQLite4Unity3d;
using System;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// NPC主表
    /// </summary>
    [Serializable]
    public class NPNPCRefObj
    {
        public long id;//唯一id
        public string name;
        public string npc_type;//NPC类型字符串
        public string show_case_offset;//showcase展示下的偏移坐标
        private ENPNPCType _m_npcType;//NPC类型
        public string icon;
        public string npc_image;//npc半身像

        public long Id { get { return id; } set { id = value; } }
        public string Name { get { return name; } set { name = value; } }
        public string Icon { get { return icon; } set { icon = value; } }
        public string NPCImage { get { return npc_image; } set { npc_image = value; } }
        public string NPCType { get { return npc_type; } set { npc_type = value; } }
        public string ShowCaseOffset { get { return show_case_offset; } set { show_case_offset = value; } }

        [Ignore]
        public string npcName
        {
            get
            {
#if NP_GAME
                if (npcType == ENPNPCType.SELF)
                {
                    return NPPlayer.instance.playerInfo.PlayerName;
                }
#endif
                return name;
            }
        }
        

        private NPGTextureIndex _m_icon;
        [Ignore]
        public NPGTextureIndex npcIcon
        {
            get
            {
#if NP_GAME
                if (npcType == ENPNPCType.SELF)
                {
                    return GCommon.getItemTexIcon(ENPItemType.ICON, NPPlayer.instance.playerInfo.getCurrentIconId());
                }
#endif
                
                if (_m_icon == null)
                    _m_icon = NPGTextureIndex.readIndexInfo(icon);
                return _m_icon;
            }
        }

        private NPGTextureIndex _m_npcImage;
        [Ignore]
        public NPGTextureIndex npcImage
        {
            get
            {
                if (_m_npcImage == null)
                    _m_npcImage = NPGTextureIndex.readIndexInfo(npc_image);
                return _m_npcImage;
            }
        }

        [Ignore]
        public ENPNPCType npcType //NPC类型
        {
            get
            {
                if (_m_npcType == ENPNPCType.NONE)
                {
                    _m_npcType = (ENPNPCType)ALCommon.EnumParse(typeof(ENPNPCType), npc_type);
                }
                return _m_npcType;
            }
        }

        [Ignore]
        public Vector3 showCaseOffset //偏移量
        {
            get
            {
                return NPResUtil.GetVector3(show_case_offset);
            }
        }
        
        public static string assetPath { get { return "refdata_db/npc.unity3d"; } }
        public static string objName { get { return "refdata_db/npc.txt"; } }
        public static string tableName { get { return "npc"; } }
    }
}