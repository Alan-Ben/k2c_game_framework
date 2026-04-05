using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 活动阶段奖励步骤列表
    /// </summary>
    public class GGUIWndActivityStepRewardStepGrid : _ANPGGUIBasicGridSubWnd<GGUIMonoActivityStepRewardStepGridItem, GGUIMonoActivityStepRewardStepGrid, GGUIWndActivityStepRewardStepGridItem>
    {
        [NotNull]private List<GActivityStepRewardRefObj> _m_showList;
        private ActivityStepRewardInfo _m_stepRewardInfo;

        public GGUIWndActivityStepRewardStepGrid(GGUIMonoActivityStepRewardStepGrid _wnd)
            : base(_wnd)
        {
            _m_showList = new List<GActivityStepRewardRefObj>();
            initWnd();
        }

        protected override GGUIWndActivityStepRewardStepGridItem _createItemWnd(GGUIMonoActivityStepRewardStepGridItem _itemMono)
        {
            return new GGUIWndActivityStepRewardStepGridItem(_itemMono);
        }

        protected override void _refreshItemwnd(GGUIWndActivityStepRewardStepGridItem _itemWnd, int _itemIdx)
        {
            if (_itemIdx < 0 || _itemIdx >= _m_showList.Count)
                return;

            GActivityStepRewardRefObj info = _m_showList[_itemIdx];
            _itemWnd.setInfo(info, _m_stepRewardInfo);
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
        }

        protected override void _onWndInitDone()
        {
        }

        /// <summary>
        /// 设置显示数据
        /// </summary>
        /// <param name="_stepRewardInfo"></param>
        public void setInfo(ActivityStepRewardInfo _stepRewardInfo)
        {
            if(_stepRewardInfo == null)
                return;

            List<GActivityStepRewardRefObj> stepRewardRefList = GRefdataCoreMgr.instance.getStepRewardRefList(_stepRewardInfo.stepRewardSetId);
            if (stepRewardRefList == null)
                return;
            stepRewardRefList.Sort((_a,_b)=>_a.step.CompareTo(_b.step));

            _m_showList.Clear();
            _m_showList.AddRange(stepRewardRefList);
            _m_stepRewardInfo = _stepRewardInfo;

            setItemCount(_m_showList.Count);

            //当前正在进行中的步骤id
            GActivityStepRewardRefObj curStepRef = _stepRewardInfo.getFirstNotGetRewardStep();
            long curStepId = curStepRef != null ? curStepRef.step : 0;
            //列表移动的目标位置
            int targetIndex = 0;
            for (int i = 0; i < _m_showList.Count; i++)
            {
                if (_m_showList[i].step == curStepId)
                {
                    targetIndex = i;
                    break;
                }
            }
            //移动到目标位置
            _moveToTarget(targetIndex);
        }

        //移动到目标位置
        private void _moveToTarget(int _index)
        {
            if (wnd == null || null == wnd.itemTemplate)
                return;

            ALCommonTaskController.CommonActionAddNextFrameTask(() =>
            {
                // 计算content的坐标活动范围
                if (wnd != null && 
                    wnd.itemTemplate != null && 
                    wnd.gridAreaUIObj != null && 
                    wnd.gridAreaUIObj.rect != null && 
                    wnd.gridAreaMaskObj != null && 
                    wnd.gridAreaMaskObj.rect != null)
                {
                    float contentRange = wnd.gridAreaUIObj.rect.height - wnd.gridAreaMaskObj.rect.height;
                    moveToVerticalRate((_index * (wnd.itemTemplate.height + wnd.spaceSize.y)) / contentRange);
                }
            });
        }
    }
}
