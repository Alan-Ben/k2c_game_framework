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
    /// 点击弹上浮提示的按钮CustomMono
    /// </summary>
    public class NPGGUICustomMonoCenterTipBtn : MonoBehaviour
    {
        [ALHeader("按钮")]
        public GameObject Btn;
        [ALHeader("翻译")]
        public string tipKey;
        
        [ALHeader("特殊翻译key,走lan_args表")]
        [ALInfo("如果这个填了默认读这个字段的，不会读上面直接翻译的")]
        public string lanArgsKey;
        
        private void Start()
        {
            // 绑定按钮点击事件
            ALUGUICommon.combineBtnClick(Btn, _onClickBtn);
        }

        protected void _onClickBtn(GameObject _go)
        {
#if NP_GAME
            if (string.IsNullOrEmpty(lanArgsKey))
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(tipKey);
            }
            else
            {
                NPLanguageArgsRefObj languageArgsRefObj = GRefdataCoreMgr.instance.languageArgsRefCore.getRef(lanArgsKey);
                if (null != languageArgsRefObj)
                {
                    NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(languageArgsRefObj.lan_key_id, languageArgsRefObj.getArgsList()));
                }
            }
#endif
        }

    }
}
