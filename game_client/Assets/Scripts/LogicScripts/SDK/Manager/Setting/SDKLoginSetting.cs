using System;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using JetBrains.Annotations;
using MJSDK_Package;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 需要缓存的token数据
    /// </summary>
    [System.Serializable]
    public class SDKTokenData
    {        
        //用户ID
        public string user_id;
        //登录token，客户端自行记录，需要验证登录的接口都必须带上该参数
        public string token;
        //token到期时间，10位时间戳
        public long token_expiry;
        /*用户绑定的帐号类型与昵称格式:*/
        public List<MJSDK_Login_2Engine_account_bind_info> bind;
        //用户充值价值
        public float user_pay;
        //用户信息
        public MJSDK_Login_2Engine_user_info_user user;
        //user_id和token验证签名
        public string token_sign;

        public SDKTokenData()
        {
        }

        public SDKTokenData(MJSDK_PhpApiCommon_2Engine_user_login _info)
        {
            setInfo(_info);
        }

        public void setInfo(MJSDK_PhpApiCommon_2Engine_user_login _info)
        {
            if (_info == null)
                return;

            user_id = _info.user_id;
            token = _info.token;
            token_expiry = _info.token_expiry;
            bind = _info.bind;
            user_pay = _info.user_pay;
            user = _info.user;
            token_sign = _info.token_sign;
        }

        public void clear()
        {
            user_id = string.Empty;
            token = string.Empty;
            token_expiry = 0;
            bind = null;
            user_pay = 0;
            user = null;
            token_sign = string.Empty;
        }
    }

    /// <summary>
    /// SDKLoginSetting序列化数据
    /// </summary>
    [System.Serializable]
    public class SDKLoginSettingData
    {
        //SDK登录token
        public SDKTokenData tokenData = new SDKTokenData();
        //SDK登录类型
        public ENPLoginWayType sdkLoginType = ENPLoginWayType.GUEST;
    }
    
    /// <summary>
    /// SDK登录相关存储
    /// </summary>
    public class SDKLoginSetting : _AALBasicSettingInfo
    {
        private static SDKLoginSetting _g_instance;
        [NotNull]
        public static SDKLoginSetting instance
        {
            get
            {
                if(null == _g_instance)
                    _g_instance = new SDKLoginSetting(_m_sPath);
                return _g_instance;
            }
        }

        private static string _m_sPath = "sdkloginsetting_230315";
        
        private SDKLoginSettingData _m_settingData;

        public SDKLoginSetting(string _savePath)
           : base(_savePath)
        {
            _m_settingData = new SDKLoginSettingData();
        }

        /// <summary>
        /// 构建需要保存的字符串
        /// </summary>
        /// <returns></returns>
        protected override string _makeSettingStr()
        {
            return JsonUtility.ToJson(_m_settingData);
        }

        /// <summary>
        /// 读取保存的字符串
        /// </summary>
        /// <param name="_infoStr"></param>
        protected override void _initSettingStr(string _infoStr)
        {
            if(string.IsNullOrEmpty(_infoStr))
                return;

            _m_settingData = JsonUtility.FromJson<SDKLoginSettingData>(_infoStr);
        }

        /// <summary>
        /// 是否有缓存token
        /// </summary>
        /// <returns></returns>
        public bool hasToken()
        {
            return _m_settingData != null &&
                   _m_settingData.tokenData != null &&
                   !string.IsNullOrEmpty(_m_settingData.tokenData.token) &&
                   !string.IsNullOrEmpty(_m_settingData.tokenData.user_id);
        }

        /// <summary>
        /// 清空token缓存
        /// </summary>
        public void clearToken()
        {
            if (null == _m_settingData || _m_settingData.tokenData == null)
                return;

            _m_settingData.tokenData.clear();
            saveSetting();
        }

        /// <summary>
        /// 设置token
        /// </summary>
        /// <param name="_token"></param>
        public void setTokenData(MJSDK_PhpApiCommon_2Engine_user_login _info)
        {
            if (null == _m_settingData)
                return;

            if (_m_settingData.tokenData == null)
                _m_settingData.tokenData = new SDKTokenData();

            _m_settingData.tokenData.setInfo(_info);
            saveSetting();
        }

        /// <summary>
        /// 获取token
        /// </summary>
        /// <returns></returns>
        public SDKTokenData getTokenData()
        {
            if (null == _m_settingData)
                return null;

            return _m_settingData.tokenData;
        }

        /// <summary>
        /// 设置上次SDK登录类型
        /// </summary>
        /// <param name="_type"></param>
        public void setSDKLoginType(ENPLoginWayType _type)
        {
            if (null == _m_settingData)
                return;

            _m_settingData.sdkLoginType = _type;
            saveSetting();
        }

        /// <summary>
        /// 获取上次SDK登录类型
        /// </summary>
        /// <returns></returns>
        public ENPLoginWayType getSDKLoginType()
        {
            if (null == _m_settingData)
                return ENPLoginWayType.GUEST;

            return _m_settingData.sdkLoginType;
        }

        /// <summary>
        /// 保存已绑定列表
        /// </summary>
        /// <param name="_str"></param>
        public void setBindList(List<string> _strList)
        {
            if (null == _m_settingData || null == _m_settingData.tokenData || _strList == null)
                return;

            List<MJSDK_Login_2Engine_account_bind_info> bindList = new List<MJSDK_Login_2Engine_account_bind_info>();
            for (int i = 0; i < _strList.Count; i++)
            {
                if(string.IsNullOrEmpty(_strList[i]))
                    continue;
                MJSDK_Login_2Engine_account_bind_info bindInfo = new MJSDK_Login_2Engine_account_bind_info();
                bindInfo.type = _strList[i];
                bindList.Add(bindInfo);
            }

            _m_settingData.tokenData.bind = bindList;
            saveSetting();
        }
    }
}