using System;
using System.Collections.Generic;

#if AL_CREATURE_SYS
namespace ALPackage
{
    public interface _IALFrameLateChecker
    {
        /***************
         * 每帧处理时，在LateUpdate调用的
         **/
        void lateUpdate(_AALBasicCreatureControl _creature);

        /*****************
         * 检测对象是否有效，不论是否有效，lateUpdate都将被执行一次
         **/
        bool laterCheckerEnable();
    }
}
#endif
