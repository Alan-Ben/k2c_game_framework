using System;
using System.Collections.Generic;
using UnityEngine;

namespace VideoExport
{
    /// <summary>
    /// 单个视频的导出记录
    /// </summary>
    [Serializable]
    public class VideoExportRecord
    {
        /// <summary>
        /// 原始视频文件名（不带--参数缩写）
        /// </summary>
        public string cleanFileName;
        
        /// <summary>
        /// 使用的参数缩写
        /// </summary>
        public string paramShortName;
        
        /// <summary>
        /// 原始视频文件大小（字节）
        /// </summary>
        public long fileSize;
        
        /// <summary>
        /// 原始视频文件修改时间（Unix时间戳）
        /// </summary>
        public long modifyTime;
        
        /// <summary>
        /// 原始视频相对路径（用于标识文件）
        /// </summary>
        public string relativePath;
        
        public VideoExportRecord()
        {
        }
        
        public VideoExportRecord(string cleanFileName, string paramShortName, long fileSize, long modifyTime, string relativePath)
        {
            this.cleanFileName = cleanFileName;
            this.paramShortName = paramShortName;
            this.fileSize = fileSize;
            this.modifyTime = modifyTime;
            this.relativePath = relativePath;
        }
        
        /// <summary>
        /// 判断记录是否匹配
        /// </summary>
        public bool Matches(string cleanFileName, string paramShortName, long fileSize, long modifyTime)
        {
            return this.cleanFileName == cleanFileName &&
                   this.paramShortName == paramShortName &&
                   this.fileSize == fileSize ;
        }
    }
    
    /// <summary>
    /// 所有视频导出记录集合
    /// </summary>
    [Serializable]
    public class VideoExportRecordCollection
    {
        public List<VideoExportRecord> records = new List<VideoExportRecord>();
        
        /// <summary>
        /// 根据相对路径查找记录
        /// </summary>
        public VideoExportRecord FindRecord(string relativePath)
        {
            return records.Find(r => r.relativePath == relativePath);
        }
        
        /// <summary>
        /// 添加或更新记录
        /// </summary>
        public void AddOrUpdateRecord(VideoExportRecord record)
        {
            var existingRecord = FindRecord(record.relativePath);
            if (existingRecord != null)
            {
                records.Remove(existingRecord);
            }
            records.Add(record);
        }
    }
}
