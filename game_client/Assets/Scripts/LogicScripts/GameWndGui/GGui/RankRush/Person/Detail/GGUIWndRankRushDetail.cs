
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 冲榜详情主界面
    /// </summary>
    public class GGUIWndRankRushDetail : _AGGUIWndRankRushDetailBase<GGUIMonoRankRushDetail>
    {
        private static GGUIWndRankRushDetail _g_instance;
        public static GGUIWndRankRushDetail instance
        {
            get
            {
                if(_g_instance == null)
                    _g_instance = new GGUIWndRankRushDetail();
                return _g_instance;
            }
        }
        protected override string _monoAssetPath { get { return GGUIMonoRankRushDetail.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoRankRushDetail.objName; } }

        protected override void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_RANK_RUSH_DETAIL);
        }
    }
}
