using ALPackage;

namespace GOE
{
    /// <summary>
    /// 招募入口点
    /// </summary>
    public class GTDHomeEntryPointView_Recruit : _AGTDHomeEntryPointView_Base<GTDHomeEntryPointMono_Recruit, GGUIHomeEntryPointItemController_Recruit, GGUIMonoHomeEntryPointItem_Recruit, GGUIWndHomeEntryPointItem_Recruit>
    {

        public GTDHomeEntryPointView_Recruit(GTDHomeEntryPointMono_Recruit _entryPointMono) : base(_entryPointMono)
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

            if(null == _m_entryPointMono)
                return;
            
            
        }

        protected override void _onHide()
        {

        }

        protected override void _onRefreshFollowControl(GGUIHomeEntryPointItemController_Recruit _followControl)
        {
            if (null == _followControl)
                return;

            _followControl.setShowData(_m_entryPointRefObj);
        }
    }
}