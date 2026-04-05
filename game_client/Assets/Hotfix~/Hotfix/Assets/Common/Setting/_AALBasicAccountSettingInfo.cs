using ALPackage;
using GOE;

namespace Hotfix
{
    /// <summary>
    /// 与账号相关的存储设置
    /// </summary>
    public abstract class _AALBasicAccountSettingInfo : _AALBasicSettingInfo
    {
        protected _AALBasicAccountSettingInfo(string _settingPath) : base($"{NPPlayer.instance.playerInfo.CID}_{_settingPath}")
        {
        }
    }
}