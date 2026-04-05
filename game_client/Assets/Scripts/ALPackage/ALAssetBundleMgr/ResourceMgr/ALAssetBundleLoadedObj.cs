using System.Collections.Generic;

using UnityEngine;

namespace ALPackage
{
    /****************
     * 下载的AssetBundle中已经载入内存的数据对象
     **/
    public class ALAssetBundleLoadedObj
    {
        /** 具体对象 */
        private ALAssetBundleObj _m_obj;
        /** 最后一次访问的时间标记 */
        private long _m_lLastViewTime;

        public ALAssetBundleLoadedObj(ALAssetBundleObj _obj)
        {
            _m_obj = _obj;
            _m_lLastViewTime = -1;
        }

        public ALAssetBundleObj obj { get { return _m_obj; } }
        public long lastViewTime { get { return _m_lLastViewTime; } }

        /******************
         * 刷新最后访问的时间
         * 仅仅提供给控制对象访问
         **/
        protected internal void _refreshLastViewTime()
        {
            //进行强制转化
            _m_lLastViewTime = ALAssetBundleLoadedMgr.getTimeTag();
        }
        /****************
         * 重置使用时间标记
         **/
        protected internal void _resetLastViewTime()
        {
            //进行强制转化
            _m_lLastViewTime = 0;
        }

        /*******************
         * 释放资源操作
         **/
        protected internal void _release()
        {
            if (null == _m_obj)
                return;

            //重置最后浏览时间
            _m_lLastViewTime = -1;
            //释放资源对象
            _m_obj._unload();
            //设置对象为空
            _m_obj = null;
        }
    }
}
