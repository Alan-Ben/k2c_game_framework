using System;
using System.IO;
using System.Net;
using System.Text;
using LitJson;
using UnityEngine;

namespace ALPackage
{
    /// <summary>
    /// 检查本地保存的CDN配置
    /// </summary>
    public class ALCDNLocalSaveCheckMgr<T>
    {
        private static ALCDNLocalSaveCheckMgr<T> _g_instance = new ALCDNLocalSaveCheckMgr<T>();
        public static ALCDNLocalSaveCheckMgr<T> instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new ALCDNLocalSaveCheckMgr<T>();
                return _g_instance;
            }
        }

        /// <summary>
        /// 检查本地存储的CDN数据是否最新的，是则返回最新数据，否则返回空
        /// </summary>
        /// <param name="_type">CDN类型</param>
        public void checkLocalConfig(_TALCommonCdnConfig<T> _curObj, ALURLDownloader _downloader, string _filePath, Action<bool, _TALCommonCdnConfig<T>> _checkNewestDone)
        {
            if (_AALMonoMain.instance.showDebugOutput)
                Debug.Log($"【{Time.frameCount}】[CDN]CDNLocalSaveCheckMgr----------检查本地保存的CDN配置 type:{typeof(T)}    downloadRootUrl:{_filePath}");

            //先赋予当前值
            _TALCommonCdnConfig<T> result = _curObj;

            //如果本地没有保存过数据，则返回空
            if (result == null || string.IsNullOrEmpty(result.version) || string.IsNullOrEmpty(result.version) || result.config == null)
            {
                if (_AALMonoMain.instance.showDebugOutput)
                    Debug.Log($"【{Time.frameCount}】[CDN]CDNLocalSaveCheckMgr----------本地没有保存过数据  type:{typeof(T)}");
                if(_checkNewestDone != null)
                    _checkNewestDone(false, null);
                _checkNewestDone = null;
                return;
            }

            //本地保存过数据，则尝试能否取到next_version的cdn数据
            _checkNextVersion(
                _downloader
                , _filePath
                , result.next_version,
                () =>
                {
                    if (_AALMonoMain.instance.showDebugOutput)
                        Debug.Log($"【{Time.frameCount}】[CDN]CDNLocalSaveCheckMgr----------成功取到数据，说明本地存储的不是最新的CDN数据，重新走循环更新  type:{typeof(T)} version:{result.version}");

                    //成功取到数据，说明本地存储的不是最新的CDN数据，重新走循环更新
                    if (_checkNewestDone != null)
                        _checkNewestDone(false, result);
                    _checkNewestDone = null;
                }, 
                () =>
                {
                    if (_AALMonoMain.instance.showDebugOutput)
                        Debug.Log($"【{Time.frameCount}】[CDN]CDNLocalSaveCheckMgr----------未取到数据，说明当前数据是最新的  type:{typeof(T)} version:{result.version} \ninfo:{JsonMapper.ToJson(result)}");

                    //未取到数据，说明当前数据是最新的
                    if (_checkNewestDone != null)
                        _checkNewestDone(true, result);
                    _checkNewestDone = null;
                });
        }

        /// <summary>
        /// 检查next_version是否可以取到
        /// </summary>
        /// <param name="_downloadRootUrl"></param>
        /// <param name="_targetVersion"></param>
        /// <param name="_onSuc"></param>
        /// <param name="_onFail"></param>
        private void _checkNextVersion(ALURLDownloader _downloader, string _filePath, string _targetVersion, Action _onSuc, Action _onFail)
        {
            if (_AALMonoMain.instance.showDebugOutput)
                Debug.Log($"【{Time.frameCount}】[CDN]CDNLocalSaveCheckMgr----------检查next_version是否可以取到  nextVersion:{_targetVersion}");

            
            if (string.IsNullOrEmpty(_targetVersion))//本地已经保存过数据，但是没有next_version
            {
                if (_AALMonoMain.instance.showDebugOutput)
                    Debug.Log($"【{Time.frameCount}】[CDN]CDNLocalSaveCheckMgr----------本地已经保存过数据，但是没有next_version  filePath:{_filePath}");
                if(_onFail != null)
                    _onFail();
                return;
            }

            string realFilePath = $"{_filePath}/{_targetVersion}.txt";

            //全部使用轮询，可以尽快得到结果
            _downloader.loopCheckFileInfoFail(realFilePath
                , (_info) => { if(null != _onSuc) _onSuc(); }
                , _onFail);
        }
    }
}
