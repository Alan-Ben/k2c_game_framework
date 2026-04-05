using ALPackage;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 情人CG分享弹窗
    /// </summary>
    public class GGUIWndShareConsortCG : _ANPGGUIBasicWnd<GGUIMonoShareConsortCG>
    {
        private static GGUIWndShareConsortCG _g_instance;
        public static GGUIWndShareConsortCG instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new GGUIWndShareConsortCG();
                return _g_instance;
            }
        }

        private ConsortCgInfo _m_curSelected;
        private GGUIWndShareConsortCGGrid _m_shareIconGrid;

        public GGUIWndShareConsortCG() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get => GGUIMonoShareConsortCG.assetPath; }
        protected override string _monoObjName { get => GGUIMonoShareConsortCG.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }

        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_shareIconGrid?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_shareIconGrid?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_curSelected = null;
            
            _m_shareIconGrid?.discard();
            _m_shareIconGrid = null;

            if (null == wnd)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _clickCloseBtn);
            ALUGUICommon.uncombineBtnClick(wnd.btnShare, _clickShareBtn);
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _clickCloseBtn);
            ALUGUICommon.combineBtnClick(wnd.btnShare, _clickShareBtn);

            if (null != wnd.monoCGGrid)
            {
                _m_shareIconGrid = new GGUIWndShareConsortCGGrid(wnd.monoCGGrid);
                _m_shareIconGrid.selectedItem += _selectedItem;
            }
        }

        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            if(null == wnd)
                return;

            List<ConsortCgInfo> consortCgInfoList = new List<ConsortCgInfo>();
            GRefdataCoreMgr.instance.consortCGRefCore.dealAllRef(_cgRef =>
            {
                if (_cgRef != null)
                {
                    ConsortCgInfo info = NPPlayer.instance.consortComp.getConsortCgInfo(_cgRef.cg_id);
                    // 只显示已获得的情人CG
                    if (info != null)
                        consortCgInfoList.Add(info);
                }
            });
            if(consortCgInfoList.Count > 0)
                consortCgInfoList.Sort((_a,_b)=>_a.cgId.CompareTo(_b.cgId));
            _m_shareIconGrid?.showWnd();
            _m_shareIconGrid?.setInfo(consortCgInfoList);
        }

        #region 点击事件

        //选中item
        private void _selectedItem(ConsortCgInfo _showData)
        {
            _m_curSelected = _showData;
        }

        //点击关闭
        private void _clickCloseBtn(GameObject obj)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_CHAT_SELECT_SHARE_CONSORT_CG);
        }

        //点击分享
        private void _clickShareBtn(GameObject obj)
        {
            if (null == _m_curSelected)
                return;
            
            GChatUtil.sendChatShareMsg(EChatShareType.CONSORT_CG, NPMsgDetailInfoFactory.createShareConsortCGInfo(_m_curSelected));
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_CHAT_SELECT_SHARE_CONSORT_CG);
        }

        #endregion
    }
}