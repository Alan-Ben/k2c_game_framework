using System;
using System.Collections.Generic;

using ALPackage;
using NPEnum;

#if NP_GAME
using GOE;
#endif

namespace GOE
{
    public class NPPlayerEffectC_Ani_Set_Int : _ANPPlayerEffectInfo
    {
        private string _m_sAnimtorName;     //处理的动作对象标记
        private string _m_sVariableName;    //变量名
        private int _m_bValue;             //值

        public NPPlayerEffectC_Ani_Set_Int()
        {
            _m_sAnimtorName = string.Empty;
            _m_sVariableName = string.Empty;
            _m_bValue = 0;
        }

        /************
         * 效果类型
         **/
        public override ENPPlayerEffectType effectType { get { return ENPPlayerEffectType.C_ANI_SET_INT; } }

        public override void DealPlayerEffect(NPVarInfo _varVariableInfo)
        {
#if NP_GAME
            AnimatorControllerMgr.instance.setInteger(_m_sAnimtorName, _m_sVariableName, _m_bValue);
#endif
        }

        public static NPPlayerEffectC_Ani_Set_Int readEffect(string _str)
        {
            string[] strs = _str.Split(':');
            if (strs.Length < 3)
            {
                UnityEngine.Debug.LogError("配置错误 - C_ANI_SET_INT   example: enum:animatorObjName:variableName:value Error Str: " + _str);
                return null;
            }

            NPPlayerEffectC_Ani_Set_Int effectObj = new NPPlayerEffectC_Ani_Set_Int();

            try
            {
                effectObj._m_sAnimtorName = strs[0];
                effectObj._m_sVariableName = strs[1];
                if (!int.TryParse(strs[2], out effectObj._m_bValue))
                {
                    UnityEngine.Debug.LogError($"Read C_ANI_SET_INT Value Fail! value:{strs[2]}");
                }

                return effectObj;
            }
            catch (Exception)
            {
                UnityEngine.Debug.LogError("配置错误 - C_ANI_SET_INT   example: enum:animatorObjName:variableName:value Error Str: " + _str);
                return null;
            }
        }
    }
}
