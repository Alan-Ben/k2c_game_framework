using System;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 具有放点击长按动画的TDMono点击基类
    /// 参考UI的Button动画功能
    /// </summary>
    public abstract class _AMonoOutCombatClick_CommonTDAni : _AMonoOutCombatClick
    {
        private int normalTriggerId = Animator.StringToHash("Normal");
        private int pressedTriggerId = Animator.StringToHash("Pressed");
        private int DisabledTriggerId = Animator.StringToHash("Disabled");

        [ALInfo("类似UI Button 目前一共三个Trigger：\n Normal 正常状态 \n Pressed 按下状态 \n Disabled 无效状态")]
        [ALHeader("动画状态机")]
        public Animator showAnimator;
        
        //按下按钮的时候的处理
        protected sealed override void _onPress()
        {
		    if(null != showAnimator)
            {
                showAnimator.SetTrigger(pressedTriggerId);
            }

            _onPressEx();
        }
        
        //弹起按钮的时候的处理
        protected sealed override void _onUnPress()
        {
            if(null != showAnimator)
            {
                showAnimator.SetTrigger(normalTriggerId);
            }
            
            _onUnPressEx();
        }

        protected sealed override void _onClick()
        {
            if(null != showAnimator)
            {
                showAnimator.SetTrigger(normalTriggerId);
            }

            _onClickEx();
        }

        //隐藏时候设置回默认状态
        private void OnDisable()
        {
            if(null == showAnimator)
                return;
            
            showAnimator.SetTrigger(DisabledTriggerId);
        }
        
        //按下按钮的时候的处理
        protected abstract void _onPressEx();
	    
        //弹起按钮的时候的处理
        protected abstract void _onUnPressEx();
        
        //点击时候处理
        protected abstract void _onClickEx();
    }
}