using System;
using System.Collections.Generic;

using UnityEngine;

/*****************
 * 通过远程下载资源的处理结果回调对象
 **/
namespace ALPackage
{
    public abstract class _AALWWWDownloadDelegate
    {
        /** 本回调对象对应的下载对象 */
        private ALWWWDownloadDealer _m_wdWWWDownloadDealer;

        public _AALWWWDownloadDelegate()
        {
            _m_wdWWWDownloadDealer = null;
        }

        /*****************
         * 设置处理完结
         **/
        public void setDealDone()
        {
            if (null != _m_wdWWWDownloadDealer)
                _m_wdWWWDownloadDealer._onDelegateDone();
            //清空关联变量
            _m_wdWWWDownloadDealer = null;
        }

        /*****************
         * 设置处理失败
         **/
        public void setDealFail()
        {
            if (null != _m_wdWWWDownloadDealer)
                _m_wdWWWDownloadDealer.retry();
            //清空关联变量
            _m_wdWWWDownloadDealer = null;
        }

        /******************
         * 设置本回调对象对应下载对象的关联
         **/
        protected internal void _setDownloadDealer(ALWWWDownloadDealer _dealer)
        {
            if (null != _m_wdWWWDownloadDealer)
            {
                Debug.LogError("Delegate Reg into different download dealer!");
            }

            _m_wdWWWDownloadDealer = _dealer;
        }

        /***************
         * 在资源下载完成时触发的回调处理
         **/
        protected abstract internal void _onWWWLoaded(AssetBundle _assetBundle);
    }
}
