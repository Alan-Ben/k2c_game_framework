using System;
using ALPackage;
using DG.Tweening;
using UnityEngine;

namespace GOE
{
    public static partial class GCommon
    {
        //关闭输入序列
        private static int _m_funcUnlockInputMaskSerialize;

        /// <summary>
        /// 处理系统解锁表现流程
        /// </summary>
        /// <param name="_funcUnlockInfo">系统数据</param>
        /// <param name="_origCameraViewValue">原始fov</param>
        /// <param name="_sfxStartWorldPos">飞行特效开始位置</param>
        /// <param name="_sfxFlyTimeSec">飞行特效展示时间</param>
        public static void dealFuncUnlockProcess(FuncUnlockInfo _funcUnlockInfo, float _origCameraViewValue, Vector3 _sfxStartWorldPos, float _sfxFlyTimeSec)
        {
            if (_funcUnlockInfo == null || _funcUnlockInfo.functionUnlockRef == null)
                return;
            
            #region 设置一些状态
            
            //设置表现过程不弹出notice
            int disableOpSerialize = NPUINoticeMgr.instance.setNoticeDisable();
            //屏蔽点击拖拽
            _m_funcUnlockInputMaskSerialize = MainCameraMono.selfInstance.openAllInputMask();
            //notice解锁算设置数据完成
            NPPlayer.instance.funcUnlockComp.setShowTipDone(_funcUnlockInfo.funcType);
            //设置引导状态避免中途弹出引导
            Game.instance.openIsInTutorial(IsInTutorialConst.FUNC_UNLOCK);
            //刷新状态
            GCommon.reloadCustomLoadPrefab();
            
            #endregion
            
            //入口点位置
            Transform entryPointTransform = null;
            //入口点对象
            _IGTDHoneEntryPointView entryPointView = null;
            
            //1 ============ 判断是否是场景入口并且对应的场景正在展示，是则播放飞行特效并移动镜头
            if (_funcUnlockInfo.functionUnlockRef.entry_point_id > 0)
            {
                if (_funcUnlockInfo.functionUnlockRef.func_belong_type == EFuncBelongType.BUILDING && MainAdditionBuildingTDScene.instance.isShow)
                {
                    GNodeBuilding buildingNode = QueueMgr.instance._lastNode as GNodeBuilding;
                    if (buildingNode != null)
                    {
                        entryPointView = buildingNode.buildingViewMgr.getEntryPointView(_funcUnlockInfo.functionUnlockRef.entry_point_id);
                        entryPointTransform = entryPointView != null ? entryPointView.entryPointTransform : null;
                    }
                }
                else if (_funcUnlockInfo.functionUnlockRef.func_belong_type == EFuncBelongType.SPACE_STATION && MainAdditionSpaceStationTDScene.instance.isShow)
                {
                    entryPointTransform = MainAdditionSpaceStationTDScene.instance.getEntryPointTransform(_funcUnlockInfo.functionUnlockRef.entry_point_id);
                    entryPointView = MainAdditionSpaceStationTDScene.instance.getEntryPointView(_funcUnlockInfo.functionUnlockRef.entry_point_id);
                }
            }

            //有获取到入口位置，移动镜头并且播放飞行特效
            if (entryPointTransform != null)
            {
                //1.1 ============ 移动镜头并拉近镜头
                switch (_funcUnlockInfo.functionUnlockRef.func_belong_type)
                {
                    case EFuncBelongType.BUILDING:

                        //移动
                        MainAdditionBuildingTDScene.instance.SmoothMoveFocusTo(entryPointTransform.position, _sfxFlyTimeSec, 0);
                        //拉近
                        if (_funcUnlockInfo.functionUnlockRef.focus_camera_view_value > 0)
                        {
                            //不用相机模式不同的处理
                            if (CameraController.instance.controlCamera.orthographic)
                                MainAdditionBuildingTDScene.instance.SmoothChgOrthographicSize(_funcUnlockInfo.functionUnlockRef.focus_camera_view_value, _sfxFlyTimeSec, 0);
                            else
                                MainAdditionBuildingTDScene.instance.SmoothChgFieldOfView(_funcUnlockInfo.functionUnlockRef.focus_camera_view_value, _sfxFlyTimeSec, 0);
                        }

                        break;
                    case EFuncBelongType.SPACE_STATION:

                        //移动
                        MainAdditionSpaceStationTDScene.instance.SmoothMoveFocusTo(entryPointTransform.position, _sfxFlyTimeSec, 0);
                        //拉近
                        if (_funcUnlockInfo.functionUnlockRef.focus_camera_view_value > 0)
                        {
                            //不用相机模式不同的处理
                            if (CameraController.instance.controlCamera.orthographic)
                                MainAdditionSpaceStationTDScene.instance.SmoothChgOrthographicSize(_funcUnlockInfo.functionUnlockRef.focus_camera_view_value, _sfxFlyTimeSec, 0);
                            else
                                MainAdditionSpaceStationTDScene.instance.SmoothChgFieldOfView(_funcUnlockInfo.functionUnlockRef.focus_camera_view_value, _sfxFlyTimeSec, 0);
                        }

                        break;
                }

                //1.2 ============ 播放飞行特效
                CommonTDSfxObj sfxObj = PlaySfxMgr.instance.playTDSfx(GRefdataCoreMgr.instance.npGeneral.func_unlock_fly_sfx_id, entryPointTransform);
                if (sfxObj != null && sfxObj.sfxMono != null)
                {
                    //设置特效坐标并开始移动
                    sfxObj.regLoadDoneDelegate(() =>
                    {
                        sfxObj.setWorldPos(_sfxStartWorldPos);
                        sfxObj.sfxMono.transform.DOMove(entryPointTransform.position, _sfxFlyTimeSec);
                    });
                }
            }

            //2 ============ 镜头移动和特效表现完后展示解锁动画
            if (entryPointView != null)
            {
                //先设置解锁动画状态为未解锁状态
                entryPointView?.setUnlockAniSample(EEntryPointAniType.LOCK, 0);

                ALCommonActionMonoTask.addMonoTask(() =>
                {
                    //播放解锁动画
                    if (entryPointView != null)
                        entryPointView.playUnlockAni(() =>
                        {
                            _onPlayUnlockAniDone(_funcUnlockInfo, _origCameraViewValue, _sfxFlyTimeSec, disableOpSerialize);
                        });
            
                }, _sfxFlyTimeSec);
            }
            else
                _onPlayUnlockAniDone(_funcUnlockInfo, _origCameraViewValue, _sfxFlyTimeSec, disableOpSerialize);
        }

        /// <summary>
        /// 解锁动画播放完需要处理的事
        /// </summary>
        private static void _onPlayUnlockAniDone(FuncUnlockInfo _funcUnlockInfo, float _origCameraViewValue, float _fSfxFlyTimeSec, int _noticeDisableOpSerialize)
        {
            if (_funcUnlockInfo == null || _funcUnlockInfo.functionUnlockRef == null)
                return;

            //恢复相机后需要处理的事
            Action onFinishResetCamera = () =>
            {
                //开启notice
                NPUINoticeMgr.instance.setNoticeEnable(_noticeDisableOpSerialize);
                //开启点击拖拽
                MainCameraMono.selfInstance.closeAllInputMask(_m_funcUnlockInputMaskSerialize);
                //设置不在引导状态
                Game.instance.closeIsInTutorial(IsInTutorialConst.FUNC_UNLOCK);

                //控制引导步骤, 系统解锁表现播放结束
                WinMsg.SendMsg(WinMsgType.CONTROL_TUROTIAL_SETP, ENPTutorialTriggerType.ON_FUNC_UNLOCK_SHOW_DONE);

                
                //执行effect
                if (_funcUnlockInfo.functionUnlockRef.end_deal_effect != null)
                    _funcUnlockInfo.functionUnlockRef.end_deal_effect.dealEffect();

                //3 ============ 播放引导
                if (_funcUnlockInfo.functionUnlockRef.tutorial_id > 0)
                    NPPlayer.instance.tutorialComp.forceForceTutorialStartWith(_funcUnlockInfo.functionUnlockRef.tutorial_id);
                else
                {
                    //没有引导尝试触发下一个解锁提示
                    NPPlayer.instance.funcUnlockComp.checkNewFuncUnlock();
                }
            };

            //恢复相机
            if (_funcUnlockInfo.functionUnlockRef.focus_camera_view_value > 0 && Math.Abs(_funcUnlockInfo.functionUnlockRef.focus_camera_view_value - _origCameraViewValue) > 0)
            {
                switch (_funcUnlockInfo.functionUnlockRef.func_belong_type)
                {
                    case EFuncBelongType.BUILDING:

                        //不用相机模式不同的处理
                        if (CameraController.instance.controlCamera.orthographic)
                            MainAdditionBuildingTDScene.instance.SmoothChgOrthographicSize(_origCameraViewValue, _fSfxFlyTimeSec, 0);
                        else
                            MainAdditionBuildingTDScene.instance.SmoothChgFieldOfView(_origCameraViewValue, _fSfxFlyTimeSec, 0);

                        break;
                    case EFuncBelongType.SPACE_STATION:

                        //不用相机模式不同的处理
                        if (CameraController.instance.controlCamera.orthographic)
                            MainAdditionSpaceStationTDScene.instance.SmoothChgOrthographicSize(_origCameraViewValue, _fSfxFlyTimeSec, 0);
                        else
                            MainAdditionSpaceStationTDScene.instance.SmoothChgFieldOfView(_origCameraViewValue, _fSfxFlyTimeSec, 0);

                        break;
                }
                ALCommonActionMonoTask.addMonoTask(onFinishResetCamera, _fSfxFlyTimeSec);
            }
            else
            {
                onFinishResetCamera();
            }
        }
    }
}
