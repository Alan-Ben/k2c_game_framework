using System;
using System.Collections.Generic;
using MJSDK_Package;
using GOE;
using NPEnum;

namespace GOESDK
{
    public abstract partial class _ASDKMgr
    {
        //已绑定第三方账号列表
        private List<string> _m_lBindList = new List<string>();
        
        /// <summary>
        /// 绑定第三方账号
        /// </summary>
        /// <param name="_isBind"></param>
        /// <param name="_bindType"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public void bind(bool _isBind, ENPLoginWayType _bindType, Action _sucDelegate, Action<int, string> _failDelegate)
        {
            SDKUtil.showSDKDebugLog("[bind] 开始绑定第三方账号：", _bindType);
            //检查是否使用SDK，是否初始化SDK
            if (!SDKUtil.checkSDK("bind", _failDelegate))
                return;

            //检查绑定类型
            if (_bindType == ENPLoginWayType.GUEST || _bindType == ENPLoginWayType.NONE)
            {
                SDKUtil.showSDKLogError("bind", COMMON_ERROR, "bind 无效绑定类型");
                if (_failDelegate != null)
                    _failDelegate(COMMON_ERROR, "bind 无效绑定类型");
                return;
            }

            //是否有缓存token
            if (!SDKLoginSetting.instance.hasToken())
            {
                //没有token尝试获取一次
                quickLogin(data =>
                {
                    if(data != null)
                        bind(_isBind, _bindType, _sucDelegate, _failDelegate);
                    else
                    {
                        SDKUtil.showSDKLogError("bind", COMMON_ERROR, "bind 用户登录token为空");
                        if (_failDelegate != null)
                            _failDelegate(COMMON_ERROR, "bind 用户登录token为空");
                    }
                }, _failDelegate);
                return;
            }

            //是否已经绑定
            if(isBind(_bindType.ToString()))
            {
                SDKUtil.showSDKLogError("bind", COMMON_ERROR, "bind 已经绑定该类型");
                if (_failDelegate != null)
                    _failDelegate(COMMON_ERROR, "bind 已经绑定该类型");
                return;
            }

            SDKTokenData tokenData = SDKLoginSetting.instance.getTokenData();

            _accountLogin(_bindType, _accountInfo =>
            {
                MJSDK_PhpApiCommon_2SDK_user_accountBind userAccountBindInfo = new MJSDK_PhpApiCommon_2SDK_user_accountBind();
                userAccountBindInfo.token = tokenData.token;
                userAccountBindInfo.account_token = _accountInfo.account_token;

                //绑定操作
                if (_isBind)
                {
                    MJSDK_PhpApiCommonLib_UserHandle.user_accountBind(userAccountBindInfo, (_bindInfo)=>
                    {
                        _analyseBindList(_bindInfo.bind);
                        SDKLoginSetting.instance.setBindList(_m_lBindList);
                        if(_sucDelegate != null)
                            _sucDelegate();
                    }, (_code, _msg) =>
                    {
                        SDKUtil.showSDKLogError("user_accountBind", _code, _msg);
                        if (_failDelegate != null)
                            _failDelegate(_code, _msg);
                    });
                }
                //解绑操作
                else
                {
                    MJSDK_PhpApiCommonLib_UserHandle.user_accountUnBind(userAccountBindInfo, (_bindInfo) =>
                    {
                        _analyseBindList(_bindInfo.bind);
                        SDKLoginSetting.instance.setBindList(_m_lBindList);
                        if (_sucDelegate != null)
                            _sucDelegate();
                    }, (_code, _msg) =>
                    {
                        SDKUtil.showSDKLogError("user_accountUnBind", _code, _msg);
                        if (_failDelegate != null)
                            _failDelegate(_code, _msg);
                    });
                }
            }, (_code, _msg) =>
            {
                SDKUtil.showSDKLogError("_accountLogin", _code, _msg);
                if (_failDelegate != null)
                    _failDelegate(_code, _msg);
            });
        }

        /// <summary>
        /// 是否已经绑定该账号
        /// </summary>
        /// <param name="_accountType"></param>
        /// <returns></returns>
        public bool isBind(string _accountType)
        {
            if (_m_lBindList == null || _m_lBindList.Count == 0)
                return false;

            return _m_lBindList.Contains(_accountType);
        }

        /// <summary>
        /// 解析已绑定列表
        /// </summary>
        private void _analyseBindList(List<MJSDK_Login_2Engine_account_bind_info> _bindList)
        {
            if (_bindList == null)
                return;

            if (_m_lBindList == null)
                _m_lBindList = new List<string>();
            else
                _m_lBindList.Clear();

            for (int i = 0; i < _bindList.Count; i++)
            {
                if(_bindList[i] == null || string.IsNullOrEmpty(_bindList[i].type))
                    continue;

                //添加已绑定类型
                _m_lBindList.Add(_bindList[i].type.ToLower());
            }
        }
    }
}
