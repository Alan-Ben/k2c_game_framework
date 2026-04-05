using System;
using UnityEngine;
using UnityEngine.Video;

namespace ALPackage
{
    /**************************************************
     *   定义宏说明：
     *   AL_SQLITE - 是否使用Sqlite数据库，需要引入对应Sqlite的库
     *   
     *   AL_LUA - 是否使用XLUA脚本，需要引入XLua相关库
     *   AL_PUERTS - 是否使用PUERTS脚本，需要引入Puerts相关库
     *   
     *   AL_UNITY_GUI - 是否使用原生的UnityGUI机制，此宏不定义可以省略一些GUI机制的调用成本
     *   AL_SEVEN_ZIP - 是否使用7Zip压缩机制，一般用于加解密或者自带的7Zip资源包压缩（不推荐）
     *   
     *   AL_TEXT_MESH_PRO - 是否对TextMeshPro提供接口支持
     *   
     *   AL_ILRUNTIME - 是否使用ILRuntime机制，可用于热更代码和脚本
     *   AL_ILRUNTIME_V2 - ILRuntime2.0版本的Mono库引用改变，如果使用2.0版本以上ILRuntime则需要附加这个宏定义确保编译通过
     *   
     *   AL_CREATURE_SYS - 是否使用底层支持的基于Animation的一套角色控制体系，包括事件机制等等。机制基于旧系统研发，新系统不建议使用
     *   
     ***************************************/

    public class ALGlobalControl
    {
        private static ALGlobalControl g_instance = new ALGlobalControl();

        public static ALGlobalControl instance
        {
            get
            {
                if (null == g_instance)
                    g_instance = new ALGlobalControl();

                return g_instance;
            }
        }

        /** 地图数据层蒙版数据 */
        public int mapItemLayerMask = 0;

        /// <summary>
        /// 新的视频播放错误回调 (支持所有播放器类型)
        /// </summary>
        public event Action<_IALVideoResource, _AALVideoPlayerDealer, string> onVideoErrorReceived;
        
        protected ALGlobalControl()
        {
        }

        /// <summary>
        /// 新的视频播放错误回调 (支持所有播放器类型)
        /// </summary>
        protected internal void _onVideoErrorReceived(_IALVideoResource _resource, _AALVideoPlayerDealer _core, string _msg)
        {
            if (onVideoErrorReceived != null) 
                onVideoErrorReceived(_resource, _core, _msg);
        }
    }
}
