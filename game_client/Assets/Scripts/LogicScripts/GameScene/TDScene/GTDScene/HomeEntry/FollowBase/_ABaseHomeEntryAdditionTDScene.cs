using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 带功能入口的场景基类
    /// </summary>
    public abstract class _ABaseHomeEntryAdditionTDScene : _ABasicAdditionMainTDScene
    {
        //入口列表的mono
        protected GTDHomeEntryMono _m_homeEntryMono;
        //入口列表
        [NotNull] protected List<_IGTDHoneEntryPointView> _m_entryPointList = new List<_IGTDHoneEntryPointView>();
        //入口解锁描述字典列表
        [NotNull] protected Dictionary<long, EntryUnlockDescPointView> _m_entryUnlockDescPointViewDic = new Dictionary<long, EntryUnlockDescPointView>();
        //入口引导手指
        protected EntryGuideHandPointView _m_guideHand;

        //上次校验时间
        private long _m_lastCheckTime = 0;
        //当前是否再隐藏ui状态
        private bool _m_isHideUI = false;
        //任务刷新定时器
        private ALCommonEnableTaskController _m_tcTickTaskController;
        //展示完成回调
        private Action _m_aOnShowDone;

        protected sealed override void _onRootGOLoaded(GameObject _go)
        {
            if (null == _m_homeEntryMono && null != _go)
            {
                _m_homeEntryMono = _go.GetComponent<GTDHomeEntryMono>();
            }

            _onRootGOLoadedEx(_go);
        }

        //在初始化本场景完成后调用的事件函数
        protected sealed override void _onSceneInited()
        {
            if (null == _m_homeEntryMono || _m_homeEntryMono.entryPointList == null)
                return;

            foreach (_AGTDHomeEntryPointMono_Base homeEntryPointMono in _m_homeEntryMono.entryPointList)
            {
                if (null == homeEntryPointMono)
                    continue;

                _IGTDHoneEntryPointView entryPointView = EntryPointViewFactory.instance.createEntryPointView(homeEntryPointMono);
                if (null == entryPointView)
                    continue;

                entryPointView.init();
                entryPointView.onLockClick += _onLockClick;
                _m_entryPointList.Add(entryPointView);
            }

            _onSceneInitedEx();
        }

        //在退出本场景时调用的事件函数
        protected sealed override void _onQuitTDScene()
        {
            foreach (_IGTDHoneEntryPointView pointView in _m_entryPointList)
            {
                if (null == pointView)
                    continue;

                pointView.onLockClick -= _onLockClick;
                pointView.discard();
            }

            _m_entryPointList.Clear();

            _m_homeEntryMono = null;

            foreach (KeyValuePair<long, EntryUnlockDescPointView> entryUnlockDescPointView in _m_entryUnlockDescPointViewDic)
            {
                entryUnlockDescPointView.Value?.discard();
            }
            _m_entryUnlockDescPointViewDic.Clear();

            _m_guideHand?.discard();
            _m_guideHand = null;

            _onQuitTDSceneEx();
        }

        //显示场景时候调用
        protected sealed override void _dealShowSceneNP(Action _delegate)
        {
            WinMsg.RegisterMsg(WinMsgType.SET_SHOW_ENTRY_GUIDE_HAND, _setShowEntryGuideHand);//展示入口手指引导
            WinMsg.RegisterMsgAct(WinMsgType.ON_START_TUTORIAL, _onStartTutorial);//开始引导消息
            WinMsg.RegisterMsg(WinMsgType.ON_REFRESH_ENTRY_STATE, _onRefreshEntryState);//刷新入口状态
            WinMsg.RegisterMsg(WinMsgType.SCREEN_CLICK, _onScreenClick);//刷新入口状态
            WinMsg.RegisterMsg(WinMsgType.COMMON_INPUT_SCENE_START_DRAG, _onScreenClick);//刷新入口状态

            foreach (_IGTDHoneEntryPointView pointView in _m_entryPointList)
            {
                if (null == pointView)
                    continue;
                pointView.refreshShow();
            }

            _m_isHideUI = false;
            //每次显示场景时候更新校验时间
            _refreshCheckTime();
            
            _dealShowSceneEx(_delegate);
            
            //停止原先任务
            _m_tcTickTaskController.setDisable();
            //开启任务进行数据逻辑的处理
            _m_tcTickTaskController = ALCommonTaskController.CommonEnableDurationActionAddMonoTask(_tick1Sec, 0.5f);

            //展示完成回调
            _m_aOnShowDone?.Invoke();
            _m_aOnShowDone = null;
        }

        //隐藏场景时候调用
        protected sealed override void _dealHideSceneNP(Action _delegate)
        {
            WinMsg.UnregisterMsg(WinMsgType.SET_SHOW_ENTRY_GUIDE_HAND, _setShowEntryGuideHand);//展示入口手指引导
            WinMsg.UnregisterMsgAct(WinMsgType.ON_START_TUTORIAL, _onStartTutorial);//开始引导消息
            WinMsg.UnregisterMsg(WinMsgType.ON_REFRESH_ENTRY_STATE, _onRefreshEntryState);//刷新入口状态
            WinMsg.UnregisterMsg(WinMsgType.SCREEN_CLICK, _onScreenClick);//刷新入口状态
            WinMsg.UnregisterMsg(WinMsgType.COMMON_INPUT_SCENE_START_DRAG, _onScreenClick);//刷新入口状态

            foreach (_IGTDHoneEntryPointView pointView in _m_entryPointList)
            {
                if (null == pointView)
                    continue;
                pointView.hide();
            }

            foreach (KeyValuePair<long, EntryUnlockDescPointView> entryUnlockDescPointView in _m_entryUnlockDescPointViewDic)
            {
                entryUnlockDescPointView.Value?.hide();
            }

            _m_guideHand?.discard();
            _m_guideHand = null;

            _m_isHideUI = false;
            //每次隐藏场景时候更新校验时间
            _refreshCheckTime();
            
            //子类函数调用
            _dealHideSceneEx(_delegate);
            
            //停止原先任务
            _m_tcTickTaskController.setDisable();
        }

        /// <summary>
        /// 根据入口点id 获取对应位置
        /// </summary>
        /// <param name="_entryPointId"></param>
        /// <returns></returns>
        public Transform getEntryPointTransform(long _entryPointId)
        {
            if (null == _m_homeEntryMono || null == _m_homeEntryMono.entryPointList || _m_homeEntryMono.entryPointList.Count == 0)
                return null;

            _AGTDHomeEntryPointMono_Base temp = null;
            for (int i = 0;i < _m_homeEntryMono.entryPointList.Count; i ++)
            {
                temp = _m_homeEntryMono.entryPointList[i];
                if (null == temp || null == temp.clickMono)
                    continue;

                if (temp.entryPointId == _entryPointId)
                    return temp.clickMono.transform;
            }
            return null;
        }

        /// <summary>
        /// 获取入口点
        /// </summary>
        /// <param name="_entryPointId"></param>
        /// <returns></returns>
        public _IGTDHoneEntryPointView getEntryPointView(long _entryPointId)
        {
            for (int i = 0; i < _m_entryPointList.Count; i++)
            {
                if (_m_entryPointList[i] != null && _m_entryPointList[i].entryPointRefObj != null && _m_entryPointList[i].entryPointRefObj.id == _entryPointId)
                    return _m_entryPointList[i];
            }

            return null;
        }
        
        public void focusToEntryPoint(long _entryPointId)
        {
            Transform transform = getEntryPointTransform(_entryPointId);
            if (transform != null)
            {
                focusToTarget(transform.position, 0.5f);
                if (!Game.instance.isInTutorial)
                {
                    ALCommonActionMonoTask.addMonoTask(() =>
                    {
                        //如果不在引导并且条件通过，展示入口手指引导
                        if (!Game.instance.isInTutorial && GRefdataCoreMgr.instance.npGeneral.entrance_show_hand_guide_cond.IsEnable(null))
                            WinMsg.SendMsg(WinMsgType.SET_SHOW_ENTRY_GUIDE_HAND, _entryPointId);
                    }, 0.5f);
                }
            }
        }

        /// <summary>
        /// 注册显示完成回调
        /// </summary>
        /// <param name="_action"></param>
        public void regShowDone(Action _action)
        {
            if (_action == null)
                return;

            if (isShow)
                _action();
            else
            {
                if (_m_aOnShowDone == null)
                    _m_aOnShowDone = _action;
                else
                    _m_aOnShowDone += _action;
            }
        }

        //展示引导的手
        protected void showGuideHand(Transform _transform, long _resId)
        {
            if (_transform == null || _resId <= 0)
                return;

            //只展示一个，先清空之前的
            if (_m_guideHand != null)
                _m_guideHand.discard();
            else
                _m_guideHand = new EntryGuideHandPointView();

            _m_guideHand.init(_transform, _resId);
            _m_guideHand.refreshShow();
        }

        //刷新
        private void _doCheck()
        {
            foreach (_IGTDHoneEntryPointView pointView in _m_entryPointList)
            {
                if (null == pointView)
                    continue;
                pointView.refreshShow();
            }
        }

        /// <summary>
        /// 展示入口手指引导
        /// </summary>
        /// <param name="_objects"></param>
        private void _setShowEntryGuideHand(params object[] _objects)
        {
            if (_objects == null || _objects.Length == 0 || _objects[0] == null)
                return;

            long entryPointId = (long) _objects[0];

            EntryPointRefObj entryPointRef = GRefdataCoreMgr.instance.entryPointRefCore.getRef(entryPointId);
            Transform entryPointTransform = getEntryPointTransform(entryPointId);
            if (entryPointTransform == null || entryPointRef == null)
                return;

            showGuideHand(entryPointTransform, entryPointRef.guide_hand_ui_res_id);
        }

        /// <summary>
        /// 开始引导消息
        /// </summary>
        private void _onStartTutorial()
        {
            //开始引导需要隐藏引导的手
            if (_m_guideHand != null)
                _m_guideHand.discard();
        }

        /// <summary>
        /// 刷新入口状态
        /// </summary>
        private void _onRefreshEntryState(params object[] _objects)
        {
            if (_objects == null || _objects.Length == 0)
                return;

            ENPFunctionType type = (ENPFunctionType)_objects[0];
            for (int i = 0; i < _m_entryPointList.Count; i++)
            {
                if (_m_entryPointList[i] != null &&
                    _m_entryPointList[i].entryPointRefObj != null &&
                    _m_entryPointList[i].entryPointRefObj.func_unlock_type == type)
                {
                    _m_entryPointList[i].refreshShow();
                    break;
                }
            }
        }

        /// <summary>
        /// 入口未解锁时点击回调
        /// </summary>
        /// <param name="_followParent"></param>
        /// <param name="_entryPointRef"></param>
        private void _onLockClick(Transform _followParent, EntryPointRefObj _entryPointRef)
        {
            if (_entryPointRef == null)
                return;

            EntryUnlockDescPointView targetPointView = null;
            if (_m_entryUnlockDescPointViewDic.TryGetValue(_entryPointRef.id, out targetPointView))
            {
                targetPointView?.init(_followParent, _entryPointRef);
                targetPointView?.refreshShow();
            }
            else
            {
                targetPointView = new EntryUnlockDescPointView();
                targetPointView.init(_followParent, _entryPointRef);
                targetPointView.refreshShow();
                _m_entryUnlockDescPointViewDic[_entryPointRef.id] = targetPointView;
            }

            //隐藏其他的
            foreach (KeyValuePair<long, EntryUnlockDescPointView> entryUnlockDescPointView in _m_entryUnlockDescPointViewDic)
            {
                if(entryUnlockDescPointView.Key != _entryPointRef.id)
                    entryUnlockDescPointView.Value?.discard();
            }
        }

        //每秒任务
        private void _tick1Sec()
        {
            if(!_isNeedCheckScreenClickAni)
                return;
            
            //显示状态下超过一定时间没有点击屏幕，显示屏幕隐藏动画
            if (!_m_isHideUI && TimeUtil.getTimeStampSec() - _m_lastCheckTime > GRefdataCoreMgr.instance.npGeneral?.check_city_screen_click_hide_ani_time_sec)
            {
                foreach (_IGTDHoneEntryPointView gtdHoneEntryPointView in _m_entryPointList)
                {
                    if(null == gtdHoneEntryPointView)
                        continue;
                    gtdHoneEntryPointView.playScreenShowHideAni(false, null);;
                }

                _m_isHideUI = true;
            }
        }
        
        /// <summary>
        /// 屏幕点击
        /// </summary>
        /// <param name="_objects"></param>
        private void _onScreenClick(params object[] _objects)
        {
            //每次点击屏幕更新校验时间
            _refreshCheckTime();

            //如果是隐藏状态下点击屏幕，显示屏幕显示动画
            if (_m_isHideUI)
            {
                foreach (_IGTDHoneEntryPointView gtdHoneEntryPointView in _m_entryPointList)
                {
                    if(null == gtdHoneEntryPointView)
                        continue;
                    gtdHoneEntryPointView.playScreenShowHideAni(true, null);;
                }

                _m_isHideUI = false;
            }
        }
        
        //更新校验时间
        private void _refreshCheckTime()
        {
            _m_lastCheckTime = TimeUtil.getTimeStampSec();
        }
        
        //重置屏幕点击隐藏动画
        private void _resetScreenClickAni()
        {
            foreach (_IGTDHoneEntryPointView gtdHoneEntryPointView in _m_entryPointList)
            {
                if(null == gtdHoneEntryPointView)
                    continue;
                gtdHoneEntryPointView.playScreenShowHideAni(true, null);;
            }
        }
        

        //是否需要校验显示屏幕点击动画
        protected virtual bool _isNeedCheckScreenClickAni { get { return false; } }
        
        //对加载的场景每一个rootGo进行处理
        protected abstract void _onRootGOLoadedEx(GameObject _go);
        // 显示本scene对象的操作，显示完成则调用回调
        protected abstract void _dealShowSceneEx(Action _delegate);
        // 隐藏本scene对象的操作
        protected abstract void _dealHideSceneEx(Action _delegate);
        //在初始化本场景完成后调用的事件函数
        protected abstract void _onSceneInitedEx();
        //在退出本场景时调用的事件函数
        protected abstract void _onQuitTDSceneEx();


    }
}