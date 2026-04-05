using System;
using ALPackage;
using CommonEnum;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 热更效果处理
    /// </summary>
    public class NPPlayerEffectC_HOTFIX_EFFECT : _ANPPlayerEffectInfo
    {
        public string effect_type;//效果类型
        public string effect_info_str;//效果参数

        public NPPlayerEffectC_HOTFIX_EFFECT()
        {

        }

        /************
         * 效果类型
         **/
        public override ENPPlayerEffectType effectType { get { return ENPPlayerEffectType.C_HOTFIX_EFFECT; } }

        public override void DealPlayerEffect(NPVarInfo _varVariableInfo)
        {
            try
            {
#if NP_GAME
                //调用热更的效果处理接口
                ALHotfixMgr_ILRuntime_Global.instance.dealStaticFunc("Hotfix.HotfixPlayerEffect", "dealPlayerEffect",_varVariableInfo, effect_type, effect_info_str);
#endif
            }
            catch(Exception _e)
            {
                UnityEngine.Debug.LogError("deal ENPPlayerEffectType.C_HOTFIX_EFFECT error" + _e.ToString());
            }
        }

        /// <summary>
        /// 主工程调用读取热更效果字符串
        /// </summary>
        /// <param name="_str"></param>
        /// <returns></returns>
        public static NPPlayerEffectC_HOTFIX_EFFECT readEffect(string _str)
        {
            NPPlayerEffectC_HOTFIX_EFFECT effectObj = new NPPlayerEffectC_HOTFIX_EFFECT();
            try
            {
                if (string.IsNullOrEmpty(_str))
                    return null;

                string[] strs = _str.Split(new char[] { ':' }, 2);

                if (strs.Length == 0)
                    return null;
                
                if (strs.Length > 0)
                    effectObj.effect_type = strs[0];
                if (strs.Length > 1)
                    effectObj.effect_info_str = strs[1];

                return effectObj;
            }
            catch (Exception)
            {
                UnityEngine.Debug.LogError($"效果配置错误 - NPPlayerEffectC_HOTFIX_EFFECT example: C_HOTFIX_EFFECT:HOTFIX_TYPE:PARAM， Error Str: C_HOTFIX_EFFECT:{_str}");
                return null;
            }
        }
    }
}