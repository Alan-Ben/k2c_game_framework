using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
#if AL_XLUA
    [XLua.LuaCallCSharp]
#endif
    /// <summary>
    /// 全局注册动画控制对象的管理器
    /// </summary>
    public class AnimatorControllerMgr
    {
        private static AnimatorControllerMgr _g_instance;
        [NotNull]public static AnimatorControllerMgr instance
        {
            get
            {
                if(_g_instance == null) {
                    _g_instance = new AnimatorControllerMgr();
                }
                return _g_instance;
            }
        }
        
        
        [NotNull]private Dictionary<string, List<_IAnimatorControlRegInfo>> _m_animationMonoDict = new Dictionary<string, List<_IAnimatorControlRegInfo>>();

        /// <summary>
        /// 注册
        /// </summary>
        public void regMono<T>(_ATAniControlMono<T> _mono) where T : _IAnimatorControlRegInfo, _IAnimatorControlGroupRegInfo
        {
            if (null == _mono || null == _mono.dataList)
                return;

            _mono.dataList.ForEach(_data =>
            {
                regMono(_data);
            });
        }

        /// <summary>
        /// 注册
        /// </summary>
        public void regMono(_IAnimatorControlRegInfo _regInfo)
        {
            if(null == _regInfo || string.IsNullOrEmpty(_regInfo.controlNameTag))
                return;
            
            List<_IAnimatorControlRegInfo> list = __safeGetItem(_regInfo.controlNameTag);

            if(null != list)
                list.Add(_regInfo);
        }

        /// <summary>
        /// 删除
        /// </summary>
        public void unregMono<T>(_ATAniControlMono<T> _mono) where T : _IAnimatorControlRegInfo, _IAnimatorControlGroupRegInfo
        {
            if (null == _mono || null == _mono.dataList)
                return;

            _mono.dataList.ForEach(_data =>
            {
                unregMono(_data);
            });
        }

        /// <summary>
        /// 删除
        /// </summary>
        public void unregMono(_IAnimatorControlRegInfo _regInfo)
        {
            if(null == _regInfo || string.IsNullOrEmpty(_regInfo.controlNameTag))
                return;
            
            List<_IAnimatorControlRegInfo> list = __getItem(_regInfo.controlNameTag);

            if (null != list)
            {
                list.Remove(_regInfo);
                //判断是否需要删除队列
                if(list.Count <= 0)
                    _m_animationMonoDict.Remove(_regInfo.controlNameTag);
            }
        }

        #region 控制接口
        /// <summary>
        /// 开始播放
        /// </summary>
        public void playAnimator(string _name, string _stateName, float _normalizedTime)
        {
            List<_IAnimatorControlRegInfo> list = __getItem(_name);
            if(null == list)
                return;

            _IAnimatorControlRegInfo tmpInfo = null;
            for(int i = 0; i < list.Count; i++)
            {
                tmpInfo = list[i];
                if(null == tmpInfo)
                    continue;

                tmpInfo.playAnimator(_stateName, _normalizedTime);
            }
        }
        
        /// <summary>
        /// 设置动画，强行切换，一般只有视频动画才会使用
        /// </summary>
        public void setAni(string _name, string _stateName)
        {
            List<_IAnimatorControlRegInfo> list = __getItem(_name);
            if(null == list)
                return;

            _IAnimatorControlRegInfo tmpInfo = null;
            for(int i = 0; i < list.Count; i++)
            {
                tmpInfo = list[i];
                if(null == tmpInfo)
                    continue;

                tmpInfo.setAni(_stateName);
            }
        }
        

        /// <summary>
        /// 设置属性
        /// </summary>
        public void setBool(string _name, string _variableName, bool _value)
        {
            List<_IAnimatorControlRegInfo> list = __getItem(_name);
            if(null == list)
                return;

            _IAnimatorControlRegInfo tmpInfo = null;
            for(int i = 0; i < list.Count; i++)
            {
                tmpInfo = list[i];
                if(null == tmpInfo)
                    continue;

                tmpInfo.setBool(_variableName, _value);
            }
        }
        public void setFloat(string _name, string _variableName, float _value)
        {
            List<_IAnimatorControlRegInfo> list = __getItem(_name);
            if(null == list)
                return;

            _IAnimatorControlRegInfo tmpInfo = null;
            for(int i = 0; i < list.Count; i++)
            {
                tmpInfo = list[i];
                if(null == tmpInfo)
                    continue;

                tmpInfo.setFloat(_variableName, _value);
            }
        }
        public void setInteger(string _name, string _variableName, int _value)
        {
            List<_IAnimatorControlRegInfo> list = __getItem(_name);
            if(null == list)
                return;

            _IAnimatorControlRegInfo tmpInfo = null;
            for(int i = 0; i < list.Count; i++)
            {
                tmpInfo = list[i];
                if(null == tmpInfo)
                    continue;

                tmpInfo.setInteger(_variableName, _value);
            }
        }

        /// <summary>
        /// 设置动画播放速度
        /// </summary>
        /// <param name="_name"></param>
        /// <param name="_speed"></param>
        public void setSpeed(string _name, float _speed)
        {
            List<_IAnimatorControlRegInfo> list = __getItem(_name);
            if(null == list)
                return;

            _IAnimatorControlRegInfo tmpInfo = null;
            for(int i = 0; i < list.Count; i++)
            {
                tmpInfo = list[i];
                if(null == tmpInfo)
                    continue;

                tmpInfo.setSpeed(_speed);
            }
        }

        #endregion

        /// <summary>
        /// 安全的获取函数，必定返回一个列表
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        private List<_IAnimatorControlRegInfo> __safeGetItem(string _name)
        {
            if(null == _name)
                return null;

            List<_IAnimatorControlRegInfo> list = null;
            if(!_m_animationMonoDict.TryGetValue(_name, out list))
            {
                list = new List<_IAnimatorControlRegInfo>();
                _m_animationMonoDict.Add(_name, list);
            }

            return list;
        }
        private List<_IAnimatorControlRegInfo> __getItem(string _name)
        {
            if(null == _name)
                return null;
            
            List<_IAnimatorControlRegInfo> list = null;
            if(!_m_animationMonoDict.TryGetValue(_name, out list))
            {
                return null;
            }

            return list;
        }
    }
}