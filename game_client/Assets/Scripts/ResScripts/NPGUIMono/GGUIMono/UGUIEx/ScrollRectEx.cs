using ALPackage;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GOE
{
    public class ScrollRectEx : ScrollRect, IPointerDownHandler, IPointerUpHandler
    {
        private int _m_onPointerDownFrame;
        private Vector2 _m_lastFrameAnchorPosition;

        private bool _m_bIsDraging;

        //结束拖拽时的处理函数
        private Action<PointerEventData> _m_dOnStartDragDelegate;
        //结束拖拽时的处理函数
        private Action<PointerEventData> _m_dOnEndDragDelegate;
        //拖拽时的处理函数
        private Action<PointerEventData, Action> _m_dOnDragDelegate;


        public bool isDraging { get { return _m_bIsDraging; } }

        public Action<PointerEventData> onStartDragDelegate { get { return _m_dOnStartDragDelegate; } set { _m_dOnStartDragDelegate = value; } }
        public Action<PointerEventData> onEndDragDelegate { get { return _m_dOnEndDragDelegate; } set { _m_dOnEndDragDelegate = value; } }
        
        /// <summary> TODO:暂时无效，目前有问题，重载OnDrag后无法拖动 </summary>
        public Action<PointerEventData, Action> onDragDelegate { get { return _m_dOnDragDelegate; } set { _m_dOnDragDelegate = value; } }

        
        //public override void OnDrag(PointerEventData eventData)
        //{
        //    //调用拖拽函数
        //    if (null != _m_dOnDragDelegate)
        //        _m_dOnDragDelegate(eventData, delegate () { base.OnDrag(eventData); });
        //}

        public virtual void OnRealBeginDrag(PointerEventData eventData)
        {
            //调用结束拖拽函数
            if (null != _m_dOnStartDragDelegate)
                _m_dOnStartDragDelegate(eventData);

            m_ContentStartPosition = content.anchoredPosition;
        }

        public virtual void OnRealEndDrag(PointerEventData eventData)
        {
            //调用结束拖拽函数
            if (null != _m_dOnEndDragDelegate)
                _m_dOnEndDragDelegate(eventData);
        }


        private static bool ShouldStartDrag(Vector2 pressPos, Vector2 currentPos, float threshold, bool useDragThreshold)
        {
            if (!useDragThreshold)
                return true;
            return (double)(pressPos - currentPos).sqrMagnitude >= (double)threshold * (double)threshold;
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            if (_m_bIsDraging)
                return;

            //            Debug.Log($"{Time.frameCount} OnPointerDown {content.anchoredPosition}  Press{eventData.pressPosition}  \n\n{eventData}");
            //            eventData.delta = TestTouch._m_touch.deltaPosition;
            _m_onPointerDownFrame = Time.frameCount;
            //            if (_AALMonoMain.instance != null && _AALMonoMain.instance.showDebugOutput)
            //            {
            //                Debug.Log($"【{Time.frameCount}】[ScrollRectFixed]{gameObject.name} OnPointerDown [anchorPos:{content.anchoredPosition}] [isDraging:{isDraging}]  [pointDownFrame:{_m_onPointerDownFrame}]\neventData:{eventData}");
            //            }
            base.OnBeginDrag(eventData);
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            //            if (_AALMonoMain.instance != null && _AALMonoMain.instance.showDebugOutput)
            //            {
            //                Debug.Log($"【{Time.frameCount}】[ScrollRectFixed]{gameObject.name} OnPointerUp [anchorPos:{content.anchoredPosition}] [isDraging:{isDraging}]  [pointDownFrame:{_m_onPointerDownFrame}]\neventData:{eventData}");
            //            }
            //            Debug.Log($"{Time.frameCount} OnPointerUp {content.anchoredPosition} Press{eventData.pressPosition}    \n\n{eventData}");
            //OnPointerUp的那一帧不会触发OnDrag，也不应该触发OnDrag，这里是为了解决只接触2帧时，没有速度，导致拖不动（当上一帧没有
            if (Time.frameCount == _m_onPointerDownFrame + 1)
            {
                if (ShouldStartDrag(eventData.pressPosition, eventData.position, (float)EventSystem.current.pixelDragThreshold, eventData.useDragThreshold))
                {
                    //                    //说明是第二种情况，在这里手动触发一次onDrag的计算
                    //                    OnDrag(eventData);
                    //                    //调用OnEndDrag结束拖拽
                    //                    OnEndDrag(eventData);

                    //                    Vector2 originVelocity = base.velocity;
                    //_m_hasDoBeginDrag = false;    
                    //_m_hasDoBeginDrag = true;
                    Vector3 newVelocity = (content.anchoredPosition - _m_lastFrameAnchorPosition) / Time.unscaledDeltaTime;
                    base.velocity = Vector3.Lerp(base.velocity, newVelocity, Time.unscaledDeltaTime * 10);
                    //                    Debug.Log($"Origin velocity:{originVelocity}    new:{base.velocity}");
                }
            }

            // 如果并没有在原生拖动，就在这里执行OnEndDrag
            if (!_m_bIsDraging)
                base.OnEndDrag(eventData);
        }

        public sealed override void OnBeginDrag(PointerEventData eventData)
        {
            //            if (_AALMonoMain.instance != null && _AALMonoMain.instance.showDebugOutput)
            //            {
            //                Debug.Log($"【{Time.frameCount}】[ScrollRectFixed]{gameObject.name} OnBeginDrag [anchorPos:{content.anchoredPosition}] [isDraging:{isDraging}] [pointDownFrame:{_m_onPointerDownFrame}]\neventData:{eventData}");
            //            }
            base.OnBeginDrag(eventData);
            //            Debug.Log($"{Time.frameCount} {content.anchoredPosition} OnBeginDrag\n\n{eventData}");
            //为了把OnBeginDrag提前一帧，在OnPointerDown触发
            //                Debug.Log($"{Time.frameCount} 基类OnBeginDrag  \n\n{eventData}");
            //                if(TestTouch.hasTouch && TestTouch._m_touch.deltaPosition.sqrMagnitude > 0.1f)
            //                {
            //                    Debug.Log($"{Time.frameCount} 基类OnBeginDrag有deltaPosition！！！！！！！  {TestTouch._m_touch.deltaPosition}\n\n{eventData}");
            //                    //不知道为什么，第一帧OnBeginDrag的时候，deltaPosition有时候会有值
            //                    Vector2 thisFramePosition = eventData.position;
            //                    eventData.position -= TestTouch._m_touch.deltaPosition;//减回上一帧位置
            //                    base.OnBeginDrag(eventData);
            //                    eventData.position = thisFramePosition;
            //                    OnDrag(eventData);
            //                }
            //                else
            //                {
            //                }
            //                Debug.Log($"{Time.frameCount} 调用完后 {content.anchoredPosition}");
            OnRealBeginDrag(eventData);
            _m_bIsDraging = true;
        }

        public sealed override void OnEndDrag(PointerEventData eventData)
        {
            //            if (_AALMonoMain.instance != null && _AALMonoMain.instance.showDebugOutput)
            //            {
            //                Debug.Log($"【{Time.frameCount}】[ScrollRectFixed]{gameObject.name} OnEndDrag [anchorPos:{content.anchoredPosition}] [isDraging:{isDraging}]  [pointDownFrame:{_m_onPointerDownFrame}]\neventData:{eventData}");
            //            }
            //            Debug.Log($"{Time.frameCount}  {content.anchoredPosition} OnEndDrag  \n\n{eventData}");
            base.OnEndDrag(eventData);
            OnRealEndDrag(eventData);
            _m_bIsDraging = false;
        }
        //
        //        public override void OnDrag(PointerEventData eventData)
        //        {
        ////            Debug.Log($"{Time.frameCount}  {content.anchoredPosition} OnDrag \n\n{eventData}");
        //            processOnDragThisFrame = true;
        //            base.OnDrag(eventData);
        //        }
        //
        //        protected override void SetContentAnchoredPosition(Vector2 position)
        //        {
        ////            var origin = content.anchoredPosition;
        //            base.SetContentAnchoredPosition(position);
        ////            Debug.Log($"{Time.frameCount}  SetContentAnchoredPosition{origin} -> {content.anchoredPosition} : {(content.anchoredPosition - origin).magnitude / Time.unscaledDeltaTime}");
        //        }

        protected override void LateUpdate()
        {
            //            if(_m_hasDoBeginDrag)
            //            {
            ////                Debug.Log($"{Time.frameCount}  LateUpdate {content.anchoredPosition}  \n\n");
            //            }
            base.LateUpdate();
            //            processOnDragLastFrame = processOnDragThisFrame;
            //            processOnDragThisFrame = false;
            _m_lastFrameAnchorPosition = base.content.anchoredPosition;

            //            if(_m_hasDoBeginDrag)
            //            {
            //                Debug.Log($"{Time.frameCount}  After LateUpdate {content.anchoredPosition}\n\n");
            //            }
        }
    }
}