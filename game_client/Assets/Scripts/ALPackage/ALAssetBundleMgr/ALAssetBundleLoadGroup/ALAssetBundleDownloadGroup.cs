using System.Collections.Generic;

using UnityEngine;

namespace ALPackage
{
    /// <summary>
    /// 资源加载集合管理对象，可以通过本对象对资源下载进度进行监控和处理
    /// </summary>
    public class ALAssetBundleDownloadGroup
    {
        /** 加载的resourceCore对象 */
        private _AALResourceCore _m_rcResCore;

        /** 本集合中需加载对象列表 */
        private List<ALAssetBundleDownloadGroupItemInfo> _m_dGroupItemInfoList;
        /** 需要加载的总资源大小 */
        private long _m_lTotalSize;

        public ALAssetBundleDownloadGroup(_AALResourceCore _resCore)
        {
            _m_rcResCore = _resCore;

            _m_dGroupItemInfoList = new List<ALAssetBundleDownloadGroupItemInfo>();
            _m_lTotalSize = 0;
        }

        /*****************
         * 添加一个需要加载统计对象
         **/
        public void addLoadItem(string _assetPath)
        {
            addLoadItem(_assetPath, true, false);
        }
        public void addLoadItem(string _assetPath, bool _useCache, bool _isNecessary)
        {
            //判断是否对象已在集合内
            if (null != _findItemInfo(_assetPath))
            {
                //重复带入，直接返回
                return;
            }

            //进行资源监测
            _m_rcResCore.checkLocalSrcAsset(_assetPath);

            //判断本地资源是否最新，如果最新则不做后续处理
            if (_m_rcResCore.isLocalAssetNewest(_assetPath))
                return ;

            //创建需要加载的信息对象
            ALAssetBundleDownloadGroupItemInfo info = new ALAssetBundleDownloadGroupItemInfo(_assetPath, _useCache, _isNecessary, _m_rcResCore);

            if (0 == info.totalSize)
            {
                //尺寸异常
                Debug.LogError("AssetBundle: " + _assetPath + " Version Info Error!");
                return;
            }

            //添加进入集合
            _m_dGroupItemInfoList.Add(info);
            //累计总大小
            _m_lTotalSize += info.totalSize;
        }

        /** 所有需要加载文件的大小 */
        public long totalSize
        {
            get { return _m_lTotalSize; }
        }

        /** 已经下载的内容大小 */
        public long loadedSize
        {
            get
            {
                long loadedSize = 0;
                for(int i = 0; i < _m_dGroupItemInfoList.Count; i++)
                {
                    loadedSize += _m_dGroupItemInfoList[i].loadedSize;
                }

                //返回总值
                return loadedSize;
            }
        }

        /** 失败数量 */
        public int failCount
        {
            get
            {
                int failCount = 0;
                for(int i = 0; i < _m_dGroupItemInfoList.Count; i++)
                {
                    if(!_m_dGroupItemInfoList[i].isSuc)
                    {
#if UNITY_EDITOR
                        UnityEngine.Debug.LogError($"Download {_m_dGroupItemInfoList[i].assetPath} Fail!");
#endif
                        failCount++;
                    }
                }

                //返回总值
                return failCount;
            }
        }

        /// <summary>
        /// 是否下载成功
        /// </summary>
        public bool isSucDone
        {
            get
            {
                for(int i = 0; i < _m_dGroupItemInfoList.Count; i++)
                {
                    if(!_m_dGroupItemInfoList[i].isSucDone)
                    {
                        return false;
                    }
                }

                //返回成功
                return true;
            }
        }

        /// <summary>
        /// 查找对应的文件下载信息
        /// </summary>
        /// <param name="_assetPath"></param>
        /// <returns></returns>
        protected ALAssetBundleDownloadGroupItemInfo _findItemInfo(string _assetPath)
        {
            if(null == _m_dGroupItemInfoList)
                return null;

            for(int i = 0; i < _m_dGroupItemInfoList.Count; i++)
            {
                if(_m_dGroupItemInfoList[i].assetPath.Equals(_assetPath, System.StringComparison.OrdinalIgnoreCase))
                    return _m_dGroupItemInfoList[i];
            }

            return null;
        }
    }
}
