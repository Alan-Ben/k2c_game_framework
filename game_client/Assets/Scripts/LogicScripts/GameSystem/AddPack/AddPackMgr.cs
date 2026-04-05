
using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// AddPack 增量包的管理器
    /// </summary>
    public class AddPackMgr
    {
        [NotNull] public static AddPackMgr instance { get { return _g_instance ??= new AddPackMgr(); } } 
        private static AddPackMgr _g_instance;

        
        // 增量包安装器列表
        [ItemNotNull, NotNull] private readonly List<AddPackInstaller> _m_installerList;
        // 是否初始化
        private bool _m_isInit;
        
        
        private AddPackMgr()
        {
            _m_installerList = new List<AddPackInstaller>();
            _m_isInit = false;
        }
        
        
        /// <summary>
        /// 各个增量包的安装器列表
        /// </summary>
        [ItemNotNull, NotNull] public IReadOnlyList<AddPackInstaller> installerList { get { return _m_installerList; } }
        /// <summary>
        /// 是否初始化
        /// </summary>
        public bool isInit { get { return _m_isInit; } }


        /// <summary>
        /// 初始化
        /// </summary>
        public void init(Action _complete)
        {
            if (_m_isInit)
            {
                ALLog.Warning("AddPackMgr.init: AddPackMgr is already init.");
                _complete?.Invoke();
                return;
            }
            if (!GRefdataCoreMgr.instance.addPackRefCore.isInit)
            {
                ALLog.Error("AddPackMgr.init failed: AddPackRefCore is not init.");
                _complete?.Invoke();
                return;
            }

            _m_isInit = true;
            
            // 启用计步器，等所有 installer 初始化完成后调用 _complete
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(GRefdataCoreMgr.instance.addPackRefCore.refList.Count + 1);
            stepCounter.regAllDoneDelegate(() => ALLog.Sys("AddPackMgr.init done"));
            stepCounter.regAllDoneDelegate(_complete);
            
            // 遍历配置的增量包内容，创建安装器
            GRefdataCoreMgr.instance.addPackRefCore.dealAllRef(_refObj =>
            {
                if (_refObj == null)
                {
                    stepCounter.addDoneStepCount();
                    return;
                }

                AddPackInstaller installer = new AddPackInstaller(_refObj);
                _m_installerList.Add(installer);
                installer.init(stepCounter.addDoneStepCount);
            });
            
            stepCounter.addDoneStepCount();
        }
        /// <summary>
        /// 销毁这个管理器
        /// </summary>
        public void discard()
        {
            if (!_m_isInit)
            { 
                ALLog.Warning("AddPackMgr.discard: AddPackMgr is already discard.");
                return;
            }

            _m_isInit = false;
            
            for (int i = _m_installerList.Count - 1; i >= 0; i--)
            {
                AddPackInstaller installer = _m_installerList[i];
                _m_installerList.Remove(installer);
                installer.discard();
            }
            
            ALLog.Sys("AddPackMgr.discard done");
        }
        public bool needDownload()
        {
            if (!_m_isInit)
            {
                ALLog.Warning("AddPackMgr.needDownload: AddPackMgr is not init.");
                return false;
            }
            
            bool needDownload = false;
            foreach (AddPackInstaller installer in _m_installerList)
            {
                if (!installer.download.isComplete)
                {
                    needDownload = true;
                    break;
                }
            }

            return needDownload;
        }
    }
}