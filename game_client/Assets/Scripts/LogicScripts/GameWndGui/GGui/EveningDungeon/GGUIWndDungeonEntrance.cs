using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIWndDungeonEntrance : _ANPGGUIBasicResBarWnd<GGUIMonoDungeonEntrance>
    {
        private static GGUIWndDungeonEntrance _g_instance;
        public static GGUIWndDungeonEntrance instance { get { return _g_instance ??= new GGUIWndDungeonEntrance(); } }
        
        private GGUISubWndEveningDungeonEntrance _m_wEveningDungeonEntrance;//晚间活动入口子窗口
        private GGUISubWndMiddayDungeonEnter _m_wMiddayDungeonEnter;//午间活动入口子窗口
        
        public GGUIWndDungeonEntrance() : base(EALUIWndLayer.NORMAL)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoDungeonEntrance.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoDungeonEntrance.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.monoMiddayDungeonEnter != null)
                _m_wMiddayDungeonEnter = new GGUISubWndMiddayDungeonEnter(wnd.monoMiddayDungeonEnter);

            if (wnd.monoEveningDungeonEntrance != null)
                _m_wEveningDungeonEntrance = new GGUISubWndEveningDungeonEntrance(wnd.monoEveningDungeonEntrance);
            
            ALUGUICommon.combineBtnClick(wnd.btnReturn, _onReturnBtnClick);
            ALUGUICommon.combineBtnClick(wnd.btnRank, _onRankBtnClick);
        }
        
        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnReturn, _onReturnBtnClick);
                ALUGUICommon.uncombineBtnClick(wnd.btnRank, _onRankBtnClick);
            }
            
            _m_wMiddayDungeonEnter?.discard();
            _m_wMiddayDungeonEnter = null;
            
            _m_wEveningDungeonEntrance?.discard();
            _m_wEveningDungeonEntrance = null;
        }
        
        protected override void _onShowWnd()
        {
            _m_wMiddayDungeonEnter?.showWnd();
            _m_wEveningDungeonEntrance?.showWnd();
        }

        protected override void _onHideWnd()
        {
            _m_wMiddayDungeonEnter?.hideWnd();
            _m_wEveningDungeonEntrance?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wMiddayDungeonEnter?.resetWnd();
            _m_wEveningDungeonEntrance?.resetWnd();
        }

        /// <summary>
        /// 排行榜按钮被点击
        /// </summary>
        /// <param name="_go"></param>
        private void _onRankBtnClick(GameObject _go)
        {
            QueueMgr.instance.AddNode(new GNodeDungeonRank(EDungeonRankTab.NONE, EEveningDungeonRankAndRewardDetailTabType.RANK));
        }

        /// <summary>
        /// 返回按钮被点击
        /// </summary>
        /// <param name="_go"></param>
        private void _onReturnBtnClick(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_DUNGEON_ENTRANCE);
        }
    }
}