using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

/*************
 * 基础的图片加载窗口对象
 **/
namespace ALPackage
{
    public abstract class _AALGuiWndBasicTexture
    {
        /** 本对象的操作序列号 */
        private int _m_iOpSerialize;

        /** 保存的图集信息 */
        private int _m_iMainId;
        private int _m_iSubId;

        /** 需要设置的图片索引 */
        private int _m_iTargetMainId;
        private int _m_iTargetSubId;

        public _AALGuiWndBasicTexture()
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
            if (_AALMonoMain.instance.showDebugOutput && ALSOGlobalSetting.Instance.logLevel <= ALLogLevel.VERBOSE)
            {
                UnityEngine.Debug.Log($"【{UnityEngine.Time.frameCount}】[UITexture][{this.GetType().Name}] discardShowTexture.");
            }
            //重置图集对象
            if (null != _getRawImage())
            {
                _getRawImage().texture = null;
            }

            //释放图集引用信息
            _getTextureCacheMgr().removeLoadTextureReq(_m_iMainId, _m_iSubId, _onTextureLoaded);

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
            _getTextureCacheMgr().removeLoadTextureReq(_m_iTargetMainId, _m_iTargetSubId, _onTextureLoaded);

            //重置索引
            _m_iTargetMainId = 0;
            _m_iTargetSubId = 0;
        }

        /*******************
         * 释放图标相关的资源信息
         **/
        public void discardTexture()
        {
            if (_AALMonoMain.instance.showDebugOutput && ALSOGlobalSetting.Instance.logLevel <= ALLogLevel.VERBOSE)
            {
                UnityEngine.Debug.Log($"【{UnityEngine.Time.frameCount}】[UITexture][{this.GetType().Name}] discardTexture.");
            }
            //调整操作序列号
            _m_iOpSerialize++;

            //重置图集对象
            if (null != _getRawImage())
            {
                _getRawImage().texture = null;
            }

            //释放图集引用信息
            _getTextureCacheMgr().removeLoadTextureReq(_m_iMainId, _m_iSubId, _onTextureLoaded);
            //释放加载目标图片资源
            _getTextureCacheMgr().removeLoadTextureReq(_m_iTargetMainId, _m_iTargetSubId, _onTextureLoaded);

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
            if (_AALMonoMain.instance.showDebugOutput && ALSOGlobalSetting.Instance.logLevel <= ALLogLevel.VERBOSE)
            {
                UnityEngine.Debug.Log($"【{UnityEngine.Time.frameCount}】[UITexture][{this.GetType().Name}] discard.");
            }
            //释放图集信息
            discardTexture();
            //重置材质
            disGray();
        }

        /***************
        * 根据指定ID设置窗口的图片
        **/
        public void setTexture(ALBasicResIndexInfo _textureIndex)
        {
            if (null == _textureIndex)
                return;

            //调整操作序列号
            _m_iOpSerialize++;

            //判断当前图片是否一致，一致则不进行后续处理
            if (_m_iMainId == _textureIndex.mainId && _m_iSubId == _textureIndex.subId)
                return;

            //释放目标图片加载关联
            _discardTargetTexture();
            //设置新的图集信息
            _m_iTargetMainId = _textureIndex.mainId;
            _m_iTargetSubId = _textureIndex.subId;

            if (null != _getRawImage() && null == _getRawImage().texture)
                _getRawImage().texture = _getDefaultTexture(); //WCGGameCommonInfo.instance.obj.defaultTexture;

            //调用加载函数
            _getTextureCacheMgr().addTexture(_m_iTargetMainId, _m_iTargetSubId, _onTextureLoaded);
            //_getTexResCore().loadObj(_m_iTargetMainId, _m_iTargetSubId, _onTextureLoaded + _doneDelegate);
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

            if (null != _getRawImage() && null == _getRawImage().texture)
                _getRawImage().texture = _getDefaultTexture(); //WCGGameCommonInfo.instance.obj.defaultTexture;

            //调用加载函数
            _getTextureCacheMgr().addTexture(_m_iTargetMainId, _m_iTargetSubId, _onTextureLoaded);
            //_getTexResCore().loadObj(_m_iTargetMainId, _m_iTargetSubId, _onTextureLoaded + _doneDelegate);
        }

        /*************
         * 设置本图片为灰色图片
         **/
        public void gray()
        {
            if (null == _getRawImage() || _getRawImage().material == _getGrayMat())//WCGGameCommonInfo.instance.obj.guiGrayMat)
                return;

            //设置为灰色材质
            _getRawImage().material = _getGrayMat();//WCGGameCommonInfo.instance.obj.guiGrayMat;
        }
        public void disGray()
        {
            if (null == _getRawImage() || _getRawImage().material == null)
                return;

            //设置为空材质
            _getRawImage().material = null;
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

            if (null != _getRawImage())
            {
                //设置图片
                if(null == _textureResObj)
                {
                    //数据无效则设置为失败图片
                    _getRawImage().texture = _getFailTexture();
                }
                else
                {
                    //数据有效则直接设置
                    _getRawImage().texture = _textureResObj;
                }
            }
        }

        /************
         * 获取对应图片显示的控件对象
         **/
        protected abstract RawImage _getRawImage();
        /// <summary>
        /// 设置默认图片
        /// </summary>
        /// <returns></returns>
        protected abstract Texture _getDefaultTexture();
        /// <summary>
        /// 当加载失败的时候显示的图片信息
        /// </summary>
        /// <returns></returns>
        protected abstract Texture _getFailTexture();
        protected abstract Material _getGrayMat();
        protected abstract _AALUITextureCacheMgr _getTextureCacheMgr();
    }
}

