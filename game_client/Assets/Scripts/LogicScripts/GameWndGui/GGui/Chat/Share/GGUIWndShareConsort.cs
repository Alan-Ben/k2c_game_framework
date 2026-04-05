
using ALPackage;
using ChatPackage;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 骑士分享弹窗
    /// </summary>
    public class GGUIWndShareConsort : _ANPGGUIBasicWnd<GGUIMonoShareConsort>
    {
        private static GGUIWndShareConsort _g_instance;

        public static GGUIWndShareConsort instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new GGUIWndShareConsort();
                return _g_instance;
            }
        }
        
        private _IShareIconSHow _m_curSelected = null;
        private GGUIWndShareIconItemGrid _m_shareIconGrid;
        private GGUIWndConsortCardItem _m_consortCardItem;
        private GGUISubWndConsortFetterInfo _m_wConsortFetter;
        
        public GGUIWndShareConsort() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get => GGUIMonoShareConsort.assetPath; }
        protected override string _monoObjName { get => GGUIMonoShareConsort.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }

        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_wConsortFetter?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wConsortFetter?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_curSelected = null;
            
            _m_shareIconGrid?.discard();
            _m_shareIconGrid = null;
            
            _m_consortCardItem?.discard();
            _m_consortCardItem = null;
            
            _m_wConsortFetter?.discard();
            _m_wConsortFetter = null;
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            
            ALUGUICommon.combineBtnClick(wnd.closeBtn, _clickCloseBtn);
            ALUGUICommon.combineBtnClick(wnd.shareBtn, _clickShareBtn);
            if (null != wnd.shareIconGrid)
            {
                _m_shareIconGrid = new GGUIWndShareIconItemGrid(wnd.shareIconGrid);
                _m_shareIconGrid.selectedItem += _selectedItem;
            }

            if (null != wnd.monoConsortCard)
            {
                _m_consortCardItem = new GGUIWndConsortCardItem(wnd.monoConsortCard,null);
            }

            if (wnd.monoConsortFetterInfo != null)
                _m_wConsortFetter = new GGUISubWndConsortFetterInfo(wnd.monoConsortFetterInfo);
        }

        private void _selectedItem(_IShareIconSHow _showData)
        {
            if (null == _showData)
                return;
            ShareIconShow_Consort shareConsortInfo = _showData as ShareIconShow_Consort;
            if(null == shareConsortInfo || shareConsortInfo.consortInfo == null)
                return;
            _m_curSelected = _showData;
            _m_consortCardItem?.showWnd();
            _m_consortCardItem?.setInfo(shareConsortInfo.consortInfo);

            if (_m_wConsortFetter != null)
            {
                _m_wConsortFetter.showWnd();
                _m_wConsortFetter.setData(shareConsortInfo.consortInfo.fetterInfo);
            }
        }

        private void _clickCloseBtn(GameObject obj)
        {
            _doCloseWnd();
        }

        private void _clickShareBtn(GameObject obj)
        {
            if (null == _m_curSelected)
                return;
            
            GChatUtil.sendChatShareMsg(EChatShareType.CONSORT, _m_curSelected.creatShareInfo());
            _doCloseWnd();
        }

        private void _doCloseWnd()
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_Main_Chat_SHARE_CONSORT);
        }

        private void _refreshWnd()
        {
            if(null == wnd)
                return;
            
            List<_IShareIconSHow> showList = new List<_IShareIconSHow>();
            List<GGottenConsortInfo> consortList = new List<GGottenConsortInfo>();
            consortList.AddRange(NPPlayer.instance.consortComp.consortList);
            consortList.Sort(ConsortUtil.sortConsortShowInfoDefaultWithoutUnlockCheck);
            for (int i = 0; i < consortList.Count; i++)
            {
                showList.Add(new ShareIconShow_Consort(consortList[i]));
            }
            
            _m_shareIconGrid?.showItemList(showList);
        }
    }
}