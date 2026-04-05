using UnityEngine;
using System.Collections;
using ALPackage;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

namespace GOE
{
    /** 加载prefab信息对象 */
    [System.Serializable]
    public class NPGGUIConditionDisableInfo
    {
        [ALHeader("注释，会展示在列表上")]
        public string annotation;
        [ALHeader("条件字符串")]
        public string conditionStr;
        [ALHeader("条件通过隐藏的GO列表")]
        public List<GameObject> disableGo;
        [ALHeader("条件通过显示的GO列表")]
        public List<GameObject> enableGo;

        private bool _m_bIsInited = false;
        private NPPlayerConditionGroupObj _m_cgConditionGroupObj;
        public NPPlayerConditionGroupObj conditionGroup
        {
            get
            {
                if (_m_bIsInited)
                    return _m_cgConditionGroupObj;

                _m_cgConditionGroupObj = NPPlayerConditionGroupObj.readConditionGroupList(conditionStr, "condPref");
                _m_bIsInited = true;
                return _m_cgConditionGroupObj;
            }
        }
    }

    
    /// <summary>
    /// 自定义条件显隐脚本
    /// </summary>
    public class NPGGUIMonoCustomConditionDisable : MonoBehaviour
    {
        //加载信息对象队列
        public List<NPGGUIConditionDisableInfo> disableList;

        //是否需要检测
        private bool _m_bNeedCheck = false;

        //有效和无效的时候分别注册和注销显示对象
        private void OnEnable()
        {
            _m_bNeedCheck = true;

            //到管理对象中进行处理
            ALCommonActionMonoTask.addNextFrameTask(_check);

            //注册消息
            WinMsg.RegisterMsgAct(WinMsgType.CUSTOM_RELOAD, _doCheck);
        }

        private void OnDisable()
        {
            _m_bNeedCheck = true;

            //到管理对象中进行处理
            ALCommonActionMonoTask.addNextFrameTask(_check);

            //注销消息
            WinMsg.UnregisterMsgAct(WinMsgType.CUSTOM_RELOAD, _doCheck);
        }

        private void OnDestroy()
        {
            _m_bNeedCheck = true;
            //直接检测
            _check();
        }

        protected void _check()
        {
            if (!_m_bNeedCheck)
                return;

            _doCheck();
        }

        protected void _doCheck()
        {
            if (null == this || null == gameObject)
            {
#if UNITY_EDITOR
                if (_m_bNeedCheck)
                    UnityEngine.Debug.LogError("Check mono Enable error!");
#endif
                return;
            }

            _m_bNeedCheck = false;

#if NP_GAME
            if (gameObject.activeInHierarchy)
            {
                NPGGUIConditionDisableInfo tmpInfo = null;
                for (int i = 0; i < disableList.Count; i++)
                {
                    tmpInfo = disableList[i];
                    if (null == tmpInfo)
                        continue;

                    if (tmpInfo.conditionGroup.IsEnable(null))
                    {
                        ALUGUICommon.setGameObjEnable(tmpInfo.disableGo, false);
                        ALUGUICommon.setGameObjEnable(tmpInfo.enableGo, true);
                    }
                    else
                    {
                        ALUGUICommon.setGameObjEnable(tmpInfo.disableGo, true);
                        ALUGUICommon.setGameObjEnable(tmpInfo.enableGo, false);
                    }
                }
            }
            else
            {
                //无效时不做处理
            }
#endif
        }
    }
}

