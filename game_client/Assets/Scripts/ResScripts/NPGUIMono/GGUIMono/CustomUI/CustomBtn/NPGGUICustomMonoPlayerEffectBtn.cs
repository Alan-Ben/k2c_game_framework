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
    /// 效果自定义脚本
    /// </summary>
    public class NPGGUICustomMonoPlayerEffectBtn : MonoBehaviour
    {
        public GameObject Btn; // 按钮

        public string playerEffectStr;//效果字符串
        public string clickConditionStr;//点击的判断条件字符串

        //提示信息翻译索引
        public string noticeTxt;
        //提示信息是否为tip
        public bool isShowTip;

        //效果数据
        private bool _m_bIsInited = false;
        private List<NPPlayerEffectSerializeInfo> _m_lPlayerEffectList;
        private NPPlayerConditionGroupObj _m_lClickCondition;
        public List<NPPlayerEffectSerializeInfo> playerEffectList
        {
            get
            {
                if (_m_bIsInited)
                    return _m_lPlayerEffectList;

                _init();

                return _m_lPlayerEffectList;
            }
        }
        public NPPlayerConditionGroupObj clickCondition
        {
            get
            {
                if (_m_bIsInited)
                    return _m_lClickCondition;

                _init();

                return _m_lClickCondition;
            }
        }

        //初始操作
        protected void _init()
        {
            _m_lPlayerEffectList = NPPlayerEffectSerializeInfo.readEffectList(playerEffectStr);
            _m_lClickCondition = NPPlayerConditionGroupObj.readConditionGroupList(clickConditionStr, "custB");
            _m_bIsInited = true;
        }

        private void Start()
        {
            // 绑定按钮点击事件
            ALUGUICommon.combineBtnClick(Btn, _onClickBtn);
        }

        /** 点击响应事件 */
        protected void _onClickBtn(GameObject _go)
        {
            //调用点击处理
            //判断条件  当提示信息为空时不弹
            if (!clickCondition.IsEnable(null) && !noticeTxt.Equals(""))
            {
#if NP_GAME
                //展示提示
                if (!isShowTip)
                    NPMesMgr.instance.showOneBtnMes(TextTranslate.instance.getLanguage(noticeTxt), TextTranslate.instance.getLanguage(TransKeyConst.ok), null);
                else
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(noticeTxt);
#endif
                return;
            }

            NPPlayerEffectSerializeInfo.dealEffect(playerEffectList, null);
        }

    }
}
