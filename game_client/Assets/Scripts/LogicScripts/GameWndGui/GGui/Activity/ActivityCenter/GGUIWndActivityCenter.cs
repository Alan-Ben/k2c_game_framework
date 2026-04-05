using System.Collections.Generic;
using ALPackage;
using DG.Tweening;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 活动中心界面
    /// </summary>
    public partial class GGUIWndActivityCenter : _ANPGGUIBasicWnd<GGUIMonoActivityCenter>
    {
        private static GGUIWndActivityCenter _g_instance;
        public static GGUIWndActivityCenter instance
        {
            get
            {
                if(_g_instance == null)
                    _g_instance = new GGUIWndActivityCenter();
                return _g_instance;
            }
        }

        //配置列表
        private List<ActivityCenterInfo> _m_lActivityCenterInfoList;
        //页签列表
        private GGUIWndActivityCenterTabContainer _m_wTabContainer;
        //当前选中的页签配置
        private ActivityCenterRefObj _m_curSelectRef;
        //显示序列
        private long _m_lShowSerialize;
        //是否正在刷新列表显隐
        private bool _m_bIsDealingRefreshList;

        public GGUIWndActivityCenter() : base(EALUIWndLayer.NORMAL)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoActivityCenter.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoActivityCenter.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        protected override bool isShowAniPlayOnlyOne { get { return true; } }
        public override bool needDiscardOnSwitch { get { return true; } }

        protected override void _onShowWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
            if (_m_lActivityCenterInfoList != null)
            {
                for (int i = 0; i < _m_lActivityCenterInfoList.Count; i++)
                {
                    _m_lActivityCenterInfoList[i]?.regMsg();
                }
            }
            _refreshTabList();
        }

        protected override void _onHideWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
            if (_m_lActivityCenterInfoList != null)
            {
                for (int i = 0; i < _m_lActivityCenterInfoList.Count; i++)
                {
                    _m_lActivityCenterInfoList[i]?.unRegMsg();
                }
            }
            _m_wTabContainer?.hideWnd();
            _m_bIsDealingRefreshList = false;

            _m_curSelectRef = null;

            _hideAllPage();
        }

        protected override void _onReset()
        {
            _m_wTabContainer?.resetWnd();

            foreach (KeyValuePair<long, _AALBasicLoadUIWndBasicClass> pair in _m_dActivityCenterWndDic)
            {
                pair.Value?.resetWnd();
            }
        }

        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            _m_wTabContainer?.discard();
            _m_wTabContainer = null;

            foreach (KeyValuePair<long, _AALBasicLoadUIWndBasicClass> pair in _m_dActivityCenterWndDic)
            {
                pair.Value?.discard();
            }
            _m_dActivityCenterWndDic.Clear();

            _m_lActivityCenterInfoList?.Clear();
            _m_lActivityCenterInfoList = null;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoTabContainer != null)
            {
                _m_wTabContainer = new GGUIWndActivityCenterTabContainer(wnd.monoTabContainer);
                _m_wTabContainer.onClickItem += _onClickItem;
            }

            _m_lActivityCenterInfoList = new List<ActivityCenterInfo>();
            GRefdataCoreMgr.instance.activityCenterRefCore.dealAllRef(_ref =>
            {
                if (_ref != null)
                {
                    ActivityCenterInfo temp = new ActivityCenterInfo(_ref);
                    temp.onRecMsg = _onActivityCenterRecMsg;
                    _m_lActivityCenterInfoList.Add(temp);
                }
            });

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }

        /// <summary>
        /// 设置选中页签
        /// </summary>
        /// <param name="_ref"></param>
        public void setSelectTab(long _activityCenterId)
        {
            _m_wTabContainer?.setSelectItem(_activityCenterId);
        }

        //刷新页签列表
        private void _refreshTabList()
        {
            if (wnd == null)
                return;

            List<ActivityCenterRefObj> activityCenterRefList = new List<ActivityCenterRefObj>();
            if (_m_lActivityCenterInfoList != null)
            {
                for (int i = 0; i < _m_lActivityCenterInfoList.Count; i++)
                {
                    if(_m_lActivityCenterInfoList[i] != null && _m_lActivityCenterInfoList[i].canShow)
                        activityCenterRefList.Add(_m_lActivityCenterInfoList[i].refObj);
                }
            }

            //排序
            if(activityCenterRefList.Count > 1)
                activityCenterRefList.Sort(_sortList);

            //展示列表
            _m_wTabContainer?.showWnd();
            _m_wTabContainer?.showItemList(activityCenterRefList);

            //设置显隐
            bool isEmpty = activityCenterRefList.Count <= 0;
            ALUGUICommon.setGameObjEnable(wnd.goEmptyShowList, isEmpty);
            ALUGUICommon.setGameObjEnable(wnd.goEmptyHideList, !isEmpty);
            if(isEmpty)
                _hideAllPage();
        }

        //排序，排序id从小到大、活动类>非活动、活动进行中>结算中>领奖中、唯一id从小到大
        private int _sortList(ActivityCenterRefObj _a, ActivityCenterRefObj _b)
        {
            if (_a == null || _b == null)
                return 0;

            //排序id从小到大排序
            int comp = _a.sort_id.CompareTo(_b.sort_id);
            if (comp != 0)
                return comp;

            //活动类>非活动
            bool isActivityA = _a.activity_id > 0;
            bool isActivityB = _b.activity_id > 0;
            comp = isActivityA.CompareTo(isActivityB);
            if (comp != 0)
                return -comp;

            //活动进行中>结算中>领奖中
            if (isActivityA && isActivityB)
            {
                _ABaseActivityInfo infoA = NPPlayer.instance.commonActivityComp.getValidActivityInfoByActivityId(_a.activity_id);
                _ABaseActivityInfo infoB = NPPlayer.instance.commonActivityComp.getValidActivityInfoByActivityId(_b.activity_id);
                if (infoA != null && infoB == null)
                    return -1;
                else if(infoA == null && infoB != null)
                    return 1;
                else if (infoA != null && infoB != null)
                {
                    comp = infoA.activityState.CompareTo(infoB.activityState);
                    if (comp != 0)
                        return comp;
                }
            }

            //唯一id从小到大排序
            return _a.id.CompareTo(_b.id);
        }

        #region 消息事件

        //监听不同页签刷新显隐消息
        private void _onActivityCenterRecMsg(ActivityCenterRefObj _ref)
        {
            if (_ref == null)
                return;

            if (_m_bIsDealingRefreshList)
                return;

            _m_bIsDealingRefreshList = true;
            long serialize = _m_lShowSerialize;
            ALCommonActionMonoTask.addNextFrameTask(() =>
            {
                if (serialize != _m_lShowSerialize)
                    return;

                _m_bIsDealingRefreshList = false;
                _refreshTabList();

                //千万目标活动结束会切换到另一个界面，如果当前在千万目标界面这里刷新一次加载的页面
                if (_m_curSelectRef != null && _m_curSelectRef.type == EActivityCenterTabType.EARNING_GOAL)
                    _dealSwitchPage(false);
            });
        }

        #endregion

        #region 点击事件

        //点击页签
        private void _onClickItem(GGUIWndActivityCenterTabContainerItem _item)
        {
            if (_item == null)
                return;

            if(_m_curSelectRef == null || (_item.activityCenterRef != null && _item.activityCenterRef.id != _m_curSelectRef.id))
            {
                _m_curSelectRef = _item.activityCenterRef;
                _dealSwitchPage(true);
            }

            //设置超出的item移动到里面
            GCommon.setContainerMoveItemWithinRangeInHorizontal(_item.rectTransform, _m_wTabContainer?.rectTransform, (RectTransform)_m_wTabContainer?.wnd?.itemContainer?.transform, wnd?.moveItemAdditionDistanceParam);
        }

        //点击关闭按钮
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_ACTIVITY_CENTER_WND);
        }

        #endregion
    }
}