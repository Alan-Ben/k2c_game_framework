using System;
using System.Collections.Generic;

#if AL_CREATURE_SYS
namespace ALPackage
{
    public interface _IALCreatureActionResponse
    {
        /** 在Action结束时处理 */
        void onActionEnd(ALBaseCreatureActionObj _actionObj);
    }
}
#endif
