using System;
using System.Collections.Generic;

using UnityEngine;

#if AL_CREATURE_SYS
namespace ALPackage
{
    public class ALBaseCreatureActionObj
        : ALCreatureAdditionObjContainer, _IALFrameChecker
    {
        /** 序列号 */
        protected int serialize;

        /** 本次行为已经经过的时间，按照1：1的比例累计的 */
        protected float passedNonScaleTime;

        /** 真实的动画播放速度 */
        private float realTimeScale;
        /** 播放速度 */
        private float actionTimeScale;
        /** 附加在本行为的播放速度加成 */
        private float actionTimeScaleBonus;

        /** 内部Operation序列号记录对象 */
        protected int operationSerialize;

        /** 信息存储对象 */
        protected ALSOCreatureActionInfo actionInfo;

        /** 本ACTION内的所有已经创建的operation列表 */
        protected LinkedList<ALCreatureActionAnimationSession> animationSessionList;

        /** 需要处理的事件队列，按照时间先后排序 */
        protected LinkedList<ALActionEventDealer> eventList;

        /** 处理事件对象队列 */
        protected List<_IALCreatureActionResponse> responseObjList;

        /** 监听位移操作的监听对象列表 */
        protected List<_IActionMovementListener> movementListenerList;

        /** 位移状态对象队列 */
        protected List<ALMovementCharacterMoveState> movementStateList;

        public ALBaseCreatureActionObj(_AALBasicCreatureControl _creature, int _serialize, ALSOCreatureActionInfo _actionInfo)
            : base(_creature)
        {
            serialize = _serialize;
            passedNonScaleTime = 0;
            actionTimeScale = _actionInfo.timeScale;
            actionTimeScaleBonus = 1f;
            realTimeScale = actionTimeScale * creatureControl.getActionControler().creatureActionTimeScale;
            actionInfo = _actionInfo;

            animationSessionList = new LinkedList<ALCreatureActionAnimationSession>();

            eventList = new LinkedList<ALActionEventDealer>();

            operationSerialize = 1;

            responseObjList = new List<_IALCreatureActionResponse>();

            movementListenerList = new List<_IActionMovementListener>();

            movementStateList = new List<ALMovementCharacterMoveState>();
        }
        public ALBaseCreatureActionObj(_AALBasicCreatureControl _creature, int _serialize, float _timeScale, ALSOCreatureActionInfo _actionInfo)
            : base(_creature)
        {
            serialize = _serialize;
            passedNonScaleTime = 0;
            actionTimeScale = _actionInfo.timeScale;
            actionTimeScaleBonus = _timeScale;
            realTimeScale = actionTimeScaleBonus * actionTimeScale * creatureControl.getActionControler().creatureActionTimeScale;
            actionInfo = _actionInfo;

            animationSessionList = new LinkedList<ALCreatureActionAnimationSession>();

            eventList = new LinkedList<ALActionEventDealer>();

            operationSerialize = 1;

            responseObjList = new List<_IALCreatureActionResponse>();

            movementListenerList = new List<_IActionMovementListener>();

            movementStateList = new List<ALMovementCharacterMoveState>();
        }

        public int getSerialize() { return serialize; }
        public float getRealTimeScale() { return realTimeScale; }

        /****************
         * 每帧执行的事件检测行为
         **/
        public void update(_AALBasicCreatureControl _creature)
        {
            //将delta时间转换为无缩放情况下的时间长度
            float addPassedNonScaleTime = Time.deltaTime * realTimeScale;
            //增加已经经过的时间长度
            passedNonScaleTime = passedNonScaleTime + addPassedNonScaleTime;

            //由于有事件是直接终止的，因此先获取出所有需要激活的事件后进行统一处理
            List<ALActionEventDealer> activeEventList = null;
            //检测第一个事件是否需要处理
            while (eventList.Count > 0)
            {
                ALActionEventDealer eventObj = eventList.First.Value;
                if (null == eventObj)
                {
                    eventList.RemoveFirst();
                    continue;
                }

                //判断事件是否需要触发，只检查第一个
                if (eventObj.activeTime <= passedNonScaleTime)
                {
                    if (null == activeEventList)
                        activeEventList = new List<ALActionEventDealer>(3);

                    //add into active list
                    activeEventList.Add(eventObj);
                    //remove event from list
                    eventList.RemoveFirst();
                }
                else
                {
                    break;
                }
            }

            if (null != activeEventList)
            {
                //deal the active event list
                for (int i = 0; i < activeEventList.Count; i++)
                {
                    activeEventList[i].dealEvent();
                }
            }

            //当位移监听队列不为空则需要提取位移操作信息
            if (movementListenerList.Count > 0)
            {
                ActionMovementInfo movementInfo = creatureControl.popMovementFactory();

                //获取到位移操作信息时，处理所有的监听对象，并将监听队列置空
                if (null != movementInfo)
                {
                    //主玩家的移动行为进入位移状态
                    ALMovementCharacterMoveState movementState = creatureControl.getMoveStateMachine().enterMovementState(movementInfo);

                    for (int i = 0; i < movementListenerList.Count; i++)
                    {
                        _IActionMovementListener listener = movementListenerList[i];
                        if (null == listener)
                            continue;

                        //处理监听对象的函数
                        listener.onEnterMovement(this);

                        //在位移状态中增加监听处理对象
                        if (null != movementState)
                            movementState.regMovementListener(listener);
                    }

                    //清空队列
                    movementListenerList.Clear();

                    //将位移状态放入检测队列中
                    if (null != movementState)
                        movementStateList.Add(movementState);
                }
            }

            //检测是否有位移状态完成了
            for (int i = 0; i < movementStateList.Count; )
            {
                ALMovementCharacterMoveState movementState = movementStateList[i];
                if (null == movementState)
                    continue;

                if (movementState.isMovementDone())
                {
                    //处理完成事件后从队列删除此状态
                    movementState.dealMovementDone(this);
                    movementStateList.RemoveAt(i);
                }
                else
                {
                    //未完成计算下一个
                    i++;
                }
            }
        }

        /****************
         * 添加新的事件
         **/
        public void addEvent(ALActionEventInfoObj _actionEvent)
        {
            if (null == _actionEvent)
                return;

            //创建处理对象
            ALActionEventDealer eventDealer = new ALActionEventDealer(_actionEvent, this, passedNonScaleTime);

            //遍历队列插入对应位置
            LinkedListNode<ALActionEventDealer> node = eventList.First;
            while(null != node)
            {
                //逐个判断，时间点按照时间顺序排列
                if (node.Value.activeTime > eventDealer.activeTime)
                {
                    //当前位置的触发事件比添加的处理时间晚则添加到当前节点之前
                    eventList.AddBefore(node, eventDealer);
                    //退出函数
                    return;
                }

                //取下个节点
                node = node.Next;
            }

            //原先都没添加成功，直接添加到末尾
            eventList.AddLast(eventDealer);
        }

        /**********************
         * 注册行为位移消息的监听对象
         **/
        public void regActionMovementListener(_IActionMovementListener _listener)
        {
            movementListenerList.Add(_listener);
        }

        /*****************
         * 检测对象是否有效
         **/
        public bool checkerEnable()
        {
            if (eventList.Count <= 0 && movementListenerList.Count <= 0 && movementStateList.Count <= 0)
            {
                return false;
            }

            return true;
        }

        /*********************
         * 注册响应对象
         **/
        public void regResponseObj(_IALCreatureActionResponse _responseObj)
        {
            if (null == _responseObj)
                return;

            responseObjList.Add(_responseObj);
        }
        public void unregResponseObj(_IALCreatureActionResponse _responseObj)
        {
            if (null == _responseObj)
                return;

            responseObjList.Remove(_responseObj);
        }

        //初始化本对象
        public void initAction()
        {
            if (null == actionInfo)
                return;

            for (int i = 0; i < actionInfo.initAnimationList.Count; i++)
            {
                ALSOBaseAnimationInfo animationInfo = actionInfo.initAnimationList[i];

                //add operation
                _addAnimation(animationInfo);
            }

            //添加物件
            for (int i = 0; i < actionInfo.initAdditionObjList.Count; i++)
            {
                _AALSOBasicAdditionObjInfo additionObjInfo = actionInfo.initAdditionObjList[i];

                //add addition object
                addAdditionObj(additionObjInfo);
            }

            //初始化事件对象
            for (int i = 0; i < actionInfo.actionEventList.Count; i++)
            {
                ALActionEventInfoObj actionEvent = actionInfo.actionEventList[i];
                if (null == actionEvent || null == actionEvent.eventObj)
                    continue;

                //使用插入函数添加事件
                addEvent(actionEvent);
            }

            if (eventList.Count > 0)
            {
                //注册update对象
                creatureControl.getFrameCheckerMgr().regChecker(this);
            }
        }

        /******************
         * 刷新本行为的动画播放速度
         **/
        public void refreshTimeScale()
        {
            //由于修改系数会引起系统内原先运算规则错误，因此开启任务在lateupdate中处理具体的参数设置
            creatureControl.getFrameCheckerMgr().regLaterChecker(new ALActionTimeScaleRefreshDealer(this));
        }

        /******************
         * 设置本行为的动画播放速度
         **/
        public void setActionTimeScale(float _timeScale)
        {
            //由于修改系数会引起系统内原先运算规则错误，因此开启任务在lateupdate中处理具体的参数设置
            creatureControl.getFrameCheckerMgr().regLaterChecker(new ALActionTimeScaleChgDealer(this, _timeScale));
        }

        /******************
         * 设置本动作的时间比例加成
         **/
        public void setTimeScaleBonus(float _timeScale)
        {
            //由于修改系数会引起系统内原先运算规则错误，因此开启任务在lateupdate中处理具体的参数设置
            creatureControl.getFrameCheckerMgr().regLaterChecker(new ALActionTimeScaleBonusChgDealer(this, _timeScale));
        }

        //移除所有子对象
        public void discard()
        {
            //移除所有operation
            while (animationSessionList.Count > 0)
            {
                ALCreatureActionAnimationSession animationSession = animationSessionList.First.Value;

                if (null != animationSession)
                {
                    //调用一次update避免部分事件被跳过
                    animationSession.update(creatureControl);
                    //调用析构函数
                    creatureControl.getAnimationControler().removeAnimation(animationSession);
                }

                animationSessionList.RemoveFirst();
            }

            //remove all child addition object
            removeAllAdditionObj();

            //judge event list,if true unrege frame checker
            if (eventList.Count > 0)
            {
                //清空事件队列，本对象自然无效
                eventList.Clear();
            }

            //deal the response function
            for (int i = 0; i < responseObjList.Count; i++)
            {
                _IALCreatureActionResponse responseObj = responseObjList[i];

                if (null == responseObj)
                    continue;

                responseObj.onActionEnd(this);
            }

            //clear response object list
            responseObjList.Clear();

            //移除所有位移监听对象
            movementListenerList.Clear();

            //移除所有等待监听的位移移动状态
            movementStateList.Clear();

            //set creature null
            _m_creatureControl = null;
        }

        /*****************
         * add operation
         **/
        public void addAnimation(ALSOBaseAnimationInfo _animationInfo)
        {
            _addAnimation(_animationInfo);
        }

        /********************
         * remove operation
         **/
        public void removeAnimation(ALCreatureActionAnimationSession _animationSession)
        {
            if (null == _animationSession)
                return;

            //从队列中删除对应对象索引
            LinkedListNode<ALCreatureActionAnimationSession> node = animationSessionList.First;
            while(null != node)
            {
                //逐个判断
                if (node.Value == _animationSession)
                {
                    //删除节点并跳出循环
                    animationSessionList.Remove(node);
                    break;
                }

                node = node.Next;
            }

            //释放对应对象的资源
            creatureControl.getAnimationControler().removeAnimation(_animationSession);
        }
        public void removeAnimation(ALSOBaseAnimationInfo _animationInfo)
        {
            if (null == _animationInfo)
                return;

            //从队列中删除符合条件的对象
            LinkedListNode<ALCreatureActionAnimationSession> node = animationSessionList.First;
            LinkedListNode<ALCreatureActionAnimationSession> nextNode = null;
            while (null != node)
            {
                //提前获取下一个节点
                nextNode = node.Next;

                //逐个判断
                if (node.Value.getAnimationInfo().name == _animationInfo.name)
                {
                    //调用析构函数
                    creatureControl.getAnimationControler().removeAnimation(node.Value);
                    //删除节点并跳出循环
                    animationSessionList.Remove(node);
                }

                //设置为下一个节点
                node = nextNode;
            }
        }

        /*****************
         * 刷新本动画行为的播放的速度
         **/
        protected internal void _refreshActionTimeScale()
        {
            //重新计算实际播放速度
            realTimeScale = actionTimeScaleBonus * actionTimeScale * creatureControl.getActionControler().creatureActionTimeScale;

            //刷新所有动画播放速度
            _refreshAllAnimationSessionList();
        }

        /*****************
         * 设置本动画行为的播放的速度
         **/
        protected internal void _setTimeScaleBonus(float _timeScaleBonus)
        {
            //重新计算实际播放速度
            realTimeScale = _timeScaleBonus * actionTimeScale * creatureControl.getActionControler().creatureActionTimeScale;
            //设置播放速度加成
            actionTimeScaleBonus = _timeScaleBonus;

            //刷新所有动画播放速度
            _refreshAllAnimationSessionList();
        }

        /*****************
         * 设置本动画播放的速度
         **/
        protected internal void _setActionTimeScale(float _actionTimeScale)
        {
            //重新计算实际播放速度
            realTimeScale = actionTimeScaleBonus * _actionTimeScale * creatureControl.getActionControler().creatureActionTimeScale;
            //设置基本播放速度
            actionTimeScale = _actionTimeScale;

            //刷新所有动画播放速度
            _refreshAllAnimationSessionList();
        }

        /********************
         * 刷新所有子动画播放速度
         **/
        protected void _refreshAllAnimationSessionList()
        {
            LinkedListNode<ALCreatureActionAnimationSession> node = animationSessionList.First;
            while (null != node)
            {
                //刷新
                node.Value._setParentTimeScale(realTimeScale);
                node = node.Next;
            }
        }

        /******************
         * 添加行为
         **/
        protected void _addAnimation(ALSOBaseAnimationInfo _animationInfo)
        {
            if (null == _animationInfo)
                return;

            ALCreatureActionAnimationSession animationObj = creatureControl.getAnimationControler().addActionAnimation(_animationInfo, this);

            //add into the list
            animationSessionList.AddLast(animationObj);
        }

        /**************
         * 获取新的Operation序列号
         **/
        private int __getNewOperationSerialize()
        {
            return operationSerialize++;
        }
    }
}
#endif
