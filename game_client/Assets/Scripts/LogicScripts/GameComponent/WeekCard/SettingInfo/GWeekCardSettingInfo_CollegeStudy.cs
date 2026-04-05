using System;
using System.Collections.Generic;
using Common.WeekCardObj;
using GS2GC.p004_PlayerOp;

namespace GOE
{
    /// <summary>
    /// 周卡设置-大学学习
    /// </summary>
    public class GWeekCardSettingInfo_CollegeStudy:_AGWeekCardSettingInfoBase
    {
        private WeekCard_ExtraInfo_CollegeStudy _m_exInfo;
        
        public GWeekCardSettingInfo_CollegeStudy(WeekCard_SingleSettingInfo _info) : base(_info)
        {
        }
        public WeekCard_ExtraInfo_CollegeStudy exInfo { get => _m_exInfo; }
        public bool isClose { get => _m_exInfo.getIsClose(); }

        protected override void _initExInfo(byte[] _exInfo)
        {
            _m_exInfo = new WeekCard_ExtraInfo_CollegeStudy();
            if(null == _exInfo || _exInfo.Length == 0)
                return;
            _m_exInfo.readPackage(_exInfo);
        }

        public void setExInfo(List<long> _heroList, Action<GS2GC_004_016_RetWeekCardChgSetting> _backAction = null)
        {
            _m_exInfo.getHeroList().Clear();
            _m_exInfo.getHeroList().AddRange(_heroList);
            
            reqWeekCardChgSetting(_backAction);
        }
        /// <summary>
        /// 设置是否关闭
        /// </summary>
        /// <param name="_isClose"></param>
        /// <param name="_backAction"></param>
        public void setIsClose(bool _isClose, Action<GS2GC_004_016_RetWeekCardChgSetting> _backAction = null)
        {
            _m_exInfo.setIsClose(_isClose);
            reqWeekCardChgSetting(_backAction);
        }

        protected override byte[] _makeExInfoPackage()
        {
            return _m_exInfo.makePackage();
        }
    }
}