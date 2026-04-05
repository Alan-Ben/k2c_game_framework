#if UNITY_EDITOR
/********************************************************************
  filename: 
  author: Cloud Zhang

  purpose:  7z.exe封装, 导出7z文件
*********************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.IO;

namespace ALPackage {
    public class SevenZipProcess {

        string _m_Error;
        public string Error {
            get { return _m_Error; }
            protected set { _m_Error = value; }
        }

        private static bool _g_bIsInit = false;
        private static string _g_datapath;
        public static void init()
        {
            if (_g_bIsInit)
                return;

            _g_bIsInit = true;
            _g_datapath = Application.dataPath;
        }

        string _m_datapath;
        class CompressFileInfo {
            public string inFile;
            public string outFile;
            public CompressFileInfo(string _inFile, string _outFile) {
                inFile = _inFile;
                outFile = _outFile;
            }
        }

        public SevenZipProcess() {
            _m_datapath = _g_datapath;
            _m_Error = string.Empty;
        }

        /// <summary>
        /// 压缩
        /// </summary>
        /// <param name="_inFile"></param>
        /// <param name="_outFile"></param>
        public static string CompressFileLZMA(string _inFile, string _outFile) {
            SevenZipProcess zipProcess = new SevenZipProcess();
            CompressFileInfo info = new CompressFileInfo(_inFile, _outFile);
            zipProcess.DoCompress(info);
            return zipProcess.Error;
        }

        private void DoCompress(object o) {
            CompressFileInfo _info = (CompressFileInfo)o;
            Compress(_info.inFile, _info.outFile);
        }

        void Compress(string _inFile, string _outFile) {
            try
            {

                if (string.IsNullOrEmpty(_m_datapath)) _m_datapath = Application.dataPath;
                string exePath = _m_datapath + "/Scripts/ALPackage/Editor/7-Zip/7z.exe";
                if (!File.Exists(exePath)) {
                        Error = "压缩工具7z.exe不存在，这个工具应该在ALPackage/Editor/7-Zip/目录";
                        UnityEngine.Debug.LogError(Error);
                        return;
                    }
      

                if (File.Exists(_outFile)) {
                    File.Delete(_outFile);
                }

                //测试极限压缩和普通压缩的尺寸只差1k，所以用普通压缩，下面选项意思是 -mx=5 普通压缩， -myx=0关闭文件扫描 -ms=off 用非固体格式（按插件作者建议，可以加快解压和减少内存使用），-y 如果遇到 y/n, 统一yes
                string cmd = string.Format(" a -t7z {0} {1} -mx=5 -myx=0 -ms=off -y", _outFile, _inFile);

                string ret = RunCmd(exePath, cmd);
                if (!string.IsNullOrEmpty(ret)) {
                    Error = string.Format("{0} 压缩错误 {1}", _inFile, ret);
                    UnityEngine.Debug.LogErrorFormat(Error);
                }
            }
            catch (System.Exception ex) {
                Error = string.Format("{0} 压缩错误 {1}", _inFile, ex.Message);
                UnityEngine.Debug.LogError(Error);
                UnityEngine.Debug.LogError(ex.StackTrace);
                //throw;
            }

        }

        public static string RunCmd(string exePath, string command) {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.UseShellExecute = false;        //Shell的使用
            p.StartInfo.FileName = exePath;           //确定程序名
            p.StartInfo.Arguments = command;    //确定程式命令行
            p.StartInfo.RedirectStandardInput = true;   //重定向输入
            p.StartInfo.RedirectStandardOutput = true; //重定向输出
            p.StartInfo.RedirectStandardError = true;   //重定向输出错误
            p.StartInfo.CreateNoWindow = true;          //设置置不显示示窗口

            //开启线程
            p.Start();
            try
            {
                p.WaitForExit();
            }
            catch (System.Exception ex)
            {
                UnityEngine.Debug.LogError(ex.ToString());
            }

            if (p.ExitCode != 0)
            {
                string err = p.StandardError.ReadToEnd();
                err = gb2312_utf8(err);
                string standoutput = p.StandardOutput.ReadToEnd();
                standoutput = gb2312_utf8(standoutput);

                p.Close();
                p.Dispose();

                if (string.IsNullOrEmpty(err))
                {
                    return standoutput;
                }
                else
                {
                    return err;        //输出出流取得命令行结果果
                }
            }

            p.Close();
            p.Dispose();
            return string.Empty;
        }

        /// <summary>
        /// GB2312转换成UTF8
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string gb2312_utf8(string text) {
            //声明字符集   
            System.Text.Encoding utf8, gb2312;
            //gb2312   
            gb2312 = System.Text.Encoding.GetEncoding("gb2312");
            //utf8   
            utf8 = System.Text.Encoding.GetEncoding("utf-8");
            byte[] gb;
            gb = gb2312.GetBytes(text);
            gb = System.Text.Encoding.Convert(gb2312, utf8, gb);
            //返回转换后的字符   
            return utf8.GetString(gb);
        }

        /// <summary>
        /// UTF8转换成GB2312
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string utf8_gb2312(string text) {
            //声明字符集   
            System.Text.Encoding utf8, gb2312;
            //utf8   
            utf8 = System.Text.Encoding.GetEncoding("utf-8");
            //gb2312   
            gb2312 = System.Text.Encoding.GetEncoding("gb2312");
            byte[] utf;
            utf = utf8.GetBytes(text);
            utf = System.Text.Encoding.Convert(utf8, gb2312, utf);
            //返回转换后的字符   
            return gb2312.GetString(utf);
        }
    } 
}
#endif
