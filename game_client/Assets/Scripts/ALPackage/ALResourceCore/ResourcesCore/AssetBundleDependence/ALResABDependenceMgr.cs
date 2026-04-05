using System.Collections.Generic;
using UnityEngine;

namespace ALPackage
{
    /// <summary>
    /// 各资源打包文件互相依赖的信息
    /// </summary>
    public class ALResABDependenceMgr
    {
        private Dictionary<string, ALResABDependenceInfo> _m_dicABDependenceDic;

        public ALResABDependenceMgr()
        {
            _m_dicABDependenceDic = new Dictionary<string, ALResABDependenceInfo>();
        }

        //初始化数据集
        public void init(ALSOAssetBundleDependenceSet _set)
        {
            ALAssetBundleDependenceInfo tmpInfo = null;
            for(int i = 0; i < _set.dependenceInfoList.Count; i++)
            {
                tmpInfo = _set.dependenceInfoList[i];
                if(null == tmpInfo)
                    continue;

                ALResABDependenceInfo newInfo = new ALResABDependenceInfo(tmpInfo);
                _m_dicABDependenceDic.Add(tmpInfo.assetPath, newInfo);
            }
        }

        /// <summary>
        /// 获取对应asset的依赖加载队列
        /// </summary>
        /// <returns></returns>
        public ALResABDependenceInfo getABDependence(string _asset)
        {
            if(null == _asset)
                return null;

            ALResABDependenceInfo info;
            if(!_m_dicABDependenceDic.TryGetValue(_asset, out info))
                return null;

            return info;
        }
    }
}