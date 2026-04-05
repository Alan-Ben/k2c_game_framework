using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

/*********************
 * 在等待加载的UI加载对象
 **/
namespace ALPackage
{
    public class ALUITextureLoadInfo
    {
        //资源索引信息
        private int _m_iMainId;
        private int _m_iSubId;
        //对应需要处理的回调函数
        private Action<int, int, Texture> _m_aLoadedDelegate;

        //对应的资源对象
        private Texture _m_resObj;

        //是否已经加载完成
        private bool _m_bIsLoaded;

        //本延迟加载图片资源的关联数量
        private int _m_iRelateCount;

        public ALUITextureLoadInfo(ALBasicResIndexInfo _indexInfo)
        {
            _m_iMainId = _indexInfo.mainId;
            _m_iSubId = _indexInfo.subId;
            _m_aLoadedDelegate = default(Action<int, int, Texture>);

            _m_resObj = null;
            _m_bIsLoaded = false;

            _m_iRelateCount = 0;
        }
        public ALUITextureLoadInfo(int _mainId, int _subId)
        {
            _m_iMainId = _mainId;
            _m_iSubId = _subId;
            _m_aLoadedDelegate = default(Action<int, int, Texture>);

            _m_resObj = null;
            _m_bIsLoaded = false;

            _m_iRelateCount = 0;
        }

        public int mainId { get { return _m_iMainId; } }
        public int subId { get { return _m_iSubId; } }
        public Texture texture { get { return _m_resObj; } }

        public int relateCount { get { return _m_iRelateCount; } }

        public void addRelateCount() { _m_iRelateCount ++; }
        public void reduceRelateCount() { _m_iRelateCount--; }

        /** 注册回调 */
        public void regDelegate(Action<int, int, Texture> _delegate)
        {
            if (null == _delegate)
                return;

            //已经加载则直接处理
            if (_m_bIsLoaded)
                _delegate(_m_iMainId, _m_iSubId, texture);

            _m_aLoadedDelegate += _delegate;
        }
        public void unregDelegate(Action<int, int, Texture> _delegate)
        {
            if (null == _delegate)
                return;

            _m_aLoadedDelegate -= _delegate;
        }

        /*****************
         * 全部失败处理
         **/
        public void discard()
        {
            _m_iMainId = 0;
            _m_iSubId = 0;

            _m_bIsLoaded = true;
            _m_resObj = null;

            _m_aLoadedDelegate = null;

            _m_iRelateCount = 0;
        }

        /*****************
         * 设置图片资源对象
         **/
        public void setTexture(Texture _textureObj)
        {
            if (_m_bIsLoaded)
                return;

            //设置资源对象
            _m_resObj = _textureObj;

            //设置加载完成
            _m_bIsLoaded = true;

            //处理回调
            if(null != _m_aLoadedDelegate)
                _m_aLoadedDelegate(_m_iMainId, _m_iSubId, texture);
            //重置回调对象
            _m_aLoadedDelegate = default(Action<int, int, Texture>);
        }

        /*****************
         * 设置图片加载失败
         **/
        public void setTextureLoadFail()
        {
            //设置资源对象
            _m_resObj = null;

            //设置加载完成
            _m_bIsLoaded = true;

            //处理回调
            if (null != _m_aLoadedDelegate)
                _m_aLoadedDelegate(_m_iMainId, _m_iSubId, null);
            //重置回调对象
            _m_aLoadedDelegate = default(Action<int, int, Texture>);
        }

        public override string ToString()
        {
            string texName = texture != null ? texture.name : "null";
            return $"[Texture][{mainId}:{subId}:{texName}][{_m_bIsLoaded}:{_m_iRelateCount}]";
        }
    }
}
