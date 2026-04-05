using System.Collections.Generic;
using ALPackage;
using GOE;
using UnityEngine;

namespace Hotfix
{
    /// <summary>
    /// 三消阶段奖励概率窗口
    /// </summary>
    public class GGUIWndTileMatchStepRewardProbability : _AHotfixBaseWnd<GGUIMonoTileMatchStepRewardProbability>
    {
        private static GGUIWndTileMatchStepRewardProbability _g_instance;
        public static GGUIWndTileMatchStepRewardProbability instance { get { return _g_instance ??= new GGUIWndTileMatchStepRewardProbability(); } }

        private List<TileMatchJackpotGroupRefObj> _m_lJackpotGroupList; // 奖池组列表
        private int _m_iTotalWeight; // 奖池组总权重

        private GGUIWndTileMatchStepRewardJackpotGroupItemContainer _m_wRewardContainer;
        
        public GGUIWndTileMatchStepRewardProbability() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return UIResPathAssistant.getAssetPath(6102); } }
        protected override string _monoObjName { get { return UIResPathAssistant.getObjName(6102); } }
        
        protected override void _onWndInitDoneHotfix()
        {
            if(hotfixWnd == null)
                return;

            if (hotfixWnd.monoJackpotGroupContainer != null)
                _m_wRewardContainer = new GGUIWndTileMatchStepRewardJackpotGroupItemContainer(hotfixWnd.monoJackpotGroupContainer);
            
            ALUGUICommon.combineBtnClick(hotfixWnd.btnClose, _onCloseBtnClick);
        }
        
        protected override void _onDiscard()
        {
            if (hotfixWnd != null)
            {
                ALUGUICommon.uncombineBtnClick(hotfixWnd.btnClose, _onCloseBtnClick);
            }
            
            _m_lJackpotGroupList?.Clear();
            _m_lJackpotGroupList = null;
            
            _m_wRewardContainer?.discard();
            _m_wRewardContainer = null;
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_lJackpotGroupList?.Clear();
            
            _m_wRewardContainer?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_lJackpotGroupList?.Clear();
            
            _m_wRewardContainer?.resetWnd();
        }
        
        public void setData(long _jackpotGroupId)
        {
            if (_m_lJackpotGroupList == null)
                _m_lJackpotGroupList = new List<TileMatchJackpotGroupRefObj>();
            _m_lJackpotGroupList.Clear();
            HotfixRefdataCoreMgr.instance.getTileMatchStepRewardJackpotList(_jackpotGroupId, _m_lJackpotGroupList);
            _m_iTotalWeight = 0;
            TileMatchJackpotGroupRefObj groupRefObj = null;
            for (int i = 0, count = _m_lJackpotGroupList.Count; i < count; i++)
            {
                groupRefObj = _m_lJackpotGroupList[i];
                if (groupRefObj != null)
                    _m_iTotalWeight += groupRefObj.weight_total;
            }
            
            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if (_m_wRewardContainer != null)
            {
                _m_wRewardContainer.showWnd();
                _m_wRewardContainer.setData(_m_lJackpotGroupList, _m_iTotalWeight);
            }
        }

        /// <summary>
        /// 关闭按钮被点击
        /// </summary>
        /// <param name="_go"></param>
        private void _onCloseBtnClick(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(HotfixUINodeTagConst.TILEMATCH_STAGE_REWARD_PREVIEW);
        }
    }
}