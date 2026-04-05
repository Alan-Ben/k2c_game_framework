using System;
using System.Collections.Generic;

#if AL_CREATURE_SYS
namespace ALPackage
{
    public class ALCreatureStateActionObj
        : ALBaseCreatureActionObj
    {
        private _AALBaseCharacterActionState actionStateObj;

        public ALCreatureStateActionObj(_AALBaseCharacterActionState _parentState, int _serialize, ALSOCreatureActionInfo _actionInfo)
            : base(_parentState.getCreatureControl(), _serialize, _actionInfo)
        {
            actionStateObj = _parentState;
        }
        public ALCreatureStateActionObj(_AALBaseCharacterActionState _parentState, int _serialize, float _timeSpeed, ALSOCreatureActionInfo _actionInfo)
            : base(_parentState.getCreatureControl(), _serialize, _timeSpeed, _actionInfo)
        {
            actionStateObj = _parentState;
        }

        public _AALBaseCharacterActionState getParentActionStateObj() { return actionStateObj; }
    }
}
#endif
