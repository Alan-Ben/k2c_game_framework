using System;
using System.IO;
using ALPackage;
using IFix.Core;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class InjectFixMgr
    {
        private static InjectFixMgr _g_instance = new InjectFixMgr();
        [NotNull]
        public static InjectFixMgr instance
        {
            get
            {
                if(null == _g_instance)
                    _g_instance = new InjectFixMgr();

                return _g_instance;
            }
        }
        
        private const string injectFixName = "__Assembly-CSharp.patch.bytes";
        
        //补丁文件存储目录
        private string _m_sPatchSavePath;
        //文件先存到临时目录
        private string _m_sPatchTempSavePath;
        
        
        //是否已经开始下载
        private bool _m_isDownloadDealing;
        //当前下载序列号
        private long _m_serialize;
        
        protected InjectFixMgr()
        {
            _m_isDownloadDealing = false;
            _m_serialize = 0;
            
            _m_sPatchSavePath = NPResUtil.makeLocalSavePath(injectFixName);
            _m_sPatchTempSavePath = NPResUtil.makeLocalSavePath("temp_" + injectFixName);
        }
        
        /// <summary>
        /// 直接加载本地的补丁
        /// </summary>
        /// <returns></returns>
        public bool LoadLocalSavedPatch()
        {
            if (null == _m_sPatchSavePath || !File.Exists(_m_sPatchSavePath))
            {
                ALLog.Debug($"[InjectFix]本地保存路径为空或者文件不存在，不加载");
                return true;
            }
            
            //卸载旧的
            _unLoadInjectFixPatch();

            // 尝试加载本地保存的补丁
            if (_loadInjectFixPatch(_m_sPatchSavePath))
            {
                ALLog.Debug($"tryLoadLocalSavedPatchOnAwake加载本地补丁成功");
                return true;
            }
            else
            {
                //如果加载失败说明本地补丁有问题，则直接删除本地补丁
                _deleteLocalSavedPatch();
                ALLog.Debug($"直接加载本地的补丁失败");
                return false;
            }
        }

        /// <summary>
        /// 开始下载并加载新的补丁
        /// </summary>
        /// <param name="_patchUrl"></param>
        /// <param name="_processDelegate"></param>
        /// <param name="_sucDelegate"></param>
        /// <param name="_failDelegate"></param>
        public void startDownloadAndLoad(string _patchUrl, Action<float> _processDelegate, Action _sucDelegate, Action _failDelegate)
        {
            //数据有问题不处理
            if (string.IsNullOrEmpty(_patchUrl))
            {
                if (_failDelegate != null)
                {
                    _failDelegate();
                }
                return;
            }
            
            //在下载中了不处理
            if (_m_isDownloadDealing)
            {
                UnityEngine.Debug.LogError($"在加载过程中尝试进行加载操作！");
                if (_failDelegate != null)
                {
                    _failDelegate();
                }
                return;
            }

            _m_isDownloadDealing = true;
            _m_serialize++;

            long serialize = _m_serialize;
            
            //创建下载对象
            ALHttpSingleDownloadMd5Dealer patchDownloadDealer = new ALHttpSingleDownloadMd5Dealer(_patchUrl, _m_sPatchTempSavePath, _processDelegate,
                //成功回调
                () =>
                {
                    if(serialize != _m_serialize)
                        return;
                    
                    //卸载旧的补丁
                    _unLoadInjectFixPatch();

                    //处理下载到的patch加载
                    if (!_loadInjectFixPatch(_m_sPatchTempSavePath))
                    {
                        _m_isDownloadDealing = false;

                        //处理成果调用结果
                        if (null != _failDelegate)
                            _failDelegate();
                    }
                    else
                    {
                        try
                        {
                            //如果成功加载，则把临时文件复制到正式文件
                            File.Copy(_m_sPatchTempSavePath, _m_sPatchSavePath, true);
                            if (!string.IsNullOrEmpty(_m_sPatchTempSavePath) && File.Exists(_m_sPatchTempSavePath))
                            {
                                File.Delete(_m_sPatchTempSavePath);
                            }
                        }
                        catch (Exception e)
                        {
                            Debug.LogError($"拷贝InjectFix文件时发生错误:\n{e}");
                        }
                        finally
                        {
                            _m_isDownloadDealing = false;

                            //处理成果调用结果
                            if (null != _sucDelegate)
                                _sucDelegate();
                        }
                    }
                },
                //失败回调
                () =>
                {
                    if(serialize != _m_serialize)
                        return;
                    
                    _m_isDownloadDealing = false;

                    if (null != _failDelegate)
                        _failDelegate();
                });
            
            patchDownloadDealer.startLoad();
        }

        /// <summary>
        /// 清除当前补丁
        /// </summary>
        public void clearCurPatch()
        {
            //卸载已经加载的补丁
            _unLoadInjectFixPatch();

            //删除补丁文件
            _deleteLocalSavedPatch();
        }
        
        /// <summary>
        /// 卸载当前已经加载的补丁补丁
        /// </summary>
        /// <returns></returns>
        private bool _unLoadInjectFixPatch()
        {
            // 加载之前先进行卸载，确保之前加载的补丁被卸载掉
            try
            {
                PatchManager.Unload(typeof(InjectFixMgr).Assembly);
            }
            catch (Exception e)
            {
                Debug.LogError($"InjectFix补丁卸载出错：{e}");
                return false;
            }

            return true;
        }
        
        
        /// <summary>
        /// 加载补丁
        /// </summary>
        /// <param name="_byteFilePath">补丁文件完整路径</param>
        /// <returns></returns>
        private bool _loadInjectFixPatch(string _byteFilePath)
        {
            if(null == _byteFilePath || !File.Exists(_byteFilePath))
                return false;
            
            using(FileStream fs = new FileStream(_byteFilePath, FileMode.Open))
            {
                try
                {
                    //加载对应数据
                    PatchManager.Load(fs);
                }
                catch (Exception e)
                {
#if UNITY_EDITOR
                    NPMesMgr.instance.showOneBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.injectfix_patch_load_fail_str, e), 
                        TextTranslate.instance.getLanguage(TransKeyConst.confirm), null);
#endif
                    Debug.LogError($"InjectFix补丁数据错误.byte，加载失败：{e}");
                    return false;
                }
                
            }
            return true;
        }
        
        /// <summary>
        /// 删除保存在本地的补丁文件
        /// </summary>
        private void _deleteLocalSavedPatch()
        {
            try
            {
                //删除本地补丁文件
                if (!string.IsNullOrEmpty(_m_sPatchSavePath) && File.Exists(_m_sPatchSavePath))
                {
                    File.Delete(_m_sPatchSavePath);
                    ALLog.Debug($"删除InjectFix本地保存的补丁文件");
                    
                    //同时删除tag标记
                    GameSetting.instance.setLastInjectFixVersion(String.Empty);
                }
                
                //删除本地临时文件
                if (!string.IsNullOrEmpty(_m_sPatchTempSavePath) && File.Exists(_m_sPatchTempSavePath))
                {
                    File.Delete(_m_sPatchTempSavePath);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"删除InjectFix本地保存的补丁文件时发生错误:\n{e}");
            }
        }
    }
}