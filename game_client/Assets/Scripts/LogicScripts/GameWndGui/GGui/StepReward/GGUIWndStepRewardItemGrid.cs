using System;
using System.Collections.Generic;
using ALPackage;
using Common.DinnerObj;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// item容器
    /// </summary>
    public class GGUIWndStepRewardItemGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoStepRewardItem,GGUIMonoStepRewardItemGrid,GGUIWndStepRewardItem>
    {
        private ActivityStepRewardInfo _m_curStepRewardSet;
        private bool _m_bIsShowGoTo = false;
        private List<GActivityStepRewardRefObj> _m_itemDataList = new List<GActivityStepRewardRefObj>();
        private Action _m_onClickGoTo;
        private Action<GActivityStepRewardRefObj> _m_onClickGet;
        public GGUIWndStepRewardItemGrid(GGUIMonoStepRewardItemGrid gridMono, Action _onClickGoTo, Action<GActivityStepRewardRefObj> _onClickGet) : base(gridMono)
        {
            _m_onClickGoTo = _onClickGoTo;
            _m_onClickGet = _onClickGet;
            initWnd();
        }

        protected override void _onShowWnd()
        {
            
        }

        protected override void _onHideWnd()
        {
            
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            _m_itemDataList?.Clear();
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;
        }

        protected override void _onRefreshItemWnd(GGUIWndStepRewardItem _itemMono, int _itemIdx)
        {
            if (_itemIdx < 0 || _itemIdx >= _m_itemDataList.Count)
                return;
            GActivityStepRewardRefObj itemData = _m_itemDataList[_itemIdx];
            if (_m_curStepRewardSet == null)
            {
                _itemMono?.setInfo(itemData, 0, 0, EStepRewardState.None, _m_bIsShowGoTo);
            }
            else
            {
                EStepRewardState state = _m_curStepRewardSet.getStepRewardState(itemData);
                _itemMono?.setInfo(itemData, _m_curStepRewardSet.stepRewardSetRef?.process_num_format ?? 0, _m_curStepRewardSet.totalScore, state, _m_bIsShowGoTo);
            }
        }

        protected override GGUIWndStepRewardItem _createItemWnd(GGUIMonoStepRewardItem _itemMono)
        {
            GGUIWndStepRewardItem itemWnd = new GGUIWndStepRewardItem(_itemMono, _onClickGoTo, _onClickGet);
            return itemWnd;
        }

        /// <summary>
        /// 显示item列表
        /// </summary>
        /// <param name="_itemDataList"></param>
        public void showItemList(ActivityStepRewardInfo _item, bool _isShowGoTo)
        {
            if(_item == null)
                return;
            _m_curStepRewardSet = _item;
            _m_bIsShowGoTo = _isShowGoTo;
            _m_itemDataList = new List<GActivityStepRewardRefObj>();
            _m_itemDataList.AddRange(GRefdataCoreMgr.instance.getStepRewardRefList(_item.stepRewardSetId));
            _m_itemDataList.Sort(_m_curStepRewardSet.sort);
            setItemCount(_m_itemDataList?.Count ?? 0);
        }

        private void _onClickGoTo()
        {
            _m_onClickGoTo?.Invoke();
        }
        private void _onClickGet(GActivityStepRewardRefObj _stepRef)
        {
            _m_onClickGet?.Invoke(_stepRef);
        }
    }
}
