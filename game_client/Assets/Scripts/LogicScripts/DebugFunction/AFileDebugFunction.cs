using System;
using System.IO;
using UnityEngine;

namespace GOE
{

    public abstract class AFileDebugFunction
    {
        private string _m_debugFilePath;
        private bool _m_fileExists;
        private string _m_content;
        public AFileDebugFunction(string _debugFileName)
        {
            //统一放到一个目录里
            _m_debugFilePath = $"{Application.persistentDataPath}/{_debugFileName}";
        }

        /// <summary>
        /// 读取文件，并且刷新调试功能的状态
        /// 如果文件存在，就开启功能
        /// 如果文件不存在，就关闭功能（可选功能）
        /// </summary>
        public void readFileAndRefreshStatus(bool _closeFunctionWhenNoFile = true)
        {
            try
            {
                if (File.Exists(_m_debugFilePath))
                {
                    _m_fileExists = true;
                    if (hasContent)
                    {
                        StreamReader stream = new StreamReader(_m_debugFilePath);
                        _m_content = stream.ReadToEnd();
                    }
                    _doRefreshStatus(_m_content);
                    _doOpenFunction();
                }
                else
                {
                    _m_fileExists = false;
                    if (_closeFunctionWhenNoFile)
                    {
                        closeFunction();
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"FileDebugFunction文件读取失败{_m_debugFilePath}:{e}");
                _m_fileExists = false;
                _doCloseFunction();
            }
        }

        /// <summary>
        /// 当前调试功能是否开启
        /// </summary>
        public bool isFileExists()
        {
            return _m_fileExists;
        }
        
        /// <summary>
        /// 由程序来开启这个Debug功能，而不是由外部来开启
        /// 按照稳健的写法，外部一定要TryCatch
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void openFunction(string _param)
        {
            //TODO 保证路径存在
            using (FileStream fileStream = File.Create(_m_debugFilePath))
            {
                _m_fileExists = true;
                if (!string.IsNullOrEmpty(_param))
                {
                    using (StreamWriter streamWriter = new StreamWriter(fileStream))
                    {
                        streamWriter.Write(_param);
                    }
                }
            }

            _doRefreshStatus(_param);
            _doOpenFunction();
        }

        /// <summary>
        /// 由程序来关闭这个Debug功能
        /// </summary>
        public void closeFunction()
        {
            if (File.Exists(_m_debugFilePath))
                File.Delete(_m_debugFilePath);
            _m_fileExists = false;
            
            _doCloseFunction();
        }

        /// <summary>
        /// 调试文件是否有内容
        /// </summary>
        protected abstract bool hasContent { get; }
        /// <summary>
        /// 根据Debug文件内的内容，刷新自己需要的信息
        /// </summary>
        /// <param name="_content"></param>
        protected abstract void _doRefreshStatus(string _content);
        /// <summary>
        /// 调试功能开启时，要如何调用其他逻辑
        /// </summary>
        protected abstract void _doOpenFunction();
        /// <summary>
        /// 调试功能关闭时，要如何调用其他逻辑
        /// </summary>
        protected abstract void _doCloseFunction();
       
    }
}