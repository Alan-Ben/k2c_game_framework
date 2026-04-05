using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 自定义高级公式刷新脚本
    /// </summary>
    public class NPGGUIMonoPlayerVariableRefresher : MonoBehaviour
    {
        [System.Serializable]
        public class NPPlayerVariableRefreshInfo
        {
            public string transKey; //翻译key
            public string refreshCondition; //玩家刷新条件
            public string playerVariableStr; //玩家计算公式
            public Text refreshText;    //刷新文本对象

            private NPPlayerConditionGroupObj _m_lRefreshCondition;
            private bool _m_bIsInitCondition = false;
            private NPPlayerVariableGroupObj _m_lReadedVariableGroup;
            private bool _m_bIsInited = false;

            public string GetLastStr()
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
            public void refresh()
            {
                if (null == refreshText)
                    return;

                if (!_m_bIsInitCondition)
                {
                    _m_bIsInitCondition = true;
                    _m_lRefreshCondition = NPPlayerConditionGroupObj.readConditionGroupList(refreshCondition, string.Empty);
                }

#if NP_GAME
                //判断条件
                if (null != _m_lRefreshCondition && !NPPlayerConditionGroupObj.IsEnable(_m_lRefreshCondition, null))
                    return;
#endif

                ALUGUICommon.setLabelTxt(refreshText, GetLastStr());
            }
        }

        //加载信息对象队列
        public List<NPPlayerVariableRefreshInfo> loadInfoList;

        //是否需要检测
        private bool _m_bNeedCheck = false;

        //有效和无效的时候分别注册和注销显示对象
        private void OnEnable()
        {
            _m_bNeedCheck = true;

            //到管理对象中进行处理
            ALCommonActionMonoTask.addNextFrameTask(_check);
        }

        private void OnDisable()
        {
            _m_bNeedCheck = true;

            //到管理对象中进行处理
            ALCommonActionMonoTask.addNextFrameTask(_check);
        }

        private void OnDestroy()
        {
            _m_bNeedCheck = true;
            //直接检测
            _check();
        }

        protected void _check()
        {
            if (!_m_bNeedCheck || null == this || null == gameObject)
            {
#if UNITY_EDITOR
                if (_m_bNeedCheck)
                    UnityEngine.Debug.LogError("Check mono Enable error!");
#endif
                return;
            }

            _m_bNeedCheck = false;

            //根据是否有效进行不同处理
            if (gameObject.activeInHierarchy)
            {
                for (int i = 0; i < loadInfoList.Count; i++)
                {
                    if (null == loadInfoList[i])
                        continue;
                    loadInfoList[i].refresh();
                }

                //注册刷新消息
                WinMsg.RegisterMsgAct(WinMsgType.CUSTOM_RELOAD, refreshAll);
            }
            else
            {
                //注销刷新消息
                WinMsg.UnregisterMsgAct(WinMsgType.CUSTOM_RELOAD, refreshAll);
            }
        }

        //刷新所有文本
        protected void refreshAll()
        {
            for (int i = 0; i < loadInfoList.Count; i++)
            {
                if (null == loadInfoList[i])
                    continue;
                loadInfoList[i].refresh();
            }
        }
    }
}

