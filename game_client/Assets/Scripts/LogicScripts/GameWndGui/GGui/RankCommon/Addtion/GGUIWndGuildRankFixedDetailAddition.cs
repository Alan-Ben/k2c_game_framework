using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 常驻联盟排行榜界面（ADDITION层级）
    /// </summary>
    public class GGUIWndGuildRankFixedDetailAddition : _AGGUIWndGuildRankFixedDetailBase<GGUIMonoGuildRankFixedDetail>
    {
        private static GGUIWndGuildRankFixedDetailAddition _g_instance;
        public static GGUIWndGuildRankFixedDetailAddition instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new GGUIWndGuildRankFixedDetailAddition();
                return _g_instance;
            }
        }
        
        protected GGUIWndGuildRankFixedDetailAddition() : base(EALUIWndLayer.ADDITION) { }

        protected override void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Rank.C_MAIN_GUILD_RANK_FIXED_ADDTION_NODE);
        }
    }
}
