using ALPackage;

namespace GOE
{
    /// <summary>
    /// 火星航行日志列表item
    /// </summary>
    public class GGUIWndMarsGoToLogGridItem : _ANPGGUIBasicGridItemWnd<GGUIMonoMarsGoToLogGridItem>
    {

        public GGUIWndMarsGoToLogGridItem(GGUIMonoMarsGoToLogGridItem  _wnd) : base(_wnd)
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
        /// 设置日志信息
        /// </summary>
        /// <param name="_info"></param>
        public void setInfo(MarsStageLogInfo _info)
        {
            if (_info == null || _info.marsGoRouteLogRef == null || wnd == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtTitle, TextTranslate.instance.getLanguage(_info.marsGoRouteLogRef.log_title, _info.marsGoRouteLogRef.log_title_args));
            ALUGUICommon.setLabelTxt(wnd.txtContent, TextTranslate.instance.getLanguage(_info.marsGoRouteLogRef.log_content, _info.marsGoRouteLogRef.log_content_args));

            long firstArrivedTimeMs = NPPlayer.instance.marsComp.goToSubComponent.firstStageArrivedTimeMs;
            ALUGUICommon.setLabelTxt(wnd.txtTime, TextTranslate.instance.getLanguage(TransKeyConst.marsGoTo_navigationTime_str, TimeUtil.millisecondsToTime_Two_NoSec(_info.triggerTimeMs - firstArrivedTimeMs)));
        }
    }
}
