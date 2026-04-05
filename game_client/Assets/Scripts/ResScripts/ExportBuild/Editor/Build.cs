using System;
using System.IO;
using System.Collections.Generic;
using ALPackage;
using UnityEditor;
using UnityEngine;
using SQLite4Unity3d;

namespace GOE
{
    public class Build
    {
        public const string ErrBeginTag = "<ERR_TEAM_CITY_TAG>";
        public const string ErrEndTag = "</ERR_TEAM_CITY_TAG>";

        /// <summary>
        /// 导出资源
        /// </summary>
        /// <param name="_versionNum"></param>
        /// <param name="_isBuildRefdata">是否正在到处refdata工程，是的话拷贝一份给服务端的refdata数据</param>
        /// <param name="_iisAssetDiffe">是否差异打包</param>
        /// <param name="_oldVersionUrl">旧version绝对路径</param>
        public static void exportAssetBundle(long _versionNum, bool _isBuildRefdata, bool _isAssetDiffe, string _oldVersionUrl)
        {
            try
            {
                if(EditorUserBuildSettings.androidETC2Fallback != AndroidETC2Fallback.Quality16Bit)
                    EditorUserBuildSettings.androidETC2Fallback = AndroidETC2Fallback.Quality16Bit;
                
                //检查特效shape使用的mesh未开启读写错误
                bool isSucCheckSfxMeshReadWrite = EditorUtil.checkAssetParticleUsedMeshSetting(out string _sfxMeshErrorInfo);
                if(!isSucCheckSfxMeshReadWrite)
                {
                    TeamCityBuildAPK.stopTeamCityTask($"有特效shape使用的mesh未开启读写：{_sfxMeshErrorInfo}" );
                    return;
                }
                
                //检查Bundle错误
                List<string> errorBundleList = EditorUtil.checkAssetBundleVariant();
                List<string> errorSameNameList = EditorUtil.checkSameNameResource();
                if((errorBundleList != null && errorBundleList.Count > 0) || (errorSameNameList != null && errorSameNameList.Count > 0))
                {
                    //有Bundle错误
                    if(errorBundleList != null && errorBundleList.Count > 0)
                        TeamCityBuildAPK.stopTeamCityTask("有Bundle的variant没有设置：" + errorBundleList.ToStringList());
                    if(errorSameNameList != null && errorSameNameList.Count > 0)
                        TeamCityBuildAPK.stopTeamCityTask("存在同一个Bundle里面有同名资源：" + errorSameNameList.ToStringList());
                    return;
                }
                
                
                Debug.LogWarning("MGBuild#---------开始打包资源，当前资源版本号为:" + _versionNum);
                //打包资源
                ALAssetBundleExportMenu.exportAsset(_versionNum, true, true);
                Debug.LogWarning("MGBuild#---------刷新工程资源");
                //刷新工程资源
                AssetDatabase.Refresh();
                Debug.LogWarning("MGBuild#---------导出给服务器使用的所有表数据");
                //==== 导出给服务器使用的所有表数据
                if(_isBuildRefdata)
                {
                    FireCreater.dlServerRef();
                }
                
                //==== 差异打包
                if(_isAssetDiffe)
                {
                    ALAssetBundleCommon.fallbackVersion(_oldVersionUrl);
                }
            }
            catch (Exception e)
            {
                TeamCityBuildAPK.stopTeamCityTask(e.ToString());
            }
        }
        
        public static void LogErrBuildTask(string _reason)
        {
            Debug.LogError($"{ErrBeginTag}{_reason}{ErrEndTag}");
        }
    }
}