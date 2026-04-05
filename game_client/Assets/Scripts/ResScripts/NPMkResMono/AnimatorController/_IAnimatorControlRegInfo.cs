using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 动画控制接口
    /// </summary>
    public interface _IAnimatorControlRegInfo
    {
        //动画对象注册名称
        public string controlNameTag { get; }

        //开始播放
        public void playAnimator(string _stateName, float _normalizedTime);
        //设置动画，强行切换，一般只有视频动画才会使用
        public void setAni(string _stateName);
        
        /// <summary>
        /// 设置属性
        /// </summary>
        public void setBool(string _variableName, bool _value);

        public void setFloat(string _variableName, float _value);

        public void setInteger(string _variableName, int _value);

        public void setSpeed(float _speed);
    }
}