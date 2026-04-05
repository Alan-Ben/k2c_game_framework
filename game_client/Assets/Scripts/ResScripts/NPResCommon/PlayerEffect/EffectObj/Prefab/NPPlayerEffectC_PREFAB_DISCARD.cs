using System;
using NPEnum;

namespace GOE
{
    public class NPPlayerEffectC_PREFAB_DISCARD : _ANPPlayerEffectInfo
    {
        private string _m_sTag;     //注册的标记

        public NPPlayerEffectC_PREFAB_DISCARD()
        {
            _m_sTag = string.Empty;
        }

        /************
         * 效果类型
         **/
        public override ENPPlayerEffectType effectType { get { return ENPPlayerEffectType.C_PREFAB_DISCARD; } }

        public override void DealPlayerEffect(NPVarInfo _varVariableInfo)
        {
#if NP_GAME
            PrefabControllerMgr.instance.discardPrefab(_m_sTag);
#endif
        }

        public static NPPlayerEffectC_PREFAB_DISCARD readEffect(string _str)
        {
            string[] strs = _str.Split(':');
            if (strs.Length < 1)
            {
                UnityEngine.Debug.LogError("配置错误 - C_PREFAB_DISCARD   example: C_PREFAB_DISCARD:tag Error Str: " + _str);
                return null;
            }

            NPPlayerEffectC_PREFAB_DISCARD effectObj = new NPPlayerEffectC_PREFAB_DISCARD();

            try
            {
                effectObj._m_sTag = strs[0];
                return effectObj;
            }
            catch (Exception)
            {
                UnityEngine.Debug.LogError("配置错误 - C_PREFAB_DISCARD   example: C_PREFAB_DISCARD:tag Error Str: " + _str);
                return null;
            }
        }
    }
}

