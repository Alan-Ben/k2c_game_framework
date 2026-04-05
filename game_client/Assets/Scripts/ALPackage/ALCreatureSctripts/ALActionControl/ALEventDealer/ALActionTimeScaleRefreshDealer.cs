using System;
using System.Collections.Generic;

#if AL_CREATURE_SYS
namespace ALPackage
{
    /**************************
     * 在lateupdate中修改action播放速度的处理任务
     **/
    public class ALActionTimeScaleRefreshDealer
        : _IALFrameLateChecker
    {
        protected ALBaseCreatureActionObj actionObj;

        public ALActionTimeScaleRefreshDealer(ALBaseCreatureActionObj _actionObj)
        {
            actionObj = _actionObj;
        }

        /***************
         * 每帧处理时，在LateUpdate调用的
         **/
        public void lateUpdate(_AALBasicCreatureControl _creature)
        {
            if (null == actionObj)
                return;

            //设置播放速度
            actionObj._refreshActionTimeScale();
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
