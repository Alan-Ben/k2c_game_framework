
using System;
using ALPackage;

namespace ChatPackage.Internal
{
    /// <summary>
    /// 缓存池使用的设置类
    /// </summary>
    /// <remarks>
    /// 这个设置类被缓存池管理器<see cref="GUICacheMgrChatMsgItem"/>所使用，存放着对应msgType的相关资源内容
    /// </remarks>
    public class GUICacheMgrChatMsgItemSetting
    {
        /// <summary>
        /// 对应的mono类型
        /// </summary>
        public Type monoType { get; internal set; }
        /// <summary>
        /// 对应的wnd类型
        /// </summary>
        public Type wndType { get; internal set; }
        /// <summary>
        /// 对应的数据类型
        /// </summary>
        public Type dataType { get; internal set; }
        /// <summary>
        /// 对应prefab的资源路径
        /// </summary>
        public string assetPath { get; internal set; }
        /// <summary>
        /// 对应prefab的资源名
        /// </summary>
        public string objName { get; internal set; }
        /// <summary>
        /// 对应的resourceCore
        /// </summary>
        public _AALResourceCore resourceCore { get; internal set; }
        /// <summary>
        /// 最小缓存数量
        /// </summary>
        public int minCacheCount { get; internal set; }
        /// <summary>
        /// 最大缓存数量，超过了会报警，并在回收时直接销毁
        /// </summary>
        public int maxCacheCount { get; internal set; }
    }
}