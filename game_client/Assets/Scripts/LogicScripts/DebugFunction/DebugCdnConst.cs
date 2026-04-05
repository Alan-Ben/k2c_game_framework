namespace GOE
{
    /* pc 端"偷渡"操作,_openDebugCDN文件存放位置为C:\Users\mj\AppData\LocalLow\mechanist\Game of Khans
     *
     *
     */
    //debugCdn配置文件用到的key，主要给qa看
    public static class DebugCdnConst
    {
        public const string CDN_URL = "cdnurl";//cdn地址
        public const string CHANNEL_ID = "channelid";//渠道id
        public const string INJECTFIX_URL = "injectfixurl";//if地址
        public const string HOTFIX_URL = "hotfixurl";//hotfix地址
        public const string GAMERES_URL = "gameresurl";//资源地址
        public const string REFDATA_URL = "refdataurl";//refdata地址
        public const string NOTICERES_URL = "noticeresurl";//提示资源地址
        public const string AUDIO_URL = "audiourl";//音效资源地址
        public const string VIDEO_URL = "videourl";//视频资源地址
    }
}
 