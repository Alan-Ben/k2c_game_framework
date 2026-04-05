using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.UI;

#if NP_GAME
using GOE;
#endif

namespace GOE
{
    /// <summary>
    /// 注册UI对象位置的脚本
    /// </summary>
    public class NPGGUICustomMonoUIObjPosMono : MonoBehaviour
    {
        [ALHeader("注册位置的标记")]
        public string regPosTag;
        [ALHeader("注册位置的UI对象")]
        public RectTransform _m_tUIObj;

        /// <summary>
        /// 注册位置信息
        /// </summary>
        public void OnEnable()
        {
            if (null == _m_tUIObj)
                return;

            NPShowPosMgr.instance.regPos(regPosTag, new NPUIObjPos(_m_tUIObj));
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

