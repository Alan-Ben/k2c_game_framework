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
    /// 点击对象判断是否满足simple_unlock条件，不满足条件弹出对应 simple_unlock表的提示描述（带参数的）
    /// </summary>
    public class NPGGUICustomMonoSimpleUnlockBtn : MonoBehaviour
    {
        [ALHeader("按钮")]
        public GameObject Btn; // 按钮

        [ALHeader("simpleUnlock的配表id")] 
        public long simpleUnlockId;
        
        [ALHeader("不满足的情况下点击是否需要提示")] 
        public bool needTip;
        
        private void Start()
        {
            // 绑定按钮点击事件
            ALUGUICommon.combineBtnClick(Btn, _onClickBtn);
        }

        /** 点击响应事件 */
        protected void _onClickBtn(GameObject _go)
        {
#if NP_GAME
            GCommon.isSimpleUnlock(simpleUnlockId, needTip);
#endif
        }

    }
}
