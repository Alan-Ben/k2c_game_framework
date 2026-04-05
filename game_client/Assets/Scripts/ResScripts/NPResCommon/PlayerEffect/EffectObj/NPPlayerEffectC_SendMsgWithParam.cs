using System;
using System.Collections.Generic;

using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 发送客户端自定义消息
    /// </summary>
    public class NPPlayerEffectC_SendMsgWithParam : _ANPPlayerEffectInfo
    {
        private WinMsgType _m_winMsgType;//窗口消息类型
        private string _m_param;//参数

        public NPPlayerEffectC_SendMsgWithParam()
        {
            _m_winMsgType = WinMsgType.NONE;
        }

        /************
         * 效果类型
         **/
        public override ENPPlayerEffectType effectType { get { return ENPPlayerEffectType.C_SEND_MSG_WITH_PARAM; } }

        public override void DealPlayerEffect(NPVarInfo _varVariableInfo)
        {
#if NP_GAME
            try
            {
                string[] paramArray = null;
                switch (_m_winMsgType)
                {
                    case WinMsgType.SIMULATE_CLICK_CONSORT_DETAIL_TAB:
                        paramArray = _m_param?.Split(':', StringSplitOptions.RemoveEmptyEntries);
                        if (paramArray == null || paramArray.Length < 1)
                        {
                            Debug.LogError($"配置错误 - C_SEND_MSG_WITH_PARAM:SIMULATE_CLICK_CONSORT_DETAIL_TAB:{_m_param}, 至少配置一个参数EUnLockConsortDetailWndTabType");
                            return;
                        }
                    
                        EUnLockConsortDetailWndTabType detailWndTabType = (EUnLockConsortDetailWndTabType)ALCommon.EnumParse(typeof(EUnLockConsortDetailWndTabType), paramArray[0], true);
                        switch (detailWndTabType)
                        {
                            case EUnLockConsortDetailWndTabType.INTERACTION:
                                if (paramArray.Length > 1)
                                {
                                    EUnlockConsortDetailWndInteractionPageTabType consortDetailWndInteractionPageTabType = 
                                        (EUnlockConsortDetailWndInteractionPageTabType)ALCommon.EnumParse(typeof(EUnlockConsortDetailWndInteractionPageTabType), paramArray[1], true);
                                    
                                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_CONSORT_DETAIL_TAB, detailWndTabType, consortDetailWndInteractionPageTabType);
                                }
                                else
                                {
                                    WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_CONSORT_DETAIL_TAB, detailWndTabType);
                                }
                                break;
                            
                            default:
                                WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_CONSORT_DETAIL_TAB, detailWndTabType);
                                break;
                        }
                        break;
                    
                    case WinMsgType.SIMULATE_CLICK_DIALOG_OPTION:
                        if (!long.TryParse(_m_param, out long _optionId))
                        {
                            Debug.LogError($"配置错误 - C_SEND_MSG_WITH_PARAM:SIMULATE_CLICK_DIALOG_OPTION:{_m_param}, 参数应该为对话选项的id");
                            return;
                        }
                        
                        WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_DIALOG_OPTION, _optionId);
                        break;
                    case WinMsgType.HERO_MAIN_SCROLL_MOVE_TO_HERO:
                        paramArray = _m_param?.Split(':', StringSplitOptions.RemoveEmptyEntries);
                        //解析, 必须有一个参数, 第一个参数为EHeroMainTargetHeroType
                        if (paramArray == null || paramArray.Length < 1 || string.IsNullOrEmpty(paramArray[0]) || 
                            !ALCommon.TryEnumParse(typeof(EHeroMainTargetHeroType), paramArray[0], out EHeroMainTargetHeroType targetHeroType))
                        {
                            Debug.LogError($"配置错误 - C_SEND_MSG_WITH_PARAM:HERO_MAIN_SCROLL_MOVE_TO_HERO:{_m_param}, 第一个参数应该为EHeroMainTargetHeroType");
                            return;
                        }
                        
                        // 第二个参数可选, 为float滚动时间
                        float moveTime = 0f;
                        if (paramArray.Length >= 2 && !string.IsNullOrEmpty(paramArray[1]))
                        {
                            if (!ALCommon.TryParseFloat(paramArray[1], out moveTime))
                            {
                                Debug.LogError($"配置错误 - C_SEND_MSG_WITH_PARAM:HERO_MAIN_SCROLL_MOVE_TO_HERO:{_m_param}, 第二个参数应该为float滚动时间");
                                return;
                            }
                        }
                        
                        WinMsg.SendMsg(WinMsgType.HERO_MAIN_SCROLL_MOVE_TO_HERO, targetHeroType, moveTime);
                        break;
                    
                    case WinMsgType.CONSORT_MAIN_SCROLL_MOVE_TO_CONSORT:
                        // 参数格式：EConsortMainTargetConsortType:类型自定义参数:滚动时间
                        // 示例：CAN_UPGRADE_BLESS_SKILL::0.25  或  SPECIFIED_ID:10001:0.25
                        paramArray = _m_param?.Split(':', 3);
                        //解析，必须有一个参数，第一个参数为 EConsortMainTargetConsortType
                        if (paramArray == null || paramArray.Length < 1 || string.IsNullOrEmpty(paramArray[0]) ||
                            !ALCommon.TryEnumParse(typeof(EConsortMainTargetConsortType), paramArray[0], out EConsortMainTargetConsortType targetConsortType))
                        {
                            Debug.LogError($"配置错误 - C_SEND_MSG_WITH_PARAM:CONSORT_MAIN_SCROLL_MOVE_TO_CONSORT:{_m_param}, 第一个参数应该为 EConsortMainTargetConsortType");
                            return;
                        }
                        // 第三个参数可选，为 float 滚动时间
                        float consortMoveTime = 0f;
                        if (paramArray.Length >= 3 && !string.IsNullOrEmpty(paramArray[2]))
                        {
                            if (!ALCommon.TryParseFloat(paramArray[2], out consortMoveTime))
                            {
                                Debug.LogError($"配置错误 - C_SEND_MSG_WITH_PARAM:CONSORT_MAIN_SCROLL_MOVE_TO_CONSORT:{_m_param}, 第三个参数应该为 float 滚动时间");
                                return;
                            }
                        }
                        // 第二个参数可选，为类型自定义参数字符串，由监听方自行解析
                        WinMsg.SendMsg(WinMsgType.CONSORT_MAIN_SCROLL_MOVE_TO_CONSORT, targetConsortType, paramArray[1], consortMoveTime);
                        break;
                    
                    case WinMsgType.SIMULATE_CLICK_HERO:
                        paramArray = _m_param?.Split(':', StringSplitOptions.RemoveEmptyEntries);
                        //解析, 必须有一个参数, 第一个参数为EHeroMainTargetHeroType
                        if (paramArray == null || paramArray.Length < 1 || string.IsNullOrEmpty(paramArray[0]) || 
                            !ALCommon.TryEnumParse(typeof(EHeroMainTargetHeroType), paramArray[0], out targetHeroType))
                        {
                            Debug.LogError($"配置错误 - C_SEND_MSG_WITH_PARAM:SIMULATE_CLICK_HERO:{_m_param}, 第一个参数应该为EHeroMainTargetHeroType");
                            return;
                        }
                        WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_HERO, targetHeroType);
                        
                        break;
                    
                    case WinMsgType.SIMULATE_CLICK_CONSORT:
                        paramArray = _m_param?.Split(':', StringSplitOptions.RemoveEmptyEntries);
                        //解析，必须有一个参数，第一个参数为 EConsortMainTargetConsortType
                        if (paramArray == null || paramArray.Length < 1 || string.IsNullOrEmpty(paramArray[0]) ||
                            !ALCommon.TryEnumParse(typeof(EConsortMainTargetConsortType), paramArray[0], out EConsortMainTargetConsortType simulateClickConsortType))
                        {
                            Debug.LogError($"配置错误 - C_SEND_MSG_WITH_PARAM:SIMULATE_CLICK_CONSORT:{_m_param}, 第一个参数应该为 EConsortMainTargetConsortType");
                            return;
                        }
                        // 第二个参数可选，为妃子ID字符串，仅当 SPECIFIED_ID 时有效，由监听方自行解析
                        WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_CONSORT, simulateClickConsortType, paramArray.Length >= 2 ? paramArray[1] : null);
                        break;
                    
                    case WinMsgType.SIMULATE_SWITCH_STEP_TASK_RUSH_RANK_TAB:
                        // 解析参数为EStepTaskRushRankActivityViewType
                        if (string.IsNullOrEmpty(_m_param) || 
                            !ALCommon.TryEnumParse(typeof(EStepTaskRushRankActivityViewType), _m_param, out EStepTaskRushRankActivityViewType viewType))
                        {
                            Debug.LogError($"配置错误 - C_SEND_MSG_WITH_PARAM:SIMULATE_SWITCH_STEP_TASK_RUSH_RANK_TAB:{_m_param}, 参数应该为EStepTaskRushRankActivityViewType");
                            return;
                        }
                        WinMsg.SendMsg(WinMsgType.SIMULATE_SWITCH_STEP_TASK_RUSH_RANK_TAB, viewType);
                        break;
                    case WinMsgType.BAG_ITEM_USE_HERO_GRID_SCROLL_MOVE_TO:
                        paramArray = _m_param?.Split(':', 2);
                        //解析, 必须有一个参数, 第一个参数为EHeroMainTargetHeroType
                        if (paramArray == null || paramArray.Length < 1 || string.IsNullOrEmpty(paramArray[0]) || 
                            !ALCommon.TryEnumParse(typeof(EGGUIMonoBagItemUseHeroGridTargetHeroType), paramArray[0], out EGGUIMonoBagItemUseHeroGridTargetHeroType _gGUIMonoBagItemUseHeroGridTargetHeroType))
                        {
                            Debug.LogError($"配置错误 - C_SEND_MSG_WITH_PARAM:BAG_ITEM_USE_HERO_GRID_SCROLL_MOVE_TO:{_m_param}, 第一个参数应该为EGGUIMonoBagItemUseHeroGridTargetHeroType");
                            return;
                        }
                        WinMsg.SendMsg(WinMsgType.BAG_ITEM_USE_HERO_GRID_SCROLL_MOVE_TO, paramArray[0], paramArray.Length >= 2 ? paramArray[1] : string.Empty);
                        break;
                    case WinMsgType.BAG_ITEM_USE_HERO_GRID_USE_FOR_HERO:
                        paramArray = _m_param?.Split(':', 2);
                        //解析, 必须有一个参数, 第一个参数为EHeroMainTargetHeroType
                        if (paramArray == null || paramArray.Length < 1 || string.IsNullOrEmpty(paramArray[0]) || 
                            !ALCommon.TryEnumParse(typeof(EGGUIMonoBagItemUseHeroGridTargetHeroType), paramArray[0], out EGGUIMonoBagItemUseHeroGridTargetHeroType __gGUIMonoBagItemUseHeroGridTargetHeroType))
                        {
                            Debug.LogError($"配置错误 - C_SEND_MSG_WITH_PARAM:BAG_ITEM_USE_HERO_GRID_USE_FOR_HERO:{_m_param}, 第一个参数应该为EGGUIMonoBagItemUseHeroGridTargetHeroType");
                            return;
                        }
                        WinMsg.SendMsg(WinMsgType.BAG_ITEM_USE_HERO_GRID_USE_FOR_HERO, paramArray[0], paramArray.Length >= 2 ? paramArray[1] : string.Empty);
                        break;
                    case WinMsgType.BAG_ITEM_GRID_SCROLL_MOVE_TO:
                        // 背包物品列表滚动到指定物品
                        // 参数格式: EGGUIMonoBagItemGridTargetItemType:附加参数
                        // 例如: ID:12345:Center:true (物品ID:滚动类型:是否平滑)
                        paramArray = _m_param?.Split(':', 2);
                        if (paramArray == null || paramArray.Length < 1 || string.IsNullOrEmpty(paramArray[0]) || 
                            !ALCommon.TryEnumParse(typeof(EGGUIMonoBagItemGridTargetItemType), paramArray[0], out EGGUIMonoBagItemGridTargetItemType _bagItemGridTargetItemType))
                        {
                            Debug.LogError($"配置错误 - C_SEND_MSG_WITH_PARAM:BAG_ITEM_GRID_SCROLL_MOVE_TO:{_m_param}, 第一个参数应该为EGGUIMonoBagItemGridTargetItemType");
                            return;
                        }
                        WinMsg.SendMsg(WinMsgType.BAG_ITEM_GRID_SCROLL_MOVE_TO, paramArray[0], paramArray.Length >= 2 ? paramArray[1] : string.Empty);
                        break;
                    case WinMsgType.SIMULATE_CLICK_BAG_ITEM_GRID_ITEM:
                        // 模拟点击背包物品
                        // 参数格式: EGGUIMonoBagItemGridTargetItemType:附加参数
                        // 例如: ITEM_ID:12345 (物品ID)
                        paramArray = _m_param?.Split(':', 2);
                        if (paramArray == null || paramArray.Length < 1 || string.IsNullOrEmpty(paramArray[0]) || 
                            !ALCommon.TryEnumParse(typeof(EGGUIMonoBagItemGridTargetItemType), paramArray[0], out EGGUIMonoBagItemGridTargetItemType _simulateClickBagItemTargetType))
                        {
                            Debug.LogError($"配置错误 - C_SEND_MSG_WITH_PARAM:SIMULATE_CLICK_BAG_ITEM_GRID_ITEM:{_m_param}, 第一个参数应该为EGGUIMonoBagItemGridTargetItemType");
                            return;
                        }
                        WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_BAG_ITEM_GRID_ITEM, paramArray[0], paramArray.Length >= 2 ? paramArray[1] : string.Empty);
                        break;
                    case WinMsgType.SIMULATE_CLICK_MARS_POPULAR_WILL_HELP_ITEM:
                        // 模拟点击火星民意中心求助列表项
                        WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_MARS_POPULAR_WILL_HELP_ITEM, _m_param);
                        break;
                    case WinMsgType.SIMULATE_TRIGGER_WND_SHAKE:
                        paramArray = _m_param?.Split(':', StringSplitOptions.RemoveEmptyEntries);
                        // 参数格式：震动强度:震动持续时间（不需要参数时请使用 C_SEND_MSG:SIMULATE_TRIGGER_WND_SHAKE）
                        if (paramArray == null || paramArray.Length < 2)
                        {
                            Debug.LogError($"配置错误 - C_SEND_MSG_WITH_PARAM:SIMULATE_TRIGGER_WND_SHAKE:{_m_param}, 参数应为 震动强度:震动持续时间，若不需要参数请使用 C_SEND_MSG:SIMULATE_TRIGGER_WND_SHAKE");
                            return;
                        }

                        if (!ALCommon.TryParseFloat(paramArray[0], out float shakeIntensity))
                        {
                            Debug.LogError($"配置错误 - C_SEND_MSG_WITH_PARAM:SIMULATE_TRIGGER_WND_SHAKE:{_m_param}, 第一个参数应该为float震动强度");
                            return;
                        }

                        if (!ALCommon.TryParseFloat(paramArray[1], out float shakeDuration))
                        {
                            Debug.LogError($"配置错误 - C_SEND_MSG_WITH_PARAM:SIMULATE_TRIGGER_WND_SHAKE:{_m_param}, 第二个参数应该为float震动持续时间");
                            return;
                        }

                        WinMsg.SendMsg(WinMsgType.SIMULATE_TRIGGER_WND_SHAKE, shakeIntensity, shakeDuration);
                        break;
                    case WinMsgType.SIMULATE_CLICK_CHANGE_ENTRANCE_CONSORT_SELECT_BY_INDEX:
                        // 参数格式：item下标（从0开始）
                        // 示例：C_SEND_MSG_WITH_PARAM:SIMULATE_CLICK_CHANGE_ENTRANCE_CONSORT_SELECT_BY_INDEX:1
                        if (!long.TryParse(_m_param, out long selectConsortIndex))
                        {
                            Debug.LogError($"配置错误 - C_SEND_MSG_WITH_PARAM:SIMULATE_CLICK_CHANGE_ENTRANCE_CONSORT_SELECT_BY_INDEX:{_m_param}, 参数应该为item下标(从0开始)");
                            return;
                        }
                        WinMsg.SendMsg(WinMsgType.SIMULATE_CLICK_CHANGE_ENTRANCE_CONSORT_SELECT_BY_INDEX, selectConsortIndex);
                        break;
                    case WinMsgType.SHOP_LIST_SCROLL_MOVE_TO_ITEM:
                        paramArray = _m_param?.Split(':', StringSplitOptions.RemoveEmptyEntries);
                        //解析, 必须有一个参数, 第一个参数为商品id
                        if (paramArray == null || paramArray.Length < 1 || string.IsNullOrEmpty(paramArray[0]))
                        {
                            Debug.LogError($"配置错误 - C_SEND_MSG_WITH_PARAM:SHOP_LIST_SCROLL_MOVE_TO_ITEM:{_m_param}, 第一个参数应该为商品id");
                            return;
                        }
                        long shopItemId = ALCommon.GetLong(paramArray[0]);

                        // 第二个参数可选, 为float滚动时间
                        float shopListMoveTime = 0f;
                        if (paramArray.Length >= 2 && !string.IsNullOrEmpty(paramArray[1]))
                        {
                            if (!ALCommon.TryParseFloat(paramArray[1], out shopListMoveTime))
                            {
                                Debug.LogError($"配置错误 - C_SEND_MSG_WITH_PARAM:SHOP_LIST_SCROLL_MOVE_TO_ITEM:{_m_param}, 第二个参数应该为float滚动时间");
                                return;
                            }
                        }
                        WinMsg.SendMsg(WinMsgType.SHOP_LIST_SCROLL_MOVE_TO_ITEM, shopItemId, shopListMoveTime);
                        break;
                    default:
                        WinMsg.SendMsg(_m_winMsgType, _m_param);
                        break;
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"配置错误 - C_SEND_MSG_WITH_PARAM:{_m_winMsgType}:{_m_param} Error: {e.Message}");
            }
#endif
        }

        public static NPPlayerEffectC_SendMsgWithParam readEffect(string _str)
        {
            string[] strs = _str.Split(':', 2);
            if (strs.Length < 1)
            {
                Debug.LogError("配置错误 - C_SEND_MSG_WITH_PARAM   example: C_SEND_MSG_WITH_PARAM:WinMsgType Error Str: " + _str);
                return null;
            }

            NPPlayerEffectC_SendMsgWithParam effectObj = new NPPlayerEffectC_SendMsgWithParam();

            try
            {
                effectObj._m_winMsgType = (WinMsgType)ALCommon.EnumParse(typeof(WinMsgType), strs[0], true);
                effectObj._m_param = strs[1];

                return effectObj;
            }
            catch (Exception)
            {
                Debug.LogError("配置错误 - C_SEND_MSG_WITH_PARAM   example: C_SEND_MSG_WITH_PARAM:WinMsgType Error Str: " + _str);
                return null;
            }
        }
    }
}

