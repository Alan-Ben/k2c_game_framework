using ALPackage;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 场景中对象基础mono
    /// </summary>
    public class NPScrollRect : ScrollRectEx
    {
        [ALHeader("是否在拖拽过程中是高帧率模式（默认true）")]
        public bool isDragHighFrame = true;

        public override void OnPointerDown(PointerEventData _eventData)
        {
            base.OnPointerDown(_eventData);
#if NP_GAME
            //开启高帧率模式
            if (isDragHighFrame)
                FrameRateController.instance.setHighFrameOpen();

            if (_AALMonoMain.instance.showDebugOutput)
            {
                Debug.Log($"【{Time.frameCount}】[ScrollRect]{gameObject.name} OnPointerDown [isDraging:{isDraging}] [contentPos:{content.anchoredPosition}]\neventData:{_eventData}");
            }
#endif
        }

        //
        // 摘要:
        //     Handling for when the content has finished being dragged.
        //
        // 参数:
        //   eventData:
        public override void OnPointerUp(PointerEventData _eventData)
        {
            base.OnPointerUp(_eventData);

#if NP_GAME
            //开启监控判断是否需要恢复正常帧率状态
            if (isDragHighFrame && !isDraging) // 如果没有在拖动则在这里取消高帧率
                FrameRateController.instance.setHighFrameOpenAndMonitGo(content);

            if (_AALMonoMain.instance.showDebugOutput)
            {
                Debug.Log($"【{Time.frameCount}】[ScrollRect]{gameObject.name} OnPointer [isDraging:{isDraging}] [contentPos:{content.anchoredPosition}]\neventData:{_eventData}");
            }
#endif
        }

        public override void OnRealEndDrag(PointerEventData _eventData)
        {
            base.OnRealEndDrag(_eventData);

#if NP_GAME
            //开启监控判断是否需要恢复正常帧率状态
            if (isDragHighFrame) // 如果没有在拖动则在这里取消高帧率
                FrameRateController.instance.setHighFrameOpenAndMonitGo(content);

            if (_AALMonoMain.instance.showDebugOutput)
            {
                Debug.Log($"【{Time.frameCount}】[ScrollRect]{gameObject.name} OnRealEndDrag [isDraging:{isDraging}] [contentPos:{content.anchoredPosition}]\neventData:{_eventData}");
            }
#endif
        }
    }
}
