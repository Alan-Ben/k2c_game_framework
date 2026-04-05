using System;
namespace GOE
{
    /// <summary>
    /// 简单的按钮状态
    /// </summary>
    public enum ENPCommonBtnState
    {
        ENABLE,
        DISABLE,
    }

    public class NPGGUIMonoCommonStateBtn : _ATNPGGUIMonoStateBtn<ENPCommonBtnState>
    {

        [ALHeader("消耗item")]
        public NPGGUIMonoCommonItem itemMono;

    }
}
