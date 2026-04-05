
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using NPEnum;

namespace GOE
{
    public class GGUIWndMain : _ATALBasicUIWnd<GGUIMonoMain>
    {
        [NotNull] public static GGUIWndMain instance { get { return _g_instance ??= new GGUIWndMain(); } }
        private static GGUIWndMain _g_instance;
        
        
        [ItemNotNull, NotNull] private readonly List<GGUISubWndMainFunctionTab> _m_functionTabList;
        private EMainFunctionTabType _m_tabType;
        
        
        public GGUIWndMain() 
            : base(EALUIWndLayer.NORMAL)
        {
            _m_functionTabList = new List<GGUISubWndMainFunctionTab>();
        }
        
        
        protected override string _monoAssetPath { get { return GGUIMonoMain.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoMain.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        protected override void _onShowWnd()
        {
            foreach (GGUISubWndMainFunctionTab tab in _m_functionTabList)
                tab.showWnd();
        }
        protected override void _onHideWnd()
        {
            foreach (GGUISubWndMainFunctionTab tab in _m_functionTabList)
                tab.hideWnd();
        }
        protected override void _onReset()
        {
            foreach (GGUISubWndMainFunctionTab tab in _m_functionTabList)
                tab.resetWnd();
        }
        protected override void _onDiscard()
        {
            foreach (GGUISubWndMainFunctionTab tab in _m_functionTabList)
            {
                tab.onClick -= _onTabClick;
                tab.discard();
            }
            _m_functionTabList.Clear();
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.functionTabList != null)
            {
                foreach (GGUIMonoMainFunctionTab tabMono in wnd.functionTabList)
                {
                    GGUISubWndMainFunctionTab tabWnd = new GGUISubWndMainFunctionTab(tabMono);
                    tabWnd.onClick += _onTabClick;
                    _m_functionTabList.Add(tabWnd);
                }
            }

            refreshWnd();
        }


        public void refreshWnd(EMainFunctionTabType _selectType)
        {
            _m_tabType = _selectType;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null)
                return;

            foreach (GGUISubWndMainFunctionTab tab in _m_functionTabList)
            {
                tab.refreshWnd(tab.type == _m_tabType);
            }
        }


        private void _onTabClick(EMainFunctionTabType _type)
        {
            if(_m_tabType == _type)
                return;
            
            switch (_type)
            {
                case EMainFunctionTabType.BUILDING:
                    QueueMgr.instance.AddNode(new GNodeBuilding());
                    break;
                case EMainFunctionTabType.HERO:
                    QueueMgr.instance.AddNode(new GNodeHero());
                    break;
                case EMainFunctionTabType.CHAPTER:
                    QueueMgr.instance.AddNode(new GNodeChapterMap());
                    break;
                case EMainFunctionTabType.BAG:
                    QueueMgr.instance.AddNode(new GNodeBag());
                    break;
                case EMainFunctionTabType.SPACE_STATION:
                    QueueMgr.instance.AddNode(new GNodeSpaceStation());
                    break;
                case EMainFunctionTabType.MARS:
                    GCommon.dealEnterMars();
                    break;
            }
        }
    }
}