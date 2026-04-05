using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

/*******************
 * UI中的图片加载管理对象，控制加载数量和速度，每帧最多加载一张
 **/
namespace ALPackage
{
    public abstract class _AALUITextureCacheMgr
    {
        //需要加载的图片信息对象队列
        private List<ALUITextureLoadInfo> _m_lNeedLoadTextureInfoList;
        //当前已经加载的所有图片对象的队列
        private Dictionary<long, ALUITextureLoadInfo> _m_dicAllTextureInfoDic;

        //当前正在加载的对象
        private ALUITextureLoadInfo _m_liLoadingInfo;

        protected _AALUITextureCacheMgr()
        {
            _m_lNeedLoadTextureInfoList = new List<ALUITextureLoadInfo>();
            _m_dicAllTextureInfoDic = new Dictionary<long, ALUITextureLoadInfo>();

            _m_liLoadingInfo = null;
        }

        /******************
         * 添加一个图片的加载需求
         **/
        public void addTexture(ALBasicResIndexInfo _indexInfo, Action<int, int, Texture> _delegate)
        {
            if (null == _indexInfo)
                return;

            addTexture(_indexInfo.mainId, _indexInfo.subId, _delegate);
        }
        public void addTexture(int _mainId, int _subId, Action<int, int, Texture> _delegate)
        {
            //判断是否需要开启加载，队列为空则需要开启
            bool needStartLoad = false;

            ALUITextureLoadInfo loadInfo = null;
            long mergeIdx = ALCommon.mergeInt(_mainId, _subId);
            //查询是否有对应信息
            if(!_m_dicAllTextureInfoDic.TryGetValue(mergeIdx, out loadInfo))
            {
                //无数据则创建后加入
                loadInfo = new ALUITextureLoadInfo(_mainId, _subId);

                //判断是否需要开启加载
                if (null == _m_liLoadingInfo && _m_lNeedLoadTextureInfoList.Count <= 0)
                    needStartLoad = true;

                //加入数据集
                _m_dicAllTextureInfoDic.Add(mergeIdx, loadInfo);
                _m_lNeedLoadTextureInfoList.Add(loadInfo);
            }

            //注册回调
            loadInfo.regDelegate(_delegate);

            //增加关联
            loadInfo.addRelateCount();

            //判断是否需要开启加载
            if (needStartLoad)
                popAndLoadTexture();
        }
        /** 移除加载图片请求 */
        public void removeLoadTextureReq(ALBasicResIndexInfo _indexInfo, Action<int, int, Texture> _delegate)
        {
            if (null == _indexInfo)
                return;

            removeLoadTextureReq(_indexInfo.mainId, _indexInfo.subId, _delegate);
        }
        public void removeLoadTextureReq(int _mainId, int _subId, Action<int, int, Texture> _delegate)
        {
            ALUITextureLoadInfo loadInfo = null;
            long mergeIdx = ALCommon.mergeInt(_mainId, _subId);
            //查询是否有对应信息
            if (!_m_dicAllTextureInfoDic.TryGetValue(mergeIdx, out loadInfo))
                return;

            //注销回调
            loadInfo.unregDelegate(_delegate);
            //删除关联
            loadInfo.reduceRelateCount();
        }

        /**************
         * 清理无效信息
         **/
        public void releaseUnusedTexture()
        {
            //清空所有未关联的资源信息
            for(int i = 0; i < _m_lNeedLoadTextureInfoList.Count; i++)
            {
                //判断数据是否有效
                if (null == _m_lNeedLoadTextureInfoList[i] || _m_lNeedLoadTextureInfoList[i].relateCount <= 0)
                {
                    //移除数据
                    if (_m_lNeedLoadTextureInfoList[i] != _m_liLoadingInfo)
                    {
                        _m_lNeedLoadTextureInfoList.RemoveAt(i);
                        continue;
                    }
                }
            }

            List<long> removeIdxList = new List<long>();
            //清空已加载数据中信息
            foreach(ALUITextureLoadInfo loadInfo in _m_dicAllTextureInfoDic.Values)
            {
                if (loadInfo.relateCount <= 0 && loadInfo != _m_liLoadingInfo)
                {
                    removeIdxList.Add(ALCommon.mergeInt(loadInfo.mainId, loadInfo.subId));
                    loadInfo.discard();
                }
            }

            //移除数据
            for(int i = 0; i < removeIdxList.Count; i++)
            {
                _m_dicAllTextureInfoDic.Remove(removeIdxList[i]);
            }
        }

        /**********************
         * 取出一个图片资源对象进行加载
         **/
        private string[] _g_arrExnameArr = new string[] { ".png", ".jpg" };
        public void popAndLoadTexture()
        {
            //取出队列第一个
            ALUITextureLoadInfo loadInfo = _popFirstLoadInfo();

            while (null != loadInfo)
            {
                //判断是否资源无效，是则不处理，并从主数据集删除
                if(loadInfo.relateCount <= 0)
                {
                    //移除数据
                    _removeFIrstLoadInfo();

                    //设置结束，并从集合中删除
                    loadInfo.setTextureLoadFail();
                    _m_dicAllTextureInfoDic.Remove(ALCommon.mergeInt(loadInfo.mainId, loadInfo.subId));

                    //取下一个数据
                    loadInfo = _popFirstLoadInfo();
                    continue;
                }

                //需要加载则开启加载
                _m_liLoadingInfo = loadInfo;
                //开启加载，在加载结果中继续之后的加载
#if UNITY_EDITOR
                //_resourceCore.loadAsset(_getLoadAssetPath(_m_liLoadingInfo.mainId, _m_liLoadingInfo.subId), _onAssetLoaded, null);
                ALLocalResLoaderMgr.instance.loadTemplateObjectAsset<Texture>(_getLoadAssetPath(_m_liLoadingInfo.mainId, _m_liLoadingInfo.subId)
                    , _getAssetObjName(_m_liLoadingInfo.mainId, _m_liLoadingInfo.subId), _g_arrExnameArr, "t:texture", _onAssetLoaded, null, _onAssetLoaded, _resourceCore);
#else
                _resourceCore.loadAsset(_getLoadAssetPath(_m_liLoadingInfo.mainId, _m_liLoadingInfo.subId), _onAssetLoaded, null);
#endif

                //跳出循环
                break;
            }
        }
        /*******************
         * 当前加载的处理
         **/
        protected void _onAssetLoaded(bool _isSuc, ALAssetBundleObj _assetObj)
        {
            //失败或无关联则直接不加载和处理
            if(!_isSuc)
            {
#if UNITY_EDITOR
                UnityEngine.Debug.LogError("Load Texture: " + _m_liLoadingInfo.mainId + " - " + _m_liLoadingInfo.subId + " Fail!");
#endif
                //失败则处理失败
                _m_liLoadingInfo.setTextureLoadFail();
                _m_liLoadingInfo = null;
                //直接删除
                _removeFIrstLoadInfo();

                //取出下一个尝试继续处理
                popAndLoadTexture();
                return;
            }

            //成功则加载对应图片后处理
            Texture texture = _assetObj.load<Texture>(_getAssetObjName(_m_liLoadingInfo.mainId, _m_liLoadingInfo.subId));
            _onAssetLoaded(texture);
            //_assetObj.loadAsyn(_getAssetObjName(_m_liLoadingInfo.mainId, _m_liLoadingInfo.subId), _onAsynAssetLoaded);
        }
        protected void _onAssetLoaded(Texture _texture)
        {
            if(null == _texture)
            {
                //失败则处理失败
                _m_liLoadingInfo.setTextureLoadFail();
                _m_liLoadingInfo = null;
                //直接删除
                _removeFIrstLoadInfo();

                //取出下一个尝试继续处理
                popAndLoadTexture();
                return;
            }

            //设置信息
            _m_liLoadingInfo.setTexture(_texture);
            _m_liLoadingInfo = null;
            //直接从加载队列删除
            _removeFIrstLoadInfo();

            //取出下一个尝试继续处理
            popAndLoadTexture();
            return;
        }
        protected void _onAsynAssetLoaded(UnityEngine.Object _obj)
        {
            if(!(_obj is Texture))
                return;

            Texture texture = (Texture)_obj;
            if(null == texture)
            {
                //失败则处理失败
                _m_liLoadingInfo.setTextureLoadFail();
                _m_liLoadingInfo = null;
                //直接删除
                _removeFIrstLoadInfo();

                //取出下一个尝试继续处理
                popAndLoadTexture();
                return;
            }

            //设置信息
            _m_liLoadingInfo.setTexture(texture);
            _m_liLoadingInfo = null;
            //直接从加载队列删除
            _removeFIrstLoadInfo();

            //取出下一个尝试继续处理
            popAndLoadTexture();
            return;
        }

        /** 取出第一个加载信息对象 */
        protected ALUITextureLoadInfo _popFirstLoadInfo()
        {
            if (_m_lNeedLoadTextureInfoList.Count <= 0)
                return null;

            //取出队列第一个
            ALUITextureLoadInfo loadInfo = _m_lNeedLoadTextureInfoList[0];

            return loadInfo;
        }
        protected void _removeFIrstLoadInfo()
        {
            if (_m_lNeedLoadTextureInfoList.Count <= 0)
                return;

            //取出队列第一个
            _m_lNeedLoadTextureInfoList.RemoveAt(0);
        }

        /**************
         * 获取用于加载资源的管理对象
         **/
        protected abstract _AALResourceCore _resourceCore { get; }
        /***************
         * 获取加载信息
         **/
        protected abstract string _getLoadAssetPath(int _mainId, int _subId);
        protected abstract string _getAssetObjName(int _mainId, int _subId);
    }
}
