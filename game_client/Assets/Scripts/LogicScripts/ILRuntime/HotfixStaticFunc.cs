using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public static class HotfixStaticFunc
    {
        //初始化hotfix，这个init没有任何依赖，不涉及业务
        public static void init()
        {
            ALHotfixMgr_ILRuntime_Global.instance.dealStaticFunc("Hotfix.HotfixMain", "init");
        }
        
        //初始化hotfix的配表，依赖资源
        public static void initHotfixRefdata(Action _action)
        {
            ALHotfixMgr_ILRuntime_Global.instance.dealStaticFunc("Hotfix.HotfixMain", "initRefdata", _action);
        }

        // 初始化热更数据组件
        public static void initHotfixDataComponent(Action<bool> _onDataComponentInitDone)
        {
            try
            {
                ALHotfixMgr_ILRuntime_Global.instance.dealStaticFunc("Hotfix.HotfixMain", "initDataComponent", _onDataComponentInitDone);
            }
            catch(Exception _e)
            {
                UnityEngine.Debug.LogError("Init Hotfix Data Component Error!!! " + _e.ToString());
            }
        }
        
        // 销毁热更数据组件
        public static void discardHotfixDataComponent()
        {
            try
            {
                ALHotfixMgr_ILRuntime_Global.instance.dealStaticFunc("Hotfix.HotfixMain", "discardDataComponent");
            }
            catch(Exception _e)
            {
                UnityEngine.Debug.LogError("Init Hotfix Data Component Error!!! " + _e.ToString());
            }
        }

        /// <summary>
        /// 处理活动中心加载热更活动页面
        /// </summary>
        /// <param name="_id"></param>
        /// <param name="_pageParent"></param>
        /// <returns></returns>
        public static _AALBasicLoadUIWndBasicClass dealLoadHotfixActivityCenterPage(long _id, Transform _pageParent)
        {
            try
            {
                return (_AALBasicLoadUIWndBasicClass)ALHotfixMgr_ILRuntime_Global.instance.dealStaticFunc("Hotfix.HotfixActivityCenter", "dealLoadActivityCenterPage", _id, _pageParent);
            }
            catch (Exception _e)
            {
                UnityEngine.Debug.LogError("dealLoadHotfixActivityCenterPage Error!!! " + _e.ToString());
            }

            return null;
        }

        public static void addMainCityPushNotice(long _addNoticeSerialize, bool _isLogin, Action _addDone)
        {
            try
            {
                ALHotfixMgr_ILRuntime_Global.instance.dealStaticFunc("Hotfix.HotfixMainCityPushNotice", "addMainCityPushNotice", _addNoticeSerialize, _isLogin, _addDone);
            }
            catch(Exception _e)
            {
                UnityEngine.Debug.LogError("Hotfix addMainCityPushNotice Error!!! " + _e.ToString());
                _addDone?.Invoke();
            }
        }
    }
}