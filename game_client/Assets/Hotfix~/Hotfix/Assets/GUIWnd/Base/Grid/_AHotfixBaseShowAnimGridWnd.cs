using ALPackage;
using GOE;

namespace Hotfix
{
    /// <summary>
    /// 热更工程通用Grid基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class _AHotfixBaseShowAnimGridWnd<T, _T_ITEM_WND> : _AGGUIHotfixBasicShowAnimGridWnd<_T_ITEM_WND>
        where T : _AHotfixBaseMono, new()
        where _T_ITEM_WND : _AGGUIHotfixBasicGridItemWnd
    {
        //对应业务的mono
        private T _m_hotfixWnd;
        protected T hotfixWnd { get { return _m_hotfixWnd; } }

        protected _AHotfixBaseShowAnimGridWnd(GGUIHotfixGridMono _wnd) : base(_wnd)
        {
            initWnd();
        }

        //sealed掉初始化窗口代码，处理特殊处理，子类实现新的
        protected sealed override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            
            _m_hotfixWnd = new T();
            _m_hotfixWnd.init(wnd.monoSkin);

            //执行子类的初始化
            _onWndInitDoneHotfix();
        }

        protected abstract void _onWndInitDoneHotfix();
    }
}