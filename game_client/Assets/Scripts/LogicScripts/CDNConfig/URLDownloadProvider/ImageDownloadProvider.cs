using System;
using System.Collections.Generic;
using System.IO;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 图片下载管理器
    /// </summary>
    public class ImageDownloadProvider
    {
        //设置文件保存路径
        private string _m_sSettingFilePath;
        //临时文件保存路径
        private string _m_sTempSettingFilePath;
        //保存文件夹
        private string _m_sTag;
        //下载管理字典，<链接，下载管理器>
        private Dictionary<string, ALHttpSingleDownloadDealer> _m_dUrl2DownloadDealerDic;
        //已下载图片数据字典，<文件路径，图片数据>
        private Dictionary<string, byte[]> _m_dImageByteDic;
        //下载完成回调
        private Dictionary<string, Action<byte[]>> _m_dDownloadDoneActionDic;

        public ImageDownloadProvider(string _tag)
        {
            _m_sTag = _tag;
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
            _m_sSettingFilePath = tmpPath + "/" + _tag;
#else
            _m_sSettingFilePath = Application.persistentDataPath + "/" + _tag;
#endif
            _m_sTempSettingFilePath = _m_sSettingFilePath + "/temp";
        }


        /// <summary>
        /// 加载图片 已经下载过，则直接取；未下载过，则开始下载
        /// </summary>
        /// <param name="_url"></param>
        /// <param name="_downloadSucAction"></param>
        /// <returns></returns>
        public void loadImage(string _url, Action<byte[]> _downloadSucAction)
        {
            //判空
            if (string.IsNullOrEmpty(_url))
            {
                _downloadSucAction?.Invoke(null);
                return;
            }

            //文件名
            string fileName = Path.GetFileName(_url);
            //判空
            if (string.IsNullOrEmpty(fileName))
            {
                _downloadSucAction?.Invoke(null);
                return;
            }

            //存储路径
            string saveFilePath = _m_sSettingFilePath + "/" + fileName;

            //已经下载过，有数据
            if (File.Exists(saveFilePath))
            {
                _downloadSucAction?.Invoke(_filePath2Bytes(saveFilePath));
                return;
            }
            else
            {
                //注册回调
                if (_m_dDownloadDoneActionDic == null)
                    _m_dDownloadDoneActionDic = new Dictionary<string, Action<byte[]>>();
                if (!_m_dDownloadDoneActionDic.ContainsKey(_url))
                    _m_dDownloadDoneActionDic[_url] = _downloadSucAction;
                else
                    _m_dDownloadDoneActionDic[_url] += _downloadSucAction;

                //下载临时存储路径
                string tempSaveFilePath = _m_sTempSettingFilePath + "/" + fileName + ALSerializeOpMgr.next();

                ALHttpSingleDownloadDealer dealer = null;
                _downloadImage(_url, saveFilePath, tempSaveFilePath, () =>
                {
                    if (_AALMonoMain.instance.showDebugOutput)
                        Debug.Log($"【{_m_sTag}】图片下载成功，下载地址：{_url}，图片大小为【{(dealer == null ? 0 : dealer.fileSize) / 1000}K】");

                    //执行回调
                    if (_m_dDownloadDoneActionDic != null && _m_dDownloadDoneActionDic.TryGetValue(_url,out Action<byte[]> _downloadDoneAction))
                    {
                        _downloadDoneAction?.Invoke(_filePath2Bytes(saveFilePath));
                        _m_dDownloadDoneActionDic?.Remove(_url);
                    }

                }, (_errStat) =>
                {
                }, out dealer);
            }
        }

        /// <summary>
        /// 下载图片
        /// </summary>
        /// <param name="_url"></param>
        /// <param name="_saveFilePath"></param>
        /// <param name="_tempSaveFilePath"></param>
        /// <param name="_onSucCallBack"></param>
        /// <param name="_onFailCallBack"></param>
        /// <param name="_dealer"></param>
        private void _downloadImage(string _url, string _saveFilePath, string _tempSaveFilePath, Action _onSucCallBack, Action<int> _onFailCallBack, out ALHttpSingleDownloadDealer _dealer)
        {
            _dealer = null;

            if (string.IsNullOrEmpty(_url) || string.IsNullOrEmpty(_saveFilePath) || string.IsNullOrEmpty(_tempSaveFilePath))
                return;

            if (_m_dUrl2DownloadDealerDic == null)
                _m_dUrl2DownloadDealerDic = new Dictionary<string, ALHttpSingleDownloadDealer>();

            //在下载中
            _m_dUrl2DownloadDealerDic.TryGetValue(_url, out ALHttpSingleDownloadDealer tempDealer);
            if (tempDealer != null)
                return;

            ALHttpSingleDownloadDealer dllDownloadDealer = new ALHttpSingleDownloadDealer(_url, _tempSaveFilePath, () =>
            {
                //如果没有记录此次下载，说明已经清空，不处理
                if (_m_dUrl2DownloadDealerDic == null || !_m_dUrl2DownloadDealerDic.ContainsKey(_url))
                    return;

                try
                {
                    //如果成功加载，则把临时文件复制到正式文件
                    File.Copy(_tempSaveFilePath, _saveFilePath, true);

                    //执行成功的回调
                    _onSucCallBack?.Invoke();
                }
                catch (Exception e)
                {
                    Debug.LogError($"【{_m_sTag}】ImageDownloadProvider _downloadImage 图片下载【成功】后拷贝文件出现异常：{e}");
                }

                //删除临时文件
                try
                {
                    ALFile.Delete(_tempSaveFilePath);
                }
                catch (Exception e)
                {
                    Debug.LogError($"【{_m_sTag}】ImageDownloadProvider _downloadImage 图片下载【成功】后删除临时文件出现异常：{e}");
                }

                _m_dUrl2DownloadDealerDic?.Remove(_url);
            }, (int _errStat) =>
            {
                _onFailCallBack?.Invoke(_errStat);

                //删除文件
                try
                {
                    ALFile.Delete(_tempSaveFilePath);
                }
                catch (Exception e)
                {
                    Debug.LogError($"【{_m_sTag}】ImageDownloadProvider _downloadImage 图片下载【失败】后删除临时文件出现异常：{e}");
                }

                _m_dUrl2DownloadDealerDic?.Remove(_url);
            });
            //记录此次下载
            _m_dUrl2DownloadDealerDic[_url] = dllDownloadDealer;
            //开始下载
            dllDownloadDealer.startLoad();

            _dealer = dllDownloadDealer;
        }

        /// <summary>
        /// 将路径下的图片转为Texture2D格式
        /// </summary>
        /// <param name="_filePath"></param>
        /// <returns></returns>
        private byte[] _filePath2Bytes(string _filePath)
        {
            if (string.IsNullOrEmpty(_filePath))
                return null;

            //是否有缓存字典，有则直接获取
            if (_m_dImageByteDic != null && _m_dImageByteDic.TryGetValue(_filePath, out byte[] _imageBytesValue))
                return _imageBytesValue;

            //转换成二进制数组
            byte[] imageBytes = File.ReadAllBytes(_filePath);

            //缓存到字典里
            if (_m_dImageByteDic == null)
                _m_dImageByteDic = new Dictionary<string, byte[]>();
            _m_dImageByteDic[_filePath] = imageBytes;

            return imageBytes;
        }

        /// <summary>
        /// 删除临时文件
        /// </summary>
        public void deleteTempFile()
        {
            _m_dUrl2DownloadDealerDic?.Clear();

            //删除文件
            try
            {
                if (string.IsNullOrEmpty(_m_sTempSettingFilePath) || !Directory.Exists(_m_sTempSettingFilePath))
                    return;

                var paths = Directory.GetFiles(_m_sTempSettingFilePath);
                foreach (var pt in paths)
                {
                    ALFile.Delete(pt);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"【{_m_sTag}】ImageDownloadProvider deleteTempFile 失败，异常报错：{e}");
            }
        }

        /// <summary>
        /// 删除所有
        /// </summary>
        public void delete()
        {
            //清空回调
            _m_dDownloadDoneActionDic?.Clear();

            //清空图片下载列表
            if (_m_dUrl2DownloadDealerDic != null)
            {
                foreach (KeyValuePair<string, ALHttpSingleDownloadDealer> dealer in _m_dUrl2DownloadDealerDic)
                {
                    if(dealer.Value != null)
                        dealer.Value.discard();
                }
                _m_dUrl2DownloadDealerDic?.Clear();
            }
            //清空图片缓存列表
            _m_dImageByteDic?.Clear();
            //删除临时文件
            deleteTempFile();
            //删除文件
            try
            {
                if (string.IsNullOrEmpty(_m_sSettingFilePath) || !Directory.Exists(_m_sTempSettingFilePath))
                    return;

                var paths = Directory.GetFiles(_m_sSettingFilePath);
                foreach (var pt in paths)
                {
                    ALFile.Delete(pt);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"【{_m_sTag}】ImageDownloadProvider delete 失败，异常报错：{e}");
            }
        }
    }
}
