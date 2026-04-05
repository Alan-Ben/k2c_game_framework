
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPEnum;

using GOE;
using ALPackage;


namespace GOE
{
    public class NPPlayerCondition_C_PLAYER_CAN_LEVEL_UP : _ANPBasicPlayerCondition
    {
        public override ENPPlayerConditionType conditionType { get { return ENPPlayerConditionType.C_PLAYER_CAN_LEVEL_UP; } }

        private _NPPlayerVariableSerializeInfo _m_playerVariable;
	    
        public _NPPlayerVariableSerializeInfo playerValueType { get { return _m_playerVariable; } }

        /// <summary>
        /// 读取条件信息
        /// </summary>
        /// <param name="_reader"></param>
        /// <returns></returns>
        public static NPPlayerCondition_C_PLAYER_CAN_LEVEL_UP readStr(ALStringReader _reader)
        {
            NPPlayerCondition_C_PLAYER_CAN_LEVEL_UP cond = new NPPlayerCondition_C_PLAYER_CAN_LEVEL_UP();
            return cond;
        }
        public override bool isEnable(NPVarInfo _varVariableInfo)
        {
#if NP_GAME
            return NPPlayer.instance.playerInfo.getUpgradeState() == ENPUpgradeState.NORMAL;
	        
#else
	        return false;
#endif
        }
    }
}