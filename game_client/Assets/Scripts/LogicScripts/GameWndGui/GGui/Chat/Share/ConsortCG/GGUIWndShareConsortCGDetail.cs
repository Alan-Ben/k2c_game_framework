using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 情人CG分享详情弹窗
    /// </summary>
    public class GGUIWndShareConsortCGDetail : _ANPGGUIBasicWnd<GGUIMonoShareConsortCGDetail>
    {
        private static GGUIWndShareConsortCGDetail _g_instance;
        public static GGUIWndShareConsortCGDetail instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new GGUIWndShareConsortCGDetail();
                return _g_instance;
            }
        }

        //CG配置
        private ConsortCGRefObj _m_cgRef;
        //CG Showcase
        private NPGGUIWndCommonShowCase _m_cgShowCase;

        public GGUIWndShareConsortCGDetail() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get => GGUIMonoShareConsortCGDetail.assetPath; }
        protected override string _monoObjName { get => GGUIMonoShareConsortCGDetail.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_cgShowCase?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_cgShowCase?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_cgShowCase?.discard();
            _m_cgShowCase = null;

            if (null == wnd)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickCloseBtn);
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;

            if(wnd.monoShowCaseWnd != null)
                _m_cgShowCase = new NPGGUIWndCommonShowCase(wnd.monoShowCaseWnd);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickCloseBtn);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_cgId"></param>
        public void setInfo(long _cgId)
        {
            if (wnd == null)
                return;

            _m_cgRef = GRefdataCoreMgr.instance.consortCGRefCore.getRef(_cgId);

            //CG 展示
            if (_m_cgShowCase != null && _m_cgRef != null)
            {
                _AShowCaseUnitInfoObj[] showCaseUnitInfoObjList = new _AShowCaseUnitInfoObj[wnd.showcaseIndex + 1];
                showCaseUnitInfoObjList.SetValue(new ShowCaseCommonResUnitInfoObj(_m_cgRef.cg_go), wnd.showcaseIndex);
                _m_cgShowCase.showWnd(showCaseUnitInfoObjList);
            }
        }

        #region 点击事件

        //点击关闭
        private void _onClickCloseBtn(GameObject obj)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_CHAT_SELECT_SHARE_CONSORT_CG_DETAIL);
        }

        #endregion
    }
}