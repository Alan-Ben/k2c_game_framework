using System;
using System.Collections.Generic;

#if AL_CREATURE_SYS
namespace ALPackage
{
    /**************************
     * 在lateupdate中修改action播放速度的处理任务
     **/
    public class ALActionTimeScaleBonusChgDealer
        : _IALFrameLateChecker
    {
        protected ALBaseCreatureActionObj actionObj;
        protected float timeScale;

        public ALActionTimeScaleBonusChgDealer(ALBaseCreatureActionObj _actionObj, float _timeScale)
        {
            actionObj = _actionObj;
            timeScale = _timeScale;
        }

        /***************
         * 每帧处理时，在LateUpdate调用的
         **/
        public void lateUpdate(_AALBasicCreatureControl _creature)
        {
            if (null == actionObj)
                return;

            //设置播放速度
            actionObj._setTimeScaleBonus(timeScale);
        }

        /*****************
         * 检测对象是否有效
         **/
        public bool laterCheckerEnable()
        {
            return false;
        }
    }
}

#endif
