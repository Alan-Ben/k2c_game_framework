using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /***************
     * 定时删除特效对象的监控处理任务
     **/
    public class SfxMonitorTask : _IALBaseMonoTask
    {
        private _ISfxObj _m_sfxObj;
        private Action _m_doneAction;
        
        public SfxMonitorTask(_ISfxObj _sfxObj, Action _doneAction)
        {
            _m_sfxObj = _sfxObj;
            _m_doneAction = _doneAction;
        }

        public void deal()
        {
            if (null == _m_sfxObj)
                return;
            
            if (null != _m_doneAction)
                _m_doneAction();
            _m_doneAction = null;
            
            //回收处理
            _m_sfxObj.forceDiscard();
        }
    }
}
