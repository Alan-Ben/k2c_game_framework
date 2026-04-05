using System;
using ALPackage;
using UnityEngine.AI;
using UnityEngine.Audio;

namespace GOE
{
    /// <summary>
    /// NP混音器加载对象
    /// </summary>
    public class AudioMixerLoader : _ATALBasicSingleAssetObj<AudioMixer>
    {
        private string _m_resPath;
        private string _m_objName;
        
        public AudioMixerLoader(string _resPath, string _objName)
        {
            _m_resPath = _resPath;
            _m_objName = _objName;
        }
        
        /** 获取资源管理对象 */
        protected override _AALResourceCore _resCore { get { return GameResCore.instance; } }
        /** 获取加载路径 */
        protected override string _resPath { get { return _m_resPath; } }
        /** 获取加载对象名 */
        protected override string _objName { get { return _m_objName; } }
    }
}