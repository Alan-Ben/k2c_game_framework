using System;
using UnityEngine;

#if AL_CREATURE_SYS
namespace ALPackage
{
    public class ALOtherPlayerControl : _AALBasicCreatureControl
    {
        /************************
         * 创建对象的行为控制状态机
         **/
        protected override _AALBasicActionStateMachine _createCharacterActionStateMachine()
        {
            return new ALOtherPlayerActionStateMachine(this);
        }
    }
}
#endif
