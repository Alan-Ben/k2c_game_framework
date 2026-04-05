using System;
using ALPackage;
using DG.Tweening;
using GOE.MiniGame;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GOE
{
    public class GGUIWndPuzzleGameMatchItem : _ANPGGUIBasicSubWnd<GGUIMonoPuzzleGameMatchItem>
    {
        private EPuzzleGameMatchItemState _m_eItemState;
        //DoTween动画对象
        private Tween _m_resumeItemPositionTween;
        
        public long matchItemId { get { return wnd == null ? 0 : wnd.matchItemId; } }
        public RectTransform matchRectTransform { get { return wnd == null ? null : wnd.matchRectTransform; } }
        public bool onMatchWrongNeedSetPosition { get { return wnd == null ? false : wnd.onMatchWrongNeedSetPosition; } }//当匹配错误时, 是否需要先将位置设置到错误的匹配位置

        /// <summary>
        /// item的世界坐标位置
        /// </summary>
        public Vector3 position
        {
            get { return rectTransform == null ? Vector3.zero : rectTransform.position; }
        }

        public event Action<GGUIWndPuzzleGameMatchItem> onItemPointDown;// 当item被鼠标点下时回调
        public event Action<GGUIWndPuzzleGameMatchItem, PointerEventData> onItemDrag;// 当item被拖动时回调
        public event Action<GGUIWndPuzzleGameMatchItem> onItemPointUp;// 当item被放下时回调
        
        public GGUIWndPuzzleGameMatchItem(GGUIMonoPuzzleGameMatchItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.opMask == null)
            {
                Debug.LogError_EditorOnly($"没有配置opMask, 可能会存在未知的操作错误, 请配置", wnd.gameObject);
            }
            
            ALUGUICommon.combinePointerDown(wnd.eventTriggerTarget, _onPointDown);
            ALUGUICommon.combineDrag(wnd.eventTriggerTarget, _onItemDrag);
            ALUGUICommon.combinePointerUp(wnd.eventTriggerTarget, _onPointUp);
        }
        
        protected override void _onDiscard()
        {
            onItemPointDown = null;
            onItemDrag = null;
            onItemPointUp = null;
            
            ALUGUICommon.uncombinePointerDown(wnd.eventTriggerTarget, _onPointDown);
            ALUGUICommon.uncombineDrag(wnd.eventTriggerTarget, _onItemDrag);
            ALUGUICommon.uncombinePointerUp(wnd.eventTriggerTarget, _onPointUp);
            
            _discardResumeItemPositionTween();
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _discardResumeItemPositionTween();
        }

        protected override void _onReset()
        {
        }

        /// <summary>
        /// 打开操作屏蔽对象
        /// </summary>
        public void openOpMask()
        {
            if(wnd != null)
                ALUGUICommon.setGameObjEnable(wnd.opMask, true);
        }

        /// <summary>
        /// 关闭操作屏蔽对象
        /// </summary>
        public void closeOpMask()
        {
            if(wnd != null)
                ALUGUICommon.setGameObjEnable(wnd.opMask, false);
        }
        
        public void setItemState(EPuzzleGameMatchItemState _state, bool _forceChg = false)
        {
            if (wnd == null || !isShow)
            {
                Debug.LogError_EditorOnly($"[GGUIWndPuzzleGameMatchItem] setGameState 在wnd == null或窗口未显示时, 就尝试设置窗口状态为:{_state}");
                return;
            }

            if (_m_eItemState == _state && !_forceChg)
                return;

            _m_eItemState = _state;
            wnd.setState(_m_eItemState);
        }

        #region 事件响应方法

        private void _onPointDown(Vector2 _vector2)
        {
            // 防止UI配置问题导致在其他状态还是能操作
            if (_m_eItemState != EPuzzleGameMatchItemState.IDLE)
            {
                Debug.LogError_EditorOnly($"[GGUIWndPuzzleGameMatchItem _onPointDown]当前item状态是:{_m_eItemState} 不处于IDLE状态, 不响应PointDown事件");
                return;
            }
            
            _discardResumeItemPositionTween();
            onItemPointDown?.Invoke(this);
        }
        
        /// <summary>
        /// 当item被拖动时
        /// </summary>
        private void _onItemDrag(PointerEventData _pointerEventData)
        {
            // 防止UI配置问题导致在其他状态还是能操作
            if (!(_m_eItemState is EPuzzleGameMatchItemState.IDLE or EPuzzleGameMatchItemState.NO_ITEM_WAITING_MATCH or
                    EPuzzleGameMatchItemState.RIGHT_ITEM_WAITING_MATCH or EPuzzleGameMatchItemState.WRONG_ITEM_WAITING_MATCH))
            {
                Debug.LogError_EditorOnly($"[GGUIWndPuzzleGameMatchItem _onItemDrag]当前item状态是:{_m_eItemState} 不处于IDLE或NO_ITEM_WAITING_MATCH或RIGHT_ITEM_WAITING_MATCH或WRONG_ITEM_WAITING_MATCH状态, 不响应Drag事件");
                return;
            }
            
            onItemDrag?.Invoke(this, _pointerEventData);
        }

        private void _onPointUp(Vector2 _vector2)
        {
            // 防止UI配置问题导致在其他状态还是能操作
            if (!(_m_eItemState is EPuzzleGameMatchItemState.IDLE or EPuzzleGameMatchItemState.NO_ITEM_WAITING_MATCH or
                    EPuzzleGameMatchItemState.RIGHT_ITEM_WAITING_MATCH or EPuzzleGameMatchItemState.WRONG_ITEM_WAITING_MATCH))
            {
                Debug.LogError_EditorOnly($"[GGUIWndPuzzleGameMatchItem _onPointUp]当前item状态是:{_m_eItemState} 不处于IDLE或NO_ITEM_WAITING_MATCH或RIGHT_ITEM_WAITING_MATCH或WRONG_ITEM_WAITING_MATCH状态, 不响应PointUp事件");
                return;
            }
            onItemPointUp?.Invoke(this);
        }

        #endregion
        
        public void moveTransformToLast()
        {
            if(wnd != null)
                ALUnityCommon.moveTransformToLast(rectTransform);
        }

        /// <summary>
        /// 设置item世界坐标
        /// </summary>
        public void setItemPosition(Vector3 _position)
        {
            if (rectTransform == null)
                return;
            
            rectTransform.position = _position;
        }

        /// <summary>
        /// 将item重置回初始位置
        /// </summary>
        public void resumeItemOriPosition(Vector3 _oriPosition, bool _needDelay, Func<long> _getSerialize, Action _onMoveDone)
        {
            if (wnd == null || rectTransform == null)
            {
                _onMoveDone?.Invoke();
                return;
            }

            if (_getSerialize == null)
            {
                Debug.LogError_EditorOnly("[GGUIWndPuzzleGameMatchItem] resumeItemOriPosition 必须带有一个获取SerializeId的方法");
                _onMoveDone?.Invoke();
                return;
            }
            
            long serialize = _getSerialize();

            Action itemStartFly = () =>
            {
                if(_getSerialize() != serialize)
                    return;
                
                if (wnd == null || rectTransform == null)
                {
                    _onMoveDone?.Invoke();
                    return;
                }
                
                _discardResumeItemPositionTween();
                
                _m_resumeItemPositionTween = rectTransform
                    .DOMove(_oriPosition, wnd.resumeItemPositionFlyTime)
                    .SetEase(Ease.Linear)
                    .OnKill(()=> setItemPosition(_oriPosition))
                    .OnComplete(() =>
                    {
                        if(_getSerialize() != serialize)
                            return;
                        
                        _onMoveDone?.Invoke();
                    });
            };
            
            if (!_needDelay || wnd.resumeItemPositionDelayTime <= 0)
            {
                itemStartFly();
            }
            else
            {
                CommonTaskController.CommonActionAddMonoTask(() =>
                {
                    if (_getSerialize() != serialize)
                        return;

                    itemStartFly();
                }, wnd.resumeItemPositionDelayTime);
            }
        }
        
        private void _discardResumeItemPositionTween()
        {
            if(_m_resumeItemPositionTween != null)
                _m_resumeItemPositionTween.Kill();
            _m_resumeItemPositionTween = null;
        }
    }
}