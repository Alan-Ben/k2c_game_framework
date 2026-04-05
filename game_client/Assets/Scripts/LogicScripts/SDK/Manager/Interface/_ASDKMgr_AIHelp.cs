using System;
using MJSDK_Package;

namespace GOESDK
{
    public abstract partial class _ASDKMgr
    {
        //AIHelp是否初始化成功
        private bool _m_bIsAIHelpInitSuc;

        /// <summary>
        /// aihelp初始化
        /// </summary>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public void aihelp_init(Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            //检查是否使用SDK，是否初始化SDK
            if (!SDKUtil.checkSDK("aihelp_init", MJSDK_AIHelpLib.g_isInit, _failDelegate))
                return;

            MJSDK_AIHelpLib.aihelp_init((_str) =>
            {
                _m_bIsAIHelpInitSuc = true;
                SDKUtil.showSDKDebugLog("[aihelp_init] aihelp初始化成功，result:", _str);
                _sucDelegate?.Invoke(_str);
            }, (_code, _msg) =>
            {
                _m_bIsAIHelpInitSuc = false;
                SDKUtil.showSDKLogError("aihelp_init", _code, _msg);
                _failDelegate?.Invoke(_code, _msg);
            });
        }

        /// <summary>
        /// 设置玩家信息
        /// </summary>
        /// <param name="_2SDK_Aihelp_SetUserInfo"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public void aihelp_setUserInfo(MJSDK_AIHelp_2SDK_aihelp_setUserInfo _2SDK_Aihelp_SetUserInfo, Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            //检查是否使用SDK，是否初始化SDK
            if (!SDKUtil.checkSDK("aihelp_setUserInfo", MJSDK_AIHelpLib.g_isInit, _failDelegate))
                return;

            //是否初始化AIHelp，初始化才能调用相关接口
            if (!_m_bIsAIHelpInitSuc)
            {
                SDKUtil.showSDKLogError("aihelp_setUserInfo", COMMON_ERROR, "AIHelp未初始化");
                _failDelegate?.Invoke(COMMON_ERROR, "AIHelp未初始化");
                return;
            }

            MJSDK_AIHelpLib.aihelp_setUserInfo(_2SDK_Aihelp_SetUserInfo, (_str) =>
            {
                SDKUtil.showSDKDebugLog("[aihelp_setUserInfo] aihelp设置玩家信息成功，result:", _str);
                _sucDelegate?.Invoke(_str);
            }, (_code, _msg) =>
            {
                SDKUtil.showSDKLogError("aihelp_setUserInfo", _code, _msg);
                _failDelegate?.Invoke(_code, _msg);
            });
        }

        /// <summary>
        /// 清除玩家信息
        /// </summary>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public void aihelp_cleanUserInfo(Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            //检查是否使用SDK，是否初始化SDK
            if (!SDKUtil.checkSDK("aihelp_cleanUserInfo", MJSDK_AIHelpLib.g_isInit, _failDelegate))
                return;

            //是否初始化AIHelp，初始化才能调用相关接口
            if (!_m_bIsAIHelpInitSuc)
            {
                SDKUtil.showSDKLogError("aihelp_cleanUserInfo", COMMON_ERROR, "AIHelp未初始化");
                _failDelegate?.Invoke(COMMON_ERROR, "AIHelp未初始化");
                return;
            }

            MJSDK_AIHelpLib.aihelp_cleanUserInfo((_str) =>
            {
                SDKUtil.showSDKDebugLog("[aihelp_cleanUserInfo] aihelp清除玩家信息成功，result:", _str);
                _sucDelegate?.Invoke(_str);
            }, (_code, _msg) =>
            {
                SDKUtil.showSDKLogError("aihelp_cleanUserInfo", _code, _msg);
                _failDelegate?.Invoke(_code, _msg);
            });
        }

        /// <summary>
        /// 更新语言
        /// </summary>
        /// <param name="_lan"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public void aihelp_updateLan(string _lan, Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            //检查是否使用SDK，是否初始化SDK
            if (!SDKUtil.checkSDK("aihelp_updateLan", MJSDK_AIHelpLib.g_isInit, _failDelegate))
                return;

            //是否初始化AIHelp，初始化才能调用相关接口
            if (!_m_bIsAIHelpInitSuc)
            {
                SDKUtil.showSDKLogError("aihelp_updateLan", COMMON_ERROR, "AIHelp未初始化");
                _failDelegate?.Invoke(COMMON_ERROR, "AIHelp未初始化");
                return;
            }

            MJSDK_AIHelpLib.aihelp_updateLan(_lan, (_str) =>
            {
                SDKUtil.showSDKDebugLog("[aihelp_updateLan] aihelp更新语言成功，result:", _str);
                _sucDelegate?.Invoke(_str);
            }, (_code, _msg) =>
            {
                SDKUtil.showSDKLogError("aihelp_updateLan", _code, _msg);
                _failDelegate?.Invoke(_code, _msg);
            });
        }

        /// <summary>
        /// 机器人流程自动化
        /// </summary>
        /// <param name="_2SDK_Aihelp_ShowRPA"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public void aihelp_showRPA(MJSDK_AIHelp_2SDK_aihelp_showRPA _2SDK_Aihelp_ShowRPA, Action<string> _sucDelegate, Action<int, string> _failDelegate)
        {
            //检查是否使用SDK，是否初始化SDK
            if (!SDKUtil.checkSDK("aihelp_showRPA", MJSDK_AIHelpLib.g_isInit, _failDelegate))
                return;

            //是否初始化AIHelp，初始化才能调用相关接口
            if (!_m_bIsAIHelpInitSuc)
            {
                SDKUtil.showSDKLogError("aihelp_showRPA", COMMON_ERROR, "AIHelp未初始化");
                _failDelegate?.Invoke(COMMON_ERROR, "AIHelp未初始化");
                return;
            }

            MJSDK_AIHelpLib.aihelp_showRPA(_2SDK_Aihelp_ShowRPA, (_str) =>
            {
                SDKUtil.showSDKDebugLog("[aihelp_showRPA] aihelp机器人流程自动化成功，result:", _str);
                _sucDelegate?.Invoke(_str);
            }, (_code, _msg) =>
            {
                SDKUtil.showSDKLogError("aihelp_showRPA", _code, _msg);
                _failDelegate?.Invoke(_code, _msg);
            });
        }
    }
}
