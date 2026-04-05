
using UnityEngine;
using ALPackage;
using System;

namespace GOE
{
    //加载 ui_res_path 资源的加载器
    public class CommonAssetLoader
    {
        private long _m_assetPathId; // 资源的路径信息
        private Transform _m_parent;                // 资源加载之后的父节点
        private GameObject _m_gShowGo;              // 加载成GO之后的资源

        //加载处理序列号
        private long _m_lLoadSerialize;

        public long assetPathId { get { return _m_assetPathId; } }
        public Transform parent { get { return _m_parent; } }
        public GameObject ShowGO { get { return _m_gShowGo; } }

        public CommonAssetLoader()
        {

        }

        public void loadAsset(long _assetPathId, Transform _parent, Action<GameObject> _doneAction = null)
        {
            //清空上一个数据
            _releaseGo();

            //刷新序列号
            _m_lLoadSerialize = ALSerializeOpMgr.next();

            //判断序列号是否有效，无效则返回
            if (0 == _assetPathId)
            {
                if (null != _doneAction)
                    _doneAction(null);

                return;
            }

            //存储临时序列号
            long curSerialize = _m_lLoadSerialize;
            string assetPath = UIResPathAssistant.getAssetPath(_assetPathId);
            string objName = UIResPathAssistant.getObjName(_assetPathId);
            if(string.IsNullOrEmpty(assetPath) || string.IsNullOrEmpty(objName))
            {
                Debug.LogError("error  : ", _parent);
                if (null != _doneAction)
                    _doneAction(null);

                return;
            }

            //设置加载数据
            _m_assetPathId = _assetPathId;
            _m_parent = _parent;

            ALAssetLoader<GameObject> loader = new ALAssetLoader<GameObject>(GameResCore.instance, assetPath, objName);
            loader.loadAsset(
                (GameObject _go)=> 
                {
                    if (_go == null)
                    {
                        Debug.LogError("load assetPathId : " + _m_assetPathId + " error!" );
                        if (null != _doneAction)
                            _doneAction(null);

                        return;
                    }
                    //判断序列号
                    if(curSerialize != _m_lLoadSerialize)
                    {
                        //直接放弃处理，由于对象是AB加载的，这里不能release
                        if (null != _doneAction)
                            _doneAction(null);

                        return;
                    }
                    _m_gShowGo = GameObject.Instantiate(_go) as GameObject;
                    if (_m_gShowGo != null)
                    {
                        _m_gShowGo.transform.SetParent(_m_parent);
                        _m_gShowGo.transform.localPosition = Vector3.zero;
                        _m_gShowGo.transform.localScale = Vector3.one;
                    }

                    if (null != _doneAction)
                        _doneAction(_m_gShowGo);
                });
        }

        public void discard()
        {
            _releaseGo();
        }

        /// <summary>
        /// 释放Go对象
        /// </summary>
        protected void _releaseGo()
        {
            ALUnityCommon.releaseGameObj(_m_gShowGo);
            _m_parent = null;
            _m_gShowGo = null;
            _m_assetPathId = 0;
        }
    }
}
