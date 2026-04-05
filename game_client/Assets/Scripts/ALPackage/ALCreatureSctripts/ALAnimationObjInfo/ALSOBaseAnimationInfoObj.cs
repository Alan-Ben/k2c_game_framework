using System;
using System.Collections.Generic;
using UnityEngine;

#if AL_CREATURE_SYS
namespace ALPackage
{
    /******************
     * 动作分层枚举
     **/
    public enum CreatureActionLayer
    {
        BASE,               //基础动作，Idle
        LOWER_ACTION,       //优先级较低动作，如攻击时的下盘动作
        BASE_ACTION,        //基本动作，如走动，跑步
        HIGER_ACTION,       //高优先级动作，如攻击动作
        ADDITION_ACTION,    //附加动作，如受击动作
    }

    /******************
     * 动作删除的方式
     **/
    public enum RemoveAnimationType
    {
        REMOVE,     //直接删除
        BLEND,      //过度方式删除
    }

    /*******************
     * 动画对象的基本属性
     **/

    [System.Serializable]
    public class ALSOBaseAnimationInfoObj : ScriptableObject
    {
        /** 归属层对象 */
        public CreatureActionLayer animationLayer;
        /** 动画对象 */
        public AnimationClip animationClipObj;
        /** 绑定骨骼Path */
        public List<ALSOCreatureBoneObjInfo> mixTransformPath;
        /** 播放模式 */
        public WrapMode animationMode;
        /** 动作blend模式 */
        public AnimationBlendMode blendMode;
        /** 创建对象时，权重过度时间 */
        public float blendTime;
        /** 动画权重 */
        public float weight;
        /** 删除的方式 */
        public RemoveAnimationType removeType;
        /** 过度删除时，过度时间 */
        public float removeBlendTime;
        /** 动作播放的开始帧位置 */
        public int firstFrame;
        /** 动作播放的结束帧位置 */
        public int lastFrame;
        /** 是否在结束帧之后添加一帧作为循环帧处理 */
        public bool addLoopFrame;
        /** 初始化时的添加物件队列 */
        public List<_AALSOBasicAdditionObjInfo> initAdditionObjList;

        public ALSOBaseAnimationInfoObj()
        {
            animationLayer = CreatureActionLayer.BASE_ACTION;
            mixTransformPath = new List<ALSOCreatureBoneObjInfo>();
            animationMode = WrapMode.Loop;
            blendMode = AnimationBlendMode.Blend;
            removeType = RemoveAnimationType.BLEND;
            weight = 1.0f;
            blendTime = 0.1f;
            removeBlendTime = 0.1f;
        }

        /***************
         * 初始化动作属性
         **/
        public virtual void initAnimationProp(AnimationState _animationStat, ALCreatureAnimationSession _animationSession, _AALBasicCreatureControl _creatureControl)
        {
            //set layer
            _animationStat.layer = (int)animationLayer;
            _animationStat.wrapMode = animationMode;
            _animationStat.blendMode = blendMode;
            _animationStat.speed = _animationSession.getRealTimeScale();

            //set mixTransform
            for (int i = 0; i < mixTransformPath.Count; i++)
            {
                ALSOCreatureBoneObjInfo objInfo = mixTransformPath[i];

                Transform obj = objInfo.getCreatureChildObj(_creatureControl);
                if (null == obj)
                {
                    Debug.LogError("Animation: " + this.ToString() + " gets an Empty object: " + objInfo);
                    continue;
                }

                //add mix transform
                _animationStat.AddMixingTransform(obj);
            }

            //添加动作的初始化添加物件
            for (int i = 0; i < initAdditionObjList.Count; i++)
            {
                if (initAdditionObjList[i] != null)
                {
                    _animationSession.addAdditionObj(initAdditionObjList[i]);
                }
            }

            /**************
             * 刷新动画播放
             **/
            refreshAnimation(_animationStat, _animationSession, _creatureControl);
        }

        /***************
         * 刷新当前相关属性
         **/
        public virtual void refreshAnimation(AnimationState _animationStat, ALCreatureAnimationSession _animationSession, _AALBasicCreatureControl _creatureControl)
        {
            //播放动作 - 默认为100%权重
            _animationStat.speed = _animationSession.getRealTimeScale();
            _creatureControl.GetComponent<Animation>().Blend(_animationStat.name, weight, blendTime);
        }
    }
}
#endif
