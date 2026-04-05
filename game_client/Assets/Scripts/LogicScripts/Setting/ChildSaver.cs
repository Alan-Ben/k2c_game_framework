using System;
using ALPackage;
using UnityEngine;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace GOE
{
    [Serializable]
    public class ChildSaveData
    {
        public List<long> chatShareHadMarriedAdultIdList;//聊天分享里已组队的成年子嗣实例id列表
        public bool isEngageLimitEnabled = true;//是否开启联谊限制
    }

    /// <summary>
    /// 子嗣相关本地保存
    /// </summary>
    public class ChildSaver : _AALBasicSettingInfo
    {
        [NotNull]private ChildSaveData _m_settingData;
        
        public ChildSaver(long _accountCID) : base($"{_accountCID}_cache_account_child_save")
        {
            _m_settingData = new ChildSaveData();
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

            _m_settingData = JsonUtility.FromJson<ChildSaveData>(_infoStr);
        }

        /// <summary>
        /// 添加聊天分享里已组队成年子嗣实例id
        /// </summary>
        /// <param name="_id"></param>
        public void addChatShareHadMarriedAdultId(long _id)
        {
            if (null == _m_settingData)
                return;

            if (_m_settingData.chatShareHadMarriedAdultIdList == null)
                _m_settingData.chatShareHadMarriedAdultIdList = new List<long>();

            if (_m_settingData.chatShareHadMarriedAdultIdList.Contains(_id))
                return;

            //最多存50个
            if(_m_settingData.chatShareHadMarriedAdultIdList.Count == 50)
                _m_settingData.chatShareHadMarriedAdultIdList.RemoveAt(0);
            _m_settingData.chatShareHadMarriedAdultIdList.Add(_id);

            //保存到本地
            saveSetting();
        }

        /// <summary>
        /// 聊天分享的成年子嗣是否已组队
        /// </summary>
        /// <param name="_id"></param>
        public bool getChatShareAdultIsMarried(long _id)
        {
            if (null == _m_settingData)
                return false;

            if (_m_settingData.chatShareHadMarriedAdultIdList == null)
                _m_settingData.chatShareHadMarriedAdultIdList = new List<long>();

            return _m_settingData.chatShareHadMarriedAdultIdList.Contains(_id);
        }

        #region 联谊限制开关

        /// <summary>
        /// 获取联谊限制开关状态
        /// </summary>
        /// <returns>是否开启联谊限制</returns>
        public bool getEngageLimitEnabled()
        {
            if (null == _m_settingData)
                return true;

            return _m_settingData.isEngageLimitEnabled;
        }

        /// <summary>
        /// 设置联谊限制开关状态
        /// </summary>
        /// <param name="_isEnabled">是否开启联谊限制</param>
        public void setEngageLimitEnabled(bool _isEnabled)
        {
            if (null == _m_settingData)
                return;

            if (_m_settingData.isEngageLimitEnabled == _isEnabled)
                return;

            _m_settingData.isEngageLimitEnabled = _isEnabled;

            //保存到本地
            saveSetting();
        }

        #endregion
    }
}