using ALPackage;
using Common.StageGoalObj;
using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 阶段目标-时代之巅阶段列表
    /// </summary>
    public class GGUIWndStageGoalPagePeakGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoStageGoalPagePeakGridItem, GGUIMonoStageGoalPagePeakGrid, GGUIWndStageGoalPagePeakGridItem>
    {
        //总的数据列表
        [NotNull] private List<StageGoalBigStepRefObj> _m_lInfoList = new List<StageGoalBigStepRefObj>();
        //首达信息字典
        [NotNull] private Dictionary<long, StageGoalFirstReachInfo> _m_dReachInfoDic = new Dictionary<long, StageGoalFirstReachInfo>();
        //是否移动到目标位置
        private bool _m_moveToTarget;

        public GGUIWndStageGoalPagePeakGrid(GGUIMonoStageGoalPagePeakGrid _containerMono) : base(_containerMono)
        {
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
            if (_m_lInfoList != null)
                _m_lInfoList.Clear();
        }

        protected override void _onWndInitDone()
        {
            // 初始化容器
            setItemCount(0);
        }

        // 创建对象
        protected override GGUIWndStageGoalPagePeakGridItem _createItemWnd(GGUIMonoStageGoalPagePeakGridItem _itemMono)
        {
            // 创建对象
            GGUIWndStageGoalPagePeakGridItem gridItem = new GGUIWndStageGoalPagePeakGridItem(_itemMono);
            return gridItem;
        }

        // 刷新对象
        protected override void _onRefreshItemWnd(GGUIWndStageGoalPagePeakGridItem _itemMono, int _itemIdx)
        {
            if (_m_lInfoList.Count <= _itemIdx)
                return;

            //获取倒序的数据
            StageGoalBigStepRefObj info = _m_lInfoList[_m_lInfoList.Count - _itemIdx - 1];
            _itemMono?.setInfo(info, _getFirstReachInfo(info != null ? info.big_step : 0));
        }

        protected override void _onFrameRefresh()
        {
            base._onFrameRefresh();

            // 移动到目标位置
            if (_m_moveToTarget)
            {
                _m_moveToTarget = false;
                _moveTargetToMiddle(_getTargetIndex());
            }
        }

        /// <summary>
        /// 初始化列表
        /// </summary>
        /// <param name="_infoList"></param>
        public void setInfo(List<StageGoalBigStepRefObj> _infoList, bool _needMoveToTarget)
        {
            if (null == _infoList)
                return;

            _m_lInfoList.Clear();
            _m_lInfoList.AddRange(_infoList);

            //刷新grid
            setItemCount(_m_lInfoList.Count);

            if(_needMoveToTarget)
            {
                // 移动到最下面，同时在下一帧也做一次，因为第一次的话 item 是在下一帧刷，后面因为 item 没有改变，直接移动才不会看到闪烁
                _moveTargetToMiddle(_getTargetIndex());
                _m_moveToTarget = true;
            }
        }

        /// <summary>
        /// 设置首达信息
        /// </summary>
        /// <param name="_reachInfoList"></param>
        public void setFirstReachInfo(List<StageGoalFirstReachInfo> _reachInfoList)
        {
            if (_reachInfoList == null || _reachInfoList.Count == 0)
                return;

            _m_dReachInfoDic.Clear();
            for (int i = 0; i < _reachInfoList.Count; i++)
            {
                if (_reachInfoList[i] == null)
                    continue;

                _m_dReachInfoDic[_reachInfoList[i].bigStepId] = _reachInfoList[i];
            }
            forceRefreshAllItem();
        }

        /// <summary>
        /// 获取首达信息
        /// </summary>
        /// <param name="_bigStepId"></param>
        /// <returns></returns>
        private StageGoalFirstReachInfo _getFirstReachInfo(long _bigStepId)
        {
            if (_m_dReachInfoDic.TryGetValue(_bigStepId, out StageGoalFirstReachInfo reachInfo))
            {
                return reachInfo;
            }

            return null;
        }

        /// <summary>
        /// 移动目标索引item到中间位置
        /// </summary>
        /// <param name="_index"></param>
        private void _moveTargetToMiddle(int _index)
        {
            ALCommonTaskController.CommonActionAddLaterMonoTask(() =>
            {
                moveToVerticalRate(1 - _getTargetVerticalRate(_index));
            });
        }

        /// <summary>
        /// 获取目标位置的垂直滚动比例
        /// </summary>
        /// <param name="_index"></param>
        /// <returns></returns>
        private float _getTargetVerticalRate(int _index)
        {
            float targetRate = 0f;

            if (wnd != null &&
                wnd.itemTemplate != null &&
                wnd.gridAreaUIObj != null &&
                wnd.gridAreaUIObj.rect != null &&
                wnd.gridAreaMaskObj != null &&
                wnd.gridAreaMaskObj.rect != null)
            {
                float contentRange = wnd.gridAreaUIObj.rect.height - wnd.gridAreaMaskObj.rect.height;
                targetRate = (_index * (wnd.itemTemplate.height + wnd.spaceSize.y) - wnd.gridAreaMaskObj.rect.height / 2.0f + wnd.itemTemplate.height / 2.0f) / contentRange;
                targetRate = Mathf.Clamp(1 - targetRate, 0f, 1f);
            }
            return targetRate;
        }

        //获取需要移动到的目标索引（无奖励可领取时，当前所在阶段；有奖励可领取时，第一个可领取阶段）
        private int _getTargetIndex()
        {
            long latestReachBigStepId = NPPlayer.instance.stageGoalComp.getLatestHadSomeoneReachBigStepId();
            int listIndex = 0;

            for (int i = 0; i < _m_lInfoList.Count; i++)
            {
                if (_m_lInfoList[i] == null)
                    continue;

                //最高有玩家到达的阶段
                if(_m_lInfoList[i].big_step == latestReachBigStepId)
                    listIndex = i;

                //首个未领取的阶段
                if (NPPlayer.instance.stageGoalComp.getFirstReachBigStepRewardType(_m_lInfoList[i].big_step) == ECommonRewardType.CAN_GET_REWARD)
                {
                    listIndex = i;
                    break;
                }
            }

            return _m_lInfoList.Count - 1 - listIndex;
        }
    }
}
