using Common.ClientData;

namespace GOE
{
    /// <summary>
    /// 运营公告记录的信息
    /// </summary>
    public class AnnouncementRemarkInfo : _ANPRemarkInfo
    {
        //已读记录信息
        private Announcement_ClientData _m_announcementClientInfo;
        //数据是否清理过，每次启动游戏第一次设置已读时会清理一次，避免数据过多
        private bool _m_bIsClear;

        /// <summary>
        /// 已读记录信息
        /// </summary>
        public Announcement_ClientData announcementClientInfo { get { return _m_announcementClientInfo; } }

        public AnnouncementRemarkInfo() : base(ENPClientDataType.ANNOUNCEMENT)
        {
            _m_announcementClientInfo = new Announcement_ClientData();
            _m_bIsClear = false;
        }

        /// <summary>
        /// 记录最大的运营公告id
        /// </summary>
        public void setMaxId()
        {
            if (_m_announcementClientInfo == null)
                _m_announcementClientInfo = new Announcement_ClientData();

            long maxId = 0;
            if (CDNSetting_AnnouncementInfo.instance.data != null && CDNSetting_AnnouncementInfo.instance.data.config != null)
            {
                for (int i = 0; i < CDNSetting_AnnouncementInfo.instance.data.config.Count; i++)
                {
                    if (CDNSetting_AnnouncementInfo.instance.data.config[i].id > maxId)
                        maxId = CDNSetting_AnnouncementInfo.instance.data.config[i].id;
                }
            }

            if (maxId > _m_announcementClientInfo.getMaxId())
            {
                _m_announcementClientInfo.setMaxId(maxId);
                saveData();
            }
        }

        /// <summary>
        /// 添加已读id
        /// </summary>
        public void addReadId(long _id)
        {
            if(!_m_bIsClear)
                _clearData();

            if (_m_announcementClientInfo == null)
                _m_announcementClientInfo = new Announcement_ClientData();

            if (!_m_announcementClientInfo.getReadIdList().Contains(_id))
            {
                _m_announcementClientInfo.addReadIdList(_id);
                saveData();
            }
        }

        /// <summary>
        /// 检查是否已读
        /// </summary>
        public bool isRead(long _id)
        {
            if (_m_announcementClientInfo == null || _m_announcementClientInfo.getReadIdList() == null)
                return false;

            return _m_announcementClientInfo.getReadIdList().Contains(_id);
        }

        /// <summary>
        /// 检查是否有新增新的运营公告
        /// </summary>
        /// <returns></returns>
        public bool haveNewAnnouncement()
        {
            if (_m_announcementClientInfo == null)
                return true;

            //查找最大的id，用于判断是否有新增运营公告
            long maxId = 0;
            if (CDNSetting_AnnouncementInfo.instance.data != null && CDNSetting_AnnouncementInfo.instance.data.config != null)
            {
                for (int i = 0; i < CDNSetting_AnnouncementInfo.instance.data.config.Count; i++)
                {
                    if (CDNSetting_AnnouncementInfo.instance.data.config[i].id > maxId)
                        maxId = CDNSetting_AnnouncementInfo.instance.data.config[i].id;
                }
            }

            //如果记录的比现在的id小，说明有新增运营公告
            return _m_announcementClientInfo.getMaxId() < maxId;
        }

        /// <summary>
        /// 删除一下已经不在运营公告列表里的数据
        /// </summary>
        private void _clearData()
        {
            if (_m_announcementClientInfo == null || _m_announcementClientInfo.getReadIdList() == null || _m_announcementClientInfo.getReadIdList().Count == 0)
                return;

            _m_bIsClear = true;

            //是否有变化
            bool _isChg = false;
            for (int i = _m_announcementClientInfo.getReadIdList().Count - 1; i >= 0 ; i--)
            {
                //数据不存在，说明已经删除
                if (CDNSetting_AnnouncementInfo.instance.getAnnouncementInfoById(_m_announcementClientInfo.getReadIdList()[i]) == null)
                {
                    _isChg = true;
                    _m_announcementClientInfo.getReadIdList().RemoveAt(i);
                }
            }
            if(_isChg)
                saveData();
        }

        protected override byte[] _makeData()
        {
            if (_m_announcementClientInfo == null)
                return null;

            return _m_announcementClientInfo.makePackage();
        }

        protected override void _readData(byte[] _data)
        {
            if (_m_announcementClientInfo == null || _data == null)
                return;

            _m_announcementClientInfo.readPackage(_data);
        }
        
        protected override void _resetRemarkInfo()
        {
            _m_announcementClientInfo = new Announcement_ClientData();
            saveData();
        }
    }
}