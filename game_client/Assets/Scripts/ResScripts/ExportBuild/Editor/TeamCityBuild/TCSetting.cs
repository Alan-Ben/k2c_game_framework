using System;
using GOE;
using UnityEditor;
using UnityEngine;

public class TCSetting
{
    public static bool isFull;       //是否打整包
    public static bool isDebug;// 是否开启Debug
    public static bool isObb; //是否打Obb
    public static bool isAAB; //是否打Aab
    public static bool isAssetDiffe;//是否进行差异资源打包
    public static bool isSDK; //是否启用SDK
    public static bool isShowSDKLog;//是否打印SDKlog
    public static bool canInjectFix;//是否注入InjectFix
    public static bool il2cpp;//是否使用il2cpp的方式打包（安卓才有得选，ios永远都用il2cpp）
    public static bool isPerfTest;//是否开启性能测试模式
    public static bool isBuildRefdata;//是否正在打包Refdata工程
    public static bool isDevelopmentBuild;//是否打开发包
    public static bool containX86;//是否需要打X86  il2cpp true 情况才生效

    public static long resVer;//资源版本号
    
    public static int VersionCoed;  //VersionCoed
    public static int platVersion;// 渠道版本号
    public static int serverId = -1;//服务器id（几服）
    public static int connectPort;//登陆Port
    public static int phpADDefaultLevel = -999;//埋点默认发送等级

    public static string ClientVer; //Client版本号
    public static string PlatName; //平台名字,也可以叫渠道（IN_TEST，QA_TEST...对应枚举 EMGPlatType）
    public static string GameName;//游戏名字
    public static string sdkpath;//SDK库本地路径
    public static string packageName;//包名
    public static string keypath;//keyStore文件路径
    public static string keyPass;//keyStore密码
    public static string keyAli;//KeyAli
    public static string keyAliasPass;//keyStore密码
    public static string apkUrl;//APK生成完整路径，包括APK包包名
    public static string oldVersionUrl;//差异资源路径
    public static string EditorPlatform;//需要打包的Editor平台
    public static string phpUrlForLoginList; //客户端请求PHP地址
    public static string phpADDefaultURL; //客户端请求 后台，充值订单地址
    public static string clientLoginType;//用户登录类型
    public static string connectIp;//登陆ip
    public static string platResURL;//平台资源更新地址
    public static string areaResURL;//区域资源更新地址
    public static string gameResURL;//游戏资源更新地址
    public static string hotfixURL;//hotfix dll更新地址
    public static string injectFixURL;//InjectFix 补丁更新地址
    public static string refdataURL;//refdata 补丁更新地址
    public static string videoResURL;//video 补丁更新地址
    public static string audioURL;//audio 补丁更新地址
    
    public static void settingVar(string[] CommonList)
    {
        parseLongVariable(CommonList, ref resVer, "-resVer:");


        parseIntVariable(CommonList, ref platVersion, "-platVersion:"); 
        parseIntVariable(CommonList, ref VersionCoed, "-VersionCoed:");
        parseIntVariable(CommonList, ref serverId, "-serverId:"); 
        parseIntVariable(CommonList, ref connectPort, "-connectPort:");
        parseIntVariable(CommonList, ref phpADDefaultLevel, "-phpADDefaultLevel:");

        parseStringVariable(CommonList, ref ClientVer, "-ClientVer:");
        parseStringVariable(CommonList, ref PlatName, "-PlatName:");
        parseStringVariable(CommonList, ref GameName, "-GameName:");
        parseStringVariable(CommonList, ref sdkpath, "-sdkpath:");
        parseStringVariable(CommonList, ref packageName, "-packageName:");
        parseStringVariable(CommonList, ref keypath, "-keypath:");
        parseStringVariable(CommonList, ref keyPass, "-keyPass:");
        parseStringVariable(CommonList, ref keyAli, "-keyAli:");
        parseStringVariable(CommonList, ref keyAliasPass, "-keyAliPass:");
        parseStringVariable(CommonList, ref apkUrl, "-apkUrl:");
        parseStringVariable(CommonList, ref oldVersionUrl, "-oldVersionUrl:");
        parseStringVariable(CommonList, ref EditorPlatform, "-EditorPlatform:");
        parseStringVariable(CommonList, ref phpUrlForLoginList, "-phpUrlForLoginList:");
        parseStringVariable(CommonList, ref phpADDefaultURL, "-phpADDefaultURL:");
        parseStringVariable(CommonList, ref clientLoginType, "-clientLoginType:");
        parseStringVariable(CommonList, ref connectIp, "-connectIp:");
        parseStringVariable(CommonList, ref platResURL, "-platResURL:");
        parseStringVariable(CommonList, ref areaResURL, "-areaResURL:");
        parseStringVariable(CommonList, ref gameResURL, "-gameResURL:");
        parseStringVariable(CommonList, ref hotfixURL, "-hotfixURL:");
        parseStringVariable(CommonList, ref injectFixURL, "-injectFixURL:");
        parseStringVariable(CommonList, ref refdataURL, "-refdataURL:");
        parseStringVariable(CommonList, ref audioURL, "-audioURL:");
        parseStringVariable(CommonList, ref videoResURL, "-videoResURL:");

        parseBoolVariable(CommonList, ref isFull, "-isFull:");
        parseBoolVariable(CommonList, ref isDebug, "-isDebug:");
        parseBoolVariable(CommonList, ref isObb, "-isObb:");
        parseBoolVariable(CommonList, ref isAAB, "-isAAB:");
        parseBoolVariable(CommonList, ref isAssetDiffe, "-isAssetDiffe:");
        parseBoolVariable(CommonList, ref isSDK, "-isSDK:");
        parseBoolVariable(CommonList, ref isShowSDKLog, "-isShowSDKLog:");
        parseBoolVariable(CommonList, ref canInjectFix, "-canInjectFix:");
        parseBoolVariable(CommonList, ref il2cpp, "-il2cpp:");
        parseBoolVariable(CommonList, ref isPerfTest, "-isPerfTest:");
        parseBoolVariable(CommonList, ref isBuildRefdata, "-isBuildRefdata:");
        parseBoolVariable(CommonList, ref isDevelopmentBuild, "-isDevelopmentBuild:");
        parseBoolVariable(CommonList, ref containX86, "-containX86:");
    }

    private static void parseBoolVariable(string[] CommonList, ref bool _variable, string _var, bool _canEmpty = true)
    {
        for ( int i = 0; i < CommonList.Length; i++)
        {
            if (CommonList[i].StartsWith(_var))
            {
                string substring = CommonList[i].Substring(_var.Length);
                if(!string.IsNullOrEmpty(substring))
                {
                    _variable = bool.Parse(substring);
                    Debug.Log("TC_C#-------parseVariable------" + _var + "= " + _variable);
                }
                else if(_canEmpty == false)
                {
                    TeamCityBuildAPK.stopTeamCityTask($"{_var}不能为空");
                }
            }
        }
    }
    private static void parseStringVariable(string[] CommonList, ref string _variable,string _var, bool _canEmpty = true)
    {
        for (int i = 0; i < CommonList.Length; i++)
        {
            if(CommonList[i].StartsWith(_var))
            {
                _variable = CommonList[i].Substring(_var.Length);
                if(string.IsNullOrEmpty(_variable) && _canEmpty == false)
                {
                    TeamCityBuildAPK.stopTeamCityTask($"{_var}不能为空");
                }
                Debug.Log("TC_C#-------parseVariable------" + _var + "= " + _variable);
            }
        }
    }
    private static void parseLongVariable(string[] CommonList, ref long _variable, string _var, bool _canEmpty = true)
    {
        for (int i = 0; i < CommonList.Length; i++)
        {
            if (CommonList[i].StartsWith(_var))
            {
                string substring = CommonList[i].Substring( _var.Length);
                if(!string.IsNullOrEmpty(substring))
                {
                    _variable = long.Parse(substring);
                    Debug.Log("TC_C#-------parseVariable------" + _var + "= " + _variable);
                }
                else if(_canEmpty == false)
                {
                    TeamCityBuildAPK.stopTeamCityTask($"{_var}不能为空");
                }
            }
        }
    }

    private static void parseIntVariable(string[] CommonList, ref int _variable, string _var, bool _canEmpty = true)
    {
      for (int i = 0; i < CommonList.Length; i++)
        {
            if (CommonList[i].StartsWith(_var))
            {
                string substring = CommonList[i].Substring(_var.Length);
                if(!string.IsNullOrEmpty(substring))
                {
                    _variable = int.Parse(substring);
                    Debug.Log("TC_C#-------parseVariable------" + _var + "= " + _variable);
                }
                else if(_canEmpty == false)
                {
                    TeamCityBuildAPK.stopTeamCityTask($"{_var}不能为空");
                }
            }
        }
    }

    //获取变量名
    public static string GetVarName<T>(System.Linq.Expressions.Expression<Func<T, T>> exp)
    {
        return ((System.Linq.Expressions.MemberExpression)exp.Body).Member.Name;

    }
}