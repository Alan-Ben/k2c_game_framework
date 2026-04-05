using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 常驻排行榜界面（NORMAL层级）
    /// </summary>
    public class GGUIWndRankFixedDetail : _AGGUIWndRankFixedDetailBase<GGUIMonoRankFixedDetail>
    {
        private static GGUIWndRankFixedDetail _g_instance;
        public static GGUIWndRankFixedDetail instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new GGUIWndRankFixedDetail();
                return _g_instance;
            }
        }
        
        protected GGUIWndRankFixedDetail() : base(EALUIWndLayer.NORMAL) { }

        protected override void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Rank.C_MAIN_RANK_FIXED_NODE);
        }
    }
}
