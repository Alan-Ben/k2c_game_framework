using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace ALPackage
{
    /*********************
     * 控制需要加载对象的队列，并对执行顺序进行管理的操作Coroutine对象
     **/
    public class ALLoadingAssetInfoLateFailDelegateTask : _IALBaseMonoTask
    {
        private ALLoadingAssetInfo _m_aiAssetInfo;

        public ALLoadingAssetInfoLateFailDelegateTask(ALLoadingAssetInfo _assetInfo)
        {
            _m_aiAssetInfo = _assetInfo;
        }

        /*******************
         * 任务具体的执行函数
         **/
        public void deal()
        {
            _m_aiAssetInfo.callLateFailDelegate();
        }
    }
}
