using System;
using System.Collections.Generic;

using UnityEngine;

/*******************
 * 资源控制中心更新所有资源时的监控操作对象
 **/
namespace ALPackage
{
    public class ALResourceUpdateMonitor : _IALBaseMonoTask
    {
        private ALHttpDownloadMgr _m_dmDownloadMgr;
        //过程回调
        private Action<long, long, long, bool> _m_dProcessDelegate;
        //完成回调
        private Action _m_dDoneDelegate;

        public ALResourceUpdateMonitor(ALHttpDownloadMgr _downloadMgr, Action<long, long, long, bool> _processDelegate, Action _doneDelegate)
        {
            _m_dmDownloadMgr = _downloadMgr;
            _m_dProcessDelegate = _processDelegate;
            _m_dDoneDelegate = _doneDelegate;
        }

        //过程函数
        public void deal()
        {
            //无回调则直接返回
            if (null == _m_dProcessDelegate && null == _m_dDoneDelegate)
                return;

            //判断是否无效数据或当前是否已经完成，是则直接调用完成回调
            if (null == _m_dmDownloadMgr || _m_dmDownloadMgr.isDone)
            {
                //直接完成
                if (null != _m_dDoneDelegate)
                    _m_dDoneDelegate();
                return;
            }

            //获取当前的进度相关数据，带入3个参数分别是，已下载字节数，已解压字节数，总字节数，当前是否在下载中
            if (null != _m_dProcessDelegate)
                _m_dProcessDelegate(_m_dmDownloadMgr.loadedSize, _m_dmDownloadMgr.unzipSize, _m_dmDownloadMgr.totalSize, (_m_dmDownloadMgr.loadingSize != _m_dmDownloadMgr.loadingLoadedSize));

            //继续添加本任务
            ALMonoTaskMgr.instance.addNextFrameTask(this);
        }
    }
}
