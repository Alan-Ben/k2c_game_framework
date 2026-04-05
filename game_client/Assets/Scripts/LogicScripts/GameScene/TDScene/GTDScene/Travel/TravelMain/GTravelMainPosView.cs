using System;
using ALPackage;
using Common.TravelEnum;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 主游历场景地点展示对象
    /// </summary>
    public class GTravelMainPosView  : _AGTravelMainCommonView<GTDTravelMainPosItemMono>, _ITravelParkable
    {
        [NotNull] private readonly TravelPosRefObj _m_travelPosRefObj;
        private GGUICommonFollowTarget _m_followInstance;
        private GGUITravelPosFollowItemController _m_followItemController;

        private int _m_iShowSerialize;
        
        public GTravelMainPosView([NotNull] TravelPosRefObj _travelPosRefObj, Transform _parent):base(_travelPosRefObj?.pos_res_go_index, _parent)
        {
            _m_travelPosRefObj = _travelPosRefObj;
        }

        public long posId { get { return _m_travelPosRefObj.id; } }
        public TravelPosRefObj posRef { get { return _m_travelPosRefObj; } }

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
            
            // 刚加载出来, 默认不处于停靠
            setIsParking(false, false);
            
            // 注册跟随实例
            _regFollowInstance();
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
            GGUIWndTravelPosInfo.instance.refreshWnd(_m_travelPosRefObj);
            QueueMgr.instance.addNode_InGame_SingleWnd_OnlyCloseDiscard(GGUIWndTravelPosInfo.instance, GGUIWndTravelPosInfo.instance.showWnd, UINodeTagConst.C_TRAVEL_POS_INFO);
        }

        protected override void _onHideEx()
        {
            _m_iShowSerialize = ALSerializeOpMgr.next();
        }

        protected override void _onShowEx()
        {
            refreshView();
            _showFollowController();
        }

        //刷新显示
        public void refreshView()
        {
            if(itemMono == null)
                return;

            _refreshBuildingInfo();
            _refreshUnlockShow();
        }

        private void _refreshBuildingInfo()
        {
            if (itemMono == null || _m_travelPosRefObj == null)
                return;

            ALUGUICommon.setLabelTxt(itemMono.txtBuildingName, TextTranslate.instance.getLanguage(_m_travelPosRefObj.name));
        }

        private void _refreshUnlockShow()
        {
            _refreshUnlockShow(_m_travelPosRefObj.isUnlock(null));
        }
        
        private void _refreshUnlockShow(bool _isUnlock)
        {
            if(itemMono == null)
                return;
            
            NPCommonEnumStatMutexShowInfo<EGameCommonUnlockType>.setStat(itemMono.unlockStatShowInfo, _isUnlock ? EGameCommonUnlockType.UNLOCK : EGameCommonUnlockType.LOCK);
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
        /// 解锁动画表现，同时播放建筑动画和跟随UI动画，全部完成后回调
        /// </summary>
        /// <param name="_onPlayDone">全部动画播放完成回调</param>
        public void showUnlock(Action _onPlayDone = null)
        {
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(2);
            stepCounter.regAllDoneDelegate(() =>
            {
                _onPlayDone?.Invoke();
            });

            _refreshUnlockShow(false);//需要进行解锁表现，则一开始为未解锁状态
            // 建筑自身解锁动画
            if (itemMono == null || string.IsNullOrEmpty(itemMono.unlockAniName))
            {
                _refreshUnlockShow(true);
                stepCounter.addDoneStepCount();
            }
            else
            {
                _playAnimation(itemMono.unlockAniName, () =>
                {
                    _refreshUnlockShow(true);//动画播放完后, 刷新为解锁状态
                    stepCounter.addDoneStepCount();
                });
            }

            // 跟随UI解锁动画
            if (_m_followItemController == null)
            {
                stepCounter.addDoneStepCount();
            }
            else
            {
                _m_followItemController.playUnlock(() =>
                {
                    stepCounter.addDoneStepCount();
                });
            }
        }

        /// <summary>
        /// 重复游历动画表现，同时播放建筑动画和跟随UI动画，全部完成后回调
        /// </summary>
        /// <param name="_onPlayDone">全部动画播放完成回调</param>
        public void showRepeatTravel(Action _onPlayDone = null)
        {
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(2);
            stepCounter.regAllDoneDelegate(() =>
            {
                _onPlayDone?.Invoke();
            });

            // 建筑自身重复游历动画
            if (itemMono == null || string.IsNullOrEmpty(itemMono.repeatTravelAniName))
            {
                stepCounter.addDoneStepCount();
            }
            else
            {
                _playAnimation(itemMono.repeatTravelAniName, () =>
                {
                    stepCounter.addDoneStepCount();
                });
            }

            // 跟随UI重复游历动画
            if (_m_followItemController == null)
            {
                stepCounter.addDoneStepCount();
            }
            else
            {
                _m_followItemController.playRepeatTravel(() =>
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
                _m_followItemController = new GGUITravelPosFollowItemController(_m_travelPosRefObj, itemMono.followResPathId);
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