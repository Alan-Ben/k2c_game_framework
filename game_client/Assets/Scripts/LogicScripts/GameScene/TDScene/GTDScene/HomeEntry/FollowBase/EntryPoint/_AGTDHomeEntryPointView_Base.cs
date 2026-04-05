using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 主城入口跟随点的基类
    /// </summary>
    /// <typeparam name="T_TD_MONO">td上item的mono</typeparam>
    /// <typeparam name="T_FOLLOW_CONTROL">UI跟随的control</typeparam>
    /// <typeparam name="T_FOLLOW_MONO">UI跟随的mono</typeparam>
    /// <typeparam name="T_FOLLOW_WND">UI跟随的wnd</typeparam>
    public abstract class _AGTDHomeEntryPointView_Base<T_TD_MONO, T_FOLLOW_CONTROL, T_FOLLOW_MONO, T_FOLLOW_WND> : _IGTDHoneEntryPointView
        where T_TD_MONO : _AGTDHomeEntryPointMono_Base
        where T_FOLLOW_CONTROL : _AGGUIEntryPointFollowItemBaseController<T_FOLLOW_MONO, T_FOLLOW_WND>, new()
        where T_FOLLOW_MONO : _AGGUIMonoEntryPointFollowItemBase
        where T_FOLLOW_WND : _AGGUIWndEntryPointFollowItemBase<T_FOLLOW_MONO>
    {
        //入口点mono
        protected T_TD_MONO _m_entryPointMono;
        //跟随点的control
        protected T_FOLLOW_CONTROL _m_followControl;

        //入口点的跟随实例
        protected HomeEntryPointFollowInstance _m_followInstance;
        //入口点配表
        protected EntryPointRefObj _m_entryPointRefObj;
        //未解锁时点击回调
        protected Action<Transform, EntryPointRefObj> _m_aOnLockClick;

        /// <summary>
        /// 未解锁时点击回调
        /// </summary>
        public Action<Transform, EntryPointRefObj> onLockClick
        {
            get { return _m_aOnLockClick;}
            set { _m_aOnLockClick = value; }
        }

        /// <summary>
        /// 入口点表配置
        /// </summary>
        public EntryPointRefObj entryPointRefObj 
        {
            get { return _m_entryPointRefObj; }
        }

        /// <summary>
        /// 入口点位置
        /// </summary>
        public Transform entryPointTransform
        {
            get { return _m_entryPointMono != null && _m_entryPointMono.clickMono != null ? _m_entryPointMono.clickMono.transform : null; }
        }

        protected _AGTDHomeEntryPointView_Base(T_TD_MONO _entryPointMono)
        {
            if(null == _entryPointMono)
            {
                Debug.Log_EditorOnly($"入口点的mono配置错误:{this.GetType()}");
                return;
            }
            
            _m_entryPointMono = _entryPointMono;

            if (null != _m_entryPointMono && null != _m_entryPointMono.followParent)
            {
                _m_followInstance = new HomeEntryPointFollowInstance(_m_entryPointMono.entryPointId, _m_entryPointMono.followParent);
            }

            //获取配置数据
            _m_entryPointRefObj = GRefdataCoreMgr.instance.entryPointRefCore.getRef(_m_entryPointMono.entryPointId);
        }

        //初始化
        public void init()
        {
            //向UI展示窗口注册本对象
            GGUIWndHomeEntryFollow.instance.regInstance(_m_followInstance);
            
            if (null != _m_entryPointMono && _m_entryPointMono.clickMono != null) 
                _m_entryPointMono.clickMono.onClick += _onClick;

            _onInit();
        }

        //销毁
        public void discard()
        {
            GGUIWndHomeEntryFollow.instance.removeInstance(_m_followInstance);

            if (null != _m_entryPointMono && _m_entryPointMono.clickMono != null) 
                _m_entryPointMono.clickMono.onClick -= _onClick;

            _m_aOnLockClick = null;
            
            _discardInfoController();
            
            _onDiscard();
        }

        //刷新显示
        public void refreshShow()
        {
            //没有数据不需要跟随信息
            if(null == _m_entryPointRefObj || null == _m_entryPointMono)
            {
                _discardInfoController();
                return;
            }

            _showInfoController();

            //判断是否已解锁 并且 解锁表现是否已完成
            if ((null == _m_entryPointRefObj.unlock_condition || _m_entryPointRefObj.unlock_condition.IsEnable(null)) && NPPlayer.instance.funcUnlockComp.isFuncUnlockTipDone(_m_entryPointRefObj.func_unlock_type))
            {
                //已解锁状态
                _m_entryPointMono?.stateAniInfo?.sample(EEntryPointAniType.UNLOCK, 1);
            }
            else
            {
                //未解锁状态
                _m_entryPointMono?.stateAniInfo?.sample(EEntryPointAniType.LOCK, 1);
            }
            
            _onRefreshShow();
        }
        
        //隐藏显示
        public void hide()
        {
            _discardInfoController();
            
            _onHide();
        }

        /// <summary>
        /// 播放解锁动画
        /// </summary>
        /// <param name="_onPlayDone"></param>
        public void playUnlockAni(Action _onPlayDone)
        {
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(2);
            stepCounter.regAllDoneDelegate(_onPlayDone);

            if (_m_entryPointMono == null || _m_entryPointMono?.stateAniInfo == null)
                stepCounter.addDoneStepCount();
            else
                _m_entryPointMono.stateAniInfo.play(EEntryPointAniType.UNLOCKING_PROCESS, stepCounter.addDoneStepCount);

            if (_m_followControl == null)
                stepCounter.addDoneStepCount();
            else
                _m_followControl.playUnlockAni(stepCounter.addDoneStepCount);
        }

        /// <summary>
        /// 设置解锁动画状态
        /// </summary>
        /// <param name="_type"></param>
        /// <param name="_normalizeTime"></param>
        public void setUnlockAniSample(EEntryPointAniType _type, long _normalizeTime)
        {
            if (_m_entryPointMono != null && _m_entryPointMono.stateAniInfo != null)
                _m_entryPointMono.stateAniInfo.sample(EEntryPointAniType.UNLOCKING_PROCESS, _normalizeTime);

            if (_m_followControl != null)
                _m_followControl.setUnlockAniSample(_type, _normalizeTime);
        }

        /// <summary>
        /// 播放屏幕隐藏显示动画
        /// </summary>
        /// <param name="_onPlayDone"></param>
        public void playScreenShowHideAni(bool _isShow, Action _onPlayDone)
        {
            //这边只处理ui，td上应该不会有需求变化，要了再加
            if (null == _m_followControl)
            {
                if (_onPlayDone != null) 
                    _onPlayDone();
                return;
            }

            _m_followControl.playScreenShowHideAni(_isShow, _onPlayDone);
        }
        
        //点击td入口处理
        protected virtual void _onClick()
        {
            if(null == _m_entryPointRefObj)
                return;

            //判断解锁分别调用不同的effect
            if (null == _m_entryPointRefObj.unlock_condition || _m_entryPointRefObj.unlock_condition.IsEnable(null))
            {
                _m_entryPointRefObj.click_effect?.dealEffect();
            }
            else
            {
                if(_m_entryPointMono != null)
                    _m_aOnLockClick?.Invoke(_m_entryPointMono.followParent, _m_entryPointRefObj);
            }
        }
        
        
        //显示跟随信息
        private void _showInfoController()
        {
            if (null == _m_followControl)
            {
                _m_followControl = new T_FOLLOW_CONTROL();
                if(null != _m_entryPointRefObj && _m_entryPointRefObj.follow_item_ui_res_id != 0)
                    _m_followControl.initUIResId(_m_entryPointRefObj.follow_item_ui_res_id);
                GGUIWndHomeEntryFollow.instance.addController(_m_followInstance, _m_followControl);
            }

            _m_followControl.regItemWndLoadDoneDelegate(() =>
            {
                //刷新跟随UI窗口信息
                _onRefreshFollowControl(_m_followControl);
            });
        }
        
        //销毁跟随信息
        private void _discardInfoController()
        {
            if (null != _m_followControl)
            {
                _m_followControl.discard();
            }
            _m_followControl = null;
        }

        //初始化
        protected abstract void _onInit();
        //销毁
        protected abstract void _onDiscard();
        //刷新显示
        protected abstract void _onRefreshShow();
        //隐藏
        protected abstract void _onHide();
        //刷新跟随UI窗口信息
        protected abstract void _onRefreshFollowControl(T_FOLLOW_CONTROL _followControl);
    }
}