using NPEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ALPackage;
using Common.MarsEnum;
using CommonEnum;
using GOE.MiniGame;
using UnityEngine;

namespace GOE
{
    public static partial class GCommon
    {
        /// <summary>
        /// 进入枚举对应的UI界面，会根据配置判断是否解锁并弹出提示
        /// </summary>
        /// <param name="_sysSceneType"></param>
        public static void enterUIMainNodeShow(ESysSceneType _sysSceneType)
        {
            enterUIMainNodeShow(_sysSceneType, null);
        }

        private static int _g_iEnterCount = 0;//循环判断的计数
        private const int _m_iMaxEnterCount = 3;

        /// <summary>
        /// 进入枚举对应的UI界面，会根据配置判断是否解锁并弹出提示
        /// </summary>
        /// <param name="_sysSceneType"></param>
        /// <param name="_args"></param>
        public static void enterUIMainNodeShow(ESysSceneType _sysSceneType, List<string> _args)
        {
            //防止死循环
            if (_g_iEnterCount > _m_iMaxEnterCount)
            {
                //跳转失败
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.enter_ui_main_node_fail));
                _g_iEnterCount = 0;
                return;
            }
            
            //判断系统功能解锁
            if (!uiMainNodeIsUnlock(_sysSceneType, () =>
            {
                NPMainNodeToFunctionTypeRefObj refObj = GRefdataCoreMgr.instance.mainNodeToFunctionTypeRefCore.getRef((long)_sysSceneType);
                if (refObj != null && refObj.condition_fail_main_node != ESysSceneType.NONE)
                {
                    //condition_fail_main_node不为空，进行下一轮判断
                    _g_iEnterCount++;
                    enterUIMainNodeShow(refObj.condition_fail_main_node, null);
                }
            }))
                return;

            //进入成功重置计数
            _g_iEnterCount = 0;
            switch (_sysSceneType)
            {
                case ESysSceneType.CITY_MAIN:
                    if (_args is { Count: > 1 })
                    {
                        bool isFindPos = false;
                        Vector3 pos = Vector3.zero;
                        ALCommon.TryParseFloat(_args[1], out float duration);

                        if (long.TryParse(_args[0], out long id))
                        {
                            isFindPos = MainAdditionBuildingTDScene.instance.tryGetBuildingPos(id, out pos);
                            if (!isFindPos)
                                isFindPos = MainAdditionBuildingTDScene.instance.tryGetAnecdotePos(id, out pos);
                        }
                        else if (Enum.TryParse(_args[0], out ECityMainJumpType jumpType))
                        {
                            EBusinessBuildingType buildingType = EBusinessBuildingType.NONE;
                            EAnecdoteType anecdoteType = EAnecdoteType.NONE;
                            switch (jumpType)
                            {
                                case ECityMainJumpType.MINIMUM_EMPLOYEE:
                                    buildingType = EBusinessBuildingType.MINIMUM_EMPLOYEE;
                                    break;
                                case ECityMainJumpType.UPGRADABLE:
                                    buildingType = EBusinessBuildingType.UPGRADABLE;
                                    break;
                                case ECityMainJumpType.HERO_SETTABLE:
                                    buildingType = EBusinessBuildingType.HERO_SETTABLE;
                                    break;
                                case ECityMainJumpType.HERO_ANECDOTE:
                                    anecdoteType = EAnecdoteType.HERO_GAIN;
                                    break;
                                case ECityMainJumpType.EMPLOYEE_LOWEST_COST:
                                    buildingType = EBusinessBuildingType.EMPLOYEE_LOWEST_COST;
                                    break;
                            }
                            
                            if (buildingType != EBusinessBuildingType.NONE)
                            {
                                BusinessBuildingInfo resultBuilding = NPPlayer.instance.buildingComp.getBusinessBuildingInfo(buildingType);
                                if (resultBuilding != null)
                                {
                                    isFindPos = MainAdditionBuildingTDScene.instance.tryGetBuildingPos(resultBuilding.id, out pos);
                                    id = resultBuilding.id;
                                }       
                            }
                            else if (anecdoteType != EAnecdoteType.NONE)
                            {
                                AnecdotePosInfo resultAnecdote = NPPlayer.instance.anecdoteComp.getPosInfo(anecdoteType);
                                if (resultAnecdote != null)
                                {
                                    isFindPos = MainAdditionBuildingTDScene.instance.tryGetAnecdotePos(resultAnecdote.posId, out pos);
                                    id = resultAnecdote.posId;
                                }
                                else // 如果没找到对应的事件位置，和别的不同，就不跳转
                                    return;
                            }
                        }

                        if (isFindPos)
                        {
                            void FocusToTarget()
                            {
                                MainAdditionBuildingTDScene.instance.focusToTarget(pos, duration);
                                if (_args is { Count: > 2 } && bool.TryParse(_args[2], out bool showGuide) && showGuide && !Game.instance.isInTutorial)
                                {
                                    ALCommonActionMonoTask.addMonoTask(() =>
                                    {
                                        //如果不在引导并且条件通过，展示入口手指引导
                                        if (!Game.instance.isInTutorial && GRefdataCoreMgr.instance.npGeneral.entrance_show_hand_guide_cond.IsEnable(null))
                                            WinMsg.SendMsg(WinMsgType.SET_SHOW_ENTRY_GUIDE_HAND, id);
                                    }, duration);
                                }
                            }

                            if (QueueMgr.instance._lastNode is GNodeBuilding)
                                FocusToTarget();
                            else
                                QueueMgr.instance.AddNode(new GNodeBuilding(FocusToTarget));
                        }
                        else
                            QueueMgr.instance.AddNode(new GNodeBuilding());
                    }
                    else
                        QueueMgr.instance.AddNode(new GNodeBuilding());
                    break;
                case ESysSceneType.BUILDING_BUILD:
                    if (_args is not { Count: > 0 } || !long.TryParse(_args[0], out long buildingId))
                    {
                        UnityEngine.Debug.LogError("打开建筑建造界面失败，找不到建筑信息(示例：C_UI_TO_SYS_SCE:BUILDING_BUILD:buildingId)");
                        return;
                    }
                    
                    BuildingInfo buildingInfo = NPPlayer.instance.buildingComp.getBuildingInfo(buildingId);
                    if (buildingInfo == null || buildingInfo.isBuilt)
                    {
                        UnityEngine.Debug.LogError("打开建筑建造界面失败，找不到建筑信息或建筑已建造: " + buildingId);
                        return;
                    }
                    
                    GGUIWndBuildingBuild.instance.refreshWnd(buildingInfo);
                    GGUIWndBuildingBuild.instance.setFocusPos(MainAdditionBuildingTDScene.instance.getBuildingFocusPos(buildingId));
                    QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndBuildingBuild.instance, GGUIWndBuildingBuild.instance.showWnd, EUIQueueStageType.MAIN, string.Empty, false, false);
                    break;
                case ESysSceneType.BUSINESS_BUILDING:
                    if (_args is not { Count: > 0 })
                        return;

                    BusinessBuildingInfo businessBuildingInfo = null;
                    if (long.TryParse(_args[0], out long businessBuildingId))
                        businessBuildingInfo = NPPlayer.instance.buildingComp.getBusinessBuildingInfo(businessBuildingId);
                    else if (Enum.TryParse(_args[0], out EBusinessBuildingType buildingType))
                        businessBuildingInfo = NPPlayer.instance.buildingComp.getBusinessBuildingInfo(buildingType);
                    
                    if (businessBuildingInfo == null)
                    {
                        UnityEngine.Debug.LogError("打开经营建筑界面失败，找不到建筑信息: " + _args[0]);  
                        return;
                    }
            
                    PlayAudioMgr.instance.playClip(businessBuildingInfo.baseRef.open_building_audio_id);
                    GGUIWndBusinessBuilding.instance.refreshWnd(businessBuildingInfo);
                    QueueMgr.instance.addNode_InGame_MainUIMainWnd(GGUIWndBusinessBuilding.instance, UINodeTagConst.C_BUILDING_MAIN, null, null, 0);
                    break;
                case ESysSceneType.FARMING_BUILDING_UPGRADE:
                    if (_args is not { Count: > 0 })
                        return;

                    FarmingBuildingInfo farmingBuildingInfo = null;
                    if (long.TryParse(_args[0], out long farmingBuildingId))
                        farmingBuildingInfo = NPPlayer.instance.buildingComp.getFarmingBuildingInfo(farmingBuildingId);

                    if (farmingBuildingInfo == null)
                    {
                        UnityEngine.Debug.LogError("打开农业建筑升级界面失败，找不到建筑信息: " + _args[0]);
                        return;
                    }
                    
                    GGUIWndFarmingBuildingUpgrade.instance.refreshWnd(farmingBuildingInfo);
                    QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndFarmingBuildingUpgrade.instance, GGUIWndFarmingBuildingUpgrade.instance.showWnd);
                    break;
                case ESysSceneType.NONE:
                    QueueMgr.instance.AddNode(new GNodeBuilding());
                    break;
                case ESysSceneType.ROOM:
                    QueueMgr.instance.AddNode(new GNodeRoom());
                    break;
                case ESysSceneType.ROOM_SKIN:
                    GGUIWndPlayerRoomSkin.instance.refreshData();
                    QueueMgr.instance.addNode_InGame_MainUIMainWnd(GGUIWndPlayerRoomSkin.instance, UINodeTagConst_PlayerInfo.C_PLAYER_ROOM_SKIN, null, null, 0);
                    break;
                case ESysSceneType.BAG:
                    bool bagSelectDefaultTab = true;
                    EBagMainTabMonoType selectBagTab = EBagMainTabMonoType.BAG_ITEM;
                    // 有参数时，解析参数
                    if (_args.Count >= 1 && !string.IsNullOrEmpty(_args[0]))
                    {
                        if (ALCommon.TryEnumParse(typeof(EBagMainTabMonoType), _args[0], out selectBagTab))
                        {
                            bagSelectDefaultTab = false;
                        }
                    }
                    QueueMgr.instance.AddNode(new GNodeBag(bagSelectDefaultTab, selectBagTab));
                    break;
                case ESysSceneType.CHAT:
                    //聊天功能未解锁
                    // if (!GRefdataCoreMgr.instance.npGeneral.chat_unlock_condition.IsEnable(null))
                    // {
                    //     NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage("#1_chat_sysLocked_none"));
                    //     return;
                    // }
                    ENPChatRoomType chatRoomType = ENPChatRoomType.US_SERVER;
                    if (_args != null && _args.Count > 0)
                    {
                        ALCommon.TryEnumParse(typeof(ENPChatRoomType), _args[0], out chatRoomType);
                    }

                    NPRoomChatInfo chatInfo = NPPlayer.instance.chatComp.getRoomChatInfo(chatRoomType);

                    //未获取到对应类型的聊天室，取默认的本服频道
                    if (chatInfo == null)
                        chatInfo = NPPlayer.instance.chatComp.getRoomChatInfo(ENPChatRoomType.US_SERVER);

                    if (chatInfo == null)
                    {
                        ALLog.Error($"[NPGGUISubWndMiniChat._onBtnOpenClick Error] : 当前不存在 chatInfo 无法进入聊天界面");
                        return;
                    }

                    QueueMgr.instance.AddNode(new NPGMainQueueChatMainNode(chatInfo, true));
                    break;
                case ESysSceneType.CHAPTER_MAIN:
                    QueueMgr.instance.AddNode(new GNodeChapterMap());
                    break;
                case ESysSceneType.CHAPTER_FORWARD:
                    QueueMgr.instance.AddNode(new GNodeChapterForward());
                    break;
                case ESysSceneType.EMPTY:
                    QueueMgr.instance.addNode_InGame_MainUIMainScene(NPGMainGUIAddSceneEmpty.instance, 0);
                    break;
                case ESysSceneType.MAIL:
                    QueueMgr.instance.addNode_InGame_MainUIAddWnd(GGUIWndMail.instance, UINodeTagConst.C_Main_MailNode);
                    break;
                case ESysSceneType.PLAYER:
                    QueueMgr.instance.AddNode(new GNodePlayerInfo());
                    break;
                case ESysSceneType.RANK_FIXED:
                    QueueMgr.instance.addNode_InGame_MainUIMainWnd(NPGGUIWndRankFixedList.instance, UINodeTagConst_Rank.C_MAIN_RANK_FIXED_LIST_NODE,0);
                    break;
                case ESysSceneType.QUEST:
                    // EQuestMainTab questMainTab = EQuestMainTab.MAIN_QUEST;
                    // if(_args != null && _args.Count > 0 && !string.IsNullOrEmpty(_args[0]))
                    //     ALCommon.TryEnumParse(typeof(EQuestMainTab), _args[0], out questMainTab);
                    //
                    // if (GGUIWndQuestMain.instance.isShow)
                    //     GGUIWndQuestMain.instance.setSelectTab(questMainTab);
                    // else
                    // {
                    //     QueueMgr.instance.addNode_InGame_MainUIAddWnd(GGUIWndQuestMain.instance,
                    //         EUIQueueStageType.MAIN, UINodeTagConst.C_MAIN_QUEST_NODE, null, () =>
                    //         {
                    //             GGUIWndQuestMain.instance.showWnd();
                    //             GGUIWndQuestMain.instance.setSelectTab(questMainTab);
                    //         }, true, false);
                    // }
                    break;
                case ESysSceneType.FRIEND:
                    NPRoomChatInfo roomChatInfo = NPPlayer.instance.chatComp.chatRoomList.GetFirst();
                    if (roomChatInfo == null)
                    {
                        ALLog.Error("[NPGGUISubWndMiniChat._onBtnOpenClick Error] : 当前不存在 chatInfo 无法进入聊天界面");
                        return;
                    }

                    if (GGUIWndChat.instance.isShow)
                    {
                        GGUIWndChat.instance.setCurPageType(EChatPageType.FRIEND);
                        return;
                    }
                    QueueMgr.instance.AddNode(new NPGMainQueueChatMainNode(roomChatInfo, true, EChatPageType.FRIEND));
                    break;
                case ESysSceneType.DAILY:
                    if (_args == null || _args.Count < 1)
                    {
                        //没有配置页签用默认页签
                        if (!GGUIWndDailyMain.instance.isShow)
                        {
                            QueueMgr.instance.AddNode(new GMainDailyMainNode());
                        }
                    }
                    else
                    {
                        //有配置要强切页签，这边不管解锁了
                        EDailyTab dailyTab = EDailyTab.DAILY_QUEST;
                        ALCommon.TryEnumParse(typeof(EDailyTab), _args[0],out dailyTab);
                        
                        if(GGUIWndDailyMain.instance.isShow)
                            GGUIWndDailyMain.instance.setSelectTab(dailyTab);
                        else
                        {
                            QueueMgr.instance.AddNode(new GMainDailyMainNode(() =>
                            {
                                GGUIWndDailyMain.instance.setSelectTab(dailyTab);
                            }));
                        }
                    }
                    break;
                case ESysSceneType.DIALOGUE:
                    {
                        if (_args == null || _args.Count <= 0 || !long.TryParse(_args[0], out long dialogueId))
                        {
                            Debug.LogError($"跳转失败，ESysSceneType.DIALOGUE需要至少一个参数dialogueId");
                        }
                        else
                        {
                            enterDialogueNode(dialogueId, null);
                        }
                        break;
                    }
                case ESysSceneType.DIALOGUE_NOTICE: //对话Notic界面:SceneTag:对话段id
                    {
                        if (_args == null || _args.Count <= 0 || !long.TryParse(_args[0], out long dialogueId))
                        {
                            Debug.LogError($"跳转失败，ESysSceneType.DIALOGUE_NOTICE需要至少一个参数dialogueId");
                        }
                        else
                        {
                            NPUINoticeMgr.instance.addDealer(new NoticeDealer_EnterDialogue(dialogueId));
                        }
                        break;
                    }
                case ESysSceneType.SHOP:
                    if (_args == null || _args.Count <= 0 || !long.TryParse(_args[0], out long shopRefId))
                    {
                        QueueMgr.instance.AddNode(new GShopMainNode());
                    }
                    else
                    {
                        long targetShopMainId = 0;
                        GRefdataCoreMgr.instance.shopMainRefCore.dealAllRef(_ref =>
                        {
                            if (_ref != null && _ref.shop_id_list != null && _ref.shop_id_list.Contains(shopRefId))
                                targetShopMainId = _ref.id;
                        });
                        if (targetShopMainId <= 0)
                        {
                            Debug.LogError($"跳转失败，未找到shopId:{shopRefId} 对应的shop_main表id");
                            return;
                        }
                        QueueMgr.instance.AddNode(new GShopMainNode(targetShopMainId, shopRefId));
                    }
                    break;
                case ESysSceneType.SHOP_NO_TAB:
                    if (_args == null || _args.Count <= 0 || !long.TryParse(_args[0], out long shop_ref_id))
                    {
                        Debug.LogError($"跳转失败，ESysSceneType.SHOP_NO_TAB需要1个参数,且第二个参数为shop_main_ref_id");
                    }
                    else
                    {
                        long targetShopMainId = 0;
                        GRefdataCoreMgr.instance.shopMainRefCore.dealAllRef(_ref =>
                        {
                            if (_ref != null && _ref.shop_id_list != null && _ref.shop_id_list.Contains(shop_ref_id))
                                targetShopMainId = _ref.id;
                        });
                        if (targetShopMainId <= 0)
                        {
                            Debug.LogError($"跳转失败，未找到shopId:{shop_ref_id} 对应的shop_main表id");
                            return;
                        }
                        QueueMgr.instance.AddNode(new GShopNoTabNode(targetShopMainId, shop_ref_id));
                    }
                    break;
                case ESysSceneType.SHOP_MAIN:
                    if (_args == null || _args.Count <= 0 || !long.TryParse(_args[0], out long shopMainRefId))
                    {
                        QueueMgr.instance.AddNode(new GShopMainNode());
                    }
                    else
                    {
                        QueueMgr.instance.AddNode(new GShopMainNode(shopMainRefId));
                    }
                    break;
                case ESysSceneType.SHOP_MAIN_NO_TAB:
                    if (_args == null || _args.Count <= 0 || !long.TryParse(_args[0], out long shop_main_ref_id))
                    {
                        Debug.LogError($"跳转失败，ESysSceneType.SHOP_NO_TAB需要1个参数,且第二个参数为shop_main_ref_id");
                    }
                    else
                    {
                        QueueMgr.instance.AddNode(new GShopNoTabNode(shop_main_ref_id));
                    }
                    break;
                case ESysSceneType.CUSTOM_MAIN_UI://打开自定义主窗口
                    if(_args == null || _args.Count < 2)
                        break;

                    bool needShowCloseBtn = false;//是否展示通用返回按钮
                    long mainUIResId = 0;//资源id
                    if (_args[0].ToLower() == "true")
                        needShowCloseBtn = true;
                    long.TryParse(_args[1], out mainUIResId);
                    if (mainUIResId <= 0)
                    {
                        Debug.LogError("效果C_UI_TO_SYS_SCE:CUSTOM_MAIN_UI配置的参数资源id错误");
                        break;
                    }

                    NPGGUIWndEffectCustomMainUI.instance.uiResId = mainUIResId;
                    QueueMgr.instance.addNode_InGame_MainUIMainWnd(NPGGUIWndEffectCustomMainUI.instance,
                        UINodeTagConst.C_EFFECT_CUSTOM_MAIN_UI, needShowCloseBtn ? UIResPathConst.WIN_COMMON_BACK : 0);

                    break;
                case ESysSceneType.CUSTOM_ADD_UI://打开自定义附加窗口
                    if (_args == null || _args.Count < 2)
                        break;

                    bool needShowBk = false;//是否展示模糊背景
                    long addUIResId = 0;//资源id
                    if (_args[0].ToLower() == "true")
                        needShowBk = true;
                    long.TryParse(_args[1], out addUIResId);
                    if (addUIResId <= 0)
                    {
                        Debug.LogError("效果C_UI_TO_SYS_SCE:CUSTOM_ADD_UI配置的参数资源id错误");
                        break;
                    }

                    QueueMgr.instance.AddNode(new GNodeEffectCustomAddUI(needShowBk, addUIResId));
                    break;
                case ESysSceneType.FUNC_UNLOCK:
                    if (_args == null || _args.Count < 1)
                        break;

                    ENPFunctionType functionType = ENPFunctionType.NONE;
                    ALCommon.TryEnumParse(typeof(ENPFunctionType), _args[0],out functionType);
                    List<FuncUnlockInfo> itemList = new List<FuncUnlockInfo>();
                    itemList.Add(NPPlayer.instance.funcUnlockComp.getFuncUnlockInfo(functionType));

                    QueueMgr.instance.addNode_InGame_SingleWnd(NPGGUIWndFuncUnlockTip.instance, () =>
                    {
                        NPGGUIWndFuncUnlockTip.instance.showWnd();
                        NPGGUIWndFuncUnlockTip.instance.setInfo(itemList);
                    },UINodeTagConst.C_FUNC_UNLOCK_TIP);
                    break;
                case ESysSceneType.RULE_LIST://规则列表
                    QueueMgr.instance.addNode_InGame_SingleWnd(NPGGUIWndRuleList.instance,NPGGUIWndRuleList.instance.showWnd, UINodeTagConst.C_ADD_RULE_LIST);
                    break;
                case ESysSceneType.RULE: //系统规则弹窗
                    if (_args.Count == 0)
                    {
#if UNITY_EDITOR
                        ALLog.Error($"-------------打开规则弹窗，参数不足，至少传入res_path_id 参数，示例：C_UI_TO_SYS_SCE:RULE:res_path_id:ruleindex");
#endif
                        return;
                    }

                    long res_path_id = 0;
                    if (!long.TryParse(_args[0], out res_path_id))
                    {
#if UNITY_EDITOR
                        ALLog.Error($"-------------打开规则弹窗，参数错误，示例：C_UI_TO_SYS_SCE:RULE:res_path_id:ruleindex");
#endif
                        return;
                    }

                    int ruleIndex = 0;
                    if (_args.Count > 1)
                    {
                        if(!int.TryParse(_args[1], out ruleIndex))
                        {
#if UNITY_EDITOR
                        ALLog.Error($"-------------打开规则弹窗，参数错误，示例：C_UI_TO_SYS_SCE:RULE:res_path_id:ruleindex");
#endif
                        }
                    }

                    //打开弹窗，res_path_id，ruleIndex
                    NPGGUIWndRuleMain wndRuleMain = new NPGGUIWndRuleMain(res_path_id, ruleIndex);
                    QueueMgr.instance.addNode_InGame_SingleWnd(wndRuleMain, () =>
                    {
                        wndRuleMain.showWnd();
                    }, UINodeTagConst.C_ADD_RULE_MAIN);
                    break;
                
                case ESysSceneType.CONSORT_MAIN_ENTER:
                    QueueMgr.instance.AddNode(new GNodeConsortChatMain());
                    break;
                case ESysSceneType.CONSORT_LIST:
                    QueueMgr.instance.AddNode(new GNodeConsortChatMain(EConsortChatMainPage.CONSORT_LIST));
                    break;
                case ESysSceneType.CONSORT_ENTER:
                    QueueMgr.instance.AddNode(new GNodeConsortEntry());
                    break;
                case ESysSceneType.CHANGE_ENTRANCE_CONSORT:
                    QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndChangeEntranceConsort.instance, () =>
                    {
                        GGUIWndChangeEntranceConsort.instance.showWnd();
                    }, UINodeTagConst.C_CHANGE_ENTRANCE_CONSORT);
                    break;
                case ESysSceneType.HERO_BATTLE_ENTRY:
                    QueueMgr.instance.AddNode(new GNodeHeroBattleEntry());
                    break;
                case ESysSceneType.CONSORT_DETAIL:
                    if (_args.Count == 0)
                    {
#if UNITY_EDITOR
                        ALLog.Error($"-------------情人详情至少需要1个参数：consortId");
#endif
                        return;
                    }

                    long consortId = 0;
                    if (!long.TryParse(_args[0], out consortId))
                    {
#if UNITY_EDITOR
                        ALLog.Error($"-------------打开-情人详情，参数错误，示例：C_UI_TO_SYS_SCE:CONSORT_DETAIL:consortId");
#endif
                        return;
                    }

                    GGottenConsortInfo consortInfo = NPPlayer.instance.consortComp.getConsortInfo(consortId);//获取以获得妃子数据
                    if (consortInfo == null)
                    {
                        Debug.LogError_EditorOnly($"还未获得妃子:{consortId}");
                        return;
                    }

                    if (_args.Count >= 2 && !string.IsNullOrEmpty(_args[1]))
                    {
                        if (ALCommon.TryEnumParse(typeof(EUnLockConsortDetailWndTabType), _args[1], out EUnLockConsortDetailWndTabType _consortDetailWndTabType))
                        {
                            GNodeUnLockConsortDetail.addConsortNode(NPPlayer.instance.consortComp.getConsortList(), consortInfo.consortId, _consortDetailWndTabType);
                        }
                        else if (ALCommon.TryEnumParse(typeof(EUnlockConsortDetailWndInteractionPageTabType), _args[1], out EUnlockConsortDetailWndInteractionPageTabType _consortInteractionPageTab))
                        {
                            GNodeUnLockConsortDetail.addConsortNodeInteraction(NPPlayer.instance.consortComp.getConsortList(), consortInfo.consortId, _consortInteractionPageTab);
                        }
                        else//参数都解析不出来时
                        {
                            GNodeUnLockConsortDetail.addConsortNode(NPPlayer.instance.consortComp.getConsortList(), consortInfo.consortId);
                        }
                    }
                    else //没有传入其他参数时
                    {
                        GNodeUnLockConsortDetail.addConsortNode(NPPlayer.instance.consortComp.getConsortList(), consortInfo.consortId);
                    }
                    break;
                
                case ESysSceneType.CONSORT_POWER_RELATIONS:
                    Debug.LogError("新项目GOM删除了, 若还需此功能后续再开发");
                    break;
                
                case ESysSceneType.CONSORT_INTIMACY_PROGRESS:
                    Debug.LogError("新项目GOM删除了, 若还需此功能后续再开发");
                    break;
                
                case ESysSceneType.CONSORT_INTIMACY_PROGRESS_SKILL:
                    Debug.LogError("新项目GOM删除了, 若还需此功能后续再开发");
                    break;
                
                case ESysSceneType.CONSORT_INTIMACY_PROGRESS_STORY:
                    Debug.LogError("新项目GOM删除了, 若还需此功能后续再开发");
                    break;
                case ESysSceneType.CHILD_MAIN:
                    SeatInfo selectSeat = null;
                    if (_args is { Count: > 0} && long.TryParse(_args[0], out long seatId))
                        selectSeat = NPPlayer.instance.childComp.getSeatInfo(seatId);

                    QueueMgr.instance.AddNode(new GNodeChild(selectSeat));
                    break;
                case ESysSceneType.ADULT:
                    QueueMgr.instance.AddNode(new GNodeAdultMain());
                    break;
                case ESysSceneType.HERO:
                    QueueMgr.instance.AddNode(new GNodeHero());
                    break;
                case ESysSceneType.HERO_DETAIL:
                {
                    if (_args is { Count: > 0 } && long.TryParse(_args[0], out long heroId))
                    {
                        HeroInfo heroInfo = NPPlayer.instance.heroComponent.getHeroInfo(heroId);
                        if (heroInfo == null)
                        {
                            Debug.LogError_EditorOnly($"还未获得大臣:{heroId}");
                            return;
                        }
                        HeroCardShowInfo showInfo = new HeroCardShowInfo(heroInfo, GRefdataCoreMgr.instance.heroRefCore.getRef(heroId));
                        QueueMgr.instance.AddNode(new GMainQueueHeroInfoNode(showInfo, null));
                    }
                }
                    break;
                case ESysSceneType.HERO_LOCK_DETAIL:
                {
                    if (_args is { Count: > 0 } && long.TryParse(_args[0], out long heroId))
                    {
                        HeroCardShowInfo showInfo = new HeroCardShowInfo(null, GRefdataCoreMgr.instance.heroRefCore.getRef(heroId));
                        QueueMgr.instance.AddNode(new GMainQueueHeroLockInfoNode(showInfo, null));
                    }
                }
                    break;
                case ESysSceneType.DINNER_MAIN:
                    Action dealShowDinnerMain = () =>
                    {
                        if (NPPlayer.instance.dinnerComp.dinnerInstanceId > 0)
                            enterDinner(new GDinnerInfo(NPPlayer.instance.dinnerComp.dinnerInstanceId));
                        else
                            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndDinnerCreate.instance, GGUIWndDinnerCreate.instance.showWnd, UINodeTagConst.C_DINNER_CREATE);
                    };

                    if (!GGUIWndDinnerEnterMain.instance.isShow)
                        QueueMgr.instance.AddNode(new GNodeDinnerEntry(dealShowDinnerMain));
                    else
                        dealShowDinnerMain();
                    break;
                case ESysSceneType.DINNER_LIST:
                    Action dealShowDinnerList = () =>
                    {
                        QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndDinnerListMain.instance, GGUIWndDinnerListMain.instance.showWnd, UINodeTagConst.C_DINNER_LIST);
                    };
                    if (!GGUIWndDinnerEnterMain.instance.isShow)
                        QueueMgr.instance.AddNode(new GNodeDinnerEntry(dealShowDinnerList));
                    else
                        dealShowDinnerList();
                    break;
                case ESysSceneType.DINNER_ENTER:
                    QueueMgr.instance.AddNode(new GNodeDinnerEntry());
                    break;
                case ESysSceneType.SPACE_STATION:
                    QueueMgr.instance.AddNode(new GNodeSpaceStation());
                    break;
                case ESysSceneType.TREASURE_HUNT_MAIN:
                    GNodeTreasureHuntMain.addNode();
                    break;
                case ESysSceneType.TREASURE_HUNT_LAB:
                    if (_args != null && _args.Count > 0 && long.TryParse(_args[0], out long labId))
                    {
                        GNodeTreasureHuntLab.addNode(labId);
                    }
                    else
                    {
                        GNodeTreasureHuntLab.addNode(null);
                    }
                    break;
                case ESysSceneType.TREASURE_HUNT_GAME_MAIN:
                    if(_args != null && _args.Count > 0 && long.TryParse(_args[0], out long areaId))
                    {
                        GNodeTreasureHuntGameMain.addNode(areaId);
                    }
                    else
                    {
                        GNodeTreasureHuntGameMain.addNode();
                    }
                    break;
                case ESysSceneType.TREASURE_HUNT_SELECT_AREA:
                    QueueMgr.instance.AddNode(new GNodeTreasureHuntSelectArea());
                    break;
                case ESysSceneType.TREASURE_HUNT_COLLECT_ACHIEVE:
                    QueueMgr.instance.AddNode(new GNodeTreasureHuntCollectAchieve());
                    break;
                case ESysSceneType.TREASURE_HUNT_COMPOSITE_CATALOG:
                    if (_args != null && _args.Count > 0 && ALCommon.TryEnumParse(typeof(ETreasureHuntCompositeCatalogFilterType), _args[0], out ETreasureHuntCompositeCatalogFilterType compositeFilterType))
                    {
                        QueueMgr.instance.AddNode(new GNodeTreasureHuntCompositeCatalog(compositeFilterType));
                    }
                    else
                    {
                        QueueMgr.instance.AddNode(new GNodeTreasureHuntCompositeCatalog());
                    }
                    break;
                case ESysSceneType.TREASURE_HUNT_ORE_CATALOG:
                    long oreTabRefId = -1;
                    ETreasureHuntOreCatalogFilterType oreFilterType = default;
                    bool hasOreTabRefId = false;
                    bool hasOreFilterType = false;
                    
                    if (_args != null && _args.Count > 0)
                    {
                        if(long.TryParse(_args[0], out oreTabRefId))
                            hasOreTabRefId = true;
                        else if (ALCommon.TryEnumParse(typeof(ETreasureHuntOreCatalogFilterType), _args[0], out oreFilterType))
                            hasOreFilterType = true;
                    }
                    
                    if (_args != null && _args.Count > 1 && ALCommon.TryEnumParse(typeof(ETreasureHuntOreCatalogFilterType), _args[1], out oreFilterType))
                    {
                        hasOreFilterType = true;
                    }
                    
                    if (hasOreTabRefId && hasOreFilterType)
                    {
                        QueueMgr.instance.AddNode(new GNodeTreasureHuntOreCatalog(oreTabRefId, oreFilterType));
                    }
                    else if (hasOreTabRefId)
                    {
                        QueueMgr.instance.AddNode(new GNodeTreasureHuntOreCatalog(oreTabRefId));
                    }
                    else if (hasOreFilterType)
                    {
                        QueueMgr.instance.AddNode(new GNodeTreasureHuntOreCatalog(oreFilterType));
                    }
                    else
                    {
                        QueueMgr.instance.AddNode(new GNodeTreasureHuntOreCatalog());
                    }
                    break;
                case ESysSceneType.TREASURE_HUNT_TREASURE_CATALOG:
                    long treasureTabRefId = -1;
                    ETreasureHuntTreasureCatalogFilterType treasureFilterType = default;
                    bool hasTreasureTabRefId = false;
                    bool hasTreasureFilterType = false;
                    
                    if (_args != null && _args.Count > 0)
                    {
                        if(long.TryParse(_args[0], out treasureTabRefId))
                            hasTreasureTabRefId = true;
                        else if (ALCommon.TryEnumParse(typeof(ETreasureHuntTreasureCatalogFilterType), _args[0], out treasureFilterType))
                            hasTreasureFilterType = true;
                    }
                    
                    if (_args != null && _args.Count > 1 && ALCommon.TryEnumParse(typeof(ETreasureHuntTreasureCatalogFilterType), _args[1], out treasureFilterType))
                    {
                        hasTreasureFilterType = true;
                    }
                    
                    if (hasTreasureTabRefId && hasTreasureFilterType)
                    {
                        QueueMgr.instance.AddNode(new GNodeTreasureHuntTreasureCatalog(treasureTabRefId, treasureFilterType));
                    }
                    else if (hasTreasureTabRefId)
                    {
                        QueueMgr.instance.AddNode(new GNodeTreasureHuntTreasureCatalog(treasureTabRefId));
                    }
                    else if (hasTreasureFilterType)
                    {
                        QueueMgr.instance.AddNode(new GNodeTreasureHuntTreasureCatalog(treasureFilterType));
                    }
                    else
                    {
                        QueueMgr.instance.AddNode(new GNodeTreasureHuntTreasureCatalog());
                    }
                    break;
                case ESysSceneType.TRAVEL:
                    QueueMgr.instance.AddNode(new GNodeTravelMain());
                    break;
                case ESysSceneType.TRAVEL_MAP:
                    Debug.LogError_EditorOnly($"新项目GOM先删除了，后续有需要再开发");
                    // QueueMgr.instance.addNode_InGame_MainUIMainWnd(GGUIWndTravelMapList.instance, UINodeTagConst.C_MAIN_TRAVEL_MAP_LIST ,UIResPathConst.WIN_COMMON_BACK);
                    break;
                case ESysSceneType.PLOT_DIALOGUE:
                {
                    if (_args == null || _args.Count <= 0 || !long.TryParse(_args[0], out long dialogueId))
                    {
                        Debug.LogError($"跳转失败，ESysSceneType.PLOT_DIALOGUE需要至少一个参数dialogueId");
                    }
                    else
                    {
                        NPDialogueRefObj plotDialogueRefObj = GRefdataCoreMgr.instance.dialogueMap.getRef(dialogueId);
                        if (plotDialogueRefObj == null)
                        {
                            Debug.LogError($"对话 dialogueId：{dialogueId} 找不到对应配置");
                            break;
                        }
                        QueueMgr.instance.AddNode(new GMainQueuePlotDialogueNode(plotDialogueRefObj, null, null));
                    }
                    break;
                }
                case ESysSceneType.MINI_GAME:
                    if (_args == null || _args.Count < 1)
                    {
                        Debug.LogError("跳转失败，ESysSceneType.MINI_GAME需要1个参数, mini_game_main表id");
                        break;
                    }
                    
                    if (!long.TryParse(_args[0], out long miniGameId))
                    {
                        Debug.LogError("跳转失败，ESysSceneType.MINI_GAME参数错误，示例：C_UI_TO_SYS_SCE:MINI_GAME:mini_game_main表id");
                        break;
                    }
                    
                    MiniGameMgr.enterGame(miniGameId, MiniGameMgr.exitGame, null);
                    break;
                    
                case ESysSceneType.SIMPLE_COMIC:
                    if (_args == null || _args.Count <= 0 || !long.TryParse(_args[0], out long comicId))
                    {
                        Debug.LogError($"跳转失败，ESysSceneType.comicId");
                    }
                    else
                    {
                        SimpleComicRefObj simpleComicRefObj = GRefdataCoreMgr.instance.simpleCommicRefCore.getRef(comicId);
                        if (null == simpleComicRefObj)
                        {
                            Debug.LogError($"简单漫画id comicId：{comicId} 找不到对应配置");
                            return;
                        }

                        QueueMgr.instance.AddNode(new GMainQueueSimpleComicNode(simpleComicRefObj, null, true));
                    }
                    break;
                case ESysSceneType.CREATE_PLAYER:
                    GCommon.showPlayerCreatName();
                    break;
                case ESysSceneType.OPEN_SET_PREFAB_WND:
                    QueueMgr.instance.addNode_InGame_MainUIMainWnd(GGUIWndCreatePlayerPrefab.instance, UINodeTagConst.C_OPEN_SET_PREFAB_WND,null, null, 0);
                    break;
                case ESysSceneType.CHAT_SHARE_HERO:
                    QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndShareHero.instance, GGUIWndShareHero.instance.showWnd, UINodeTagConst.C_CHAT_SELECT_SHARE_HERO);
                    break;
                case ESysSceneType.CHAT_SHARE_CONSORT:
                    QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndShareConsort.instance, GGUIWndShareConsort.instance.showWnd, UINodeTagConst.C_Main_Chat_SHARE_CONSORT);
                    break;
                case ESysSceneType.CHAT_SHARE_CHILD:
                    QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndShareChild.instance, GGUIWndShareChild.instance.showWnd, UINodeTagConst.C_CHAT_SELECT_SHARE_CHILD);
                    break;
                case ESysSceneType.WEEK_CARD_NOTICE:
                    NPUINoticeMgr.instance.addDealer(new NPNoticeDealer_WeekCard());
                    break;
                case ESysSceneType.WEEK_CARD:
                    QueueMgr.instance.addNode_InGame_MainUIMainWnd(GGUIWndWeekCard.instance);
                    break;
                case ESysSceneType.WEEK_CARD_ASSIGN:
                    QueueMgr.instance.addNode_InGame_MainUIMainWnd(GGUIWndWeekCardAssign.instance);
                    break;
                case ESysSceneType.ADD_FRIEND:
                    QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndFriendAdd.instance, GGUIWndFriendAdd.instance.showWnd);
                    break;
                case ESysSceneType.ADD_PACK:
                    QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndAddPack.instance, GGUIWndAddPack.instance.showWnd);
                    break;
                case ESysSceneType.ANNOUNCEMENT:
                    QueueMgr.instance.AddNode(new GNodeAnnouncement());
                    break;
                case ESysSceneType.QUESTIONNAIRE:
                    QuestionnaireInfo questionnaireInfo = NPPlayer.instance.questionnaireComp.getQuestionnaireInfo();
                    if (questionnaireInfo == null)
                        break;

                    QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndQuestionnaire.instance, () =>
                    {
                        GGUIWndQuestionnaire.instance.showWnd();
                        GGUIWndQuestionnaire.instance.setInfo(questionnaireInfo);
                    }, UINodeTagConst.C_QUESTIONNAIRE);
                    break;
                case ESysSceneType.SHOW_INTRO_STORY:
                    QueueMgr.instance.AddNode(new GNodeIntroStoryNew(null, null));
                    break;
                case ESysSceneType.PLAYER_RENAME:
                    showPlayerCommonRename();
                    break;
                case ESysSceneType.GUILD:
                    //是否有加入联盟
                    if (NPPlayer.instance.guildComp.isJoinGuild())
                    {
                        //已加入
                        QueueMgr.instance.AddNode(new GNodeGuildMain());
                    }
                    else
                    {
                        //未加入，打开申请界面
                        QueueMgr.instance.AddNode(new GNodeGuildApply());
                    }
                    break;
                case ESysSceneType.EQUIP:
                    QueueMgr.instance.addNode_InGame_MainUIMainScene(GMainGUIMainSceneEquipMain.instance, UINodeTagConst.C_EQUIP_MAIN, 0);
                    break;
                case ESysSceneType.SUMMON_MAIN://召唤主界面
                    QueueMgr.instance.AddNode(new GNodeSummonMain());
                    break;
                case ESysSceneType.RECRUIT_MAIN://招募主界面
                    QueueMgr.instance.AddNode(new GNodeRecruitMain());
                    break;
                case ESysSceneType.ARENA:
                    BaseQueueNode arenaNode = QueueMgr.instance.findLastNode(UINodeTagConst.C_ARENA_MAIN);
                    if (arenaNode != null)
                    {
                        //如果已经存在，说明已经在系统中，退出到主界面，重新检查需要打开的界面
                        QueueMgr.instance.QuitUntilCanStop(_node => _node == arenaNode);
                        GGUIWndArenaMain.instance.checkIsInBattle();
                    }
                    else
                    {
                        QueueMgr.instance.AddNode(new GNodeArenaMain());
                    }
                    break;
                case ESysSceneType.STAGE_GOAL:
                    if (_args != null && _args.Count > 0 && ALCommon.TryEnumParse(typeof(GGUIMonoStageGoalTabType), _args[0], out GGUIMonoStageGoalTabType targetStageGoalTabType))
                    {
                        if (GGUIWndStageGoal.instance.isShow)
                            GGUIWndStageGoal.instance.setSelectTab(targetStageGoalTabType);
                        else
                            QueueMgr.instance.addNode_InGame_MainUIMainWnd(GGUIWndStageGoal.instance, UINodeTagConst.C_STAGE_GOAL, null, () =>
                            {
                                GGUIWndStageGoal.instance.setSelectTab(targetStageGoalTabType);
                            }, 0);
                    }
                    else
                    {
                        if(!GGUIWndStageGoal.instance.isShow)
                            QueueMgr.instance.addNode_InGame_MainUIMainWnd(GGUIWndStageGoal.instance, UINodeTagConst.C_STAGE_GOAL, null, GGUIWndStageGoal.instance.setDefaultTab, 0);
                    }

                    break;
                case ESysSceneType.COMMON_TARGET_REWARD:
                    if (_args is not { Count: > 0 })
                        return;
                    
                    if (long.TryParse(_args[0], out long resId))
                    {
                        GGUIWndCommonTargetReward wnd = new GGUIWndCommonTargetReward(resId);
                        QueueMgr.instance.addNode_InGame_SingleWnd(wnd, () =>
                        {
                            wnd.showWnd();
                        }, UINodeTagConst.C_COMMON_TARGET_REWARD);
                    }
                    break;
                case ESysSceneType.CONSORT_LOCK_DETAIL:
                    if (_args is not { Count: > 0 })
                        return;
                    
                    if (long.TryParse(_args[0], out long consortID))
                    {
                        QueueMgr.instance.AddNode(new GNodeLockConsortDetail(new ConsortRefShowInfo(consortID)));
                    }
                    break;
                case ESysSceneType.RANK_RUSH:

                    //如果界面已经打开，直接切换页签
                    if(GGUIWndLimitActivityMain.instance.isShow)
                        GGUIWndLimitActivityMain.instance.setInfo(ELimitActivityTabType.RANK_RUSH);
                    else
                        QueueMgr.instance.AddNode(new GMainQueueLimitActivityMainNode(ELimitActivityTabType.RANK_RUSH));

                    break;
                case ESysSceneType.STEP_REWARD:

                    //如果界面已经打开，直接切换页签
                    if(GGUIWndLimitActivityMain.instance.isShow)
                        GGUIWndLimitActivityMain.instance.setInfo(ELimitActivityTabType.STEP_REWARD);
                    else
                        QueueMgr.instance.AddNode(new GMainQueueLimitActivityMainNode(ELimitActivityTabType.STEP_REWARD));

                    break;
                case ESysSceneType.PLAYER_HERO_UNLOCK:
                    QueueMgr.instance.AddNode(new GNodePlayerHeroGain());
                    break;
                case ESysSceneType.TOWER_MAIN:
                    QueueMgr.instance.addNode_InGame_MainUIMainWnd(GGUIWndTowerMain.instance, UINodeTagConst.C_TOWER_MAIN, null, GGUIWndTowerMain.instance.setToCurrentChapterPos, 0);
                    break;
                case ESysSceneType.TOWER_CHALLENGE_INFO:
                    if (_args != null && _args.Count > 0 && bool.TryParse(_args[0], out bool moveToBottom))
                        GGUIWndTowerChallenge.instance.setInfo(moveToBottom);
                    else
                        GGUIWndTowerChallenge.instance.setInfo(false);
                    QueueMgr.instance.addNode_InGame_MainUIMainWnd(GGUIWndTowerChallenge.instance, UINodeTagConst.C_TOWER_CHALLENGE, 0);
                    break;
                case ESysSceneType.TOWER_RESEARCH:
                    QueueMgr.instance.addNode_InGame_MainUIAddWnd(GGUIWndTowerResearch.instance, UINodeTagConst.C_TOWER_RESEARCH);
                    break;
                case ESysSceneType.EVENING_DUNGEON_GAME:
                    QueueMgr.instance.AddNode(new GNodeEveningDungeonGame());
                    break;
                case ESysSceneType.MIDDAY_DUNGEON_BATTLE:
                {
                    if (NPPlayer.instance.middayDungeonComp.activityState != EMiddayDungeonActivityState.ONGOING)
                    {
                        NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.midday_dungeon_activity_no_open_tip);
                        return;
                    }
                    if (NPPlayer.instance.middayDungeonComp.bossInfo == null 
                        || GRefdataCoreMgr.instance.middayDungeonWaveRefCore.getRef(NPPlayer.instance.middayDungeonComp.bossInfo.wave) == null)
                    {
                        NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.midday_dungeon_boss_has_clear_tip) ;
                        return;
                    }
                    GGUIWndMiddayDungeonBattle.instance.setInfo();
                    QueueMgr.instance.addNode_InGame_MainUIMainWnd(GGUIWndMiddayDungeonBattle.instance, UINodeTagConst.C_MIDDAY_DUNGEON_BATTLE, 0);
                }
                    break;
                case ESysSceneType.EVENING_DUNGEON_ENTRANCE:
                    QueueMgr.instance.AddNode(new GNodeEveningDungeonEntrance());
                    break;
                case ESysSceneType.DUNGEON_ENTRANCE:
                    QueueMgr.instance.AddNode(new GNodeDungeonEntrance());
                    break;
                case ESysSceneType.GUILD_MEMBER:
                    QueueMgr.instance.addNode_InGame_MainUIMainWnd(GGUIWndGuildMemberList.instance, UINodeTagConst_Guild.C_GUILD_MEMBER_LIST);
                    break;
                case ESysSceneType.GUILD_ENTRUST:
                    QueueMgr.instance.AddNode(new GNodeGuildEntrust());
                    break;
                case ESysSceneType.GUILD_DISPATCH:
                    QueueMgr.instance.AddNode(new GNodeGuildDispatch());
                    break;
                case ESysSceneType.SEVEN_DAY_LOGIN:
                    QueueMgr.instance.addNode_InGame_MainUIAddWnd(GGUIWndSevenDayLogin.instance, UINodeTagConst.C_SEVEN_DAY_LOGIN);
                    break;
                case ESysSceneType.ACTIVITY_EXCHANGE_SHOP:
                    if (_args == null || _args.Count <= 0 || !long.TryParse(_args[0], out long activityId_2))
                    {
                        Debug.LogError($"跳转失败，ESysSceneType.ACTIVITY_EXCHANGE_SHOP 需要1个参数 activityId");
                        break;
                    }
                    QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndActivityExchangeShop.instance, () =>
                    {
                        GGUIWndActivityExchangeShop.instance.showWnd();
                        GGUIWndActivityExchangeShop.instance.setInfo(activityId_2);
                    },UINodeTagConst.C_ACTIVITY_EXCHANGE_SHOP);
                    break;
                case ESysSceneType.ACTIVITY_GIFT_PACK:
                    if (_args == null || _args.Count <= 0 || !long.TryParse(_args[0], out long activityId_3))
                    {
                        Debug.LogError($"跳转失败，ESysSceneType.ACTIVITY_GIFT_PACK 需要1个参数 activityId");
                        break;
                    }
                    QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndActivityGiftPack.instance, () =>
                    {
                        GGUIWndActivityGiftPack.instance.showWnd();
                        GGUIWndActivityGiftPack.instance.setInfo(activityId_3);
                    },UINodeTagConst.C_ACTIVITY_GIFT_PACK);
                    break;
                case ESysSceneType.ACTIVITY_RANK:
                    if (_args == null || _args.Count <= 0 || !long.TryParse(_args[0], out long activityId_4))
                    {
                        Debug.LogError($"跳转失败，ESysSceneType.ACTIVITY_RANK 需要1个参数 activityId");
                        break;
                    }

                    ERankRushDetailTabType targetDetailTab = ERankRushDetailTabType.REWARD;
                    bool needShowGiftBtn = false;
                    if (_args != null && _args.Count > 1)
                        ALCommon.TryEnumParse(typeof(ERankRushDetailTabType), _args[1], out targetDetailTab);
                    if (_args != null && _args.Count > 2)
                        needShowGiftBtn = ALCommon.GetBool(_args[2]);

                    _ABaseActivityInfo activityInfo = NPPlayer.instance.commonActivityComp.getValidActivityInfoByActivityId(activityId_4);
                    if (activityInfo != null)
                    {
                        //没有重复的活动，默认获取第一个活动
                        if (activityInfo != null && activityInfo.rankRushInfoList != null && activityInfo.rankRushInfoList.Count > 0)
                        {
                            //默认获取第一个冲榜信息
                            ActivityRankRushInfo rankRushInfo = activityInfo.rankRushInfoList[0];

                            //TODO 由于活动排行榜界面和冲榜界面一样，数据也一样，这里直接打开冲榜界面，有不同之后再改
                            if (rankRushInfo.isGuildRankRush)
                                QueueMgr.instance.AddNode(new GNodeGuildRankRushDetail(rankRushInfo, targetDetailTab, needShowGiftBtn));
                            else
                                QueueMgr.instance.AddNode(new GNodeRankRushDetail(rankRushInfo, targetDetailTab, needShowGiftBtn));
                        }
                        else
                            Debug.LogError($"跳转失败，ESysSceneType.ACTIVITY_RANK 未找到对应活动，activityId:{activityId_4}");
                    }
                    else
                        Debug.LogError($"跳转失败，ESysSceneType.ACTIVITY_RANK 未找到对应活动，activityId:{activityId_4}");

                    break;
                case ESysSceneType.EARNING_GOAL_MAIN:
                    // if (NPPlayer.instance.earningGoalComp.isEarningGoalActivityOpen())
                    //     QueueMgr.instance.addNode_InGame_MainUIMainWnd(GGUIWndEarningGoalMain.instance, UINodeTagConst.C_EARNING_GOAL_MAIN, 0);
                    // else
                    //     QueueMgr.instance.addNode_InGame_MainUIAddWnd(GGUIWndEarningGoalSelf.instance, UINodeTagConst.C_EARNING_GOAL_SELF_REWARD);
                    
                    break;
                case ESysSceneType.EARNING_GOAL_GLOBAL_REWARD:
                    QueueMgr.instance.addNode_InGame_MainUIAddWnd(GGUIWndEarningGoalGlobal.instance, UINodeTagConst.C_EARNING_GOAL_GLOBAL_REWARD);
                    break;
                case ESysSceneType.EARNING_GOAL_SELF_REWARD:
                    QueueMgr.instance.addNode_InGame_MainUIAddWnd(GGUIWndEarningGoalSelf.instance, UINodeTagConst.C_EARNING_GOAL_SELF_REWARD);
                    break;
                case ESysSceneType.SEVEN_DAY_GOALS:
                    // GGUIWndSevenDayGoals.instance.refreshWnd(NPPlayer.instance.sevenDayGoalsComp.data.getNowDay());
                    // QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndSevenDayGoals.instance, GGUIWndSevenDayGoals.instance.showWnd, UINodeTagConst.C_SEVEN_DAY_GOALS);
                    break;
                case ESysSceneType.RANK_FIXED_DETAIL_NORMAL: //常驻排行榜界面
                    {
                    if (_args.Count == 0)
                    {
#if UNITY_EDITOR
                        ALLog.Error($"-------------打开常驻排行榜界面，参数不足，至少传入rank_fixed_id 参数，示例：C_UI_TO_SYS_SCE:RANK_FIXED_DETAIL_NORMAL:rank_fixed_id:ui_path_id");
#endif
                        return;
                    }

                    if (!long.TryParse(_args[0], out long rank_fixed_id))
                    {
#if UNITY_EDITOR
                        ALLog.Error($"-------------打开常驻排行榜界面，参数错误，示例：C_UI_TO_SYS_SCE:RANK_FIXED_DETAIL_NORMAL:rank_fixed_id:ui_path_id");
#endif
                        return;
                    }

                    //是否是联盟排行榜
                    NPRankFixedRefObj rankFixRef = GRefdataCoreMgr.instance.rankFixedRefCore.getRef(rank_fixed_id);
                    NPRankRefObj rankRef = GRefdataCoreMgr.instance.rankCommonRefCore.getRef(rankFixRef != null ? rankFixRef.rank_id : 0);
                    bool isGuild = rankRef != null && rankRef.rank_type == ERankType.GUILD;

                    long ui_path_id = isGuild ? UIResPathConst.C_COMMON_RANK_GUILD_NORMAL_RES_ID : UIResPathConst.C_COMMON_RANK_NORMAL_RES_ID;
                    if (_args.Count > 1)
                    {
                        if(!long.TryParse(_args[1], out ui_path_id))
                        {
#if UNITY_EDITOR
                            ALLog.Error($"-------------打开常驻排行榜界面，参数错误，示例：C_UI_TO_SYS_SCE:RANK_FIXED_DETAIL_NORMAL:rank_fixed_id:ui_path_id");
#endif
                        }
                    }
                    if(isGuild)
                        QueueMgr.instance.AddNode(new GNodeGuildRank(rank_fixed_id, ui_path_id));
                    else
                        QueueMgr.instance.AddNode(new GNodeRank(rank_fixed_id, ui_path_id));
                    }
                    break;
                case ESysSceneType.RANK_FIXED_DETAIL_ADDITION: //常驻排行榜附加界面
                    {
                    if (_args.Count == 0)
                    {
#if UNITY_EDITOR
                        ALLog.Error($"-------------打开常驻排行榜附加界面，参数不足，至少传入rank_id 参数，示例：C_UI_TO_SYS_SCE:RANK_FIXED_DETAIL_ADDITION:rank_fixed_id:ui_path_id");
#endif
                        return;
                    }

                    if (!long.TryParse(_args[0], out long rank_fixed_id))
                    {
#if UNITY_EDITOR
                        ALLog.Error($"-------------打开常驻排行榜附加界面，参数错误，示例：C_UI_TO_SYS_SCE:RANK_FIXED_DETAIL_ADDITION:rank_fixed_id:ui_path_id");
#endif
                        return;
                    }

                    //是否是联盟排行榜
                    NPRankFixedRefObj rankFixRef = GRefdataCoreMgr.instance.rankFixedRefCore.getRef(rank_fixed_id);
                    NPRankRefObj rankRef = GRefdataCoreMgr.instance.rankCommonRefCore.getRef(rankFixRef != null ? rankFixRef.rank_id : 0);
                    bool isGuild = rankRef != null && rankRef.rank_type == ERankType.GUILD;

                    long ui_path_id = isGuild ? UIResPathConst.C_COMMON_RANK_GUILD_ADDITION_RES_ID : UIResPathConst.C_COMMON_RANK_ADDITION_RES_ID;
                    if (_args.Count > 1)
                    {
                        if(!long.TryParse(_args[1], out ui_path_id))
                        {
#if UNITY_EDITOR
                            ALLog.Error($"-------------打开常驻排行榜附加界面，参数错误，示例：C_UI_TO_SYS_SCE:RANK_FIXED_DETAIL_ADDITION:rank_fixed_id:ui_path_id");
#endif
                        }
                    }

                    if(isGuild)
                        QueueMgr.instance.AddNode(new GNodeGuildRankAddition(rank_fixed_id, ui_path_id));
                    else
                        QueueMgr.instance.AddNode(new GNodeRankAddition(rank_fixed_id, ui_path_id));
                    }
                    break;
                case ESysSceneType.FUNC_PREVIEW:
                    QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndFuncDetail.instance, GGUIWndFuncDetail.instance.showWnd, UINodeTagConst.C_FUNC_DETAIL);
                    break;
                case ESysSceneType.ACTIVITY_CENTER:
                    long activityCenterId = 0;
                    if (_args != null && _args.Count > 0)
                        long.TryParse(_args[0], out activityCenterId);

                    if (GGUIWndActivityCenter.instance.isShow)
                    {
                        //如果有传入activityCenterId，则设置选中tab
                        if (activityCenterId > 0)
                            GGUIWndActivityCenter.instance.setSelectTab(activityCenterId);
                    }
                    else
                    {
                        QueueMgr.instance.addNode_InGame_MainUIMainWnd(GGUIWndActivityCenter.instance, UINodeTagConst.C_ACTIVITY_CENTER_WND, null,
                            () =>
                            {
                                GGUIWndActivityCenter.instance.showWnd();

                                //如果有传入activityCenterId，则设置选中tab
                                if (activityCenterId > 0)
                                {
                                    GGUIWndActivityCenter.instance.setSelectTab(activityCenterId);
                                }
                            }, 0);
                    }
                    break;
                case ESysSceneType.COUNTDOWN_EVENT:
                    CountdownEventInfo countdownEventInfo = NPPlayer.instance.countdownEventComp.countdownEventInfo;
                    if (countdownEventInfo == null || !countdownEventInfo.isValid)
                    {
                        Debug.LogError($"打开倒计时事件弹窗失败，没有有效的倒计时事件");
                        return;
                    }

                    GGUIWndCountdownEvent cdEventWnd = new GGUIWndCountdownEvent(countdownEventInfo.countdownEventRef.ui_res_id);
                    QueueMgr.instance.addNode_InGame_SingleWnd(cdEventWnd, ()=>
                    {
                        cdEventWnd.showWnd();
                        cdEventWnd.setInfo(countdownEventInfo);
                    }, UINodeTagConst.C_COUNTDOWN_EVENT_WND);
                    break;
                case ESysSceneType.CONSORT_CHAT_MAIN:
                    QueueMgr.instance.AddNode(new GNodeConsortChatMain(EConsortChatMainPage.CHAT));
                    break;
                case ESysSceneType.INN_MAIN:
                    BaseQueueNode innNode = QueueMgr.instance.findLastNode(typeof(GNodeInnMain));
                    if (innNode == null)
                        QueueMgr.instance.AddNode(new GNodeInnMain());
                    else
                        QueueMgr.instance.QuitUntilCanStop(_node => _node == innNode);
                    break;
                case ESysSceneType.STAGE_GOAL_OVERVIEW:
                    QueueMgr.instance.addNode_InGame_MainUIMainWnd(GGUIWndStageGoal.instance, UINodeTagConst.C_STAGE_GOAL, null, ()=>
                    {
                        GGUIWndStageGoal.instance.setSelectTab(GGUIMonoStageGoalTabType.OVERVIEW);
                    }, 0);
                    break;
                case ESysSceneType.HIRE_MAIN:
                    long _hireBuildingId = 0;
                    if (_args != null && _args.Count > 0 && long.TryParse(_args[0], out _hireBuildingId))
                    {
                        QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndHireMain.instance, () =>
                        {
                            GGUIWndHireMain.instance.showWnd();
                            GGUIWndHireMain.instance.setInfo(_hireBuildingId);
                        }, UINodeTagConst.C_HIRE_MAIN);
                    }
                    break;
                case ESysSceneType.CASH_GIFT_PACK:
                    ECashGiftPackMainTabType cashGiftPackTabType = ECashGiftPackMainTabType.PERMANENT;
                    if (_args != null && _args.Count > 0)
                        ALCommon.TryEnumParse(typeof(ECashGiftPackMainTabType), _args[0], out cashGiftPackTabType);
                    ECashGiftPackSpecialDealType cashGiftPackSpecialDealType = ECashGiftPackSpecialDealType.NONE;
                    if (_args != null && _args.Count > 1)
                        ALCommon.TryEnumParse(typeof(ECashGiftPackSpecialDealType), _args[1], out cashGiftPackSpecialDealType);

                    QueueMgr.instance.addNode_InGame_MainUIMainWnd(GGUIWndCashGiftPackMain.instance, UINodeTagConst.C_CASH_GIFT_PACK_MAIN, () =>
                    {
                        GGUIWndCashGiftPackMain.instance.setInfo(cashGiftPackTabType);
                        if(cashGiftPackSpecialDealType != ECashGiftPackSpecialDealType.NONE)
                            GGUIWndCashGiftPackMain.instance.setSpecialDealType(cashGiftPackSpecialDealType);

                    }, null, 0);
                    break;
                case ESysSceneType.FUND:
                    if (!GCommon.isFuncUnlock(ENPFunctionType.ACTIVITY_FUND, true))
                        return;
                    
                    long fundTabId = -1;
                    if (_args != null && _args.Count > 0)
                        long.TryParse(_args[0], out fundTabId);

                    QueueMgr.instance.addNode_InGame_MainUIMainWnd(GGUIWndCashGiftPackMain.instance, UINodeTagConst.C_CASH_GIFT_PACK_MAIN, () =>
                    {
                        GGUIWndCashGiftPackMain.instance.setSelectTab(ECashGiftPackMainTabType.FUND);
                        GGUIWndCashGiftPackMain.instance.setFundPageId(fundTabId);
                    }, null, 0);
                    break;
                case ESysSceneType.GRAVE_MAIN:
                    MainAdditionGraveMainTDScene.instance.initData(() =>
                    {
                        QueueMgr.instance.AddNode(new GNodeGraveMain());
                    });
                    break;
                case ESysSceneType.CONSORT_CG_MAIN:
                    if (_args != null && _args.Count > 0 && ALCommon.TryEnumParse(typeof(EConsortCGType), _args[0], out EConsortCGType cgType))
                    {
                        QueueMgr.instance.AddNode(new GNodeConsortCGMain(cgType));
                    }
                    else
                    {
                        QueueMgr.instance.AddNode(new GNodeConsortCGMain());
                    }
                    break;
                case ESysSceneType.PLAYER_LEVEL_PREVIEW:
                    QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndPlayerLvPreview.instance, GGUIWndPlayerLvPreview.instance.showWnd, EUIQueueStageType.MAIN, UINodeTagConst_PlayerInfo.C_ADD_PLAYERINFO_LV_PREVIEW_NODE, false, false);
                    break;
   				case ESysSceneType.GUILD_DUNGEON_MAIN:
                    QueueMgr.instance.addNode_InGame_MainUIMainWnd(GGUIWndGuildDungeonMain.instance, UINodeTagConst.C_GUIlD_DUNGEON_MAIN, null, null, 0);
                    break;
                case ESysSceneType.GUILD_MARS_HELP:
                    QueueMgr.instance.addNode_InGame_MainUIMainWnd(GGUIWndGuildMarsHelp.instance, UINodeTagConst.C_GUIlD_MARS_HELP, null, null, 0);
                    break;
   				case ESysSceneType.MARS_MISSION_PREVIEW:
                    QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMarsMissionPreview.instance, GGUIWndMarsMissionPreview.instance.showWnd,UINodeTagConst.C_MARS_MISSION_PREVIEW);
                    break;
   				case ESysSceneType.MARS:
                    if (_args.Count > 1)
                    {
                        Vector3 pos = Vector3.zero;
                        ALCommon.TryParseFloat(_args[1], out float duration);
                        long.TryParse(_args[0], out long id);
                        void FocusToTarget()
                        {
                            if (_args is {Count: > 2} && bool.TryParse(_args[2], out bool showGuide) && showGuide && !Game.instance.isInTutorial)
                            {
                                MainAdditionMarsTDScene.instance.focusAndShowGuideHand(id, duration);
                            }
                            else
                            {
                                MainAdditionMarsTDScene.instance.tryGetBuildingPos(id, out pos);
                                MainAdditionMarsTDScene.instance.focusToTarget(pos, duration);
                            }
                        }

                        if (QueueMgr.instance._lastNode is GNodeMars)
                            FocusToTarget();
                        else
                            GCommon.dealEnterMars(FocusToTarget);
                    }
                    else
                    {
                        GCommon.dealEnterMars();
                    }
                    break;
                case ESysSceneType.MARS_INTELLIGENT_CONTROL:
                    // 火星智能控制主界面
                    QueueMgr.instance.addNode_InGame_MainUIMainWnd(GGUIWndMarsIntelligentControl.instance, UINodeTagConst.C_MARS_INTELLIGENT_CONTROL, null,
                        () =>
                        {
                            GGUIWndMarsIntelligentControl.instance.showWnd();
                        },
                        0);
                    break;
                case ESysSceneType.MARS_INTELLIGENT_CONTROL_DETAIL:
                    // 参数: 智能控制配置id
                    if (_args == null || _args.Count <= 0 || !long.TryParse(_args[0], out long intelligentControlId))
                        break;

                    var info = NPPlayer.instance?.marsComp?.peopleSubComponent?.getIntelligentControlInfo(intelligentControlId);
                    if (info == null)
                        break;

                    GGUIWndMarsIntelligentControlDetail.addNode(info);
                    // QueueMgr.instance.addNode_InGame_MainUIMainWnd(GGUIWndMarsIntelligentControlDetail.instance, UINodeTagConst.C_MARS_INTELLIGENT_CONTROL_DETAIL, null,
                    //     () =>
                    //     {
                    //         GGUIWndMarsIntelligentControlDetail.instance.showWnd();
                    //         GGUIWndMarsIntelligentControlDetail.instance.setInfo(info);
                    //     },
                    //     0);
                    break;
                case ESysSceneType.MARS_POPULAR_WILL:
                    EMarsPopularWillTabType tabType = EMarsPopularWillTabType.NONE;
                    if (_args != null && _args.Count > 0 && !string.IsNullOrEmpty(_args[0]))
                    {
                        ALCommon.TryEnumParse(typeof(EMarsPopularWillTabType), _args[0], out tabType);
                    }
                    QueueMgr.instance.AddNode(new GNodeMarsPeopleWill(tabType));
                    break;
                case ESysSceneType.GEM_GIFT_PACK:
                    QueueMgr.instance.addNode_InGame_MainUIMainWnd(GGUIWndGemGiftPackMain.instance, UINodeTagConst.C_GEM_GIFT_PACK_MAIN, null, null, 0);
                    break;
                case ESysSceneType.SCHOOL:
                    QueueMgr.instance.AddNode(new GNodeSchool());
                    break;
                case ESysSceneType.EQUIP_RECYCLE:
                    QueueMgr.instance.AddNode(new GNodeEquipRecycle());
                    break;
                case ESysSceneType.GUILD_COOPERATE:
                    QueueMgr.instance.AddNode(new GNodeGuildCooperate());
                    break;
                case ESysSceneType.INN_STATION:
                    InnStationInfo stationInfo = null;
                    if (_args is { Count: > 0 } && long.TryParse(_args[0], out long stationId))
                    {
                        stationInfo = NPPlayer.instance.innComp.getStationInfoById(stationId);
                        if (stationInfo == null)
                        {
                            Debug.LogError($"跳转失败，ESysSceneType.INN_STATION 站点id错误，未找到对应站点，stationId:{stationId}");
                            return;
                        }
                    }
                    
                    GNodeInnMain innMainNode = QueueMgr.instance.findLastNode(typeof(GNodeInnMain)) as GNodeInnMain;
                    if (innMainNode == null)
                    {
                        innMainNode = new GNodeInnMain();
                        QueueMgr.instance.AddNode(innMainNode);
                    }
                    
                    GGUIWndInnStationList.instance.refreshWnd(innMainNode.viewMgr, stationInfo);
                    QueueMgr.instance.addNode_InGame_MainUIAddWnd(GGUIWndInnStationList.instance, UINodeTagConst.C_INN_STATION_LIST);
                    break;
                case ESysSceneType.VIP:
                    QueueMgr.instance.addNode_InGame_MainUIMainWnd(GGUIWndVIPMain.instance, UINodeTagConst.C_VIP_MAIN, null, null, 0);
                    break;
                case ESysSceneType.FIRST_RECHARGE:
                    QueueMgr.instance.AddNode(new GNodeFirstRecharge());
                    break;
                case ESysSceneType.INN_DISH_UNLOCK:
                    InnDishInfo dishInfo = null;
                    if (_args is { Count: > 0 } && long.TryParse(_args[0], out long dishId))
                        dishInfo = NPPlayer.instance.innComp.getDishInfoById(dishId);

                    if (dishInfo == null)
                    {
                        Debug.LogError($"跳转失败，ESysSceneType.INN_DISH_UNLOCK id 错误，未找到对应菜品");
                        return;
                    }
                    
                    QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_INN_MENU);
                    QueueMgr.instance.addNode_InGame_MainUIMainWnd(GGUIWndInnMenu.instance, UINodeTagConst.C_INN_MENU,null,
                        () =>
                        {
                            List<InnDishInfo> dishList = GGUIWndInnMenu.instance.itemGridWnd?.getDishList();
                            GGUIWndInnDishUnlock.instance.refreshWnd(dishInfo, dishList);
                            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndInnDishUnlock.instance, GGUIWndInnDishUnlock.instance.showWnd, UINodeTagConst.C_INN_DISH_UNLOCK);
                        });
                    break;
                case ESysSceneType.MARS_TECHNOLOGY_TREE:
                    MarsBuildingInfo technologyBuildingInfo = NPPlayer.instance.marsComp.buildingSubComponent.getBuildingInfoByType(EMarsBuildingType.TECHNOLOGY);
                    if (technologyBuildingInfo == null)
                    {
                        Debug.LogError($"跳转失败，ESysSceneType.MARS_TECHNOLOGY_TREE 未找到科技建筑");
                        break;
                    }

                    if (technologyBuildingInfo.state == MarsBuildingInfo.StateType.Unbuilt || technologyBuildingInfo.state == MarsBuildingInfo.StateType.Constructing)
                    {
                        if (!technologyBuildingInfo.checkCanShowBuildBtn())
                        {
                            NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.mars_technology_buildingUnbuiltTip_none);
                            break;
                        }
                            
                        MarsUtil.jumpToBuildingUpgrade(technologyBuildingInfo);
                    }
                    else
                    {
                        // 参数格式: C_UI_TO_SYS_SCE:MARS_TECHNOLOGY_TREE(:technologyType/EMarsEnterTreeWndParamType:param)
                        EMarsTechnologyType technologyType = EMarsTechnologyType.NONE;
                        if (_args != null && _args.Count > 0)
                        {
                            if (!ALCommon.TryEnumParse(typeof(EMarsTechnologyType), _args[0], out technologyType))
                            {
                                if (ALCommon.TryEnumParse(typeof(EMarsEnterTreeWndParamType), _args[0], out EMarsEnterTreeWndParamType _enterTreeWndParamType))
                                {
                                    switch (_enterTreeWndParamType)
                                    {
                                        case EMarsEnterTreeWndParamType.TECHNOLOGY_TYPE:
                                            if(_args.Count > 1)
                                                ALCommon.TryEnumParse(typeof(EMarsTechnologyType), _args[1], out technologyType);
                                            break;
                                        case EMarsEnterTreeWndParamType.TECHNOLOGY_ID:
                                            if (_args.Count > 1 && long.TryParse(_args[1], out long technologyId))
                                            {
                                                if (QueueMgr.instance._lastNode.nodeTag == UINodeTagConst.C_MARS_TECHNOLOGY_TREE)
                                                {
                                                    GGUIWndMarsTechnologyTree.instance.focusTechnology(technologyId);
                                                }
                                                else
                                                {
                                                    QueueMgr.instance.AddNode(new GNodeMarsTechnologyTree_ByTechnologyId(technologyId));
                                                }
                                                return;
                                            }
                                            break;
                                        case EMarsEnterTreeWndParamType.TECHNOLOGY_SKILL_TYPE:
                                            if(_args.Count > 1 && int.TryParse(_args[1], out int techSkillType))
                                            {
                                                if (QueueMgr.instance._lastNode.nodeTag == UINodeTagConst.C_MARS_TECHNOLOGY_TREE)
                                                {
                                                    GGUIWndMarsTechnologyTree.instance.focusTechnologyBySkillType(techSkillType);
                                                }
                                                else
                                                {
                                                    QueueMgr.instance.AddNode(new GNodeMarsTechnologyTree_ByTechnologySkillType(techSkillType));
                                                }
                                                return;
                                            }
                                            break;
                                    }
                                }
                            }
                        }
                        
                        if (QueueMgr.instance._lastNode.nodeTag == UINodeTagConst.C_MARS_TECHNOLOGY_TREE)
                        {
                            GGUIWndMarsTechnologyTree.instance.setSelectTab(technologyType, false, true);
                        }
                        else
                        {
                            QueueMgr.instance.AddNode(new GNodeMarsTechnologyTree(technologyType));
                        }
                    }
                    break;
                case ESysSceneType.MARS_EXPLORE:
                    GNodeMarsExplore.openOrQuitToNode(null);
                    break;
                case ESysSceneType.MARS_BUILDING_UPGRADE:
                {
                    if (_args is not { Count: > 0 } || !long.TryParse(_args[0], out long marsBuildingId))
                        break;

                    MarsBuildingInfo marsBuildingInfo = NPPlayer.instance.marsComp.buildingSubComponent.getBuildingInfoById(marsBuildingId);
                    if (marsBuildingInfo == null)
                    {
                        Debug.LogError($"跳转失败，ESysSceneType.MARS_BUILDING_UPGRADE 火星建筑id错误，未找到对应建筑，marsBuildingId:{marsBuildingId}");
                        break;
                    }

                    MarsUtil.jumpToBuildingUpgrade(marsBuildingInfo);
                    break;
                }
                case ESysSceneType.MARS_FIRST_EQUIPMENT_UPGRADE:
                {
                    MarsBuildingInfo marsBuildingInfo = NPPlayer.instance.marsComp.buildingSubComponent.getFirstEquipmentUpgradableBuilding();
                    if (marsBuildingInfo == null)
                    {
                        Debug.LogError($"跳转失败，ESysSceneType.MARS_FIRST_EQUIPMENT_UPGRADE 未找到可升级的火星建筑");
                        break;
                    }
                    
                    MarsUtil.jumpToBuildingUpgrade(marsBuildingInfo);
                    break;
                }
                case ESysSceneType.MARS_RESIDENT_REPLENISH:
                {
                    // 火星居民补充界面
                    EMarsResidentReplenishState residentReplenishState = MarsUtil.getMarsResidentReplenishState();
                    switch (residentReplenishState)
                    {
                        case EMarsResidentReplenishState.Idle:
                            // 打开补充窗口
                            QueueMgr.instance.addNode_InGame_SingleWnd(
                                GGUIWndMarsResidentReplenish.instance,
                                GGUIWndMarsResidentReplenish.instance.showWnd,
                                UINodeTagConst.C_MARS_RESIDENT_REPLENISH);
                            break;
                        
                        case EMarsResidentReplenishState.Replenishing:
                            Common.MarsObj.Mars_PeopleImmigrant immigrantInfo = NPPlayer.instance.marsComp.peopleSubComponent.getImmigrantInfo();
                            if (immigrantInfo != null)
                            {
                                long countDownMs = immigrantInfo.getEndMs() - FpsAndPingMgr.instance.serverTimeTag;//获取移民倒计时
                                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.mars_resident_replenishingTip_str, TimeUtil.millisecondsToTime_hms(countDownMs)));
                            }
                            break;
                        
                        case EMarsResidentReplenishState.ReplenishComplete:
                            // 打开结果窗口
                            GGUIWndMarsResidentReplenishResult.addWndNode();
                            break;
                        case EMarsResidentReplenishState.ReplenishCountLimit:
                            NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.mars_resident_replenishCountLimitTip_none));
                            break;
                    }
                    break;
                }
                case ESysSceneType.MARS_BUILDING_INFO:
                {
                    if (_args is not { Count: > 0 } || !long.TryParse(_args[0], out long marsBuildingId))
                        break;

                    MarsBuildingInfo marsBuildingInfo = NPPlayer.instance.marsComp.buildingSubComponent.getBuildingInfoById(marsBuildingId);
                    if (marsBuildingInfo == null)
                    {
                        Debug.LogError($"跳转失败，ESysSceneType.MARS_BUILDING_INFO 火星建筑id错误，未找到对应建筑，marsBuildingId:{marsBuildingId}");
                        break;
                    }

                    GGUIMonoMarsBuildingInfoPageTabType pageType = GGUIMonoMarsBuildingInfoPageTabType.Equipment;
                    if (_args is { Count: > 1 } && Enum.TryParse(_args[1], out GGUIMonoMarsBuildingInfoPageTabType type))
                        pageType = type;
                    
                    MarsUtil.jumpToBuildingInfo(marsBuildingInfo, pageType);
                    break;
                }
                case ESysSceneType.MARS_TECHNOLOGY_DETAIL:
                {
                    if (_args is not { Count: > 0 } || !long.TryParse(_args[0], out long technologyId))
                        break;

                    GGUIWndMasrTechnologyDetail.instance.setData(technologyId);
                    QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMasrTechnologyDetail.instance, GGUIWndMasrTechnologyDetail.instance.showWnd, UINodeTagConst.C_MARS_TECHNOLOGY_DETAIL);
                    break;
                }
                case ESysSceneType.MARS_BUILDING_SETTLE:
                {
                    MarsBuildingInfo marsBuildingInfo = null;
                    if (_args is { Count: > 0 } && long.TryParse(_args[0], out long marsBuildingId))
                    {
                        marsBuildingInfo = NPPlayer.instance.marsComp.buildingSubComponent.getBuildingInfoById(marsBuildingId);
                        if (!marsBuildingInfo.settleSlotData.isValid())
                        {
                            marsBuildingInfo = null;
                            Debug.LogError($"跳转失败，ESysSceneType.MARS_BUILDING_SETTLE 火星建筑id错误，未找到对应可入住建筑，marsBuildingId:{marsBuildingId}, 默认跳转到人数最少的可入住建筑");
                        }
                    }

                    marsBuildingInfo ??= NPPlayer.instance.marsComp.buildingSubComponent.getPeopleNumMinBuilding();
                    if (marsBuildingInfo == null)
                    {
                        NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.mars_noSettlableBuildingTip_none);
                        break;
                    }

                    MarsUtil.jumpToBuildingSettle(marsBuildingInfo);
                    break;
                }
                case ESysSceneType.MARS_CASH_GIFT_PACK:
                    EMarsCashGiftPackMainTabType marsCashGiftPackTabType = EMarsCashGiftPackMainTabType.NONE;
                    if (_args != null && _args.Count > 0)
                        ALCommon.TryEnumParse(typeof(EMarsCashGiftPackMainTabType), _args[0], out marsCashGiftPackTabType);
                    QueueMgr.instance.addNode_InGame_MainUIMainWnd(GGUIWndMarsCashGiftPackMain.instance, UINodeTagConst.C_MARS_CASH_GIFT_PACK_MAIN, () =>
                    {
                        GGUIWndMarsCashGiftPackMain.instance.setInfo(marsCashGiftPackTabType);
                    }, null, 0);
                    break;
                case ESysSceneType.COMMON_ACHIEVE:
                {
                    if (_args == null || _args.Count < 3)
                    {
                        Debug.LogError($"COMMON_ACHIEVE跳转失败，请检查参数，C_UI_TO_SYS_SCE:COMMON_ACHIEVE:EAchieveType:UIResId:isMainWnd");
                        break;
                    }
                    if (!ALCommon.TryEnumParse(typeof(EAchieveType), _args[0], out EAchieveType commonAchieveType))
                    {
                        Debug.LogError($"COMMON_ACHIEVE跳转失败，参数错误，achieveType:{_args[0]}");
                        break;
                    }
                    if (!long.TryParse(_args[1], out long uiResId))
                    {
                        Debug.LogError($"COMMON_ACHIEVE跳转失败，参数错误，uiResId:{_args[1]}");
                        break;
                    }
                    bool isMainWnd = _args[2] != null && _args[2].ToLower() == "true";

                    GGUIWndCommonAchieve commonAchieveWnd = new GGUIWndCommonAchieve(commonAchieveType, uiResId, isMainWnd);
                    if (isMainWnd)
                        QueueMgr.instance.addNode_InGame_MainUIMainWnd(commonAchieveWnd, UINodeTagConst.C_COMMON_ACHIEVE, null, commonAchieveWnd.showWnd, 0);
                    else
                        QueueMgr.instance.addNode_InGame_SingleWnd(commonAchieveWnd, commonAchieveWnd.showWnd, UINodeTagConst.C_COMMON_ACHIEVE);
                    break;
                }
                case ESysSceneType.CHAT_SHARE_CONSORT_CG:
                    QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndShareConsortCG.instance, GGUIWndShareConsortCG.instance.showWnd, UINodeTagConst.C_CHAT_SELECT_SHARE_CONSORT_CG);
                    break;
                case ESysSceneType.ACTIVITY_STEP_REWARD:
                    if (_args == null || _args.Count <= 0 || !long.TryParse(_args[0], out long activityId_5))
                    {
                        Debug.LogError($"跳转失败，ESysSceneType.ACTIVITY_STEP_REWARD 需要1个参数 activityId");
                        break;
                    }
                    QueueMgr.instance.addNode_InGame_MainUIMainWnd(GGUIWndActivityStepReward.instance, UINodeTagConst.C_ACTIVITY_STEP_REWARD, null,
                    () =>
                    {
                        GGUIWndActivityStepReward.instance.showWnd();
                        GGUIWndActivityStepReward.instance.setInfo(activityId_5);
                    }, 0);
                    break;

                case ESysSceneType.ACTIVITY_MULTIPLE_RANK:
                    if (_args == null || _args.Count <= 0 || !long.TryParse(_args[0], out long activityId_6))
                    {
                        Debug.LogError($"跳转失败，ESysSceneType.ACTIVITY_MULTIPLE_RANK 需要1个参数 activityId");
                        break;
                    }

                    ERankRushDetailTabType activityMultipleRankTargetDetailTab = ERankRushDetailTabType.REWARD;
                    bool multipleRankNeedShowGiftBtn = false;
                    if (_args != null && _args.Count > 1)
                        ALCommon.TryEnumParse(typeof(ERankRushDetailTabType), _args[1], out activityMultipleRankTargetDetailTab);
                    if (_args != null && _args.Count > 2)
                        multipleRankNeedShowGiftBtn = ALCommon.GetBool(_args[2]);

                    _ABaseActivityInfo activityMultipleRankActivityInfo = NPPlayer.instance.commonActivityComp.getValidActivityInfoByActivityId(activityId_6);
                    if (activityMultipleRankActivityInfo != null && activityMultipleRankActivityInfo.rankRushInfoList != null && activityMultipleRankActivityInfo.rankRushInfoList.Count > 0)
                        QueueMgr.instance.AddNode(new GNodeRankRushMultipleDetail(activityId_6, activityMultipleRankTargetDetailTab, multipleRankNeedShowGiftBtn));
                    else
                        Debug.LogError($"跳转失败，ESysSceneType.ACTIVITY_MULTIPLE_RANK 未找到对应活动，activityId:{activityId_6}");
                    break;
                case ESysSceneType.MARS_BUILDING:
                    if (_args == null || _args.Count < 4)
                    {
                        Debug.LogError($"跳转失败，ESysSceneType.MARS_BUILDING 需要4个参数");
                        break;
                    }

                    if (Enum.TryParse(_args[0], out EMarsBuildingType marsBuildingType) && 
                        Enum.TryParse(_args[1], out EMarsBuildingJumpType marsBuildingJumpType) &&
                        ALCommon.TryParseFloat(_args[2], out float marsBuildingMoveDuration) &&
                        bool.TryParse(_args[3], out bool showGuide))
                    {
                        void FocusToTarget()
                        {
                            if (!Game.instance.isInTutorial)
                            {
                                MarsBuildingInfo marsBuildingInfo = NPPlayer.instance.marsComp.buildingSubComponent.getBuildingInfoByType(marsBuildingType, marsBuildingJumpType);
                                if(marsBuildingInfo == null)
                                {
                                    Debug.LogError($"跳转失败，ESysSceneType.MARS_BUILDING 未找到对应建筑，marsBuildingType:{marsBuildingType}, marsBuildingJumpType:{marsBuildingJumpType}");
                                    return;
                                }
                                if (showGuide)
                                {
                                    MainAdditionMarsTDScene.instance.focusAndShowGuideHand(marsBuildingInfo.buildRefId, marsBuildingMoveDuration);
                                }
                                else
                                {
                                    MainAdditionMarsTDScene.instance.tryGetBuildingPos(marsBuildingInfo.buildRefId, out Vector3 targetPos);
                                    MainAdditionMarsTDScene.instance.focusToTarget(targetPos, marsBuildingMoveDuration);
                                }
                            }
                        }

                        if (QueueMgr.instance._lastNode is GNodeMars)
                            FocusToTarget();
                        else
                            GCommon.dealEnterMars(FocusToTarget);
                    }
                    else
                    {
                        Debug.LogError($"跳转失败，ESysSceneType.MARS_BUILDING 解析失败，{_args[0]}, {_args[1]}, {_args[2]}, {_args[3]}");
                    }

                    break;
                case ESysSceneType.RUSH_EXCHANGE:
                {
                    if(NPPlayer.instance.rushExchangeComp.rushExchangeState != ERushExchangeState.Lock)
                        QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndRushExchangeMain.instance, GGUIWndRushExchangeMain.instance.showWnd, UINodeTagConst.C_RUSH_EXCHANGE_MAIN);
                } break;
                case ESysSceneType.IMPROVE_WAY:
                    if (_args == null || _args.Count == 0)
                    {
                        Debug.LogError($"跳转失败，ESysSceneType.IMPROVE_WAY 需要1个参数");
                        break;
                    }
                    EImproveTargetType improveTargetType = EImproveTargetType.NONE;
                    ALCommon.TryEnumParse(typeof(EImproveTargetType), _args[0], out improveTargetType);
                    if (improveTargetType == EImproveTargetType.NONE)
                    {
                        Debug.LogError($"跳转失败，ESysSceneType.IMPROVE_WAY 参数类型错误");
                        return;
                    }
                    //打开界面
                    QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndImproveWay.instance, () =>
                    {
                        GGUIWndImproveWay.instance.showWnd();
                        GGUIWndImproveWay.instance.setInfo(improveTargetType);
                    }, UINodeTagConst.C_IMPROVE_WAY);
                    break;
                case ESysSceneType.LOVER_COLLECT:
                {
                    if (NPPlayer.instance.loverCollectComp.targetLoverId <= 0)
                        QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndLoverCollectSelect.instance, GGUIWndLoverCollectSelect.instance.showWnd, UINodeTagConst.C_LOVER_COLLECT_SELECT);
                    else
                        QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndLoverCollectRescue.instance, GGUIWndLoverCollectRescue.instance.showWnd, UINodeTagConst.C_LOVER_COLLECT_RESCUE);
                    
                    break;
                }
                case ESysSceneType.ARENA_CELEBRITY_LIST:
                    //先打开竞技场主界面
                    enterUIMainNodeShow(ESysSceneType.ARENA);
                    //如果不是正在谈判中，再打开名人榜
                    if(!NPPlayer.instance.arenaComp.isInBattle())
                        GGUIWndArenaMain.instance.showCelebrityList();
                    break;
                case ESysSceneType.BUILDING_EFFECT:
                    QueueMgr.instance.AddNode(new GNodeBuildingEffect());
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 判断系统是否解锁
        /// </summary>
        /// <param name="_sysSceneType">系统类型</param>
        /// <param name="_dealUnlockButConditionNotSatisfied">已解锁但条件不满足需要处理的回调</param>
        /// <param name="_showTip">是否需要展示未解锁提示</param>
        /// <returns></returns>
        public static bool uiMainNodeIsUnlock(ESysSceneType _sysSceneType, Action _dealUnlockButConditionNotSatisfied = null, bool _showTip = true)
        {
            //有配置才判断
            NPMainNodeToFunctionTypeRefObj refObj = GRefdataCoreMgr.instance.mainNodeToFunctionTypeRefCore.getRef((long)_sysSceneType);
            if (refObj != null)
            {
                //判断系统功能解锁
                if (!isFuncUnlock(refObj.function_type, _showTip))
                    return false;

                //不需要判断系统解锁或者系统已经解锁，但是条件不满足
                if (refObj.condition_info != null && !refObj.condition_info.IsEnable(null))
                {
                    //弹unlock_tip
                    if (!string.IsNullOrEmpty(refObj.unlock_tip) && _showTip)
                        NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(refObj.unlock_tip, refObj.unlock_tip_args));

                    _dealUnlockButConditionNotSatisfied?.Invoke();
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 跳转到私聊 test
        /// </summary>
        public static void jumpToPrivateChat(long _cid)
        {
            NPPrivateChatInfo chatInfo = NPPlayer.instance.chatComp.getPrivateChatInfo(_cid);
            if (null == chatInfo)
            {
                //添加私聊会话
                chatInfo = NPPlayer.instance.chatComp.createPrivateChat(_cid);
                NPPlayer.instance.chatComp.addChatInfo(chatInfo);
            }
            NPPlayer.instance.chatComp.accountSaver.removeChatRemoveTimeTag(chatInfo.chatInfoTag);
            // 会话加载完之后再处理跳转
            chatInfo.regLoadDoneDelegate(() =>
            {
                if (GGUIWndChat.instance.isShow)
                {
                    GGUIWndChat.instance.setShowData(chatInfo);
                    GGUIWndChat.instance.setCurPageType(EChatPageType.CHAT_INFO_LIST);
                }
                else
                {
                    QueueMgr.instance.AddNode(new NPGMainQueueChatMainNode(chatInfo, true));
                }
            });
        }

        /// <summary>
        /// 进入对话node
        /// </summary>
        /// <param name="_dialogId"></param>
        /// <param name="_doneAction">对话展示完成回调(这时对话Node还未退出)</param>
        /// <param name="_isMain"></param>
        /// <param name="_needAutoPlay"></param>
        /// <param name="_dealCloseDialog">关闭对话处理, 若非null, 则不会自动退出对话, 会调用这个方法, 应该由外部调用者关闭对话</param>
        public static void enterDialogueNode(long _dialogId, Action _doneAction, bool _isMain = true, bool _needAutoPlay = false, Action _dealCloseDialog = null)
        {
            if (_dialogId == 0)
            {
                _doneAction?.Invoke();
                return;
            }

            NPDialogueRefObj refObj = GRefdataCoreMgr.instance.dialogueMap.getRef(_dialogId);
            if (null == refObj)
            {
#if UNITY_EDITOR
                ALLog.Error($"对话 dialogueId：{_dialogId} 找不到对应配置");
#endif
                _doneAction?.Invoke();

                //发送对话结束消息
                ALCommonTaskController.CommonActionAddNextFrameTask(() =>
                    {
                        WinMsg.SendMsg(WinMsgType.DIALOG_END, _dialogId);

                        //发送mini游戏结束的引导消息
                        WinMsg.SendMsg(WinMsgType.CONTROL_TUROTIAL_SETP, ENPTutorialTriggerType.DIALOG_END);
                        
                        WinMsg.SendMsg(WinMsgType.MSG_NEXT_STEP_TRIGGER, ENextStepTriggerMsgType.DIALOG_END);
                    });

                return;
            }

            //触发引导检查效果，只有在notice全部关闭后才触发引导。
            QueueMgr.instance.AddNode(new NPGMainQueueDialogueNode(refObj, _doneAction, _needAutoPlay, _dealCloseDialog));
        }

        /// <summary>
        /// 进入对话node
        /// </summary>
        /// <param name="_dialogIdList"></param>
        /// <param name="_doneAction">对话展示完成回调(这时对话Node还未退出)</param>
        /// <param name="_needAutoPlay"></param>
        public static void enterDialogueNode(List<long> _dialogIdList, Action _doneAction, bool _needAutoPlay = false)
        {
            if (_dialogIdList == null || _dialogIdList.Count <= 0)
            {
                _doneAction?.Invoke();
                return;
            }

            ALProcess dialogShowProgress = ALProcess.CreateProcess();
            foreach (var dialogId in _dialogIdList)
            {
                dialogShowProgress.addDelegateProcess((_done) =>
                {
                    enterDialogueNode(dialogId, _done, true, _needAutoPlay);
                });
            }

            dialogShowProgress.addProcess(_doneAction);
            dialogShowProgress.deal();
        }
        
        /// <summary>
        /// 进入宴会
        /// </summary>
        /// <param name="_dinnerInfo"></param>
        public static void enterDinner(GDinnerInfo _dinnerInfo, Action _onDealDone = null, Action _onDealFail = null, Action _onEnterDone = null, Action _onQuitAction = null)
        {
            _dinnerInfo.regDetailInfo((info) =>
            {
                QueueMgr.instance.AddNode(new GNodeDinnerMain(info, _onEnterDone, _onQuitAction));
                _onDealDone?.Invoke();
            }, () =>
            {
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.dinner_is_over_tip));
                _onDealFail?.Invoke();
            });
        }
    }
}
