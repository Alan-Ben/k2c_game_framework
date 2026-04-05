using ALPackage;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 检查玩家是否拥有指定权限 CS_CHECK_PLAYER_PERMISSION:权限配置ID
    /// </summary>
    public class NPPlayerCondition_CS_CHECK_PLAYER_PERMISSION : _ANPBasicPlayerCondition
    {
        //配置id
        private long _m_lRefId = 0;

        public override ENPPlayerConditionType conditionType { get { return ENPPlayerConditionType.CS_CHECK_PLAYER_PERMISSION; } }

        /// <summary>
        /// 读取条件信息
        /// </summary>
        /// <param name="_reader"></param>
        /// <returns></returns>
        public static NPPlayerCondition_CS_CHECK_PLAYER_PERMISSION readStr(ALStringReader _reader)
        {
            NPPlayerCondition_CS_CHECK_PLAYER_PERMISSION cond = new NPPlayerCondition_CS_CHECK_PLAYER_PERMISSION();

            //配置id
            string refIdS = _reader.readItem(':');
            if (refIdS == null)
            {
                UnityEngine.Debug.LogError("Can not read str for ENPPlayerConditionType.CS_CHECK_PLAYER_PERMISSION [" + _reader.srcString + "]");
                return null;
            }

            cond._m_lRefId = long.Parse(refIdS);
            return cond;
        }

        public override bool isEnable(NPVarInfo _varVariableInfo)
        {
#if NP_GAME
            if (_m_lRefId <= 0)
                return false;

            return NPPlayer.instance.playerPermissionsComp.checkHavePermissions(_m_lRefId);
#else
            return false;
#endif
        }
    }
}
