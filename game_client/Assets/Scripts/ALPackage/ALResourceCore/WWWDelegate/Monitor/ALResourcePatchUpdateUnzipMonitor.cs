using System;
using System.Collections.Generic;

using UnityEngine;

#if AL_SEVEN_ZIP
/***********************
 * 资源更新处理对象的解压进度监控任务对象
 **/
namespace ALPackage
{
    public class ALResourcePatchUpdateUnzipMonitor : _IALBaseMonoTask
    {
        /** 下载的处理对象 */
        private ALHttpDownloadMgr _m_dmDownloadMgr;
        /** 更新的回调处理对象 */
        private ALResourcePatchUpdateWWWCompressDelegate _m_udUpadateDelegate;

        /** 原先的处理字节数 */
        private long _m_lPreUnzipSize;

        public ALResourcePatchUpdateUnzipMonitor(ALHttpDownloadMgr _downloadMgr, ALResourcePatchUpdateWWWCompressDelegate _updateDelegate)
        {
            _m_dmDownloadMgr = _downloadMgr;
            _m_udUpadateDelegate = _updateDelegate;

            _m_lPreUnzipSize = 0;
        }

        public void deal()
        {
            //获取对应处理对象的解压数据进度，并调整总处理的管理对象进度数据
            //获取当前解压字节数
            long curUnzipSize = _m_udUpadateDelegate.unzipSize;

            //调整管理对象总的总解压字节数
            _m_dmDownloadMgr.chgUnzipSize(curUnzipSize - _m_lPreUnzipSize);
            //设置更新原先的处理字节数
            _m_lPreUnzipSize = curUnzipSize;

            //判断是否已经超出总的需要处理字节数，是则不再监控
            if (_m_lPreUnzipSize >= _m_udUpadateDelegate.totalUnzipSize)
                return;

            //下帧继续监控
            ALMonoTaskMgr.instance.addNextFrameTask(this);
        }
    }
}
#endif
