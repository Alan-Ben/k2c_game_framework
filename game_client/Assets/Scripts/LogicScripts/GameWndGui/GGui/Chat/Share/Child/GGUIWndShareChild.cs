using ALPackage;
using System.Collections.Generic;
using Common.ChildEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 子嗣分享弹窗
    /// </summary>
    public class GGUIWndShareChild : _ANPGGUIBasicWnd<GGUIMonoShareChild>
    {
        private static GGUIWndShareChild _g_instance;

        public static GGUIWndShareChild instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new GGUIWndShareChild();
                return _g_instance;
            }
        }

        private _IShareIconSHow _m_curSelected = null;
        private GGUIWndShareIconItemGrid _m_shareIconGrid;
        private GGUISubWndChildInfo _m_wCardItem;

        public GGUIWndShareChild() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get => GGUIMonoShareChild.assetPath; }
        protected override string _monoObjName { get => GGUIMonoShareChild.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }

        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_shareIconGrid?.hideWnd();
            _m_wCardItem?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_shareIconGrid?.resetWnd();
            _m_wCardItem?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_curSelected = null;
            
            _m_shareIconGrid?.discard();
            _m_shareIconGrid = null;

            _m_wCardItem?.discard();
            _m_wCardItem = null;
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

            if (null != wnd.monoCardItem)
                _m_wCardItem = new GGUISubWndChildInfo(wnd.monoCardItem);
        }

        private void _refreshWnd()
        {
            if(null == wnd)
                return;

            List<_IShareIconSHow> showList = new List<_IShareIconSHow>();
            //1、获取已毕业成人列表
            List<UnmarriedInfo> adultList = new List<UnmarriedInfo>();
            NPPlayer.instance.childComp.getUnmarriedChildListNonAlloc(adultList);
            adultList.Sort(_sortAdultList);
            for (int i = 0; i < adultList.Count; i++)
            {
                if(adultList[i] != null && adultList[i].status != EAdultStatus.MARRIED)
                    showList.Add(new ShareIconShow_Adult(adultList[i].adultInfo));
            }

            //2、获取子嗣列表
            List<ChildInfo> childList = new List<ChildInfo>();
            NPPlayer.instance.childComp.getChildInfoList(childList);
            //去除未取名的
            for (int i = childList.Count - 1; i >= 0; i--)
            {
                if (string.IsNullOrEmpty(childList[i].name))
                    childList.RemoveAt(i);
            }
            childList.Sort(_sortChildList);
            for (int i = 0; i < childList.Count; i++)
            {
                showList.Add(new ShareIconShow_Child(childList[i]));
            }
            
            _m_shareIconGrid?.showItemList(showList);
        }

        //默认排序，品质从高到低，收益从高到低 
        private int _sortChildList(ChildInfo _a, ChildInfo _b)
        {
            if (_a == null || _b == null)
                return 0;

            if (_a.qualityRef.id.CompareTo(_b.qualityRef.id) != 0)
                return -_a.qualityRef.id.CompareTo(_b.qualityRef.id);

            return -_a.earnings.CompareTo(_b.earnings);
        }

        //默认排序，品质从高到低，收益从高到低 
        private int _sortAdultList(UnmarriedInfo _a, UnmarriedInfo _b)
        {
            if (_a == null || _b == null || _a.adultInfo.qualityRef == null || _b.adultInfo.qualityRef == null)
                return 0;

            if (_a.adultInfo.qualityRef.id.CompareTo(_b.adultInfo.qualityRef.id) != 0)
                return -_a.adultInfo.qualityRef.id.CompareTo(_b.adultInfo.qualityRef.id);

            return -_a.adultInfo.earnings.CompareTo(_b.adultInfo.earnings);
        }

        #region 点击事件

        //选中item
        private void _selectedItem(_IShareIconSHow _showData)
        {
            if (null == _showData)
                return;

            ShareIconShow_Child childInfo = _showData as ShareIconShow_Child;
            ShareIconShow_Adult adultInfo = _showData as ShareIconShow_Adult;
            if(null == childInfo && adultInfo == null)
                return;

            _m_curSelected = _showData;

            _m_wCardItem?.showWnd();
            _m_wCardItem?.refreshWnd(childInfo == null ? adultInfo.info : childInfo.info);
        }

        //点击关闭
        private void _clickCloseBtn(GameObject obj)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_CHAT_SELECT_SHARE_CHILD);
        }

        //点击分享
        private void _clickShareBtn(GameObject obj)
        {
            if (null == _m_curSelected)
                return;

            GChatUtil.sendChatShareMsg(EChatShareType.CHILD, _m_curSelected.creatShareInfo());
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_CHAT_SELECT_SHARE_CHILD);
        }

        #endregion
    }
}