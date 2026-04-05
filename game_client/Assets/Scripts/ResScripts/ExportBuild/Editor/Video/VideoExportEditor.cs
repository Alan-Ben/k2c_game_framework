using System.IO;
using ALPackage;
using UnityEditor;
using UnityEngine;

namespace VideoExport
{
    /// <summary>
    /// 视频导出编辑器菜单
    /// </summary>
    public class VideoExportEditor
    {
        private static VideoExportConfig _cachedConfig;
        
        /// <summary>
        /// 获取或创建配置
        /// </summary>
        public static VideoExportConfig GetOrCreateConfig()
        {
            if (_cachedConfig != null)
            {
                return _cachedConfig;
            }
            
            // 尝试从Assets中查找配置文件
            string[] guids = AssetDatabase.FindAssets("t:VideoExportConfig");
            if (guids.Length > 0)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                _cachedConfig = AssetDatabase.LoadAssetAtPath<VideoExportConfig>(path);
                return _cachedConfig;
            }
            
            // 如果没有找到，创建默认配置
            _cachedConfig = ScriptableObject.CreateInstance<VideoExportConfig>();
            string savePath = "Assets/Resources/Video/VideoExportConfig.asset";
            AssetDatabase.CreateAsset(_cachedConfig, savePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            Debug.Log($"已创建默认视频导出配置: {savePath}");
            return _cachedConfig;
        }
        
        [MenuItem("Tools/视频工具/批量导出视频（VP8 + H.264）", false, 100)]
        public static void BatchExportVideos()
        {
            VideoExportConfig config = GetOrCreateConfig();
            
            if (config == null)
            {
                EditorUtility.DisplayDialog("错误", "无法加载视频导出配置文件", "确定");
                return;
            }
            
            bool confirm = EditorUtility.DisplayDialog(
                "批量导出视频", 
                $"将导出两种格式的视频：\n\n" +
                $"【原始路径】\n{config.originalVideoFolder}\n\n" +
                $"【VP8 格式】（WebM 容器）\n{config.exportVideoFolderVP8}\n\n" +
                $"【H.264 格式】（MP4 容器）\n{config.exportVideoFolderH264}\n\n" +
                $"⚠ 注意：\n" +
                $"• 仅导出修改过或新增的视频\n" +
                $"• 支持文件名中的参数标记（--参数名）\n" +
                $"• 此操作可能需要较长时间\n\n" +
                $"是否开始导出？",
                "开始导出", 
                "取消"
            );
            
            if (!confirm)
            {
                return;
            }
            
            VideoExport exporter = new VideoExport(config);
            exporter.BatchExport();
        }
        
        [MenuItem("Tools/视频工具/打开视频导出配置", false, 101)]
        public static void OpenConfig()
        {
            VideoExportConfig config = GetOrCreateConfig();
            Selection.activeObject = config;
            EditorGUIUtility.PingObject(config);
        }
        
        [MenuItem("Tools/视频工具/清空导出记录", false, 102)]
        public static void ClearExportRecords()
        {
            VideoExportConfig config = GetOrCreateConfig();
            
            string recordPathVP8 = System.IO.Path.Combine(Application.dataPath, config.exportRecordPathVP8);
            string recordPathH264 = System.IO.Path.Combine(Application.dataPath, config.exportRecordPathH264);
            
            bool vp8Exists = System.IO.File.Exists(recordPathVP8);
            bool h264Exists = System.IO.File.Exists(recordPathH264);
            
            if (!vp8Exists && !h264Exists)
            {
                EditorUtility.DisplayDialog("提示", "导出记录文件不存在，无需清空", "确定");
                return;
            }
            
            string message = "清空后将重新导出所有视频文件。\n\n";
            message += "将清空以下记录：\n";
            if (vp8Exists) message += $"• VP8 记录：{config.exportRecordPathVP8}\n";
            if (h264Exists) message += $"• H.264 记录：{config.exportRecordPathH264}\n";
            message += "\n是否确定清空？";
            
            bool confirm = EditorUtility.DisplayDialog(
                "清空导出记录", 
                message,
                "确定清空", 
                "取消"
            );
            
            if (confirm)
            {
                int deletedCount = 0;
                try
                {
                    if (vp8Exists)
                    {
                        System.IO.File.Delete(recordPathVP8);
                        deletedCount++;
                        Debug.Log($"已清空 VP8 导出记录：{recordPathVP8}");
                    }
                    
                    if (h264Exists)
                    {
                        System.IO.File.Delete(recordPathH264);
                        deletedCount++;
                        Debug.Log($"已清空 H.264 导出记录：{recordPathH264}");
                    }
                    
                    EditorUtility.DisplayDialog("成功", $"已清空 {deletedCount} 个导出记录文件", "确定");
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"清空导出记录失败: {e.Message}");
                    EditorUtility.DisplayDialog("错误", $"清空失败：{e.Message}", "确定");
                }
            }
        }
        
        // [MenuItem("Tools/视频工具/打包Android视频", false, 103)]
        // public static void packAndroidVideo()
        // {
        //     oneKeyExportVideo(TCSetting.resVer, VideoPlat.Android);
        // }
        // [MenuItem("Tools/视频工具/打包IOS视频", false, 103)]
        // public static void packIOSVideo()
        // {
        //     oneKeyExportVideo(TCSetting.resVer, VideoPlat.IOS);
        // }
        // [MenuItem("Tools/视频工具/打包pc视频", false, 103)]
        // public static void packPcVideo()
        // {
        //     oneKeyExportVideo(TCSetting.resVer, VideoPlat.PC);
        // }
        //
        [MenuItem("Tools/视频工具/打包Android视频", false, 103)]
        public static void packAndroidVideoOnly()
        {
            oneKeyExportVideo(TCSetting.resVer, VideoPlat.Android, false);
        }
        [MenuItem("Tools/视频工具/打包IOS视频", false, 103)]
        public static void packIOSVideoOnly()
        {
            oneKeyExportVideo(TCSetting.resVer, VideoPlat.IOS, false);
        }
        [MenuItem("Tools/视频工具/打包pc视频", false, 103)]
        public static void packPcVideoOnly()
        {
            oneKeyExportVideo(TCSetting.resVer, VideoPlat.PC, false);
        }
        public enum VideoPlat
        {
            Android,
            IOS,
            PC,
        }

        public static void oneKeyExportVideo(long _versionNum, VideoPlat _plat, bool _encodeVideo = true)
        {
            VideoExportConfig config = GetOrCreateConfig();
            
            if (config == null)
            {
                Debug.LogError("无法加载视频导出配置文件");
                return;
            }      
            VideoExport exporter = new VideoExport(config);
            if(_encodeVideo)
                exporter.BatchExport();
        
            string targetPath = System.IO.Path.Combine(Application.dataPath, config.relativePackPath);
            if(config.useAbsolutePackPath)
                targetPath = config.absolutePackPath;
            
            if(Directory.Exists(targetPath))
                Directory.Delete(targetPath, true);
            
            string version;
            //修改版本号
            if(0 == _versionNum)
                version = ALCommon.getNowTimeTagS().ToString();
            else
                version = _versionNum.ToString();
            // 不同平台执行不同的处理
            if (_plat == VideoPlat.Android)
            {
                string sourceFolder = System.IO.Path.Combine(Application.dataPath, config.exportVideoFolderH264);
                exporter.CopyFileToExportPath(sourceFolder, targetPath, true, ".gobvc");
                // 开始构建
                new AudioBuilder.AudioBuilder("remote_video_version", version, targetPath).deal();
            }
            else if (_plat == VideoPlat.IOS)
            {
                string sourceFolder = System.IO.Path.Combine(Application.dataPath, config.exportVideoFolderH264);
                exporter.CopyFileToExportPath(sourceFolder, targetPath);
                // 开始构建
                new AudioBuilder.AudioBuilder("remote_video_version", version, targetPath).deal();
            }
            else if (_plat == VideoPlat.PC)
            {
                string sourceFolder = System.IO.Path.Combine(Application.dataPath, config.exportVideoFolderH264);
                exporter.CopyFileToExportPath(sourceFolder, targetPath, true, ".mp4");
        
                // 开始构建
                new AudioBuilder.AudioBuilder("remote_video_version", version, targetPath).deal();
            }
        }

        private static void packVideo()
        {
            
        }
        
    }
}
