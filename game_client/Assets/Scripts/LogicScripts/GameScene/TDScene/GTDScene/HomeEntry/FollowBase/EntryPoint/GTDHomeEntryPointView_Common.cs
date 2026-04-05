using ALPackage;

namespace GOE
{
    //通用的功能入口点
    public class GTDHomeEntryPointView_Common : _AGTDHomeEntryPointView_Base<GTDHomeEntryPointMono_Common, GGUIHomeEntryPointItemController, GGUIMonoHomeEntryPointItem, GGUIWndHomeEntryPointItem>
    {
        public GTDHomeEntryPointView_Common(GTDHomeEntryPointMono_Common _entryPointMono) : base(_entryPointMono)
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

        protected override void _onRefreshFollowControl(GGUIHomeEntryPointItemController _followControl)
        {
            if(null == _followControl)
                return;
            
            _followControl.setShowData(_m_entryPointRefObj);
        }
    }
}