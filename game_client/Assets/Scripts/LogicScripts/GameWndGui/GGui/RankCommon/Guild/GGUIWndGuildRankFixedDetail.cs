using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 常驻联盟排行榜界面（NORMAL层级）
    /// </summary>
    public class GGUIWndGuildRankFixedDetail : _AGGUIWndGuildRankFixedDetailBase<GGUIMonoGuildRankFixedDetail>
    {
        private static GGUIWndGuildRankFixedDetail _g_instance;
        public static GGUIWndGuildRankFixedDetail instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new GGUIWndGuildRankFixedDetail();
                return _g_instance;
            }
        }
        
        protected GGUIWndGuildRankFixedDetail() : base(EALUIWndLayer.NORMAL) { }

        protected override void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Rank.C_MAIN_GUILD_RANK_FIXED_NODE);
        }
    }
}
