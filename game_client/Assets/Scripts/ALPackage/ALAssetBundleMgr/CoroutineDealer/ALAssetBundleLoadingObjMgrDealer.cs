using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace ALPackage
{
    /*********************
     * 控制需要加载对象的队列，并对执行顺序进行管理的操作Coroutine对象
     **/
    public class ALAssetBundleLoadingObjMgrDealer : _IALCoroutineDealer
    {
        /** 加载控制对象 */
        private _AALBasicAssetBundleLoadMgr _m_lmLoadMgr;

        public ALAssetBundleLoadingObjMgrDealer(_AALBasicAssetBundleLoadMgr _loadMgr)
        {
            _m_lmLoadMgr = _loadMgr;
        }

        /*******************
         * Coroutine的执行函数体
         **/
        public IEnumerator dealCoroutine()
        {
            //版本信息初始化完成后才可以开始进行相关加载处理
            while (true)
            {
                //每帧进行一次检测
                _m_lmLoadMgr.tryPopLoadingObj();

                yield return null;
            }
        }
    }
}
