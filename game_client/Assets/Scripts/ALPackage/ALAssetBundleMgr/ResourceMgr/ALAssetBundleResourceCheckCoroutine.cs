using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace ALPackage
{
    public class ALAssetBundleResourceCheckCoroutine : _IALCoroutineDealer
    {
        /*******************
         * Coroutine的执行函数体
         **/
        public IEnumerator dealCoroutine()
        {
            //循环进行资源检查
            while (true)
            {
                //执行检查任务
                ALAssetBundleLoadedMgr.instance.checkLoadedObjLiveTime();

                //返回等待时间
                yield return new WaitForSeconds(ALAssetBundleLoadedMgr.instance.checkCycleTime);
            }
        }
    }
}
