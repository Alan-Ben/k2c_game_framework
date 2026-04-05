using System;
using System.Collections.Generic;
using ALPackage;
using CommonEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 杰出者庆祝
    /// </summary>
    public class GGUIWndGraveCongrats : _ATALBasicUIWnd<GGUIMonoGraveCongrats>
    {
        private static GGUIWndGraveCongrats _g_instance = new GGUIWndGraveCongrats();
    
        public static GGUIWndGraveCongrats instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndGraveCongrats();
                return _g_instance;
            }
        }
        
        // <AutoGen:WndDeclaration>
        private NPGGUIWndCommonShowCase _m_playerInfoWnd;  // 玩家形象
        
        private NPGGUIWndCommonItem _m_gemCostItemWnd;  // 钻石奖励
        private GGUIWndTextTypewriter _m_txtTypewriterWnd;  // 奖励打字机文本
        private NPGGUIWndPlayerIcon _m_playerIconWnd;
        // </AutoGen:WndDeclaration>
        private Action _m_setDealerDone;
        private NPCommonSimplePlayerInfo _m_playerInfo;  // 玩家信息
        private bool _m_isNewProminent;  // 是否是新晋杰出者祝福
        private List<NPCommonCostItem> _m_rewardList;  // 奖励列表

        public GGUIWndGraveCongrats() : base(EALUIWndLayer.ADDITION)
        {
        }
    
        protected override string _monoAssetPath { get => GGUIMonoGraveCongrats.assetPath; }
        protected override string _monoObjName { get => GGUIMonoGraveCongrats.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }
    
        protected override void _onShowWnd()
        {
            _refreshWnd();
            // <AutoGen:_onShowWnd>
            _m_playerInfoWnd?.showWnd();
            // </AutoGen:_onShowWnd>
        }
    
        protected override void _onHideWnd()
        {
            // <AutoGen:_onHideWnd>
            _m_playerInfoWnd?.hideWnd();
            
            _m_gemCostItemWnd?.hideWnd();
            _m_txtTypewriterWnd?.hideWnd();
            // </AutoGen:_onHideWnd>
        }
    
        protected override void _onReset()
        {
            // <AutoGen:_onReset>
            _m_playerInfoWnd?.resetWnd();
            
            _m_gemCostItemWnd?.resetWnd();
            _m_txtTypewriterWnd?.resetWnd();
            // </AutoGen:_onReset>
        }
    
        protected override void _onDiscard()
        {
            // <AutoGen:_onDiscard>
            _m_playerInfoWnd?.discard();
            _m_playerInfoWnd = null;
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickbtnClose);
            _m_gemCostItemWnd?.discard();
            _m_gemCostItemWnd = null;
            _m_txtTypewriterWnd?.discard();
            _m_txtTypewriterWnd = null;
            _m_playerIconWnd?.discard();
            _m_playerIconWnd = null;
            // </AutoGen:_onDiscard>
        }
    
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            // <AutoGen:_onWndInitDone>
            if (wnd.playerInfo != null)
                _m_playerInfoWnd = new NPGGUIWndCommonShowCase(wnd.playerInfo);
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickbtnClose);
            if (wnd.gemCostItem != null)
                _m_gemCostItemWnd = new NPGGUIWndCommonItem(wnd.gemCostItem);
            if (wnd.txtTypewriter != null)
                _m_txtTypewriterWnd = new GGUIWndTextTypewriter(wnd.txtTypewriter);
            if (wnd.playerIcon != null)
                _m_playerIconWnd = new NPGGUIWndPlayerIcon(wnd.playerIcon);
            // </AutoGen:_onWndInitDone>
        }
        
        public void setInfo(bool _isNewProminent,NPCommonSimplePlayerInfo _playerInfo, List<NPCommonCostItem> _rewardList, Action _setDealerDone)
        {
            _m_isNewProminent = _isNewProminent;
            _m_playerInfo = _playerInfo;
            _m_rewardList = _rewardList;
          
            
            _m_setDealerDone = _setDealerDone;
            _refreshWnd();
        }
        
        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            if(null == wnd)
                return;
                
            if(_m_playerInfo == null)
                return;
            // <AutoGen:_refreshWnd>
            // <UserCode name="playerInfo">
            if(_m_playerInfoWnd != null)
            {
                _m_playerInfoWnd.showWnd(new ShowCaseCommonResUnitInfoObj(_m_playerInfo.skinRef?.td_show));
            }
            // </UserCode>
            
            // <UserCode name="gemCostItem">
            if(_m_gemCostItemWnd != null)
            {
                NPCommonCostItem gemItem = null;
                foreach (var item in _m_rewardList)
                {
                    if (item != null && item.getCurrencyType() == ECurrency.GEM)
                    {
                        gemItem = item;
                        break;
                    }         
                }

                if (gemItem != null)
                {
                    _m_gemCostItemWnd.showWnd();
                    _m_gemCostItemWnd.setItem(gemItem);
                }
                else
                {
                    _m_gemCostItemWnd.hideWnd();
                }
            }
            // </UserCode>
            // <UserCode name="txtTypewriter">
            if(_m_txtTypewriterWnd != null)
            {
                _m_txtTypewriterWnd.showWnd();
                _m_txtTypewriterWnd.showTextTypewriter(_m_isNewProminent
                    ? TextTranslate.instance.getLanguage(GRefdataCoreMgr.instance.npGeneral.grave_congratulate_text_list.GetRandomItem())
                    : TextTranslate.instance.getLanguage(GRefdataCoreMgr.instance.npGeneral.grave_celebrate_text_list.GetRandomItem()));
            }
            // </UserCode>
            // <UserCode name="playerIcon">
            if(_m_playerIconWnd != null)
            {
                _m_playerIconWnd.showWnd();
                _m_playerIconWnd.setPlayerInfo(_m_playerInfo);
            }
            // </UserCode>
            // </AutoGen:_refreshWnd>
        }
        
        // <AutoGen:Method>
        // 关闭按钮点击事件
        private void _onClickbtnClose(GameObject go)
        {
            // <UserCode name="btnClose">
            _m_setDealerDone?.Invoke();
            // </UserCode>
        }
        // </AutoGen:Method>
    }
}