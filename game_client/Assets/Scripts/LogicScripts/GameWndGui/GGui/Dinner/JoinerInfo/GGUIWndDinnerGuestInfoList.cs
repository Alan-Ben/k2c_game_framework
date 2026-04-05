using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 赴宴信息列表
    /// </summary>
    public class GGUIWndDinnerGuestInfoList : _ATALBasicUIWnd<GGUIMonoDinnerGuestInfoList>
    {
        private static GGUIWndDinnerGuestInfoList _g_instance = new GGUIWndDinnerGuestInfoList();

        public static GGUIWndDinnerGuestInfoList instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndDinnerGuestInfoList();
                return _g_instance;
            }
        }
        private GGUIWndDinnerGuestItemGrid _mGuestItemGrid;
        private List<GDinnerGuestInfo> _m_dinnerGuestLogList;

        public GGUIWndDinnerGuestInfoList() : base(EALUIWndLayer.ADDITION)
        {
        }


        protected override string _monoAssetPath { get => GGUIMonoDinnerGuestInfoList.assetPath; }
        protected override string _monoObjName { get => GGUIMonoDinnerGuestInfoList.objName; }
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
            _mGuestItemGrid?.discard();
            _mGuestItemGrid = null;
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
            if (null != wnd.guestItemGrid)
            {
                _mGuestItemGrid = new GGUIWndDinnerGuestItemGrid(wnd.guestItemGrid);
            }
        }

        /// <summary>
        /// 设置显示信息
        /// </summary>
        /// <param name="_dinnerId"></param>
        public void setInfo(List<GDinnerGuestInfo> _guestLogList)
        {
            _m_dinnerGuestLogList = _guestLogList;
            _refreshWnd();
        }


        private void _refreshWnd()
        {
            if(null == wnd)
                return;
            _mGuestItemGrid?.showWnd();
            _mGuestItemGrid?.showItemList(_m_dinnerGuestLogList);
        }

        /// <summary>
        /// 点击关闭
        /// </summary>
        /// <param name="obj"></param>
        private void _onClickClose(GameObject obj)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_DINNER_GUEST_LIST);
        }
    }
}