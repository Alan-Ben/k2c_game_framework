
namespace GOE
{
    /// <summary>
    ///获得骑士弹窗
    /// </summary>
    public class NoticeDealer_GetHero : NPUINoticeMgr._ANPUINoticeDealer
    {
        private readonly HeroRefObj _m_heroRef;

        public NoticeDealer_GetHero(HeroRefObj _heroRef)
        {
            _m_heroRef = _heroRef;
        }
        
        public override bool canCurShow { get { return true; } }
        public override bool canPlayPriority { get { return false; } }
        public override bool isPriorityDealer { get { return false; } }
        public override bool needTransBk { get { return false; } }
        public override bool isNoticeFullScreen { get { return true; } }
        public override bool isOnlyUINode { get { return true; } }
        public override string nodeTag { get { return UINodeTagConst.C_HERO_GET; } }

        public override void dealShowNotice()
        {
            if (_m_heroRef == null)
            {
                setDealerDone();
                return;
            }

            GGUIWndHeroGet.instance.setInfo(_m_heroRef, setDealerDone);
            GUISceneMain.instance.showMainWnd(GGUIWndHeroGet.instance);
        }

        public override void dealHideNotice()
        {
            GGUIWndHeroGet.instance.hideWnd();
        }

        protected override void _onDealerDone()
        {
        }
    }
}
