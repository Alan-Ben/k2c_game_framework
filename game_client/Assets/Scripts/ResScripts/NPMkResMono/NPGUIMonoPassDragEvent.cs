using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

namespace GOE
{
    //将点击事件穿透到下一层, 挂载此脚本前必须挂载ALUGUIEventTriggerListener
    public class NPGUIMonoPassDragEvent : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IMoveHandler
    {
        private GameObject _m_gDragGo;

        public void OnBeginDrag(PointerEventData _EventData)
        {
            _m_gDragGo = PassEvent(_EventData, ExecuteEvents.beginDragHandler);
        }

        public void OnDrag(PointerEventData _EventData)
        {
            PassDragEvent(_EventData, ExecuteEvents.dragHandler);
        }

        public void OnEndDrag(PointerEventData _EventData)
        {
            PassDragEvent(_EventData, ExecuteEvents.endDragHandler);

            //重置变量
            _m_gDragGo = null;
        }

        public void OnMove(AxisEventData eventData)
        {
            PassEvent(eventData, ExecuteEvents.moveHandler);
        }


        //把事件透下去
        //TODO 底层Listener有点击间隔_g_lastClickFrame控制，穿透不支持点击穿透，可以用来点击和拖拽
        public GameObject PassEvent<T>(PointerEventData data, ExecuteEvents.EventFunction<T> function)
            where T : IEventSystemHandler
        {
            //            UnityEngine.Debug.LogError($"======={gameObject.name}");

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(data, results);
            //            GameObject current = data.pointerCurrentRaycast.gameObject;
            for (int i = 0; i < results.Count; i++)
            {
                if (gameObject != results[i].gameObject)
                {
                    //                    UnityEngine.Debug.LogError($"-------{results[i].gameObject.name}");
                    if (ExecuteEvents.Execute(results[i].gameObject, data, function))
                    {
                        //RaycastAll后ugui会自己排序，如果你只想响应透下去的最近的一个响应，这里ExecuteEvents.Execute后直接break就行。
                        return results[i].gameObject;
                    }
                }
            }

            return null;
        }
        public void PassDragEvent<T>(PointerEventData data, ExecuteEvents.EventFunction<T> function)
            where T : IEventSystemHandler
        {
            if (null == _m_gDragGo)
                return;
            //            UnityEngine.Debug.LogError($"======={gameObject.name}");

            ExecuteEvents.Execute(_m_gDragGo, data, function);
        }

        public void PassEvent<T>(AxisEventData data, ExecuteEvents.EventFunction<T> function)
            where T : IEventSystemHandler
        {
            ExecuteEvents.Execute(data.selectedObject, data, function);
        }
    }


}
