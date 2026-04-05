using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndMarsPosItemSelect : _ATALBasicUIWnd<GGUIMonoMarsPosItemSelect>
    {
        [NotNull] public static GGUIWndMarsPosItemSelect instance { get { return _g_instance ??= new GGUIWndMarsPosItemSelect(); } }
        private static GGUIWndMarsPosItemSelect _g_instance;


        private List<_IMarsExplorePosItem> _m_posItemList;
        private GGUISubWndMarsPosItemSelectContainer _m_containerWnd;


        public GGUIWndMarsPosItemSelect()
            : base(EALUIWndLayer.ADDITION)
        {
        }


        protected override string _monoAssetPath { get { return GGUIMonoMarsPosItemSelect.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoMarsPosItemSelect.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        protected override void _onShowWnd()
        {
            _m_containerWnd?.showWnd();
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_containerWnd?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_containerWnd?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_containerWnd?.discard();
            _m_containerWnd = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoItemContainer != null)
                _m_containerWnd = new GGUISubWndMarsPosItemSelectContainer(wnd.monoItemContainer);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }


        public void refreshWnd(long _posId)
        {
            _m_posItemList = NPPlayer.instance.marsComp.exploreSubComponent.getPosItemListAtPos(_posId);
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            _m_containerWnd?.refreshWnd(_m_posItemList);
        }


        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_MARS_EXPLORE_POS_ITEM_SELECT);
        }
    }
}
