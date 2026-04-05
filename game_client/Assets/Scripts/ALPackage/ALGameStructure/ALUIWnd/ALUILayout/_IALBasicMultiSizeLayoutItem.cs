
namespace ALPackage
{
    /// <summary>
    /// 实现这个接口，就可以作为Item被<see cref="ALUGUIWndVerticalMultiSizeLayout{T}"/>使用
    /// </summary>
    public interface _IALBasicMultiSizeLayoutItem
    {
        /// <summary>
        /// wnd是否在视野中
        /// </summary>
        /// <remarks>
        /// 需要注意，这个属性应该根据下面的outViewport方法和inViewport方法，正确的反应这两个方法调用之后的状态
        /// </remarks>
        bool isInViewport { get; }

        /// <summary>
        /// 返回layoutPos的值
        /// </summary>
        /// <remarks>
        /// 在这里应该将setLayoutPos中赋予的值原封不动的返回
        /// </remarks>
        float layoutPosY { get; }

        /// <summary>
        /// 退出视野时将被调用，应该在这里设置isInViewport属性为false
        /// </summary>
        void outViewport();
        /// <summary>
        /// 进入视野时将被调用，应该在这里设置isInViewport属性为true
        /// </summary>
        void inViewport();

        /// <summary>
        /// 返回这个Item的高度
        /// </summary>
        float getHeight();

        /// <summary>
        /// 设置wnd的坐标，layout会自动调用这个方法
        /// </summary>
        /// <param name="_itemPos">传入的pos为wnd最上端的点在content容器中的localPosition的y值</param>
        void setLayoutPos(float _itemPos);
    }
}