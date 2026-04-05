using ALPackage;

namespace GOE
{
    public class NPGGUIWndCommonPageTagItem : _ATALBasicUISubWnd<NPGGUIMonoCommonPageTagItem>
    {
        public NPGGUIWndCommonPageTagItem(NPGGUIMonoCommonPageTagItem _mono) : base(_mono)
        {
            initWnd();
        }


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

        }


        /// <summary>
        /// 设置是否选中
        /// </summary>
        /// <param name="_isSelected"></param>
        public void setSelected(bool _isSelected)
        {
            ALUGUICommon.setGameObjEnable(wnd.goListShowOnSelect, _isSelected);
            ALUGUICommon.setGameObjEnable(wnd.goListHideOnSelect, !_isSelected);
        }
    }
}
