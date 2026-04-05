using ALPackage;
using JetBrains.Annotations;
using NPEnum;
using UnityEngine;

namespace GOE
{
    public class GGUIWndAnecdoteEventEarnings : _ATALBasicUIWnd<GGUIMonoAnecdoteEventEarnings>
    {
        [NotNull] public static GGUIWndAnecdoteEventEarnings instance { get { return _g_instance ??= new GGUIWndAnecdoteEventEarnings(); } }
        private static GGUIWndAnecdoteEventEarnings _g_instance;

        
        private AnecdoteEventEarningsRefObj _m_refObj;
        private NPGGUIWndCommonItemContainer _m_itemContainer;
        private NPGGuiWndTexture _m_heroRewardTexWnd;
        private NPGGuiWndTexture _m_heroRewardTexWndAdditional;
        

        public GGUIWndAnecdoteEventEarnings() 
            : base(EALUIWndLayer.ADDITION)
        {
        }
        

        protected override string _monoAssetPath { get { return GGUIMonoAnecdoteEventEarnings.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoAnecdoteEventEarnings.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        

        protected override void _onShowWnd()
        {
            _m_itemContainer?.showWnd();
            _m_heroRewardTexWnd?.showWnd();
            _m_heroRewardTexWndAdditional?.showWnd();
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_itemContainer?.hideWnd();
            _m_heroRewardTexWnd?.hideWnd();
            _m_heroRewardTexWndAdditional?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_itemContainer?.resetWnd();
            _m_heroRewardTexWnd?.discardTexture();
            _m_heroRewardTexWndAdditional?.discardTexture();
        }
        protected override void _onDiscard()
        {
            _m_itemContainer?.discard();
            _m_itemContainer = null;
            _m_heroRewardTexWnd?.discard();
            _m_heroRewardTexWnd = null;
            _m_heroRewardTexWndAdditional?.discard();
            _m_heroRewardTexWndAdditional = null;

            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onCloseBtnClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnGainReward, _onCloseBtnClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            if (wnd.monoResultRewardList != null)
                _m_itemContainer = new NPGGUIWndCommonItemContainer(wnd.monoResultRewardList);
            if (wnd.imgHeroRewardImage != null)
                _m_heroRewardTexWnd = new NPGGuiWndTexture(wnd.imgHeroRewardImage);
            if (wnd.imgHeroRewardImageAdditional != null)
                _m_heroRewardTexWndAdditional = new NPGGuiWndTexture(wnd.imgHeroRewardImageAdditional);
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onCloseBtnClick);
            ALUGUICommon.combineBtnClick(wnd.btnGainReward, _onCloseBtnClick);
        }
        
        
        public void refreshWnd(AnecdoteEventEarningsRefObj _refObj)
        {
            _m_refObj = _refObj;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_refObj == null)
                return;
            
            ALUGUICommon.setLabelTxt(wnd.txtEarningsTargetDesc, TextTranslate.instance.getLanguage(_m_refObj.event_desc, _m_refObj.event_desc_params));
            long curEarnings = NPPlayer.instance.specialItemComp.goldData.earnings;
            long targetEarnings = _m_refObj.earnings;
            ALUGUICommon.setLabelTxt(wnd.txtCurEarnings, TextTranslate.instance.getLanguage(TransKeyConst.common_value, curEarnings.ToLargeString(PrimitiveExtension.ELargeStringType.GOLD)));
            ALUGUICommon.setLabelTxt(wnd.txtTargetEarnings, TextTranslate.instance.getLanguage(TransKeyConst.anecdote_earnings_goal_num, targetEarnings.ToLargeString(PrimitiveExtension.ELargeStringType.GOLD)));
            if (wnd.sldProgress != null)
            {
                wnd.sldProgress.minValue = 0;
                wnd.sldProgress.maxValue = targetEarnings;
                wnd.sldProgress.value = curEarnings;
            }
            
            _m_itemContainer?.showItemList(_m_refObj.result_reward_item);
            HeroRefObj heroRewardRef = null;
            if (_m_refObj.result_reward_item != null)
            {
                NPCommonCostItem heroRewardItem = _m_refObj.result_reward_item.Find(_item => _item.getItemType() == ENPItemType.HERO);
                if (heroRewardItem != null)
                    heroRewardRef = GRefdataCoreMgr.instance.heroRefCore.getRef(heroRewardItem.subId);
            }
            _m_heroRewardTexWnd?.setTexture(heroRewardRef?.card_image);
            _m_heroRewardTexWndAdditional?.setTexture(heroRewardRef?.card_image);
            wnd.setHasHeroReward(heroRewardRef != null);
            wnd.setComplete(curEarnings >= targetEarnings);
        }
        
        
        private void _onCloseBtnClick(GameObject _obj)
        {
            QueueMgr.instance.DoUIRollBackByEsc();
        }
    }
}