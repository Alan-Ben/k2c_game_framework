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
    /// 引导进入下一步的处理按钮，统一发送TURORIAL_NEXT引导消息
    /// </summary>
    public class NPGGUICustomMonoNextTutorialBtn : MonoBehaviour
    {
        public GameObject Btn; // 按钮

        private void Start()
        {
            // 绑定按钮点击事件
            ALUGUICommon.combineBtnClick(Btn, _dealClickBtn);
        }

        /** 点击响应事件 */
        protected void _dealClickBtn(GameObject _go)
        {
            //直接发送消息
            WinMsg.SendMsg(WinMsgType.CONTROL_TUROTIAL_SETP, ENPTutorialTriggerType.TURORIAL_NEXT);
        }
    }
}

