using System;
using System.Collections.Generic;

#if AL_CREATURE_SYS
namespace ALPackage
{
    public interface _IALFrameChecker
    {
        /***************
         * 每帧处理时的检测函数
         **/
        void update(_AALBasicCreatureControl _creature);

        /*****************
         * 检测对象是否有效，不论是否有效，update都将被执行一次
         **/
        bool checkerEnable();
    }
}
#endif
