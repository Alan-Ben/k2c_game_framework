using ALPackage;
using UnityEngine;

namespace GOE
{
    public abstract class _ATDSceneBasicTexture
    {
        /** 本对象的操作序列号 */
        private int _m_iOpSerialize;

        /** 保存的图集信息 */
        private int _m_iMainId;
        private int _m_iSubId;

        /** 需要设置的图片索引 */
        private int _m_iTargetMainId;
        private int _m_iTargetSubId;

        public _ATDSceneBasicTexture()
        {
            _m_iOpSerialize = 0;

            _m_iMainId = 0;
            _m_iSubId = 0;

            _m_iTargetMainId = 0;
            _m_iTargetSubId = 0;
        }

        public int opSerialize { get { return _m_iOpSerialize; } }

        /*******************
         * 释放图标相关的资源信息
         **/
        public void discardShowTexture()
        {
            //重置图集对象
            if (null != material)
            {
                material.mainTexture = null;
            }

            //释放图集引用信息
            textureCacheMgr.removeLoadTextureReq(_m_iMainId, _m_iSubId, _onTextureLoaded);

            //重置索引
            _m_iMainId = 0;
            _m_iSubId = 0;
        }
        //释放目标图片加载关联
        protected void _discardTargetTexture()
        {
            if (0 == _m_iTargetMainId && 0 == _m_iTargetSubId)
                return;

            //释放图集引用信息
            textureCacheMgr.removeLoadTextureReq(_m_iTargetMainId, _m_iTargetSubId, _onTextureLoaded);

            //重置索引
            _m_iTargetMainId = 0;
            _m_iTargetSubId = 0;
        }

        /*******************
         * 释放图标相关的资源信息
         **/
        public virtual void discardTexture()
        {
            //调整操作序列号
            _m_iOpSerialize++;

            //重置图集对象
            if (null != material)
            {
                material.mainTexture = null;
            }

            //释放图集引用信息
            textureCacheMgr.removeLoadTextureReq(_m_iMainId, _m_iSubId, _onTextureLoaded);
            //释放加载目标图片资源
            textureCacheMgr.removeLoadTextureReq(_m_iTargetMainId, _m_iTargetSubId, _onTextureLoaded);

            //重置索引
            _m_iMainId = 0;
            _m_iSubId = 0;
            _m_iTargetMainId = 0;
            _m_iTargetSubId = 0;
        }

        /********
         * 销毁实例化出来的对象
         * */
        public virtual void discard()
        {
            //释放图集信息
            discardTexture();
        }

        /***************
        * 根据指定ID设置窗口的图片
        **/
        public void setTexture(ALBasicResIndexInfo _textureIndex)
        {
            if (null == _textureIndex)
                return;

            setTexture(_textureIndex.mainId, _textureIndex.subId);
        }
        public void setTexture(int _iMainID, int _iSubID)
        {
            //调整操作序列号
            _m_iOpSerialize++;

            //判断当前图片是否一致，一致则不进行后续处理
            if (_m_iMainId == _iMainID && _m_iSubId == _iSubID)
                return;

            //释放目标图片加载关联
            _discardTargetTexture();
            //设置新的图集信息
            _m_iTargetMainId = _iMainID;
            _m_iTargetSubId = _iSubID;

            if (null != material && null == material.mainTexture)
                material.mainTexture = defaultTexture; //WCGGameCommonInfo.instance.obj.defaultTexture;

            //调用加载函数
            textureCacheMgr.addTexture(_m_iTargetMainId, _m_iTargetSubId, _onTextureLoaded);
            //_getTexResCore().loadObj(_m_iTargetMainId, _m_iTargetSubId, _onTextureLoaded);
        }

        
        /***************
         * 直接设置图片信息
         * */
        protected internal void _onTextureLoaded(int _mainId, int _subId, Texture _textureResObj)
        {
            //匹配索引信息
            if (_mainId != _m_iTargetMainId || _subId != _m_iTargetSubId)
            {
                return;
            }

            //释放当前图片信息
            discardShowTexture();
            //索引匹配则设置当前图片信息
            _m_iMainId = _m_iTargetMainId;
            _m_iSubId = _m_iTargetSubId;
            //重置目标信息
            _m_iTargetMainId = 0;
            _m_iTargetSubId = 0;
            
            if (null != material)
            {
                //设置图片
                material.mainTexture = _textureResObj;
            }
             
            //当图片为Null的时候加载提示No Res的图片
            if(_textureResObj == null)
                _loadFallbackTexture();
        }

        // 加载空图片的替代图片
        private void _loadFallbackTexture()
        {
            //设置替代贴图id
            _m_iTargetMainId = 999;
            _m_iTargetSubId = 2;
            textureCacheMgr.addTexture(_m_iTargetMainId, _m_iTargetSubId, (_mainId, _subId, _textureResObj) =>
            {
                //匹配索引信息
                if (_mainId != _m_iTargetMainId || _subId != _m_iTargetSubId)
                    return;
                
                //释放当前图片信息
                discardShowTexture();
                //索引匹配则设置当前图片信息
                _m_iMainId = _m_iTargetMainId;
                _m_iSubId = _m_iTargetSubId;
                //重置目标信息
                _m_iTargetMainId = 0;
                _m_iTargetSubId = 0;

                //设置图片
                if (null != material)
                    material.mainTexture = _textureResObj;
            });
        }

        /************
         * 获取对应图片显示的控件对象
         **/
        public abstract Material material { get; }
        protected abstract Texture defaultTexture{ get; }
        protected abstract _AUITextureCacheMgr textureCacheMgr { get; }
    }
}