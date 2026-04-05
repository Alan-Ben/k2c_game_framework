using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    public class GGUISubWndCommonPropertyDetail : _ATALBasicUISubWnd<GGUIMonoCommonPropertyDetail>
    {
        [ItemNotNull, NotNull] private readonly List<GGUISubWndCommonPropertyItem> _m_propertyItems;
        
        private JudgeUnionBonusPart[] _m_judgePartData;
        
        
        public GGUISubWndCommonPropertyDetail(GGUIMonoCommonPropertyDetail _wnd) 
            : base(_wnd)
        {
            _m_propertyItems = new List<GGUISubWndCommonPropertyItem>();
            initWnd();
        }
        

        protected override void _onShowWnd()
        {
            foreach (GGUISubWndCommonPropertyItem itemWnd in _m_propertyItems)
                itemWnd.showWnd();
        }
        protected override void _onHideWnd()
        {
            foreach (GGUISubWndCommonPropertyItem itemWnd in _m_propertyItems)
                itemWnd.hideWnd();
        }
        protected override void _onReset()
        {
            foreach (GGUISubWndCommonPropertyItem itemWnd in _m_propertyItems)
                itemWnd.resetWnd();
        }
        protected override void _onDiscard()
        {
            foreach (GGUISubWndCommonPropertyItem itemWnd in _m_propertyItems)
                itemWnd.discard();
            _m_propertyItems.Clear();
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.listPropertyItem != null)
            {
                foreach (GGUIMonoCommonPropertyItem item in wnd.listPropertyItem)
                {
                    if (item == null)
                        continue;
                    
                    _m_propertyItems.Add(new GGUISubWndCommonPropertyItem(item));
                }
            }
            
            refreshWnd();
        }


        public void refreshWnd(JudgeUnionBonusPart[] _judgeData)
        {
            _m_judgePartData = _judgeData;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null)
                return;
            
            foreach (GGUISubWndCommonPropertyItem itemWnd in _m_propertyItems)
                itemWnd.refreshWnd(_m_judgePartData);
        }
    }
}