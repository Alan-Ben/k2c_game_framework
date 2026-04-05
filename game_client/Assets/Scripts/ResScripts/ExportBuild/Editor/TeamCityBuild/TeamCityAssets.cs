using System;
#if NP_GAME
using IFix.Editor;
#endif
using UnityEngine;
using UnityEditor;
using GOE;


class TeamCityAssets
{
    public static void TeamCityScriptError()
    {
        Debug.LogError(" Compile and pass!");
    }

    /// <summary>
    /// 因为Unity只支持调用无参静态函数，参数必须使用Environment.GetCommandLineArgs()传进来，所以这里只能这样写
    /// https://docs.unity3d.com/Manual/CommandLineArguments.html
    /// </summary>
    public static void exportTeamCityAsset()
    {
        try
        {
            string[] arguments = Environment.GetCommandLineArgs();

            if(arguments == null || arguments.Length < 1)
            {
                Debug.Log("TC_C#-----------------------------------------------------------python  传参数小于等于1:" + arguments.Length);
                return;
            }
            else
            {
                for (int i = 0; i < arguments.Length; i++)
                {
                    Debug.Log("TC_C#-----------------------------------------------------------" + i + ":  " + arguments[i]);
                }
            }
            //设置参数
            TCSetting.settingVar(arguments);
            //设置环境
            setBuildTarget();
            
            //"需要对AI文件进行一下Import"
            string srcPath = Application.dataPath + "/Resources/Refdata/__DLExport";
            //import 资源
            AssetDatabase.ImportAsset(srcPath);
            
            //打包资源
            Build.exportAssetBundle(TCSetting.resVer, TCSetting.isBuildRefdata, TCSetting.isAssetDiffe, TCSetting.oldVersionUrl);
            
            Debug.Log("TC_C#-----------------------------------------------------------资源导出完成！");
        }
        catch (Exception e)
        {
            TeamCityBuildAPK.stopTeamCityTask(e.ToString());
        }
    }

    /// <summary>
    /// 切换资源编译环境
    /// </summary>
    public static void setBuildTarget()
    {
        if (string.IsNullOrEmpty(TCSetting.EditorPlatform))
        {
            TeamCityBuildAPK.stopTeamCityTask("没有设置资源打包环境");
        }
        
        //////////////对运行环境进行设置
        if (TCSetting.EditorPlatform.Contains("android"))
        {
            if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android)
            {
                EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
            }
            if(EditorUserBuildSettings.androidETC2Fallback != AndroidETC2Fallback.Quality16Bit)
                EditorUserBuildSettings.androidETC2Fallback = AndroidETC2Fallback.Quality16Bit;
        }
        else if (TCSetting.EditorPlatform.Contains("ios"))
        {
            if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.iOS)
            {
                EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.iOS, BuildTarget.iOS);
            }
        }
        else if (TCSetting.EditorPlatform.Contains("StandaloneWindows") || TCSetting.EditorPlatform.Contains("pc"))
        {
            if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.StandaloneWindows)
            {
                EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows);
            }
        }
    }
    
    /// <summary>
    /// 直接导出全部平台的补丁
    /// </summary>
    public static void exportTeamCityInjecFixAllPlatform()
    {
#if NP_GAME
        Debug.LogError("TC_C#-----------------------------------------------------------InjectFix补丁PC开始导出");
        IFixEditor.Patch();
        Debug.LogError("TC_C#-----------------------------------------------------------InjectFix补丁PC导出完成！");

        Debug.LogError("TC_C#-----------------------------------------------------------InjectFix补丁Android开始导出");
        IFixEditor.CompileToAndroid();
        Debug.LogError("TC_C#-----------------------------------------------------------InjectFix补丁Android导出完成！");

        Debug.LogError("TC_C#-----------------------------------------------------------InjectFix补丁IOS开始导出");
        IFixEditor.CompileToIOS();
        Debug.LogError("TC_C#-----------------------------------------------------------InjectFix补丁IOS导出完成！");
#endif
    }
    
    /// <summary>
    /// 根据当前平台导出指定平台的补丁
    /// </summary>
    public static void exportTeamCityInjecFix()
    {
#if NP_GAME
        
#if UNITY_IPHONE
        Debug.LogError("TC_C#-----------------------------------------------------------InjectFix补丁IOS开始导出");
        IFix.Editor.IFixEditor.CompileToIOS();
#endif
#if UNITY_ANDROID
        Debug.LogError("TC_C#-----------------------------------------------------------InjectFix补丁Android开始导出");
        IFix.Editor.IFixEditor.CompileToAndroid();
#endif
#if UNITY_STANDALONE_WIN
        Debug.LogError("TC_C#-----------------------------------------------------------InjectFix补丁PC开始导出");
        IFixEditor.Patch();
#endif
        Debug.LogError("TC_C#-----------------------------------------------------------InjectFix补丁导出完成！");
        
#endif
    }
}