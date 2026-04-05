using System;
using ALPackage;
using UnityEngine;

/*********************
 * 全局对Vedio播放进行管理的管理类
 * 本管理类管理的是所有的Vedio播放的处理类，播放的操作由另外的对象进行处理
 **/
namespace ALPackage
{
    public class ALVideoPlayerMgr
    {
        private static ALVideoPlayerMgr _g_instance = new ALVideoPlayerMgr();
        public static ALVideoPlayerMgr instance 
        {
            get 
            {
                if(null == _g_instance)
                    _g_instance = new ALVideoPlayerMgr();

                return _g_instance; 
            }
        }

        //根节点对象，用于保存所有videoPlayer脚本对象
        private GameObject _m_tRootGo;
        private Transform _m_tRootTrans;

        //对使用的videoPlayer对象 clip资源进行缓存管理
        protected internal ALVideoPlayerCacheController _m_vcVideoPlayerCacheController;
        //对使用的videoPlayer对象 url资源 进行缓存管理
        protected internal ALVideoPlayerCacheController _m_vcVideoURLPlayerCacheController;
#if AL_AVPRO_V2
        //对使用的videoPlayer对象进行缓存管理
        protected internal ALVideoPlayerCacheController _m_vcVideoAVProPlayerCacheController;
        //AVPro加载准备处理队列
        protected internal ALVideoPlayerAVProPrepareQueue _m_avqAVProPrepareQueue;
        // AVPro安卓播放API
        protected internal RenderHeads.Media.AVProVideo.Android.VideoApi _m_avProAndroidVideoApi;
#endif

        public ALVideoPlayerMgr()
        {
        }

        /// <summary>
        /// 调用的初始化函数
        /// </summary>
        /// <param name="playerType">播放器类型</param>
        /// <param name="_avProAndroidVideoApi">AVPro安卓播放API</param>
        public void init(
#if AL_AVPRO_V2
            RenderHeads.Media.AVProVideo.Android.VideoApi _avProAndroidVideoApi = RenderHeads.Media.AVProVideo.Android.VideoApi.ExoPlayer
#endif
            )
        {
            if (null != _m_tRootGo)
            {
                UnityEngine.Debug.LogError("ALVideoPlayerMgr Init Error, _m_tRootGo is not null");
                return;
            }

            _m_tRootGo = new GameObject("video_player_root");
            _m_tRootTrans = _m_tRootGo.transform;
            GameObject.DontDestroyOnLoad(_m_tRootGo);

            _m_vcVideoPlayerCacheController = new ALVideoPlayerCacheController((Transform parent, string name) => { return new ALVideoPlayerDealerUnity(parent, name); }
                                                    , 4, 32, _m_tRootTrans);
            
            _m_vcVideoURLPlayerCacheController = new ALVideoPlayerCacheController((Transform parent, string name) => { return new ALVideoPlayerDealerUnityURL(parent, name); }
                                                    , 4, 32, _m_tRootTrans);
#if AL_AVPRO_V2
            _m_avProAndroidVideoApi = _avProAndroidVideoApi;
            _m_avqAVProPrepareQueue = new ALVideoPlayerAVProPrepareQueue();
            _m_vcVideoAVProPlayerCacheController = new ALVideoPlayerCacheController((Transform parent, string name) => { return new ALVideoPlayerDealerAVPro(parent, name); }
                                                    , 4, 32, _m_tRootTrans);

            //开启AVPro的Prepare处理队列
            _m_avqAVProPrepareQueue.init();
#endif
        }

        /// <summary>
        /// 创建一个播放对象
        /// </summary>
        /// <returns></returns>
        public ALVideoPlayer createVideoPlayer(Material _renderMat)
        {
            if(null == _m_tRootGo)
            {
                UnityEngine.Debug.LogError("createVideoPlayer Error, _m_tRootGo is null");
                return null;
            }

            return new ALVideoPlayer(_renderMat);
        }
    }
}
