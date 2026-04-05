using System;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 游历主城跟随控制器
    /// </summary>
    public class GGUITravelMainCityFollowItemController : _ATALGGUICommonFollowItemController<GGUIMonoTravelMainCityFollow, GGUIWndTravelMainCityFollow>
    {
        private readonly GResPathIndex _m_index;

        public GGUITravelMainCityFollowItemController(int _resPathId) : base()
        {
            _m_index = new GResPathIndex(_resPathId);
        }


        public override _AALBasicLoadResIndexInfo followItemIndex { get => _m_index; }

        protected override GGUIWndTravelMainCityFollow _createItemWnd(GGUIMonoTravelMainCityFollow _wndMono)
        {
            return new GGUIWndTravelMainCityFollow(_wndMono);
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
        /// 播放首次进入动画
        /// </summary>
        /// <param name="_onPlayDone">动画播放完成回调</param>
        public void playFirstEnter(Action _onPlayDone = null)
        {
            regItemWndLoadDoneDelegate(() =>
            {
                if (wnd == null)
                {
                    _onPlayDone?.Invoke();
                    return;
                }

                wnd.playFirstEnter(_onPlayDone);
            });
        }
    }
    
    /// <summary>
    /// 游历主城跟随UI窗口
    /// </summary>
    public class GGUIWndTravelMainCityFollow : _ATALGGUIWndCommonFollowItem<GGUIMonoTravelMainCityFollow>
    {
        private int _m_iShowSerialize;
        
        public GGUIWndTravelMainCityFollow(GGUIMonoTravelMainCityFollow _wndMono) : base(_wndMono)
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
        /// 刷新显示
        /// </summary>
        private void _refreshWnd()
        {
            if (null == wnd)
                return;

            // TODO: 根据需求补充主城跟随UI的刷新逻辑
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
        /// 播放首次进入动画
        /// </summary>
        /// <param name="_onPlayDone">动画播放完成回调</param>
        public void playFirstEnter(Action _onPlayDone = null)
        {
            if (wnd == null)
            {
                _onPlayDone?.Invoke();
                return;
            }

            _playAnimation(wnd.firstEnterAniName, () =>
            {
                _onPlayDone?.Invoke();
            });
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