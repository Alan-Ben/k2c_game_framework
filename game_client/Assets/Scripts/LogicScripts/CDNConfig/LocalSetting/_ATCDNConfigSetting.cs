using ALPackage;

namespace GOE
{
    //GOD项目内使用的cdn配置信息基类，包含版本号信息
    public abstract class _ATCDNConfigSetting<T> : _ATALCDNConfigSetting<T>
    {
        protected _ATCDNConfigSetting(string _settingName) 
            : base(_settingName, ClientVersionSetting.instance.ClientVersionInfo.clientId)
        {
        }
    }
}