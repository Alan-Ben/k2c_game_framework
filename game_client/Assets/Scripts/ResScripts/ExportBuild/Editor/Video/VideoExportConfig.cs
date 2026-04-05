using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace VideoExport
{
    /// <summary>
    /// 视频导出参数配置
    /// </summary>
    [Serializable]
    public class VideoExportParamConfig
    {
        /// <summary>
        /// 参数缩写（如：hd, sd, ld）
        /// </summary>
        public string shortName;
        
        /// <summary>
        /// VP8格式的详细导出参数（如：-c:v libvpx -b:v 2M -crf 10）
        /// </summary>
        public string detailParamsVP8;
        
        /// <summary>
        /// H.264格式的详细导出参数（如：-c:v libx264 -b:v 2M -crf 23）
        /// </summary>
        public string detailParamsH264;
    }

    /// <summary>
    /// 视频导出配置
    /// </summary>
    [CreateAssetMenu(fileName = "VideoExportConfig", menuName = "GOE/Video Export Config")]
    public class VideoExportConfig : ScriptableObject
    {
        /// <summary>
        /// bat脚本路径（相对于Assets目录）
        /// </summary>
        public string batScriptPath = "Scripts/video_export.bat";

        public string relativePackPath = "Resources/Video/__VideoExport~";
        // 使用绝对路径
        [Header("打包视频使用绝对路径")] public bool useAbsolutePackPath;
        public string absolutePackPath;
        
        /// <summary>
        /// 原始视频文件夹路径
        /// </summary>
        public string originalVideoFolder = "video_clip_original";
        
        /// <summary>
        /// VP8导出视频文件夹路径
        /// </summary>
        public string exportVideoFolderVP8 = "video_clip_vp8";
        
        /// <summary>
        /// VP8导出记录文件路径
        /// </summary>
        public string exportRecordPathVP8 = "video_clip_vp8/export_record.json";
        
        /// <summary>
        /// H.264导出视频文件夹路径
        /// </summary>
        public string exportVideoFolderH264 = "video_clip_h264";
        
        /// <summary>
        /// H.264导出记录文件路径
        /// </summary>
        public string exportRecordPathH264 = "video_clip_h264/export_record.json";
        
        /// <summary>
        /// 默认参数缩写
        /// </summary>
        public string defaultParamShortName = "default";
        
        /// <summary>
        /// 支持的视频文件扩展名
        /// </summary>
        public string[] videoExtensions = new string[] { ".mp4", ".avi", ".mov", ".mkv", ".webm" };
        
        /// <summary>
        /// 参数配置列表
        /// </summary>
        public List<VideoExportParamConfig> paramConfigs = new List<VideoExportParamConfig>()
        {
            new VideoExportParamConfig() 
            { 
                shortName = "default", 
                detailParamsVP8 = "-c:v libvpx -b:v 1M -crf 10",
                detailParamsH264 = "-c:v libx264 -b:v 1M -crf 23"
            },
            new VideoExportParamConfig() 
            { 
                shortName = "hd", 
                detailParamsVP8 = "-c:v libvpx -b:v 2M -crf 8",
                detailParamsH264 = "-c:v libx264 -b:v 2M -crf 20"
            },
            new VideoExportParamConfig() 
            { 
                shortName = "sd", 
                detailParamsVP8 = "-c:v libvpx -b:v 1M -crf 12",
                detailParamsH264 = "-c:v libx264 -b:v 1M -crf 23"
            },
            new VideoExportParamConfig() 
            { 
                shortName = "ld", 
                detailParamsVP8 = "-c:v libvpx -b:v 500K -crf 15",
                detailParamsH264 = "-c:v libx264 -b:v 500K -crf 26"
            }
        };
        
        /// <summary>
        /// 根据缩写获取详细参数（VP8格式）
        /// </summary>
        public string GetDetailParams(string shortName)
        {
            if (string.IsNullOrEmpty(shortName))
            {
                shortName = defaultParamShortName;
            }
            
            var config = paramConfigs.Find(c => c.shortName == shortName);
            if (config != null)
            {
                return config.detailParamsVP8;
            }
            
            // 如果没找到，返回默认参数
            config = paramConfigs.Find(c => c.shortName == defaultParamShortName);
            return config != null ? config.detailParamsVP8 : "";
        }
        
        /// <summary>
        /// 根据缩写获取详细参数（H.264格式）
        /// </summary>
        public string GetDetailParamsH264(string shortName)
        {
            if (string.IsNullOrEmpty(shortName))
            {
                shortName = defaultParamShortName;
            }
            
            var config = paramConfigs.Find(c => c.shortName == shortName);
            if (config != null)
            {
                return config.detailParamsH264;
            }
            
            // 如果没找到，返回默认参数
            config = paramConfigs.Find(c => c.shortName == defaultParamShortName);
            return config != null ? config.detailParamsH264 : "";
        }
    }
}
