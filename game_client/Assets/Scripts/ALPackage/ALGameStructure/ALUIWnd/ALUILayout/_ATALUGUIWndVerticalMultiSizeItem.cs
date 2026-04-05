
using UnityEngine;

namespace ALPackage
{
    /// <summary>
    /// 这个类作为<c>ALUGUIWndVerticalMultiSizeLayout</c>的Item控制类来使用
    /// </summary>
    /// <remarks>
    /// 为了要使用<see cref="ALUGUIWndVerticalMultiSizeLayout{T}"/>，你必须实现这个类，并作为_T_ITEM_WND来配合Layout使用
    /// <para>你必须实现下面的方法：</para>
    ///     <list type="bullet">
    ///         <item>
    ///             <term>getHeight</term>
    ///             <description>
    ///             需要告知Layout，这个wnd的高度为多少
    ///             </description>
    ///         </item>
    ///     </list>
    /// <para>你可以实现下面的方法，或者直接空着：</para>
    ///     <list type="bullet">
    ///         <item>
    ///             <term>inViewport</term>
    ///             <description>
    ///             这个方法会在wnd进入视野的时候触发，你可以在这里才加载wnd
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>outViewport</term>
    ///             <description>
    ///             这个方法会在wnd退出视野的时候触发，你可以在这里回收wnd
    ///             </description>
    ///         </item> 
    ///         <item>
    ///             <term>setLayoutPos</term>
    ///             <description>
    ///             你可以重载覆盖这个方法，自定义修改根据layoutPos和alignment来设置wnd坐标的方法
    ///             </description>
    ///         </item>
    ///     </list>
    /// <para>你需要注意：</para>
    ///     <list type="bullet">
    ///         <item>
    ///             <term>_onWndInitDone</term>
    ///             <description>
    ///             这个方法在这里拥有逻辑，会在initDone时赋值一次layoutPos给wnd，如果要重载，还需要考虑这一点
    ///             </description>
    ///         </item>
    ///     </list>
    /// </remarks>
    /// <typeparam name="T">该wnd对应的Mono</typeparam>
    public abstract class _ATALUGUIWndVerticalMultiSizeItem<T> : _ATALBasicLoadUIWnd<T>, _IALBasicMultiSizeLayoutItem
        where T : _AALBasicUIWndMono
    {
        /// <summary>
        /// wnd是否在视野中
        /// </summary>
        public bool isInViewport { get { return _m_bIsInViewport; } }

        /// <summary>
        /// <para>在wnd的容器里的PosY</para>
        /// <para>如果是上对齐，pos为最上端的点在content容器中的localPosition的y值</para>
        /// </summary>
        public float layoutPosY { get { return _m_fLayoutPosY; } }

        // 标识这个wnd是否在viewport中
        private bool _m_bIsInViewport;

        // 在wnd的容器里的PosY
        // 如果是上对齐，pos为wnd最上端的点在content容器中的localPosition的y值
        // 如果是下对齐，pos为wnd最下端的点在content容器中的localPosition的y值
        private float _m_fLayoutPosY;

        /// <summary>
        /// 这个wnd的高度
        /// </summary>
        public abstract float getHeight();

        /// <summary>
        /// 当wnd进入视野时调用
        /// </summary>
        protected abstract void _onInViewport();
        /// <summary>
        /// 当wnd退出视野时调用
        /// </summary>
        protected abstract void _onOutViewport();

        /// <summary>
        /// 在wnd加载完成时赋值一次坐标
        /// </summary>
        protected override void _onWndInitDone()
        {
            setLayoutPos(_m_fLayoutPosY);
        }

        /// <summary>
        /// 设置wnd的坐标，layout会自动调用这个方法
        /// </summary>
        public virtual void setLayoutPos(float _itemPos)
        {
            // 传入的pos为wnd最上端的点在content容器中的localPosition的y值
            _m_fLayoutPosY = _itemPos;

            if (wnd == null)
                return;

            // 这里直接把posY值赋值到localPosition上，而不是anchoredPosition，如果有特殊的需要，可以重载这个方法
            Vector3 oriPos = wnd.transform.localPosition;
            oriPos.y = _m_fLayoutPosY;
            wnd.transform.localPosition = oriPos;
        }

        /// <summary>
        /// 当wnd进入视野时调用
        /// </summary>
        public void inViewport()
        {
            _m_bIsInViewport = true;

            _onInViewport();
        }

        /// <summary>
        /// 当wnd退出视野时调用
        /// </summary>
        public void outViewport()
        {
            _m_bIsInViewport = false;

            _onOutViewport();
        }
    }
}