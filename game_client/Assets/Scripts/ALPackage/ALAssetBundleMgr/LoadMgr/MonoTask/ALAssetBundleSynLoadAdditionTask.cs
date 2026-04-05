using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace ALPackage
{
    /// <summary>
    /// 加载资源使用同步方案时，如已经加载则使用本特殊任务进行额外加载处理
    /// </summary>
    public class ALAssetBundleSynLoadAdditionTask : _IALBaseMonoTask
    {
        /** 加载控制对象 */
        private _AALBasicAssetBundleLoadMgr _m_lmLoadMgr;

        public ALAssetBundleSynLoadAdditionTask(_AALBasicAssetBundleLoadMgr _loadMgr)
        {
            _m_lmLoadMgr = _loadMgr;
        }

        /*******************
         * Coroutine的执行函数体
         **/
        public void deal()
        {
            //每帧进行一次检测
            _m_lmLoadMgr.tryPopLoadingNecObj();
        }
    }
}
