using System;
using System.Collections.Generic;

using UnityEngine;

namespace ALPackage
{
    public class ALAssetBundleResourceMgr
    {
        /** 加载URL对应的资源加载对象 */
        private Dictionary<string, ALAssetBundleObj> _m_dAssetBundleDic;

        public ALAssetBundleResourceMgr()
        {
            _m_dAssetBundleDic = new Dictionary<string, ALAssetBundleObj>();
        }

        /******************
         * 向已经加载完成的集合中添加资源对象
         **/
        public ALAssetBundleObj addLoadedAssetBundle(string _idx, AssetBundle _assetBundle, bool _isWWWAssetBundle)
        {
            string idxStr = _idx.ToLowerInvariant();

            if (_m_dAssetBundleDic.ContainsKey(idxStr))
            {
                //重复索引报错
                Debug.LogError("Multiple AssetBundle: " + _idx);
                return null;
            }

            ALAssetBundleObj assetObj = new ALAssetBundleObj(this, _idx, _assetBundle, _isWWWAssetBundle);
            //添加到集合中
            _m_dAssetBundleDic.Add(idxStr, assetObj);

            //刷新对象的访问时间
            ALAssetBundleLoadedMgr.instance.refreshLoadedObj(assetObj.loadedInfoObj);

            return assetObj;
        }

        /**********************
         * 从集合中检索指定索引的资源对象
         **/
        public ALAssetBundleObj getLoadedAssetBundle(string _idx)
        {
            string idxStr = _idx.ToLowerInvariant();

            if (!_m_dAssetBundleDic.ContainsKey(idxStr))
                return null;

            return _m_dAssetBundleDic[idxStr];
        }

        /**********************
         * 从集合删除指定资源集合
         **/
        public void unregAssetBundle(string _idx)
        {
            string idxStr = _idx.ToLowerInvariant();

            if (!_m_dAssetBundleDic.ContainsKey(idxStr))
                return;

            _m_dAssetBundleDic.Remove(idxStr);
        }
    }
}
