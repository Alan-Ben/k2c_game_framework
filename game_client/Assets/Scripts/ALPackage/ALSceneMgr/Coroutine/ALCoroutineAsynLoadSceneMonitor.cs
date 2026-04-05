using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

/*****************
 * 监控场景加载的异步加载监控对象
 **/
namespace ALPackage
{
    public class ALCoroutineAsynLoadSceneMonitor : _IALCoroutineDealer
    {
        /** 场景信息对象 */
        private ALSceneInfo _m_siSceneInfo;
        /** 操作对象 */
        private AsyncOperation _m_aoAsyncOpObj;

        public ALCoroutineAsynLoadSceneMonitor(ALSceneInfo _sceneInfo, AsyncOperation _sceneAsyncOp)
        {
            _m_siSceneInfo = _sceneInfo;
            _m_aoAsyncOpObj = _sceneAsyncOp;
        }

        /*******************
         * Coroutine的执行函数体
         **/
        public IEnumerator dealCoroutine()
        {
            while (!_m_aoAsyncOpObj.isDone)
            {
                //调用过程函数
                _m_siSceneInfo.onLoadProcess(_m_aoAsyncOpObj.progress);
                yield return 0;
            }

            //调用完成函数
            _m_siSceneInfo.onLoadProcess(1f);

            if(_AALMonoMain.instance.showDebugOutput && ALSOGlobalSetting.Instance.logLevel <= ALLogLevel.DEBUG)
            {
                UnityEngine.Debug.Log($"【{Time.frameCount}】[Asset]load Unity Scene Async done: {_m_siSceneInfo.sceneName}");
            }

            //调用场景管理对象操作，设置之后会自动调用结果函数
            ALSceneMgr.instance.checkAndSetSceneInfoObj(_m_siSceneInfo);
        }
    }
}
