
using System;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    public class AddPackInstaller
    {
        [NotNull] private readonly AddPackRefObj _m_refObj;
        private AssetBundleDownloadMgr _m_downloadMgr;
        private bool _m_isInit;
        
        
        public AddPackInstaller([NotNull] AddPackRefObj _refObj)
        {
            _m_refObj = _refObj;
        }


        public AddPackRefObj refObj { get { return _m_refObj; } }
        public AssetBundleDownloadMgr download { get { return _m_downloadMgr; } }
        public bool isInit { get { return _m_isInit; } }


        public void init(Action _complete)
        {
            if (_m_isInit)
            {
                ALLog.Warning("AddPackInstaller.init: AddPackInstaller is already init.");
                _complete?.Invoke();
                return;
            }

            _m_isInit = true;
            
            _m_downloadMgr = new AssetBundleDownloadMgr(GameResCore.instance, _m_refObj.pack_paths);
            _complete?.Invoke();
        }
        public void discard()
        {
            if (!_m_isInit)
            {
                ALLog.Warning("AddPackInstaller.discard: AddPackInstaller is not init.");
                return;
            }
            
            _m_isInit = false;
            
            _m_downloadMgr.abortDownload();
            _m_downloadMgr = null;
        }
    }
}