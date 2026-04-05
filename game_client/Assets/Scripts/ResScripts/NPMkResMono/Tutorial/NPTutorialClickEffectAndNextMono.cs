
using System;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 一个通用的乐园场景点击操作
    /// </summary>
    public class NPTutorialClickEffectAndNextMono : _AMonoOutCombatClick
    {
        [ALHeader("在发送引导到下一步之前执行的处理")]
        public string dealEffectString;

        protected override void _onClick()
        {
            //执行效果操作
#if NP_GAME
            List<NPPlayerEffectSerializeInfo> effectList = NPPlayerEffectSerializeInfo.readEffectList(dealEffectString);
            //执行效果
            NPPlayerEffectSerializeInfo.dealEffect(effectList, null);
#endif

            //发送引导到下一步的消息
            WinMsg.SendMsg(WinMsgType.CONTROL_TUROTIAL_SETP, ENPTutorialTriggerType.TURORIAL_NEXT);
        }

        protected override void _onHolding()
        {
        }

        protected override void _onPress()
        {
        }

        protected override void _onUnPress()
        {
        }
    }
}