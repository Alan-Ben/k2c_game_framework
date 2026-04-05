using System;
using System.Collections.Generic;
using ALPackage;
using GOE;
using NPCommon;
using NPEnum;
using UnityEngine;

namespace GOE
{
    
    /// <summary>
    /// 
    /// </summary>
    public class GGUIWndChapterEventResult: _ATALBasicUIWnd<GGUIMonoChapterEventResult>
    {
        private static GGUIWndChapterEventResult _g_instance = new GGUIWndChapterEventResult();

        public static GGUIWndChapterEventResult instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndChapterEventResult();
                return _g_instance;
            }
        }
        
        private NPGGUIWndGetItemContainer _m_itemContainer;
        private List<NPCommon_ItemInfo> _m_itemList;
        private string _m_title;
        private string _m_desc;
        private Action _m_onClose;


        public GGUIWndChapterEventResult() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get => GGUIMonoChapterEventResult.assetPath; }
        protected override string _monoObjName { get => GGUIMonoChapterEventResult.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }

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
            //展示粒子效果
            if (wnd != null) 
                GCommon.showItemParticle(_m_itemList, wnd.particleStartRectTransform);

            // 奖励列表
            _m_itemContainer?.discard();
            _m_itemContainer = null;
            
            _m_itemList = null;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnBgClose, _onBtnCloseClick);
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClick);
            ALUGUICommon.combineBtnClick(wnd.btnBgClose, _onBtnCloseClick);
                
            // 奖励列表
            if (null != wnd.itemContainer)
                _m_itemContainer = new NPGGUIWndGetItemContainer(wnd.itemContainer);
        }
        
        public void refreshWnd(string _title, string _desc, List<NPCommon.NPCommon_ItemInfo> _itemInfos)
        {
            _m_title = _title;
            _m_desc = _desc;

            // 奖励列表
            _m_itemList = _itemInfos;
            _refreshWnd();
        }
        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            if(null == wnd)
                return;
            ALUGUICommon.setLabelTxt(wnd.txtTitle, TextTranslate.instance.getLanguage(_m_title));
            ALUGUICommon.setLabelTxt(wnd.txtDesc, TextTranslate.instance.getLanguage(_m_desc));
            
            _m_itemContainer?.showWnd();
            _m_itemContainer?.showItemList(_m_itemList);
        }

        
        /// <summary>
        /// 点击关闭按钮
        /// </summary>
        /// <param name="_"></param>
        private void _onBtnCloseClick(GameObject _)
        {
            QueueMgr.instance.DoUIRollBackByEsc();
        }
        
    }
}