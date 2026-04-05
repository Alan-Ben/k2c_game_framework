using Common.ClientData;

namespace GOE
{
    /// <summary>
    /// 商店的信息
    /// </summary>
    public class GShopRemarkInfo:_ANPRemarkInfo
    {
        /// <summary>
        /// 商店的客户端数据
        /// </summary>
        private Shop_ClientInfo _m_clientInfo;

        public GShopRemarkInfo() : base(ENPClientDataType.SHOP)
        {
            _m_clientInfo = new Shop_ClientInfo();
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

        /// <summary>
        /// 获取商店上次看过的时间戳
        /// </summary>
        /// <param name="_shopId"></param>
        /// <returns></returns>
        public long getShopShowTimeS(long _shopId)
        {
            foreach (Shop_ClientInfoData infoData in _m_clientInfo.getShopInfoData())
            {
                if (infoData.getShopId() != _shopId)
                {
                    continue;
                }
            
                return infoData.getLastShowTimeS();
            }
            return 0;
        }

        /// <summary>
        /// 获取商店下次刷新的时间戳
        /// </summary>
        /// <param name="_shopId"></param>
        /// <returns></returns>
        public long getShopNextRefreshTimeMs(long _shopId)
        {
            foreach (Shop_ClientInfoData infoData in _m_clientInfo.getShopInfoData())
            {
                if (infoData.getShopId() != _shopId)
                {
                    continue;
                }
            
                return infoData.getNextRefreshTimeMs();
            }
            return 0;
        }
        


        /// <summary>
        /// 设置商店上次看过的时间戳
        /// </summary>
        /// <param name="_shopId"></param>
        /// <returns></returns>
        public void setShopShowTimeS(long _shopId, long _lastShowTimeS)
        {
            Shop_ClientInfoData curInfoData = null;
            foreach (Shop_ClientInfoData infoData in _m_clientInfo.getShopInfoData())
            {
                if (infoData.getShopId() != _shopId)
                {
                    continue;
                }

                curInfoData = infoData;
                break;
            }

            if (null == curInfoData)
            {
                curInfoData = new Shop_ClientInfoData();
                curInfoData.setShopId(_shopId);
                curInfoData.setLastShowTimeS(_lastShowTimeS);
                _m_clientInfo.getShopInfoData().Add(curInfoData);
            }
            else
            {
                curInfoData.setLastShowTimeS(_lastShowTimeS);
            }
            saveData();
        }

        /// <summary>
        /// 设置下次刷新时间戳
        /// </summary>
        /// <param name="_shopId"></param>
        /// <param name="_nextRefreshTimeMs"></param>
        public void setShopNextRefreshTimeMs(long _shopId, long _nextRefreshTimeMs)
        {
            Shop_ClientInfoData curInfoData = null;
            foreach (Shop_ClientInfoData infoData in _m_clientInfo.getShopInfoData())
            {
                if (infoData.getShopId() != _shopId)
                {
                    continue;
                }

                curInfoData = infoData;
                break;
            }

            if (null == curInfoData)
            {
                curInfoData = new Shop_ClientInfoData();
                curInfoData.setShopId(_shopId);
                curInfoData.setNextRefreshTimeMs(_nextRefreshTimeMs);
                _m_clientInfo.getShopInfoData().Add(curInfoData);
            }
            else
            {
                curInfoData.setNextRefreshTimeMs(_nextRefreshTimeMs);
            }
            saveData();
        }

        protected override void _resetRemarkInfo()
        {
            _m_clientInfo = new Shop_ClientInfo();
            saveData();
        }
    }
}