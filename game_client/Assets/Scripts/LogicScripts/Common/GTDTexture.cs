using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 3D 用的 Texture 赋值器，把贴图赋值在对应 render 的第一个材质的 main texture 上
    /// </summary>
    public class GTDTexture
    {
        public Renderer _m_render;

        /** 保存的图集信息 */
        private int _m_iMainId;
        private int _m_iSubId;

        /** 需要设置的图片索引 */
        private int _m_iTargetMainId;
        private int _m_iTargetSubId;
        
        public GTDTexture(Renderer _renderer)
        {
            _m_render = _renderer;
            
            _m_iMainId = 0;
            _m_iSubId = 0;

            _m_iTargetMainId = 0;
            _m_iTargetSubId = 0;
        }

        /*******************
         * 释放图标相关的资源信息
         **/
        public void discardShowTexture()
        {
            //重置图集对象
            if (_m_render != null && _m_render.material != null)
            {
                _m_render.material.mainTexture = null;
            }

            //释放图集引用信息
            GGUITextureCacheMgr.instance.removeLoadTextureReq(_m_iMainId, _m_iSubId, _onTextureLoaded);

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
            GGUITextureCacheMgr.instance.removeLoadTextureReq(_m_iTargetMainId, _m_iTargetSubId, _onTextureLoaded);

            //重置索引
            _m_iTargetMainId = 0;
            _m_iTargetSubId = 0;
        }

        /*******************
         * 释放图标相关的资源信息
         **/
        public void discardTexture()
        {
            //重置图集对象
            if (_m_render != null && _m_render.material != null)
            {
                _m_render.material.mainTexture = null;
            }

            //释放图集引用信息
            GGUITextureCacheMgr.instance.removeLoadTextureReq(_m_iMainId, _m_iSubId, _onTextureLoaded);
            //释放加载目标图片资源
            GGUITextureCacheMgr.instance.removeLoadTextureReq(_m_iTargetMainId, _m_iTargetSubId, _onTextureLoaded);

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

            //判断当前图片是否一致，一致则不进行后续处理
            if (_m_iMainId == _textureIndex.mainId && _m_iSubId == _textureIndex.subId)
                return;

            //释放目标图片加载关联
            _discardTargetTexture();
            //设置新的图集信息
            _m_iTargetMainId = _textureIndex.mainId;
            _m_iTargetSubId = _textureIndex.subId;
            
            if (_m_render != null && _m_render.material != null)
                _m_render.material.mainTexture = PLoginCommonInfo.instance.obj.defaultTexture;

            //调用加载函数
            GGUITextureCacheMgr.instance.addTexture(_m_iTargetMainId, _m_iTargetSubId, _onTextureLoaded);
        }
        public void setTexture(int _iMainID, int _iSubID)
        {
            //判断当前图片是否一致，一致则不进行后续处理
            if (_m_iMainId == _iMainID && _m_iSubId == _iSubID)
                return;

            //释放目标图片加载关联
            _discardTargetTexture();
            //设置新的图集信息
            _m_iTargetMainId = _iMainID;
            _m_iTargetSubId = _iSubID;

            if (_m_render != null && _m_render.material != null)
                _m_render.material.mainTexture = PLoginCommonInfo.instance.obj.defaultTexture;

            //调用加载函数
            GGUITextureCacheMgr.instance.addTexture(_m_iTargetMainId, _m_iTargetSubId, _onTextureLoaded);
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

            if (_m_render != null && _m_render.material != null)
                _m_render.material.mainTexture = _textureResObj;
        }
    }
}