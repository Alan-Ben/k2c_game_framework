using System;
using System.Collections.Generic;

using UnityEngine;

/*****************
 * 对远程下载资源的处理对象进行管理的对象
 **/
namespace ALPackage
{
    public class ALWWWDownloadMgr
    {
        /** 总的下载安全线文件总尺寸（字节） */
        private long _m_iSaveTotalLoadingSizeB;
        //通过本管理对象下载的资源的重试次数
        private int _m_iRetryCount;

        //所有的下载处理对象
        private List<ALWWWDownloadDealer> _m_lAllDealer;
        //等待处理的下载处理对象
        private LinkedList<ALWWWDownloadDealer> _m_lNeedDealDealer;
        //正在处理的下载处理对象队列
        private List<ALWWWDownloadDealer> _m_lLoadingDealDealer;

        //总的并行处理对象数量上限，避免过多并发。-1表示无上限
        private int _m_iTotalDealingCount;

        //整体需要下载的文件大小
        private long _m_lTotalLoadSize;
        //当前正在加载的文件大小
        private long _m_lCurLoadingSize;
        //已经解压处理的字节数
        private long _m_lUnzipSize;

        public ALWWWDownloadMgr(int _retryCount, long _saveLoadingSizeB)
        {
            _m_iRetryCount = _retryCount;
            _m_iSaveTotalLoadingSizeB = _saveLoadingSizeB;

            _m_iTotalDealingCount = 3;

            _m_lAllDealer = new List<ALWWWDownloadDealer>();
            _m_lNeedDealDealer = new LinkedList<ALWWWDownloadDealer>();
            _m_lLoadingDealDealer = new List<ALWWWDownloadDealer>();
            _m_lTotalLoadSize = 0;
            _m_lCurLoadingSize = 0;
            _m_lUnzipSize = 0;
        }
        public ALWWWDownloadMgr(int _retryCount, long _saveLoadingSizeB, int _totalDealingCount)
        {
            _m_iRetryCount = _retryCount;
            _m_iSaveTotalLoadingSizeB = _saveLoadingSizeB;

            _m_iTotalDealingCount = _totalDealingCount;

            _m_lAllDealer = new List<ALWWWDownloadDealer>();
            _m_lNeedDealDealer = new LinkedList<ALWWWDownloadDealer>();
            _m_lLoadingDealDealer = new List<ALWWWDownloadDealer>();
            _m_lTotalLoadSize = 0;
            _m_lCurLoadingSize = 0;
            _m_lUnzipSize = 0;
        }

        public long totalSize { get { return _m_lTotalLoadSize; } }
        public long loadingSize { get { return _m_lCurLoadingSize; } }
        public long unzipSize { get { return _m_lUnzipSize; } }
        //是否全部下载都完成了
        public bool isDone { get { if (_m_lNeedDealDealer.Count > 0) return false; if (_m_lLoadingDealDealer.Count > 0) return false; return true; } }
        //所有对象已经完成的加载尺寸
        public long loadedSize
        {
            get
            {
                long size = 0;
                for (int i = 0; i < _m_lAllDealer.Count; i++)
                {
                    ALWWWDownloadDealer dealer = _m_lAllDealer[i];
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
                for(int i = 0; i < _m_lLoadingDealDealer.Count; i++)
                {
                    ALWWWDownloadDealer dealer = _m_lLoadingDealDealer[i];
                    if (null == dealer)
                        continue;

                    size += dealer.downloadedBytes;
                }

                return size;
            }
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
        public ALWWWDownloadDealer download(string _resUrl, long _fileSize, _AALWWWDownloadDelegate _delegate)
        {
            ALWWWDownloadDealer dealer = new ALWWWDownloadDealer(this, _resUrl, _fileSize, _delegate);
            //增加总加载大小
            _m_lTotalLoadSize += _fileSize;
            //将处理对象放入队列
            _m_lAllDealer.Add(dealer);
            //获取是否需要开启任务
            bool needStartMonitor = (_m_lNeedDealDealer.Count <= 0);
            //放入等待处理的队列，等待机制调用开启加载
            _m_lNeedDealDealer.AddLast(dealer);

            //判断是否需要开启任务，当队列原先为空则需要开启
            if (needStartMonitor)
                ALMonoTaskMgr.instance.addMonoTask(new ALWWWDownloadMonitor(this));

            return dealer;
        }
        /***************
         * 重试对应的加载对象
         **/
        public void retryDealer(ALWWWDownloadDealer _dealer)
        {
            if (null == _dealer || _dealer.downloadMgr != this)
                return;

            //从在处理的队列中删除，如删除失败表示本对象非在处理对象，不进行后续处理
            if (!_m_lLoadingDealDealer.Remove(_dealer))
                return;
            //减少正在加载的尺寸
            _m_lCurLoadingSize -= _dealer.fileSize;

            //获取是否需要开启任务
            bool needStartMonitor = (_m_lNeedDealDealer.Count <= 0);
            //放入等待处理的队列，等待机制调用开启加载
            _m_lNeedDealDealer.AddLast(_dealer);

            //判断是否需要开启任务，当队列原先为空则需要开启
            if (needStartMonitor)
                ALMonoTaskMgr.instance.addMonoTask(new ALWWWDownloadMonitor(this));
        }

        /*****************
         * 取出需要下载的任务信息，并开启加载，返回是否需要继续任务
         **/
        protected internal bool _popAndDownload()
        {
            //在加载尺寸还在安全区域内时进入循环判断
            while(_m_lCurLoadingSize < _m_iSaveTotalLoadingSizeB
                && (_m_iTotalDealingCount < 0 || _m_lLoadingDealDealer.Count < _m_iTotalDealingCount))
            {
                if(null == _m_lNeedDealDealer || _m_lNeedDealDealer.Count <= 0)
                    return false;

                //取出第一个需要加载的对象
                ALWWWDownloadDealer dealer = _m_lNeedDealDealer.First.Value;
                if (null == dealer)
                {
                    _m_lNeedDealDealer.RemoveFirst();
                    continue;
                }

                //同时下载文件数量是否超过上限
                if (_m_iTotalDealingCount > 0 && _m_lLoadingDealDealer.Count > _m_iTotalDealingCount)
                    return true;

                //判断是否需要继续下载
                if (_m_lLoadingDealDealer.Count > 0 && (_m_lCurLoadingSize + dealer.fileSize > _m_iSaveTotalLoadingSizeB))
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

        /**********************
         * 在有处理对象完成处理后的回调操作
         **/
        protected internal void _onDealerDone(ALWWWDownloadDealer _dealer)
        {
            //从队列中删除
            _m_lLoadingDealDealer.Remove(_dealer);
            //减少正在加载的尺寸
            _m_lCurLoadingSize -= _dealer.fileSize;
        }
    }
}
