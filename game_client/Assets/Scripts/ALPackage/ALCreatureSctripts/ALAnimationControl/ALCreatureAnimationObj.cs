using System;
using System.Collections.Generic;

using UnityEngine;

#if AL_CREATURE_SYS
namespace ALPackage
{
    /*************************
     * 保存所有用户动作对象的信息，以及获取的名称
     **/
    public class ALCreatureAnimationObj
    {
        private _AALBasicCreatureControl creatureControl;
        /** 存储本对象的父节点 */
        private ALCreatureAnimationSession parentSession;

        /** 处理本对象动画的索引字符串 */
        private string animationName;
        /** 存放动作信息的对象，可根据不同的处理位置转化为不同的信息子类 */
        private ALSOBaseAnimationInfoObj animationInfoObj;
        /** 动画添加到对象内后，动画状态控制对象 */
        private AnimationState animationStateObj;

        public ALCreatureAnimationObj(_AALBasicCreatureControl _creatureControl, ALCreatureAnimationSession _parent
            , string _animationName, ALSOBaseAnimationInfoObj _animationObj)
        {
            creatureControl = _creatureControl;
            parentSession = _parent;

            animationName = _animationName;
            animationInfoObj = _animationObj;

            animationStateObj = null;
        }

        public string name { get { return animationName; } }

        /**********************
         * 添加对应的动画到对象中
         **/
        public bool initAnimation()
        {
            if (null == animationInfoObj.animationClipObj)
                return false;

            //add animation clip
            if (animationInfoObj.firstFrame != 0 || animationInfoObj.lastFrame != 0)
                creatureControl.GetComponent<Animation>().AddClip(animationInfoObj.animationClipObj, animationName, animationInfoObj.firstFrame, animationInfoObj.lastFrame, animationInfoObj.addLoopFrame);
            else
                creatureControl.GetComponent<Animation>().AddClip(animationInfoObj.animationClipObj, animationName);

            //get state object
            animationStateObj = creatureControl.GetComponent<Animation>()[animationName];

            //init animation state
            animationInfoObj.initAnimationProp(animationStateObj, parentSession, creatureControl);

            return true;
        }

        /*****************
         * 设置本动作对象的动作层级
         **/
        public void setLayer(CreatureActionLayer _newLayer)
        {
            animationStateObj.layer = (int)_newLayer;
        }

        /*******************
         * 刷新动作播放速度
         **/
        public void refreshAnimationSpeed()
        {
            animationStateObj.speed = parentSession.getRealTimeScale();
        }

        /*******************
         * 刷新动作播放所有属性
         **/
        public void refreshAnimation()
        {
            animationInfoObj.refreshAnimation(animationStateObj, parentSession, creatureControl);
        }

        /******************
         * 将本动作设置为无效
         **/
        public void disableAnimation()
        {
            if (null == animationStateObj)
                return;

            if (RemoveAnimationType.BLEND == animationInfoObj.removeType)
            {
                creatureControl.GetComponent<Animation>().Blend(animationName, 0, animationInfoObj.removeBlendTime);
            }
            else if (RemoveAnimationType.REMOVE == animationInfoObj.removeType)
            {
                if (null != animationStateObj)
                {
                    AnimationClip needReleaseClip = animationStateObj.clip;
                    creatureControl.GetComponent<Animation>().RemoveClip(animationName);
                    GameObject.DestroyImmediate(needReleaseClip);
                }

                animationStateObj = null;
            }
        }

        /********************
         * 动作是否无效
         **/
        public bool checkUseless()
        {
            if (null == animationStateObj)
                return true;

            if (animationStateObj.weight <= 0)
            {
                //remove from creature animation control
                if (null != animationStateObj)
                {
                    AnimationClip needReleaseClip = animationStateObj.clip;
                    creatureControl.GetComponent<Animation>().RemoveClip(animationName);
                    GameObject.DestroyImmediate(needReleaseClip);
                }

                return true;
            }

            return false;
        }

        /*********************
         * 删除资源操作
         **/
        public void discard()
        {
            if (null != animationStateObj)
            {
                AnimationClip needReleaseClip = animationStateObj.clip;
                creatureControl.GetComponent<Animation>().RemoveClip(animationName);
                GameObject.DestroyImmediate(needReleaseClip);
            }
        }
    }
}
#endif
