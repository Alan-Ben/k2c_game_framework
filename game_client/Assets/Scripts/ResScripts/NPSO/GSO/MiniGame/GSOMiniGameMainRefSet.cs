using System;
using ALPackage;
using SQLite4Unity3d;

namespace GOE
{
    /// <summary>
    /// 小游戏主表
    /// </summary>
    [Serializable]
    public class MiniGameMainRefObj
    {
        public long id;//唯一id
        public long Id { get { return id; } set { id = value; } }
        
        public string game_type;//游戏类型
        public string gameType { get { return game_type; } set { game_type = value; } }
        private EMiniGameType _m_eGameType;
        [Ignore]
        public EMiniGameType eGameType 
        {
            get
            {
                if (_m_eGameType == EMiniGameType.NONE)
                {
                    ALCommon.TryEnumParse(typeof(EMiniGameType), gameType, out _m_eGameType);
                }

                return _m_eGameType;
            }
        }
        
        public long game_sub_id;//游戏子id
        public long gameSubId { get { return game_sub_id; } set { game_sub_id = value; } }
        
        public bool can_skip;//是否可以跳过
        public bool canSkip { get { return can_skip; } set { can_skip = value; } }

        public long scene_id;//场景id
        public long sceneId { get { return scene_id; } set { scene_id = value; } }
        
        public static string assetPath { get { return "refdata_db/mini_game.unity3d"; } }
        public static string objName { get { return "refdata_db/mini_game_main.txt"; } }
        public static string tableName { get { return "mini_game_main"; } }
    }
}