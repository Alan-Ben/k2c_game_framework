namespace GOE
{
    /// <summary>
    /// sys_info获取对应的系统信息类型
    /// </summary>
    public enum ESDKSysInfoType
    {
        LAN,//系统语言
        COUNTRY,//国家
        MODEL,//设备型号
        VERSION,//设备系统
        RESOLUTION,//设备分辨率
        MAC_ADDRESS,//获取设备mac地址
        CPU_NUMCORES,//设备cpu核数
        CPU_MAXFREQ,//设备cpu最大频率
        CPU_MINFREQ,//设备cpu最小频率
        CPU_USAGE,//cpu占有量
        MEMORY_USAGE,//内存占有量
        DEVICE_ID,//设备id
    }
}
