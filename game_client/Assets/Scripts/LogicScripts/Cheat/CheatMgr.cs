using System.Collections.Generic;
using ALPackage;
using System;
using System.IO;
using System.Linq;
using System.Text;
using GS2GC.p004_PlayerOp;
using NPCommon;
using NPEnum;
using UnityEngine;

namespace GOE
{
    public enum EClientCheatType
    {
        NONE,
        TUTORIAL,//====引导从某个id开始，之前的都直接设置为完成  TUTORIAL:tutorialId
        PVEREPLAY,//====重放pve  PVEREPLAY:roomSerialize
        MISSION,//====测试进入关卡  MISSION:enterType
        MISSION_EDIT,//====编辑关卡  MISSION_EDIT:missionId
        SHOW_TIP,//====显示tip  SHOW_TIP:[TEXT,TEXT_NUM,ICON_TEXT]:[参数...]
        C_GAIN_ITEM,//====获得物品，客户端纯展示  C_GAIN_ITEM:itemType:subId:count
        CLIENT_PLAYER_DETAIL_PROP,//====玩家详细属性  CLIENT_PLAYER_DETAIL_PROP
        UNION_BONUS,//====测试属性读取  UNION_BONUS:string
        C_CHECK_ITEM_COUNT,//====检查物品数量，不够则弹出获取途径  C_CHECK_ITEM_COUNT:itemType:subId:count
        C_CHECK_FUNC_UNLOCK,//====检查功能是否已解锁  C_CHECK_FUNC_UNLOCK:funcType
        PRINT_RED_TIP_NODE_TREE,//====打印指定红点树  PRINT_RED_TIP_NODE_TREE:redTipId
        CREATE_DEBUG_FUNCTION_FILE,//====创建让客户端在白名单下打开debugLog的文件  CREATE_DEBUG_FUNCTION_FILE
        DELETE_DEBUG_FUNCTION_FILE,//====删除让客户端在白名单下打开debugLog的文件  DELETE_DEBUG_FUNCTION_FILE
        CREATE_DEBUG_CDN_FILE,//====创建让客户端在白名单下使用自定义的cdn地址的文件  CREATE_DEBUG_CDN_FILE
        DELETE_DEBUG_CDN_FILE,//====删除让客户端在白名单下使用自定义的cdn地址的文件  DELETE_DEBUG_CDN_FILE
        CREATE_DEBUG_PROTOCOL_FILE,//====创建让客户端在白名单下打印协议的文件  CREATE_DEBUG_PROTOCOL_FILE
        DELETE_DEBUG_PROTOCOL_FILE,//====删除让客户端在白名单下打印协议的文件  DELETE_DEBUG_PROTOCOL_FILE
        SET_NEED_TRANSLATE_KEY,//====设置是否需要翻译(当前仅在Editor下生效, 若需要真机也生效之后再改)  SET_NEED_TRANSLATE_KEY:bool
        PRINT_EARNINGS,//====输出收益速度  PRINT_EARNINGS
        TEST_TABLE_PATCH,//====测试热更配表
        C_SHOW_CONSORT_GET,//====展示情人获得弹窗  C_SHOW_CONSORT_GET:consortId
        TEST_CLEAR_DAILY_CHECK,//====删除本地签到存储记录
        TEST_CLEAR_FRIEND_VISIT,//====删除好友拜访记录
        RESET_FUNC_UNLOCK_SHOW_TIP_TYPE,//====重置功能解锁表现,RESET_FUNC_UNLOCK_SHOW_TIP:ENPFunctionType
        RESET_FUNC_UNLOCK_SHOW_TIP_ALL,//====重置所有功能解锁表现,RESET_FUNC_UNLOCK_SHOW_TIP_ALL
        TEST_SHOW_GAIN_HERO,//====测试获得骑士表现,TEST_SHOW_GAIN_HERO:heroId
        TEST_CLEAR_COLLEGE_CHOOSE_HERO,//====删除大学选择大臣本地的缓存
        PRINT_HERO_POWER,//====输出伙伴实力详情,PRINT_HERO_POWER:heroId
        PRINT_BUSINESS_BUILDING_EARNINGS_DETAIL,//====输出经营建筑赚速详情,PRINT_BUSINESS_BUILDING_EARNINGS_DETAIL:buildingId
        SHOW_MAIN_QUEST_ENTRY_TIP,//====展示主线任务入口提示,SHOW_MAIN_QUEST_ENTRY_TIP
        CONSORT_CHAT_ADD_MOMENT, // 妃子增加朋友圈
        CONSORT_CHAT_ADD_AI_FIRST_MSG, // 妃子AI主动聊天Msg
        SET_VOICE_LANGUAGE,//====设置语音语言  SET_VOICE_LANGUAGE:languageEnum
        SHOW_PERMISSIONS_LIST,//====显示玩家拥有的权限ID列表  SHOW_PERMISSIONS_LIST
        RESET_STORE_REVIEWS_RECORD,//====重置客户端商店好评记录  RESET_STORE_REVIEWS_RECORD
        DELETE_ALL_LOCAL_CACHING,//====删除所有本地缓存  DELETE_ALL_LOCAL_CACHING
    }

    public class CheatMgr : WCGSingleton<CheatMgr>
    {
        private const int MAX_LOG_NUM = 10;
        private const string CLIENT_CHEAT_MARK = "&";

        public Action<string> OnGmCmdRet;
        private List<string> _m_lConsoleLogList = new List<string>();

        public int consoleLogCount { get { return _m_lConsoleLogList.Count; } }
        public List<string> consoleLogList { get { return _m_lConsoleLogList; } }


        public string getConsoleLogString()
        {
            StringBuilder builder = new StringBuilder();
            for(int i = 0; i < _m_lConsoleLogList.Count; ++i)
            {
                if(i > 0)
                    builder.Append("\n");
                builder.Append(_m_lConsoleLogList[i]);
            }
            return builder.ToString();
        }

        public string getConsoleLogStringByIdx(int _index)
        {
            if(_index < 0 || _index >= _m_lConsoleLogList.Count)
                return "";
            return _m_lConsoleLogList[_index];
        }

        public void clearLog()
        {
            _m_lConsoleLogList.Clear();

            // WinMsg.SendMsg(WinMsgType.GEM_COMMAND_CONSOLE_LOG_CHANGE, _m_lConsoleLogList);
        }

        public void dealCheat(string _cheatString)
        {
            if(string.IsNullOrEmpty(_cheatString))
            {
                NPMesMgr.instance.showOneBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.cheat_enter_command_none), 
                    TextTranslate.instance.getLanguage(TransKeyConst.confirm), null);
                return;
            }

            //不是以特殊字符开头，直接发消息向服务器请求
            if(!_cheatString.StartsWith(CLIENT_CHEAT_MARK))
            {
                reqGmCommand(_cheatString);
                return;
            }

            _dealClientCheat(_cheatString);

        }

        //暂时只处理引导的作弊命令k
        //处理单纯的客户端作弊命令，没有和服务器交互
        //命令格式为 &EWCGClientCheatType|(command params)
        private void _dealClientCheat(string _command)
        {
            Debug.LogError("处理客户端的作弊命令:  " + _command);
            _command = _command.Substring(1);
            string[] subStrs = _command.Split(' ',':');

            if(subStrs.Length <= 0)
            {
                Debug.LogError("字符串格式错误, _command: " + _command);
                return;
            }
            EClientCheatType clientCheat = EClientCheatType.NONE;
            try
            {
                clientCheat = (EClientCheatType)ALCommon.EnumParse(typeof(EClientCheatType), subStrs[0], true);
            }
            catch(Exception ex)
            {
                clientCheat = EClientCheatType.NONE;
                Debug.LogError(ex.ToString());
            }

            if(clientCheat == EClientCheatType.NONE)
            {
                Debug.LogError("字符串格式错误, _command: " + _command);
                return;
            }

            List<string> paramList = new List<string>();
            for(int i = 1; i < subStrs.Length; ++i)
            {
                paramList.Add(subStrs[i]);
            }

            switch (clientCheat)
            {
                case EClientCheatType.TUTORIAL:
                    _dealTutorialStartWith(paramList);
                    break;
                case EClientCheatType.MISSION:
                    _dealEnterMission(clientCheat, paramList);
                    break;
                case EClientCheatType.PVEREPLAY:
                    _dealpvereplay(paramList);
                    break;
                case EClientCheatType.SHOW_TIP:
                    _dealShowTip(paramList);
                    break;
                case EClientCheatType.C_GAIN_ITEM:
                    _dealGainItem(paramList);
                    break;
                    break;
                case EClientCheatType.UNION_BONUS:
                    // UnionBonus unionBonus = _UnionBonusSerializeInfo.ReadFromString(paramList[0]);
                    //
                    // foreach (string s in unionBonus.getPropTransStrList())
                    // {
                    //     Debug.LogError(s);
                    // }
                    //
                    // Debug.LogError(unionBonus.getPropertyModifier(new JudgeUnionBonus(ENPPetElementType.WATER, ENPPetZodiac.ARIES)));
                    // Debug.LogError(unionBonus.getSkillMap(new JudgeUnionBonus(ENPPetElementType.WATER, ENPPetZodiac.LIBRA)).ToArray().ToStringList());
                    break;
                case EClientCheatType.CLIENT_PLAYER_DETAIL_PROP:
                    string detalValue = NPPlayer.instance.playerPropertyMgr.toDetailString();
                    Debug.LogError(detalValue);
                    if(null != OnGmCmdRet)
                        OnGmCmdRet(detalValue);
                    break;
                case EClientCheatType.C_CHECK_ITEM_COUNT:
                    _dealCheckItemCount(paramList);
                    break;
                case EClientCheatType.C_CHECK_FUNC_UNLOCK:
                    _dealFuncUnlock(paramList);
                    break;
                case EClientCheatType.PRINT_RED_TIP_NODE_TREE:
                    _dealPrintRedTipNodeTree(paramList, _command);
                    break;
                case EClientCheatType.CREATE_DEBUG_FUNCTION_FILE:
                    _dealClientCheat_CREATE_DEBUG_FUNCTION_FILE(_command);
                    break;
                case EClientCheatType.DELETE_DEBUG_FUNCTION_FILE:
                    _dealClientCheat_DELETE_DEBUG_FUNCTION_FILE(_command);
                    break;
                case EClientCheatType.CREATE_DEBUG_CDN_FILE:
                    _dealClientCheat_CREATE_DEBUG_CDN_FILE(paramList, _command);
                    break;
                case EClientCheatType.DELETE_DEBUG_CDN_FILE:
                    _dealClientCheat_DELETE_DEBUG_CDN_FILE(_command);
                    break;
                case EClientCheatType.CREATE_DEBUG_PROTOCOL_FILE:
                    _dealClientCheat_CREATE_DEBUG_PROTOCOL_FILE(_command);
                    break;
                case EClientCheatType.DELETE_DEBUG_PROTOCOL_FILE:
                    _dealClientCheat_DELETE_DEBUG_PROTOCOL_FILE(_command);
                    break;
                case EClientCheatType.SET_NEED_TRANSLATE_KEY:
                    _dealClientCheat_SET_NEED_TRANSLATE_KEY(paramList, _command);
                    break;
                case EClientCheatType.PRINT_EARNINGS:
                    string earnings = null;
                    earnings = _dealClientCheat_PRINT_EARNINGS(_command);
                    Debug.LogError(earnings);
                    if (null != OnGmCmdRet)
                        OnGmCmdRet(earnings);
                    break;
                case EClientCheatType.C_SHOW_CONSORT_GET:
                    string value;
                    if (paramList.Count <= 0 || string.IsNullOrEmpty(paramList[0]))
                    {
                        value = "客户端命令执行错误，缺少情人id";
                        if (null != OnGmCmdRet)
                            OnGmCmdRet(value);
                    }
                    else
                    {
                        if (long.TryParse(paramList[0], out long consortId))
                        {
                            _dealClientCheat_C_SHOW_CONSORT_GET(consortId);
                        }
                        else
                        {
                            value = "客户端命令执行错误，格式：C_SHOW_CONSORT_GET:consortId";
                            if (null != OnGmCmdRet)
                                OnGmCmdRet(value);
                        }
                    }
                    break;
                case EClientCheatType.TEST_CLEAR_FRIEND_VISIT:
                    _dealClientCheat_TEST_CLEAR_FRIEND_VISIT();
                    break;
                case EClientCheatType.RESET_FUNC_UNLOCK_SHOW_TIP_TYPE:
                    _dealClientCheat_RESET_FUNC_UNLOCK_SHOW_TIP_TYPE(paramList);
                    break;
                case EClientCheatType.RESET_FUNC_UNLOCK_SHOW_TIP_ALL:
                    _dealClientCheat_RESET_FUNC_UNLOCK_SHOW_TIP_ALL();
                    break;
                case EClientCheatType.TEST_SHOW_GAIN_HERO:
                    _dealClientCheat_TEST_SHOW_GAIN_HERO(paramList);
                    break;
                case EClientCheatType.TEST_CLEAR_COLLEGE_CHOOSE_HERO:
                    _dealClientCheat_TEST_CLEAR_COLLEGE_CHOOSE_HERO();
                    break;
                case EClientCheatType.PRINT_HERO_POWER:
                    string heroPowerDetail = _dealClientCheat_PRINT_HERO_POWER(paramList);
                    Debug.LogError(heroPowerDetail);
                    if (null != OnGmCmdRet)
                        OnGmCmdRet(heroPowerDetail);
                    break;
                case EClientCheatType.PRINT_BUSINESS_BUILDING_EARNINGS_DETAIL:
                    string businessBuildingEarningsDetail = _dealClientCheat_PRINT_BUSINESS_BUILDING_EARNINGS_DETAIL(paramList);
                    Debug.LogError(businessBuildingEarningsDetail);
                    if (null != OnGmCmdRet)
                        OnGmCmdRet(businessBuildingEarningsDetail);
                    break;
                case EClientCheatType.SHOW_MAIN_QUEST_ENTRY_TIP:
                    NPGUIAddSceneCenterTip.instance.showQuestEntryTip();
                    break;
                case EClientCheatType.CONSORT_CHAT_ADD_MOMENT:
                    _dealClientCheat_CONSORT_CHAT_ADD_MOMENT(paramList);
                    break;
                case EClientCheatType.CONSORT_CHAT_ADD_AI_FIRST_MSG:
                    _dealClientCheat_CONSORT_CHAT_ADD_AI_FIRST_MSG(paramList);
                    break;
                case EClientCheatType.SET_VOICE_LANGUAGE:
                    _dealClientCheat_SET_VOICE_LANGUAGE(paramList);
                    break;
                case EClientCheatType.SHOW_PERMISSIONS_LIST:
                    string permissionsList = _dealClientCheat_SHOW_PERMISSIONS_LIST(paramList);
                    Debug.LogError(permissionsList);
                    if (null != OnGmCmdRet)
                        OnGmCmdRet(permissionsList);
                    break;
                case EClientCheatType.RESET_STORE_REVIEWS_RECORD:
                    _dealClientCheat_RESET_STORE_REVIEWS_RECORD();
                    break;
                case EClientCheatType.DELETE_ALL_LOCAL_CACHING:
                    _dealClientCheat_DELETE_ALL_LOCAL_CACHING(paramList);
                    break;
            }
        }
        
        private void _dealClientCheat_TEST_CLEAR_FRIEND_VISIT()
        {
            reqGmCommand("player setparam LAST_GAIN_VISIT_REWARD_DAY 0");
        }

        private void _dealClientCheat_C_SHOW_CONSORT_GET(long _consortId)
        {
            GGottenConsortInfo consortInfo = NPPlayer.instance.consortComp.getConsortInfo(_consortId);
            if (null == consortInfo)
            {
                UnityEngine.Debug.LogError($"找不到已解锁妃子:{_consortId}数据");
                return;
            }
            NPUINoticeMgr.instance.addDealer(new NPNoticeDealer_GetConsort(consortInfo));
        }
        private void _dealClientCheat_TEST_CLEAR_COLLEGE_CHOOSE_HERO()
        {
            AccountSettingMgr.instance.accountSetting.setCollegeLastSelectHeroIdList(new List<long>());
        }

        //输出伙伴实力详情
        private string _dealClientCheat_PRINT_HERO_POWER(List<string> _paramList)
        {
            if (_paramList == null || _paramList.Count == 0)
                return "请输入伙伴id";

            long heroId = 0;
            long.TryParse(_paramList[0], out heroId);

            HeroInfo heroInfo = NPPlayer.instance.heroComponent.getHeroInfo(heroId);
            if (heroInfo == null)
                return $"伙伴id：{heroId}未获得";

            StringBuilder sb = new StringBuilder();
            HeroCommon.calPower(heroInfo, sb);
            return sb.ToString();
        }

        /// <summary>
        /// 输出经营建筑赚速详情
        /// </summary>
        /// <param name="_paramList"></param>
        /// <returns></returns>
        private string _dealClientCheat_PRINT_BUSINESS_BUILDING_EARNINGS_DETAIL(List<string> _paramList)
        {
            if (_paramList == null || _paramList.Count == 0)
                return "请输入经营建筑id";

            long buildingId = 0;
            long.TryParse(_paramList[0], out buildingId);

            BusinessBuildingInfo buildingInfo = NPPlayer.instance.buildingComp.getBusinessBuildingInfo(buildingId);
            if (buildingInfo == null)
                return $"经营建筑id：{buildingId}未获得";

            StringBuilder sb = new StringBuilder();
            BuildingCommon.calBusinessBuildingEarnings(buildingInfo, sb);
            return sb.ToString();
        }

        private void _dealTutorialStartWith(List<string> _paramList)
        {
            if(_paramList == null || _paramList.Count == 0)
            {
                Debug.LogError("tutorial start with command err!! ");
                return;
            }

            long tutorialId = 0;
            try
            {
                tutorialId = long.Parse(_paramList[0]);
            }
            catch(Exception ex)
            {
                tutorialId = 0;
                Debug.LogError(ex.ToString());
            }

            if(tutorialId <= 0)
            {
                Debug.LogError("tutorial start with command err!!, tutorialId: " + tutorialId);
                return;
            }

            NPPlayerTutorialComponent.instance.forceForceTutorialStartWith(tutorialId);
        }


        //    private long lastPassMissionTime;
        private void _dealEnterMission(EClientCheatType _clientCheat, List<string> _paramList)
        {
            if(_paramList == null || _paramList.Count == 0)
                return;

            if(_paramList.Count < 1)
            {
                Debug.LogError("enter dungeon command err!!, _paramsCount: " + _paramList.Count);
                return;
            }
            string enterType;
            long levelId = 0;
            try
            {
                enterType = _paramList[0];
                levelId = long.Parse(_paramList[1]);
            }
            catch(Exception ex)
            {
                enterType = "d";
                levelId = 0;
                Debug.LogError(ex.ToString());
            }
        }

        /// <summary>
        /// 显示tip
        /// </summary>
        /// <param name="_paramList"></param>
        private void _dealShowTip(List<string> _paramList)
        {
            if (_paramList == null || _paramList.Count <= 2)
            {
                ALLog.Error($"客户端作弊命令【SHOW_TIP】需要2个以上参数，【{CLIENT_CHEAT_MARK}{EClientCheatType.SHOW_TIP} tip类型（ETipNum） 是否是覆盖类型tip（bool） [参数...]】");
                return;
            }

            //是否是覆盖类型tip
            bool isSysTip = Convert.ToBoolean(_paramList[1]);
            if (_paramList[0].Equals("TEXT", StringComparison.OrdinalIgnoreCase))
            {
                if (_paramList.Count < 2 + 1)
                {
                    ALLog.Error($"客户端作弊命令【SHOW_TIP】：TEXT类型需要至少1个参数，【{CLIENT_CHEAT_MARK}{EClientCheatType.SHOW_TIP} TEXT {isSysTip} 文本 [ID]】");
                }
                else
                {
                    string text = _paramList[2];
                    long tipId = NPConst.C_DEFAULT_TEXT_TIP_REF_ID;
                    if (_paramList.Count >= 4)
                    {
                        long.TryParse(_paramList[3], out tipId);
                    }
                    if (isSysTip)
                        NPGUIAddSceneCenterTip.instance.showTextSysTip(text, tipId);
                    else
                        NPGUIAddSceneCenterTip.instance.showTextTip(text, tipId);
                }
            }
            else if (_paramList[0].Equals("TEXT_NUM", StringComparison.OrdinalIgnoreCase))
            {
                if (_paramList.Count < 2 + 2)
                {
                    ALLog.Error($"客户端作弊命令【SHOW_TIP】：TEXT_NUM类型需要至少2个参数，【{CLIENT_CHEAT_MARK}{EClientCheatType.SHOW_TIP} TEXT_NUM {isSysTip} 文本 数量 [ID]】");
                }
                else
                {
                    string text = _paramList[2];
                    float num = 0f;
                    float.TryParse(_paramList[3], out num);
                    long tipId = NPConst.C_DEFAULT_TEXT_NUM_TIP_REF_ID;
                    if (_paramList.Count >= 5)
                    {
                        long.TryParse(_paramList[4], out tipId);
                    }
                    if (isSysTip)
                        NPGUIAddSceneCenterTip.instance.showTextNumSysTip(text, num, tipId);
                    else
                        NPGUIAddSceneCenterTip.instance.showTextNumTip(text, num, tipId);
                }
            }
            else if (_paramList[0].Equals("ICON_TEXT", StringComparison.OrdinalIgnoreCase))
            {
                if (_paramList.Count < 2 + 3)
                {
                    ALLog.Error($"客户端作弊命令【SHOW_TIP】：ICON_TEXT类型需要至少3个参数，【{CLIENT_CHEAT_MARK}{EClientCheatType.SHOW_TIP}SHOW_TIP ICON_TEXT {isSysTip} texture资源mainId texture资源subId 文本 [ID]】");
                }
                else
                {
                    int mainId = 1;
                    int subId = 1;
                    int.TryParse(_paramList[2], out mainId);
                    int.TryParse(_paramList[3], out subId);
                    NPGTextureIndex icon = new NPGTextureIndex();
                    icon.mainId = mainId;
                    icon.subId = subId;

                    string text = _paramList[4];
                    long tipId = NPConst.C_DEFAULT_ICON_TEXT_TIP_REF_ID;
                    if (_paramList.Count >= 6)
                    {
                        long.TryParse(_paramList[5], out tipId);
                    }
                    if (isSysTip)
                        NPGUIAddSceneCenterTip.instance.showIconTextSysTip(icon, text, tipId);
                    else
                        NPGUIAddSceneCenterTip.instance.showIconTextTip(icon, text, tipId);
                }
            }
        }

        /// <summary>
        /// 获得奖励，客户端纯展示
        /// </summary>
        /// <param name="_paramList"></param>
        private void _dealGainItem(List<string> _paramList)
        {
            if (_paramList == null || _paramList.Count <= 2)
            {
                ALLog.Error($"客户端作弊命令【C_GAIN_ITEM】需要3个参数，【{CLIENT_CHEAT_MARK}{EClientCheatType.C_GAIN_ITEM} 物品类型枚举(ENPItemType) 物品id 展示数量");
                return;
            }
            NPEnum.ENPItemType itemType = (NPEnum.ENPItemType)ALCommon.EnumParse(typeof(NPEnum.ENPItemType), _paramList[0], true);
            long subId = 1;
            long.TryParse(_paramList[1], out subId);
            int count = 1;
            int.TryParse(_paramList[2], out count);

            List<NPCommon.NPCommon_ItemInfo> itemList = new List<NPCommon.NPCommon_ItemInfo>();
            NPCommon.NPCommon_ItemInfo tmpInfo = null;
            for (int i = 0; i < count; i++)
            {
                tmpInfo = new NPCommon.NPCommon_ItemInfo((int)itemType, subId, 1, null);

                //添加到队列
                itemList.Add(tmpInfo);
            }
            NPUINoticeMgr.instance.addDealer(NPNoticeDealer_GetReward.makeRewardNotice(itemList));
        }


        private void _dealCheckItemCount(List<string> _paramList)
        {
            if (_paramList == null || _paramList.Count <= 2)
            {
                ALLog.Error($"客户端作弊命令【C_CHECK_ITEM_COUNT】需要3个参数，【{CLIENT_CHEAT_MARK}{EClientCheatType.C_CHECK_ITEM_COUNT} 物品类型枚举(ENPItemType) 物品id 数量");
                return;
            }
            NPEnum.ENPItemType itemType = (NPEnum.ENPItemType)ALCommon.EnumParse(typeof(NPEnum.ENPItemType), _paramList[0], true);
            long subId = 1;
            long.TryParse(_paramList[1], out subId);
            int count = 1;
            int.TryParse(_paramList[2], out count);

            Debug.Log($"C_CHECK_ITEM_COUNT:{itemType} {subId} {count},result: {GCommon.isItemEnough(itemType, subId, count, true)}");
        }

        private void _dealFuncUnlock(List<string> _paramList)
        {
            if (_paramList == null || _paramList.Count <= 0)
            {
                ALLog.Error($"客户端作弊命令【C_CHECK_FUNC_UNLOCK】需要1个参数，【{CLIENT_CHEAT_MARK}{EClientCheatType.C_CHECK_FUNC_UNLOCK} 功能类型枚举（ENPFunctionType）");
                return;
            }

            NPEnum.ENPFunctionType funcType = (NPEnum.ENPFunctionType)ALCommon.EnumParse(typeof(NPEnum.ENPFunctionType), _paramList[0], true);

            FuncUnlockInfo funcUnlockInfo = NPPlayer.instance.funcUnlockComp.getFuncUnlockInfo(funcType);
            if(funcUnlockInfo == null)
            {
                NPGUIAddSceneCenterTip.instance.showTextInfo($"未找到功能解锁信息，ENPFunctionType:{funcType}");
            }
            else
            {
                if (funcUnlockInfo.isUnlock)
                {
                    NPGUIAddSceneCenterTip.instance.showTextInfo($"{funcUnlockInfo.funcName}已解锁");
                }
                else
                {
                    NPGUIAddSceneCenterTip.instance.showTextInfo($"{funcUnlockInfo.funcName}未解锁");
                }
            }
        }

        public void onRetGmCommand(string _consoleLog)
        {
            if(string.IsNullOrEmpty(_consoleLog))
            {
                Debug.LogError("wtf, 服务器返回的是什么鬼? log:  " + _consoleLog);
                return;
            }

            _m_lConsoleLogList.Insert(0, _consoleLog);

            int count = _m_lConsoleLogList.Count;
            if(count > MAX_LOG_NUM)
            {
                _m_lConsoleLogList.RemoveAt(count - 1);
            }

            if(null != OnGmCmdRet)
                OnGmCmdRet(_consoleLog);
            //WinMsg.SendMsg(WinMsgType.GEM_COMMAND_CONSOLE_LOG_CHANGE, _m_lConsoleLogList);
        }

        public void reqGmCommand(string _gmCommand, Action<string> _onSuc = null, Action _onFail = null)
        {
            NPGSClientListener.sendRequestByLog(NPGSWriter_004_PlayerOp.make_002_ReqGmCommand(_gmCommand),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_004_002_RetGmCommand>((_isSuc, _msg) =>
                {
                    if (_msg == null || !_isSuc || !_msg.getIsSucc())
                        _onFail?.Invoke();
                    else
                    {
                        //服务端数据太长会压缩给到
                        string value = _msg.getResult();
                        if (string.IsNullOrEmpty(value))
                        {
                            byte[] zipedResult = _msg.getZipedResult();
                            if (null != zipedResult)
                                value = System.Text.Encoding.UTF8.GetString(SevenZipTransition.instance.Decoder(zipedResult));
                        }

                        onRetGmCommand(value);
                        _onSuc?.Invoke(value);
                    }
                }));
        }

        /// <summary>
        /// pve相关处理
        /// </summary>
        /// <param name="_paramList"></param>
        private void _dealpvereplay(List<string> _paramList)
        {
            if (_paramList == null || _paramList.Count == 0)
            {
                ALLog.Error($"_dealpve need room serialize id! [pvereplay xxx]");
                return;
            }

            //获取第一个参数作为id
            long roomSerialize = long.Parse(_paramList[0]);
        }

        /// <summary>
        /// 获取指定saveKey的红点树的信息
        /// </summary>
        private void _dealPrintRedTipNodeTree(List<string> _paramList, string _cmd)
        {
            if (_paramList == null || _paramList.Count < 1)
            {
                onRetGmCommand(_cmd + " failed, 需要一个参数，为红点id");
                return;
            }

            string saveKey = _paramList[0];
            _ARedTipNode node = RedTipMgr.instance.getNodeBySaveKeyRecursive(saveKey);
            if (node == null)
            {
                onRetGmCommand(_cmd + " failed, 没有找到对应的红点，saveKey是" + saveKey);
                return;
            }

            string nodeTreeStr = node.getNodeTreeString();
            Debug.Log_EditorOnly(nodeTreeStr);
            onRetGmCommand(_cmd + " ok, 查询结果为：\n" + nodeTreeStr);
        }

        // 创建调试文件，白名单用户检测到该文件会开启 showDebugOutput
        private void _dealClientCheat_CREATE_DEBUG_FUNCTION_FILE(string _cmd)
        {
            try
            {
                DebugFunctionFacade.instance.openDebugFunction.openFunction(null);
            }
            catch (Exception ex)
            {
                onRetGmCommand(_cmd + " failed, error: " + ex);
                return;
            }
            onRetGmCommand(_cmd + " ok");
        }

        // 删除调试文件，白名单用户检测到该文件会开启 showDebugOutput
        private void _dealClientCheat_DELETE_DEBUG_FUNCTION_FILE(string _cmd)
        {
            try
            {
                DebugFunctionFacade.instance.openDebugFunction.closeFunction();
            }
            catch (Exception ex)
            {
                onRetGmCommand(_cmd + " failed, error: " + ex);
                return;
            }
            onRetGmCommand(_cmd + " ok");
        }

        // 创建调试文件，白名单用户检测到该文件会使用文件里设置的cdn地址进行登录
        private void _dealClientCheat_CREATE_DEBUG_CDN_FILE(List<string> _paramList, string _cmd)
        {
            if (_paramList == null || _paramList.Count < 1)
            {
                onRetGmCommand(_cmd + " failed，需要一个参数，内容是openDebugCDN的json内容");
                return;
            }

            string jsonStr = _paramList[0];
            try
            {
                DebugFunctionFacade.instance.openDebugCDN.openFunction(jsonStr);
            }
            catch (Exception ex)
            {
                onRetGmCommand(_cmd + " failed, error: " + ex);
                return;
            }

            onRetGmCommand(_cmd + " ok, 创建的文件内容为：" + jsonStr);
        }

        // 删除调试文件，白名单用户检测到该文件会使用文件里设置的cdn地址进行登录
        private void _dealClientCheat_DELETE_DEBUG_CDN_FILE(string _cmd)
        {
            try
            {
                DebugFunctionFacade.instance.openDebugCDN.closeFunction();
            }
            catch (Exception ex)
            {
                onRetGmCommand(_cmd + " failed, error: " + ex);
                return;
            }
            onRetGmCommand(_cmd + " ok");
        }

        // 创建调试文件，白名单用户检测到该文件会开启 协议打印
        private void _dealClientCheat_CREATE_DEBUG_PROTOCOL_FILE(string _cmd)
        {
            try
            {
                DebugFunctionFacade.instance.openDebugProtocol.openFunction(null);
            }
            catch (Exception ex)
            {
                onRetGmCommand(_cmd + " failed, error: " + ex);
                return;
            }
            onRetGmCommand(_cmd + " ok");
        }

        // 删除调试文件，白名单用户检测到该文件会开启 协议打印
        private void _dealClientCheat_DELETE_DEBUG_PROTOCOL_FILE(string _cmd)
        {
            try
            {
                DebugFunctionFacade.instance.openDebugProtocol.closeFunction();
            }
            catch (Exception ex)
            {
                onRetGmCommand(_cmd + " failed, error: " + ex);
                return;
            }
            onRetGmCommand(_cmd + " ok");
        }

        private void _dealClientCheat_SET_NEED_TRANSLATE_KEY(List<string> _paramList, string _cmd)
        {
            if (_paramList == null || _paramList.Count < 1 ||
                !(bool.TryParse(_paramList[0], out bool _needTranslateKey)))
            {
                onRetGmCommand(_cmd + " failed, 请传入一个bool值");
                return;
            }

            CharacterDetermineMgr.instance.setNeedTranslateKeyKey(_needTranslateKey);
            onRetGmCommand(_cmd + " ok");
        }

        private string _dealClientCheat_PRINT_EARNINGS(string _cmd)
        {
            string str = $"总国力：{NPPlayer.instance.specialItemComp.goldData.earnings}\n";
            return str;
        }

        private void _dealClientCheat_RESET_FUNC_UNLOCK_SHOW_TIP_TYPE(List<string> _paramList)
        {
            if (_paramList == null || _paramList.Count == 0)
                return;

            ENPFunctionType type = (ENPFunctionType)ALCommon.EnumParse(typeof(ENPFunctionType), _paramList[0]);
            NPPlayer.instance.funcUnlockComp.resetShowTip(type);
        }

        private void _dealClientCheat_RESET_FUNC_UNLOCK_SHOW_TIP_ALL()
        {
            NPPlayer.instance.funcUnlockComp.resetAllShowTip();
        }

        private void _dealClientCheat_TEST_SHOW_GAIN_HERO(List<string> _paramList)
        {
            if (_paramList == null || _paramList.Count == 0)
                return;

            long heroId = 0;
            long.TryParse(_paramList[0], out heroId);
            if (heroId != 0)
            {
                HeroRefObj heroRef = GRefdataCoreMgr.instance.heroRefCore.getRef(heroId);
                if (heroRef != null)
                {
                    if (heroRef.gain_hero_dialog_id > 0)
                        NPUINoticeMgr.instance.addDealer(new NoticeDealer_EnterDialogue(heroRef.gain_hero_dialog_id));
                    NPUINoticeMgr.instance.addDealer(new NoticeDealer_GetHero(heroRef));
                }
                else
                    Debug.LogError($"伙伴id不存在，id:{heroId}");
            }
        }

        private void _dealClientCheat_CONSORT_CHAT_ADD_MOMENT(List<string> _paramList)
        {
            if (_paramList.Count > 0 && long.TryParse(_paramList[0], out long consortId))
            {
                NPPlayer.instance.consortChatComp.testMomentsAdd(new List<long>(){consortId});
            }
            else
            {
                NPPlayer.instance.consortChatComp.testMomentsAdd(NPPlayer.instance.consortChatComp.getChatConsortList());
            }
        }
        private void _dealClientCheat_CONSORT_CHAT_ADD_AI_FIRST_MSG(List<string> _paramList)
        {
            if (_paramList.Count > 0 && long.TryParse(_paramList[0], out long consortId))
            {
                NPPlayer.instance.consortChatComp.testAIFirstMsgAdd(new List<long>(){consortId});
            }
            else
            {
                NPPlayer.instance.consortChatComp.testAIFirstMsgAdd(NPPlayer.instance.consortChatComp.getChatConsortList());
            }
        }

        /// <summary>
        /// 设置语音语言
        /// </summary>
        /// <param name="_paramList">参数：_paramList[0] 为 ENPLanguage 枚举值</param>
        private void _dealClientCheat_SET_VOICE_LANGUAGE(List<string> _paramList)
        {
            if (_paramList == null || _paramList.Count < 1)
            {
                string errorMsg = $"客户端作弊命令【{EClientCheatType.SET_VOICE_LANGUAGE}】需要1个参数，【{CLIENT_CHEAT_MARK}{EClientCheatType.SET_VOICE_LANGUAGE} 语言枚举(ENPLanguage)】";
                Debug.LogError(errorMsg);
                if (null != OnGmCmdRet)
                    OnGmCmdRet(errorMsg);
                return;
            }

            try
            {
                ENPLanguage language = (ENPLanguage)ALCommon.EnumParse(typeof(ENPLanguage), _paramList[0], true);
                
                if (language == ENPLanguage.NONE)
                {
                    string errorMsg = $"语言枚举不能为NONE，请输入有效的ENPLanguage枚举值";
                    Debug.LogError(errorMsg);
                    if (null != OnGmCmdRet)
                        OnGmCmdRet(errorMsg);
                    return;
                }

                //设置语音语言
                GameVoiceLanguageMgr.instance.setVoiceLanguage(language, true);
                
                string successMsg = $"成功设置语音语言为：{language}";
                Debug.Log(successMsg);
                if (null != OnGmCmdRet)
                    OnGmCmdRet(successMsg);
            }
            catch (Exception ex)
            {
                string errorMsg = $"设置语音语言失败，错误信息：{ex.Message}";
                Debug.LogError(errorMsg);
                if (null != OnGmCmdRet)
                    OnGmCmdRet(errorMsg);
            }
        }

        /// <summary>
        /// 显示玩家拥有的权限ID列表
        /// </summary>
        /// <param name="_paramList"></param>
        private string _dealClientCheat_SHOW_PERMISSIONS_LIST(List<string> _paramList)
        {
            string str = "玩家权限ID列表：";
            List<long> permissionsList = NPPlayer.instance.playerPermissionsComp.getAllPermissions();
            for (int i = 0; i < permissionsList.Count; i++)
            {
                if (i > 0)
                    str += ", ";
                str += $"{permissionsList[i]}";
            }
            return str;
        }

        /// <summary>
        /// 重置客户端商店好评记录
        /// </summary>
        private void _dealClientCheat_RESET_STORE_REVIEWS_RECORD()
        {
            GameSetting.instance.setStoreReviewsCdStartTimeMs(0);
            GameSetting.instance.setIsFinishStoreReviews(false);
            NPPlayer.instance.playerInfoComp.clientDataRemarkInfo.setIsFinishStoreReviews(false);
            if (null != OnGmCmdRet)
                OnGmCmdRet("重置客户端商店好评记录完成");
        }

        /// <summary>
        /// 删除所有本地缓存
        /// </summary>
        private void _dealClientCheat_DELETE_ALL_LOCAL_CACHING(List<string> _paramList)
        {
            string tmpPath = string.Empty;
#if UNITY_ANDROID && !UNITY_EDITOR
            tmpPath = Application.persistentDataPath;
            if (null == tmpPath)
            {
                tmpPath = "/data/data" + Application.dataPath.Substring(9);
                if (tmpPath.LastIndexOf('-') == -1)
                    tmpPath = tmpPath.Substring(0, tmpPath.LastIndexOf('/')) + "/files";
                else
                    tmpPath = tmpPath.Substring(0, tmpPath.LastIndexOf('-')) + "/files";
            }
#else
            tmpPath = Application.persistentDataPath;
#endif
            try
            {
                if (!Directory.Exists(tmpPath))
                {
                    string errorMsg = $"本地缓存路径不存在，路径：{tmpPath}";
                    Debug.LogError(errorMsg);
                    if (null != OnGmCmdRet)
                        OnGmCmdRet(errorMsg);
                    return;
                }
                
                bool deleteWithOutRes = false;
                if (_paramList != null && _paramList.Count > 0)
                {
                    bool.TryParse(_paramList[0], out deleteWithOutRes);
                }

                if (!deleteWithOutRes)
                {
                    //直接删除文件夹
                    Directory.Delete(tmpPath, true);    
                }
                else
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(tmpPath);
                    foreach (var fileInfo in directoryInfo.GetFiles())
                    {
                        if(fileInfo != null)
                            fileInfo.Delete();
                    }
                }

                Debug.Log("ok");
                if (null != OnGmCmdRet)
                    OnGmCmdRet("ok");

                // 删除完成后关闭游戏
                Application.Quit();
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            }
            catch (Exception ex)
            {
                string errorMsg = $"删除所有本地缓存失败，错误信息：{ex.Message}";
                Debug.LogError(errorMsg);
                if (null != OnGmCmdRet)
                    OnGmCmdRet(errorMsg);
            }
        }
    }
}
