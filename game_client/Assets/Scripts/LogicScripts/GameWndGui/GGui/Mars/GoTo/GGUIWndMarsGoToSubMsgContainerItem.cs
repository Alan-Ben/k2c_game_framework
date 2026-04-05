using ALPackage;
using Common.MarsObj;

namespace GOE
{
    /// <summary>
    /// 前往火星留言容器item
    /// </summary>
    public class GGUIWndMarsGoToSubMsgContainerItem : _ATALBasicUISubWnd<GGUIMonoMarsGoToSubMsgContainerItem>
    {
        public GGUIWndMarsGoToSubMsgContainerItem(GGUIMonoMarsGoToSubMsgContainerItem _mono) : base(_mono)
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

        protected override void _onDiscard()
        {
        }

        protected override void _onWndInitDone()
        {
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_info"></param>
        public void setInfo(Mars_GoRoute_StageMsg _info, int _stageId)
        {
            if (wnd == null || _info == null)
                return;

            MarsGoRouteRefObj marsGoRouteRef = GRefdataCoreMgr.instance.getMarsGoRouteRefByStageId(_stageId);
            ALUGUICommon.setLabelTxt(wnd.txtContent, _info.getContent());
            ALUGUICommon.setLabelTxt(wnd.txtTimeDesc, TextTranslate.instance.getLanguage(TransKeyConst.marsGoto_msgSendTimeDesc_time_str_str, 
                TimeUtil.DateTime2StringMDYHM(TimeUtil.FromUTCMilliseconds(_info.getTimeMs())), 
                _info.getPlayerName(), 
                TextTranslate.instance.getLanguage(marsGoRouteRef?.msg_stage_name)));
        }
    }
}