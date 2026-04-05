using System;
using System.Collections.Generic;
using System.Linq;
using ALPackage;
using JetBrains.Annotations;
using NPEnum;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GOE
{
    /// <summary>
    /// 支持左右拖拽的showcase，需要一次性加载3个单位跟背景
    /// </summary>
    public partial class _NGGUIWndCommonShowCase_LeftRightMove<T_ITEM> : _ANPGGUIWndCommonShowCase<GGUIMonoCommonShowCase_LeftRightMove>
        where T_ITEM : _IShowcaseLeftRightMove
    {
        //是否初始化
        private bool _m_isInit;
        //序列号
        private int _m_refreshSerialize = -1;
        //数据类
        private LeftRightMoveData _m_data;
        //数据列表
        [NotNull]private List<T_ITEM> _m_itemList = new List<T_ITEM>();
        //当前选中的index
        private int _m_curSelectIndex;
        
        //状态机
        [NotNull] private MoveShowcaseStateMachine _m_stateMachine;
        // 刷新任务
        private ALCommonEnableTaskController _m_tickTask;
        //移动状态序列号
        private long _m_moveStateSerialize;
        //每次拖拽总的移动水平屏幕距离
        private float _m_totalDeltaMoveX;

        //显示单位变化事件
        public event Action<int> onShowItemChg;
        //点击item事件
        public event Action<_AShowCaseUnitInfoObj> onClickItem;
        //开始拖拽事件
        public event Action onBeginDrag;

        
        public _NGGUIWndCommonShowCase_LeftRightMove(GGUIMonoCommonShowCase_LeftRightMove _wnd) : base(_wnd)
        {
            _m_isInit = false;
            _m_curSelectIndex = 0;
        }
        
        public new void showWnd(_AShowCaseUnitInfoObj _unit, NPGShowcaseIndex _templateIndex = null)
        {
            UnityEngine.Debug.LogError($"左右滑动的showcase不支持这种重载方式，需要调用initShowWnd初始化");
        }

        public new void showWnd(_AShowCaseUnitInfoObj[] _unitList, NPGShowcaseIndex _templateIndex = null)
        {
            UnityEngine.Debug.LogError($"左右滑动的showcase不支持这种重载方式，需要调用initShowWnd初始化");
        }

        /// <summary>
        /// 初始化showcase
        /// </summary>
        public void initShowWnd(List<T_ITEM> _itemList, int _defaultSelect = 0, Action _doneAction = null)
        {
            if(_m_isInit || null == _itemList)
                return;
            base.showWnd(new _AShowCaseUnitInfoObj[]{}, null);
            
            //初始化showcase
            _m_refreshSerialize = ALSerializeOpMgr.next();
            int refreshSerialize = _m_refreshSerialize;
            
            //添加数据
            _m_itemList.AddRange(_itemList);
            
            regInitDoneDelegate(() =>
            {
                if(null == wnd)
                    return;
                
                if (refreshSerialize != _m_refreshSerialize)
                    return;
                
                _m_isInit = true;
                
                if (null != _m_showcaseInfo && _m_showcaseInfo.unitIndexCount < 4)
                    UnityEngine.Debug.LogError($"支持左右拖拽的showcase舞台需要4个挂点，目前只有{_m_showcaseInfo.unitIndexCount}个");
                
                //初始化数据类
                _m_data = new LeftRightMoveData(0, 1, 2, wnd.nextOffsetDistance, wnd.moveTimeS);
                _m_data.initData(_m_showcaseInfo);
                //初始化状态机
                _m_stateMachine = new MoveShowcaseStateMachine(new MoveShowcaseStateFactory(this));
                
                // 开始刷新任务
                _m_tickTask.setDisable();
                _m_tickTask = ALCommonEnableTickActionMonoTask.addMonoTask(_tick, 0);
                
                //刷新显示
                refreshSelectIndex(_defaultSelect, true);

                //执行回调
                if (_doneAction != null) 
                    _doneAction();
            });
        }
        
        //刷新显示
        public void refreshSelectIndex(int _selectIndex, bool _needRefreshCenter = false)
        {
            if(!_m_isInit || null == _m_data)
                return;

            if(_m_itemList.Count <= _selectIndex || _selectIndex < 0)
                return;

            _m_curSelectIndex = _selectIndex;
            
            //左边单位
            if(_m_itemList.Count > _m_curSelectIndex - 1 && _m_curSelectIndex - 1 >= 0)
                _m_data.left.setInfoObj(new ShowCaseCommonResUnitInfoObj(_m_itemList[_m_curSelectIndex - 1].unitIndex));
            else
                _m_data.left.setInfoObj(null);

            //右边单位
            if(_m_itemList.Count > _m_curSelectIndex + 1)
                _m_data.right.setInfoObj(new ShowCaseCommonResUnitInfoObj(_m_itemList[_m_curSelectIndex + 1].unitIndex));
            else
                _m_data.right.setInfoObj(null);

            //中间单位
            if (_needRefreshCenter)
            {
                ShowCaseCommonResUnitInfoObj centerUnitInfoObj = new ShowCaseCommonResUnitInfoObj(_m_itemList[_m_curSelectIndex].unitIndex);
                _m_data.center.setInfoObj(centerUnitInfoObj);
            }
            
            if(null != _m_itemList[_m_curSelectIndex].bgIndex)
                _m_data.bg = new ShowCaseCommonResUnitInfoObj(_m_itemList[_m_curSelectIndex].bgIndex);
            
            //切换到重置状态
            _m_stateMachine.setState<MoveShowcaseState_RESET>((item) =>
            {
                if (item != null) 
                    item.setInfo(_needRefreshCenter);
            });
            
            //触发回调
            onShowItemChg?.Invoke(_m_curSelectIndex);
        }

        //移动到上一个
        public void moveToLast()
        {
            if(_m_curSelectIndex == 0)
                return;
            
            _m_totalDeltaMoveX = +Screen.width;
            
            //切换到回弹状态
            _m_stateMachine.changeState<MoveShowcaseState_SPRING>((_state) =>
            {
                if (_state != null) 
                    _state.setTotalDeltaMoveX(_m_totalDeltaMoveX);
            });
        }
        
        //移动到下一个
        public void moveToNext()
        {
            if(_m_curSelectIndex >= _m_itemList.Count - 1)
                return;
            
            _m_totalDeltaMoveX = -Screen.width;
            
            //切换到回弹状态
            _m_stateMachine.changeState<MoveShowcaseState_SPRING>((_state) =>
            {
                if (_state != null) 
                    _state.setTotalDeltaMoveX(_m_totalDeltaMoveX);
            });
        }

        /// <summary>
        /// 刷新背景
        /// </summary>
        public void refreshCenterBg()
        {
            if (_m_data == null || _m_itemList.Count <= _m_curSelectIndex)
                return;

            if (null != _m_itemList[_m_curSelectIndex].bgIndex)
                _m_data.bg = new ShowCaseCommonResUnitInfoObj(_m_itemList[_m_curSelectIndex].bgIndex);

            _m_stateMachine.setState<MoveShowcaseState_RESET>((item) =>
            {
                if (item != null)
                    item.setInfo(false);
            });
        }

        /// <summary>
        /// 中间当前显示对象播放动画
        /// </summary>
        /// <param name="_aniName"></param>
        public void playCenterAni(string _aniName)
        {
            if(null == _m_data || null == _m_data.center || null == _m_data.center.infoObj)
                return;
            
            _m_data.center.infoObj.playAnim(_aniName);
        }

        /// <summary>
        /// 中间当前显示对象播放特效
        /// </summary>
        /// <param name="_sfxId"></param>
        public void playCenterSfx(long _sfxId)
        {
            if (null == _m_data || null == _m_data.center || null == _m_data.center.infoObj)
                return;

            _m_data.center.infoObj.playSfx(_sfxId);
        }
        
        protected override void _onShowWndEx()
        {
        }

        protected override void _onHideWndEx()
        {
            _clearAll();
        }

        protected override void _onResetEx()
        {
            _clearAll();
        }

        protected override void _onDiscardEx()
        {
            _clearAll();
        }

        protected override void _onWndInitDoneEx()
        {
            _m_isInit = false;
        }

        private void _clearAll()
        {
            _m_refreshSerialize = ALSerializeOpMgr.next();
            _m_moveStateSerialize = ALSerializeOpMgr.next();
            
            _m_isInit = false;
            _m_data = null;
            _m_tickTask.setDisable();
            _m_stateMachine?.setState(typeof(MoveShowcaseState_NONE));
            _m_itemList?.Clear();
            _m_curSelectIndex = 0;
        }
        
        /// <summary>
        /// showcase开始拖拽事件
        /// </summary>
        /// <param name="_pointData"></param>
        protected override void _onBeginDragShowcaseEx(PointerEventData _pointData)
        {
            _m_totalDeltaMoveX = 0;
            //切换到重置状态
            _m_stateMachine.changeState<MoveShowcaseState_MOVE>((_state) =>
            {
                
            });
            
            _m_moveStateSerialize = _m_stateMachine.curState.enterSerialize;

            onBeginDrag?.Invoke();
        }

        /// <summary>
        /// showcase拖拽事件
        /// </summary>
        /// <param name="_pointData"></param>
        protected override void _onDragShowcaseEx(PointerEventData _pointData)
        {
            if(null == _pointData)
                return;
            //总滑动距离累加
            _m_totalDeltaMoveX += _pointData.delta.x;
            
            //设置数据
            _m_stateMachine.checkAndSetStateData<MoveShowcaseState_MOVE>(_m_moveStateSerialize, (_state) =>
            {
                if (_state != null) 
                    _state.onDrag(_pointData.delta.x);
            });
        }

        /// <summary>
        /// showcase停止拖拽事件
        /// </summary>
        /// <param name="_pointData"></param>
        protected override void _onEndDragShowcaseEx(PointerEventData _pointData)
        {
            //切换到回弹状态
            _m_stateMachine.changeState<MoveShowcaseState_SPRING>((_state) =>
            {
                if (_state != null) 
                    _state.setTotalDeltaMoveX(_m_totalDeltaMoveX);
            });
            
            _m_moveStateSerialize = ALSerializeOpMgr.next();
        }

        /// <summary>
        /// 点击反馈
        /// </summary>
        /// <param name="_gameObject"></param>
        protected override void _onClickShowcaseEx(GameObject _gameObject)
        {
            if(null == _m_data)
                return;
            
            onClickItem?.Invoke(_m_data.center.infoObj);
        }

        private void _tick()
        {
            _m_stateMachine.tick(Time.deltaTime);
        }
    }
}