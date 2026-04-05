using System;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 游历地点跟随控制器
    /// </summary>
    public class GGUITravelPosFollowItemController : _ATALGGUICommonFollowItemController<GGUIMonoTravelPosFollow, GGUIWndTravelPosFollow>
    {
        private readonly GResPathIndex _m_index;
        [NotNull] private readonly TravelPosRefObj _m_posRefObj;

        public GGUITravelPosFollowItemController([NotNull] TravelPosRefObj _posRefObj, int _resPathId) : base()
        {
            _m_posRefObj = _posRefObj;
            _m_index = new GResPathIndex(_resPathId);
        }


        public override _AALBasicLoadResIndexInfo followItemIndex { get => _m_index; }

        protected override GGUIWndTravelPosFollow _createItemWnd(GGUIMonoTravelPosFollow _wndMono)
        {
            return new GGUIWndTravelPosFollow(_wndMono);
        }

        /// <summary>
        /// 设置显示数据
        /// </summary>
        public void setShowData()
        {
            regItemWndLoadDoneDelegate(() =>
            {
                if (wnd == null)
                    return;

                wnd.showWnd();
                wnd.setInfo(_m_posRefObj);
            });
        }
        
        public void setIsParking(bool _isParking, bool _needPlayAnimation, Action _onPlayDone = null)
        {
            regItemWndLoadDoneDelegate(() =>
            {
                if (wnd == null)
                    return;

                wnd.setIsParking(_isParking, _needPlayAnimation, _onPlayDone);
            });
        }

        /// <summary>
        /// 播放解锁动画
        /// </summary>
        /// <param name="_onPlayDone">动画播放完成回调</param>
        public void playUnlock(Action _onPlayDone = null)
        {
            regItemWndLoadDoneDelegate(() =>
            {
                if (wnd == null)
                {
                    _onPlayDone?.Invoke();
                    return;
                }

                wnd.playUnlock(_onPlayDone);
            });
        }

        /// <summary>
        /// 播放重复游历动画
        /// </summary>
        /// <param name="_onPlayDone">动画播放完成回调</param>
        public void playRepeatTravel(Action _onPlayDone = null)
        {
            regItemWndLoadDoneDelegate(() =>
            {
                if (wnd == null)
                {
                    _onPlayDone?.Invoke();
                    return;
                }

                wnd.playRepeatTravel(_onPlayDone);
            });
        }
    }
    
    /// <summary>
    /// 游历地点跟随UI窗口
    /// </summary>
    public class GGUIWndTravelPosFollow : _ATALGGUIWndCommonFollowItem<GGUIMonoTravelPosFollow>
    {
        private TravelPosRefObj _m_posRefObj;

        private int _m_iShowSerialize;
        
        public GGUIWndTravelPosFollow(GGUIMonoTravelPosFollow _wndMono) : base(_wndMono)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
        }

        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_iShowSerialize = ALSerializeOpMgr.next();
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
        }

        /// <summary>
        /// 设置地点信息
        /// </summary>
        public void setInfo(TravelPosRefObj _posRef)
        {
            _m_posRefObj = _posRef;
            _refreshWnd();
        }

        /// <summary>
        /// 刷新显示
        /// </summary>
        private void _refreshWnd()
        {
            if (null == wnd || null == _m_posRefObj)
                return;

            // 显示地点名称
            if (wnd.txtName != null)
            {
                string posName = TextTranslate.instance.getLanguage(_m_posRefObj.name);
                ALUGUICommon.setLabelTxt(wnd.txtName, posName);
            }

            _refreshUnlockShow();
        }
        
        private void _refreshUnlockShow()
        {
            _refreshUnlockShow(_m_posRefObj?.isUnlock(null) ?? false);
        }
        
        private void _refreshUnlockShow(bool _isUnlock)
        {
            if(wnd == null)
                return;
            
            NPCommonEnumStatMutexShowInfo<EGameCommonUnlockType>.setStat(wnd.unlockStatShowInfo, _isUnlock ? EGameCommonUnlockType.UNLOCK : EGameCommonUnlockType.LOCK);
        }
        
        public void setIsParking(bool _isParking, bool _needPlayAnimation, Action _onPlayDone = null)
        {
            if (wnd == null)
            {
                _onPlayDone?.Invoke();
                return;
            }
            
            if (_isParking)
            {
                if (_needPlayAnimation)
                {
                    _playAnimation(wnd.parkingAniName, () =>
                    {
                        ALUGUICommon.setGameObjEnable(wnd.curParkingShowGoList, true);
                        
                        _onPlayDone?.Invoke();
                    });
                }
                else
                {
                    // 不需要播放动画时, 直接将停靠动画置为最后一帧
                    _samplePlayAnimation(wnd.parkingAniName, 1f);
                    ALUGUICommon.setGameObjEnable(wnd.curParkingShowGoList, true);
                    
                    _onPlayDone?.Invoke();
                }
            }
            else
            {
                ALUGUICommon.setGameObjEnable(wnd.curParkingShowGoList, false);
                _onPlayDone?.Invoke();
            }
        }

        /// <summary>
        /// 播放解锁动画
        /// </summary>
        /// <param name="_onPlayDone">动画播放完成回调</param>
        public void playUnlock(Action _onPlayDone = null)
        {
            if (wnd == null)
            {
                _refreshUnlockShow(true);
                _onPlayDone?.Invoke();
                return;
            }

            _refreshUnlockShow(false);//需要进行解锁表现, 则一开始应该为未解锁状态
            _playAnimation(wnd.unlockAniName, () =>
            {
                _refreshUnlockShow(true);//解锁动画完成后, 刷新为解锁状态
                _onPlayDone?.Invoke();
            });
        }

        /// <summary>
        /// 播放重复游历动画
        /// </summary>
        /// <param name="_onPlayDone">动画播放完成回调</param>
        public void playRepeatTravel(Action _onPlayDone = null)
        {
            if (wnd == null)
            {
                _onPlayDone?.Invoke();
                return;
            }

            _playAnimation(wnd.repeatTravelAniName, _onPlayDone);
        }
        
        #region 动画

        private void _playAnimation(string _aniName, Action _onPlayDone)
        {
            if(wnd == null || wnd.wndAnimation == null || string.IsNullOrEmpty(_aniName))
            {
                _onPlayDone?.Invoke();
                return;
            }
            
            long showSerialize = _m_iShowSerialize;
            wnd.wndAnimation.ForcePlay(_aniName, 0, ()=>
            {
                if(showSerialize != _m_iShowSerialize)
                    return;
                
                _onPlayDone?.Invoke();
            });
        }

        private void _samplePlayAnimation(string _aniName, float _normalizedTime)
        {
            if(wnd == null || wnd.wndAnimation == null || string.IsNullOrEmpty(_aniName))
                return;
            
            wnd.wndAnimation.Sample(_aniName, _normalizedTime);
        }
        
        #endregion
    }
}