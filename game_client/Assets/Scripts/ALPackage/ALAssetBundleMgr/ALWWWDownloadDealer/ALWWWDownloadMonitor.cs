using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

/*****************
 * 每帧处理的下载任务处理对象
 **/
namespace ALPackage
{
    public class ALWWWDownloadMonitor : _IALBaseMonoTask
    {
        /** 下载管理对象 */
        private ALWWWDownloadMgr _m_dmDownloadMgr;

        protected internal ALWWWDownloadMonitor(ALWWWDownloadMgr _mgr)
        {
            _m_dmDownloadMgr = _mgr;
        }

        public void deal()
        {
            if (_m_dmDownloadMgr._popAndDownload())
            {
                //注册本任务到下一帧
                ALMonoTaskMgr.instance.addNextFrameTask(this);
            }
        }
    }
}
