using ALPackage;

namespace GOE
{
    public class GGUIWndCommonPropertyShow : _ANPGGUIBasicGridItemWnd<GGUIMonoCommonPropertyShow>
    {
        public GGUIWndCommonPropertyShow(GGUIMonoCommonPropertyShow _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
        }
        
        protected override void _onDiscard()
        {
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
        
        protected override void _resetGridItem()
        {
        }
        
        public void refreshWnd(_IPropertyShow _property, string _value)
        {
            if(wnd == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtPropertySimpleName, TextTranslate.instance.getLanguage(_property?.simpleName));
            ALUGUICommon.setLabelTxt(wnd.txtPropertyValue, _value);
        }
    }
}