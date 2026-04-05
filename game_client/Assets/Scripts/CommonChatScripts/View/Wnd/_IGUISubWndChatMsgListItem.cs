
using System;
using ALPackage;

namespace ChatPackage
{
    /// <summary>
    /// <see cref="GUISubWndChatMsgList"/>所使用的item的接口
    /// </summary>
    /// <remarks>
    /// 这个接口其实是冗余的，没有什么特别的意义，这个接口是因为<see cref="_AGUISubWndChatMsgListItem{T_MONO, T_DATA}"/>代有泛型，无法作为一个统一的抽象进行储存，如果C#可以在接口中写逻辑，或是多重继承就可以避免
    /// </remarks>
    public interface _IGUISubWndChatMsgListItem : _IALBasicMultiSizeLayoutItem
    {
        /// <summary>
        /// 当item的高度发生了变化的事件
        /// </summary>
        event Action onHeightChg;
        /// <summary>
        /// 加载模板对象
        /// </summary>
        void loadTemplate();
        /// <summary>
        /// 释放模板对象
        /// </summary>
        void discardTemplate();
    }
}