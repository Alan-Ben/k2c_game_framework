using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using static UnityEngine.EventSystems.ExecuteEvents;

namespace GOE
{
    //将点击事件穿透到下一层, 挂载此脚本前必须挂载ALUGUIEventTriggerListener
    public class NPGUIMonoPassEvent : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
    {
        //是否需要优先触发自己
        [ALHeader("是否需要优先触发自己身上的点击事件")]
        public bool priorityTriggerSelf = false;
        
        //监听按下
        public void OnPointerDown(PointerEventData eventData)
        {
            PassEvent(eventData, ExecuteEvents.pointerDownHandler);
        }

        //监听抬起
        public void OnPointerUp(PointerEventData eventData)
        {
            PassEvent(eventData, ExecuteEvents.pointerUpHandler);
        }

        private bool _m_reenter = false;
        //监听点击
        public void OnPointerClick(PointerEventData eventData)
        {
            if (priorityTriggerSelf)
            {
                if (!_m_reenter)
                {
                    // 1) 先触发自己（包括同物体上的 Button 等 IPointerClickHandler）
                    _m_reenter = true;
                    ExecuteEvents.Execute<IPointerClickHandler>(gameObject, eventData, ExecuteEvents.pointerClickHandler);
                    _m_reenter = false;
                    ALUGUIEventTriggerListener.g_resetLastClickFrame();
                    
                    //触发穿透点击
                    PassEvent(eventData, ExecuteEvents.pointerClickHandler);
                }
            }
            else
            {
                // 2) 直接穿透点击事件
                PassEvent(eventData, ExecuteEvents.pointerClickHandler, true);
            }
        }


        //把事件透下去
        //TODO 底层Listener有点击间隔_g_lastClickFrame控制，穿透不支持点击穿透，可以用来点击和拖拽
        public GameObject PassEvent<T>(PointerEventData data, ExecuteEvents.EventFunction<T> function, bool _resetLastClick = false)
            where T : IEventSystemHandler
        {
            //            UnityEngine.Debug.LogError($"======={gameObject.name}");

            bool trigger = false;
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(data, results);
            //            GameObject current = data.pointerCurrentRaycast.gameObject;
            for (int i = 0; i < results.Count; i++)
            {
                if (gameObject != results[i].gameObject)
                {
                    //                    UnityEngine.Debug.LogError($"-------{results[i].gameObject.name}");
                    //下一个捕获到事件的需要进行处理
                    if (trigger)
                    {
                        //实际执行事件的处理，需要向父节点查询任意能处理该事件的节点进行处理，不论是否处理捕获成功都不会继续判断下一个
                        _executeEvents(results[i].gameObject, data, function);

                        //重置点击帧判断，保证事件可执行
                        if (_resetLastClick)
                            ALUGUIEventTriggerListener.g_resetLastClickFrame();

                        //RaycastAll后ugui会自己排序，如果你只想响应透下去的最近的一个响应，这里ExecuteEvents.Execute后直接break就行。
                        return results[i].gameObject;
                    }
                }
                else
                {
                    trigger = true;
                }
            }

            return null;
        }

        /// <summary>
        /// 循环对父节点尝试执行事件，直至执行完成
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_castGo"></param>
        /// <param name="eventData"></param>
        /// <param name="functor"></param>
        protected void _executeEvents<T>(GameObject _castGo, BaseEventData eventData, EventFunction<T> functor)
            where T : IEventSystemHandler
        {
            if (null == _castGo)
                return;

            //不断获取往上层寻找处理的父节点
            Transform dealTrans = _castGo.transform;

            //递归处理
            do
            {
                if (null == dealTrans || null == dealTrans.gameObject)
                    break;

                //尝试处理事件
                if(ExecuteEvents.Execute<T>(dealTrans.gameObject, eventData, functor))
                {
                    //处理成功，结束
                    break;
                }

                //获取父节点
                dealTrans = dealTrans.parent;
            } while (true);
        }
    }


}
