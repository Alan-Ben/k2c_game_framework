using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUISubWndSevenDayGoalsDayContentPageTabList : GGUISubWndCommonPageTabList<GGUIMonoSevenDayGoalsDayContentTabType, GGUIMonoSevenDayGoalsDayContentPageTabList, GGUIMonoSevenDayGoalsDayContentPageTabListItem>
    {
        private int _m_day;
        
        
        public GGUISubWndSevenDayGoalsDayContentPageTabList(GGUIMonoSevenDayGoalsDayContentPageTabList _wnd) 
            : base(_wnd)
        {
            initWnd();
        }
        
        
        protected override _AALBasicLoadUIWndBasicClass _createPageWnd(GGUIMonoSevenDayGoalsDayContentTabType _type, Transform _wndParent)
        {
            switch (_type)
            {
                case GGUIMonoSevenDayGoalsDayContentTabType.Shop:
                {
                    GGUIPrefabSubWndSevenDayGoalsDayContentPageShop pageWnd = new GGUIPrefabSubWndSevenDayGoalsDayContentPageShop(_wndParent);
                    pageWnd.refreshWnd(_m_day, true);
                    return pageWnd;
                }
                case GGUIMonoSevenDayGoalsDayContentTabType.Task:
                {
                    GGUIPrefabSubWndSevenDayGoalsDayContentPageTask pageWnd = new GGUIPrefabSubWndSevenDayGoalsDayContentPageTask(_wndParent);
                    pageWnd.refreshWnd(_m_day, true);
                    return pageWnd;
                }
                default:
                    return null;
            }
        }

        /// <summary>
        /// 点击选中页签之后事件
        /// </summary>
        /// <param name="_type"></param>
        protected override void _onAfterClickSelectTab(GGUIMonoSevenDayGoalsDayContentTabType _type)
        {
            //设置礼包红点已读
            _setRedTipRead();
        }

        public void refreshWnd(int _day, bool _reset)
        {
            _m_day = _day;
            refreshWnd(_reset);
        }
        public void refreshWnd(bool _reset = false)
        {
            actionForAllLoadedPageWndSync(_wnd =>
            {
                switch (_wnd)
                {
                    case GGUIPrefabSubWndSevenDayGoalsDayContentPageShop shopPage:
                        shopPage.refreshWnd(_m_day, _reset);
                        break;
                    case GGUIPrefabSubWndSevenDayGoalsDayContentPageTask taskPage:
                        taskPage.refreshWnd(_m_day, _reset);
                        break;
                }
            });

            //刷新红点
            refreshRedTip();
            //设置礼包红点已读
            _setRedTipRead();
        }
        /// <summary>
        /// 刷新红点
        /// </summary>
        public void refreshRedTip()
        {
            setTabRedTipShow(GGUIMonoSevenDayGoalsDayContentTabType.Shop, getCurSelectType() == GGUIMonoSevenDayGoalsDayContentTabType.Shop ? false : NPPlayer.instance.sevenDayGoalsComp.getNeedShowRedTip(_m_day, ESevenDayGoalsRedTipType.GIFT));
            setTabRedTipShow(GGUIMonoSevenDayGoalsDayContentTabType.Task, NPPlayer.instance.sevenDayGoalsComp.getNeedShowRedTip(_m_day, ESevenDayGoalsRedTipType.TASK));
        }

        //设置礼包红点已读
        private void _setRedTipRead()
        {
            if (getCurSelectType() == GGUIMonoSevenDayGoalsDayContentTabType.Shop)
                NPPlayer.instance.sevenDayGoalsComp.setReadRedTip(_m_day, ESevenDayGoalsRedTipType.GIFT);
        }
    }
}