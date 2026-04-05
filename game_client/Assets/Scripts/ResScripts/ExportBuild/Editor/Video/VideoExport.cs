using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace VideoExport
{
    /// <summary>
    /// 视频批量导出工具
    /// </summary>
    public class VideoExport
    {
        private VideoExportConfig _config;
        
        public VideoExport(VideoExportConfig config)
        {
            _config = config;
        }
        
        /// <summary>
        /// 加载导出记录
        /// </summary>
        private VideoExportRecordCollection LoadRecords(string recordFilePath)
        {
            if (File.Exists(recordFilePath))
            {
                try
                {
                    string json = File.ReadAllText(recordFilePath);
                    VideoExportRecordCollection recordCollection = JsonUtility.FromJson<VideoExportRecordCollection>(json);
                    if (recordCollection == null)
                    {
                        recordCollection = new VideoExportRecordCollection();
                    }
                    return recordCollection;
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogError($"加载导出记录失败: {e.Message}");
                    return new VideoExportRecordCollection();
                }
            }
            else
            {
                return new VideoExportRecordCollection();
            }
        }
        
        /// <summary>
        /// 保存导出记录
        /// </summary>
        private void SaveRecords(string recordFilePath, VideoExportRecordCollection recordCollection)
        {
            try
            {
                string directory = Path.GetDirectoryName(recordFilePath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                
                string json = JsonUtility.ToJson(recordCollection, true);
                File.WriteAllText(recordFilePath, json);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError($"保存导出记录失败: {e.Message}");
            }
        }
        
        /// <summary>
        /// 从路径中提取参数缩写
        /// 规则：查找--开头，以\或.结尾的字符串，使用最靠近文件的
        /// </summary>
        private string ExtractParamShortName(string fullPath)
        {
            // 将路径分割为各个部分
            string[] parts = fullPath.Split(new char[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries);
            
            // 从最后一个部分开始向前查找（最靠近文件的优先）
            for (int i = parts.Length - 1; i >= 0; i--)
            {
                string part = parts[i];
                
                // 使用正则表达式匹配--开头的参数
                // 匹配模式：--后面跟着字母数字下划线，直到遇到.或字符串结尾
                Match match = Regex.Match(part, @"--([a-zA-Z0-9_-]+)(?:\.|$)");
                if (match.Success)
                {
                    return match.Groups[1].Value;
                }
            }
            
            // 没找到则返回空，使用默认参数
            return null;
        }
        
        /// <summary>
        /// 从文件名中移除参数缩写标记
        /// </summary>
        private string GetCleanFileName(string fileName)
        {
            // 移除--参数部分
            string cleanName = Regex.Replace(fileName, @"--[a-zA-Z0-9_-]+", "");
            return cleanName;
        }
        
        /// <summary>
        /// 清理路径中的参数标记
        /// 处理如 abc/cd--def-a/bbb/ -> abc/cd/bbb/
        /// 以及 abc/--def-a/bbb/ -> abc/bbb/
        /// </summary>
        private string CleanDirectoryPath(string dirPath)
        {
            if (string.IsNullOrEmpty(dirPath))
                return dirPath;
            
            // 分割路径
            string[] parts = dirPath.Split(new char[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> cleanParts = new List<string>();
            
            foreach (string part in parts)
            {
                // 清理每个路径段中的参数标记
                string cleanPart = Regex.Replace(part, @"--[a-zA-Z0-9_-]+", "");
                
                // 只保留非空的路径段
                if (!string.IsNullOrEmpty(cleanPart))
                {
                    cleanParts.Add(cleanPart);
                }
            }
            
            // 重新组合路径
            return string.Join(Path.DirectorySeparatorChar.ToString(), cleanParts);
        }
        
        /// <summary>
        /// 判断是否需要导出
        /// </summary>
        private bool NeedExport(string originalPath, string exportPath, string relativePath, 
            string cleanFileName, string paramShortName, VideoExportRecordCollection recordCollection, out VideoExportRecord newRecord)
        {
            FileInfo originalInfo = new FileInfo(originalPath);
            long fileSize = originalInfo.Length;
            long modifyTime = new DateTimeOffset(originalInfo.LastWriteTime).ToUnixTimeSeconds();
            
            newRecord = new VideoExportRecord(cleanFileName, paramShortName, fileSize, modifyTime, relativePath);
            
            // 如果导出文件不存在，需要导出
            if (!File.Exists(exportPath))
            {
                return true;
            }
            
            // 查找历史记录
            VideoExportRecord existingRecord = recordCollection.FindRecord(relativePath);
            if (existingRecord == null)
            {
                // 没有记录，需要导出
                return true;
            }
            
            // 比对记录
            if (!existingRecord.Matches(cleanFileName, paramShortName, fileSize, modifyTime))
            {
                Debug.Log($"记录不匹配：\n local:fileName:{cleanFileName}, paramName:{paramShortName}, fileSize:{fileSize}, time:{modifyTime}\n" +
                          $"record:fileName:{existingRecord.cleanFileName}, paramName:{existingRecord.paramShortName}, fileSize:{existingRecord.fileSize}, time:{existingRecord.modifyTime}");
                // 记录不匹配，需要重新导出
                return true;
            }
            
            // 记录匹配，跳过
            return false;
        }
        
        /// <summary>
        /// 执行单个视频导出
        /// </summary>
        private bool ExportSingleVideo(string inputPath, string outputPath, string exportParams)
        {
            try
            {
                string batFilePath = Path.Combine(Application.dataPath, _config.batScriptPath);
                if (!File.Exists(batFilePath))
                {
                    UnityEngine.Debug.LogError($"Bat脚本不存在: {batFilePath}");
                    return false;
                }
                
                // 确保输出目录存在
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                bool realEncode = true;
                if (realEncode)
                {
                    Process process = new Process();
                    process.StartInfo.FileName = batFilePath;
                    process.StartInfo.WorkingDirectory = Path.GetDirectoryName(batFilePath);
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;
                    process.StartInfo.CreateNoWindow = true;

                    // 传递参数：输入路径 输出路径 导出参数
                    if(Path.GetExtension(inputPath) == ".webm")
                        process.StartInfo.Arguments = $" -y -c:v libvpx -i \"{inputPath}\" {exportParams} \"{outputPath}\"";
                    else
                        process.StartInfo.Arguments = $" -y -i \"{inputPath}\" {exportParams} \"{outputPath}\"";
                    
                    UnityEngine.Debug.Log($"执行命令: {batFilePath} {process.StartInfo.Arguments}");

                    process.OutputDataReceived += (sender, args) =>
                    {
                        if (!string.IsNullOrEmpty(args.Data))
                        {
                            UnityEngine.Debug.Log($"[FFmpeg] {args.Data}");
                        }
                    };
                    process.ErrorDataReceived += (sender, args) =>
                    {
                        if (!string.IsNullOrEmpty(args.Data))
                        {
                            UnityEngine.Debug.LogWarning($"[FFmpeg] {args.Data}");
                        }
                    };

                    process.Start();
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();
                    process.WaitForExit();

                    int exitCode = process.ExitCode;
                    process.Close();

                    if (exitCode == 0)
                    {
                        UnityEngine.Debug.Log($"视频导出成功: {Path.GetFileName(outputPath)}");
                        return true;
                    }
                    else
                    {
                        UnityEngine.Debug.LogError($"视频导出失败，退出代码: {exitCode}");
                        return false;
                    }
                }
                else
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError($"执行导出时发生错误: {e.Message}\n{e.StackTrace}");
                return false;
            }
        }

        public void BatchExport()
        {
            UnityEngine.Debug.Log("\n========================================");
            UnityEngine.Debug.Log("开始批量导出视频（VP8 + H.264 双格式）");
            UnityEngine.Debug.Log("========================================\n");
            
            // // 先全部导出vp8
            // var vp8Result = BatchExportVP8(_config.exportVideoFolderVP8, _config.exportRecordPathVP8);
            //
            // // 检查是否取消
            // if (vp8Result.cancelled)
            // {
            //     EditorUtility.ClearProgressBar();
            //     UnityEngine.Debug.LogWarning("\n用户取消了导出操作");
            //     EditorUtility.DisplayDialog("导出已取消", 
            //         $"VP8 格式导出已取消\n\n" +
            //         $"已完成：{vp8Result.exported} 个\n" +
            //         $"已跳过：{vp8Result.skipped} 个", 
            //         "确定");
            //     return;
            // }
            
            // 再全部导出h264
            var h264Result = BatchExportH264(_config.exportVideoFolderH264, _config.exportRecordPathH264);
            
            // 检查是否取消
            if (h264Result.cancelled)
            {
                EditorUtility.ClearProgressBar();
                UnityEngine.Debug.LogWarning("\n用户取消了导出操作");
                
            
                EditorUtility.DisplayDialog("导出已取消", 
                    $"H.264 格式导出已取消\n\n" +
                    // $"VP8 格式：已完成 {vp8Result.exported} 个\n" +
                    $"H.264 格式：已完成 {h264Result.exported} 个（未完成）\n\n" +
                    // $"总计已导出：{vp8Result.exported + h264Result.exported} 个"
                    "", 
                    "确定");
                return;
            }
            // // 统计总结果
            // int totalExported = vp8Result.exported + h264Result.exported;
            // int totalSkipped = vp8Result.skipped + h264Result.skipped;
            // int totalFailed = vp8Result.failed + h264Result.failed;
            // 统计总结果
            int totalExported =  h264Result.exported;
            int totalSkipped =  h264Result.skipped;
            int totalFailed =  h264Result.failed;
            
            UnityEngine.Debug.Log("\n========================================");
            UnityEngine.Debug.Log("=== 全部导出完成 ===");
            UnityEngine.Debug.Log("========================================");
            // UnityEngine.Debug.Log($"VP8格式  - 导出: {vp8Result.exported} | 跳过: {vp8Result.skipped} | 失败: {vp8Result.failed}");
            UnityEngine.Debug.Log($"H.264格式 - 导出: {h264Result.exported} | 跳过: {h264Result.skipped} | 失败: {h264Result.failed}");
            UnityEngine.Debug.Log($"总计     - 导出: {totalExported} | 跳过: {totalSkipped} | 失败: {totalFailed}");
            UnityEngine.Debug.Log("========================================\n");
            
            // 显示最终结果对话框
            if (totalFailed == 0 && totalExported > 0)
            {
                EditorUtility.DisplayDialog("批量导出完成", 
                    $"✓ 成功导出 {totalExported} 个视频文件\n\n" +
                    // $"  VP8 格式：{vp8Result.exported} 个\n" +
                    $"  H.264 格式：{h264Result.exported} 个\n\n" +
                    $"跳过 {totalSkipped} 个未修改的文件", 
                    "确定");
            }
            else if (totalFailed > 0)
            {
                EditorUtility.DisplayDialog("批量导出完成（有错误）", 
                    $"⚠ 导出过程中出现错误\n\n" +
                    $"成功：{totalExported} 个\n" +
                    // $"  VP8 格式：{vp8Result.exported} 个\n" +
                    $"  H.264 格式：{h264Result.exported} 个\n\n" +
                    $"跳过：{totalSkipped} 个\n" +
                    $"失败：{totalFailed} 个\n\n" +
                    $"请查看 Console 了解详细错误信息", 
                    "确定");
            }
            else
            {
                EditorUtility.DisplayDialog("批量导出完成", 
                    $"所有视频文件都已是最新版本\n\n" +
                    // $"VP8 格式：{vp8Result.skipped} 个文件已是最新\n" +
                    $"H.264 格式：{h264Result.skipped} 个文件已是最新\n\n" +
                    $"无需重新导出", 
                    "确定");
            }
        }
        
        /// <summary>
        /// 复制视频文件到导出路径
        /// 文件名格式：vc_[mainid]_[subid].ext
        /// 目标路径格式：_targetPath/[mainid]/vc_[mainid]_[subid].ext
        /// </summary>
        /// <param name="sourceFolder">源文件夹路径</param>
        /// <param name="targetPath">目标文件夹路径</param>
        public void CopyFileToExportPath(string sourceFolder, string targetPath, bool _changeExtension = false, string _extension = ".gobvc")
        {
            if (!Directory.Exists(sourceFolder))
            {
                UnityEngine.Debug.LogError($"源文件夹不存在: {sourceFolder}");
                return;
            }

            UnityEngine.Debug.Log($"\n开始复制视频文件到导出路径...");
            UnityEngine.Debug.Log($"源路径: {sourceFolder}");
            UnityEngine.Debug.Log($"目标路径: {targetPath}");
            
            // 获取所有视频文件
            List<string> videoFiles = new List<string>();
            foreach (string ext in _config.videoExtensions)
            {
                string[] files = Directory.GetFiles(sourceFolder, $"*{ext}", SearchOption.AllDirectories);
                videoFiles.AddRange(files);
            }
            
            UnityEngine.Debug.Log($"找到 {videoFiles.Count} 个视频文件\n");
            
            int copiedCount = 0;
            int skippedCount = 0;
            int errorCount = 0;
            
            // 正则表达式匹配 vc_mainid_subid 格式
            Regex regex = new Regex(@"^vc_(\d+)_(\d+)", RegexOptions.IgnoreCase);
            
            for (int i = 0; i < videoFiles.Count; i++)
            {
                string sourceFile = videoFiles[i];
                string sourceFileName = Path.GetFileName(sourceFile);
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(sourceFile);
                string fileExt = Path.GetExtension(fileNameWithoutExtension);
                if (_changeExtension)
                    fileExt = _extension;
                string targetFileName = fileNameWithoutExtension + fileExt;
                
                // 匹配文件名格式
                Match match = regex.Match(targetFileName);
                if (match.Success)
                {
                    string mainId = match.Groups[1].Value;
                    string subId = match.Groups[2].Value;
                    
                    // 构建目标路径: targetPath/mainid/vc_mainid_subid.ext
                    string targetDir = Path.Combine(targetPath, mainId);
                    string targetFile = Path.Combine(targetDir, targetFileName);
                    
                    try
                    {
                        // 确保目标目录存在
                        if (!Directory.Exists(targetDir))
                        {
                            Directory.CreateDirectory(targetDir);
                        }
                        
                        // 检查文件是否已存在且相同
                        bool needCopy = true;
                        if (File.Exists(targetFile))
                        {
                            FileInfo sourceInfo = new FileInfo(sourceFile);
                            FileInfo targetInfo = new FileInfo(targetFile);
                            
                            // 比较文件大小和修改时间
                            if (sourceInfo.Length == targetInfo.Length && 
                                sourceInfo.LastWriteTime == targetInfo.LastWriteTime)
                            {
                                needCopy = false;
                                UnityEngine.Debug.Log($"[{i + 1}/{videoFiles.Count}] ⊘ 跳过（已存在且相同）: {sourceFileName} -> {mainId}/{targetFileName}");
                                skippedCount++;
                            }
                        }
                        
                        if (needCopy)
                        {
                            EditorUtility.DisplayProgressBar("复制视频文件", 
                                $"正在复制 ({i + 1}/{videoFiles.Count}): {targetFileName}", 
                                (float)i / videoFiles.Count);
                            
                            File.Copy(sourceFile, targetFile, true);
                            
                            // 保持原文件的修改时间
                            File.SetLastWriteTime(targetFile, File.GetLastWriteTime(sourceFile));
                            
                            UnityEngine.Debug.Log($"[{i + 1}/{videoFiles.Count}] ✓ 复制成功: {sourceFileName} -> {mainId}/{targetFileName}");
                            copiedCount++;
                        }
                    }
                    catch (Exception e)
                    {
                        UnityEngine.Debug.LogError($"[{i + 1}/{videoFiles.Count}] ✗ 复制失败: {targetFileName} - {e.Message}");
                        errorCount++;
                    }
                }
                else
                {
                    UnityEngine.Debug.LogWarning($"[{i + 1}/{videoFiles.Count}] ⚠ 文件名格式不匹配（非 vc_mainid_subid 格式）: {targetFileName}");
                    skippedCount++;
                }
            }
            
            EditorUtility.ClearProgressBar();
            
            UnityEngine.Debug.Log($"\n--- 视频文件复制完成 ---");
            UnityEngine.Debug.Log($"总计: {videoFiles.Count} 个文件");
            UnityEngine.Debug.Log($"✓ 复制: {copiedCount} 个");
            UnityEngine.Debug.Log($"⊘ 跳过: {skippedCount} 个");
            UnityEngine.Debug.Log($"✗ 错误: {errorCount} 个");
            
            // 显示结果对话框
            if (errorCount == 0 && copiedCount > 0)
            {
                EditorUtility.DisplayDialog("复制完成", 
                    $"✓ 成功复制 {copiedCount} 个视频文件\n" +
                    $"⊘ 跳过 {skippedCount} 个文件", 
                    "确定");
            }
            else if (errorCount > 0)
            {
                EditorUtility.DisplayDialog("复制完成（有错误）", 
                    $"✓ 成功: {copiedCount} 个\n" +
                    $"⊘ 跳过: {skippedCount} 个\n" +
                    $"✗ 失败: {errorCount} 个\n\n" +
                    $"请查看 Console 了解详细错误信息", 
                    "确定");
            }
            else
            {
                EditorUtility.DisplayDialog("复制完成", 
                    $"所有文件都已存在且相同\n" +
                    $"跳过 {skippedCount} 个文件", 
                    "确定");
            }
        }
        
        /// <summary>
        /// 导出结果
        /// </summary>
        public struct ExportResult
        {
            public int exported;
            public int skipped;
            public int failed;
            public bool cancelled;
            
            public ExportResult(int exported, int skipped, int failed, bool cancelled = false)
            {
                this.exported = exported;
                this.skipped = skipped;
                this.failed = failed;
                this.cancelled = cancelled;
            }
        }

        /// <summary>
        /// 同步删除导出文件夹中不存在于原始文件夹的文件
        /// </summary>
        /// <param name="exportFolder">导出文件夹路径</param>
        /// <param name="validExportFilePaths">有效的导出文件路径集合（完整路径）</param>
        /// <param name="targetExtension">目标文件扩展名（如.webm或.mp4）</param>
        /// <param name="recordCollection">记录集合，用于删除对应的记录</param>
        /// <returns>删除的文件数量</returns>
        private int SyncDeleteExportedFiles(string exportFolder, HashSet<string> validExportFilePaths, 
            string targetExtension, VideoExportRecordCollection recordCollection)
        {
            if (!Directory.Exists(exportFolder))
            {
                return 0;
            }
            
            int deletedCount = 0;
            
            // 获取导出文件夹中所有指定扩展名的文件
            string[] exportedFiles = Directory.GetFiles(exportFolder, $"*{targetExtension}", SearchOption.AllDirectories);
            
            foreach (string exportedFile in exportedFiles)
            {
                // 标准化路径以便比较（转为小写，统一路径分隔符）
                string normalizedExportedFile = exportedFile.ToLower().Replace('/', '\\');
                
                // 如果这个导出文件不在有效路径集合中，说明原始文件已被删除
                if (!validExportFilePaths.Contains(normalizedExportedFile))
                {
                    try
                    {
                        // 删除文件
                        File.Delete(exportedFile);
                        UnityEngine.Debug.Log($"🗑 已删除（原始文件不存在）: {exportedFile}");
                        deletedCount++;
                        
                        // 从记录中删除（需要找到对应的relativePath）
                        // 这里简化处理，实际可以通过反向查找relativePath来精确删除记录
                        // 为了安全起见，我们可以在保存记录时清理无效记录
                    }
                    catch (Exception e)
                    {
                        UnityEngine.Debug.LogError($"删除文件失败: {exportedFile} - {e.Message}");
                    }
                }
            }
            
            // 清理空文件夹
            CleanEmptyDirectories(exportFolder);
            
            return deletedCount;
        }
        
        /// <summary>
        /// 递归清理空文件夹
        /// </summary>
        private void CleanEmptyDirectories(string rootPath)
        {
            if (!Directory.Exists(rootPath))
                return;
            
            try
            {
                foreach (string directory in Directory.GetDirectories(rootPath))
                {
                    CleanEmptyDirectories(directory);
                    
                    // 如果文件夹为空（没有文件和子文件夹），删除它
                    if (Directory.GetFiles(directory).Length == 0 && 
                        Directory.GetDirectories(directory).Length == 0)
                    {
                        Directory.Delete(directory);
                        UnityEngine.Debug.Log($"🗑 已删除空文件夹: {directory}");
                    }
                }
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError($"清理空文件夹时出错: {e.Message}");
            }
        }

        /// <summary>
        /// 批量导出视频（VP8格式）
        /// </summary>
        public ExportResult BatchExportVP8(string _exportFolder, string _recordPath)
        {
            string originalFolder = Path.Combine(Application.dataPath, _config.originalVideoFolder);
            string exportFolder = Path.Combine(Application.dataPath, _exportFolder);
            string recordFilePath = Path.Combine(Application.dataPath, _recordPath);
            
            // 加载记录
            VideoExportRecordCollection recordCollection = LoadRecords(recordFilePath);
            
            if (!Directory.Exists(originalFolder))
            {
                UnityEngine.Debug.LogError($"原始视频文件夹不存在: {originalFolder}");
                return new ExportResult(0, 0, 0);
            }
            
            UnityEngine.Debug.Log($"\n开始批量导出视频（VP8 格式 - WebM 容器）...");
            UnityEngine.Debug.Log($"原始路径: {originalFolder}");
            UnityEngine.Debug.Log($"导出路径: {exportFolder}");
            
            // 获取所有视频文件
            List<string> videoFiles = new List<string>();
            foreach (string ext in _config.videoExtensions)
            {
                string[] files = Directory.GetFiles(originalFolder, $"*{ext}", SearchOption.AllDirectories);
                videoFiles.AddRange(files);
            }
            
            UnityEngine.Debug.Log($"找到 {videoFiles.Count} 个视频文件\n");
            
            int exportedCount = 0;
            int skippedCount = 0;
            int failedCount = 0;
            
            // 用于检测重名的字典：key=导出文件路径（小写），value=原始文件路径
            Dictionary<string, string> exportPathMap = new Dictionary<string, string>();
            // 用于同步删除的集合：存储所有应该存在的导出文件完整路径（标准化后的小写路径）
            HashSet<string> validExportFilePaths = new HashSet<string>();
            
            for (int i = 0; i < videoFiles.Count; i++)
            {
                string originalPath = videoFiles[i];
                string fileName = Path.GetFileName(originalPath);
                
                // 计算相对路径
                string relativePath = originalPath.Substring(originalFolder.Length).TrimStart('\\', '/');
                
                // 提取参数缩写
                string paramShortName = ExtractParamShortName(originalPath);
                if (string.IsNullOrEmpty(paramShortName))
                {
                    paramShortName = _config.defaultParamShortName;
                }
                
                // 获取详细参数（VP8格式）
                string detailParams = _config.GetDetailParams(paramShortName);
                
                // 获取清理后的文件名（用于记录）
                string cleanFileName = GetCleanFileName(fileName);
                
                // 构建导出路径（保持目录结构，但使用清理后的文件名）
                string relativeDir = Path.GetDirectoryName(relativePath);
                // 清理目录路径中的参数标记
                relativeDir = CleanDirectoryPath(relativeDir);
                
                string exportFileName = GetCleanFileName(fileName);
                // 将扩展名改为.webm（VP8格式）
                exportFileName = Path.ChangeExtension(exportFileName, ".webm");
                
                string exportPath = string.IsNullOrEmpty(relativeDir) 
                    ? Path.Combine(exportFolder, exportFileName)
                    : Path.Combine(exportFolder, relativeDir, exportFileName);
                
                // 检测重名问题（使用cleanFileName）
                string uniqueKey = cleanFileName.ToLower();
                
                if (exportPathMap.ContainsKey(uniqueKey))
                {
                    UnityEngine.Debug.LogError($"[VP8 {i + 1}/{videoFiles.Count}] ✗ 检测到重名冲突！");
                    UnityEngine.Debug.LogError($"  清理后文件名: {cleanFileName}");
                    UnityEngine.Debug.LogError($"  原始文件1: {exportPathMap[uniqueKey]}");
                    UnityEngine.Debug.LogError($"  原始文件2: {originalPath}");
                    UnityEngine.Debug.LogError($"  导出路径: {exportPath}");
                    failedCount++;
                    continue;
                }
                exportPathMap[uniqueKey] = originalPath;
                
                // 将导出路径添加到有效路径集合（标准化为小写）
                validExportFilePaths.Add(exportPath.ToLower().Replace('/', '\\'));
                
                // 判断是否需要导出
                VideoExportRecord newRecord;
                if (NeedExport(originalPath, exportPath, relativePath, cleanFileName, paramShortName, recordCollection, out newRecord))
                {
                    // 使用可取消的进度条
                    bool userCancelled = EditorUtility.DisplayCancelableProgressBar(
                        "批量导出视频（VP8）", 
                        $"正在导出 ({i + 1}/{videoFiles.Count}): {fileName}", 
                        (float)i / videoFiles.Count);
                    
                    // 检查用户是否取消
                    if (userCancelled)
                    {
                        UnityEngine.Debug.LogWarning($"\n[VP8] 用户在第 {i + 1}/{videoFiles.Count} 个文件时取消了导出");
                        EditorUtility.ClearProgressBar();
                        return new ExportResult(exportedCount, skippedCount, failedCount, true);
                    }
                    
                    UnityEngine.Debug.Log($"[VP8 {i + 1}/{videoFiles.Count}] 导出: {fileName}");
                    UnityEngine.Debug.Log($"  → 参数: {paramShortName} ({detailParams})");
                    
                    bool success = ExportSingleVideo(originalPath, exportPath, detailParams);
                    if (success)
                    {
                        // 更新记录
                        recordCollection.AddOrUpdateRecord(newRecord);
                        SaveRecords(recordFilePath, recordCollection);
                        exportedCount++;
                        UnityEngine.Debug.Log($"  ✓ 导出成功");
                    }
                    else
                    {
                        failedCount++;
                        UnityEngine.Debug.LogError($"  ✗ 导出失败");
                    }
                }
                else
                {
                    UnityEngine.Debug.Log($"[VP8 {i + 1}/{videoFiles.Count}] ⊘ 跳过（已是最新）: {fileName}");
                    skippedCount++;
                }
            }
            
            EditorUtility.ClearProgressBar();
            
            // 同步删除：删除导出文件夹中不存在于原始文件夹的文件
            int deletedCount = SyncDeleteExportedFiles(exportFolder, validExportFilePaths, ".webm", recordCollection);
            
            // 保存更新后的记录
            SaveRecords(recordFilePath, recordCollection);
            
            UnityEngine.Debug.Log($"\n--- VP8 格式导出完成 ---");
            UnityEngine.Debug.Log($"总计: {videoFiles.Count} 个文件");
            UnityEngine.Debug.Log($"✓ 导出: {exportedCount} 个");
            UnityEngine.Debug.Log($"⊘ 跳过: {skippedCount} 个");
            UnityEngine.Debug.Log($"✗ 失败: {failedCount} 个");
            UnityEngine.Debug.Log($"🗑 删除: {deletedCount} 个（原始文件已不存在）");
            
            return new ExportResult(exportedCount, skippedCount, failedCount);
        }

        /// <summary>
        /// 批量导出视频（H.264格式）
        /// </summary>
        public ExportResult BatchExportH264(string _exportFolder, string _recordPath)
        {
            string originalFolder = Path.Combine(Application.dataPath, _config.originalVideoFolder);
            string exportFolder = Path.Combine(Application.dataPath, _exportFolder);
            string recordFilePath = Path.Combine(Application.dataPath, _recordPath);
            
            // 加载记录
            VideoExportRecordCollection recordCollection = LoadRecords(recordFilePath);
            
            if (!Directory.Exists(originalFolder))
            {
                UnityEngine.Debug.LogError($"原始视频文件夹不存在: {originalFolder}");
                return new ExportResult(0, 0, 0);
            }
            
            UnityEngine.Debug.Log($"\n开始批量导出视频（H.264 格式 - MP4 容器）...");
            UnityEngine.Debug.Log($"原始路径: {originalFolder}");
            UnityEngine.Debug.Log($"导出路径: {exportFolder}");
            
            // 获取所有视频文件
            List<string> videoFiles = new List<string>();
            foreach (string ext in _config.videoExtensions)
            {
                string[] files = Directory.GetFiles(originalFolder, $"*{ext}", SearchOption.AllDirectories);
                videoFiles.AddRange(files);
            }
            
            UnityEngine.Debug.Log($"找到 {videoFiles.Count} 个视频文件\n");
            
            int exportedCount = 0;
            int skippedCount = 0;
            int failedCount = 0;
            
            // 用于检测重名的字典：key=cleanFileName（小写），value=原始文件路径
            Dictionary<string, string> exportPathMap = new Dictionary<string, string>();
            // 用于同步删除的集合：存储所有应该存在的导出文件完整路径（标准化后的小写路径）
            HashSet<string> validExportFilePaths = new HashSet<string>();
            
            for (int i = 0; i < videoFiles.Count; i++)
            {
                string originalPath = videoFiles[i];
                string fileName = Path.GetFileName(originalPath);
                
                // 计算相对路径
                string relativePath = originalPath.Substring(originalFolder.Length).TrimStart('\\', '/');
                
                // 提取参数缩写
                string paramShortName = ExtractParamShortName(originalPath);
                if (string.IsNullOrEmpty(paramShortName))
                {
                    paramShortName = _config.defaultParamShortName;
                }
                
                // 获取详细参数（H.264格式）
                string detailParams = _config.GetDetailParamsH264(paramShortName);
                
                // 获取清理后的文件名（用于记录）
                string cleanFileName = GetCleanFileName(fileName);
                
                // 构建导出路径（保持目录结构，但使用清理后的文件名）
                string relativeDir = Path.GetDirectoryName(relativePath);
                // 清理目录路径中的参数标记
                relativeDir = CleanDirectoryPath(relativeDir);
                
                string exportFileName = GetCleanFileName(fileName);
                // 将扩展名改为.mp4（H.264格式）
                exportFileName = Path.ChangeExtension(exportFileName, ".mp4");
                
                string exportPath = string.IsNullOrEmpty(relativeDir) 
                    ? Path.Combine(exportFolder, exportFileName)
                    : Path.Combine(exportFolder, relativeDir, exportFileName);
                
                // 检测重名问题（使用cleanFileName）
                string uniqueKey = cleanFileName.ToLower();
                
                if (exportPathMap.ContainsKey(uniqueKey))
                {
                    UnityEngine.Debug.LogError($"[H.264 {i + 1}/{videoFiles.Count}] ✗ 检测到重名冲突！");
                    UnityEngine.Debug.LogError($"  清理后文件名: {cleanFileName}");
                    UnityEngine.Debug.LogError($"  原始文件1: {exportPathMap[uniqueKey]}");
                    UnityEngine.Debug.LogError($"  原始文件2: {originalPath}");
                    UnityEngine.Debug.LogError($"  导出路径: {exportPath}");
                    failedCount++;
                    continue;
                }
                exportPathMap[uniqueKey] = originalPath;
                
                // 将导出路径添加到有效路径集合（标准化为小写）
                validExportFilePaths.Add(exportPath.ToLower().Replace('/', '\\'));
                
                // 判断是否需要导出
                VideoExportRecord newRecord;
                if (NeedExport(originalPath, exportPath, relativePath, cleanFileName, paramShortName, recordCollection, out newRecord))
                {
                    // 使用可取消的进度条
                    bool userCancelled = EditorUtility.DisplayCancelableProgressBar(
                        "批量导出视频（H.264）", 
                        $"正在导出 ({i + 1}/{videoFiles.Count}): {fileName}", 
                        (float)i / videoFiles.Count);
                    
                    // 检查用户是否取消
                    if (userCancelled)
                    {
                        UnityEngine.Debug.LogWarning($"\n[H.264] 用户在第 {i + 1}/{videoFiles.Count} 个文件时取消了导出");
                        EditorUtility.ClearProgressBar();
                        return new ExportResult(exportedCount, skippedCount, failedCount, true);
                    }
                    
                    UnityEngine.Debug.Log($"[H.264 {i + 1}/{videoFiles.Count}] 导出: {fileName}");
                    UnityEngine.Debug.Log($"  → 参数: {paramShortName} ({detailParams})");
                    
                    bool success = ExportSingleVideo(originalPath, exportPath, detailParams);
                    if (success)
                    {
                        // 更新记录
                        recordCollection.AddOrUpdateRecord(newRecord);
                        SaveRecords(recordFilePath, recordCollection);
                        exportedCount++;
                        UnityEngine.Debug.Log($"  ✓ 导出成功");
                    }
                    else
                    {
                        failedCount++;
                        UnityEngine.Debug.LogError($"  ✗ 导出失败");
                    }
                }
                else
                {
                    UnityEngine.Debug.Log($"[H.264 {i + 1}/{videoFiles.Count}] ⊘ 跳过（已是最新）: {fileName}");
                    skippedCount++;
                }
            }
            
            EditorUtility.ClearProgressBar();
            
            // 同步删除：删除导出文件夹中不存在于原始文件夹的文件
            int deletedCount = SyncDeleteExportedFiles(exportFolder, validExportFilePaths, ".mp4", recordCollection);
            
            // 保存更新后的记录
            SaveRecords(recordFilePath, recordCollection);
            
            UnityEngine.Debug.Log($"\n--- H.264 格式导出完成 ---");
            UnityEngine.Debug.Log($"总计: {videoFiles.Count} 个文件");
            UnityEngine.Debug.Log($"✓ 导出: {exportedCount} 个");
            UnityEngine.Debug.Log($"⊘ 跳过: {skippedCount} 个");
            UnityEngine.Debug.Log($"✗ 失败: {failedCount} 个");
            UnityEngine.Debug.Log($"🗑 删除: {deletedCount} 个（原始文件已不存在）");
            
            return new ExportResult(exportedCount, skippedCount, failedCount);
        }
    }
}