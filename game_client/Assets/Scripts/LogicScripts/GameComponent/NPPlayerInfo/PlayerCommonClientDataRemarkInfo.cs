using Common.ClientData;
using JetBrains.Annotations;
using RemarkData;

namespace GOE
{
    /// <summary>
    /// 玩家升级记录数据
    /// </summary>
    public class PlayerCommonClientDataRemarkInfo : _ANPRemarkInfo
    {
        /// <summary>
        /// 玩家升级记录数据
        /// </summary>
        [NotNull] private Common_ClientData _m_clientInfo;

        public PlayerCommonClientDataRemarkInfo() : base(ENPClientDataType.COMMON_CLIENT_DATA)
        {
            _m_clientInfo = new Common_ClientData();
        }

        public void setIsReadStoryVideo()
        {
            _m_clientInfo.setIsReadStoryVideo(true);
            //保存数据
            saveData();
        }
        public bool isReadStoryVideo()
        {
            return _m_clientInfo.getIsReadStoryVideo();
        }

        /// <summary>
        /// 设置是否选择了火星登录地点
        /// </summary>
        public void setIsSelectLandingMarsArea()
        {
            _m_clientInfo.setIsSelectLandingMarsArea(true);
            //保存数据
            saveData();
        }

        /// <summary>
        /// 是否选择了火星登录地点
        /// </summary>
        public bool isSelectLandingMarsArea()
        {
            return _m_clientInfo.getIsSelectLandingMarsArea();
        }

        /// <summary>
        /// 设置是否完成了商店评价
        /// </summary>
        public void setIsFinishStoreReviews(bool _isFinish)
        {
            _m_clientInfo.setIsFinishStoreReviews(_isFinish);
            //保存数据
            saveData();
        }

        /// <summary>
        /// 是否完成了商店评价
        /// </summary>
        public bool isFinishStoreReviews()
        {
            return _m_clientInfo.getIsFinishStoreReviews();
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
            _m_clientInfo = new Common_ClientData();
            saveData();
        }
    }
}