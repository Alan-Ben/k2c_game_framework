
using System;
using ALPackage;
using NPEnum;

namespace GOE
{
    //摄像头聚焦到当前存在的指定 entry_point C_Camera_Move_Focus_To_EntryPoint:entry_point_id:duration
    public class NPPlayerEffectC_Camera_Move_Focus_To_EntryPoint : _ANPPlayerEffectInfo
    {
        private long _m_entryPointId;
        private float _m_duration;
        private bool _m_isNeedShowHand;//是否展示手指

        /************
         * 效果类型
         **/
        public override ENPPlayerEffectType effectType { get { return ENPPlayerEffectType.C_CAMERA_MOVE_FOCUS_TO_ENTRYPOINT; } }

        public override void DealPlayerEffect(NPVarInfo _varVariableInfo)
        {
#if NP_GAME
            HomeEntryPointFollowInstance followInstance = GGUIWndHomeEntryFollow.instance.getInstance(_m_entryPointId);
            if (followInstance == null)
            {
                ALLog.Error("NPPlayerEffectC_Camera_Move_Focus_To_EntryPoint: 当前不存在指定的 entry_point id : " + _m_entryPointId);
                return;
            }

            if (GTDSceneMain.instance.curShowScene is not _ABasicAdditionMainTDScene focusableScene)
            {
                ALLog.Error("NPPlayerEffectC_Camera_Move_Focus_To_EntryPoint: 当前场景不支持相机移动");
                return;
            }

            focusableScene.focusToTarget(followInstance.itemWorldPos, _m_duration);

            if (_m_isNeedShowHand)
            {
                ALCommonActionMonoTask.addMonoTask(() =>
                {
                    //如果不在引导并且条件通过，展示入口手指引导
                    if (!Game.instance.isInTutorial && GRefdataCoreMgr.instance.npGeneral.entrance_show_hand_guide_cond.IsEnable(null))
                        WinMsg.SendMsg(WinMsgType.SET_SHOW_ENTRY_GUIDE_HAND, _m_entryPointId);
                }, _m_duration);
            }
#endif
        }

        public static NPPlayerEffectC_Camera_Move_Focus_To_EntryPoint readEffect(string _str)
        {
            string[] strs = _str.Split(':');
            if (strs.Length < 2)
            {
                UnityEngine.Debug.LogError("配置错误 - NPPlayerEffectC_Camera_Move_Focus_To_EntryPoint   example: C_Camera_Move_Focus_To_EntryPoint:entry_point_id:duration Error Str: " + _str);
                return null;
            }

            NPPlayerEffectC_Camera_Move_Focus_To_EntryPoint effectObj = new NPPlayerEffectC_Camera_Move_Focus_To_EntryPoint();

            try
            {
                effectObj._m_entryPointId = ALCommon.ParseLong(strs[0]);
                effectObj._m_duration = ALCommon.ParseFloat(strs[1]);
                if(strs.Length> 2)
                    effectObj._m_isNeedShowHand = ALCommon.GetBool(strs[2]);
                
                return effectObj;
            }
            catch (Exception)
            {
                UnityEngine.Debug.LogError("配置错误 - NPPlayerEffectC_Camera_Move_Focus_To_EntryPoint   example: C_Camera_Move_Focus_To_EntryPoint:entry_point_id:duration Error Str: " + _str);
                return null;
            }
        }
    }
}