using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Text;

using UnityEngine;
using UnityEditor;

/******************
 * 资源文件压缩打包的处理函数
 **/
namespace ALPackage
{
    public class ALAssetBundleCompressThreadController
    {
        /** 导出和目标目录的路径 */
        private string _m_sSrcFolderPath;
        private string _m_sExportFolderPath;
        //需要处理的文件路径
        private List<string> _m_lNeedExportFilePath;
        //需要删除的文件路径输出部分
        private List<string> _m_lNeedRemoveFilePath;

        //是否需要退出
        private bool _m_bNeedExitThread;
        //是否在运行
        private bool _m_bIsWorking;

        public ALAssetBundleCompressThreadController(string _srcFolderPath, string _exportFolderPath, List<string> _exportPath, List<string> _removePath)
        {
            _m_sSrcFolderPath = _srcFolderPath;
            _m_sExportFolderPath = _exportFolderPath;

            _m_lNeedExportFilePath = new List<string>();
            if (null != _exportPath)
                _m_lNeedExportFilePath.AddRange(_exportPath);
            _m_lNeedRemoveFilePath = new List<string>();
            if (null != _removePath)
                _m_lNeedRemoveFilePath.AddRange(_removePath);

            _m_bNeedExitThread = false;
            _m_bIsWorking = false;
        }

        public bool isWorking { get { return _m_bIsWorking; } }

        /*******************
         * 开启对应的线程
         **/
        public void startThread()
        {
            if (_m_bIsWorking)
                return;

            _m_bNeedExitThread = false;
            _m_bIsWorking = true;

            //初始化环境
            SevenZipProcess.init();
            //创建并开启线程
            ALThread thread = new ALThread(_threadPack);
            thread.startThread();
        }

        //同步处理任务
        public void syncDeal()
        {
            _threadPack();
        }

        /******************
         * 关闭线程对象
         **/
        public void stopThread()
        {
            _m_bNeedExitThread = true;
        }

        /***************
         * 打包的线程处理
         **/
        protected void _threadPack()
        {
            //判断路径是否存在
            if (!Directory.Exists(_m_sExportFolderPath))
                Directory.CreateDirectory(_m_sExportFolderPath);

            //将资源从源目录压缩到指定目录
            //删除需要删除的文件
            if (null != _m_lNeedRemoveFilePath)
            {
                StringBuilder allRemoveFileStr = new StringBuilder();
                allRemoveFileStr.Append("Need Delete: " + _m_lNeedRemoveFilePath.Count + " Files:\n");
                for (int i = 0; i < _m_lNeedRemoveFilePath.Count; i++)
                {
                    //判断是否需要退出
                    if (_m_bNeedExitThread)
                    {
                        if (null != _m_lNeedRemoveFilePath)
                            _m_lNeedRemoveFilePath.Clear();
                        _m_lNeedRemoveFilePath = null;
                        if (null != _m_lNeedExportFilePath)
                            _m_lNeedExportFilePath.Clear();
                        _m_lNeedExportFilePath = null;

                        _m_bIsWorking = false;
                        return;
                    }

                    string removeFileFullPath = _m_sExportFolderPath + "/" + _m_lNeedRemoveFilePath[i];
                    //删除多余文件
                    File.Delete(removeFileFullPath);

                    //添加到集合中一并输出
                    allRemoveFileStr.Append(removeFileFullPath + "\n");
                }

                UnityEngine.Debug.LogError(allRemoveFileStr.ToString());

                //清空数据
                _m_lNeedRemoveFilePath.Clear();
                _m_lNeedRemoveFilePath = null;
            }

            //查询源目录的所有文件
            DirectoryInfo srcDirectionInfo = new DirectoryInfo(_m_sSrcFolderPath);
            DirectoryInfo exportDirectionInfo = new DirectoryInfo(_m_sExportFolderPath);
            FileInfo[] allFiles = null;

            StringBuilder allFileStr = new StringBuilder();
            allFileStr.Append("Need Export: " + _m_lNeedExportFilePath.Count + " Files:\n");
            allFiles = new FileInfo[_m_lNeedExportFilePath.Count];
            for (int i = 0; i < _m_lNeedExportFilePath.Count; i++)
            {
                allFiles[i] = new FileInfo(_m_sSrcFolderPath + "/" + _m_lNeedExportFilePath[i]);

                //添加到集合中一并输出
                allFileStr.Append(_m_sSrcFolderPath + "/" + _m_lNeedExportFilePath[i] + "\n");
            }

            UnityEngine.Debug.LogError("Export Files:\n" + allFileStr.ToString());

            bool hasError = false;
            //逐个信息进行处理
            for (int i = 0; i < allFiles.Length; i++)
            {
                //判断是否需要退出
                if (_m_bNeedExitThread)
                {
                    if (null != _m_lNeedExportFilePath)
                        _m_lNeedExportFilePath.Clear();
                    _m_lNeedExportFilePath = null;

                    _m_bIsWorking = false;

                    //输出导出完毕
                    UnityEngine.Debug.LogError("Export Web Resource Exit By Operation!");

                    return;
                }

                //获取文件信息
                FileInfo file = allFiles[i];
                if (null == file)
                    continue;

                if (file.Name.Equals("version", StringComparison.OrdinalIgnoreCase))
                {
                    //version文件不进行处理
                    continue;
                }
                else
                {
                    //替换目录，在新文件位置创建目录
                    string newDirectory = file.DirectoryName.Replace(srcDirectionInfo.FullName, exportDirectionInfo.FullName);
                    //判断路径是否存在
                    if (!Directory.Exists(newDirectory))
                        Directory.CreateDirectory(newDirectory);

                    int retryCount = 0;
                    string error;

                    do
                    {
                        //判断是否重试
                        if (retryCount > 0)
                        {
                            UnityEngine.Debug.LogErrorFormat(newDirectory + "/" + file.Name + " 等待1秒重试");
                            Thread.Sleep(1000);
                        }

                        //将文件压缩
                        //ALLZMACommon.CompressFileLZMA(file.FullName, newDirectory + "/" + file.Name);
                        error = SevenZipProcess.CompressFileLZMA(file.FullName, newDirectory + "/" + file.Name);
                        //增加重试次数
                        retryCount++;
                    } while (!string.IsNullOrEmpty(error) && retryCount <= 3);

                    if (!string.IsNullOrEmpty(error))
                    {
                        UnityEngine.Debug.LogErrorFormat(newDirectory + "/" + file.Name + " 错误：{0}", error);
                        UnityEngine.Debug.LogErrorFormat(newDirectory + "/" + file.Name + " 错误：{0}", error);
                        UnityEngine.Debug.LogErrorFormat(newDirectory + "/" + file.Name + " 错误：{0}", error);

                        //抛出异常
                        throw new Exception("压缩失败");
                    }
                    else
                    {
                        //进行压缩提示
                        UnityEngine.Debug.Log(newDirectory + "/" + file.Name + " Exported!");
                    }
                }
            }

            if (!hasError)
            {
                //输出导出完毕
                UnityEngine.Debug.LogError("Export Resource Done!");
            }
            else
            {
                //输出导出完毕,但是又错误！
                UnityEngine.Debug.LogError("Export Resource 存在错误，请查看错误日志!");
            }

            //设置操作完成
            _m_bIsWorking = false;
        }
    }
}
