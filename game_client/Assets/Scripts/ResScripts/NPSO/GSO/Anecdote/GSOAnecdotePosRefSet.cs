
using ALPackage;
using System;

namespace GOE
{
    /// <summary>
    /// 经营事件位置表
    /// </summary>
    [Serializable]
    public class AnecdotePosRefObj : _IALBasicRefObj
    {
        public long _refId { get { return id; } }
        public long id; // 主键 id
        public long guide_hand_ui_res_id; // 引导手指资源 id
    }
    public class GSOAnecdotePosRefSet : _TALSOBasicRefSet<AnecdotePosRefObj>
    {
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return "refdata/anecdote_refdata.unity3d"; } }
        public static string objName { get { return "anecdote_pos"; } }
    }
}