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
    public class NPGGUICustomMonoTutorialClickEffectAndNextBtn : MonoBehaviour
    {
        [ALHeader("在发送引导到下一步之前执行的处理")]
        public string dealEffectString;

        public GameObject Btn; // 按钮

        private void Start()
        {
            // 绑定按钮点击事件
            ALUGUICommon.combineBtnClick(Btn, _dealClickBtn);
        }

        /** 点击响应事件 */
        protected void _dealClickBtn(GameObject _go)
        {
            //执行效果操作
#if NP_GAME
            List<NPPlayerEffectSerializeInfo> effectList = NPPlayerEffectSerializeInfo.readEffectList(dealEffectString);
            //执行效果
            NPPlayerEffectSerializeInfo.dealEffect(effectList, null);
#endif

            //直接发送消息
            WinMsg.SendMsg(WinMsgType.CONTROL_TUROTIAL_SETP, ENPTutorialTriggerType.TURORIAL_NEXT);
        }
    }
}

