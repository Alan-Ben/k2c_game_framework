
using System;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 一个通用的乐园场景点击操作
    /// </summary>
    public class NPSceneGoPosMono : MonoBehaviour
    {
        [ALHeader("注册位置的标记")]
        public string regPosTag;
        [ALHeader("注册位置的3D对象")]
        public Transform trans;

        /// <summary>
        /// 注册位置信息
        /// </summary>
        public void OnEnable()
        {
            if (null == trans)
                return;

            NPShowPosMgr.instance.regPos(regPosTag, new NPSceneGoPos(trans));
        }

        /// <summary>
        /// 注销位置信息
        /// </summary>
        public void OnDisable()
        {
            //直接注销
            NPShowPosMgr.instance.unregPos(regPosTag);
        }
    }
}