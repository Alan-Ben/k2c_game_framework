using ALPackage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 带CD的按钮，某些情况下，比如发送协议给服务器的按钮，需要一个时间间隔
    /// </summary>
    public class NPButton
    {
        private float _m_fCDTime;//相应间隔时间
        private GameObject _m_goBtnClick;//点击按钮
        private bool _m_bIsInCD;//是否在cd中

        public Action _m_dAction;


        public NPButton(GameObject _btn, Action _onClickAction, float _cdTime)
        {
            _m_goBtnClick = _btn;
            _m_dAction = _onClickAction;
            _m_fCDTime = _cdTime;
            ALUGUICommon.combineBtnClick(_m_goBtnClick, _onClickButton);
            _m_bIsInCD = false;
        }


        private void _onClickButton(GameObject _go)
        {
            if(_m_bIsInCD)
                return;

            _m_bIsInCD = true;
            if(_m_dAction != null)
                _m_dAction();

            ALCommonActionMonoTask.addMonoTask(() => _m_bIsInCD = false, _m_fCDTime);
        }

        public void discard()
        {
            _m_dAction = null;
            _m_goBtnClick = null;
            _m_fCDTime = 0;
        }
    }
}
