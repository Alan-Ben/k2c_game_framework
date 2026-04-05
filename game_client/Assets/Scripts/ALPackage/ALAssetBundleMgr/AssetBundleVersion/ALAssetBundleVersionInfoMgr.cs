using System;
using System.Collections.Generic;

using UnityEngine;

namespace ALPackage
{
    public class ALAssetBundleVersionInfoMgr
    {
        /** 存储资源相对路径与版本信息的映射关系 */
        private Dictionary<string, ALAssetBundleVersionInfo> _m_dVersionInfoDic;
        /** 是否完成了初始化 */
        private bool _m_bInit;
        /** 初始化版本管理对象的触发回调函数 */
        private Action _m_dInitedDelegate;

        public ALAssetBundleVersionInfoMgr()
        {
            _m_dVersionInfoDic = new Dictionary<string, ALAssetBundleVersionInfo>();
            _m_bInit = false;
            _m_dInitedDelegate = null;
        }

        public bool isInit
        {
            get { return _m_bInit; }
        }
        
        /// <summary>
        /// 是否有资源
        /// </summary>
        public bool hasResource
        {
            get { return _m_dVersionInfoDic != null && _m_dVersionInfoDic.Count > 0; }
        }

        /****************
         * 将加载的版本信息初始化
         **/
        public void initVersionDic(ALSOAssetBundleVersionSet _versionSet)
        {
            if (null == _versionSet)
                return;

            initVersionDic(_versionSet.versionInfoList);
        }
        public void initVersionDic(List<ALAssetBundleVersionInfo> _versionInfoList)
        {
            if (_m_bInit)
                return;

            _m_bInit = true;

            if (null == _versionInfoList) 
            {
                //触发回调
                if (null != _m_dInitedDelegate)
                    _m_dInitedDelegate();
                _m_dInitedDelegate = null;

                return;
            }

            for (int i = 0; i < _versionInfoList.Count; i++)
            {
                ALAssetBundleVersionInfo versionInfo = _versionInfoList[i];

                if (null == versionInfo)
                    continue;

                string path = versionInfo.assetPath.ToLowerInvariant();

                if (_m_dVersionInfoDic.ContainsKey(path))
                {
                    //已经存在索引表明有重复索引
                    Debug.LogError("Multi Index of Asset: " + path);
                    continue;
                }

                //添加进数据集
                _m_dVersionInfoDic.Add(path, versionInfo);
            }

            //触发回调
            if (null != _m_dInitedDelegate)
                _m_dInitedDelegate();
            _m_dInitedDelegate = null;
        }

        /******************
         * 添加一个单独对象的版本信息到本对象中
         **/
        public void addVersionInfo(ALAssetBundleVersionInfo _versionInfo)
        {
            if (null == _versionInfo)
                return;

            //判断是否已经初始化，未初始化调用本函数为错误使用
            if(!_m_bInit)
            {
                Debug.LogError("Add Version Info when the version manager is not inited!");
                return;
            }

            string path = _versionInfo.assetPath.ToLowerInvariant();

            if (_m_dVersionInfoDic.ContainsKey(path))
            {
                //已经存在索引表明有重复索引
                Debug.LogError("Multi Index when add version info! Asset: " + path);
                return;
            }

            //添加进数据集
            _m_dVersionInfoDic.Add(path, _versionInfo);
        }

        /*******************
         * 注册初始化完成后的回调操作函数
         **/
        public void regInitedDelegate(Action _delegate)
        {
            if (null == _delegate)
                return;

            if (_m_bInit)
            {
                _delegate();
            }
            else
            {
                if (null == _m_dInitedDelegate)
                    _m_dInitedDelegate = _delegate;
                else
                    _m_dInitedDelegate += _delegate;
            }
        }

        /****************
         * 获取下载资源的相关信息
         **/
        public ALAssetBundleVersionInfo getVersionInfo(string _assetPath)
        {
            if (null == _assetPath)
                return null;

            string path = _assetPath.ToLowerInvariant();

            if (!_m_dVersionInfoDic.ContainsKey(path))
                return null;

            return _m_dVersionInfoDic[path];
        }

        /****************
         * 获取所有资源对象信息
         **/
        public List<ALAssetBundleVersionInfo> getAllVersionInfo()
        {
            List<ALAssetBundleVersionInfo> list = new List<ALAssetBundleVersionInfo>();
            list.AddRange(_m_dVersionInfoDic.Values);

            return list;
        }
        public void getAllVersionInfo(List<ALAssetBundleVersionInfo> _recList)
        {
            if (null == _recList)
                return;

            _recList.Clear();
            _recList.AddRange(_m_dVersionInfoDic.Values);
        }
    }
}
