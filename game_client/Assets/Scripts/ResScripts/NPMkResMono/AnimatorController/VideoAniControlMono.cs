using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 动画对象注册的信息
    /// </summary>
    [Serializable]
    public class VideoAniControlRegInfo : _IAnimatorControlRegInfo, _IAnimatorControlGroupRegInfo
    {
        [ALHeader("动画对象注册名称")]
        public string controlName;
        [ALHeader("绑定动画控制对象")]
        public List<_AVideoAniMono> animator;
        
        #region 控制接口

        public string controlNameTag
        {
            get { return controlName; }
        }

        /// <summary>
        /// 开始播放
        /// </summary>
        public void playAnimator(string _stateName, float _normalizedTime)
        {
            if(null == animator)
                return;
            
            for(int n = 0; n < animator.Count; n++)
            {
                if(null == animator[n])
                    continue;

                animator[n].playAniTag(_stateName);
            }
        }

        public void setAni(string _stateName)
        {
            if(null == animator)
                return;
            
            for(int n = 0; n < animator.Count; n++)
            {
                if(null == animator[n])
                    continue;

                animator[n].setAniTag(_stateName);
            }
        }

        /// <summary>
        /// 设置属性
        /// </summary>
        public void setBool(string _variableName, bool _value)
        {
        }
        public void setFloat(string _variableName, float _value)
        {
        }
        public void setInteger(string _variableName, int _value)
        {
        }

        /// <summary>
        /// 设置动画播放速度
        /// </summary>
        /// <param name="_name"></param>
        /// <param name="_speed"></param>
        public void setSpeed(float _speed)
        {
        }

        #endregion

        #region 注册信息注册到group的信息集合
        private List<string> _m_lRegGroupList = new List<string>();

        //存储的group名称
        public void addGroupName(string _groupName)
        {
            _m_lRegGroupList.Add(_groupName);
        }
        public List<string> getGroupList()
        {
            return _m_lRegGroupList;
        }

        /// <summary>
        /// 删除存储的group关联，内部函数，不建议外部调用
        /// </summary>
        /// <param name="_groupName"></param>
        public void _removeGroupName(string _groupName)
        {
            _m_lRegGroupList.Remove(_groupName);
        }

        /// <summary>
        /// 从group管理器中析构所有注册了本对象的地方
        /// </summary>
        public void disposeFromGroupMgr()
        {
            //逐个注销
            for(int i = 0; i < _m_lRegGroupList.Count; i++)
            {
                AnimatorControllerGroupMgr.instance._unregMono(_m_lRegGroupList[i], this);
            }
            //清空列表
            _m_lRegGroupList.Clear();
        }
        #endregion
    }
    /// <summary>
    /// 注册动画控制对象的mono对象
    /// </summary>
    public class VideoAniControlMono : _ATAniControlMono<VideoAniControlRegInfo>
    {
    }
}
