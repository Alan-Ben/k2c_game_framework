using System;
using NPEnum;

namespace GOE
{
    public class NPPlayerEffectC_CLOSE_NODE : _ANPPlayerEffectInfo
    {
        private string _m_sTag;     //node标记
        public NPPlayerEffectC_CLOSE_NODE()
        {
            _m_sTag = string.Empty;
        }

        /************
         * 效果类型
         **/
        public override ENPPlayerEffectType effectType { get { return ENPPlayerEffectType.C_CLOSE_NODE; } }

        public override void DealPlayerEffect(NPVarInfo _varVariableInfo)
        {
#if NP_GAME
            if(string.IsNullOrEmpty(_m_sTag))
                QueueMgr.instance.DoUIRollBackByEsc();
            else
                QueueMgr.instance.forceCloseNodeByTag(_m_sTag);
#endif
        }

        public static NPPlayerEffectC_CLOSE_NODE readEffect(string _str)
        {
            NPPlayerEffectC_CLOSE_NODE effectObj = new NPPlayerEffectC_CLOSE_NODE();

            try
            {
                effectObj._m_sTag = _str;
                return effectObj;
            }
            catch (Exception)
            {
                UnityEngine.Debug.LogError("配置错误 - C_CLOSE_NODE   example: C_CLOSE_NODE(:nodeTag) Error Str: " + _str);
                return null;
            }
        }
    }
}

