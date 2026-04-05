using System;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 尝试触发推送礼包 C_TRY_TRIGGER_PUSH_GIFT_PACK:礼包组id
    /// </summary>
    public class NPPlayerEffectC_TryTriggerPushGiftPack : _ANPPlayerEffectInfo
    {
        private long _m_lGroupId; // 礼包组id

        public NPPlayerEffectC_TryTriggerPushGiftPack()
        {
        }

        /// <summary>
        /// 效果类型
        /// </summary>
        public override ENPPlayerEffectType effectType { get { return ENPPlayerEffectType.C_TRY_TRIGGER_PUSH_GIFT_PACK; } }

        public override void DealPlayerEffect(NPVarInfo _varVariableInfo)
        {
#if NP_GAME
            // 尝试触发推送礼包
            NPPlayer.instance.pushGiftComp.tryTriggerPushGiftPack(_m_lGroupId, false);
#endif
        }

        public static NPPlayerEffectC_TryTriggerPushGiftPack readEffect(string _str)
        {
            NPPlayerEffectC_TryTriggerPushGiftPack effectObj = new NPPlayerEffectC_TryTriggerPushGiftPack();

            // 拆分字符串后进行读取
            string[] strs = _str.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
            if (strs.Length < 1)
            {
                UnityEngine.Debug.LogWarning("NPPlayerEffectC_TryTriggerPushGiftPack 配置错误! C_TRY_TRIGGER_PUSH_GIFT_PACK  example: C_TRY_TRIGGER_PUSH_GIFT_PACK:礼包组id  error str:  " + _str);
                return null;
            }

            try
            {
                effectObj._m_lGroupId = long.Parse(strs[0]);

                return effectObj;
            }
            catch (Exception)
            {
                UnityEngine.Debug.LogWarning("NPPlayerEffectC_TryTriggerPushGiftPack 配置错误! C_TRY_TRIGGER_PUSH_GIFT_PACK  example: C_TRY_TRIGGER_PUSH_GIFT_PACK:礼包组id  error str:  " + _str);
                return null;
            }
        }
    }
}
