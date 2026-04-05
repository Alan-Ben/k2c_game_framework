using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    public abstract class _ATAssetPathResCore<T> where T : UnityEngine.Object
    {
        /** 是否计数为0自动释放 */
        protected bool _m_bIsAutoRelease;
        /** 资源与索引id的映射表 */
        [NotNull][ItemNotNull] protected Dictionary<long, System.Object> _m_dLoadedObjInfoDic = new Dictionary<long, System.Object>();

        protected _ATAssetPathResCore()
        {
            _m_bIsAutoRelease = true;
        }
        protected _ATAssetPathResCore(bool _autoRelease = true)
        {
            _m_bIsAutoRelease = _autoRelease;
        }

        public bool isAutoRelease { get { return _m_bIsAutoRelease; } }

        /******************
         * 根据对应的索引加载对应的资源对象，并在回调中对具体资源进行处理
         **/
        public void loadObj(NPCommonAssetPathInfo _assetPath, Action<_ATAssetPathLoadedResInfo<T>> _delegate)
        {
            if (null == _assetPath || !_assetPath.enable)
            {
                _delegate?.Invoke(null);
                return;
            }

            long hashCode = _assetPath.GetHashCode();
            //检索是否有对应的数据，有则放入回调
            if (_m_dLoadedObjInfoDic.ContainsKey(hashCode))
            {
                _ATAssetPathLoadedResInfo<T> loadedInfo = (_ATAssetPathLoadedResInfo<T>)_m_dLoadedObjInfoDic[hashCode];
                loadedInfo.regDelegate(()=>
                {
                    _delegate?.Invoke(loadedInfo);
                });
            }
            else
            {
                //无对应数据则创建对应数据并开始加载
                _ATAssetPathLoadedResInfo<T> loadedInfo = _createLoadedObjInfo(_assetPath.asset_path, _assetPath.obj_name);
                //放入数据集合
                _m_dLoadedObjInfoDic.Add(hashCode, loadedInfo);
                //注册回调
                loadedInfo.regDelegate(()=>
                {
                    _delegate?.Invoke(loadedInfo);
                });

                //开启加载
                loadedInfo.loadRes();
            }
        }
        
        /******************
         * 强制释放对应资源信息的操作，带入对应资源对象是为了验证是否删除的资源是正确的资源
         **/
        public void discardRes(NPCommonAssetPathInfo _assetPath, _ATAssetPathLoadedResInfo<T> _info)
        {
            if (null == _assetPath)
                return;

            discardRes(_assetPath.asset_path, _assetPath.obj_name, _info);
        }
        public void discardRes(string _assetPath, string _objName, _ATAssetPathLoadedResInfo<T> _info)
        {
            //合并id，作为数据索引
            long hashCode = NPCommonAssetPathInfo.GetAssetPathHashCode(_assetPath, _objName);
            //判断数据是否存在
            if (!_m_dLoadedObjInfoDic.ContainsKey(hashCode))
                return;

            //处理释放操作
            _ATAssetPathLoadedResInfo<T> loadedInfo = (_ATAssetPathLoadedResInfo<T>)_m_dLoadedObjInfoDic[hashCode];
            //检查当前资源是否处理释放操作的资源
            if (null != _info && loadedInfo != _info)
                return;

            //删除映射表
            _m_dLoadedObjInfoDic.Remove(hashCode);
            //释放资源
            loadedInfo.discard();
        }

        /// <summary>
        /// 尝试遍历所有资源，将无引用资源释放
        /// </summary>
        public void tryDiscardAllDisableRes()
        {
            List<_ATAssetPathLoadedResInfo<T>> removeList = new List<_ATAssetPathLoadedResInfo<T>>();
            foreach(_ATAssetPathLoadedResInfo<T> objInfo in _m_dLoadedObjInfoDic.Values)
            {
                if(null == objInfo)
                    continue;

                //先添加到队列
                if(objInfo.useCount <= 0)
                    removeList.Add(objInfo);
            }

            //逐个删除
            _ATAssetPathLoadedResInfo<T> tmpInfo = null;
            for(int i = 0; i < removeList.Count; i++)
            {
                tmpInfo = removeList[i];
                if(null == tmpInfo)
                    continue;

                //移除数据
                discardRes(tmpInfo.assetPath, tmpInfo.objName, tmpInfo);
            }
        }

        /***************
         * 创建一个载入资源信息的对象
         **/
        [NotNull] protected abstract _ATAssetPathLoadedResInfo<T> _createLoadedObjInfo(string _assetPath, string _objName);
    }
}