using ALPackage;
using GOE;

namespace Hotfix
{
    /// <summary>
    /// container Item 范例
    /// </summary>
    public class GGUIWndDemoContainerItem : _AHotfixBaseSubWnd<GGUIMonoDemoContainerItem>
    {
        public GGUIWndDemoContainerItem(GGUIHotfixCommonMono _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
            Debug.LogError($"=====NPGGUIWndDemoContainerItem===_onShowWnd");
        }

        protected override void _onHideWnd()
        {
            Debug.LogError($"=====NPGGUIWndDemoContainerItem===_onHideWnd");
        }

        protected override void _onReset()
        {
            Debug.LogError($"=====NPGGUIWndDemoContainerItem===_onReset");
        }

        protected override void _onDiscard()
        {
            Debug.LogError($"=====NPGGUIWndDemoContainerItem===_onDiscard");
        }

        protected override void _onWndInitDoneHotfix()
        {
            Debug.LogError($"=====NPGGUIWndDemoContainerItem===_onWndInitDoneHotfix");
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