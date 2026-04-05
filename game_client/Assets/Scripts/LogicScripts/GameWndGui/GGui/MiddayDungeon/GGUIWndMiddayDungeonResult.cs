using System;
using System.Collections.Generic;
using ALPackage;
using CommonEnum;
using NPCommon;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 
    /// </summary>
    public class GGUIWndMiddayDungeonResult : _ATALBasicUIWnd<GGUIMonoMiddayDungeonResult>
    {
        private static GGUIWndMiddayDungeonResult _g_instance = new GGUIWndMiddayDungeonResult();
    
        public static GGUIWndMiddayDungeonResult instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndMiddayDungeonResult();
                return _g_instance;
            }
        }
    
        private NPGGUIWndGetItemContainer _m_itemContainer;
        private List<NPCommon_ItemInfo> _m_itemList;
        private long _m_dungeon_coin_count;
        private Action _m_closeAction;

        public GGUIWndMiddayDungeonResult() : base(EALUIWndLayer.ADDITION)
        {
        }
    
        protected override string _monoAssetPath { get => GGUIMonoMiddayDungeonResult.assetPath; }
        protected override string _monoObjName { get => GGUIMonoMiddayDungeonResult.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }
        public override bool needDiscardOnSwitch { get { return true; } }

        protected override void _onShowWnd()
        {
            _refreshWnd();
        }
    
        protected override void _onHideWnd()
        {
        }
    
        protected override void _onReset()
        {
            
        }
    
        protected override void _onDiscard()
        {
            // 奖励列表
            _m_itemContainer?.discard();
            _m_itemContainer = null;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClick);
        }
    
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            // 奖励列表
            if (null != wnd.itemContainer)
                _m_itemContainer = new NPGGUIWndGetItemContainer(wnd.itemContainer);
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClick);
        }

        private void _onBtnCloseClick(GameObject _obj)
        {
            _m_closeAction?.Invoke();
        }

        public void setInfo(List<NPCommon.NPCommon_ItemInfo> _itemList, Action _closeAction)
        {
            if(_m_itemList == null)
                _m_itemList = new List<NPCommon.NPCommon_ItemInfo>();
            _m_itemList.Clear();
            foreach (var item in _itemList)
            {
                if ((ENPItemType) item.getItemType() == ENPItemType.CURRENCY &&
                    (ECurrency) item.getSubId() == ECurrency.DUNGEON_COIN)
                {
                    _m_dungeon_coin_count = item.getCount();
                    continue;
                }
                _m_itemList.Add(item);
            }
            _m_closeAction = _closeAction;
            _refreshWnd();
        }
        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            if(null == wnd)
                return;
            _m_itemContainer?.showWnd();
            _m_itemContainer?.showItemList(_m_itemList);
            ALUGUICommon.setLabelTxt(wnd.txtScore, TextTranslate.instance.getLanguage(TransKeyConst.midday_dungeon_result_point_num, _m_dungeon_coin_count));
        }
    }
}