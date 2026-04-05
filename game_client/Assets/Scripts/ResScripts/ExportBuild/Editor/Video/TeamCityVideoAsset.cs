
using System;
using UnityEngine;
using VideoExport;

public class TeamCityVideoAsset
{
    /// <summary>
    /// 因为Unity只支持调用无参静态函数，参数必须使用Environment.GetCommandLineArgs()传进来，所以这里只能这样写
    /// https://docs.unity3d.com/Manual/CommandLineArguments.html
    /// </summary>
    public static void exportTeamCityVideoAsset()
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
            
            //打包资源
            oneKeyExportVideo();
            
            Debug.Log("TC_C#-----------------------------------------------------------资源导出完成！");
        }
        catch (Exception e)
        {
            TeamCityBuildAPK.stopTeamCityTask(e.ToString());
        }
    }
    public static void oneKeyExportVideo()
    {
        //////////////对运行环境进行设置
        if (TCSetting.EditorPlatform.Contains("android"))
        {
            VideoExportEditor.oneKeyExportVideo(TCSetting.resVer, VideoExportEditor.VideoPlat.Android);
        }
        else if (TCSetting.EditorPlatform.Contains("ios"))
        {
            VideoExportEditor.oneKeyExportVideo(TCSetting.resVer, VideoExportEditor.VideoPlat.IOS);
        }
        else if (TCSetting.EditorPlatform.Contains("StandaloneWindows") || TCSetting.EditorPlatform.Contains("pc"))
        {
            VideoExportEditor.oneKeyExportVideo(TCSetting.resVer, VideoExportEditor.VideoPlat.PC);
        }
    }
}
