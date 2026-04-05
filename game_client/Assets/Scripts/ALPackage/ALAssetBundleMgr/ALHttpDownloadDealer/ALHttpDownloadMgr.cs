using System;
using System.Collections.Generic;

using UnityEngine;

/*****************
 * 对远程下载资源的处理对象进行管理的对象
 **/
namespace ALPackage
{
    public class ALHttpDownloadMgr
    {
        //下载序列号
        private int _m_iDownloadSerialize;

        /** 总的下载安全线文件总尺寸（字节） */
        private long _m_iSaveTotalLoadingSizeB;
        //通过本管理对象下载的资源的重试次数
        private int _m_iRetryCount;

        //失败的统计数量
        private int _m_iFailCount;

        //所有的下载处理对象
        private List<ALHttpDownloadDealer> _m_lAllDealer;
        //等待处理的下载处理对象
        private LinkedList<ALHttpDownloadDealer> _m_lNeedDealDealer;
        //正在处理的下载处理对象队列
        private List<ALHttpDownloadDealer> _m_lLoadingDealDealer;

        //总的并行处理对象数量上限，避免过多并发。-1表示无上限
        private int _m_iTotalDealingCount;

        //整体需要下载的文件大小
        private long _m_lTotalLoadSize;
        //当前正在加载的文件大小
        private long _m_lCurLoadingSize;
        //已经解压处理的字节数
        private long _m_lUnzipSize;

        public ALHttpDownloadMgr(int _retryCount, long _saveLoadingSizeB)
        {
            _m_iDownloadSerialize = 1;

            _m_iRetryCount = _retryCount;
            _m_iFailCount = 0;
            _m_iSaveTotalLoadingSizeB = _saveLoadingSizeB;

            _m_iTotalDealingCount = 3;

            _m_lAllDealer = new List<ALHttpDownloadDealer>();
            _m_lNeedDealDealer = new LinkedList<ALHttpDownloadDealer>();
            _m_lLoadingDealDealer = new List<ALHttpDownloadDealer>();
            _m_lTotalLoadSize = 0;
            _m_lCurLoadingSize = 0;
            _m_lUnzipSize = 0;
        }
        public ALHttpDownloadMgr(int _retryCount, long _saveLoadingSizeB, int _totalDealingCount)
        {
            _m_iDownloadSerialize = 1;

            _m_iRetryCount = _retryCount;
            _m_iFailCount = 0;
            _m_iSaveTotalLoadingSizeB = _saveLoadingSizeB;

            _m_iTotalDealingCount = _totalDealingCount;

            _m_lAllDealer = new List<ALHttpDownloadDealer>();
            _m_lNeedDealDealer = new LinkedList<ALHttpDownloadDealer>();
            _m_lLoadingDealDealer = new List<ALHttpDownloadDealer>();
            _m_lTotalLoadSize = 0;
            _m_lCurLoadingSize = 0;
            _m_lUnzipSize = 0;
        }

        public int downloadSerialize { get { return _m_iDownloadSerialize; } }
        public int failCount { get { return _m_iFailCount; } }
        public long totalSize { get { return _m_lTotalLoadSize; } }
        public long loadingSize { get { return _m_lCurLoadingSize; } }
        public long unzipSize { get { return _m_lUnzipSize; } }
        //是否全部下载都完成了
        public bool isDone {
            get {
                lock(_m_lLoadingDealDealer)
                {
                    if(_m_lNeedDealDealer.Count > 0)
                        return false;
                    if(_m_lLoadingDealDealer.Count > 0)
                        return false;

                    return true;
                }
            }
        }
        //所有对象已经完成的加载尺寸
        public long loadedSize
        {
            get
            {
                long size = 0;
                for (int i = 0; i < _m_lAllDealer.Count; i++)
                {
                    ALHttpDownloadDealer dealer = _m_lAllDealer[i];
                    if (null == dealer)
                        continue;

                    size += dealer.downloadedBytes;
                }

                return size;
            }
        }
        //正在加载的对象中已经加载完成的文件尺寸
        public long loadingLoadedSize
        {
            get
            {
                long size = 0;
                lock (_m_lLoadingDealDealer)
                {
                    for (int i = 0; i < _m_lLoadingDealDealer.Count; i++)
                    {
                        ALHttpDownloadDealer dealer = _m_lLoadingDealDealer[i];
                        if (null == dealer)
                            continue;

                        size += dealer.downloadedBytes;
                    }
                }

                return size;
            }
        }

        public void addFailCount() { _m_iFailCount++; }

        /// <summary>
        /// 获取下载资源的进度
        /// </summary>
        /// <param name="_resUrl"></param>
        /// <returns></returns>
        public long getAssetProgressTenThouthan(string _resUrl)
        {
            //先在所有加载集合中寻找是否有一样的下载资源
            ALHttpDownloadDealer tmpDealer = null;
            for(int i = 0; i < _m_lAllDealer.Count; i++)
            {
                tmpDealer = _m_lAllDealer[i];
                if(null == tmpDealer)
                    continue;

                if(tmpDealer.URL.Equals(_resUrl))
                {
                    return tmpDealer.downloadedBytes * 10000 / tmpDealer.fileSize;
                }
            }

            return 10000;
        }

        /// <summary>
        /// 查询下载对象
        /// </summary>
        /// <param name="_resUrl"></param>
        /// <returns></returns>
        public ALHttpDownloadDealer getDownloadDealer(string _assetPath)
        {
            ALHttpDownloadDealer tmpDealer = null;
            for(int i = 0; i < _m_lAllDealer.Count; i++)
            {
                tmpDealer = _m_lAllDealer[i];
                if(null == tmpDealer)
                    continue;

                if(tmpDealer.assetPath.Equals(_assetPath, StringComparison.OrdinalIgnoreCase))
                {
                    return tmpDealer;
                }
            }

            return null;
        }

        /*******************
         * 调整解压处理的文件大小
         **/
        public void chgUnzipSize(long _chgSize)
        {
            _m_lUnzipSize += _chgSize;
        }

        /****************
         * 下载对应资源并使用回调对象进行处理
         **/
        public void download(string _assetPath, string _resUrl, long _fileSize, string _tmpFilePath, _AALHttpDownloadDelegate _delegate)
        {
            //先在所有加载集合中寻找是否有一样的下载资源
            ALHttpDownloadDealer tmpDealer = null;
            ALHttpDownloadDealer fitDealer = null;
            for(int i = 0; i < _m_lAllDealer.Count; i++)
            {
                tmpDealer = _m_lAllDealer[i];
                if(null == tmpDealer)
                    continue;

                if(tmpDealer.URL.Equals(_resUrl))
                {
                    fitDealer = tmpDealer;
                    break;
                }
            }

            //根据是否匹配到对应下载对象做不同处理
            if(null != fitDealer)
            {
                //在已有对象中注册回调
                fitDealer.addDelegate(_delegate);
            }
            else
            {
                fitDealer = new ALHttpDownloadDealer(this, _assetPath, _resUrl, _fileSize, _tmpFilePath);
                //注册回调
                fitDealer.addDelegate(_delegate);
                //增加总加载大小
                _m_lTotalLoadSize += _fileSize;
                //将处理对象放入队列
                _m_lAllDealer.Add(fitDealer);
                //获取是否需要开启任务
                bool needStartMonitor = false;

                lock(_m_lLoadingDealDealer)
                {
                    needStartMonitor = (_m_lNeedDealDealer.Count <= 0);
                    //放入等待处理的队列，等待机制调用开启加载
                    _m_lNeedDealDealer.AddLast(fitDealer);
                }

                //判断是否需要开启任务，当队列原先为空则需要开启
                if(needStartMonitor)
                    ALMonoTaskMgr.instance.addMonoTask(new ALHttpDownloadMonitor(this));
            }
        }
        /***************
         * 重试对应的加载对象
         **/
        public void retryDealer(ALHttpDownloadDealer _dealer)
        {
            if (null == _dealer || _dealer.downloadMgr != this)
                return;


            lock (_m_lLoadingDealDealer)
            {
                //从在处理的队列中删除，如删除失败表示本对象非在处理对象，不进行后续处理
                if (!_m_lLoadingDealDealer.Remove(_dealer))
                    return;
            }

            //减少正在加载的尺寸
            _m_lCurLoadingSize -= _dealer.fileSize;

            //获取是否需要开启任务
            bool needStartMonitor = false;

            lock(_m_lLoadingDealDealer)
            {
                needStartMonitor = (_m_lNeedDealDealer.Count <= 0);
                //放入等待处理的队列，等待机制调用开启加载
                _m_lNeedDealDealer.AddLast(_dealer);
            }

            //判断是否需要开启任务，当队列原先为空则需要开启
            if (needStartMonitor)
                ALMonoTaskMgr.instance.addMonoTask(new ALHttpDownloadMonitor(this));
        }

        /// <summary>
        /// 清空下载信息
        /// </summary>
        public void clearDownloadInfo()
        {
            _m_iDownloadSerialize++;

            _m_iFailCount = 0;

            _m_lAllDealer.Clear();
            _m_lNeedDealDealer.Clear();
            _m_lLoadingDealDealer.Clear();
            _m_lTotalLoadSize = 0;
            _m_lCurLoadingSize = 0;
            _m_lUnzipSize = 0;
        }

        /***************
         * 释放相关资源
         **/
        public void discard()
        {
            //清空调用队列
            clearDownloadInfo();
        }

        /*****************
         * 取出需要下载的任务信息，并开启加载，返回是否需要继续任务
         **/
        protected internal bool _popAndDownload()
        {
            lock(_m_lLoadingDealDealer)
            {
                //在加载尺寸还在安全区域内时进入循环判断
                while(_m_lCurLoadingSize < _m_iSaveTotalLoadingSizeB
                    && (_m_iTotalDealingCount < 0 || _m_lLoadingDealDealer.Count < _m_iTotalDealingCount))
                {
                    if(null == _m_lNeedDealDealer || _m_lNeedDealDealer.Count <= 0)
                        return false;

                    //取出第一个需要加载的对象
                    ALHttpDownloadDealer dealer = _m_lNeedDealDealer.First.Value;
                    if(null == dealer)
                    {
                        _m_lNeedDealDealer.RemoveFirst();
                        continue;
                    }

                    //同时下载文件数量是否超过上限
                    if (_m_iTotalDealingCount > 0 && _m_lLoadingDealDealer.Count > _m_iTotalDealingCount)
                        return true;

                    //判断是否需要继续下载
                    if(_m_lLoadingDealDealer.Count > 0 && (_m_lCurLoadingSize + dealer.fileSize > _m_iSaveTotalLoadingSizeB))
                        return true;

                    //增加正在加载的尺寸
                    _m_lCurLoadingSize += dealer.fileSize;

                    //开启下载，并删除队列节点
                    _m_lNeedDealDealer.RemoveFirst();
                    //增加处理对象到队列中
                    _m_lLoadingDealDealer.Add(dealer);
                    //开启下载
                    dealer._startLoad(_m_iRetryCount);
                }

                //正常退出循环需要继续任务
                return true;
            }
        }

        /**********************
         * 在有处理对象完成处理后的回调操作
         **/
        protected internal void _onDealerDone(ALHttpDownloadDealer _dealer)
        {
            lock(_m_lLoadingDealDealer)
            {
                //从队列中删除
                _m_lLoadingDealDealer.Remove(_dealer);
                //减少正在加载的尺寸
                _m_lCurLoadingSize -= _dealer.fileSize;
            }
        }

        /// <summary>
        /// 在处理对象失败时调用的处理
        /// </summary>
        /// <param name="_dealer"></param>
        protected internal void _onDealerFail(ALHttpDownloadDealer _dealer)
        {
            lock(_m_lLoadingDealDealer)
            {
                //从队列中删除
                _m_lLoadingDealDealer.Remove(_dealer);
                //减少正在加载的尺寸
                _m_lCurLoadingSize -= _dealer.fileSize;
            }
        }
    }
}
