using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUISubWndMarsBuildingInfoEquipmentPropertyContainerItem : _ATALBasicUISubWnd<GGUIMonoMarsBuildingInfoEquipmentPropertyContainerItem>
    {
        private NPGGuiWndTexture _m_iconWnd;
        private MarsBuildingEquipmentPropertyShowData _m_propertyShowData;
        
        
        public GGUISubWndMarsBuildingInfoEquipmentPropertyContainerItem([NotNull] GGUIMonoMarsBuildingInfoEquipmentPropertyContainerItem _mono)
            : base(_mono)
        {
            initWnd();
        }


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
            
            ALUGUICommon.uncombineBtnClick(wnd.btnDesc, _onBtnDescClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            if (wnd.imgIcon != null)
                _m_iconWnd = new NPGGuiWndTexture(wnd.imgIcon);
            
            ALUGUICommon.combineBtnClick(wnd.btnDesc, _onBtnDescClick);
        }


        public void refreshWnd(MarsBuildingEquipmentPropertyShowData _propertyShowData)
        {
            _m_propertyShowData = _propertyShowData;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;
                
            _m_iconWnd?.setTexture(_m_propertyShowData.icon);
            ALUGUICommon.setLabelTxt(wnd.txtTitle, _m_propertyShowData.propertyName);
            ALUGUICommon.setLabelTxt(wnd.txtValue, _m_propertyShowData.currentValueStr);
            
            bool isLevelMax = _m_propertyShowData.isLevelMax;
            wnd.setLevelMaxState(isLevelMax);
            
            if (!isLevelMax)
            {
                ALUGUICommon.setLabelTxt(wnd.txtNextValue, _m_propertyShowData.nextValueStr);
            }
        }
        
        
        private void _onBtnDescClick(GameObject _go)
        {
            if (wnd == null || _go == null)
                return;
            
            QueueMgr.instance.AddNode(new GNodeCommonToolTip_Title_Text(
                UIResPathAssistant.getAssetPath(UIResPathConst.WIN_TOOL_TIP_TITLE_TEXT),
                UIResPathAssistant.getObjName(UIResPathConst.WIN_TOOL_TIP_TITLE_TEXT),
                _m_propertyShowData.propertyName,
                _m_propertyShowData.propertyDesc,
                _go.GetComponent<RectTransform>(), 0, wnd.toolTipInterval));
        }
    }
}