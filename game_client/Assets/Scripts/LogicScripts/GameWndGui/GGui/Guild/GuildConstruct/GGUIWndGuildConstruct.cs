using System.Collections.Generic;
using ALPackage;
using Common.GuildEnum;

namespace GOE
{
    /// <summary>
    /// 联盟建设界面
    /// </summary>
    public class GGUIWndGuildConstruct : _ANPGGUIBasicWnd<GGUIMonoGuildConstruct>
    {
        private static GGUIWndGuildConstruct _g_instance = new GGUIWndGuildConstruct();

        public static GGUIWndGuildConstruct instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndGuildConstruct();
                return _g_instance;
            }
        }

        //联盟基础信息
        private GGUIWndGuildSubBaseInfo _m_wSubBaseInfo;
        //金币建设列表
        private GGUIWndGuildConstructContainer _m_wGoldConstructContainer;
        //道具建设列表
        private GGUIWndGuildConstructContainer _m_wItemConstructContainer;

        public GGUIWndGuildConstruct() : base(EALUIWndLayer.NORMAL)
        {

        }

        protected override string _monoAssetPath { get { return GGUIMonoGuildConstruct.assetPath; } }

        protected override string _monoObjName { get { return GGUIMonoGuildConstruct.objName; } }

        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsgAct(WinMsgType.ON_GUILD_CONSTRUCT_INFO_CHG, _onConstructCountChg);//联盟建设次数变更
            WinMsg.RegisterMsgAct(WinMsgType.ON_GUILD_WEALTH_CHG, _onWealthChg);//联盟财富变更
            WinMsg.RegisterMsgAct(WinMsgType.ON_GUILD_SHOW_INFO_CHG, _onGuildShowInfoChg);//联盟展示信息变更
            WinMsg.RegisterMsgAct(WinMsgType.ON_GUILD_CONSTRUCT_DONE, _onGuildConstructDone);//请求建造联盟完成
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.ON_GUILD_CONSTRUCT_INFO_CHG, _onConstructCountChg);//联盟建设次数变更
            WinMsg.UnregisterMsgAct(WinMsgType.ON_GUILD_WEALTH_CHG, _onWealthChg);//联盟财富变更
            WinMsg.UnregisterMsgAct(WinMsgType.ON_GUILD_SHOW_INFO_CHG, _onGuildShowInfoChg);//联盟展示信息变更
            WinMsg.UnregisterMsgAct(WinMsgType.ON_GUILD_CONSTRUCT_DONE, _onGuildConstructDone);//请求建造联盟完成
            _m_wGoldConstructContainer?.hideWnd();
            _m_wItemConstructContainer?.hideWnd();
            _m_wSubBaseInfo?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wGoldConstructContainer?.resetWnd();
            _m_wItemConstructContainer?.resetWnd();
            _m_wSubBaseInfo?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wGoldConstructContainer?.discard();
            _m_wGoldConstructContainer = null;
            _m_wItemConstructContainer?.discard();
            _m_wGoldConstructContainer = null;
            _m_wSubBaseInfo?.discard();
            _m_wSubBaseInfo = null;
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoGoldConstructContainer != null)
                _m_wGoldConstructContainer = new GGUIWndGuildConstructContainer(wnd.monoGoldConstructContainer);

            if (wnd.monoItemConstructContainer != null)
                _m_wItemConstructContainer = new GGUIWndGuildConstructContainer(wnd.monoItemConstructContainer);

            if (wnd.monoBaseInfo != null)
                _m_wSubBaseInfo = new GGUIWndGuildSubBaseInfo(wnd.monoBaseInfo);
        }

        //刷新窗口
        private void _refreshWnd()
        {
            _refreshBaseInfo();
            _refreshList();
            _refreshBar();
        }

        //刷新联盟财富
        private void _refreshBaseInfo()
        {
            if (wnd == null)
                return;

            GuildInfo guildInfo = NPPlayer.instance.guildComp.guildInfo;
            if (guildInfo == null)
                return;

            if (_m_wSubBaseInfo != null)
            {
                _m_wSubBaseInfo.showWnd();
                _m_wSubBaseInfo.setBaseInfo(guildInfo);
                _m_wSubBaseInfo.setWealth(guildInfo.guildWealth);
            }
        }

        //刷新列表
        private void _refreshList()
        {
            if (wnd == null)
                return;


            List<GuildConstructRefObj> refList = GRefdataCoreMgr.instance.guildConstructRefCore.refList;
            if (refList == null)
                return;

            //金币类型建设列表
            List<GuildConstructRefObj> goldRefList = new List<GuildConstructRefObj>();
            //道具类型建设列表
            List<GuildConstructRefObj> itemRefList = new List<GuildConstructRefObj>();
            for (int i = 0; i < refList.Count; i++)
            {
                if (refList[i] == null)
                    continue;

                if (refList[i].type == EGuildConstructType.GOLD)
                    goldRefList.Add(refList[i]);
                else if (refList[i].type == EGuildConstructType.ITEM)
                    itemRefList.Add(refList[i]);
            }

            //刷新列表
            if (_m_wGoldConstructContainer != null)
            {
                _m_wGoldConstructContainer.showWnd();
                _m_wGoldConstructContainer.showItemList(goldRefList);
            }
            if (_m_wItemConstructContainer != null)
            {
                _m_wItemConstructContainer.showWnd();
                _m_wItemConstructContainer.showItemList(itemRefList);
            }
        }

        //刷新列表横栏描述
        private void _refreshBar()
        {
            if (wnd == null)
                return;

            GuildInfo guildInfo = NPPlayer.instance.guildComp.guildInfo;
            if (guildInfo == null)
                return;

            //刷新bar的描述
            Common.GuildObj.Guild_ConstructInfo curGoldConstructInfo = guildInfo.getTodayConstructInfo(EGuildConstructType.GOLD);
            ALUGUICommon.setLabelTxt(wnd.txtGoldBarDesc, TextTranslate.instance.getLanguage(TransKeyConst.guild_todayGoldConstructCount_num_num, curGoldConstructInfo?.getNum() ?? 0, guildInfo.memberCount));
            Common.GuildObj.Guild_ConstructInfo curItemConstructInfo = guildInfo.getTodayConstructInfo(EGuildConstructType.ITEM);
            ALUGUICommon.setLabelTxt(wnd.txtItemBarDesc, TextTranslate.instance.getLanguage(TransKeyConst.guild_todayItemConstructCount_num_num, curItemConstructInfo?.getNum() ?? 0, guildInfo.memberCount));
        }


        #region 消息事件

        //联盟建设次数变更
        private void _onConstructCountChg()
        {
            _refreshBar();
        }

        //联盟财富变更
        private void _onWealthChg()
        {
            _refreshBaseInfo();
        }

        //请求建设联盟完成
        private void _onGuildConstructDone()
        {
            _refreshList();
        }

        //联盟展示信息变更
        private void _onGuildShowInfoChg()
        {
            _refreshBaseInfo();
        }

        #endregion
    }
}