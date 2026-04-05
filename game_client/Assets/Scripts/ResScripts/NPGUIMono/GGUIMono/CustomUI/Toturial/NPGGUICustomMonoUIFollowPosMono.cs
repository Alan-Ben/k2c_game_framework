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
    /// 将UI对象设置为跟随对应标记位置的处理脚本
    /// </summary>
    public class NPGGUICustomMonoUIFollowPosMono : MonoBehaviour
    {
        [ALHeader("跟随位置的标记")]
        public string regPosTag;
        [ALHeader("跟随的UI对象")]
        public RectTransform _m_tUIObj;

        /// <summary>
        /// 注册位置信息
        /// </summary>
        public void OnEnable()
        {
            if (null == _m_tUIObj)
                return;

            _INPShowPos pos = NPShowPosMgr.instance.getPos(regPosTag);
            if (null == pos)
                return;

            //设置跟随位置
            Vector2 uiPos = pos.getUIPos();
            ALUGUICommon.setUIPos(_m_tUIObj, uiPos);
        }
    }
}

