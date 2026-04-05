using ALPackage;
using GOE;

namespace Hotfix
{
    /// <summary>
    /// grid Item 范例
    /// </summary>
    public class GGUIWndDemoGridItem : _AHotfixBaseGridItemWnd<GGUIMonoDemoGridItem>
    {
        public GGUIWndDemoGridItem(GGUIHotfixCommonMono _wnd) : base(_wnd)
        {
        }

        protected override void _onShowWnd()
        {
            Debug.LogError($"=====NPGGUIWndDemoGridItem===_onShowWnd");
        }

        protected override void _onHideWnd()
        {
            Debug.LogError($"=====NPGGUIWndDemoGridItem===_onHideWnd");
        }

        protected override void _onReset()
        {
            Debug.LogError($"=====NPGGUIWndDemoGridItem===_onReset");
        }

        protected override void _onDiscard()
        {
            Debug.LogError($"=====NPGGUIWndDemoGridItem===_onDiscard");
        }

        protected override void _onWndInitDoneHotfix()
        {
            Debug.LogError($"=====NPGGUIWndDemoGridItem===_onWndInitDoneHotfix");
        }
        protected override void _resetGridItem()
        {
        }

        /// <summary>
        /// 外部调用测试显示方法
        /// </summary>
        /// <param name="_value"></param>
        public void setInfo(int _value)
        {
            if(null == hotfixWnd)
                return;
            
            ALUGUICommon.setLabelTxt(hotfixWnd.goText, _value);
        }

    }
}