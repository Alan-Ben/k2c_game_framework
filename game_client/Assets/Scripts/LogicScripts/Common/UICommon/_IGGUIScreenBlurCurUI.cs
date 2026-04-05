using ALPackage;

namespace GOE
{
    /// <summary>
    /// 游戏中作为模糊背景展示UI时，当前显示的窗口对象接口
    /// </summary>
    public interface _IGGUIScreenBlurCurUI
    {
        /// <summary>
        /// 隐藏UI，一般使用scale缩放，不影响本身窗口的show，hide逻辑
        /// </summary>
        void hideUI();

        /// <summary>
        /// 显示UI，一般使用scale缩放，不影响本身窗口的show，hide逻辑
        /// </summary>
        void showUI();
    }
}