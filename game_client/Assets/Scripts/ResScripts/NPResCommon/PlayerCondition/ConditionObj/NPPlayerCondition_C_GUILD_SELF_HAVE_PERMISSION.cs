using ALPackage;
using Common.GuildEnum;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 联盟自己的职位是否拥有对应权限
    /// </summary>
    public class NPPlayerCondition_C_GUILD_SELF_HAVE_PERMISSION : _ANPBasicPlayerCondition
    {
        private EGuildPermissionType _m_ePermissionType;//权限
        
        public override ENPPlayerConditionType conditionType { get { return ENPPlayerConditionType.C_GUILD_SELF_HAVE_PERMISSION; } }

        /// <summary>
        /// 读取条件信息
        /// </summary>
        /// <param name="_reader"></param>
        /// <returns></returns>
        public static NPPlayerCondition_C_GUILD_SELF_HAVE_PERMISSION readStr(ALStringReader _reader)
        {
            NPPlayerCondition_C_GUILD_SELF_HAVE_PERMISSION cond = new NPPlayerCondition_C_GUILD_SELF_HAVE_PERMISSION();
            string refIdS = _reader.readItem(':');
            if (refIdS != null)
            {
                cond._m_ePermissionType = (EGuildPermissionType)ALCommon.EnumParse(typeof(EGuildPermissionType), refIdS);
            }
            return cond;
        }

        public override bool isEnable(NPVarInfo _varVariableInfo)
        {
#if NP_GAME
            return NPPlayer.instance.guildComp.checkHavePermission(_m_ePermissionType);
#else
            return false;
#endif
        }
    }
}
