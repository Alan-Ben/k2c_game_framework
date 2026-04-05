using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Security;
using System.Security.Cryptography;

using UnityEngine;

/**************
 * 记录的用户信息
 **/
namespace ALPackage
{
    public abstract class _AALBasicSettingInfo
    {
        private const string _c_noEncodingTag = "[noenco]";
        private const int _c_iHeaderReadLen = 32;

        //设置文件保存路径
        private string _m_sSettingFilePath;
        //是否初始化了数据
        private bool _m_bIsInited;

        //是否加密处理，默认不加密
        private bool _m_bIsEncoding = false;
        //用于区分加密与否的读取信息
        private char[] _m_arrReadTagArr;

        //是否需要保存
        private bool _m_bNeedSave;
        //是否在保存中
        private bool _m_bIsSaving;

        public _AALBasicSettingInfo(string _settingPath)
        {
            //拼凑完整路径
#if UNITY_ANDROID && !UNITY_EDITOR
            string tmpPath = Application.persistentDataPath;
            if (null == tmpPath)
            {
                tmpPath = "/data/data" + Application.dataPath.Substring(9);
                if (tmpPath.LastIndexOf('-') == -1)
                    tmpPath = tmpPath.Substring(0, tmpPath.LastIndexOf('/')) + "/files";
                else
                    tmpPath = tmpPath.Substring(0, tmpPath.LastIndexOf('-')) + "/files";
            }
            _m_sSettingFilePath = tmpPath + "/" + _settingPath;
#else
            _m_sSettingFilePath = Application.persistentDataPath + "/" + _settingPath;
#endif

            _m_bIsInited = false;

            _m_bIsEncoding = false;
            _m_arrReadTagArr = new char[_c_iHeaderReadLen];

            _m_bNeedSave = false;
            _m_bIsSaving = false;
        }

        public _AALBasicSettingInfo(string _settingPath, bool _isEncoding)
        {
            //拼凑完整路径
#if UNITY_ANDROID && !UNITY_EDITOR
            string tmpPath = Application.persistentDataPath;
            if (null == tmpPath)
            {
                tmpPath = "/data/data" + Application.dataPath.Substring(9);
                if (tmpPath.LastIndexOf('-') == -1)
                    tmpPath = tmpPath.Substring(0, tmpPath.LastIndexOf('/')) + "/files";
                else
                    tmpPath = tmpPath.Substring(0, tmpPath.LastIndexOf('-')) + "/files";
            }
            _m_sSettingFilePath = tmpPath + "/" + _settingPath;
#else
            _m_sSettingFilePath = Application.persistentDataPath + "/" + _settingPath;
#endif

            _m_bIsInited = false;

            _m_bIsEncoding = _isEncoding;
            _m_arrReadTagArr = new char[_c_iHeaderReadLen];

            _m_bNeedSave = false;
            _m_bIsSaving = false;
        }

        public string settingFilePath { get { return _m_sSettingFilePath; } }
        public bool isInited { get { return _m_bIsInited; } }
        public bool isEncoding { get { return _m_bIsEncoding; } }

        /// <summary>
        /// 初始化处理
        /// </summary>
        public void init()
        {
            //不进行多次初始化
            if (_m_bIsInited)
                return;

            _m_bIsInited = true;

            try
            {
                //开启文件进行读取
                StreamReader sr = null;
                try
                {
                    sr = File.OpenText(_m_sSettingFilePath);
                }
                catch (Exception)
                {
                    if (null != sr)
                    {
                        //关闭文件
                        sr.Close();
                        //销毁流
                        sr.Dispose();
                    }
                    //路径与名称未找到文件则直接返回空
                    return;
                }

                //先读取指定标记的字符，按照32个读取，这样可以在加密和不加密中都可使用。加密需要32个字符对应16字节，对应2个关键值
                int readLen = sr.Read(_m_arrReadTagArr, 0, _c_iHeaderReadLen);
                //判断字符串是否合法长度，必须要有16个长度的头表示key或不加密
                if (readLen < _c_iHeaderReadLen)
                {
                    _initSettingStr(string.Empty);
                    return;
                }

                //判断是否加密
                //由于早期版本必须加密，所以如果是不加密的，需要判断数据是否匹配，如果不匹配则需要继续按照加密处理
                bool isEncoding = false;
                //对前置的几个字符串进行判断，因为hex只存在a-f所以只要匹配那么一定是未加密的
                for (int i = 0; i < _c_noEncodingTag.Length; i++)
                {
                    if (_c_noEncodingTag[i] != _m_arrReadTagArr[i])
                    {
                        isEncoding = true;
                        break;
                    }
                }

                //根据加密与否进行不同的处理
                if (isEncoding)
                {
                    //读取剩余内容
                    string allInfoHexStr = null;
                    try
                    {
                        //读取所有信息，并进行后续处理
                        allInfoHexStr = sr.ReadToEnd();
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError("Init setting: " + _m_sSettingFilePath + " sr.ReadToEnd err:" + ex);
                        if (sr != null)
                        {
                            //关闭文件
                            sr.Close();
                            //销毁流
                            sr.Dispose();
                        }
                        return;
                    }

                    if (sr != null)
                    {
                        //关闭文件
                        sr.Close();
                        //销毁流
                        sr.Dispose();
                    }

                    //将头部转化为固定信息
                    byte[] headerByte = ALCommon.getHexBytes(_m_arrReadTagArr);
                    //将所有剩余信息转化为字节对象
                    byte[] allByteInfo = ALCommon.getHexBytes(allInfoHexStr);

                    //获取前8字节作为des解密key
                    //获取第8 ～ 16字节作为des解密iv
                    byte[] desKey = new byte[8];
                    byte[] desIv = new byte[8];
                    for (int i = 0; i < 8; i++)
                    {
                        desKey[i] = headerByte[i];
                        desIv[i] = headerByte[i + 8];
                    }

                    //使用对应的key和iv进行解密
                    string infoStr;
                    //开始des解密
                    DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                    using (MemoryStream ms = new MemoryStream())
                    {
                        try
                        {
                            using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(desKey, desIv), CryptoStreamMode.Write))
                            {
                                cs.Write(allByteInfo, 0, allByteInfo.Length);
                                cs.FlushFinalBlock();
                            }

                            //获取解密后字符串
                            infoStr = Encoding.Default.GetString(ms.ToArray());
                        }
                        catch (Exception ex)
                        {
                            Debug.LogError("Init setting: " + _m_sSettingFilePath + " err:" + ex.Message);
                            return;
                        }
                    }

                    //根据字符串初始化相关用户配置
                    _initSettingStr(infoStr);
                }
                else
                {
                    //读取剩余内容
                    string allInfoStr = null;
                    try
                    {
                        //读取所有信息，并进行后续处理
                        allInfoStr = sr.ReadToEnd();
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError("Init setting: " + _m_sSettingFilePath + " sr.ReadToEnd err:" + ex);
                        if (sr != null)
                        {
                            //关闭文件
                            sr.Close();
                            //销毁流
                            sr.Dispose();
                        }
                        return;
                    }

                    if (sr != null)
                    {
                        //关闭文件
                        sr.Close();
                        //销毁流
                        sr.Dispose();
                    }

                    //根据字符串初始化相关用户配置
                    try
                    {
                        _initSettingStr(allInfoStr);
                    }
                    catch (Exception _e)
                    {
                        UnityEngine.Debug.LogError($"Init Setting Error\n!{_e}");
                    }
                }
            }
            catch(Exception _ex)
            {
                //抛出异常
                UnityEngine.Debug.LogError($"Init Setting General Error\n!{_ex}");

                //调用子类函数
                _onInitErr();
            }
        }

        /// <summary>
        /// 保存配置信息的接口函数
        /// </summary>
        public void saveSetting()
        {
            lock (this)
            {
                //判断是否在保存中，是则只设置标记，不进行保存
                if (_m_bIsSaving)
                {
                    _m_bNeedSave = true;
                }
                else
                {
                    //开启线程进行保存
                    _startSavingThread();
                }
            }
        }

        /// <summary>
        /// 开启保存线程的处理
        /// </summary>
        private void _startSavingThread()
        {
            lock (this)
            {
                //判断是否在保存中，是则不做处理
                if (_m_bIsSaving)
                    return;

                //设置不需要保存
                _m_bNeedSave = false;

                //获取保存内容
                string totalStr = _makeSettingStr();

                _m_bIsSaving = true;
                //开启线程处理
                ALThread tmpThread = new ALThread(()=> _dealSavingSetting(totalStr));
                tmpThread.startThread();
            }
        }

        /// <summary>
        /// 保存配置的实际处理函数
        /// </summary>
        private void _dealSavingSetting(string _totalStr)
        {
            try
            {
                //根据是否加密进行不同的处理
                if (_m_bIsEncoding)
                {
                    //获取加密的随机密钥
                    string key = ALCommon.getRndDexStr(16);
                    string iv = ALCommon.getRndDexStr(16);
                    //转换为字节
                    byte[] keyByte = ALCommon.getHexBytes(key);
                    byte[] ivByte = ALCommon.getHexBytes(iv);

                    //开始进行加密处理
                    DESCryptoServiceProvider des = new DESCryptoServiceProvider();

                    string encryptStr;
                    //获取加密后的字节数组
                    using (MemoryStream ms = new MemoryStream())
                    {
                        byte[] infoBytes = Encoding.Default.GetBytes(_totalStr);
                        try
                        {
                            using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(keyByte, ivByte), CryptoStreamMode.Write))
                            {
                                cs.Write(infoBytes, 0, infoBytes.Length);
                                cs.FlushFinalBlock();
                            }

                            encryptStr = ALCommon.getHexString(ms.ToArray());
                        }
                        catch (Exception ex)
                        {
                            Debug.LogError("Save setting: " + _m_sSettingFilePath + " err:" + ex.Message);

                            return;
                        }
                    }

                    //将字符串存储到文件中
                    //文件流信息
                    StreamWriter sw = null;
                    FileInfo t = new FileInfo(_m_sSettingFilePath);

                    // CreateText内部会自动判断文件是否存在并覆盖
                    // //判断文件是否存在
                    // if(t.Exists)
                    // {
                    //     try
                    //     {
                    //         //文件存在则删除文件
                    //         t.Delete();
                    //     }
                    //     catch(System.Exception ex)
                    //     {
                    //         Debug.LogError("Save setting:" + _m_sSettingFilePath + " t.Delete err:" + ex.Message);
                    //         return;
                    //     }
                    //
                    // }

                    try
                    {
                        //创建新的文件
                        sw = t.CreateText();
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError("Save setting:" + _m_sSettingFilePath + " t.CreateText err:" + ex);
                        if (sw != null)
                        {
                            //关闭流
                            sw.Close();
                            //销毁流
                            sw.Dispose();
                        }
                        return;
                    }

                    try
                    {
                        //写入key和iv信息
                        sw.Write(key);
                        sw.Write(iv);
                        //写入新的文件信息
                        sw.Write(encryptStr);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError("Save setting:" + _m_sSettingFilePath + " sw.Write err:" + ex);
                    }

                    if (sw != null)
                    {
                        //关闭流
                        sw.Close();
                        //销毁流
                        sw.Dispose();
                    }
                }
                else
                {
                    //======开始不加密的处理方式============

                    //将字符串存储到文件中
                    //文件流信息
                    StreamWriter sw = null;
                    FileInfo t = new FileInfo(_m_sSettingFilePath);

                    // //判断文件是否存在
                    // if(t.Exists)
                    // {
                    //     try
                    //     {
                    //         //文件存在则删除文件
                    //         t.Delete();
                    //     }
                    //     catch(System.Exception ex)
                    //     {
                    //         Debug.LogError("Save setting:" + _m_sSettingFilePath + " t.Delete err:" + ex.Message);
                    //         return;
                    //     }
                    //
                    // }

                    try
                    {
                        //创建新的文件
                        sw = t.CreateText();
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError("Save setting:" + _m_sSettingFilePath + " t.CreateText err:" + ex);
                        if (sw != null)
                        {
                            //关闭流
                            sw.Close();
                            //销毁流
                            sw.Dispose();
                        }
                        return;
                    }

                    try
                    {
                        //写入头部字符信息
                        sw.Write(_c_noEncodingTag);
                        //补充满32个长度
                        for (int i = _c_noEncodingTag.Length; i < 32; i++)
                        {
                            sw.Write(' ');
                        }
                        //写入新的文件信息
                        sw.Write(_totalStr);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError("Save setting:" + _m_sSettingFilePath + " sw.Write err:" + ex);
                    }

                    if (sw != null)
                    {
                        //关闭流
                        sw.Close();
                        //销毁流
                        sw.Dispose();
                    }
                }
            }
            finally
            {
                lock (this)
                {
                    //设置保存标记位
                    _m_bIsSaving = false;

                    //判断是否需要保存如果需要保存则需要继续处理保存操作
                    if (_m_bNeedSave)
                    {
                        //开启任务继续处理保存行为
                        //开启任务是为了返回主线程，避免获取信息的时候出现访问冲突
                        ALCommonTaskController.CommonActionAddMonoTask(_startSavingThread);
                    }
                }
            }
        }

        /// <summary>
        /// 删除配置存储文件
        /// </summary>
        public void delete()
        {
            try
            {
                //直接删除文件
                ALFile.Delete(_m_sSettingFilePath);
            }
            catch (Exception ex)
            {
                Debug.LogError("Save setting:" + _m_sSettingFilePath + " File.Delete err:" + ex);
            }
        }

        /// <summary>
        /// 初始化报错的事件函数
        /// </summary>
        protected virtual void _onInitErr()
        {

        }

        /*************
         * 构建需要保存的字符串
         **/
        protected abstract string _makeSettingStr();
        /**************
         * 读取保存的字符串
         **/
        protected abstract void _initSettingStr(string _infoStr);
    }
}
