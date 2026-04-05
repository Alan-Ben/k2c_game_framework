using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 自定义打开链接脚本
    /// </summary>
    public class NPGUIMonoURLBtn : _AALBasicUIWndMono
    {
        [System.Serializable]
        public class NPPlayerURLInfo
        {
            public string transKey; //翻译key
            public string refreshCondition; //玩家刷新条件
            public string playerVariableStr; //玩家计算公式

            private NPPlayerConditionGroupObj _m_lRefreshCondition;
            private bool _m_bIsInitCondition = false;
            private NPPlayerVariableGroupObj _m_lReadedVariableGroup;
            private bool _m_bIsInited = false;

            public string GetURL()
            {
                if (!_m_bIsInited)
                {
                    _m_bIsInited = true;
                    _m_lReadedVariableGroup = NPPlayerVariableGroupObj.readVariableGroup(playerVariableStr, string.Empty);
                }

#if NP_GAME
                //计算高级公式
                long res = NPPlayerVariableGroupObj.CalculateVariableResult(_m_lReadedVariableGroup, null);
                //根据文本计算翻译
                if (string.IsNullOrEmpty(transKey))
                {
                    return res.ToString();
                }
                else
                {
                    return TextTranslate.instance.getLanguage(transKey, res);
                }
#else
            return string.Empty;
#endif
            }

            //刷新
            public bool judgeEnable()
            {
                if (!_m_bIsInitCondition)
                {
                    _m_bIsInitCondition = true;
                    _m_lRefreshCondition = NPPlayerConditionGroupObj.readConditionGroupList(refreshCondition, string.Empty);
                }

#if NP_GAME
                //判断条件
                return NPPlayerConditionGroupObj.IsEnable(_m_lRefreshCondition, null);
#else
            return false;
#endif
            }
        }

        public GameObject Btn; // 按钮

        //加载信息对象队列
        public List<NPPlayerURLInfo> loadInfoList;

        private void Start()
        {
            // 绑定按钮点击事件
            ALUGUICommon.combineBtnClick(Btn, _onClickBtn);
        }

        /** 点击响应事件 */
        protected void _onClickBtn(GameObject _go)
        {
            for (int i = 0; i < loadInfoList.Count; i++)
            {
                if (null == loadInfoList[i])
                    continue;
                if (loadInfoList[i].judgeEnable())
                {
#if NP_GAME
                    GCommon.openURL(loadInfoList[i].GetURL());
#endif
                    return;
                }
            }
        }

    }
}

