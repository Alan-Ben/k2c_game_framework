using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 注册动画控制对象的mono对象
    /// </summary>
    public class _ATAniControlMono<T> : MonoBehaviour where T : _IAnimatorControlRegInfo, _IAnimatorControlGroupRegInfo
    {
        /// <summary>
        /// 需要注册的数据列表
        /// </summary>
        public List<T> dataList;

        private void Awake()
        {
            AnimatorControllerMgr.instance.regMono(this);
        }

        private void OnDestroy()
        {
            AnimatorControllerMgr.instance.unregMono(this);

            //从集合管理中释放
            AnimatorControllerGroupMgr.instance.unregMono(this);
        }

        #region 控制接口
        /// <summary>
        /// 开始播放
        /// </summary>
        public void playAnimator(string _name, string _stateName, float _normalizedTime = 0)
        {
            T tmpInfo = default(T);
            for(int i = 0; i < dataList.Count; i++)
            {
                tmpInfo = dataList[i];
                if(null == tmpInfo)
                    continue;

                tmpInfo.playAnimator(_stateName, _normalizedTime);
            }
        }
        
        /// <summary>
        /// 开始播放
        /// </summary>
        public void setAni(string _name, string _stateName)
        {
            T tmpInfo = default(T);
            for(int i = 0; i < dataList.Count; i++)
            {
                tmpInfo = dataList[i];
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
            T tmpInfo = default(T);
            for (int i = 0; i < dataList.Count; i++)
            {
                tmpInfo = dataList[i];
                if(null == tmpInfo)
                    continue;

                tmpInfo.setBool(_variableName, _value);
            }
        }
        public void setFloat(string _name, string _variableName, float _value)
        {
            T tmpInfo = default(T);
            for (int i = 0; i < dataList.Count; i++)
            {
                tmpInfo = dataList[i];
                if(null == tmpInfo)
                    continue;

                tmpInfo.setFloat(_variableName, _value);
            }
        }
        public void setInteger(string _name, string _variableName, int _value)
        {
            T tmpInfo = default(T);
            for (int i = 0; i < dataList.Count; i++)
            {
                tmpInfo = dataList[i];
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
            T tmpInfo = default(T);
            for (int i = 0; i < dataList.Count; i++)
            {
                tmpInfo = dataList[i];
                if(null == tmpInfo)
                    continue;

                tmpInfo.setSpeed(_speed);
            }
        }

        #endregion
    }
}
