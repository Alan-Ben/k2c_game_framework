using System;
using ALPackage;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class AccountSettingMgr
    {
        private static AccountSettingMgr _g_instance;

        [NotNull]
        public static AccountSettingMgr instance
        {
            get
            {
                if(null == _g_instance)
                {
                    _g_instance = new AccountSettingMgr();
                }
                return _g_instance;
            }
        }


        //是否初始化了数据
        private bool _m_bIsInited;

        //跟随账号相关
        private AccountSetting _m_accountSetting;
        private NewTagSaver _m_newTagSaver;//新标签缓存
        private WarningTipSaver _m_warningTipSaver;//警告提示
        private PlayerInfoSaver _m_playerInfoSaver;//其他玩家信息
        private DailyTagSetting _m_dailyTagSaver;//每日标记信息
        private CommonEventSaver _m_commonEventSaver;//通用事件信息存储器
        private DinnerSetting _m_dinnerSaver;//宴会相关信息存储
        private MiddayDungeonSetting _m_middayDungeonSaver;//午间副本相关信息存储
        private GachaSetting _m_gachaSetting;//抽卡相关信息存储
        private ConsortMomentSaverMgr _m_consortMomentSaverMgr;
        private ConsortAIChatSaverMgr _m_consortAIChatSaverMgr;
        private ConsortPresetChatSaver _m_consortPresetChatSaver;
        private ChildSaver _m_childSaver;//子嗣相关信息存储
        private GuildSaver _m_guildSaver;//联盟相关信息存储
        private UnreadMarsBuildingChangeSaver _m_unreadMarsBuildingChangeSaver;//火星建筑未读变化信息存储
        private BagItemWndRedTipSaver _m_bagItemWndRedTipSaver;//背包窗口红点信息存储

        private AccountSettingMgr()
        {
            _m_bIsInited = false;
        }

        public bool isInited { get { return _m_bIsInited; } }
        public AccountSetting accountSetting { get { return _m_accountSetting; } }
        public NewTagSaver newTagSaver { get { return _m_newTagSaver; } }
        public WarningTipSaver warningTipSaver { get { return _m_warningTipSaver; } }        
        public PlayerInfoSaver playerInfoSaver { get { return _m_playerInfoSaver; } }
        public DailyTagSetting dailyTagSaver { get { return _m_dailyTagSaver; } }
        public CommonEventSaver commonEventSaver { get { return _m_commonEventSaver; } }
        
        public DinnerSetting dinnerSaver { get { return _m_dinnerSaver; } }
        public MiddayDungeonSetting middayDungeonSaver { get { return _m_middayDungeonSaver; } }
        
        public GachaSetting gachaSetting { get { return _m_gachaSetting; } }
        
        public ConsortMomentSaverMgr consortMomentSaverMgr { get { return _m_consortMomentSaverMgr; } }
        public ConsortAIChatSaverMgr consortAIChatSaverMgr { get { return _m_consortAIChatSaverMgr; } }
        public ConsortPresetChatSaver consortPresetChatSaver { get { return _m_consortPresetChatSaver; } }
        public ChildSaver childSaver { get { return _m_childSaver; } }
        public GuildSaver guildSaver { get { return _m_guildSaver; } }
        public UnreadMarsBuildingChangeSaver unreadMarsBuildingChangeSaver { get { return _m_unreadMarsBuildingChangeSaver; } }
        public BagItemWndRedTipSaver bagItemWndRedTipSaver { get { return _m_bagItemWndRedTipSaver; } }

        public void init()
        {
            //不进行多次初始化
            if (_m_bIsInited)
                return;
            _m_bIsInited = true;

            try
            {
                
                try
                {
                    _m_accountSetting = new AccountSetting(NPPlayer.instance.playerInfo.CID);
                    _m_accountSetting.init();
                }
                catch (Exception e)
                {
                    Debug.LogError($"{_m_accountSetting.GetType()} init时出错：{e}");
                    _m_accountSetting.delete();
                    _m_accountSetting = new AccountSetting(NPPlayer.instance.playerInfo.CID);
                    _m_accountSetting.init();
                }

                //新标签缓存
                try
                {
                    _m_newTagSaver = new NewTagSaver(NPPlayer.instance.playerInfo.CID);
                    _m_newTagSaver.init();
                }
                catch (Exception e)
                {
                    Debug.LogError($"{_m_newTagSaver.GetType()} init时出错：{e}");
                    _m_newTagSaver.delete();
                    _m_newTagSaver = new NewTagSaver(NPPlayer.instance.playerInfo.CID);
                    _m_newTagSaver.init();
                }

                //警告提示
                try
                {
                    _m_warningTipSaver = new WarningTipSaver(NPPlayer.instance.playerInfo.CID);
                    _m_warningTipSaver.init();
                }
                catch (Exception e)
                {
                    Debug.LogError($"{_m_warningTipSaver.GetType()} init时出错：{e}");
                    _m_warningTipSaver.delete();
                    _m_warningTipSaver = new WarningTipSaver(NPPlayer.instance.playerInfo.CID);
                    _m_warningTipSaver.init();
                }

                //其他玩家信息
                try
                {
                    _m_playerInfoSaver = new PlayerInfoSaver();
                    _m_playerInfoSaver.init();
                }
                catch (Exception e)
                {
                    Debug.LogError($"{_m_playerInfoSaver.GetType()} init时出错：{e}");
                    _m_playerInfoSaver.delete();
                    _m_playerInfoSaver = new PlayerInfoSaver();
                    _m_playerInfoSaver.init();
                }

                //每日标记信息
                try
                {
                    _m_dailyTagSaver = new DailyTagSetting(NPPlayer.instance.playerInfo.CID);
                    _m_dailyTagSaver.init();
                }
                catch (Exception e)
                {
                    Debug.LogError($"{_m_dailyTagSaver.GetType()} init时出错：{e}");
                    _m_dailyTagSaver.delete();
                    _m_dailyTagSaver = new DailyTagSetting(NPPlayer.instance.playerInfo.CID);
                    _m_dailyTagSaver.init();
                }

                try
                {
                    _m_commonEventSaver = new CommonEventSaver(NPPlayer.instance.playerInfo.CID);
                    _m_commonEventSaver.init();
                }
                catch (Exception e)
                {
                    Debug.LogError($"{_m_commonEventSaver.GetType()} init时出错：{e}");
                    _m_commonEventSaver.delete();
                    _m_commonEventSaver = new CommonEventSaver(NPPlayer.instance.playerInfo.CID);
                    _m_commonEventSaver.init();
                }

                try
                {
                    _m_dinnerSaver = new DinnerSetting(NPPlayer.instance.playerInfo.CID);
                    _m_dinnerSaver.init();
                }
                catch (Exception e)
                {
                    Debug.LogError($"{_m_dinnerSaver.GetType()} init时出错：{e}");
                    _m_dinnerSaver.delete();
                    _m_dinnerSaver = new DinnerSetting(NPPlayer.instance.playerInfo.CID);
                    _m_dinnerSaver.init();
                }
                
                try
                {
                    _m_middayDungeonSaver = new MiddayDungeonSetting(NPPlayer.instance.playerInfo.CID);
                    _m_middayDungeonSaver.init();
                }
                catch (Exception e)
                {
                    Debug.LogError($"{_m_middayDungeonSaver.GetType()} init时出错：{e}");
                    _m_middayDungeonSaver.delete();
                    _m_middayDungeonSaver = new MiddayDungeonSetting(NPPlayer.instance.playerInfo.CID);
                    _m_middayDungeonSaver.init();
                }

                try
                {
                    _m_gachaSetting = new GachaSetting(NPPlayer.instance.playerInfo.CID);
                    _m_gachaSetting.init();
                }
                catch (Exception e)
                {
                    Debug.LogError($"{_m_gachaSetting.GetType()} init时出错：{e}");
                    _m_gachaSetting.delete();
                    _m_gachaSetting = new GachaSetting(NPPlayer.instance.playerInfo.CID);
                    _m_gachaSetting.init();
                }

                try
                {
                    _m_consortMomentSaverMgr = new ConsortMomentSaverMgr(NPPlayer.instance.playerInfo.CID);
                    _m_consortMomentSaverMgr.init();
                }
                catch (Exception e)
                {
                    Debug.LogError($"{_m_consortMomentSaverMgr.GetType()} init时出错：{e}");
                    _m_consortMomentSaverMgr.delete();
                    _m_consortMomentSaverMgr = new ConsortMomentSaverMgr(NPPlayer.instance.playerInfo.CID);
                    _m_consortMomentSaverMgr.init();
                }

                try
                {
                    _m_consortAIChatSaverMgr = new ConsortAIChatSaverMgr(NPPlayer.instance.playerInfo.CID);
                    _m_consortAIChatSaverMgr.init();
                }
                catch (Exception e)
                {
                    Debug.LogError($"{_m_consortAIChatSaverMgr.GetType()} init时出错：{e}");
                    _m_consortAIChatSaverMgr.delete();
                    _m_consortAIChatSaverMgr = new ConsortAIChatSaverMgr(NPPlayer.instance.playerInfo.CID);
                    _m_consortAIChatSaverMgr.init();
                }
                
                try
                {
                    _m_consortPresetChatSaver = new ConsortPresetChatSaver(NPPlayer.instance.playerInfo.CID);
                    _m_consortPresetChatSaver.init();
                }
                catch (Exception e)
                {
                    Debug.LogError($"{_m_consortPresetChatSaver.GetType()} init时出错：{e}");
                    _m_consortPresetChatSaver.delete();
                    _m_consortPresetChatSaver = new ConsortPresetChatSaver(NPPlayer.instance.playerInfo.CID);
                    _m_consortPresetChatSaver.init();
                }
                
                try
                {
                    _m_childSaver = new ChildSaver(NPPlayer.instance.playerInfo.CID);
                    _m_childSaver.init();
                }
                catch (Exception e)
                {
                    Debug.LogError($"{_m_childSaver.GetType()} init时出错：{e}");
                    _m_childSaver.delete();
                    _m_childSaver = new ChildSaver(NPPlayer.instance.playerInfo.CID);
                    _m_childSaver.init();
                }
                
                try
                {
                    _m_guildSaver = new GuildSaver(NPPlayer.instance.playerInfo.CID);
                    _m_guildSaver.init();
                }
                catch (Exception e)
                {
                    Debug.LogError($"{_m_guildSaver.GetType()} init时出错：{e}");
                    _m_guildSaver.delete();
                    _m_guildSaver = new GuildSaver(NPPlayer.instance.playerInfo.CID);
                    _m_guildSaver.init();
                }

                try
                {
                    _m_unreadMarsBuildingChangeSaver = new UnreadMarsBuildingChangeSaver(NPPlayer.instance.playerInfo.CID);
                    _m_unreadMarsBuildingChangeSaver.init();
                }
                catch (Exception e)
                {
                    Debug.LogError($"{_m_unreadMarsBuildingChangeSaver.GetType()} init时出错：{e}");
                    _m_unreadMarsBuildingChangeSaver.delete();
                    _m_unreadMarsBuildingChangeSaver = new UnreadMarsBuildingChangeSaver(NPPlayer.instance.playerInfo.CID);
                    _m_unreadMarsBuildingChangeSaver.init();
                }

                try
                {
                    _m_bagItemWndRedTipSaver = new BagItemWndRedTipSaver(NPPlayer.instance.playerInfo.CID);
                    _m_bagItemWndRedTipSaver.init();
                }
                catch (Exception e)
                {
                    Debug.LogError($"{_m_bagItemWndRedTipSaver.GetType()} init时出错：{e}");
                    _m_bagItemWndRedTipSaver.delete();
                    _m_bagItemWndRedTipSaver = new BagItemWndRedTipSaver(NPPlayer.instance.playerInfo.CID);
                    _m_bagItemWndRedTipSaver.init();
                }

            }
            catch (Exception e)
            {
                Debug.LogError($"NPAccountSettingMgr init时出错时又出错：{e}");
            }
        }

        /// <summary>
        /// 运行时删除该账号的setting  删除账号时调用
        /// </summary>
        public void deleteAll()
        {
            _m_bIsInited = false;

            if(_m_accountSetting == null) 
                _m_accountSetting = new AccountSetting(NPPlayer.instance.playerInfo.CID);
            _m_accountSetting.delete();

            if (_m_dailyTagSaver == null)
                _m_dailyTagSaver = new DailyTagSetting(NPPlayer.instance.playerInfo.CID);
            _m_dailyTagSaver.delete();

            if (_m_commonEventSaver == null)
                _m_commonEventSaver = new CommonEventSaver(NPPlayer.instance.playerInfo.CID);
            _m_commonEventSaver.delete();
        }

        public void discard()
        {
            _m_bIsInited = false;
            _m_accountSetting = null;
            _m_dailyTagSaver = null;
            _m_commonEventSaver = null;
            _m_dinnerSaver = null;
            _m_gachaSetting = null;
            _m_middayDungeonSaver = null;
            _m_consortMomentSaverMgr = null;
            _m_consortAIChatSaverMgr = null;
            _m_consortPresetChatSaver = null;
            _m_childSaver = null;
            _m_guildSaver = null;
            _m_unreadMarsBuildingChangeSaver = null;
            _m_bagItemWndRedTipSaver = null;
        }
    }
}