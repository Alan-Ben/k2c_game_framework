
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    public class GGUISubWndCommonGridTextBarController : _AALUGUIGridBarController
    {
        [NotNull] private readonly GGUISubWndCommonGridTextBar _m_bar;
        
        
        public GGUISubWndCommonGridTextBarController(int _insertIndex, GGUIMonoBusinessBuildingOperatingHeroSelectGridTextBar _wnd) 
            : base(_insertIndex)
        {
            _m_bar = new GGUISubWndCommonGridTextBar(_wnd);
        }
        
        
        public override bool isAfterLineBar { get { return false; } }
        public override int barHeight { get { if (_m_bar.rectTransform != null) return (int)_m_bar.rectTransform.rect.height; return 0; } }


        public override void show()
        {
            _m_bar.showWnd();
        }
        public override void hide()
        {
            _m_bar.hideWnd();
        }
        protected override void _reset()
        {
            _m_bar.resetWnd();
        }
        protected override void _discard()
        {
            _m_bar.discard();
        }
        public override void setPos(float _x, float _y)
        {
            ALUGUICommon.setUIPos(_m_bar.rectTransform, _x, _y);
        }
        
        
        public void setBarText(string _textTranslated)
        {
            _m_bar.refreshWnd(_textTranslated);
        }
    }
    public class GGUISubWndCommonGridTextBar : _ATALBasicUISubWnd<GGUIMonoBusinessBuildingOperatingHeroSelectGridTextBar>
    {
        private string _m_textTranslated;
        
        
        public GGUISubWndCommonGridTextBar(GGUIMonoBusinessBuildingOperatingHeroSelectGridTextBar _wnd) : base(_wnd)
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
            refreshWnd();
        }


        public void refreshWnd(string _textTranslated)
        {
            _m_textTranslated = _textTranslated;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtBarDesc, _m_textTranslated);
        }
    }
}