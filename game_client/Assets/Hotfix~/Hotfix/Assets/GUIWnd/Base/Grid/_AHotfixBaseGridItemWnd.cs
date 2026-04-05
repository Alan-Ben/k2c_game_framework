using ALPackage;
using GOE;

namespace Hotfix
{
    /// <summary>
    /// 热更工程通用Grid Item基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class _AHotfixBaseGridItemWnd<T> : _AGGUIHotfixBasicGridItemWnd
        where T : _AHotfixBaseMono, new()
    {
        //对应业务的mono
        private T _m_hotfixWnd;
        protected T hotfixWnd { get { return _m_hotfixWnd; } }

        protected _AHotfixBaseGridItemWnd(GGUIHotfixCommonMono _wnd) : base(_wnd)
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