using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 主游历场景主城展示对象
    /// </summary>
    public class GTravelMainCityView : _AGTravelMainCommonView<GTDTravelMainPosItemMono>, _ITravelParkable
    {
        private PlayerRoomSkinRefObj _m_roomSkinRefObj;
        private GGUICommonFollowTarget _m_followInstance;
        private GGUITravelMainCityFollowItemController _m_followItemController;
        
        private int _m_iShowSerialize;
        
        public GTravelMainCityView(PlayerRoomSkinRefObj _roomSkinRefObj, Transform _parent):base(_roomSkinRefObj?.travel_pos_res_go_index, _parent)
        {
            _m_roomSkinRefObj = _roomSkinRefObj;
        }

        protected override void _onLoadedEx(GameObject _go)
        {
            if (null == _go)
                return;

            if (_m_viewTrans != null)
            {
                _m_viewTrans.localPosition = Vector3.zero;
                _m_viewTrans.localScale = Vector3.one;
                _m_viewTrans.localRotation = Quaternion.identity;
            }
            
            if (null != itemMono && null != itemMono.clickMono)
                itemMono.clickMono.onClick += _onClick;
            
            // 注册跟随实例
            _regFollowInstance();
            refreshView();
        }

        protected override void _onDiscardEx()
        {
            if (null != itemMono && null != itemMono.clickMono)
                itemMono.clickMono.onClick -= _onClick;
            
            _discardFollowController();
            _discardFollowInstance();
        }

        protected void _onClick()
        {
            NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.travel_clickMainCityPosTip_none);
        }

        protected override void _onHideEx()
        {
            _m_iShowSerialize = ALSerializeOpMgr.next();
        }

        protected override void _onShowEx()
        {
            _showFollowController();
        }

        //刷新显示
        public void refreshView()
        {
            if(itemMono == null)
                return;
            
            // 主城默认解锁
            NPCommonEnumStatMutexShowInfo<EGameCommonUnlockType>.setStat(itemMono.unlockStatShowInfo, EGameCommonUnlockType.UNLOCK);
        }
        
        /// <summary>
        /// 设置是否当前停靠位置，显示对应的停靠展示信息
        /// </summary>
        /// <param name="_isParking"></param>
        public void setIsParking(bool _isParking, bool _needPlayAnimation, Action _onPlayDone = null)
        {
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(2);
            stepCounter.regAllDoneDelegate(() =>
            {
                _onPlayDone?.Invoke();
            });

            if (itemMono == null)
            {
                stepCounter.addDoneStepCount();
            }
            else
            {
                if (_isParking)
                {
                    if (_needPlayAnimation)
                    {
                        _playAnimation(itemMono.parkingAniName, () =>
                        {
                            ALUGUICommon.setGameObjEnable(itemMono.curParkingShowGoList, true);
                        
                            stepCounter.addDoneStepCount();
                        });
                    }
                    else
                    {
                        // 不需要播放动画时, 直接将停靠动画置为最后一帧
                        _samplePlayAnimation(itemMono.parkingAniName, 1f);
                        ALUGUICommon.setGameObjEnable(itemMono.curParkingShowGoList, true);
                    
                        stepCounter.addDoneStepCount();
                    }
                }
                else
                {
                    ALUGUICommon.setGameObjEnable(itemMono.curParkingShowGoList, false);
                    stepCounter.addDoneStepCount();
                }
            }

            if (_m_followItemController == null)
            {
                stepCounter.addDoneStepCount();
            }
            else
            {
                _m_followItemController.setIsParking(_isParking, _needPlayAnimation, () =>
                {
                    stepCounter.addDoneStepCount();
                });
            }
        }

        public Vector3 getAircraftParkingPos()
        {
            Vector3 parkingPos = Vector3.zero;
            if (itemMono != null)
            {
                if (itemMono.aircraftParkingPos != null)
                {
                    parkingPos = itemMono.aircraftParkingPos.position;
                }
                else if (viewTrans != null)
                {
                    parkingPos = viewTrans.position;
                }
            }

            return parkingPos;
        }

        /// <summary>
        /// 首次进入表现，同时播放建筑动画和跟随UI动画，全部完成后回调
        /// </summary>
        /// <param name="_onShowDone">全部动画播放完成回调</param>
        public void showFirstEnter(Action _onShowDone = null)
        {
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(2);
            stepCounter.regAllDoneDelegate(() =>
            {
                _onShowDone?.Invoke();
            });

            // 建筑自身首次进入动画
            if (itemMono == null || string.IsNullOrEmpty(itemMono.firstEnterAniName))
            {
                stepCounter.addDoneStepCount();
            }
            else
            {
                _playAnimation(itemMono.firstEnterAniName, () =>
                {
                    stepCounter.addDoneStepCount();
                });
            }

            // 跟随UI首次进入动画
            if (_m_followItemController == null)
            {
                stepCounter.addDoneStepCount();
            }
            else
            {
                _m_followItemController.playFirstEnter(() =>
                {
                    stepCounter.addDoneStepCount();
                });
            }
        }
        
        #region 动画

        private void _playAnimation(string _aniName, Action _onPlayDone)
        {
            if(itemMono == null || itemMono.animation == null || string.IsNullOrEmpty(_aniName))
            {
                _onPlayDone?.Invoke();
                return;
            }
            
            long showSerialize = _m_iShowSerialize;
            itemMono.animation.ForcePlay(_aniName, 0, ()=>
            {
                if(showSerialize != _m_iShowSerialize)
                    return;
                
                _onPlayDone?.Invoke();
            });
        }

        private void _samplePlayAnimation(string _aniName, float _normalizedTime)
        {
            if(itemMono == null || itemMono.animation == null || string.IsNullOrEmpty(_aniName))
                return;
            
            itemMono.animation.Sample(_aniName, _normalizedTime);
        }
        
        #endregion

        #region 跟随相关

        /// <summary>
        /// 注册跟随实例
        /// </summary>
        private void _regFollowInstance()
        {
            if (null == itemMono || null == itemMono.uiFollowParent || itemMono.followResPathId < 0)
                return;
            
            _m_followInstance = new GGUICommonFollowTarget(itemMono.uiFollowParent, Vector3.zero);
            GGUIWndTravelFollowRoot.instance.regInstance(_m_followInstance);
        }

        /// <summary>
        /// 显示跟随控制器
        /// </summary>
        private void _showFollowController()
        {
            if (null == _m_followInstance || null == itemMono || itemMono.followResPathId < 0)
                return;
            
            if (null == _m_followItemController)
            {
                _m_followItemController = new GGUITravelMainCityFollowItemController(itemMono.followResPathId);
                _m_followInstance.addController(_m_followItemController);
            }
            _m_followItemController.setShowData();
        }

        /// <summary>
        /// 销毁跟随控制器
        /// </summary>
        private void _discardFollowController()
        {
            _m_followItemController?.discard();
            _m_followItemController = null;
        }

        /// <summary>
        /// 销毁跟随实例
        /// </summary>
        private void _discardFollowInstance()
        {
            _m_followInstance?.discard();
            _m_followInstance = null;
        }

        #endregion
    }
}