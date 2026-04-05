using ALPackage;
using Common.GuildCooperateObj;

namespace GOE
{
    /// <summary>
    /// 公会协作日志列表item
    /// </summary>
    public class GGUIWndGuildCooperateLogGridItem : _ANPGGUIBasicGridItemWnd<GGUIMonoGuildCooperateLogGridItem>
    {

        public GGUIWndGuildCooperateLogGridItem(GGUIMonoGuildCooperateLogGridItem  _wnd)
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
        public void setInfo(GuildCooperate_AttackLog _logInfo)
        {
            if (wnd == null || _logInfo == null)
                return;

            string time = TimeUtil.DateTime2StringMDYHMS(TimeUtil.FromUTCMilliseconds(_logInfo.getTimeMs()));
            string playerName = _logInfo.getName();
            GuildCooperateAreaPosRefObj rewardPosRef = GRefdataCoreMgr.instance.guildCooperateAreaPosRefCore.getRef(_logInfo.getPosId());
            string rewardPosName = TextTranslate.instance.getLanguage(rewardPosRef?.name);
            BasicAttrRefObj basicAttrRef = GRefdataCoreMgr.instance.basicAttrRefCore.getRef((long) _logInfo.getAttr());
            string attrPosName = TextTranslate.instance.getLanguage(basicAttrRef?.guild_cooperate_attr_pos_name);
            string attackValue = _logInfo.getAttackHp().ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT);

            //{0} {1}增加了{2}-{3}的{4}点建设值
            ALUGUICommon.setLabelTxt(wnd.txtLog, TextTranslate.instance.getLanguage(
                TransKeyConst.guildCooperate_logDesc_time_name_str_str_num,
                time,
                playerName,
                rewardPosName,
                attrPosName,
                attackValue));
        }
    }
}
