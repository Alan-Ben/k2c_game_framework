using System;
using ALPackage;

namespace GOE
{
    public class ConsortChatCacheSetting
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