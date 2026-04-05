
using System;

namespace GOE
{
    /// <summary>
    /// 一个通用的乐园场景点击操作
    /// </summary>
    public class NPTutorialClickNextMono : _AMonoOutCombatClick
    {
        protected override void _onClick()
        {
            //直接发送消息
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