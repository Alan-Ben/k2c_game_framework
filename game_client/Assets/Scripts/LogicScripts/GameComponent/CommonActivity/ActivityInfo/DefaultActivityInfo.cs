using System;
using Common.ActivityObj;

namespace GOE
{
    /// <summary>
    /// 为了兼容旧数据的默认活动数据类，暂时没有任何逻辑，后续活动按子类模式做可以新增逻辑
    /// </summary>
    public class DefaultActivityInfo : _ABaseActivityInfo
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

        protected override void _onSubDataInit(Action _doneDelegate)
        {
            if (_doneDelegate != null) 
                _doneDelegate();        
        }

        protected override void _onDiscard()
        {
        }
    }
}