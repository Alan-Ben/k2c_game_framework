using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPEnum;

namespace GOE
{
    public class NPPlayerConditionFalse : _ANPBasicPlayerCondition
    {
        public override ENPPlayerConditionType conditionType { get { return ENPPlayerConditionType.NONE; } }

        /*********
         * 判断条件是否满足
         **/
        public override bool isEnable(NPVarInfo _varVariableInfo)
        {
            return false;
        }
    }
}

