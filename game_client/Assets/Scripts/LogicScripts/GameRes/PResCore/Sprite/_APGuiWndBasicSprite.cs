using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /*************
     * 基础的图片加载窗口对象
     **/
    public abstract class _APGuiWndBasicSprite
    {
        /** 本对象的操作序列号 */
        private int _m_iOpSerialize;

        /** 保存的图集信息 */
        private int _m_iMainId;
        private int _m_iSubId;

        /** 需要设置的图片索引 */
        private int _m_iTargetMainId;
        private int _m_iTargetSubId;

        /** 当前资源对象 */
        private WCGPSpriteResObj _m_roSpriteResObj;
        /** 资源管理对象 */
        private ALResObjSingleContainer _m_scSingleResContainer;

        public _APGuiWndBasicSprite()
        {
            _m_iOpSerialize = 0;

            _m_iMainId = 0;
            _m_iSubId = 0;

            _m_iTargetMainId = 0;
            _m_iTargetSubId = 0;

            _m_roSpriteResObj = null;
            _m_scSingleResContainer = new ALResObjSingleContainer();
        }

        public int opSerialize { get { return _m_iOpSerialize; } }

        /*******************
         * 释放图标相关的资源信息
         **/
        public void discardShowTexture()
        {
            //重置图集对象
            if(null != _getImage())
            {
                _getImage().sprite = null;
            }

            //释放图集引用信息
            if(null != _m_roSpriteResObj)
                _m_roSpriteResObj.discard();

            //重置索引
            _m_iMainId = 0;
            _m_iSubId = 0;
        }

        /*******************
         * 释放图标相关的资源信息
         **/
        public void discardTexture()
        {
            //调整操作序列号
            _m_iOpSerialize++;

            //重置图集对象
            if(null != _getImage())
            {
                _getImage().sprite = null;
            }

            //释放图集引用信息
            if(null != _m_roSpriteResObj)
                _m_roSpriteResObj.discard();
            //释放资源容器对象
            _m_scSingleResContainer.discard();

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
            //重置材质
            disGray();
        }

        /***************
        * 根据指定ID设置窗口的图片
        **/
        public void setTexture(NPPSpriteIndex _spriteIndex, Action<_ATALObjResObj<Sprite>> _doneDelegate = null)
        {
            if(null == _spriteIndex)
                return;

            //调整操作序列号
            _m_iOpSerialize++;

            //判断当前图片是否一致，一致则不进行后续处理
            if(_m_iMainId == _spriteIndex.mainId && _m_iSubId == _spriteIndex.subId)
                return;

            //设置新的图集信息
            _m_iTargetMainId = _spriteIndex.mainId;
            _m_iTargetSubId = _spriteIndex.subId;
            //调用加载函数
            PSpriteResCore.instance.loadObj(_m_iTargetMainId, _m_iTargetSubId, _onSpriteLoaded + _doneDelegate);
        }
        public void setTexture(int _iMainID, int _iSubID, Action<_ATALObjResObj<Sprite>> _doneDelegate = null)
        {
            //调整操作序列号
            _m_iOpSerialize++;

            //判断当前图片是否一致，一致则不进行后续处理
            if(_m_iMainId == _iMainID && _m_iSubId == _iSubID)
                return;

            //设置新的图集信息
            _m_iTargetMainId = _iMainID;
            _m_iTargetSubId = _iSubID;
            //调用加载函数
            PSpriteResCore.instance.loadObj(_m_iTargetMainId, _m_iTargetSubId, _onSpriteLoaded + _doneDelegate);
        }

        /*************
         * 设置本图片为灰色图片
         **/
        public void gray()
        {
            if(null == _getImage() || _getImage().material == GGameCommonInfo.instance.obj.guiGrayMat)
                return;

            //设置为灰色材质
            _getImage().material = GGameCommonInfo.instance.obj.guiGrayMat;
        }
        public void disGray()
        {
            if(null == _getImage() || _getImage().material == null)
                return;

            //设置为空材质
            _getImage().material = null;
        }

        /***************
         * 直接设置图片信息
         * */
        protected internal void _onSpriteLoaded(_ATALObjResObj<Sprite> _spriteResObj)
        {
            //匹配索引信息
            if(_spriteResObj.mainId != _m_iTargetMainId || _spriteResObj.subId != _m_iTargetSubId)
            {
                _spriteResObj.discard();
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

            //设置资源对象
            _m_roSpriteResObj = (WCGPSpriteResObj)_spriteResObj;

            if(null != _getImage())
            {
                //设置图片
                _getImage().sprite = _spriteResObj.createObj(_m_scSingleResContainer);
            }
        }

        /************
         * 获取对应图片显示的控件对象
         **/
        protected abstract Image _getImage();
    }
}
