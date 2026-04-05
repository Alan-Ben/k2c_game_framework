namespace GOE
{
    /// <summary>
    /// 抽奖入口点
    /// </summary>
    public class GTDHomeEntryPointView_AvatarGacha : _AGTDHomeEntryPointView_Base<GTDHomeEntryPointMono_AvatarGacha, GGUIHomeEntryPointItemController_AvatarGacha, GGUIMonoHomeEntryPointItem_AvatarGacha, GGUIWndHomeEntryPointItem_AvatarGacha>
    {

        public GTDHomeEntryPointView_AvatarGacha(GTDHomeEntryPointMono_AvatarGacha _entryPointMono) : base(_entryPointMono)
        {
        }

        protected override void _onInit()
        {
        }

        protected override void _onDiscard()
        {

        }

        protected override void _onRefreshShow()
        {

        }

        protected override void _onHide()
        {

        }

        protected override void _onRefreshFollowControl(GGUIHomeEntryPointItemController_AvatarGacha _followControl)
        {
            if (null == _followControl)
                return;

            _followControl.setShowData(_m_entryPointRefObj);
        }
    }
}