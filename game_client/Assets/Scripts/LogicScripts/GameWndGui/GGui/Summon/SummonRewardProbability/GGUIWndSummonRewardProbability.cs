using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIWndSummonRewardProbability : _ANPGGUIBasicWnd<GGUIMonoSummonRewardProbability>
    {
        private static GGUIWndSummonRewardProbability _g_instance;
        public static GGUIWndSummonRewardProbability instance { get { return _g_instance ??= new GGUIWndSummonRewardProbability(); } }

        private GachaPoolRefObj _m_rGachaPoolRefObj;
        
        private GGUIWndSummonRewardProbabilityGroupContainer _m_wndGroupContainer;
        
        public GGUIWndSummonRewardProbability() : base(EALUIWndLayer.ADDITION)
        {
        }
        
        protected override string _monoAssetPath { get { return GGUIMonoSummonRewardProbability.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoSummonRewardProbability.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if(wnd.monoSummonRewardProbabilityGroupContainer != null)
                _m_wndGroupContainer = new GGUIWndSummonRewardProbabilityGroupContainer(wnd.monoSummonRewardProbabilityGroupContainer);
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onCloseBtnClick);
        }
        
        protected override void _onDiscard()
        {
            if(wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onCloseBtnClick);
            }
            
            _m_wndGroupContainer?.discard();
            _m_wndGroupContainer = null;
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wndGroupContainer?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wndGroupContainer?.resetWnd();
        }

        public void setData(GachaPoolRefObj _gachaPoolRefObj)
        {
            _m_rGachaPoolRefObj = _gachaPoolRefObj;
            
            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if(wnd == null || _m_rGachaPoolRefObj == null)
                return;

            if (_m_wndGroupContainer != null)
            {
                _m_wndGroupContainer.showWnd();
                _m_wndGroupContainer.setData(_m_rGachaPoolRefObj.showItemGroupList);
            }

            if (!string.IsNullOrEmpty(wnd.txtCanDrawRewardNeedSummonCountKey))
            {
                ALUGUICommon.setLabelTxt(wnd.txtCanDrawRewardNeedSummonCount,
                    TextTranslate.instance.getLanguage(wnd.txtCanDrawRewardNeedSummonCountKey,
                        _m_rGachaPoolRefObj.roll_cost?.getItemName(),
                        _m_rGachaPoolRefObj.cumulative_reward_need_num,
                        _m_rGachaPoolRefObj.cumulative_reward_item?.getItemName()));
            }
        }

        /// <summary>
        /// 关闭按钮被点击
        /// </summary>
        /// <param name="_go"></param>
        private void _onCloseBtnClick(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_SUMMON_REWARD_PROBABILITY);
        }
    }
}