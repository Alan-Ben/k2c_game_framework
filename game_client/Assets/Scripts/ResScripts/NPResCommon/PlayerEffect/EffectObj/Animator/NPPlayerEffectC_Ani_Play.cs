using System;
using System.Collections.Generic;

using ALPackage;
using NPEnum;

#if NP_GAME
using GOE;
#endif

namespace GOE
{
    public class NPPlayerEffectC_Ani_Play : _ANPPlayerEffectInfo
    {
        private string _m_sAnimtorName;     //处理的动作对象标记
        private string _m_sPlayAni;         //播放的动作名称
        private float _m_sNormalizedTime;   //播放的开始比例

        public NPPlayerEffectC_Ani_Play()
        {
            _m_sAnimtorName = string.Empty;
            _m_sPlayAni = string.Empty;
            _m_sNormalizedTime = 0;
        }

        /************
         * 效果类型
         **/
        public override ENPPlayerEffectType effectType { get { return ENPPlayerEffectType.C_ANI_PLAY; } }

        public override void DealPlayerEffect(NPVarInfo _varVariableInfo)
        {
#if NP_GAME
            AnimatorControllerMgr.instance.playAnimator(_m_sAnimtorName, _m_sPlayAni, _m_sNormalizedTime);
#endif
        }

        public static NPPlayerEffectC_Ani_Play readEffect(string _str)
        {
            string[] strs = _str.Split(':');
            if (strs.Length < 2)
            {
                UnityEngine.Debug.LogError("配置错误 - C_ANI_PLAY   example: enum:animatorObjName:animatorStateName(:normalizedTime) Error Str: " + _str);
                return null;
            }

            NPPlayerEffectC_Ani_Play effectObj = new NPPlayerEffectC_Ani_Play();

            try
            {
                effectObj._m_sAnimtorName = strs[0];
                effectObj._m_sPlayAni = strs[1];

                if (strs.Length == 3)
                    effectObj._m_sNormalizedTime = ALCommon.ParseFloat(strs[2]);
                else
                    effectObj._m_sNormalizedTime = 0;

                return effectObj;
            }
            catch (Exception)
            {
                UnityEngine.Debug.LogError("配置错误 - C_ANI_PLAY   example: enum:animatorObjName:animatorStateName(:normalizedTime) Error Str: " + _str);
                return null;
            }
        }
    }
}

