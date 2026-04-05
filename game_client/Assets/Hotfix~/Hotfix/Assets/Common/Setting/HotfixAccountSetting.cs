using System;
using ALPackage;
using GOE;
using IlRuntimeLitJson;
using UnityEngine;

namespace Hotfix
{
    public class HotfixAccountSetting : _AALBasicAccountSettingInfo
    {
        private HotfixAccountSettingData _m_settingData;
        
        public HotfixAccountSetting() : base("Hotfix_cache_account_setting_20250522")
        {
            _m_settingData = new HotfixAccountSettingData();
        }

        protected override string _makeSettingStr()
        {
            try
            {
                string str = JsonMapper.ToJson(_m_settingData);
                
                return str;
            }
            catch (Exception e)
            {
                Debug.LogError($"JsonMapper.ToJson fail : {e}");
            }
            
            return string.Empty;
        }

        protected override void _initSettingStr(string _infoStr)
        {
            if (string.IsNullOrEmpty(_infoStr))
                return;
            try
            {
                _m_settingData = JsonMapper.ToObject<HotfixAccountSettingData>(_infoStr);
            }
            catch (Exception e)
            {
                Debug.LogError($"Hotfix账号相关本地保存反序列化失败:{_infoStr};/n Exception:{e.ToString_ILRuntime()}");
            }
        }
        
        public HotfixAccountSettingData settingData { get { return _m_settingData; } }
        
        public TileMatchEnum.ETileMatch_ModeType getTileMatchModelType()
        {
            if (_m_settingData == null)
                return TileMatchEnum.ETileMatch_ModeType.NORMAL;
            
            return _m_settingData.tileMatchModelType;
        }
        
        public void setTileMatchModelType(TileMatchEnum.ETileMatch_ModeType _type)
        {
            if (_m_settingData == null || _m_settingData.tileMatchModelType == _type)
                return;

            _m_settingData.tileMatchModelType = _type;

            saveSetting();
        }

        public NumMergeEnum.ENumMerge_ModeType getNumMergeModeType()
        {
            if (_m_settingData == null)
                return NumMergeEnum.ENumMerge_ModeType.NORMAL;

            return _m_settingData.numMergeModeType;
        }

        public void setNumMergeModeType(NumMergeEnum.ENumMerge_ModeType _type)
        {
            if (_m_settingData == null || _m_settingData.numMergeModeType == _type)
                return;

            _m_settingData.numMergeModeType = _type;

            saveSetting();
        }

        /// <summary>
        /// 判断是否需要显示首次进入三消活动对话
        /// </summary>
        /// <param name="_startTimeMs">当前</param>
        /// <returns></returns>
        public bool checkNeedShowFirstEnterTileMatchActivityDialog(long _startTimeMs)
        {
            if (_m_settingData == null || _m_settingData.latestFirstEnterTileMatchDialogActivityStartTimeMs == _startTimeMs)
                return false;

            _m_settingData.latestFirstEnterTileMatchDialogActivityStartTimeMs = _startTimeMs;
            saveSetting();
            return true;
        }

        /// <summary>
        /// 判断是否需要显示首次进入2048活动对话
        /// </summary>
        /// <param name="_startTimeMs">当前活动开始时间戳</param>
        /// <returns></returns>
        public bool checkNeedShowFirstEnterNumMergeActivityDialog(long _startTimeMs)
        {
            if (_m_settingData == null || _m_settingData.latestFirstEnterNumMergeDialogActivityStartTimeMs == _startTimeMs)
                return false;

            _m_settingData.latestFirstEnterNumMergeDialogActivityStartTimeMs = _startTimeMs;
            saveSetting();
            return true;
        }
    }
}