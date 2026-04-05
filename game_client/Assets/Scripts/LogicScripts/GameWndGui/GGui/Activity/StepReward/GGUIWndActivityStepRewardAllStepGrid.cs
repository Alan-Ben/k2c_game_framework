using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    public class GGUIWndActivityStepRewardAllStepGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoActivityStepRewardAllStepGridItem, GGUIMonoActivityStepRewardAllStepGrid, GGUIWndActivityStepRewardAllStepGridItem>
    {
        private struct ActivityStepRewardInfoStepRefStruct
        {
            public ActivityStepRewardInfo stepRewardInfo;
            public GActivityStepRewardRefObj stepRewardRefObj;
        }
        
        [NotNull]private List<ActivityStepRewardInfoStepRefStruct> _m_showList = new List<ActivityStepRewardInfoStepRefStruct>();
        private List<GActivityStepRewardRefObj> _m_lTmpStepRewardRefList;
        private Dictionary<GActivityStepRewardRefObj, EStepRewardState> _m_dTmpStepRewardStateDict = new Dictionary<GActivityStepRewardRefObj, EStepRewardState>();

        public GGUIWndActivityStepRewardAllStepGrid(GGUIMonoActivityStepRewardAllStepGrid _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override GGUIWndActivityStepRewardAllStepGridItem _createItemWnd(GGUIMonoActivityStepRewardAllStepGridItem _itemMono)
        {
            return new GGUIWndActivityStepRewardAllStepGridItem(_itemMono);
        }
        
        protected override void _onRefreshItemWnd(GGUIWndActivityStepRewardAllStepGridItem _itemMono, int _itemIdx)
        {
            if (_itemIdx < 0 || _itemIdx >= _m_showList.Count || _itemMono == null)
                return;
            
            ActivityStepRewardInfoStepRefStruct info = _m_showList[_itemIdx];
            _itemMono.setInfo(info.stepRewardRefObj, info.stepRewardInfo);
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_lTmpStepRewardRefList?.Clear();
            _m_dTmpStepRewardStateDict?.Clear();
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            _m_showList.Clear();
            
            _m_lTmpStepRewardRefList?.Clear();
            _m_lTmpStepRewardRefList = null;
            
            _m_dTmpStepRewardStateDict?.Clear();
            _m_dTmpStepRewardStateDict = null;
        }

        protected override void _onWndInitDone()
        {
        }

        /// <summary>
        /// 设置显示数据
        /// </summary>
        /// <param name="_stepRewardInfoList"></param>
        public void setInfo(List<ActivityStepRewardInfo> _stepRewardInfoList)
        {
            if(_stepRewardInfoList == null)
                return;

            _m_showList.Clear();
            if(_m_lTmpStepRewardRefList == null)
                _m_lTmpStepRewardRefList = new List<GActivityStepRewardRefObj>();
            if(_m_dTmpStepRewardStateDict == null)
                _m_dTmpStepRewardStateDict = new Dictionary<GActivityStepRewardRefObj, EStepRewardState>();
            
            foreach (var stepRewardInfo in _stepRewardInfoList)
            {
                if(stepRewardInfo == null)
                    continue;
                
                _m_lTmpStepRewardRefList.Clear();
                _m_dTmpStepRewardStateDict.Clear();
                GRefdataCoreMgr.instance.getStepRewardRefList(stepRewardInfo.stepRewardSetId, _m_lTmpStepRewardRefList);
                foreach (var stepRewardRefObj in _m_lTmpStepRewardRefList)
                {
                    if(stepRewardRefObj == null)
                        continue;
                    
                    EStepRewardState rewardState = stepRewardInfo.getStepRewardState(stepRewardRefObj);
                    _m_dTmpStepRewardStateDict[stepRewardRefObj] = rewardState;
                    
                    //加入显示列表中
                    _m_showList.Add(new ActivityStepRewardInfoStepRefStruct(){stepRewardInfo = stepRewardInfo, stepRewardRefObj = stepRewardRefObj});
                }
            }
            _m_showList.Sort(_sort);
            
            int firstCanGetOrUndoneIndex = -1;//第一个可以领取或未完成的阶段奖励索引
            for (int i = 0; i < _m_showList.Count; i++)
            {
                var stepRewardRefObj = _m_showList[i].stepRewardRefObj;
                if(stepRewardRefObj == null)
                    continue;
                
                _m_dTmpStepRewardStateDict.TryGetValue(stepRewardRefObj, out EStepRewardState stepRewardState);
                // 若找到了第一个可领取的就跳出循环
                if (stepRewardState == EStepRewardState.CanGet)
                {
                    firstCanGetOrUndoneIndex = i;
                    break;
                }
                
                // 若找到了一个未达成状态 且 之前没有记录可以领取或未完成的阶段, 记录下来
                if(stepRewardState == EStepRewardState.None && firstCanGetOrUndoneIndex == -1)
                {
                    firstCanGetOrUndoneIndex = i;
                }
            }
            
            setItemCount(_m_showList.Count);
            
            //移动到目标位置
            _moveToTarget(firstCanGetOrUndoneIndex);
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
        
        private int _sort(ActivityStepRewardInfoStepRefStruct a, ActivityStepRewardInfoStepRefStruct b)
        {
            if (b.stepRewardInfo == null || b.stepRewardRefObj == null)
                return -1;
            if (a.stepRewardInfo == null || a.stepRewardRefObj == null)
                return 1;
            
            // 获取奖励状态
            EStepRewardState stateA = EStepRewardState.None;
            EStepRewardState stateB = EStepRewardState.None;
            _m_dTmpStepRewardStateDict?.TryGetValue(a.stepRewardRefObj, out stateA);
            _m_dTmpStepRewardStateDict?.TryGetValue(b.stepRewardRefObj, out stateB);
            
            // 可领取的排在最前面
            if (stateA == EStepRewardState.CanGet && stateB != EStepRewardState.CanGet)
                return -1;
            if (stateB == EStepRewardState.CanGet && stateA != EStepRewardState.CanGet)
                return 1;
            
            // 已领取的排在最后面
            if (stateA == EStepRewardState.AlreadyGet && stateB != EStepRewardState.AlreadyGet)
                return 1;
            if (stateB == EStepRewardState.AlreadyGet && stateA != EStepRewardState.AlreadyGet)
                return -1;

            // 相同状态下按step_reward_set_id从小到大排序
            int compareRewardSetId = a.stepRewardInfo.stepRewardSetId.CompareTo(b.stepRewardInfo.stepRewardSetId);
            if (compareRewardSetId != 0)
                return compareRewardSetId;
            
            // 最后按step从小到大排序
            return a.stepRewardRefObj.step.CompareTo(b.stepRewardRefObj.step);
        }
    }
}