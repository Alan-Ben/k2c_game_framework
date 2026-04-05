using System;

namespace GOE
{
    /// <summary>
    /// 功能入口点的点击脚本
    /// </summary>
    public class GTDCommonPosClickMono : _AMonoOutCombatClick_CommonTDAni
    {
        //点击事件
        public event Action onClick;
        
        protected override void _onClickEx()
        {
            if (onClick != null) 
                onClick();
        }

        protected override void _onHolding()
        {
            
        }

        protected override void _onPressEx()
        {
            
        }

        protected override void _onUnPressEx()
        {
            
        }
    }
}