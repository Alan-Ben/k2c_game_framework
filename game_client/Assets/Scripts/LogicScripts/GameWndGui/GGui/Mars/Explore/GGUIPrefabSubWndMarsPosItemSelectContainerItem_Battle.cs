using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIPrefabSubWndMarsPosItemSelectContainerItem_Battle : _ANPGGUIBasicLoadPrefabSubWnd<GGUIMonoMarsPosItemSelectContainerItem_Battle>
    {
        private MarsExploreBattleEventInfo _m_eventInfo;
        private NPGGuiWndTexture _m_iconWnd;


        public GGUIPrefabSubWndMarsPosItemSelectContainerItem_Battle(Transform _parent)
            : base(_parent)
        {
        }


        protected override string _monoAssetPath { get { return GGUIMonoMarsPosItemSelectContainerItem_Battle.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoMarsPosItemSelectContainerItem_Battle.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        protected override void _onShowWnd()
        {
            _m_iconWnd?.showWnd();
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_iconWnd?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_iconWnd?.discardTexture();
        }
        protected override void _onDiscard()
        {
            _m_iconWnd?.discard();
            _m_iconWnd = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnGo, _onClickGo);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.imgIcon != null)
                _m_iconWnd = new NPGGuiWndTexture(wnd.imgIcon);

            ALUGUICommon.combineBtnClick(wnd.btnGo, _onClickGo);
        }


        public void refreshWnd(MarsExploreBattleEventInfo _eventInfo)
        {
            _m_eventInfo = _eventInfo;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_eventInfo == null)
                return;

            _m_iconWnd?.setTexture(_m_eventInfo.typeRefObj.icon);
            ALUGUICommon.setLabelTxt(wnd.txtName, TextTranslate.instance.getLanguage(_m_eventInfo.typeRefObj.name));
            ALUGUICommon.setLabelTxt(wnd.txtLevel, TextTranslate.instance.getLanguage(TransKeyConst.common_level_num, _m_eventInfo.exploreLvl));
            ALUGUICommon.setLabelTxt(wnd.txtRecommendPower, _m_eventInfo.getPower().ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
        }


        private void _onClickGo(GameObject _go)
        {
            if (_m_eventInfo == null)
                return;

            GGUIWndMarsExploreBattleInfo.instance.refreshWnd(_m_eventInfo);
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMarsExploreBattleInfo.instance, GGUIWndMarsExploreBattleInfo.instance.showWnd,
                UINodeTagConst.C_MARS_EXPLORE_BATTLE_INFO);
        }
    }
}
