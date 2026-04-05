
using System;

namespace GOE
{
    public class GNodeAnecdoteBanner : _AGNodeWndWithCloseFunc<GGUIWndAnecdoteBannerShow>
    {
        private static GGUIWndAnecdoteBannerShow _createWnd(GGUIWndAnecdoteBannerShowType _type)
        {
            return _type switch 
            {
                GGUIWndAnecdoteBannerShowType.SPECIAL_EVENT => GGUIWndAnecdoteBannerShow.specialEventInstance,
                GGUIWndAnecdoteBannerShowType.TO_BE_CONTINUED => GGUIWndAnecdoteBannerShow.toBeContinuedInstance,
                GGUIWndAnecdoteBannerShowType.EVENT_ENDED => GGUIWndAnecdoteBannerShow.eventEndedInstance,
                _ => GGUIWndAnecdoteBannerShow.eventEndedInstance
            };
        }
        
        private GGUIWndAnecdoteBannerShowType _m_showType;
        
        public GNodeAnecdoteBanner(GGUIWndAnecdoteBannerShowType _type, Action _onNodeClose) 
            : base(_createWnd(_type), _onNodeClose, string.Empty, true)
        {
            _m_showType = _type;
        }
        

        /// <summary>
        /// 在进行回退处理的时候，队列系统是否能直接退出本节点
        /// </summary>
        public override bool IsCanRollBackQuit { get { return wnd is { isAnimating: false }; } }
        /// <summary>
        /// 本节点在进行前进跳转的时候是否需要自动删除
        /// </summary>
        public override bool NeedAutoRemove { get { return false; } }
        /// <summary>
        /// 在回退操作的时候，是否需要执行上一个节点的进入操作
        /// </summary>
        public override bool IsMainViewNode { get { return false; } }
        /// <summary>
        /// 当前节点是否还有效
        /// </summary>
        public override bool isEnable { get { return true; } }
        /// <summary>
        /// 本节点是否一个纯粹的UI节点，如果是则将关闭主摄像头
        /// </summary>
        public override bool isOnlyUINode { get { return false; } }

        protected override void _onCloseEx()
        {
            if (_m_showType == GGUIWndAnecdoteBannerShowType.EVENT_ENDED)
            {
                WinMsg.SendMsg(WinMsgType.CONTROL_TUROTIAL_SETP, ENPTutorialTriggerType.ANECDOTE_END_EFFECT_END);   
            }
        }
    }
}