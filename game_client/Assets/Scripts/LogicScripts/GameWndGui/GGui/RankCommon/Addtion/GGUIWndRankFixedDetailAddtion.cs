using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 常驻排行榜界面（ADDITION层级）
    /// </summary>
    public class GGUIWndRankFixedDetailAddtion : _AGGUIWndRankFixedDetailBase<GGUIMonoRankFixedDetail>
    {
        private static GGUIWndRankFixedDetailAddtion _g_instance;
        public static GGUIWndRankFixedDetailAddtion instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new GGUIWndRankFixedDetailAddtion();
                return _g_instance;
            }
        }

        protected GGUIWndRankFixedDetailAddtion() : base(EALUIWndLayer.ADDITION) { }

        protected override void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Rank.C_MAIN_RANK_FIXED_ADDTION_NODE);
        }
    }
}
