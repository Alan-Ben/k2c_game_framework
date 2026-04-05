using ALPackage;

namespace GOE
{
    /// <summary>
    /// 通用游历事件结果预制体子窗口接口
    /// </summary>
    public interface _ITravelResultWnd
    {
        void discard();

        void showWnd();
        
        void hideWnd();
        
        void resetWnd();
        
        _AALBasicLoadUIWndBasicClass getBasicLoadUIWndBasicClass();
    }
}