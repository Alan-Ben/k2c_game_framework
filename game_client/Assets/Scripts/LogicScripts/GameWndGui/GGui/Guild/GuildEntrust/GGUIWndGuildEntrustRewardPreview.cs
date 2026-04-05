using ALPackage;

namespace GOE
{
    /// <summary>
    /// 联盟委托奖励预览
    /// </summary>
    public class GGUIWndGuildEntrustRewardPreview : _ANPGGUIBasicWnd<GGUIMonoGuildEntrustRewardPreview>
    {
        private static GGUIWndGuildEntrustRewardPreview _g_instance;

        public static GGUIWndGuildEntrustRewardPreview instance { get { return _g_instance ??= new GGUIWndGuildEntrustRewardPreview(); } }

        private NPGGUIWndCommonItemContainer _m_wRewardContainer;//奖励列表
        
        public GGUIWndGuildEntrustRewardPreview() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoGuildEntrustRewardPreview.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoGuildEntrustRewardPreview.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.monoRewardContainer != null)
                _m_wRewardContainer = new NPGGUIWndCommonItemContainer(wnd.monoRewardContainer);
        }
        
        protected override void _onDiscard()
        {
            _m_wRewardContainer?.discard();
            _m_wRewardContainer = null;
        }
        
        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_wRewardContainer?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wRewardContainer?.resetWnd();
        }

        private void _refreshWnd()
        {
            GuildInfo guildInfo = NPPlayer.instance.guildComp.guildInfo;
            if(wnd == null || guildInfo == null || guildInfo.guildEntrustInfo == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtGuildTotalEarnings, guildInfo.totalEarnings.ToLargeString(PrimitiveExtension.ELargeStringType.GOLD));
            ALUGUICommon.setLabelTxt(wnd.txtPerDealGainSilver, guildInfo.guildEntrustInfo.getPerDealGainCoin().ToLargeString(PrimitiveExtension.ELargeStringType.GOLD));

            if (_m_wRewardContainer != null)
            {
                _m_wRewardContainer.showWnd();
                _m_wRewardContainer.showItemList(guildInfo.guildEntrustInfo.entrustQualityRefObj?.reawrd_item_list);
            }
        }
    }
}