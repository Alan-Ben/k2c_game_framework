using System;

namespace GOE
{
    /// <summary>
    /// 功能入口点的点击脚本
    /// </summary>
    public class GTDHomeEntryPointClickMono : _AMonoOutCombatClick_CommonTDAni
    {
        //点击事件
        public event Action onClick;
        
        public event Action onHolding;

        public event Action onPress;
        public event Action onUnPress;
        
        protected override void _onClickEx()
        {
            if (onClick != null) 
                onClick();
        }

        protected override void _onHolding()
        {
            if (onHolding != null)
                onHolding();
        }

        protected override void _onPressEx()
        {
            if (onPress != null)
                onPress();
        }

        protected override void _onUnPressEx()
        {
            if (onUnPress != null)
                onUnPress();
        }
        
    }
}