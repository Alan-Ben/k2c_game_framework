using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using NPCommon;
using UnityEngine;

namespace GOE
{
    public class GGUIWndAnecdoteEventChoiceResult : _ATALBasicUIWnd<GGUIMonoAnecdoteEventChoiceResult>
    {
        [NotNull] public static GGUIWndAnecdoteEventChoiceResult instance { get { return _g_instance ??= new GGUIWndAnecdoteEventChoiceResult(); } }
        private static GGUIWndAnecdoteEventChoiceResult _g_instance;
        
        
        private NPGGUIWndCommonItemContainer _m_itemContainer;
        
        private AnecdoteEventChoiceOptionRefObj _m_refObj;
        private List<NPCommon_ItemInfo> _m_rewardList;
        
        
        public GGUIWndAnecdoteEventChoiceResult() 
            : base(EALUIWndLayer.ADDITION)
        {
        }
        
        
        protected override string _monoAssetPath { get { return GGUIMonoAnecdoteEventChoiceResult.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoAnecdoteEventChoiceResult.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        

        protected override void _onShowWnd()
        {
            _m_itemContainer?.showWnd();
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_itemContainer?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_itemContainer?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_itemContainer?.discard();
            _m_itemContainer = null;

            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onCloseBtnClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            if (wnd.monoItemContainer != null)
                _m_itemContainer = new NPGGUIWndCommonItemContainer(wnd.monoItemContainer);
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onCloseBtnClick);
        }


        public void refreshWnd(AnecdoteEventChoiceOptionRefObj _refObj, List<NPCommon_ItemInfo> _rewardList)
        {
            _m_refObj = _refObj;
            _m_rewardList = _rewardList;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_refObj == null)
                return;
            
            ALUGUICommon.setLabelTxt(wnd.txtChoiceResult, TextTranslate.instance.getLanguage(_m_refObj.option_result_desc));
            if (_m_itemContainer != null)
            {
                if (_m_rewardList != null)
                    _m_itemContainer.showItemList(_m_rewardList.toItemDataList());
                else
                    _m_itemContainer.showItemList(_m_refObj.reward_item_list);
            }
            wnd.setChoiceRight(_m_refObj.is_right_choice);
        }
        
        
        private void _onCloseBtnClick(GameObject _obj)
        {
            QueueMgr.instance.DoUIRollBackByEsc();
        }
    }
}