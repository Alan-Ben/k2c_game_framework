using ALPackage;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    public class GGUISubWndStageGoalOverviewGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoStageGoalOverviewGridItem, GGUIMonoStageGoalOverviewGrid, GGUISubWndStageGoalOverviewGridItem>
    {
        private readonly Action _m_jumpToFunc;
        
        private List<StageGoalBigStepRefObj> _m_refList;
        private long _m_lMaxBigStageNum;
        private int _m_privateCount;
        private bool _m_moveToTarget;
        // 当前阶段bar控制器
        private GGUIWndStageGoalOverviewCurBarController _m_curBarController;
        // 结束bar控制器
        private GGUIWndStageGoalOverviewEndBarController _m_endBarController;
        // 总的Content高度
        private float _m_fTotalContentHeight;
        // 是否需要播放到达新阶段动画
        private bool _m_bNeedPlayNewAni;


        public GGUISubWndStageGoalOverviewGrid(GGUIMonoStageGoalOverviewGrid _wnd, Action _jumpToFunc) 
            : base(_wnd)
        {
            _m_jumpToFunc = _jumpToFunc;
            
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
            if (null != _m_curBarController)
            {
                removeBar(_m_curBarController);
                _m_curBarController.discard();
                _m_curBarController = null;
            }

            if (null != _m_endBarController)
            {
                removeBar(_m_endBarController);
                _m_endBarController.discard();
                _m_endBarController = null;
            }
        }
        protected override void _onWndInitDone()
        {
        }
        protected override GGUISubWndStageGoalOverviewGridItem _createItemWnd(GGUIMonoStageGoalOverviewGridItem _itemMono)
        {
            return new GGUISubWndStageGoalOverviewGridItem(_itemMono, _onBtnJumpToClick);
        }
        protected override void _onRefreshItemWnd(GGUISubWndStageGoalOverviewGridItem _itemMono, int _itemIdx)
        {
            if (_m_refList == null || _itemMono == null)
                return;

            StageGoalBigStepRefObj refObj = _m_refList.SafeGet(_m_refList.Count - 1 - (_itemIdx - _m_privateCount));
            bool isFirst = _itemIdx == _m_refList.Count - 1 + _m_privateCount;
            bool isLast = _itemIdx == 0;
            bool isPrivate = _itemIdx < _m_privateCount;
            long privateNum = isPrivate ? _m_privateCount - _itemIdx + _m_lMaxBigStageNum : 0;
            _itemMono.refreshWnd(refObj, isFirst, isLast, isPrivate, privateNum);
        }

        protected override void _onFrameRefresh()
        {
            base._onFrameRefresh();

            // 移动到目标位置
            if (_m_moveToTarget)
            {
                _m_moveToTarget = false;
                _moveTargetToMiddle(_getTargetIndex()); 
                //移动当前bar到前面
                _m_curBarController?.setMoveTrasnformToLast();
                // 处理播放到达新阶段动画
                _dealPlayNewStageProcess();
            }
        }

        // 刷新列表
        public void refreshWnd(List<StageGoalBigStepRefObj> _refList, int _privateCount)
        {
            // 先设置列表可拖动
            if (wnd != null && wnd.scrollRect != null)
                wnd.scrollRect.enabled = true;
            // 当前需要展示的列表
            _m_refList = _refList;
            // 记录最大阶段数id
            _m_lMaxBigStageNum = _m_refList != null && _m_refList.Count > 0 ? _m_refList[_m_refList.Count - 1].big_step : 0;
            // 如果完成了最后一个阶段，不展示不公开阶段
            _m_privateCount = NPPlayer.instance.stageGoalComp.isAllDone ? 0 : _privateCount;
            // 计算Content总高度
            _m_fTotalContentHeight = _calContentTotalHeight();
            // 检查是否需要播放到达新阶段动画
            _m_bNeedPlayNewAni = _checkNeedPlayNewAni();
            // 设置item数量
            setItemCount((_m_refList?.Count ?? 0) + _m_privateCount);
            // 移动到最下面，同时在下一帧也做一次，因为第一次的话 item 是在下一帧刷，后面因为 item 没有改变，直接移动才不会看到闪烁
            _moveTargetToMiddle(_getTargetIndex());
            _m_moveToTarget = true;
            // 刷新bar
            _showBar();
        }

        // 检查是否需要播放到达新阶段动画
        private bool _checkNeedPlayNewAni()
        {
            if (_m_refList == null)
                return false;
            StageGoalBigStepRefObj curBigStageRefObj = NPPlayer.instance.stageGoalComp.bigStepRefObj;
            bool needPlay = curBigStageRefObj != null && curBigStageRefObj.big_step > AccountSettingMgr.instance.accountSetting.alreadyShowUnlockAniBigStageGoalId;
            return needPlay;
        }

        //展示当前阶段bar和结束bar
        private void _showBar()
        {
            if (wnd == null)
                return;

            // 展示结束bar
            if (NPPlayer.instance.stageGoalComp.isAllDone)
            {
                if (_m_endBarController == null)
                {
                    _m_endBarController = new GGUIWndStageGoalOverviewEndBarController(wnd.gridAreaUIObj);
                    addBar(_m_endBarController);
                }

                _m_endBarController.regLoadDoneDelegate(() =>
                {
                    _m_endBarController.setInsertIndex(0);
                    forceRefreshBar();
                });
            }

            // 展示当前bar
            if (_m_curBarController == null)
            {
                _m_curBarController = new GGUIWndStageGoalOverviewCurBarController(wnd.gridAreaUIObj);
                addBar(_m_curBarController);
            }

            StageGoalBigStepRefObj curBigStageRefObj = NPPlayer.instance.stageGoalComp.bigStepRefObj;
            int targetIndex = -1;
            if (curBigStageRefObj != null)
            {
                for (int i = 0; i < _m_refList.Count; i++)
                {
                    if (_m_refList[i].big_step == curBigStageRefObj.big_step)
                    {
                        targetIndex = _m_refList.Count - 1 - i + _m_privateCount;
                        break;
                    }
                }
            }

            _m_curBarController.regLoadDoneDelegate(() =>
            {
                _m_curBarController.setInsertIndex(targetIndex + 1);
                _m_curBarController.setMoveTrasnformToLast();

                // 是否需要播放动画，需要的话重置到第一帧，否则重置到最后一帧
                if(_m_bNeedPlayNewAni)
                    _m_curBarController.sampleStageAni(0);
                else
                    _m_curBarController.sampleStageAni(1);

                forceRefreshBar();
            });
        }

        // 处理播放到达新阶段动画
        private void _dealPlayNewStageProcess()
        {
            if (!_m_bNeedPlayNewAni)
                return;

            ALCommonTaskController.CommonActionAddNextFrameTask(() =>
            {
                // 当前大阶段
                StageGoalBigStepRefObj curBigStageRefObj = NPPlayer.instance.stageGoalComp.bigStepRefObj;
                // 先屏蔽所有输入
                int maskSerialize = MainCameraMono.selfInstance.openAllInputMask();
                ALProcess process = ALProcess.CreateProcess();
                process
                    .addDelegateProcess(_onDone =>
                    {
                        // ========播放完成动画========
                        if (curBigStageRefObj == null)
                            _onDone?.Invoke();
                        else
                        {
                            bool isPlayDone = false;
                            bool isPlay = false;
                            refreshAllItem((_item, _index) =>
                            {
                                // 先设置当前阶段的解锁动画在第一帧
                                if (_item != null && _item.refObj != null && curBigStageRefObj.big_step == _item.refObj.big_step)
                                    _item.sampleUnlockAni(0);

                                // 如果还没播放完成动画，播放一次
                                if (_item != null && _item.refObj != null && curBigStageRefObj.big_step > _item.refObj.big_step && !AccountSettingMgr.instance.accountSetting.getIsBigStageGoalAlreadyShowFinishAni(_item.refObj.big_step))
                                {
                                    isPlay = true;
                                    _item.playFinishAni(() =>
                                    {
                                        // 播放完了，执行一次回调
                                        if (!isPlayDone)
                                        {
                                            isPlayDone = true;
                                            _onDone?.Invoke();
                                        }
                                    });
                                }

                            });
                            if (!isPlay)
                                _onDone?.Invoke();
                        }
                    })
                    .addDelegateProcess(_onDone =>
                    {
                        // ========播放火箭移动动画========
                        if (_m_curBarController == null)
                        {
                            _onDone?.Invoke();
                        }
                        else
                        {
                            _m_curBarController.regLoadDoneDelegate(() =>
                            {
                                _m_curBarController.playNewStageAni(_onDone);
                            });
                        }
                    })
                    .addDelegateProcess(_onDone =>
                    {
                        // ========播放解锁动画========
                        if (curBigStageRefObj == null)
                            _onDone?.Invoke();
                        else
                        {
                            bool isFind = false;
                            refreshAllItem((_item, _index) =>
                            {
                                if (_item != null && _item.refObj != null && _item.refObj.big_step == curBigStageRefObj.big_step)
                                {
                                    isFind = true;
                                    _item.playUnlockAni(_onDone);
                                }
                            });
                            if (!isFind)
                                _onDone?.Invoke();
                        }
                    })
                    .addProcess(() =>
                    {
                        // ========恢复所有输入========
                        MainCameraMono.selfInstance.closeAllInputMask(maskSerialize);
                        //发送引导触发消息
                        WinMsg.SendMsg(WinMsgType.CONTROL_TUROTIAL_SETP, ENPTutorialTriggerType.STAGE_GOAL_PLAY_NEW_STAGE_ANI_DONE);
                    })
                    .deal();
            });
        }

        #region 移动相关方法

        /// <summary>
        /// 获取当前需要展示的目标位置的垂直滚动比例
        /// </summary>
        /// <returns></returns>
        public float getCurShowTargetVerticalRate()
        {
            return _getTargetVerticalRate(_getTargetIndex());
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
                // 计算content的坐标活动范围
                float contentRange = _m_fTotalContentHeight - wnd.gridAreaMaskObj.rect.height;
                // 计算目标位置在gridAreaMaskObj居中的垂直滚动比例，(目标item高度 + 列表上方偏移 - mask高度的一半 + item高度的一半) / contentRange
                targetRate = (_index * (wnd.itemTemplate.height + wnd.spaceSize.y) + wnd.paddingForSide.x - wnd.gridAreaMaskObj.rect.height / 2.0f + wnd.itemTemplate.height / 2.0f) / contentRange;
                targetRate = Mathf.Clamp(targetRate, 0f, 1f);
            }
            return targetRate;
        }

        // 计算Content总高度
        private float _calContentTotalHeight()
        {
            if (wnd == null)
                return 0;

            bool isAllDone = NPPlayer.instance.stageGoalComp.isAllDone;
            float totalHeight = 0f;
            float itemCount = 0;
            float endBarHeight = 0f;
            if (isAllDone)
            {
                // 全部完成时，计算结束bar高度，并且item数量不包含不公开阶段
                RectTransform endBarRect = wnd.monoEndBar != null ? wnd.monoEndBar.transform as RectTransform : null;
                endBarHeight = endBarRect != null ? endBarRect.rect.height : 0;
                itemCount = _m_refList != null ? _m_refList.Count : 0;
            }
            else
            {
                // 未全部完成时，计算item数量包含不公开阶段
                itemCount = _m_refList != null ? _m_refList.Count + _m_privateCount : _m_privateCount;
            }

            // 计算总的item高度
            if (wnd != null && wnd.itemTemplate != null)
                totalHeight = wnd.itemTemplate.height * itemCount + wnd.spaceSize.y * (itemCount - 1);

            // 加上结束bar和padding的高度
            totalHeight = totalHeight + endBarHeight + wnd.paddingForSide.x + wnd.paddingForSide.y;
            return totalHeight;
        }

        //获取需要移动到的目标索引（无奖励可领取时，当前所在阶段；有奖励可领取时，第一个可领取阶段）
        private int _getTargetIndex()
        {
            if (_m_refList == null)
                return 0;

            StageGoalBigStepRefObj curBigStageRefObj = NPPlayer.instance.stageGoalComp.bigStepRefObj;
            if (curBigStageRefObj == null)
                return _m_refList.Count - 1 + _m_privateCount;

            int listIndex = 0;
            for (int i = 0; i < _m_refList.Count; i++)
            {
                if (_m_refList[i] == null)
                    continue;

                if (NPPlayer.instance.stageGoalComp.bigGoalCanGetReward(_m_refList[i].big_step) || _m_refList[i].big_step == curBigStageRefObj.big_step)
                {
                    listIndex = i;
                    break;
                }
            }

            return _m_refList.Count - 1 - listIndex + _m_privateCount;
        }

        //移动目标索引item到中间位置
        private void _moveTargetToMiddle(int _index)
        {
            ALCommonTaskController.CommonActionAddLaterMonoTask(() =>
            {
                moveToVerticalRate(_getTargetVerticalRate(_index));
            });
        }

        #endregion

        #region 点击事件

        // 点击跳转按钮
        private void _onBtnJumpToClick()
        {
            _m_jumpToFunc?.Invoke();
        }

        #endregion
    }
}