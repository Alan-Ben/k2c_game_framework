
using System;
using System.IO;
using System.Collections.Generic;

using SQLite4Unity3d;

namespace AudioBuilder
{
    public class AudioBuilder
    {
        private string _m_sGitVersionTag;
        private string _m_sBuildPath;
        private string _m_sDBName;

        public AudioBuilder(string _dbName, string _gitVersionTag, string _buildPath)
        {
            _m_sDBName = _dbName;
            _m_sGitVersionTag = _gitVersionTag;
            _m_sBuildPath = _buildPath;
        }

        public void deal()
        {
            if (!_legalCheck())
                return;

 
            _m_sBuildPath = Path.GetFullPath(_m_sBuildPath);
            _m_sBuildPath = _m_sBuildPath.Replace("\\", "/");
            _m_sBuildPath = BuildUtility.directoryInsure(_m_sBuildPath);

            _deleteGitVersionANdDBMD5File();
            _makeMD5VersionTextFile();
            _makeVersionOfSQLiteMD5VersionFile();
            _makeGitVersionFile();
        }

        private bool _legalCheck()
        {
            try
            {
                // 尝试获取绝对路径
                if (!string.IsNullOrEmpty(Path.GetFullPath(_m_sBuildPath)))
                    return true;
                else
                {
                    Debug.Log($"{_m_sBuildPath} 识别成路径失败");
                    return false;
                }
            }
            catch (Exception _ex)
            {
                Debug.Log($"{_m_sBuildPath} 识别成路径失败 \n: {_ex.ToString()}");
                return false;
            }
        }

        /// <summary>
        /// 在RootPath下创建一个【包含rootPath下所有文件的md5值和文件大小】的文件，用于生成该类使用的数据库文件
        /// </summary>
        private void _makeMD5VersionSQLiteFile()
        {
            string dbPath = _m_sBuildPath + _m_sDBName;

            if (!Directory.Exists(_m_sBuildPath))
            {
                Debug.Log($"{_m_sBuildPath} 路径不存在");
                return;
            }

            // 先删除旧文件
            if (File.Exists(dbPath))
                File.Delete(dbPath);

            // 使用数据库构建配置文件
            SQLiteConnection ds = new SQLiteConnection(dbPath);
            List<SQLiteObj> objList = new List<SQLiteObj>();
            BuildUtility.actionRecursiveFileByRootPath(_m_sBuildPath, (FileInfo _file) =>
            {
                if (!_file.Exists)
                    return;

                if (_file.Name.Contains(_m_sDBName))
                    return;

                Debug.Log($"[生成数据库文件] 将{_file.FullName}导入数据库文件");

                SQLiteObj fileObj = new SQLiteObj();
                fileObj.fileKey = _file.FullName.Replace("\\", "/").Replace(_m_sBuildPath, "");
                fileObj.fileMD5 = BuildUtility.generateMD5(_file);
                fileObj.fileSize = _file.Length;

                objList.Add(fileObj);
            });
            try
            {
                // 尝试丢弃模板表，保证不出错丢弃此表
                ds.DropTable<SQLiteObj>();

                // 创建模板表
                ds.CreateTable<SQLiteObj>();

                // 把数据插入模板表
                ds.InsertAll(objList);

                //提交事务
                ds.Commit();

                Debug.Log($"[生成数据库文件] 生成成功，路径是：{dbPath}");
            }
            catch (Exception ex)
            {
                ds.Rollback();
                Debug.Log("数据库生成出错：\n" + ex.ToString());
            }
            finally
            {
                ds.Execute("VACUUM");
                ds.Close();
            }
        }
        /// <summary>
        /// 在RootPath下创建一个【包含rootPath下所有文件的md5值和文件大小】的文本文件
        /// </summary>
        private void _makeMD5VersionTextFile()
        {
            // 将数据库文件名改为文本文件名
            string textFilePath = _m_sBuildPath + _m_sDBName.Replace(".db", ".txt");

            if (!Directory.Exists(_m_sBuildPath))
            {
                Debug.Log($"{_m_sBuildPath} 路径不存在");
                return;
            }

            // 先删除旧文件
            if (File.Exists(textFilePath))
                File.Delete(textFilePath);

            List<string> fileInfoList = new List<string>();
            
            BuildUtility.actionRecursiveFileByRootPath(_m_sBuildPath, (FileInfo _file) =>
            {
                if (!_file.Exists)
                    return;

                if (_file.Name.Contains(_m_sDBName) || _file.Name.Contains(_m_sDBName.Replace(".db", ".txt")))
                    return;

                Debug.Log($"[生成文本文件] 将{_file.FullName}导入文本文件");

                string fileKey = _file.FullName.Replace("\\", "/").Replace(_m_sBuildPath, "");
                string fileMD5 = BuildUtility.generateMD5(_file);
                long fileSize = _file.Length;

                // 格式：文件路径|MD5值|文件大小
                string fileInfo = $"{fileKey}|{fileMD5}|{fileSize}";
                fileInfoList.Add(fileInfo);
            });

            try
            {
                // 将所有文件信息写入文本文件
                File.WriteAllLines(textFilePath, fileInfoList);
                Debug.Log($"[生成文本文件] 生成成功，路径是：{textFilePath}，共处理{fileInfoList.Count}个文件");
            }
            catch (Exception ex)
            {
                Debug.Log("文本文件生成出错：\n" + ex.ToString());
            }
        }
        
        private void _makeVersionOfSQLiteMD5VersionFile()
        {
            string dbPath = _m_sBuildPath + _m_sDBName;
            string outputPath = dbPath + ".md5";

            FileInfo file = new FileInfo(dbPath);
            if (!file.Exists)
            {
                Debug.Log($"[生成数据库文件的版本信息] 没有找到数据库文件{dbPath}");
                return;
            }

            try
            {
                if (File.Exists(outputPath))
                    File.Delete(outputPath);

                string md5 = BuildUtility.generateMD5(file);
                string fileSize = file.Length.ToString(); ;
                string outputStr = md5 + "|" + fileSize;
                File.WriteAllText(outputPath, outputStr);
                Debug.Log($"[生成数据库文件的版本信息] 生成成功，路径是：{outputPath}");
            }
            catch (Exception _ex)
            {
                Debug.Log($"[生成数据库文件的版本信息] 出错：{outputPath} \n{_ex.ToString()}");
            }
        }


        private void _deleteGitVersionANdDBMD5File()
        {

            string outputPath = _m_sBuildPath + "git_version";

            string dbPath = _m_sBuildPath + _m_sDBName;
            string dbMD5OutputPath = dbPath + ".md5";
                  
            try
            {
                if (File.Exists(dbMD5OutputPath))
                    File.Delete(dbMD5OutputPath);

                Debug.Log($"[删除DB MD5] 成功，路径是：{dbMD5OutputPath}");

                if (File.Exists(outputPath))
                    File.Delete(outputPath);

                Debug.Log($"[删除Version] 成功，路径是：{outputPath}");
            }
            catch (Exception _ex)
            {
                Debug.Log($"[删除Git的版本信息] 出错：{outputPath} \n{_ex.ToString()}");
            }
        }

        private void _makeGitVersionFile()
        {
            string outputPath = _m_sBuildPath + "git_version";
            
            try
            {
                if (File.Exists(outputPath))
                    File.Delete(outputPath);

                string outputStr = _m_sGitVersionTag;
                File.WriteAllText(outputPath, outputStr);
                Debug.Log($"[生成Git的版本信息] 生成成功，路径是：{outputPath}");
            }
            catch (Exception _ex)
            {
                Debug.Log($"[生成Git的版本信息] 出错：{outputPath} \n{_ex.ToString()}");
            }
        }

        /// <summary>
        /// 内部使用的数据库对象
        /// </summary>
        [TableAttribute("SQLiteObj")]
        private class SQLiteObj
        {
            // 文件路径，也作为主键
            protected string _m_sFileKey;
            // 文件的md5值
            protected string _m_sFileMD5;
            // 文件的大小
            protected long _m_lFileSize;

            [PrimaryKey]
            public virtual string fileKey { get { return _m_sFileKey; } set { _m_sFileKey = value; } }
            public virtual string fileMD5 { get { return _m_sFileMD5; } set { _m_sFileMD5 = value; } }
            public virtual long fileSize { get { return _m_lFileSize; } set { _m_lFileSize = value; } }
        }
    }
}
