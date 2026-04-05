using System;
using System.Collections.Generic;
using ALPackage;
using GC2GS.p002_InitOp;
using GS2GC.p002_InitOp;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 运营公告的相关配置，此数据要求CDNSetting_ClientConfigInfo和CDNSetting_AreaInfo先下载完成，并且登录服务器获得服务器id
    /// </summary>
    public class CDNSetting_AnnouncementInfo : _ATCDNConfigSetting<List<AnnouncementInfo>>
    {
        private static CDNSetting_AnnouncementInfo _g_instance;
        [NotNull]
        public static CDNSetting_AnnouncementInfo instance
        {
            get
            {
                if(null == _g_instance)
                    _g_instance = new CDNSetting_AnnouncementInfo();
                return _g_instance;
            }
        }

        [NotNull]
        private List<AnnouncementShowInfo> _m_lShowInfoList;//运营公告展示信息列表
        private ImageDownloadProvider _m_imageDownloadProvider;//图片下载管理器
        private string _m_sLastVersion;//上个版本号

        protected CDNSetting_AnnouncementInfo() : base("CDNSetting_AnnouncementInfo")
        {
            _m_lShowInfoList = new List<AnnouncementShowInfo>();
            _m_imageDownloadProvider = new ImageDownloadProvider("announcement");
        }

        /// <summary>
        /// 获取下载的文件路径
        /// </summary>
        /// <returns></returns>
        protected override string _getDownloadFilePath()
        {
            return string.Format("/consult/{0}/{1}/{2}/{3}", 
                CDNSetting_ClientConfigInfo.instance.platformId, 
                CDNSetting_AreaInfo.instance.areaId, 
                GameInit_SelectServer.instance.loginServerLogicId, 
                Game.instance.mainCamera.platInfo.channelId);
        }

        /// <summary>
        /// 获取下载操作的处理对象
        /// </summary>
        /// <returns></returns>
        protected override _ATALCDNDownloadProvider _getURLDownloaderProvider()
        {
            return CDNURLProvider_Client.instance;
        }

        /// <summary>
        /// 在数据清除后的事件
        /// </summary>
        protected override void _onClearData()
        {
            //清除保存的图片
            _m_imageDownloadProvider?.delete();
        }

        /// <summary>
        /// 在数据加载完成后的事件
        /// </summary>
        protected override void _onDataLoaded()
        {
            if (isValid)
            {
                //先判断本地缓存版本号与加载完的数据版本号是否一致，不一致则删除图片资源
                if (string.IsNullOrEmpty(_m_sLastVersion) || !_m_sLastVersion.Equals(data.version))
                    _m_imageDownloadProvider?.delete();

                //构建新的运营公告展示数据
                _m_lShowInfoList.Clear();
                for (int i = 0; i < data.config.Count; i++)
                {
                    AnnouncementShowInfo info = new AnnouncementShowInfo(data.config[i], _m_imageDownloadProvider);
                    _m_lShowInfoList.Add(info);
                    //排序
                    _m_lShowInfoList.Sort(_sortList);
                }

                //发送埋点-CDN-初始化运营公告成功
                GCommon.sendStepReport(TraceConst.START_INIT_ANNOUNCEMENT_SUC.setMarkParam(_m_lShowInfoList.Count));
                //刷新一下ui显示
                GCommon.reloadCustomLoadPrefab();
            }
            else
            {
                //发送埋点-CDN-初始化运营公告失败
                GCommon.sendStepReport(TraceConst.START_INIT_ANNOUNCEMENT_FAIL);
            }
        }

        /// <summary>
        /// 初始化运营公告
        /// </summary>
        /// <param name="_onDone"></param>
        public void initAnnouncement(Action _onDone = null)
        {
            if (!Game.instance.isUseCdn)
                return;

            //发送埋点-CDN-开始初始化运营公告
            GCommon.sendStepReport(TraceConst.START_INIT_ANNOUNCEMENT);

            //先初始化本地存储，记录本地存储的版本号
            init();
            if (settingData != null)
                _m_sLastVersion = settingData.version;

            commonDownload(_onDone);
        }

        /// <summary>
        /// 游戏内更新运营公告
        /// </summary>
        public void updateAnnouncement()
        {
            clearData();
            initAnnouncement();
            //刷新一下红点
            regCDNInitDelegate(AnnouncementMgr.instance.refreshRedTip);
        }

        /// <summary>
        /// 获取有效的运营公告列表
        /// </summary>
        /// <param name="_resultValidList"></param>
        public void getValidAnnouncementList(List<AnnouncementShowInfo> _resultValidList)
        {
            if (_resultValidList == null)
                return;

            _resultValidList.Clear();
            for (int i = 0; i < _m_lShowInfoList.Count; i++)
            {
                if(_m_lShowInfoList[i] == null)
                    continue;

                //是否有效
                if(_m_lShowInfoList[i].isValid())
                    _resultValidList.Add(_m_lShowInfoList[i]);
            }
        }

        /// <summary>
        /// 根据id获取运营公告信息
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        public AnnouncementShowInfo getAnnouncementInfoById(long _id)
        {
            for (int i = 0; i < _m_lShowInfoList.Count; i++)
            {
                if (_m_lShowInfoList[i] != null && _m_lShowInfoList[i].id == _id)
                    return _m_lShowInfoList[i];
            }

            return null;
        }

        /// <summary>
        /// 排序，按照sort从大到小，开始时间从大到小，公告id从大到小
        /// </summary>
        /// <param name="_a"></param>
        /// <param name="_b"></param>
        /// <returns></returns>
        private int _sortList(AnnouncementShowInfo _a, AnnouncementShowInfo _b)
        {
            if (_a == null || _b == null)
                return 0;

            if(_a.sort != _b.sort)
                return -_a.sort.CompareTo(_b.sort);
            else
            {
                DateTime dateTimeA = TimeUtil.parseToLocalTime(_a.startTime, 0);
                DateTime dateTimeB = TimeUtil.parseToLocalTime(_b.startTime, 0);
                if (dateTimeA != dateTimeB)
                    return -dateTimeA.CompareTo(dateTimeB);
                else
                    return -_a.id.CompareTo(_b.id);
            }
        }
    }
}