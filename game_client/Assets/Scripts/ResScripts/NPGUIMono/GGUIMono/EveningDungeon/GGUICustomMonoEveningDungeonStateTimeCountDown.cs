using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.Serialization;

namespace GOE
{
    /// <summary>
    /// 显示晚间活动时间的CustomMono
    /// </summary>
    public class GGUICustomMonoEveningDungeonStateTimeCountDown : MonoBehaviour
    {
        [ALHeader("晚间副本状态时间倒计时子窗口")]
        public GGUISubMonoEveningDungeonStateTimeCountDown monoStateTimeCountDown;

#if NP_GAME
        private GGUISubWndEveningDungeonStateTimeCountDown _m_StateTimeCountDownSubWnd;

        private void Awake()
        {
            if(monoStateTimeCountDown != null)
                _m_StateTimeCountDownSubWnd = new GGUISubWndEveningDungeonStateTimeCountDown(monoStateTimeCountDown);
        }

        private void OnDestroy()
        {
            _m_StateTimeCountDownSubWnd?.discard();
            _m_StateTimeCountDownSubWnd = null;
        }

        private void OnEnable()
        {
            _m_StateTimeCountDownSubWnd?.showWnd();
        }

        private void OnDisable()
        {
            _m_StateTimeCountDownSubWnd?.hideWnd();
        }
#endif
    }
}