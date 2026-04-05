using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 招募妃子窗口
    /// </summary>
    public class GGUIWndRecruitConsort : _ANPGGUIBasicWnd<GGUIMonoRecruitConsort>
    {
        private static GGUIWndRecruitConsort _g_instance;
        public static GGUIWndRecruitConsort instance { get { return _g_instance ??= new GGUIWndRecruitConsort(); } }
        
        private RecruitShopInfo _m_rRecruitShopInfo;
        private RecruitConsortItemInfo _m_iRecruitConsortItemInfo;
        
        private GGUISubWndConsortDetailInfo _m_wndConsortDetailInfo;
        private GGUISubWndRecruitExchangeBtn _m_wndRecruitExchangeBtn;
        
        public GGUIWndRecruitConsort() : base(EALUIWndLayer.NORMAL)
        {
        }
        
        protected override string _monoAssetPath { get { return GGUIMonoRecruitConsort.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoRecruitConsort.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsgAct(WinMsgType.ON_RECRUIT_EXCHANGE_SUCC, _onRecruitExchangeSucc);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.ON_RECRUIT_EXCHANGE_SUCC, _onRecruitExchangeSucc);

            _m_wndConsortDetailInfo?.hideWnd();
            
            _m_wndRecruitExchangeBtn?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wndConsortDetailInfo?.resetWnd();
            
            _m_wndRecruitExchangeBtn?.resetWnd();
        }

        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnMoreInfo, _onMoreInfoBtnClick);
                ALUGUICommon.uncombineBtnClick(wnd.btnReturn, _onReturnBtnClick);
            }
            
            _m_wndConsortDetailInfo?.discard();
            _m_wndConsortDetailInfo = null;
            
            _m_wndRecruitExchangeBtn?.discard();
            _m_wndRecruitExchangeBtn = null;
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.monoConsortDetailInfo != null)
                _m_wndConsortDetailInfo = new GGUISubWndConsortDetailInfo(wnd.monoConsortDetailInfo);

            if (wnd.monoExchangeBtn != null)
                _m_wndRecruitExchangeBtn = new GGUISubWndRecruitExchangeBtn(wnd.monoExchangeBtn);
            
            ALUGUICommon.combineBtnClick(wnd.btnMoreInfo, _onMoreInfoBtnClick);
            ALUGUICommon.combineBtnClick(wnd.btnReturn, _onReturnBtnClick);
        }
        
        public void setData(RecruitShopInfo _recruitShopInfo, RecruitConsortItemInfo _recruitConsortItemInfo)
        {
            _m_rRecruitShopInfo = _recruitShopInfo;
            _m_iRecruitConsortItemInfo = _recruitConsortItemInfo;

            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if(wnd == null || _m_iRecruitConsortItemInfo == null)
                return;

            if (_m_wndConsortDetailInfo != null)
            {
                _m_wndConsortDetailInfo.showWnd();
                _m_wndConsortDetailInfo.setData(_m_iRecruitConsortItemInfo.consortInfo);
            }

            if (_m_wndRecruitExchangeBtn != null)
            {
                _m_wndRecruitExchangeBtn.showWnd();
                _m_wndRecruitExchangeBtn.setData(_m_rRecruitShopInfo, _m_iRecruitConsortItemInfo);
            }
        }
        
        /// <summary>
        /// 兑换成功消息
        /// </summary>
        private void _onRecruitExchangeSucc()
        {
            // 刷新窗口
            _refreshWnd();
        }
        
        /// <summary>
        /// 更多信息按钮点击
        /// </summary>
        private void _onMoreInfoBtnClick(GameObject _go)
        {
            if(_m_iRecruitConsortItemInfo == null || _m_iRecruitConsortItemInfo.consortInfo == null)
                return;
            
            QueueMgr.instance.AddNode(new GNodeLockConsortDetail(_m_iRecruitConsortItemInfo.consortInfo));
        }
        
        /// <summary>
        /// 返回按钮点击
        /// </summary>
        private void _onReturnBtnClick(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_RECRUIT_CONSORT);
        }
    }
}