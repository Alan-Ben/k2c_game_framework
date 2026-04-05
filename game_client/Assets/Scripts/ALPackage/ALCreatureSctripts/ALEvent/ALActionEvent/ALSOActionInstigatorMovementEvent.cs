using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

#if AL_CREATURE_SYS
namespace ALPackage
{
    [System.Serializable]
    public class ALSOActionInstigatorMovementEvent : _AALSOBaseActionEvent, _IActionMovementListener
    {
        /** 进入此操作时的增加Operation队列 */
        public List<ALSOBaseAnimationInfo> initAddAnimationList;
        /** 进入此操作时的增加物件队列 */
        public List<_AALSOBasicAdditionObjInfo> initAdditionObjList;

        /** 位移行为触发时触发的Action事件队列，位移行为触发时添加的Action事件队列 */
        public List<ALActionEventInfoObj> activeMoveAddEventList;

        /** 位移操作完成时触发的Action事件，位移操作完成时添加的Action事件队列 */
        public List<ALActionEventInfoObj> movementDoneAddEventList;

        public ALSOActionInstigatorMovementEvent()
            : base()
        {
            initAddAnimationList = new List<ALSOBaseAnimationInfo>();
            initAdditionObjList = new List<_AALSOBasicAdditionObjInfo>();
            activeMoveAddEventList = new List<ALActionEventInfoObj>();
            movementDoneAddEventList = new List<ALActionEventInfoObj>();
        }

        /***********************
         * 处理事件的函数
         **/
        public override void activeEvent(ALBaseCreatureActionObj _actionObj)
        {
            //进入主动移动操作时，进入初始化相关事件
            for (int i = 0; i < initAddAnimationList.Count; i++)
            {
                _actionObj.addAnimation(initAddAnimationList[i]);
            }
            for (int i = 0; i < initAdditionObjList.Count; i++)
            {
                _actionObj.addAdditionObj(initAdditionObjList[i]);
            }

            //需要注册本处理对象为对应行为的移动消息处理对象
            _actionObj.regActionMovementListener(this);
        }

        /***********************
         * 处理事件的函数
         **/
        public override void lateActiveEvent(ALBaseCreatureActionObj _actionObj)
        {
        }

        /***********************
         * 需要Update后处理么
         **/
        public override bool needLaterActive()
        {
            return false;
        }

        /**************
         * 处理位移到指定位置的操作
         **/
        public void onEnterMovement(ALBaseCreatureActionObj _actionObj)
        {
            //处理进入位移状态时添加事件的操作
            for (int i = 0; i < activeMoveAddEventList.Count; i++)
            {
                ALActionEventInfoObj addEvent = activeMoveAddEventList[i];
                if (null == addEvent)
                    continue;

                //处理事件
                _actionObj.addEvent(addEvent);
            }
        }

        /**************
         * 在完成位移行为操作时的事件函数
         **/
        public void onMovementDone(ALBaseCreatureActionObj _actionObj)
        {
            //处理进入位移状态时添加事件的操作
            for (int i = 0; i < movementDoneAddEventList.Count; i++)
            {
                ALActionEventInfoObj addEvent = movementDoneAddEventList[i];
                if (null == addEvent)
                    continue;

                //处理事件
                _actionObj.addEvent(addEvent);
            }
        }
    }
}
#endif
