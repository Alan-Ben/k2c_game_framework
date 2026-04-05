using ALPackage;

namespace GOE
{
    /// <summary>
    /// 我的宴会入口点
    /// </summary>
    public class GTDHomeEntryPointView_SelfDinner : _AGTDHomeEntryPointView_Base<GTDHomeEntryPointMono_SelfDinner, GGUIHomeEntryPointItemController_SelfDinner, GGUIMonoHomeEntryPointItem_SelfDinner, GGUIWndHomeEntryPointItem_SelfDinner>
    {

        public GTDHomeEntryPointView_SelfDinner(GTDHomeEntryPointMono_SelfDinner _entryPointMono) : base(_entryPointMono)
        {
        }

        protected override void _onInit()
        {
            WinMsg.RegisterMsgAct(WinMsgType.ON_DINNER_CHG, _onRefreshShow);
        }

        protected override void _onDiscard()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.ON_DINNER_CHG, _onRefreshShow);
        }

        protected override void _onRefreshShow()
        {

            if(null == _m_entryPointMono)
                return;
            if (NPPlayer.instance.dinnerComp.dinnerInstanceId > 0)
            {
                _playAni(_m_entryPointMono.hasSelfDinnerAniName);
            }
            else
            {
                _playAni(_m_entryPointMono.normalAniName);
            }
        }
        
        private void _playAni(string _aniName)
        {
            if (null == _m_entryPointMono || null == _m_entryPointMono.pointAnimation)
                return;
            _m_entryPointMono.pointAnimation.Play(_aniName);
        }

        protected override void _onHide()
        {

        }

        protected override void _onRefreshFollowControl(GGUIHomeEntryPointItemController_SelfDinner _followControl)
        {
            if (null == _followControl)
                return;

            _followControl.setShowData(_m_entryPointRefObj);
        }
    }
}