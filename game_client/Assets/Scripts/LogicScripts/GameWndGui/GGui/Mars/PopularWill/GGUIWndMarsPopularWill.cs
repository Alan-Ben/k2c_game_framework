using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星基地 - 民意 主窗口
    /// </summary>
    public class GGUIWndMarsPopularWill : _ANPGGUIBasicWnd<GGUIMonoMarsPopularWill>
    {
        private static GGUIWndMarsPopularWill _g_instance;
        public static GGUIWndMarsPopularWill instance { get { return _g_instance ??= new GGUIWndMarsPopularWill(); } }
        
        private NPGGUIWndProgress _m_wSatisfactionProgress; // 满意度进度条
        private GGUIWndMarsPopularWillTabList _m_wTabList;   // 页签

        public GGUIWndMarsPopularWill() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoMarsPopularWill.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoMarsPopularWill.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.satisfactionDegreeProgressBar != null)
                _m_wSatisfactionProgress = new NPGGUIWndProgress(wnd.satisfactionDegreeProgressBar);

            if (wnd.tabList != null)
                _m_wTabList = new GGUIWndMarsPopularWillTabList(wnd.tabList);

            if (wnd.btnReturnList != null)
            {
                foreach (var returnBtn in wnd.btnReturnList)
                {
                    ALUGUICommon.combineBtnClick(returnBtn, _onClickReturn);
                }
            }
        }

        protected override void _onDiscard()
        {
            _m_wTabList?.discard();
            _m_wTabList = null;

            _m_wSatisfactionProgress?.discard();
            _m_wSatisfactionProgress = null;

            if (wnd != null)
            {
                if (wnd.btnReturnList != null)
                {
                    foreach (var returnBtn in wnd.btnReturnList)
                    {
                        ALUGUICommon.uncombineBtnClick(returnBtn, _onClickReturn);
                    }
                }
            }
        }

        protected override void _onShowWnd()
        {
            _refreshWnd();

            WinMsg.RegisterMsgAct(WinMsgType.ON_MARS_SATISFACTION_DREGREE_CHG, _onSatisfactionChg);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.ON_MARS_SATISFACTION_DREGREE_CHG, _onSatisfactionChg);

            _m_wSatisfactionProgress?.hideWnd();
            _m_wTabList?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wSatisfactionProgress?.resetWnd();
            _m_wTabList?.resetWnd();
        }

        /// <summary>
        /// 设置选中页签
        /// </summary>
        public void setSelectTab(EMarsPopularWillTabType _tabType)
        {
            if(_m_wTabList == null)
                return;
            
            if (_tabType != EMarsPopularWillTabType.NONE)
            {
                _m_wTabList.setSelectTab(_tabType);
            }
            else
            {
                // 若有未处理的求助，优先选中求助页签
                if(NPPlayer.instance.marsComp.peopleSubComponent.hasUnHandleHelp())
                    _m_wTabList.setSelectTab(EMarsPopularWillTabType.HELP);
                // 若有未处理的信件，优先选中信件页签
                else if (NPPlayer.instance.marsComp.peopleSubComponent.hasUnDealLetter())
                    _m_wTabList.setSelectTab(EMarsPopularWillTabType.LETTER);
                else//否则选中默认页签
                    _m_wTabList.selectDefaultTab();
            }
        }

        private void _refreshWnd()
        {
            _m_wTabList?.showWnd();
            
            _refreshSatisfaction();
        }
        
        private void _refreshSatisfaction()
        {
            if (wnd == null || !isShow)
                return;

            // 满意度为万分比
            int dregreeTenThousand = NPPlayer.instance?.marsComp?.peopleSubComponent?.satisfactionDregree ?? 0;
            // 满意度百分比(不保留小数)
            int dregreePercentage = dregreeTenThousand / 100;
            
            float sliderValue = Mathf.Clamp01(dregreeTenThousand / 10000f);

            if (_m_wSatisfactionProgress != null)
            {
                _m_wSatisfactionProgress.showWnd();
                
                // 进度条数值/文本
                _m_wSatisfactionProgress?.setProgress(sliderValue);
                _m_wSatisfactionProgress?.setProgressTxt(TextTranslate.instance.getLanguage(TransKeyConst.common_percentage_num, dregreePercentage), dregreePercentage >= 100);
            }

            MarsSatisfactionDegreeRefObj satisfactionDegreeRefObj = GRefdataCoreMgr.instance.getMarsSatisfactionDegreeRefByPer(dregreeTenThousand);
            ALUGUICommon.setLabelTxt(wnd.satisfactionDegreeDesc, TextTranslate.instance.getLanguage(satisfactionDegreeRefObj?.desc));
        }
        
        private void _onClickReturn(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_MARS_POPULAR_WILL);
        }

        private void _onSatisfactionChg()
        {
            _refreshSatisfaction();
        }
    }
}
