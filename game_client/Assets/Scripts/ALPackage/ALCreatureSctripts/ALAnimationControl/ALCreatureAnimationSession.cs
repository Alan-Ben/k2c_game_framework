using System;
using System.Collections.Generic;

using UnityEngine;

#if AL_CREATURE_SYS
namespace ALPackage
{
    public class ALCreatureAnimationSession
        : ALCreatureAdditionObjContainer, _IALFrameChecker
    {
        /** 在角色中，对应动作片段的编号 */
        protected int serialize;

        /** 本次行为已经经过的时间，按照1：1的比例累计的 */
        protected float passedNonScaleTime;
        /** 真实的动画播放速度 */
        protected float realTimeScale;
        /** 本动画的播放速度，不包含行为加成 */
        protected float aniTimeScale;

        /** 动作组合的信息 */
        protected ALSOBaseAnimationInfo animationInfo;

        /** 存放动作组的队列 */
        protected List<ALCreatureAnimationObj> animationList;

        /** 需要处理的事件队列 */
        private LinkedList<ALAnimationEventDealer> eventList;

        /** 是否已经初始化过 */
        private bool _m_bIsInited;

        /*********************
         * 带入默认的播放速度_playSpeed，在ACTION下需要根据Action速度进行调整
         **/
        public ALCreatureAnimationSession(_AALBasicCreatureControl _creatureControl, int _serialize, float _parentTimeScale, ALSOBaseAnimationInfo _animationInfo)
            : base(_creatureControl)
        {
            serialize = _serialize;
            passedNonScaleTime = 0;
            //计算带入速度与默认速度的乘积
            aniTimeScale = _animationInfo.defaultPlaySpeed;
            realTimeScale = _parentTimeScale * aniTimeScale;
            animationInfo = _animationInfo;

            animationList = new List<ALCreatureAnimationObj>();
            eventList = new LinkedList<ALAnimationEventDealer>();

            _m_bIsInited = false;
        }

        public ALSOBaseAnimationInfo getAnimationInfo() { return animationInfo; }
        public int getSerialize() { return serialize; }
        public float getRealTimeScale() { return realTimeScale; }
        public float getAniTimeScale() { return aniTimeScale; }

        /****************
         * 每帧执行的事件检测行为
         **/
        public void update(_AALBasicCreatureControl _creature)
        {
            //将delta时间转换为无缩放情况下的时间长度
            float addPassedNonScaleTime = Time.deltaTime * realTimeScale;
            //增加已经经过的时间长度
            passedNonScaleTime = passedNonScaleTime + addPassedNonScaleTime;

            List<ALAnimationEventDealer> activeEventList = null;
            //检测第一个事件是否需要处理
            while (eventList.Count > 0)
            {
                ALAnimationEventDealer eventObj = eventList.First.Value;
                if (null == eventObj)
                {
                    eventList.RemoveFirst();
                    continue;
                }

                //判断事件是否需要触发，只检查第一个
                if (eventObj.activeTime <= passedNonScaleTime)
                {
                    if (null == activeEventList)
                        activeEventList = new List<ALAnimationEventDealer>(3);

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
        }

        /****************
         * 添加新的事件
         **/
        public void addEvent(ALEventInfoObj _event)
        {
            if (null == _event)
                return;

            //创建处理对象
            ALAnimationEventDealer eventDealer = new ALAnimationEventDealer(_event, this, passedNonScaleTime);

            //遍历队列插入对应位置
            LinkedListNode<ALAnimationEventDealer> node = eventList.First;
            while (null != node)
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

        /*****************
         * 检测对象是否有效
         **/
        public bool checkerEnable()
        {
            if (eventList.Count <= 0)
            {
                return false;
            }

            return true;
        }

        /*****************
         * 初始化所有动作队列
         **/
        public void initAnimationList()
        {
            if (_m_bIsInited)
                return;

            _m_bIsInited = true;

            int idx = 1;

            ALSOBaseAnimationInfoObj obj = null;
            for(int i = 0; i < animationInfo.animationObjList.Count; i++)
            {
                obj = animationInfo.animationObjList[i];
                if(null == obj)
                    continue;

                string animationName = serialize.ToString() + "_" + idx;

                //create animation obj
                ALCreatureAnimationObj creatureAnimationObj = new ALCreatureAnimationObj(creatureControl, this, animationName, obj);

                //add animation
                if (!creatureAnimationObj.initAnimation())
                {
                    //init fail
                    Debug.LogError("Init animation: " + obj.ToString() + " into creature fail!");
                    continue;
                }

                //add into list
                animationList.Add(creatureAnimationObj);

                //add idx
                idx++;
            }

            //初始化事件对象
            for (int i = 0; i < animationInfo.animationEventList.Count; i++)
            {
                ALEventInfoObj eventObj = animationInfo.animationEventList[i];
                if (null == eventObj || null == eventObj.eventObj)
                    continue;

                //调用函数进行排序插入
                addEvent(eventObj);
            }

            if (eventList.Count > 0)
            {
                //注册update对象
                creatureControl.getFrameCheckerMgr().regChecker(this);
            }
        }

        /**********************
         * 设置此动作对象的所有子动作层级
         **/
        public void setAnimationLayer(CreatureActionLayer _layer)
        {
            for(int i = 0; i < animationList.Count; i++)
            {
                animationList[i].setLayer(_layer);
            }
        }

        /********************
         * 刷新动画属性
         **/
        public void refreshAnimationList()
        {
            for (int i = 0; i < animationList.Count; i++)
            {
                animationList[i].refreshAnimation();
            }
        }

        /********************
         * 刷新动画播放速度
         **/
        public void refreshAnimationSpeedList()
        {
            for (int i = 0; i < animationList.Count; i++)
            {
                animationList[i].refreshAnimationSpeed();
            }
        }

        /***********************
         * 将本组动作中所有动作都无效
         **/
        public void disableAnimation()
        {
            //触发移除时事件
            for (int i = 0; i < animationInfo.animationFinishEventList.Count; i++)
            {
                _AALSOBaseEvent eventObj = animationInfo.animationFinishEventList[i];
                if (null == eventObj)
                    continue;

                eventObj.activeEvent(this, creatureControl);
            }

            for (int i = 0; i < animationList.Count; i++)
            {
                animationList[i].disableAnimation();
            }

            //删除所有添加物件
            removeAllAdditionObj();

            //judge event list,if true unrege frame checker
            if (eventList.Count > 0)
            {
                //清空事件队列，本对象自然无效
                eventList.Clear();
            }

            //call the function
            onDisable();
        }

        /********************
         * 检查本动作组合是否已经无效
         **/
        public bool checkAnimationUseless()
        {
            //循环删除无效动画
            for (int i = 0; i < animationList.Count; )
            {
                if (animationList[i].checkUseless())
                {
                    animationList.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }

            //判断动作是否为空，表示本动作组完全无效
            if (animationList.Count <= 0)
                return true;

            return false;
        }

        /************************
         * 删除具体资源的操作
         **/
        public void discard()
        {
            //删除所有添加物件
            removeAllAdditionObj();

            //循环删除无效动画
            for (int i = 0; i < animationList.Count; i++)
            {
                animationList[i].discard();
            }
            animationList.Clear();

            //set creature null
            _m_creatureControl = null;
        }

        /***************
         * 删除本对象时处理
         **/
        public virtual void onDisable()
        {
            //set creature null
            _m_creatureControl = null;
        }

        /*****************
         * 设置本动画归属行为的播放的速度
         **/
        protected internal void _setParentTimeScale(float _parentTimeScale)
        {
            //重新计算实际播放速度
            realTimeScale = _parentTimeScale * aniTimeScale;

            //刷新所有动画播放速度
            refreshAnimationSpeedList();
        }

        /*****************
         * 设置本动画播放的速度
         **/
        protected internal void _setAniTimeScale(float _aniTimeScale)
        {
            //重新计算实际播放速度
            realTimeScale = realTimeScale * _aniTimeScale / aniTimeScale;
            //设置基本播放速度
            aniTimeScale = _aniTimeScale;

            //刷新所有动画播放属性
            refreshAnimationSpeedList();
        }
    }
}
#endif
