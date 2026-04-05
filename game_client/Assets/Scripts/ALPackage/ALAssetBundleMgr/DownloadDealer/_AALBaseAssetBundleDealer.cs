using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace ALPackage
{
    /*********************
     * 进行具体WWW加载的Coroutine执行任务对象
     **/
    public abstract class _AALBaseAssetBundleDealer
    {
        /** 加载的相关信息 */
        private ALLoadingAssetInfo _m_sLoadingInfo;

        public _AALBaseAssetBundleDealer(ALLoadingAssetInfo _info)
        {
            _m_sLoadingInfo = _info;
        }

        public ALLoadingAssetInfo loadingAssetInfo { get { return _m_sLoadingInfo; } }

        /****************
         * 开启任务执行
         **/
        public abstract void startLoad();
    }
}
