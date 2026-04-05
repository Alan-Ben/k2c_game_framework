
using UnityEngine;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace ALPackage
{
    /// <summary>
    /// 竖直方向滚动，可支持item的高度各不相同的列表。
    /// </summary>
    /// <remarks>
    ///     需要注意：
    ///     <para>1.这个类内部不含循环滚动，不含有任何cache</para>
    ///     <para>2.但是可以触发item进出viewport的事件，可以在事件里进行加载和回收实现循环滚动</para>
    ///     <para>3.这个类会修改wnd上的ScrollRect的content的pivot和anchor</para>
    ///     <para>主要方法：</para>
    ///     <list type="bullet">
    ///         <item>
    ///             <term>Wnd的通用操作</term>
    ///             <description>
    ///             可以正常使用showWnd，hideWnd之类的wnd操作来控制窗口
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>refresh</term>
    ///             <description>
    ///             使用refresh方法传入所有需要显示的Item即可，可以传入null，就可以吧列表清空
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>setAlignment</term>
    ///             <description>
    ///             可以在Runtime中改变Item的对齐方式，目前支持的有上对齐和下对齐两种
    ///             </description>
    ///         </item> 
    ///     </list>
    /// </remarks>
    /// <typeparam name="T">列表自己容器的mono类型</typeparam>
    public class ALUGUIWndVerticalMultiSizeLayout<T> : _ATALBasicUISubWnd<T>, _IALBasicRefreshUIWndInterface
        where T : ALUGUIMonoVerticalMultiSizeLayout
    {
        // 需要显示的所有item，包括不在视野范围内的
        [NotNull] protected List<_IALBasicMultiSizeLayoutItem> _m_lItemList = new List<_IALBasicMultiSizeLayoutItem>();
        
        // update任务的序列数
        private int _m_iShowOpSerialize;

#if UNITY_EDITOR
        // mono是否合法
        private bool _m_bIsLegal;
        // 用来实现mono上修改spacing可以实时刷新的功能
        private float _m_fLastSpacing;
        // 用来实现mono上修改topPadding可以实时刷新的功能
        private float _m_fLastTopPadding;
        // 用来实现mono上修改bottomPadding可以实时刷新的功能
        private float _m_fLastBottomPadding;
#endif
        // 对齐方式
        private EALVerticalLayoutChildAlignment _m_eAlignment;
        // Item的容器
        private RectTransform _m_rTransItemContainer;
        // 列表的视口Transform
        private RectTransform _m_rTransItemViewport;
        // 上次刷新时content的坐标Y值
        private float _m_fLastContainerPosY;
        // 是否需要无视有没有发生滚动而强制刷新Item进出viewport的情况
        private bool _m_bNeedForceRefreshItemShow;

        /// <summary>
        /// 使用容器的Mono构造该方法
        /// </summary>
        protected ALUGUIWndVerticalMultiSizeLayout(T _wnd)
            : base(_wnd)
        {
#if UNITY_EDITOR
            // Editor环境下检查mono设置的是否合法
            _m_bIsLegal = _legalCheck();
            if (!_m_bIsLegal)
                return;
#endif

            // 赋值到本地
            _m_rTransItemContainer = _wnd.scrollRect.content;
            _m_rTransItemViewport = _wnd.scrollRect.viewport;
            _m_eAlignment = wnd.alignment;

            // 设置
            _m_rTransItemContainer.pivot = new Vector2(_m_rTransItemContainer.pivot.x, _m_eAlignment == EALVerticalLayoutChildAlignment.Up ? 1 : 0);
            _m_rTransItemContainer.anchorMin = new Vector2(_m_rTransItemContainer.anchorMin.x, _m_eAlignment == EALVerticalLayoutChildAlignment.Up ? 1 : 0);
            _m_rTransItemContainer.anchorMax = new Vector2(_m_rTransItemContainer.anchorMax.x, _m_eAlignment == EALVerticalLayoutChildAlignment.Up ? 1 : 0);

            _m_iShowOpSerialize = ALSerializeOpMgr.next();
        }

        /// <summary>
        /// 数据的总个数
        /// </summary>
        public int itemCount { get { return _m_lItemList == null ? 0 : _m_lItemList.Count; } }
        /// <summary>
        /// 对齐方向
        /// </summary>
        public EALVerticalLayoutChildAlignment alignment { get { return _m_eAlignment; } }

        /// <summary>
        /// update任务的序列数
        /// </summary>
        public int showOpSerialize { get { return _m_iShowOpSerialize; } }
        /// <summary>
        /// item的父对象
        /// </summary>
        public Transform itemContainer { get { return _m_rTransItemContainer; } }
        /// <summary>
        /// 当前内容的高度
        /// </summary>
        public float contentHeight
        {
            get
            {
                if (_m_rTransItemContainer == null)
                    return 0;
                return _m_rTransItemContainer.sizeDelta.y;
            }
        }
        /// <summary>
        /// 当前视口的高度 
        /// </summary>
        public float viewportHeight
        {
            get
            {
                if (_m_rTransItemViewport == null)
                    return 0;
                return _m_rTransItemViewport.rect.height;
            }
        }

        public sealed override void showWnd()
        {
            base.showWnd();

#if UNITY_EDITOR
            if (!_m_bIsLegal)
                return;
#endif
            _m_iShowOpSerialize = ALSerializeOpMgr.next();

            //开启刷新任务
            ALMonoTaskMgr.instance.addNextFrameTask(new ALUGUIRefreshMonoTask(this));
        }
        public sealed override void hideWnd()
        {
            base.hideWnd();

#if UNITY_EDITOR
            if (!_m_bIsLegal)
                return;
#endif
            _m_iShowOpSerialize = ALSerializeOpMgr.next();
        }
        public sealed override void resetWnd()
        {
            _clearAll();

            base.resetWnd();
        }
        public sealed override void discard()
        {
            _clearAll();

            base.discard();
        }

        /// <summary>
        /// 每帧刷新，Grid的主要方法，由task调用
        /// </summary>
        public void frameRefresh()
        {
#if UNITY_EDITOR
            if (!_m_bIsLegal)
                return;

            // 如果在编辑器下，spacing，topPadding，bottomPadding发生变化后，就进行容器的刷新
            if (wnd != null && 
                (!Mathf.Approximately(_m_fLastSpacing, wnd.spacing) ||
                 !Mathf.Approximately(_m_fLastTopPadding, wnd.topPadding) ||
                 !Mathf.Approximately(_m_fLastBottomPadding, wnd.bottomPadding)))
            {
                // 记录spacing，用来比对是否发生变化
                _m_fLastSpacing = wnd.spacing;
                // 记录TopPadding，用来比对是否发生变化
                _m_fLastTopPadding = wnd.topPadding;
                // 记录bottomPadding，用来比对是否发生变化
                _m_fLastBottomPadding = wnd.bottomPadding;
                // 根据传入的ItemList，刷新容器的Size，让容器的Height足够容纳所有的Item，并讲Item设置到正确的位置上
                _refreshContentSizeAndItemPos();
            }
#endif
            // 刷新Item进出viewport的情况
            _refreshItemShow();
            //在每帧刷新的时候调用的事件函数
            _onFrameRefresh();
        }

        /// <summary>
        /// 每帧刷新的时候调用的事件函数，子类有需求则重写
        /// </summary>
        protected virtual void _onFrameRefresh()
        { }


        /// <summary>
        /// 设置需要显示的Item列表
        /// </summary>
        public void refresh(List<_IALBasicMultiSizeLayoutItem> _itemList)
        {
#if UNITY_EDITOR
            if (!_m_bIsLegal)
                return;
#endif
            _m_lItemList.Clear();
            if (_itemList != null)
                _m_lItemList.AddRange(_itemList);

            // 根据传入的ItemList，刷新容器的Size，让容器的Height足够容纳所有的Item，并讲Item设置到正确的位置上
            _refreshContentSizeAndItemPos();
        }

        /// <summary>
        /// 可以在Runtime中改变Item的对齐方式，目前支持的有上对齐和下对齐两种
        /// </summary>
        public void setAlignment(EALVerticalLayoutChildAlignment _alignment)
        {
            if (_m_eAlignment == _alignment)
                return;

            _m_eAlignment = _alignment;
            
            // 根据传入的新的对齐方式，设置对应的属性
            _m_rTransItemContainer.pivot = new Vector2(_m_rTransItemContainer.pivot.x, _m_eAlignment == EALVerticalLayoutChildAlignment.Up ? 1 : 0);
            _m_rTransItemContainer.anchorMin = new Vector2(_m_rTransItemContainer.anchorMin.x, _m_eAlignment == EALVerticalLayoutChildAlignment.Up ? 1 : 0);
            _m_rTransItemContainer.anchorMax = new Vector2(_m_rTransItemContainer.anchorMax.x, _m_eAlignment == EALVerticalLayoutChildAlignment.Up ? 1 : 0);

            // 根据传入的ItemList，刷新容器的Size，让容器的Height足够容纳所有的Item，并讲Item设置到正确的位置上
            _refreshContentSizeAndItemPos();
        }

        /// <summary>
        /// 容器size发生变化的可自定义方法
        /// </summary>
        /// <param name="_beforeHeight">容器之前的大小</param>
        protected virtual void _onRefreshContentSize(float _beforeHeight) { }
        /// <summary>
        /// item的位置刷新完毕之后的可自定义方法
        /// </summary>
        protected virtual void _onRefreshItemPos() { }

        // wnd基本方法重载
        protected override void _onShowWnd() { }
        protected override void _onHideWnd() { }
        protected override void _onReset() { }
        protected override void _onDiscard() { }
        protected override void _onWndInitDone() { }

        // 清空所有内容
        private void _clearAll()
        {
            if (_m_lItemList != null)
            {
                for (int i = 0; i < _m_lItemList.Count; i++)
                {
                    _IALBasicMultiSizeLayoutItem item = _m_lItemList[i];
                    if (item == null)
                        continue;

                    if (item.isInViewport)
                        item.outViewport();
                }
                _m_lItemList.Clear();
            }
        }

        // 根据传入的ItemList，刷新容器的Size，让容器的Height足够容纳所有的Item，并讲Item设置到正确的位置上
        private void _refreshContentSizeAndItemPos()
        {
            // 开始计算content的高度
            float contentHeight = 0;
            // 如果list不为空，才进行计算，否则高度直接当成0
            if (_m_lItemList != null && _m_lItemList.Count > 0)
            {
                // 遍历所有的Item，将item的高度加起来，并且加上每个Item之间的spacing
                for (int i = 0; i < _m_lItemList.Count; i++)
                {
                    _IALBasicMultiSizeLayoutItem item = _m_lItemList[i];
                    // 如果是null就跳过
                    if (item == null)
                        continue;

                    // 将item的高度加起来，并且加上每个Item之间的spacing
                    contentHeight += item.getHeight() + wnd.spacing;
                }

                // 如果上面计算出来的height为0，说明列表里都是null，下面的计算就不做了
                if (contentHeight != 0)
                {
                    contentHeight -= wnd.spacing; // 减掉最后多加的一条空隙
                    contentHeight += wnd.topPadding; // 加上上端的偏移值
                    contentHeight += wnd.bottomPadding; // 加上下端的偏移值
                }
            }

            // 记录之前的高度，来触发后面的size变化的事件
            float beforeHeight = _m_rTransItemContainer.rect.height;
            // 赋值size给content，content的x并不会被控制，可以是任意值
            _m_rTransItemContainer.sizeDelta = new Vector2(_m_rTransItemContainer.sizeDelta.x, contentHeight);
            // 触发容器size发生变化的可自定义方法
            _onRefreshContentSize(beforeHeight);

            // 容器Size部分至此结束，下面开始更新itemPos的位置

            // 列表不为空才进行这里的处理
            if (_m_lItemList != null && _m_lItemList.Count > 0)
            {
                // 用来累计计算Item的配置
                float itemPos = 0;
                // 根据对齐方向，Item的位置累加的符号
                int posPlusSign = alignment == EALVerticalLayoutChildAlignment.Up ? -1 : 1;
                // 第一个偏移值，如果是上对齐就是topPadding，如果是下对齐就是bottomPadding
                float firstPadding = alignment == EALVerticalLayoutChildAlignment.Up ? wnd.topPadding : wnd.bottomPadding;
                // 如果是上对齐，那么从第一个item开始赋值，如果是下对齐就从最后一个item进行赋值
                int startIndex = alignment == EALVerticalLayoutChildAlignment.Up ? 0 : _m_lItemList.Count - 1;

                // 进行外边缘的偏移
                itemPos += firstPadding * posPlusSign;
                // 遍历所有的Item，并给上正确的pos
                for (int i = startIndex; i < _m_lItemList.Count && i >= 0; i -= posPlusSign)
                {
                    _IALBasicMultiSizeLayoutItem item = _m_lItemList[i];
                    // 如果是null就跳过
                    if (item == null)
                        continue;

                    // 计算item的高度
                    float itemHeight = item.getHeight();
                    // 计算根据对齐方式，坐标的偏移
                    float offset = alignment == EALVerticalLayoutChildAlignment.Up ? 0 : itemHeight;

                    // 设置item的pos
                    // 传入的pos为每个Item最上端的点在content容器中的localPosition的y值
                    item.setLayoutPos(itemPos + offset);
                    // 累加itemPos，计算下一个Item的位置
                    itemPos += (itemHeight + wnd.spacing) * posPlusSign;
                }
            }
            // 触发Item的位置发生了变化的自定义方法
            _onRefreshItemPos();

            // 既然调用了这个方法，那么认为Item的情况可能发生了变化，通知itemShow不管滚动有没有发生变化都进行强行刷新
            _m_bNeedForceRefreshItemShow = true;
        }

        // 刷新Item进出viewport的情况
        private void _refreshItemShow()
        {
            // 如果需要强制刷新，或是ScrollRect发生了滚动，就进行刷新，ScrollRect的滚动会体现在content的坐标变化上
            if (!_m_bNeedForceRefreshItemShow && Mathf.Approximately(_m_rTransItemContainer.anchoredPosition.y, _m_fLastContainerPosY))
                return;

            // 记录这次刷新的content的坐标，来判定是否发生了滚动
            _m_fLastContainerPosY = _m_rTransItemContainer.anchoredPosition.y;
            // 强制刷新标记取消，让这个标记只会让这个方法刷新一次
            _m_bNeedForceRefreshItemShow = false;

            // 如果列表为空，后面的计算都没必要做了
            if (_m_lItemList == null || _m_lItemList.Count <= 0)
                return;

            // 视野区域的最顶端在content里的坐标
            float viewTop;
            // 视野区域的最低端在content里的坐标
            float viewBottom;

            // 为上面两个值赋值
            if (_m_eAlignment == EALVerticalLayoutChildAlignment.Up)
            {
                viewTop = -_m_rTransItemContainer.anchoredPosition.y;
                viewBottom = viewTop - _m_rTransItemViewport.rect.height;
            }
            else // alignment == EALVerticalLayoutChildAlignment.Down
            {
                viewBottom = -_m_rTransItemContainer.anchoredPosition.y;
                viewTop = viewBottom + _m_rTransItemViewport.rect.height;
            }

            // 遍历所有内容
            for (int i = 0; i < _m_lItemList.Count; i++)
            {
                _IALBasicMultiSizeLayoutItem item = _m_lItemList[i];
                // 如果是null就跳过
                if (item == null)
                    continue;

                // 判断这个Item是否在Viewport内
                if (_checkItemInViewport(item, viewTop, viewBottom))
                {
                    // 如果在Viewport内，并且没有设置为在Viewport内，就做相应操作
                    if (!item.isInViewport)
                        item.inViewport();
                }
                else // 不在Viewport内
                {
                    // 如果不在Viewport内，并且已经被设置为在Viewport内，就做相应操作
                    if (item.isInViewport)
                        item.outViewport();
                }
            }
        }

        // 判断item是否在viewport内
        private bool _checkItemInViewport(_IALBasicMultiSizeLayoutItem _item, float _viewportTopLayoutPos, float _viewportBottomLayoutPos)
        {
            // 计算自身的顶端和低端，因为
            // 如果是上对齐，pos为wnd最上端的点在content容器中的localPosition的y值
            // 如果是下对齐，pos为wnd最下端的点在content容器中的localPosition的y值
            // 这里根据这一点计算wnd的顶端和低端的pos
            float itemTop, itemBottom;
            if (_m_eAlignment == EALVerticalLayoutChildAlignment.Up)
            {
                itemTop = _item.layoutPosY;
                itemBottom = itemTop - _item.getHeight();
            }
            else
            {
                itemBottom = _item.layoutPosY;
                itemTop = itemBottom + _item.getHeight();
            }

            // 判断是否在视野中，这里注释说不清楚，简单来说就是一个一维的线段求交
            return (_viewportTopLayoutPos > itemBottom && _viewportTopLayoutPos < itemTop) ||
                   (_viewportBottomLayoutPos > itemBottom && _viewportBottomLayoutPos < itemTop) ||
                   (itemTop > _viewportBottomLayoutPos && itemTop <= _viewportTopLayoutPos);
        }

#if UNITY_EDITOR
        /// <summary>
        /// 检查Mono配置是否正确
        /// </summary>
        private bool _legalCheck()
        {
            if (wnd == null)
            {
                Debug.LogError("【ALUGUIWndVerticalMultiSizeLayout错误】传入的 wnd 为 null，请检查代码正确性和Mono丢失的情况");
                return false;
            }

            if (wnd.scrollRect == null || wnd.scrollRect.viewport == null || wnd.scrollRect.content == null)
            {
                Debug.LogError("【ALUGUIWndVerticalMultiSizeLayout错误】mono 上的 scrollRect 不可以是None，同时 scrollRect 的 viewport 和 content 都不可以为None");
                return false;
            }

            if (wnd.scrollRect.content.parent != wnd.scrollRect.viewport)
            {
                Debug.LogError("【ALUGUIWndVerticalMultiSizeLayout错误】scrollRect 的 viewport 必须是 content 的父对象");
                return false;
            }

            if (wnd.scrollRect.viewport.localScale != Vector3.one || wnd.scrollRect.content.localScale != Vector3.one)
            {
                Debug.LogError("【ALUGUIWndVerticalMultiSizeLayout错误】暂不支持 viewport 和 content 的 scale 非 1 ");
                return false;
            }

            return true;
        }
#endif
    }
}