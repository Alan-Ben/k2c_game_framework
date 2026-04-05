using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPEnum;

namespace GOE
{
    [System.Serializable]
    public abstract class _ANPPlayerEffectInfo
    {
        /************
         * 效果类型
         **/
        public abstract ENPPlayerEffectType effectType { get; }

        public abstract void DealPlayerEffect(NPVarInfo _varVariableInfo);

        /***********************
         * 获取本对象的实际条件数据对象
         */
        public static _ANPPlayerEffectInfo readEffectInfo(ENPPlayerEffectType _effectType, string _infoStr)
        {
            switch (_effectType)
            {
                case ENPPlayerEffectType.C_UI_TO_SYS_SCE:
                    return NPPlayerEffectC_UI_TO_SYS_SCE.readEffect(_infoStr);
                case ENPPlayerEffectType.C_UI_SHOW_CUST_WND://显示自定义窗口
                    return NPPlayerEffectC_UI_SHOW_CUST_WND.readEffect(_infoStr);
                case ENPPlayerEffectType.C_USE_BAG_ITEM:
                    return NPPlayerEffectCUseBagItem.readEffect(_infoStr);
                case ENPPlayerEffectType.C_DEAL_ID:
                    return NPPlayerEffectC_Deal_Id.readEffect(_infoStr);
                case ENPPlayerEffectType.C_SHOW_ERR:
                    return NPPlayerEffectC_Show_Err.readEffect(_infoStr);

                case ENPPlayerEffectType.C_ANI_PLAY:
                    return NPPlayerEffectC_Ani_Play.readEffect(_infoStr);
                case ENPPlayerEffectType.C_ANI_SET_ANI:
                    return NPPlayerEffectC_Ani_Set_Ani.readEffect(_infoStr);
                case ENPPlayerEffectType.C_ANI_SET_BOOL:
                    return NPPlayerEffectC_Ani_Set_Bool.readEffect(_infoStr);
                case ENPPlayerEffectType.C_ANI_SET_FLOAT:
                    return NPPlayerEffectC_Ani_Set_Float.readEffect(_infoStr);
                case ENPPlayerEffectType.C_ANI_SET_INT:
                    return NPPlayerEffectC_Ani_Set_Int.readEffect(_infoStr);

                case ENPPlayerEffectType.C_ANI_G_PLAY:
                    return NPPlayerEffectC_Ani_G_Play.readEffect(_infoStr);
                case ENPPlayerEffectType.C_ANI_G_SET_ANI:
                    return NPPlayerEffectC_Ani_G_Set_Ani.readEffect(_infoStr);
                case ENPPlayerEffectType.C_ANI_G_SET_BOOL:
                    return NPPlayerEffectC_Ani_G_Set_Bool.readEffect(_infoStr);
                case ENPPlayerEffectType.C_ANI_G_SET_FLOAT:
                    return NPPlayerEffectC_Ani_G_Set_Float.readEffect(_infoStr);
                case ENPPlayerEffectType.C_ANI_G_SET_INT:
                    return NPPlayerEffectC_Ani_G_Set_Int.readEffect(_infoStr);
                case ENPPlayerEffectType.C_SPECIAL:
                    return NPPlayerEffectCSpecial.readEffect(_infoStr);
                case ENPPlayerEffectType.C_SHOWCASE_LOAD:
                    return NPPlayerEffectC_Showcase_Load.readEffect(_infoStr);
                case ENPPlayerEffectType.C_SHOWCASE_PLAY_SFX:
                    return NPPlayerEffectC_Showcase_Play_Sfx.readEffect(_infoStr);
                case ENPPlayerEffectType.C_SHOWCASE_GRAY:
                    return NPPlayerEffectC_Showcase_Gray.readEffect(_infoStr);
                case ENPPlayerEffectType.C_SPECIAL_DIALOGUE:
                    return NPPlayerEffectCSpecialDialogue.readEffect(_infoStr);
                
                case ENPPlayerEffectType.C_SEND_MSG:
                    return NPPlayerEffectC_SendMsg.readEffect(_infoStr);
                case ENPPlayerEffectType.C_SEND_MSG_WITH_PARAM:
                    return NPPlayerEffectC_SendMsgWithParam.readEffect(_infoStr);
                case ENPPlayerEffectType.C_CAMERA_MOVE_FOCUS_TO:
                    return NPPlayerEffectC_Camera_Move_Focus_To.readEffect(_infoStr);
                case ENPPlayerEffectType.C_CAMERA_MOVE_TO:
                    return NPPlayerEffectC_Camera_Move_To.readEffect(_infoStr);
                case ENPPlayerEffectType.C_CAMERA_MOVE_FOCUS_TO_ENTRYPOINT:
                    return NPPlayerEffectC_Camera_Move_Focus_To_EntryPoint.readEffect(_infoStr);
                case ENPPlayerEffectType.C_GOTO_EFFECT:
                    return NPPlayerEffectC_GOTO_EFFECT.readEffect(_infoStr);
                case ENPPlayerEffectType.C_DEAL_STRING:
                    return NPPlayerEffectC_Deal_String.readEffect(_infoStr);
                case ENPPlayerEffectType.C_SPECIAL_SEND_EVENT:
                    return NPPlayerEffectCSpecialSendEvent.readEffect(_infoStr);

                case ENPPlayerEffectType.C_PREFAB_LOAD:
                    return NPPlayerEffectC_PREFAB_LOAD.readEffect(_infoStr);
                case ENPPlayerEffectType.C_CLOSE_NODE:
                    return NPPlayerEffectC_CLOSE_NODE.readEffect(_infoStr);
                case ENPPlayerEffectType.C_SHOWCASE_LOAD_SPEC:
                    return NPPlayerEffectC_Showcase_Load_Spec.readEffect(_infoStr);
                case ENPPlayerEffectType.C_SHOWCASE_MAGICA_CLOTH:
                    return NPPlayerEffectC_Showcase_MaigicCloth.readEffect(_infoStr);
                case ENPPlayerEffectType.C_SHOW_BATCH_USE_ITEM_WND:
                    return NPPlayerEffectC_Show_Batch_Use_Item_Wnd.readEffect(_infoStr);
                case ENPPlayerEffectType.C_PLAY_FUNC_UNLOCK_PROCESS:
                    return NPPlayerEffectC_PLAY_FUNC_UNLOCK_PROCESS.readEffect(_infoStr);
                case ENPPlayerEffectType.C_REFRESH_ENTRY_STATE:
                    return NPPlayerEffectC_REFRESH_ENTRY_STATE.readEffect(_infoStr);
                case ENPPlayerEffectType.C_PREFAB_DISCARD:
                    return NPPlayerEffectC_PREFAB_DISCARD.readEffect(_infoStr);
                case ENPPlayerEffectType.C_PLAY_DIALOG_AUDIO:
                    return NPPlayerEffectC_PLAY_DIALOG_AUDIO.readEffect(_infoStr);
                case ENPPlayerEffectType.C_HOTFIX_EFFECT:
                    return NPPlayerEffectC_HOTFIX_EFFECT.readEffect(_infoStr);
                case ENPPlayerEffectType.C_TRY_TRIGGER_PUSH_GIFT_PACK:
                    return NPPlayerEffectC_TryTriggerPushGiftPack.readEffect(_infoStr);
                case ENPPlayerEffectType.C_PLAY_AUDIO:
                    return NPPlayerEffectC_PLAY_AUDIO.readEffect(_infoStr);
                default:
                    return null;
            }
        }
    }
}

