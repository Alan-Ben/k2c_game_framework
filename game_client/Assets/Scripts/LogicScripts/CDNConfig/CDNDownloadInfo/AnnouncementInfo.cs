using System;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 运营公告
    /// </summary>
    [Serializable]
    public class AnnouncementInfo
    {
        public int id;//公告id(运营平台中的自增ID)
        public int sort;//排序，越大越前面
        public string start_time;//公告开始时间（大区时区）
        public string end_time;//公告结束时间（大区时区）
        public string banner;//banner图下载地址
        public string icon;//icon图下载地址
        public int default_banner;//游戏配表banner图片id
        public int default_icon;//游戏配表icon图片id
        public bool ios = true;//ios设备是否展示
        public bool android = true;//安卓设备是否展示
        public bool pc = true;//pc设备是否展示
        public List<AnnouncementContent> content_list;//运营公告内容
        public List<AnnouncementReward> reward_list;//运营公告奖励（暂时用不到）

        public override string ToString()
        {
            return $"[{nameof(id)}: {id}], [{nameof(sort)}: {sort}], [{nameof(start_time)}: {start_time}], [{nameof(end_time)}: {end_time}], [{nameof(banner)}: {banner}], [{nameof(icon)}: {icon}], [{nameof(default_banner)}: {default_banner}], [{nameof(default_icon)}: {default_icon}], [{nameof(ios)}: {ios}], [{nameof(android)}: {android}], [{nameof(pc)}: {pc}], [{nameof(content_list)}: {string.Join(", ", content_list)}], [{nameof(reward_list)}: {string.Join(", ", reward_list)}]";
        }
    }

    /// <summary>
    /// 运营公告内容
    /// </summary>
    [Serializable]
    public class AnnouncementContent
    {
        public string language;//语言id
        public string icon_text;//ICON 文字描述
        public string title;//标题
        public string content;//内容
        public int redirect_to_game;//内链跳转
        public string redirect_url;//外链地址

        public override string ToString()
        {
            return $"[{nameof(language)}: {language}], [{nameof(icon_text)}: {icon_text}], [{nameof(title)}: {title}], [{nameof(content)}: {content}], [{nameof(redirect_to_game)}: {redirect_to_game}], [{nameof(redirect_url)}: {redirect_url}]";
        }
    }

    /// <summary>
    /// 运营公告奖励
    /// </summary>
    [Serializable]
    public class AnnouncementReward
    {
        public string subId;//item
        public int count;//数量

        public override string ToString()
        {
            return $"[{nameof(subId)}: {subId}], [{nameof(count)}: {count}]";
        }
    }




    /// <summary>
    /// 运营公告展示数据
    /// </summary>
    public class AnnouncementShowInfo
    {
        private long _m_lId;//公告id(运营平台中的自增ID)
        private int _m_iSort;//排序，越大越前面
        private string _m_dStartTime;//公告开始时间（大区时区）
        private string _m_dEndTime;//公告结束时间（大区时区）
        private string _m_sBannerImgDownloadUrl;//banner图下载地址
        private string _m_sIconImgDownloadUrl;//icon图下载地址
        private long _m_lDefaultBannerId;//游戏配表banner图片id
        private long _m_lDefaultIconId;//游戏配表icon图片id
        private bool _m_bCanShow;//是否可以展示
        private List<AnnouncementContent> _m_lContentList;//公告内容列表
        private ImageDownloadProvider _m_imageDownloadProvider;//图片下载管理器

        /// <summary>
        /// 公告id
        /// </summary>
        public long id { get { return _m_lId; } }
        /// <summary>
        /// 排序，越大越前面
        /// </summary>
        public int sort { get { return _m_iSort; } }
        /// <summary>
        /// 公告开始时间
        /// </summary>
        public string startTime { get { return _m_dStartTime; } }
        /// <summary>
        /// 公告结束时间
        /// </summary>
        public string endTime { get { return _m_dEndTime; } }
        /// <summary>
        /// banner图下载地址
        /// </summary>
        public string bannerImgDownloadUrl { get { return _m_sBannerImgDownloadUrl; } }
        /// <summary>
        /// icon图下载地址
        /// </summary>
        public string iconImgDownloadUrl { get { return _m_sIconImgDownloadUrl; } }
        /// <summary>
        /// 游戏配表banner图片id
        /// </summary>
        public long defaultBannerId { get { return _m_lDefaultBannerId; } }
        /// <summary>
        /// 游戏配表icon图片id
        /// </summary>
        public long defaultIconId { get { return _m_lDefaultIconId; } }


        public AnnouncementShowInfo(AnnouncementInfo _info, ImageDownloadProvider _imageDownloadProvider)
        {
            if (_info == null)
                return;

            _m_lId = _info.id;
            _m_iSort = _info.sort;
            _m_dStartTime = _info.start_time;
            _m_dEndTime = _info.end_time;
            _m_sBannerImgDownloadUrl = _info.banner;
            _m_sIconImgDownloadUrl = _info.icon;
            _m_lDefaultBannerId = _info.default_banner;
            _m_lDefaultIconId = _info.default_icon;
            _m_bCanShow = true;
#if UNITY_IOS
                _m_bCanShow = _info.ios;
#elif UNITY_ANDROID
                _m_bCanShow = _info.android;
#elif UNITY_STANDALONE || UNITY_EDITOR
            _m_bCanShow = _info.pc;
#endif
            _m_lContentList = _info.content_list;
            _m_imageDownloadProvider = _imageDownloadProvider;
        }

        /// <summary>
        /// 当前是否有效
        /// </summary>
        /// <returns></returns>
        public bool isValid()
        {
            //时间是否有效，运营后台配置时间不加时区直接与服务器时间比较，比如后台配置12点就是按服务器时间是否12点判断
            DateTime serverNow = TimeUtil.FromUTCByTimeZone(FpsAndPingMgr.instance.serverTimeTag);
            if (serverNow < TimeUtil.parseToLocalTime(_m_dStartTime, 0) || serverNow >= TimeUtil.parseToLocalTime(_m_dEndTime, 0))
                return false;

            //是否可以展示
            return _m_bCanShow;
        }

        /// <summary>
        /// 是否已读
        /// </summary>
        /// <returns></returns>
        public bool isRead()
        {
            return AnnouncementMgr.instance.isRead(_m_lId);
        }

        /// <summary>
        /// 设置已读
        /// </summary>
        public void setIsRead()
        {
            AnnouncementMgr.instance.setIsRead(_m_lId);
        }

        /// <summary>
        /// 获取对应语言的公告
        /// </summary>
        /// <param name="_lan"></param>
        /// <returns></returns>
        public AnnouncementContent getCurLanguageContent()
        {
            string targetLanguage = GameSetting.instance.getCurrentLanguage().toPHPLanguageCode();
            AnnouncementContent tmpContent = null;
            for (int i = 0; i < _m_lContentList.Count; i++)
            {
                tmpContent = _m_lContentList[i];
                if (null == tmpContent)
                    continue;

                //判断语言
                if (tmpContent.language == targetLanguage)
                    return tmpContent;
            }

            //如果没有符合的则使用第一个
            if (_m_lContentList.Count <= 0)
                return null;

            return _m_lContentList[0];
        }

        /// <summary>
        /// 获取页签图标下载的图片
        /// </summary>
        /// <returns></returns>
        public void getIconImageBytes(Action<byte[]> _downloadSucAction)
        {
            if (string.IsNullOrEmpty(_m_sIconImgDownloadUrl))
            {
                _downloadSucAction?.Invoke(null);
                return;
            }

            _m_imageDownloadProvider?.loadImage(_m_sIconImgDownloadUrl, _downloadSucAction);
        }

        /// <summary>
        /// 获取公告下载的图片
        /// </summary>
        /// <returns></returns>
        public void getBannerImageBytes(Action<byte[]> _downloadSucAction)
        {
            if (string.IsNullOrEmpty(_m_sBannerImgDownloadUrl))
            {
                _downloadSucAction?.Invoke(null);
                return;
            }

            _m_imageDownloadProvider?.loadImage(_m_sBannerImgDownloadUrl, _downloadSucAction);
        }
    }
}