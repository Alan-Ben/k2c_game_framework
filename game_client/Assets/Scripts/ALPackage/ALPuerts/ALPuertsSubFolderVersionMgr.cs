using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

#if AL_PUERTS
using Puerts;
#endif

namespace ALPackage
{
#if AL_PUERTS

    /// <summary>
    /// 每个子文件夹的目录版本信息
    /// </summary>
    public class ALPuertsSubFolderVersionInfo
    {
        /// <summary>
        /// 子目录名
        /// </summary>
        private string _m_sSubFolderPath;
        //版本号
        private long _m_lVersionNum;

        public ALPuertsSubFolderVersionInfo(string _path, long _version)
        {
            _m_sSubFolderPath = _path;
            _m_lVersionNum = _version;
        }

        /// <summary>
        /// 设置版本号
        /// </summary>
        /// <param name="_version"></param>
        public void setVersion(long _version)
        {
            _m_lVersionNum = _version;
        }

        public string subFolderPath { get { return _m_sSubFolderPath; } }
        public long versionNum { get { return _m_lVersionNum; } }
    }

    /// <summary>
    /// 针对puertsMgr下的各子目录进行的版本管理数据对象
    /// </summary>
    public class ALPuertsSubFolderVersionMgr : _AALBasicSettingInfo
    {
        /** 分隔符 */
        private const char ALPuertsSubFolderSplit = '&';
        private const char ALPuertsSubFolderInnerSplit = ':';

        //脚本管理器对象
        private ALPuertsManager _m_pmPuertsMgr;

        //存储不同版本信息的队列
        private List<ALPuertsSubFolderVersionInfo> _m_lSubFolderVersionInfo;
        //是否已经保存
        private bool _m_bIsSaved;

        public ALPuertsSubFolderVersionMgr(ALPuertsManager _puertsMgr)
            : base($"_alpuerts_{_puertsMgr.rootPath}.alsetting")
        {
            _m_pmPuertsMgr = _puertsMgr;

            _m_lSubFolderVersionInfo = new List<ALPuertsSubFolderVersionInfo>();
            _m_bIsSaved = false;
        }

        /*************
         * 构建需要保存的字符串
         **/
        protected override string _makeSettingStr()
        {
            StringBuilder strBuild = new StringBuilder();

            ALPuertsSubFolderVersionInfo tempData;
            for (int i = 0; i < _m_lSubFolderVersionInfo.Count; i++)
            {
                tempData = _m_lSubFolderVersionInfo[i];

                if (tempData == null)
                    continue;

                //插入大数据分隔符
                if (i != 0)
                    strBuild.Append(ALPuertsSubFolderSplit);

                strBuild.Append(tempData.subFolderPath);
                strBuild.Append(ALPuertsSubFolderInnerSplit);
                strBuild.Append(tempData.versionNum);
            }

            return strBuild.ToString();
        }
        /**************
         * 读取保存的字符串
         **/
        protected override void _initSettingStr(string _infoStr)
        {
            if (null == _infoStr || _infoStr.Length < 1 || _m_lSubFolderVersionInfo == null)
                return;

            _m_lSubFolderVersionInfo.Clear();

            string[] spStrs = _infoStr.Split(ALPuertsSubFolderSplit);

            long oldDate;
            for (int i = 0; i < spStrs.Length; i++)
            {
                string tempStr = spStrs[i];
                if (null == tempStr)
                    continue;

                string[] subSpStr = tempStr.Split(ALPuertsSubFolderInnerSplit);

                if (subSpStr.Length < 2)
                    continue;

                if (long.TryParse(subSpStr[1], out oldDate))
                {
                    //检查是否已有数据，有则报错
                    if(_findFolderVersion(subSpStr[0]) != null)
                    {
                        ALLog.Error($"Puerts [{_m_pmPuertsMgr.rootPath}] Contain multi version info [{subSpStr[0]}]");
                    }

                    //曾经登陆过并且间隔不超过7天才算曾经登陆
                    _m_lSubFolderVersionInfo.Add(new ALPuertsSubFolderVersionInfo(subSpStr[0], oldDate));
                }
                else
                {
                    ALLog.Error($"Init PuertsSubFolderVersion Error![{tempStr}]");
                }
            }

            //初始化完成，先设置为已保存
            _m_bIsSaved = true;
        }

        /// <summary>
        /// 比对对应文件夹的版本信息，一致则返回true否则返回false
        /// </summary>
        /// <param name="_folderPath"></param>
        /// <param name="_version"></param>
        /// <returns></returns>
        public bool checkFolderVersion(string _folderPath, long _version)
        {
            ALPuertsSubFolderVersionInfo info = _findFolderVersion(_folderPath);
            if (null == info)
                return false;

            return info.versionNum == _version;
        }

        /// <summary>
        /// 添加对应文件夹的版本信息
        /// </summary>
        /// <param name="_folder"></param>
        /// <param name="_version"></param>
        public void setFolderVersion(string _folder, long _version)
        {
            ALPuertsSubFolderVersionInfo info = _findFolderVersion(_folder);
            if(null != info)
            {
                info.setVersion(_version);
            }
            else
            {
                _m_lSubFolderVersionInfo.Add(new ALPuertsSubFolderVersionInfo(_folder, _version));
            }

            //保存数据
            saveData();
        }

        /// <summary>
        /// 移除对应文件夹的版本信息
        /// </summary>
        /// <param name="_folder"></param>
        public void removeFolderInfo(string _folder)
        {
            ALPuertsSubFolderVersionInfo tmpInfo = null;
            for (int i = 0; i < _m_lSubFolderVersionInfo.Count; i++)
            {
                tmpInfo = _m_lSubFolderVersionInfo[i];
                if (null == tmpInfo)
                    continue;

                if (tmpInfo.subFolderPath.Equals(_folder, StringComparison.OrdinalIgnoreCase))
                {
                    _m_lSubFolderVersionInfo.RemoveAt(i);
                    break;
                }
            }

            //保存数据
            saveData();
        }

        /// <summary>
        /// 保存本版本信息
        /// </summary>
        public void saveData()
        {
            //设置需要保存
            _m_bIsSaved = false;

            //延迟3秒执行对应的任务
            ALCommonTaskController.CommonActionAddMonoTask(_dealSaveData, 3f);
        }

        /// <summary>
        /// 实际执行保存的操作
        /// </summary>
        protected void _dealSaveData()
        {
            //如果已经保存则不需要执行
            if (_m_bIsSaved)
                return;

            _m_bIsSaved = true;

            //处理保存操作
            saveSetting();
        }

        /// <summary>
        /// 查询对应子目录的版本信息
        /// </summary>
        /// <param name="_folder"></param>
        /// <returns></returns>
        protected ALPuertsSubFolderVersionInfo _findFolderVersion(string _folder)
        {
            ALPuertsSubFolderVersionInfo tmpInfo = null;
            for (int i = 0; i < _m_lSubFolderVersionInfo.Count; i++)
            {
                tmpInfo = _m_lSubFolderVersionInfo[i];
                if (null == tmpInfo)
                    continue;

                if (tmpInfo.subFolderPath.Equals(_folder, StringComparison.OrdinalIgnoreCase))
                    return tmpInfo;
            }

            return null;
        }
    }
#endif
}
