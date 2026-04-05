using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace ALPackage
{
    /*********************
     * 控制需要加载对象的队列，并对执行顺序进行管理的操作Coroutine对象
     **/
    public class ALLoadingAssetInfoLateSucDelegateTask : _IALBaseMonoTask
    {
        private ALLoadingAssetInfo _m_aiAssetInfo;
        private ALAssetBundleObj _m_aoAssetObj;

        public ALLoadingAssetInfoLateSucDelegateTask(ALLoadingAssetInfo _assetInfo, ALAssetBundleObj _assetObj)
        {
            _m_aiAssetInfo = _assetInfo;
            _m_aoAssetObj = _assetObj;
        }

        /*******************
         * 任务具体的执行函数
         **/
        public void deal()
        {
            _m_aiAssetInfo.callLateSucDelegate(_m_aoAssetObj);

            //调用减少操作统计的函数
            _m_aoAssetObj.onLoadingOpDone();
        }

        public override string ToString()
        {
            return base.ToString() + "--" + _m_aiAssetInfo.path;
        }
    }
}
