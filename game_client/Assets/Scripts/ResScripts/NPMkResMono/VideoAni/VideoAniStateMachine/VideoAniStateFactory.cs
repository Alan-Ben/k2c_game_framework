using ALPackage;
using System;
using System.Collections;
using Unity.IO.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;

namespace GOE
{
    public partial class VideoAniStateMachine
    {
        /// <summary>
        /// 视频播放器的状态工厂
        /// 为了避免重复创建状态对象所以构建的数据对象
        /// </summary>
        public class VideoAniStateFactory : _ATALStateFactory<_AVideoAniBaseState, EVideoAniState>
        {
            private static VideoAniStateFactory _g_instance = new VideoAniStateFactory();
            public static VideoAniStateFactory instance
            {
                get
                {
                    if (null == _g_instance)
                        _g_instance = new VideoAniStateFactory();
                    return _g_instance;
                }
            }

            public VideoAniStateFactory() : base()
            {
                //逐个注册缓存管理器
                regCacheController(typeof(VideoAniNoneState), new VideoAniStateCache(() => { return new VideoAniNoneState(); }));
                regCacheController(typeof(VideoAniSingleClipState), new VideoAniStateCache(() => { return new VideoAniSingleClipState(); }));
                regCacheController(typeof(VideoAniEntryClipState), new VideoAniStateCache(() => { return new VideoAniEntryClipState(); }));
                regCacheController(typeof(VideoAniLoopClipState), new VideoAniStateCache(() => { return new VideoAniLoopClipState(); }));
                regCacheController(typeof(VideoAniExitClipState), new VideoAniStateCache(() => { return new VideoAniExitClipState(); }));
            }

            /// <summary>
            /// 视频相关状态的缓存对象，一般播放视频不会过多，所以最高缓存使用8个
            /// </summary>
            protected class VideoAniStateCache : _TALBasicStateCacheController<_AVideoAniBaseState, EVideoAniState>
            {
                public VideoAniStateCache(Func<_AVideoAniBaseState> _createFunc)
                    : base(_createFunc, 1, 8)
                {

                }
            }
        }
    }
}