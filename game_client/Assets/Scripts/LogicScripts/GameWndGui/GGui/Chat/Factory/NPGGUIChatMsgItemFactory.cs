
using ChatPackage;
using JetBrains.Annotations;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 聊天列表的东西的工厂
    /// </summary>
    public class NPGGUIChatMsgItemFactory
    {
        private static NPGGUIChatMsgItemFactory _g_instance = new NPGGUIChatMsgItemFactory();
        [NotNull] 
        public static NPGGUIChatMsgItemFactory instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new NPGGUIChatMsgItemFactory();
                return _g_instance;
            }
        }

        // 聊天消息列表的item缓存
        [NotNull] private readonly GUICacheMgrChatMsgItem _m_msgItemCacheMgr;
        // 聊天气泡的缓存
        [NotNull] private readonly NPGGUICacheMgrChatMsgItemTextBubble _m_msgItemTextBubbleCacheMgr; 

        private NPGGUIChatMsgItemFactory()
        {
            _m_msgItemCacheMgr = new GUICacheMgrChatMsgItem();
            _m_msgItemCacheMgr.registerCache<NPGGUIWndChatMsgItemText, NPGGUIMonoChatMsgItemText, NPChatTextMsgDetailInfo>((int) ENPChatMsgType.TEXT, true, NPGGUIMonoChatMsgItemText.myAssetPath, NPGGUIMonoChatMsgItemText.myObjName, GameResCore.instance);
            _m_msgItemCacheMgr.registerCache<NPGGUIWndChatMsgItemText, NPGGUIMonoChatMsgItemText, NPChatTextMsgDetailInfo>((int) ENPChatMsgType.TEXT, false, NPGGUIMonoChatMsgItemText.othersAssetPath, NPGGUIMonoChatMsgItemText.othersObjName, GameResCore.instance);
            _m_msgItemCacheMgr.registerCache<NPGGUIWndChatMsgItemTime, NPGGUIMonoChatMsgItemTime, NPChatTimeMsgItemInfo>((int) ENPChatMsgType.TIME, false, NPGGUIMonoChatMsgItemTime.assetPath, NPGGUIMonoChatMsgItemTime.objName, GameResCore.instance);
            _m_msgItemCacheMgr.registerCache<NPGGUIWndChatMsgItemTime, NPGGUIMonoChatMsgItemTime, NPChatTimeMsgItemInfo>((int) ENPChatMsgType.TIME, true, NPGGUIMonoChatMsgItemTime.assetPath, NPGGUIMonoChatMsgItemTime.objName, GameResCore.instance);
            
            //系统消息
            _m_msgItemCacheMgr.registerCache<NPGGUIWndChatMsgItemSystem, NPGGUIMonoChatMsgItemSystem,  NPChatTextMsgSystemInfo>((int) ENPChatMsgType.SYSTEM, false, NPGGUIMonoChatMsgItemSystem.assetPath, NPGGUIMonoChatMsgItemSystem.systemObjName, GameResCore.instance);

            //宝箱分享
            _m_msgItemCacheMgr.registerCache<NPGGUIWndChatMsgItemCommonBox, NPGGUIMonoChatMsgItemCommonBox, NPChatMsgCommonBoxInfo>((int)ENPChatMsgType.COMM_BOX, true, NPGGUIMonoChatMsgItemCommonBox.myAssetPath, NPGGUIMonoChatMsgItemCommonBox.myObjName, GameResCore.instance);
            _m_msgItemCacheMgr.registerCache<NPGGUIWndChatMsgItemCommonBox, NPGGUIMonoChatMsgItemCommonBox, NPChatMsgCommonBoxInfo>((int)ENPChatMsgType.COMM_BOX, false, NPGGUIMonoChatMsgItemCommonBox.othersAssetPath, NPGGUIMonoChatMsgItemCommonBox.othersObjName, GameResCore.instance);

            //宴会邀请
            _m_msgItemCacheMgr.registerCache<NPGGUIWndChatMsgItemDinnerInvite, NPGGUIMonoChatMsgItemDinnerInvite, NPChatMsgDinnerInviteInfo>((int)ENPChatMsgType.DINNER_INVITE, true, NPGGUIMonoChatMsgItemDinnerInvite.myAssetPath, NPGGUIMonoChatMsgItemDinnerInvite.myObjName, GameResCore.instance);
            _m_msgItemCacheMgr.registerCache<NPGGUIWndChatMsgItemDinnerInvite, NPGGUIMonoChatMsgItemDinnerInvite, NPChatMsgDinnerInviteInfo>((int)ENPChatMsgType.DINNER_INVITE, false, NPGGUIMonoChatMsgItemDinnerInvite.othersAssetPath, NPGGUIMonoChatMsgItemDinnerInvite.othersObjName, GameResCore.instance);

            //表情消息
            _m_msgItemCacheMgr.registerCache<GGUIWndChatMsgItemEmote, GGUIMonoChatMsgItemEmote, ChatEmoteMsgDetailInfo>((int) ENPChatMsgType.EMOTE, true, GGUIMonoChatMsgItemEmote.myAssetPath, GGUIMonoChatMsgItemEmote.myObjName, GameResCore.instance);
            _m_msgItemCacheMgr.registerCache<GGUIWndChatMsgItemEmote, GGUIMonoChatMsgItemEmote, ChatEmoteMsgDetailInfo>((int) ENPChatMsgType.EMOTE, false, GGUIMonoChatMsgItemEmote.othersAssetPath, GGUIMonoChatMsgItemEmote.othersObjName, GameResCore.instance);
            //骑士分享消息
            _m_msgItemCacheMgr.registerCache<GGUIWndChatMsgItemShareHero, GGUIMonoChatMsgItemShareCommon, ChatShareHeroMsgDetailInfo>((int) ENPChatMsgType.SHARE_HERO, true, GGUIMonoChatMsgItemShareCommon.myAssetPath, GGUIMonoChatMsgItemShareCommon.myObjName, GameResCore.instance);
            _m_msgItemCacheMgr.registerCache<GGUIWndChatMsgItemShareHero, GGUIMonoChatMsgItemShareCommon, ChatShareHeroMsgDetailInfo>((int) ENPChatMsgType.SHARE_HERO, false, GGUIMonoChatMsgItemShareCommon.othersAssetPath, GGUIMonoChatMsgItemShareCommon.othersObjName, GameResCore.instance);
            //火星矿分享消息
            _m_msgItemCacheMgr.registerCache<GGUIWndChatMsgItemShareMarsExploreMine, GGUIMonoChatMsgItemMarsExploreMine, ChatShareMarsExploreMineMsgDetailInfo>((int) ENPChatMsgType.SHARE_MARS_EXPLORE_MINE, true, GGUIMonoChatMsgItemMarsExploreMine.myAssetPath, GGUIMonoChatMsgItemMarsExploreMine.myObjName, GameResCore.instance);
            _m_msgItemCacheMgr.registerCache<GGUIWndChatMsgItemShareMarsExploreMine, GGUIMonoChatMsgItemMarsExploreMine, ChatShareMarsExploreMineMsgDetailInfo>((int) ENPChatMsgType.SHARE_MARS_EXPLORE_MINE, false, GGUIMonoChatMsgItemMarsExploreMine.othersAssetPath, GGUIMonoChatMsgItemMarsExploreMine.othersObjName, GameResCore.instance);
            //妃子分享消息
            _m_msgItemCacheMgr.registerCache<GGUIWndChatMsgItemShareConsort, GGUIMonoChatMsgItemShareCommon, ChatShareConsortMsgDetailInfo>((int) ENPChatMsgType.SHARE_CONSORT, true, GGUIMonoChatMsgItemShareCommon.myAssetPath, GGUIMonoChatMsgItemShareCommon.myObjName, GameResCore.instance);
            _m_msgItemCacheMgr.registerCache<GGUIWndChatMsgItemShareConsort, GGUIMonoChatMsgItemShareCommon, ChatShareConsortMsgDetailInfo>((int) ENPChatMsgType.SHARE_CONSORT, false, GGUIMonoChatMsgItemShareCommon.othersAssetPath, GGUIMonoChatMsgItemShareCommon.othersObjName, GameResCore.instance);
            //妃子CG分享消息
            _m_msgItemCacheMgr.registerCache<GGUIWndChatMsgItemShareConsortCG, GGUIMonoChatMsgItemConsortCG, ChatShareConsortCGMsgDetailInfo>((int) ENPChatMsgType.SHARE_CONSORT_CG, true, GGUIMonoChatMsgItemConsortCG.myAssetPath, GGUIMonoChatMsgItemConsortCG.myObjName, GameResCore.instance);
            _m_msgItemCacheMgr.registerCache<GGUIWndChatMsgItemShareConsortCG, GGUIMonoChatMsgItemConsortCG, ChatShareConsortCGMsgDetailInfo>((int) ENPChatMsgType.SHARE_CONSORT_CG, false, GGUIMonoChatMsgItemConsortCG.othersAssetPath, GGUIMonoChatMsgItemConsortCG.othersObjName, GameResCore.instance);
            //子嗣分享消息
            _m_msgItemCacheMgr.registerCache<GGUIWndChatMsgItemShareChild, GGUIMonoChatMsgItemShareCommon, ChatShareChildMsgDetailInfo>((int)ENPChatMsgType.SHARE_CHILD, true, GGUIMonoChatMsgItemShareCommon.myAssetPath, GGUIMonoChatMsgItemShareCommon.myObjName, GameResCore.instance);
            _m_msgItemCacheMgr.registerCache<GGUIWndChatMsgItemShareChild, GGUIMonoChatMsgItemShareCommon, ChatShareChildMsgDetailInfo>((int)ENPChatMsgType.SHARE_CHILD, false, GGUIMonoChatMsgItemShareCommon.othersAssetPath, GGUIMonoChatMsgItemShareCommon.othersObjName, GameResCore.instance);

            // 私聊的联盟公告
            _m_msgItemCacheMgr.registerCache<GGUIWndChatMsgGuildPrivateInform, GGUIMonoChatMsgItemGuildPrivateInform, ChatGuildPrivateInformMsgInfo>((int) ENPChatMsgType.GUILD_PRIVATE_INFORM, true, GGUIMonoChatMsgItemGuildPrivateInform.myAssetPath, GGUIMonoChatMsgItemGuildPrivateInform.myObjName, GameResCore.instance);
            _m_msgItemCacheMgr.registerCache<GGUIWndChatMsgGuildPrivateInform, GGUIMonoChatMsgItemGuildPrivateInform, ChatGuildPrivateInformMsgInfo>((int) ENPChatMsgType.GUILD_PRIVATE_INFORM, false, GGUIMonoChatMsgItemGuildPrivateInform.othersAssetPath, GGUIMonoChatMsgItemGuildPrivateInform.othersObjName, GameResCore.instance);
            
            // 联盟日志消息
            _m_msgItemCacheMgr.registerCache<GGUIWndChatMsgItemGuildLog, GGUIMonoChatMsgItemGuildLog,  ChatGuildLogMsgInfo>((int) ENPChatMsgType.GUILD_LOG, false, GGUIMonoChatMsgItemGuildLog.assetPath, GGUIMonoChatMsgItemGuildLog.objName, GameResCore.instance);
            
            _m_msgItemCacheMgr.registerCache<GGUIWndChatMsgItemGuildRecruit, GGUIMonoChatMsgItemGuildRecruit, ChatGuildRecruitMsgInfo>((int) ENPChatMsgType.GUILD_RECRUIT, true, GGUIMonoChatMsgItemGuildRecruit.myAssetPath, GGUIMonoChatMsgItemGuildRecruit.myObjName, GameResCore.instance);
            _m_msgItemCacheMgr.registerCache<GGUIWndChatMsgItemGuildRecruit, GGUIMonoChatMsgItemGuildRecruit, ChatGuildRecruitMsgInfo>((int) ENPChatMsgType.GUILD_RECRUIT, false, GGUIMonoChatMsgItemGuildRecruit.othersAssetPath, GGUIMonoChatMsgItemGuildRecruit.othersObjName, GameResCore.instance);

            //午间活动宝箱
            _m_msgItemCacheMgr.registerCache<NPGGUIWndChatMsgItemMiddayDungeonBox, NPGGUIMonoChatMsgItemMiddayDungeonBox, NPChatMsgMiddayDungeonBoxInfo>((int)ENPChatMsgType.MIDDAY_DUNGEON_BOX, true, NPGGUIMonoChatMsgItemMiddayDungeonBox.myAssetPath, NPGGUIMonoChatMsgItemMiddayDungeonBox.myObjName, GameResCore.instance);
            _m_msgItemCacheMgr.registerCache<NPGGUIWndChatMsgItemMiddayDungeonBox, NPGGUIMonoChatMsgItemMiddayDungeonBox, NPChatMsgMiddayDungeonBoxInfo>((int)ENPChatMsgType.MIDDAY_DUNGEON_BOX, false, NPGGUIMonoChatMsgItemMiddayDungeonBox.othersAssetPath, NPGGUIMonoChatMsgItemMiddayDungeonBox.othersObjName, GameResCore.instance);

            //晚间活动宝箱
            _m_msgItemCacheMgr.registerCache<NPGGUIWndChatMsgItemEveningDungeonBox, NPGGUIMonoChatMsgItemEveningDungeonBox, NPChatMsgEveningDungeonBoxInfo>((int)ENPChatMsgType.EVENING_DUNGEON_BOX, true, NPGGUIMonoChatMsgItemEveningDungeonBox.myAssetPath, NPGGUIMonoChatMsgItemEveningDungeonBox.myObjName, GameResCore.instance);
            _m_msgItemCacheMgr.registerCache<NPGGUIWndChatMsgItemEveningDungeonBox, NPGGUIMonoChatMsgItemEveningDungeonBox, NPChatMsgEveningDungeonBoxInfo>((int)ENPChatMsgType.EVENING_DUNGEON_BOX, false, NPGGUIMonoChatMsgItemEveningDungeonBox.othersAssetPath, NPGGUIMonoChatMsgItemEveningDungeonBox.othersObjName, GameResCore.instance);

            // 系统消息
            _m_msgItemCacheMgr.registerCache<GGUIWndChatMsgItemSystemLog, GGUIMonoChatMsgItemSystemLog,  ChatSystemLogMsgInfo>((int) ENPChatMsgType.SYSTEM_LOG, false, GGUIMonoChatMsgItemSystemLog.assetPath, GGUIMonoChatMsgItemSystemLog.objName, GameResCore.instance);

            //冲榜宝箱分享
            _m_msgItemCacheMgr.registerCache<GGUIWndChatMsgItemActivityRankBox, GGUIMonoChatMsgItemActivityRankBox, ChatMsgActivityRankBoxInfo>((int)ENPChatMsgType.ACTIVITY_RANK_BOX, true, GGUIMonoChatMsgItemActivityRankBox.myAssetPath, GGUIMonoChatMsgItemActivityRankBox.myObjName, GameResCore.instance);
            _m_msgItemCacheMgr.registerCache<GGUIWndChatMsgItemActivityRankBox, GGUIMonoChatMsgItemActivityRankBox, ChatMsgActivityRankBoxInfo>((int)ENPChatMsgType.ACTIVITY_RANK_BOX, false, GGUIMonoChatMsgItemActivityRankBox.othersAssetPath, GGUIMonoChatMsgItemActivityRankBox.othersObjName, GameResCore.instance);
            // 联盟成员火星矿被攻击
            _m_msgItemCacheMgr.registerCache<GGUIWndChatMsgItemGuildMarsMineOccupy, GGUIMonoChatMsgItemGuildMarsMineOccupy, ChatGuildMarsMineOccupyMsgInfo>((int) ENPChatMsgType.GUILD_MARS_MINE_OCCUPY, false, GGUIMonoChatMsgItemGuildMarsMineOccupy.assetPath, GGUIMonoChatMsgItemGuildMarsMineOccupy.objName, GameResCore.instance);

            _m_msgItemTextBubbleCacheMgr = new NPGGUICacheMgrChatMsgItemTextBubble();
        }

        /// <summary>
        /// 聊天消息列表的item缓存
        /// </summary>
        [NotNull] public GUICacheMgrChatMsgItem msgItemCacheMgr { get { return _m_msgItemCacheMgr; } }
        /// <summary>
        /// 聊天气泡的缓存
        /// </summary>
        [NotNull] public NPGGUICacheMgrChatMsgItemTextBubble msgItemTextBubbleCacheMgr { get { return _m_msgItemTextBubbleCacheMgr; } }
    }
}