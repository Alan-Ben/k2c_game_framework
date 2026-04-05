using System;
using System.Collections.Generic;

using ALPackage;

namespace GOE
{
    /** 资源更新的管理对象 */
    public class ResUpdateMonitor
    {
        /** 资源更新某个源的信息 */
        public class ResUpdateCoreInfo
        {
            /** 监控对象 */
            private ResUpdateMonitor _m_umMonitor;
            /** 总更新大小 */
            private long _m_lTotalSize;
            /** 更新行为函数 */
            private Action _m_dUpdateAction;

            /** 是否初始化完成 */
            private bool _m_bIsInited;
            /** 是否完成 */
            private bool _m_bIsDone;

            /** 是否已经记录了log */
            private bool _m_bDealProcessLog;

            public ResUpdateCoreInfo(ResUpdateMonitor _monitor)
            {
                _m_umMonitor = _monitor;

                _m_lTotalSize = 0;
                _m_dUpdateAction = null;

                _m_bIsInited = false;
                _m_bIsDone = false;

                _m_bDealProcessLog = false;
            }

            public long totalSize { get { return _m_lTotalSize; } }
            public void setTotalSize(long _totalSize) { _m_lTotalSize = _totalSize; }

            public bool isInited { get { return _m_bIsInited; } }
            public bool isDone { get { return _m_bIsDone; } }

            /** 设置处理函数 */
            public void setDealFunc(Action _func)
            {
                _m_dUpdateAction = _func;
            }

            /** 开始处理下载更新 */
            public void startDeal()
            {
                if(_m_bIsDone)
                    return;

                //开启函数
                if(null != _m_dUpdateAction)
                {
                    _m_dUpdateAction();
                }
                else
                {
                    //直接当作处理完成
                    onDownloadDone();
                }
            }

            /** 进度处理函数，此时调用父节点的处理函数 */
            public void onDownloadingProcess(long _loadedSize, long _unzipSize, long _totalSize, bool _isDownloading)
            {
                if(null == _m_umMonitor)
                    return;

                //判断进度
                if(!_m_bDealProcessLog && _loadedSize >= (_totalSize / 10))
                {
                    _m_bDealProcessLog = true;
                    //记录log
                    //WCGLoginLogPointMgr.instance.addDetailLog(_m_eProcessLog);
                }

                _m_umMonitor._onDownloadingGameRes(_loadedSize, _unzipSize, _totalSize, _isDownloading, this);
            }

            /**************
             * 处理下载完成的操作
             **/
            public void onDownloadDone()
            {
                _m_bIsInited = true;
                _m_bIsDone = true;

                //记录log
                //WCGLoginLogPointMgr.instance.addDetailLog(_m_eDoneLog);

                //调用父节点完成
                _m_umMonitor.onDownloadDoneGame(this);

                //释放资源
                discard();
            }

            /** 释放相关资源 */
            public void discard()
            {
                _m_dUpdateAction = null;
            }
        }

        //监控不同源的信息
        private List<ResUpdateCoreInfo> _m_lCoreInfoList;
        //总的更新大小
        private long _m_lTotalSize;
        //已经完成的所有节点的大小
        private long _m_lDoneTotalSize;

        //更新过程响应函数
        private Action<long, long, long, bool> _m_dProcessDelegate;
        //结束的时候响应函数
        private Action _m_dDoneDelegate;

        //是否初始化完成
        private bool _m_bIsInited;

        public ResUpdateMonitor()
        {
            _m_lCoreInfoList = new List<ResUpdateCoreInfo>();
            _m_lTotalSize = 0;
            _m_lDoneTotalSize = 0;

            _m_dProcessDelegate = null;
            _m_dDoneDelegate = null;

            _m_bIsInited = false;
        }

        /** 注册一个源信息 */
        public ResUpdateCoreInfo regCoreInfo()
        {
            ResUpdateCoreInfo newInfo = new ResUpdateCoreInfo(this);
            //加入队列
            _m_lCoreInfoList.Add(newInfo);

            return newInfo;
        }

        /** 设置进度函数 */
        public void setProcessDelegate(Action<long, long, long, bool> _processDelegate, Action _onDoneDelegate)
        {
            _m_dProcessDelegate = _processDelegate;
            _m_dDoneDelegate = _onDoneDelegate;
        }

        //计算总尺寸
        public long calTotalSize()
        {
            long totalSize = 0;
            for(int i = 0; i < _m_lCoreInfoList.Count; i++)
            {
                totalSize += _m_lCoreInfoList[i].totalSize;
            }

            return totalSize;
        }

        /** 开始下载 */
        public void startDownload()
        {
            //遍历所有节点计算总尺寸
            _m_lTotalSize = 0;
            _m_lDoneTotalSize = 0;
            for(int i = 0; i < _m_lCoreInfoList.Count; i++)
            {
                _m_lTotalSize += _m_lCoreInfoList[i].totalSize;
            }

            _m_bIsInited = true;

            //开始下载处理
            _checkNextCoreInfo();
        }

        /** 下载完成的处理操作 */
        public void onDownloadDoneGame(ResUpdateCoreInfo _coreInfo)
        {
            //累加完成进度
            _m_lDoneTotalSize += _coreInfo.totalSize;

            //检查是否全部完成，并开启下一个任务处理
            _checkNextCoreInfo();
        }

        /** 在某个源更新的过程处理函数 */
        protected void _onDownloadingGameRes(long _loadedSize, long _unzipSize, long _totalSize, bool _isDownloading, ResUpdateCoreInfo _coreInfo)
        {
            //计算进度，实际下载进度
            if(null != _m_dProcessDelegate)
                _m_dProcessDelegate(_m_lDoneTotalSize + _loadedSize, _m_lDoneTotalSize + _unzipSize, _m_lTotalSize, _isDownloading);
        }

        /** 检查是否全部完成 */
        protected void _checkNextCoreInfo()
        {
            //判断是否初始化
            if(!_m_bIsInited)
                return;

            //检测任意未开启的，处理开启
            for(int i = 0; i < _m_lCoreInfoList.Count; i++)
            {
                if(!_m_lCoreInfoList[i].isDone)
                {
                    _m_lCoreInfoList[i].startDeal();
                    return;
                }
            }

            //设置全部完成
            if(null != _m_dDoneDelegate)
            {
                _m_dDoneDelegate();
            }

            //释放相关资源
            _m_lCoreInfoList.Clear();
            _m_dDoneDelegate = null;
            _m_dProcessDelegate = null;
        }
    }
}
