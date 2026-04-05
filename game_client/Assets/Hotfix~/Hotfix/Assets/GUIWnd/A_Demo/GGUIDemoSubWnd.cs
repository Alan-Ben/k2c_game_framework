using ALPackage;
using GOE;

namespace Hotfix
{
    /// <summary>
    /// SubWnd 范例
    /// </summary>
    public class GGUIDemoSubWnd : _AHotfixBaseSubWnd<GGUIDemoSubWndMono>
    {
        public GGUIDemoSubWnd(GGUIHotfixCommonMono _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
            Debug.LogError($"=====NPGGUIDemoSubWnd===_onShowWnd");
        }

        protected override void _onHideWnd()
        {
            Debug.LogError($"=====NPGGUIDemoSubWnd===_onHideWnd");
        }

        protected override void _onReset()
        {
            Debug.LogError($"=====NPGGUIDemoSubWnd===_onReset");
        }

        protected override void _onDiscard()
        {
            Debug.LogError($"=====NPGGUIDemoSubWnd===_onDiscard");
        }

        protected override void _onWndInitDoneHotfix()
        {
            Debug.LogError($"=====NPGGUIDemoSubWnd===_onWndInitDoneHotfix");
        }

        /// <summary>
        /// 外部调用测试显示方法
        /// </summary>
        /// <param name="_value"></param>
        public void showText(string _value)
        {
            if(null == hotfixWnd)
                return;
            
            ALUGUICommon.setLabelTxt(hotfixWnd.goText, _value);
        }
    }
}