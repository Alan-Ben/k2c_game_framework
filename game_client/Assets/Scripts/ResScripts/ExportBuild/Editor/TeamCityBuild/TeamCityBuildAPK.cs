#if NP_GAME
#endif
using System.IO;
using UnityEditor;
using UnityEditor.Build.Content;
using UnityEngine;


public class TeamCityBuildAPK
{
    public static void exportTeamCityApk()
    {
        //获得传参
        string[] CommonList = System.Environment.GetCommandLineArgs();

        for (int i = 0; i < CommonList.Length; i++)
        {
            Debug.Log("TC_C#--------------------------------CommonList---------------------------" + i + "=" + CommonList[i]);
        }
        //设置所有TC会用到的变量
        TCSetting.settingVar(CommonList);

        Debug.Log("TC_C#----------------- TCSetting.apkUrl = " + TCSetting.apkUrl);
        Debug.Log("TC_C#----------------- TCSetting.resVer = " + TCSetting.resVer);
        Debug.Log("TC_C#----------------- TCSetting.platVersion = " + TCSetting.platVersion);
        Debug.Log("TC_C#----------------- TCSetting.GameName = " + TCSetting.GameName);
        

        //如果没传GameName，就使用工程里现在的名字
        if(string.IsNullOrEmpty(TCSetting.GameName))
        {
            TCClientExportFunc.setProductName(PlayerSettings.productName, TCSetting.packageName, TCSetting.isObb);
        }
        else
        {
            TCClientExportFunc.setProductName(TCSetting.GameName, TCSetting.packageName, TCSetting.isObb);
        }
#if UNITY_ANDROID
        //设置安卓key
        TCClientExportFunc.setKeyStoreInfo(TCSetting.keypath, TCSetting.keyPass, TCSetting.keyAli, TCSetting.keyAliasPass);
        //安卓il2cpp的打包设置
        if(TCSetting.il2cpp)
        {
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);

            if(TCSetting.containX86)
            {
                PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64 | AndroidArchitecture.ARMv7;
            }
            else
            {
                PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64 | AndroidArchitecture.ARMv7;   
            }
        }
        else
        {
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.Mono2x);
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7 | AndroidArchitecture.ARM64;
        }
#endif
        TCClientExportFunc.setVersionCode(TCSetting.ClientVer, TCSetting.VersionCoed);
        
        
        //TODO:根据参数传Internal还是Gradle，现在是分了不同工程，在打包机上设置好一个用internal，一个用gradle
        TCClientExportFunc.buildAPK(TCSetting.PlatName, TCSetting.platVersion, TCSetting.isSDK, TCSetting.apkUrl, TCSetting.clientLoginType,
            TCSetting.isShowSDKLog, TCSetting.isDevelopmentBuild, TCSetting.isAAB);
    }

    public static void stopTeamCityTask(string _reason)
    {
        string begin = "<STOP_TEAM_CITY_TAG>";
        string end = "</STOP_TEAM_CITY_TAG>";
        Debug.LogError($"{begin}{_reason}{end}");
    }
}