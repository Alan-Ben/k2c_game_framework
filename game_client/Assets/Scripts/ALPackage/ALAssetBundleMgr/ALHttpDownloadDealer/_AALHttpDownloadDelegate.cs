using System;
using System.Collections.Generic;

using UnityEngine;

/*****************
 * 通过远程下载资源的处理结果回调对象
 **/
namespace ALPackage
{
    public abstract class _AALHttpDownloadDelegate
    {
        /** 本回调对象对应的下载对象 */
        private ALHttpDownloadDealer _m_wdHttpDownloadDealer;

        public _AALHttpDownloadDelegate()
        {
            _m_wdHttpDownloadDealer = null;
        }

        /*****************
         * 设置处理完结
         **/
        protected void _setDealDone()
        {
            if (null != _m_wdHttpDownloadDealer)
                _m_wdHttpDownloadDealer._onDelegateDone();
            //清空关联变量
            _m_wdHttpDownloadDealer = null;
        }

        protected void _setDealFail()
        {
            if(null != _m_wdHttpDownloadDealer)
                _m_wdHttpDownloadDealer._onDelegateFail();
            //清空关联变量
            _m_wdHttpDownloadDealer = null;
        }

        /******************
         * 设置本回调对象对应下载对象的关联
         **/
        protected internal void _setDownloadDealer(ALHttpDownloadDealer _dealer)
        {
            if (null != _m_wdHttpDownloadDealer)
            {
                Debug.LogError("Delegate Reg into different download dealer!");
            }

            _m_wdHttpDownloadDealer = _dealer;
        }

        /***************
         * 在资源下载完成时触发的回调处理
         **/
        protected abstract internal void _onHttpLoaded(string _localPath);
        /***************
         * 在资源下载失败时触发的回调处理
         **/
        protected abstract internal void _onHttpFail();
    }
}
