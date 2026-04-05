using System;
using Common.ActivityObj;
using GOE;

namespace Hotfix
{
    public class HotfixDefaultActivityInfo : _ABaseActivityInfo
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