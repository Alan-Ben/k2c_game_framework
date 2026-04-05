using System;

namespace GOE
{
    /// <summary>
    /// 测试的组队活动
    /// </summary>
    public class FirstTeamActivityInfo : _ABaseTeamActivityInfo
    {
        protected override void _onActivityStart()
        {
        }

        protected override void _onActivityEnd()
        {
        }

        protected override void _onActivityClosed()
        {
        }

        protected override void _onInit()
        {
        }

        protected override void _onDiscard()
        {
        }

        protected override void _onSubDataInitEx(Action _doneDelegate)
        {
            _doneDelegate?.Invoke();
        }
    }
}