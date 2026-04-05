using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 联盟捐赠详情 跟随窗口
    /// </summary>
    public class GGUIWndCommonToolTip_GuildDonateDetailInfo : _ATNPGGUIWndCommonItemToolTip<GGUIMonoCommonToolTip_GuildDonateDetailInfo>
    {
        
        public GGUIWndCommonToolTip_GuildDonateDetailInfo(string _assetPath, string _assetName) : base(_assetPath, _assetName)
        {
            
        }

        protected override void _onWndInitDone()
        {
            base._onWndInitDone();
            
            if(wnd == null)
                return;
        }
        
        protected override void _onDiscard()
        {
            base._onDiscard();
        }
        
        protected override void _onShowWnd()
        {
            base._onShowWnd();
            
        }

        protected override void _onHideWnd()
        {
            base._onHideWnd();
        }

        protected override void _onReset()
        {
            base._onReset();
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo(RectTransform _targetTransRoot, float _intervalX,float _intervalY)
        {
            if(null == wnd)
                return;

            long todayGainGuildExp = 0;//今日获取的联盟经验
            long todayGainGuildWealth = 0;//今日获取的联盟财富
            NPPlayer.instance.guildComp.guildInfo?.getTodayConstructExpAndWealth(out todayGainGuildExp, out todayGainGuildWealth);
            GuildLevelRefObj guildLevelRef = NPPlayer.instance.guildComp.guildInfo.guildLevelRefObj;

            string key = string.IsNullOrEmpty(wnd.txtTodayGainGuildExpKey) ? TransKeyConst.common_value : wnd.txtTodayGainGuildExpKey;
            ALUGUICommon.setLabelTxt(wnd.txtTodayGainGuildExp, TextTranslate.instance.getLanguage(key, todayGainGuildExp));
            
            key = string.IsNullOrEmpty(wnd.txtDailyGainGuildExpLimitKey) ? TransKeyConst.common_value : wnd.txtDailyGainGuildExpLimitKey;
            ALUGUICommon.setLabelTxt(wnd.txtDailyGainGuildExpLimit, TextTranslate.instance.getLanguage(key, guildLevelRef?.construct_gain_guild_exp_limit));

            key = string.IsNullOrEmpty(wnd.txtDailyGainGuildWealthKey) ? TransKeyConst.common_value : wnd.txtDailyGainGuildWealthKey;
            ALUGUICommon.setLabelTxt(wnd.txtDailyGainGuildWealth, TextTranslate.instance.getLanguage(key, todayGainGuildWealth));
            
            key = string.IsNullOrEmpty(wnd.txtDailyGainGuildWealthLimitKey) ? TransKeyConst.common_value : wnd.txtDailyGainGuildWealthLimitKey;
            ALUGUICommon.setLabelTxt(wnd.txtDailyGainGuildWealthLimit, TextTranslate.instance.getLanguage(key, guildLevelRef?.construct_gain_guild_wealth_limit));
            
            //设置位置
            setPos(_targetTransRoot, _intervalX,_intervalY);
        }
        
        protected override void _onClose()
        {
            QueueMgr.instance.forceCloseNodeByType(typeof(GNodeCommonToolTip_GuildDonateDetailInfo));
        }
    }
}