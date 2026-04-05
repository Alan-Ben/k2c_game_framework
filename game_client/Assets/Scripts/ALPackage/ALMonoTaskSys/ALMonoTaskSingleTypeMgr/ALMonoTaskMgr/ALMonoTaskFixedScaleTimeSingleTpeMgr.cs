using System;
using System.Collections.Generic;

using UnityEngine;

/*****************************
 * 实际时间的任务管理对象
 **/
namespace ALPackage
{
    public class ALMonoTaskFixedScaleTimeSingleTpeMgr : _AALMonoTaskSingleTypeMgr
    {
        public ALMonoTaskFixedScaleTimeSingleTpeMgr(int _checkTimePerSec, int _checkAreaNodeSize)
            : base(_checkTimePerSec, _checkAreaNodeSize)
        {
        }

        /**************
         * 获取时间标记
         **/
        protected override float _getNowTime()
        {
            return Time.fixedTime;
        }
    }
}
