using System;
using System.Collections.Generic;

using UnityEngine;

#if AL_CREATURE_SYS
/*******************
 * 动画对象的基本属性
 **/

namespace ALPackage
{
    [System.Serializable]
    public class ALSOMoveAnimationInfoObj : ALSOBaseAnimationInfoObj
    {
        /** 速度，用于权重计算，0则表示无极限 */
        public float minSpeed;
        public float maxSpeed;
        public float defaultSpeed;

        public ALSOMoveAnimationInfoObj()
            : base()
        {
            minSpeed = 0;
            maxSpeed = 0;
            defaultSpeed = 0;
        }

        /***************
         * 初始化动作属性
         **/
        public override void initAnimationProp(AnimationState _animationStat, ALCreatureAnimationSession _animationSession, _AALBasicCreatureControl _creatureControl)
        {
            base.initAnimationProp(_animationStat, _animationSession, _creatureControl);
        }

        /***************
         * 刷新当前相关属性
         **/
        public override void refreshAnimation(AnimationState _animationStat, ALCreatureAnimationSession _animationSession, _AALBasicCreatureControl _creatureControl)
        {
            if ((minSpeed != 0 && _creatureControl.moveSpeed < minSpeed)
                || (maxSpeed != 0 && _creatureControl.moveSpeed > maxSpeed))
            {
                //速度超出范围，不进行播放
                _animationStat.speed = 1.0f * _animationSession.getRealTimeScale();
                _creatureControl.GetComponent<Animation>().Blend(_animationStat.name, 0f, 0.2f);
                return;
            }

            //计算权重或播放速度
            float moveSpeed = _creatureControl.moveSpeed;

            //当无限制，为100%权重
            if (minSpeed == 0 && maxSpeed == 0)
            {
                float newSpeed = 1;
                if(defaultSpeed > 0)
                    newSpeed = moveSpeed / defaultSpeed;

                _animationStat.speed = newSpeed * _animationSession.getRealTimeScale();
                _creatureControl.GetComponent<Animation>().Blend(_animationStat.name, 1.0f, 0.2f);
                return;
            }

            if (moveSpeed < defaultSpeed)
            {
                //在默认速度下，进行权重计算
                float newWeight = 1;
                if (defaultSpeed > 0)
                    newWeight = moveSpeed / defaultSpeed;

                _animationStat.speed = 1.0f * _animationSession.getRealTimeScale();
                _creatureControl.GetComponent<Animation>().Blend(_animationStat.name, newWeight, 0.2f);
                return;
            }
            else
            {
                float newSpeed = 1;
                if (defaultSpeed > 0)
                    newSpeed = moveSpeed / defaultSpeed;

                _animationStat.speed = newSpeed * _animationSession.getRealTimeScale();
                _creatureControl.GetComponent<Animation>().Blend(_animationStat.name, 1.0f, 0.2f);
                return;
            }
        }
    }
}
#endif
