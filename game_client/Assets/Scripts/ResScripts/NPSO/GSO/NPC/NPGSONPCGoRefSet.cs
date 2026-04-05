using SQLite4Unity3d;
using System;

namespace GOE
{
    /// <summary>
    /// NPC-独立模型子表
    /// </summary>
    [Serializable]
    public class NPNPCGoRefObj
    {
        public long id;//唯一id
        public string go_index;//资源下标字符串
        private NPGGoIndex _m_goIndex;//资源下标

        public long Id { get { return id; } set { id = value; } }
        public string GoIndex { get { return go_index; } set { go_index = value; } }

        [Ignore]
        public NPGGoIndex goIndex //NPC类型
        {
            get
            {
                if (_m_goIndex == null)
                {
                    _m_goIndex = new NPGGoIndex();
                    _m_goIndex.readIndex(go_index);
                }
                return _m_goIndex;
            }
        }

        public static string assetPath { get { return "refdata_db/npc.unity3d"; } }
        public static string objName { get { return "refdata_db/npc_go.txt"; } }
        public static string tableName { get { return "npc_go"; } }
    }
}