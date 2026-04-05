using ALPackage;
using Common.DinnerEnum;
using Common.DinnerObj;
using UnityEngine;

namespace GOE
{
    public class GGUIWndDinnerCreateItem_Bar : _AGGUIWndDinnerCreateItemBase
    {
        private string _m_sAssetPath;
        private string _m_sObjName;
        GGUIMonoDinnerCreateItem_Bar _m_realWndMono;
        private DinnerCreateItemShowInfo _m_itemInfo;

        public GGUIWndDinnerCreateItem_Bar(string _assetPath, string _objName, Transform _parent) : base(_parent)
        {
            _m_sAssetPath = _assetPath;
            _m_sObjName = _objName;
        }

        protected override string _monoAssetPath => _m_sAssetPath;

        protected override string _monoObjName => _m_sObjName;

        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            _m_realWndMono = wnd as GGUIMonoDinnerCreateItem_Bar;
            if(null == _m_realWndMono)
                return;
        }

        public override void setInfo(DinnerCreateItemShowInfo _info)
        { 
            _m_itemInfo = _info;
            if(null == _m_itemInfo)
                return;
            _refreshWnd();
        }

        public override void setCreatDinner()
        {
            
        }


        private void _refreshWnd()
        {
            if (_m_realWndMono == null)
                return;
            if(_m_itemInfo.itemType == EDinnerCreateItemType.Bar_Celebration)
                ALUGUICommon.setLabelTxt(_m_realWndMono.txtName, TextTranslate.instance.getLanguage(TransKeyConst.dinner_bar_celebration_name));
            else
                ALUGUICommon.setLabelTxt(_m_realWndMono.txtName, TextTranslate.instance.getLanguage(TransKeyConst.dinner_bar_party_name));

        }
    }
}