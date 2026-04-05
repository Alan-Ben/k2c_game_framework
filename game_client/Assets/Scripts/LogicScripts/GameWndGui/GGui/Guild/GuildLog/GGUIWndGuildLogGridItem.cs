using ALPackage;
using Common.GuildObj;

namespace GOE
{
    /// <summary>
    /// 联盟日志列表item
    /// </summary>
    public class GGUIWndGuildLogGridItem : _ANPGGUIBasicGridItemWnd<GGUIMonoGuildLogGridItem>
    {
        public GGUIWndGuildLogGridItem(GGUIMonoGuildLogGridItem _wnd)
           : base(_wnd) 
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
        }

        protected override void _resetGridItem()
        {
        }

        protected override void _onDiscard()
        {
        }

        protected override void _onWndInitDone()
        {
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_logInfo"></param>
        public void setInfo(Guild_LogInfo _logInfo)
        {
            if (wnd == null || _logInfo == null)
                return;

            _IGuildLogMsgDetailInfo targetInfo = GuildLogMsgDetailFactory.getGuildLogMsgDetailInfo(_logInfo.getLogType(), _logInfo.getData());
            if (targetInfo == null)
                return;

            string timeStr = TimeUtil.DateTime2StringHM(TimeUtil.FromUTCByTimeZone(_logInfo.getSendTimeMs()));
            ALUGUICommon.setLabelTxt(wnd.txtContent, TextTranslate.instance.getLanguage(TransKeyConst.guild_logContent_time_string, timeStr, targetInfo.content));
        }
    }
}
