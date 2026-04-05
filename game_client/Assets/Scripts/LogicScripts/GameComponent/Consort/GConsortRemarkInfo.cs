using Common.ClientData;

namespace GOE
{
    /// <summary>
    /// 妃子的信息
    /// </summary>
    public class GConsortRemarkInfo:_ANPRemarkInfo
    {
        /// <summary>
        /// 查看一键约会的解锁特效
        /// </summary>
        private Consort_ClientInfo _m_clientInfo;

        public GConsortRemarkInfo() : base(ENPClientDataType.CONSORT)
        {
            _m_clientInfo = new Consort_ClientInfo();
        }

        /// <summary>
        /// 是否展示过一键约会的解锁特效
        /// </summary>
        /// <returns></returns>
        public bool isShowAKeyUnlockSfx()
        {
            return _m_clientInfo.getIsShowAKeyUnlockSfx();
        }

        //设置是否展示过一键约会的解锁特效
        public void setIsShowAKeyUnlockSfx(bool _isShowAKeyUnlockSfx)
        {
            _m_clientInfo.setIsShowAKeyUnlockSfx(_isShowAKeyUnlockSfx);

            //保存数据
            saveData();
        }

        protected override byte[] _makeData()
        {   
            
            return _m_clientInfo.makePackage();
        }

        protected override void _readData(byte[] _data)
        {
            if (_m_clientInfo != null && null != _data) 
                _m_clientInfo.readPackage(_data);
        }
        
        protected override void _resetRemarkInfo()
        {
            _m_clientInfo = new Consort_ClientInfo();
            saveData();
        }

        /// <summary>
        /// 妃子解锁的皮肤是否已经看过
        /// </summary>
        /// <param name="_consortId"></param>
        /// <param name="_skinId"></param>
        /// <returns></returns>
        public bool isConsortSkinUnlockLooked(long _consortId, long _skinId)
        {
            foreach (Consort_ClientInfoData infoData in _m_clientInfo.getConsortInfoData())
            {
                if (infoData.getConsortId() != _consortId)
                {
                    continue;
                }

                return infoData.getLookedSkinIdList().Contains(_skinId);
            }
            return false;
        }
        

        /// <summary>
        /// 妃子解锁的皮肤是否已经看过
        /// </summary>
        /// <param name="_consortId"></param>
        /// <param name="_skinId"></param>
        /// <returns></returns>
        public void setConsortSkinUnlockLooked(long _consortId, long _skinId)
        {
            Consort_ClientInfoData curInfoData = null;
            foreach (Consort_ClientInfoData infoData in _m_clientInfo.getConsortInfoData())
            {
                if (infoData.getConsortId() != _consortId)
                {
                    continue;
                }

                curInfoData = infoData;
                break;
            }

            if (null == curInfoData)
            {
                curInfoData = new Consort_ClientInfoData();
                curInfoData.setConsortId(_consortId);
                curInfoData.getLookedSkinIdList().Add(_skinId);
                _m_clientInfo.getConsortInfoData().Add(curInfoData);
            }
            else
            {
                if(!curInfoData.getLookedSkinIdList().Contains(_skinId))
                    curInfoData.getLookedSkinIdList().Add(_skinId);
            }
            saveData();
        }

        /// <summary>
        /// 亲密历程的解锁是否已经看过
        /// </summary>
        /// <param name="_consortId"></param>
        /// <param name="_experienceId"></param>
        /// <returns></returns>
        public bool isConsortExperienceUnlockLooked(long _consortId, long _experienceId)
        {
            foreach (Consort_ClientInfoData infoData in _m_clientInfo.getConsortInfoData())
            {
                if (infoData.getConsortId() != _consortId)
                {
                    continue;
                }

                return infoData.getLookedExperienceIdList().Contains(_experienceId);
            }
            return false;
        }

        /// <summary>
        /// 亲密历程的解锁是否已经看过
        /// </summary>
        /// <param name="_consortId"></param>
        /// <param name="_experienceId"></param>
        /// <returns></returns>
        public void setConsortExperienceUnlockLooked(long _consortId, long _experienceId)
        {
            Consort_ClientInfoData curInfoData = null;
            foreach (Consort_ClientInfoData infoData in _m_clientInfo.getConsortInfoData())
            {
                if (infoData.getConsortId() != _consortId)
                {
                    continue;
                }

                curInfoData = infoData;
                break;
            }

            if (null == curInfoData)
            {
                curInfoData = new Consort_ClientInfoData();
                curInfoData.setConsortId(_consortId);
                curInfoData.getLookedExperienceIdList().Add(_experienceId);
                _m_clientInfo.getConsortInfoData().Add(curInfoData);
            }
            else
            {
                if(!curInfoData.getLookedExperienceIdList().Contains(_experienceId))
                    curInfoData.getLookedExperienceIdList().Add(_experienceId);
            }
            saveData();
        }
        
        /// <summary>
        /// 获取妃子入口展示的妃子ID
        /// </summary>
        /// <returns></returns>
        public long getConsortEntranceShowConsortId()
        {
            return _m_clientInfo?.getConsortEntranceShowConsortId() ?? 0;
        }

        /// <summary>
        /// 设置妃子入口展示的妃子ID
        /// </summary>
        public void setConsortEntranceShowConsortId(long _consortId)
        {
            if(_m_clientInfo == null || _m_clientInfo.getConsortEntranceShowConsortId() == _consortId)
                return;
            
            _m_clientInfo.setConsortEntranceShowConsortId(_consortId);
            WinMsg.SendMsg(WinMsgType.ON_CONSORT_ENTRANCE_CONSORT_CHG);
            
            saveData();
        }
    }
}