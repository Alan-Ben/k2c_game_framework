using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
#if AL_XLUA
    [XLua.LuaCallCSharp]
#endif
    /// <summary>
    /// 全局注册动画控制对象的管理器
    /// </summary>
    public class AnimatorControllerGroupMgr : WCGSingleton<AnimatorControllerGroupMgr>
    {
        private Dictionary<string, List<_IAnimatorControlRegInfo>> _m_animationMonoDict = new Dictionary<string, List<_IAnimatorControlRegInfo>>();

        /// <summary>
        /// 注册
        /// </summary>
        public void regMono<GI_T, T>(_ATAniControlGroupMono<GI_T, T> _mono) where GI_T : _ATAniControlGroupRegItemInfo<T> where T : _IAnimatorControlRegInfo, _IAnimatorControlGroupRegInfo
        {
            //获取队列
            List<_IAnimatorControlRegInfo> list = __safeGetItem(_mono.controlGroupName);

            _mono.regItemList.ForEach(_data =>
            {
                regMono(_mono.controlGroupName, _data.controlMono, _data.siftName);
            });
        }
        public void regMono<T>(string _groupName, _ATAniControlMono<T> _mono) where T : _IAnimatorControlRegInfo, _IAnimatorControlGroupRegInfo
        {
            if(null == _mono)
                return;

            //获取队列
            List<_IAnimatorControlRegInfo> list = __safeGetItem(_groupName);

            _mono.dataList.ForEach(_data =>
            {
                if(null != list)
                {
                    //添加到队列
                    list.Add(_data);
                    //添加group名称
                    _data.addGroupName(_groupName);
                }
            });
        }
        public void regMono<T>(string _groupName, _ATAniControlMono<T> _mono, string _siftName) where T : _IAnimatorControlRegInfo, _IAnimatorControlGroupRegInfo
        {
            if(null == _mono)
                return;

            //获取队列
            List<_IAnimatorControlRegInfo> list = __safeGetItem(_groupName);

            _mono.dataList.ForEach(_data =>
            {
                //当数据无效或者过滤字符串不匹配则处理下一个
                if(null != _siftName && _siftName.Length > 0 && !string.Equals(_data.controlNameTag, _siftName, StringComparison.OrdinalIgnoreCase))
                    return;

                if(null != list)
                {
                    //添加到队列
                    list.Add(_data);
                    //添加group名称
                    _data.addGroupName(_groupName);
                }
            });
        }

        /// <summary>
        /// 删除
        /// </summary>
        public void unregMono<GI_T, T>(_ATAniControlGroupMono<GI_T, T> _mono) where GI_T : _ATAniControlGroupRegItemInfo<T> where T : _IAnimatorControlRegInfo, _IAnimatorControlGroupRegInfo
        {
            _mono.regItemList.ForEach(_data =>
            {
                unregMono(_data.controlMono);
            });
        }
        public void unregMono<T>(_ATAniControlMono<T> _mono) where T : _IAnimatorControlRegInfo, _IAnimatorControlGroupRegInfo
        {
            if(null == _mono)
                return;

            _mono.dataList.ForEach(_data =>
            {
                _data.disposeFromGroupMgr();
            });
        }
        public void unregMono<T>(string _groupName, _ATAniControlMono<T> _mono, string _siftName) where T : _IAnimatorControlRegInfo, _IAnimatorControlGroupRegInfo
        {
            if(null == _mono)
                return;

            //获取队列
            List<_IAnimatorControlRegInfo> list = __safeGetItem(_groupName);

            _mono.dataList.ForEach(_data =>
            {
                //当数据无效或者过滤字符串不匹配则处理下一个
                if(null != _siftName && _siftName.Length > 0 && !string.Equals(_data.controlNameTag, _siftName, StringComparison.OrdinalIgnoreCase))
                    return;

                if(null != list)
                {
                    //添加到队列
                    list.Remove(_data);
                    //删除group名称
                    _data._removeGroupName(_groupName);
                }
            });
        }
        /// <summary>
        /// 模块内部开放函数，不建议外部逻辑使用
        /// </summary>
        /// <param name="_groupName"></param>
        /// <param name="_info"></param>
        public void _unregMono(string _groupName, _IAnimatorControlRegInfo _info)
        {
            List<_IAnimatorControlRegInfo> list = __getItem(_groupName);
            if(null == list)
                return;

            list.Remove(_info);
        }

        #region 控制接口
        /// <summary>
        /// 开始播放
        /// </summary>
        public void playAnimator(string _groupName, string _stateName, float _normalizedTime = 0)
        {
            List<_IAnimatorControlRegInfo> list = __getItem(_groupName);
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
        /// 开始播放
        /// </summary>
        public void setAni(string _groupName, string _stateName)
        {
            List<_IAnimatorControlRegInfo> list = __getItem(_groupName);
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
        public void setBool(string _groupName, string _variableName, bool _value)
        {
            List<_IAnimatorControlRegInfo> list = __getItem(_groupName);
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
        public void setFloat(string _groupName, string _variableName, float _value)
        {
            List<_IAnimatorControlRegInfo> list = __getItem(_groupName);
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
        public void setInteger(string _groupName, string _variableName, int _value)
        {
            List<_IAnimatorControlRegInfo> list = __getItem(_groupName);
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
        public void setSpeed(string _groupName, float _speed)
        {
            List<_IAnimatorControlRegInfo> list = __getItem(_groupName);
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
        private List<_IAnimatorControlRegInfo> __safeGetItem(string _groupName)
        {
            if(null == _groupName)
                return null;

            List<_IAnimatorControlRegInfo> list = null;
            if(!_m_animationMonoDict.TryGetValue(_groupName, out list))
            {
                list = new List<_IAnimatorControlRegInfo>();
                _m_animationMonoDict.Add(_groupName, list);
            }

            return list;
        }
        private List<_IAnimatorControlRegInfo> __getItem(string _groupName)
        {
            List<_IAnimatorControlRegInfo> list = null;
            if(!_m_animationMonoDict.TryGetValue(_groupName, out list))
            {
                return null;
            }

            return list;
        }
    }
}