using System;
using System.Collections;

using UnityEngine;

/******************
 * 资源异步加载的监控处理类对象
 **/
namespace ALPackage
{
    public class ALAssetBundleAsynMonitor : _IALCoroutineDealer
    {
        /** 资源对象 */
        private ALAssetBundleObj _m_aboAssetBundleObj;
        /** 资源异步加载处理对象 */
        private AssetBundleRequest _m_abrAssetRequest;
        /** 资源处理回调 */
        private Action<UnityEngine.Object> _m_delegate;

        public ALAssetBundleAsynMonitor(ALAssetBundleObj _assetObj, AssetBundleRequest _request, Action<UnityEngine.Object> _delegate)
        {
            _m_aboAssetBundleObj = _assetObj;
            _m_abrAssetRequest = _request;
            _m_delegate = _delegate;
        }

        /*******************
         * Coroutine的执行函数体
         **/
        public IEnumerator dealCoroutine()
        {
            //等待加载完毕
            yield return _m_abrAssetRequest;
            if(_AALMonoMain.instance.showDebugOutput && ALSOGlobalSetting.Instance.logLevel <= ALLogLevel.DEBUG)
            {
                if(_m_abrAssetRequest != null && _m_abrAssetRequest.asset != null)
                {
                    UnityEngine.Debug.Log($"【{Time.frameCount}】[Asset]Load Asset async done: {_m_abrAssetRequest.asset.name}");
                }
                else
                {
                    UnityEngine.Debug.Log($"【{Time.frameCount}】[Asset]Load Asset async done: null");
                }
            }

            //处理加载对象
            UnityEngine.Object obj = _m_abrAssetRequest.asset;

            //处理资源对象异步加载完成事件
            _m_aboAssetBundleObj.onLoadingOpDone();

            //调用回调
            if (null != _m_delegate)
                _m_delegate(obj);
            _m_delegate = null;
        }
    }
}
