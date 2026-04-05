using ALPackage;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 功能解锁提示
    /// </summary>
    public class NPNoticeDealer_FuncUnlockTip : NPUINoticeMgr._ANPUINoticeDealer
    {
        //解锁数据
        private FuncUnlockInfo _m_funcUnlockInfo;
        //飞行特效开始位置
        private Vector3 _m_sfxStartWorldPos;
        //飞行特效展示时间
        private float _m_fSfxFlyTimeSec;
        //原始fov
        private float _m_origCameraViewValue;
        //是否强制展示，强制展示主要用于一些效果
        private bool _m_bForceShow;
        //窗口是否已经加载，如果已经加载的情况下。在shownotice的时候不会再加载，避免窗口驻留
        private bool _m_bWndLoaded;
        //背景部分对象
        private NPPGUIWndInstanceTransparentBk _m_wTransBk;
        //操作屏蔽序列号
        private int _m_iMaskSerialize;
        //操作序列号
        private long _m_lSerialize;

        //是否看过表现
        private bool _m_isShow;

        public NPNoticeDealer_FuncUnlockTip(FuncUnlockInfo _info, bool _isForceShow = false)
        {
            _m_funcUnlockInfo = _info;
            _m_bForceShow = _isForceShow;
            _m_isShow = false;
        }


        /// <summary>
        /// 本提示对应的提示类型
        /// //此处设置OnlyHome，在Node上需要在乐园主界面Node开启这个展示
        /// </summary>
        public override ENoticeType[] noticeType
        {
            get { return NPNoticeType.g_buildingRoomWallStreetTypeArr; }
        } 

        public override bool canCurShow
        {
            get
            {
                //是否强制展示，强制展示主要用于一些效果
                if (_m_bForceShow)
                    return true;

                if (Game.instance.isInTutorial)
                    return false;

                if (_m_funcUnlockInfo == null || _m_funcUnlockInfo.functionUnlockRef == null)
                    return false;

                return _m_funcUnlockInfo.isNeedTip && 
                       ((_m_funcUnlockInfo.functionUnlockRef.func_belong_type == EFuncBelongType.ALL && (QueueMgr.instance._lastNode is GNodeBuilding || QueueMgr.instance._lastNode is GNodeSpaceStation)) ||
                       (QueueMgr.instance._lastNode is GNodeBuilding && _m_funcUnlockInfo.functionUnlockRef.func_belong_type == EFuncBelongType.BUILDING) ||
                       (QueueMgr.instance._lastNode is GNodeSpaceStation && _m_funcUnlockInfo.functionUnlockRef.func_belong_type == EFuncBelongType.SPACE_STATION));
            }
        }
        public override bool canPlayPriority { get { return true; } }
        public override bool isPriorityDealer { get { return false; } }
        public override bool needTransBk { get { return false; } }
        public override string nodeTag { get { return UINodeTagConst.C_FUNC_UNLOCK_TIP; } }
        public override bool needAutoRemove { get { return true; } }


        public override void dealShowNotice()
        {
            //不需要提示了直接结束
            if (_m_funcUnlockInfo == null)
            {
                setDealerDone();
                return;
            }

            if (!_m_bWndLoaded)
            {
                _m_bWndLoaded = true;
                NPGGUIWndFuncUnlockTip.instance.load(() =>
                {
                    if (NPGGUIWndFuncUnlockTip.instance.wnd == null)
                    {
                        setDealerDone();
                        return;
                    }

                    _dealDelayShowWnd();
                });
            }
            else
            {
                if (NPGGUIWndFuncUnlockTip.instance.wnd == null)
                {
                    setDealerDone();
                    return;
                }

                _dealDelayShowWnd();
            }
        }

        public override void dealHideNotice()
        {
            //先记录一下移动前相机位置
            _m_origCameraViewValue = CameraController.instance.controlCamera.orthographic
                ? CameraController.instance.cameraOrthographicSize
                : CameraController.instance.cameraFieldOfView;
            //记录开始位置和特效飞行时间
            _m_sfxStartWorldPos = NPGGUIWndFuncUnlockTip.instance.getFlySfxStartWorldPos();
            _m_fSfxFlyTimeSec = NPGGUIWndFuncUnlockTip.instance.getSfxFlyTimeSec();

            //销毁窗口
            if (_m_bWndLoaded)
            {
                _m_bWndLoaded = false;
                NPGGUIWndFuncUnlockTip.instance.discard();
            }

            //隐藏模糊背景
            NPUIInstanceTransparentBkController.instance.hideTransparentBk(_m_wTransBk);
            _m_wTransBk = null;

            //取消屏蔽操作
            _m_lSerialize = ALSerializeOpMgr.next();
            MainCameraMono.selfInstance.closeAllInputMask(_m_iMaskSerialize);
        }

        protected override void _onDealerDone()
        {
            if(_m_isShow)
                GCommon.dealFuncUnlockProcess(_m_funcUnlockInfo, _m_origCameraViewValue, _m_sfxStartWorldPos, _m_fSfxFlyTimeSec);
        }

        /// <summary>
        /// 因为这边node被单独new了，所以要这样处理
        /// </summary>
        public override void clickBkAction()
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_FUNC_UNLOCK_TIP);
        }

        //处理延时展示界面及模糊背景
        private void _dealDelayShowWnd()
        {
            if (!NPGGUIWndFuncUnlockTip.instance.isLoaded || NPGGUIWndFuncUnlockTip.instance.wnd == null)
                return;

            //屏蔽操作
            _m_iMaskSerialize = MainCameraMono.selfInstance.openAllInputMask();
            //设置显示序列号
            _m_lSerialize = ALSerializeOpMgr.next();
            long serialize = _m_lSerialize;
            //延时展示模糊背景和窗口
            float delayTime = NPGGUIWndFuncUnlockTip.instance.wnd.delayShowTimeSec;
            ALCommonActionMonoTask.addMonoTask(() =>
            {
                //检测序列号是否一致，如果不一致说明已经被关闭了，不再继续处理
                if (serialize != _m_lSerialize)
                    return;

                //取消操作屏蔽
                MainCameraMono.selfInstance.closeAllInputMask(_m_iMaskSerialize);
                //展示模糊背景
                _m_wTransBk = NPPGUIWndInstanceTransparentBk.showTransparentBk(
                    NPGGUIWndFuncUnlockTip.instance
                    , () => { QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_FUNC_UNLOCK_TIP); }
                    , () =>
                    {
                        if (serialize != _m_lSerialize)
                            return;

                        _m_isShow = true;
                        _afterTransBkAction();
                    });
            }, delayTime);
        }

        //展示模糊背景后执行的操作
        protected void _afterTransBkAction()
        {
            NPGGUIWndFuncUnlockTip.instance.regLoadDoneDelegate(() =>
            {
                //模糊背景需要跟着窗口后面，先将模糊背景移动到最前
                if (_m_wTransBk != null)
                    GCommon.moveTransformToLastAndRefreshLayer(_m_wTransBk.getGameObj(), NPGGUIWndFuncUnlockTip.instance.getGameObj());
                else
                    //将窗口移到最前
                    GCommon.moveTransformToLastAndRefreshLayer(NPGGUIWndFuncUnlockTip.instance.getGameObj());

                List<FuncUnlockInfo> infoList = new List<FuncUnlockInfo>();
                infoList.Add(_m_funcUnlockInfo);
                NPGGUIWndFuncUnlockTip.instance.showWnd();
                NPGGUIWndFuncUnlockTip.instance.setInfo(infoList);
            });
        }
    }
}
