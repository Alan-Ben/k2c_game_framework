using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 动画控制接口
    /// </summary>
    public interface _IAnimatorControlGroupRegInfo
    {
        //存储的group名称
        public void addGroupName(string _groupName);

        /// <summary>
        /// 删除存储的group关联，内部函数，不建议外部调用
        /// </summary>
        /// <param name="_groupName"></param>
        public void _removeGroupName(string _groupName);

        /// <summary>
        /// 从group管理器中析构所有注册了本对象的地方
        /// </summary>
        public void disposeFromGroupMgr();
    }
}