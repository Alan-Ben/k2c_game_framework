using UnityEngine;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 特效粒子管理对象，管理mono及简单的窗口处理
    /// </summary>
    public class ParticleCacheItem
    {
        //粒子对象Mono
        private NPParticleMono _m_mPraticleMono;

        //控制显示图片的对象
        private GGuiWndSprite _m_wIconWnd;

        //道具窗口
        private GGUIWndCommonSimpleItem _m_wSimpleItemWnd;

        /// <summary> 粒子对象Mono </summary>
        public NPParticleMono mono { get { return _m_mPraticleMono; } }

        public ParticleCacheItem(NPParticleMono _mono)
        {
            _m_mPraticleMono = _mono;
            
            if (null != _mono.imgIcon)
                _m_wIconWnd = new GGuiWndSprite(_mono.imgIcon);

            if (null != _mono.monoSimpleItem)
                _m_wSimpleItemWnd = new GGUIWndCommonSimpleItem(_mono.monoSimpleItem);
        }


        #region 外部管理

        /// <summary>
        /// 显示
        /// </summary>
        public void showItem()
        {
            if (null == _m_mPraticleMono || null == _m_mPraticleMono.transform)
                return;

            //初始化缩放
            _m_mPraticleMono.transform.localPosition = Vector3.zero;
            _m_mPraticleMono.transform.localScale = Vector3.one;

            //显示图片
            if (null != _m_wIconWnd)
                _m_wIconWnd.showWnd();

            //设置对象有效
            ALUGUICommon.setGameObjEnable(_m_mPraticleMono.gameObject);
        }

        /// <summary>
        /// 重置信息 
        /// </summary>
        public void reset()
        {
            //重置图片
            if (null != _m_wIconWnd)
                _m_wIconWnd.discardTexture();

            if (null != _m_wSimpleItemWnd)
                _m_wSimpleItemWnd.resetWnd();

            //设置本对象无效
            if (null != _m_mPraticleMono)
                ALUGUICommon.setGameObjDisable(_m_mPraticleMono.gameObject);

            //恢复透明度
            setAlpha(1);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void discard()
        {
            if (null != _m_wIconWnd)
                _m_wIconWnd.discard();
            _m_wIconWnd = null;

            if (null != _m_wSimpleItemWnd)
                _m_wSimpleItemWnd.discard();
            _m_wSimpleItemWnd = null;

            _m_mPraticleMono = null;
        }

        #endregion


        #region 功能方法

        /// <summary>
        /// 初始化item
        /// </summary>
        /// <param name="_parentTrans">粒子父节点</param>
        public void initItem(Transform _parentTrans)
        {
            if (null == _m_mPraticleMono || null == _m_mPraticleMono.transform)
                return;

            //设置父节点
            _m_mPraticleMono.transform.SetParent(_parentTrans);

            //设置缩放为0
            _m_mPraticleMono.transform.localPosition = Vector3.zero;
            _m_mPraticleMono.transform.localScale = Vector3.zero;

            //设置对象无效
            ALUGUICommon.setGameObjDisable(_m_mPraticleMono.gameObject);
        }

        /// <summary>
        /// 设置默认图标
        /// </summary>
        /// <param name="_textureIndex">图片资源下标</param>
        public void setDefaultIcon(NPGSpriteIndex _textureIndex)
        {
            //设置图片信息
            if (null != _m_wIconWnd)
                _m_wIconWnd.setTexture(_textureIndex);
        }

        /// <summary>
        /// 设置道具信息
        /// </summary>
        /// <param name="_iconIndex"></param>
        /// <param name="_bgIndex"></param>
        /// <param name="_count"></param>
        /// <param name="_isShowCount"></param>
        public void setBagItemInfo(NPGTextureIndex _iconIndex, NPGSpriteIndex _bgIndex, long _count, bool _isShowCount = true)
        {
            if (_m_wSimpleItemWnd != null)
            {
                _m_wSimpleItemWnd.showWnd();
                _m_wSimpleItemWnd.setItem(_iconIndex, _bgIndex, _count, _isShowCount);
            }
        }

        //设置透明度
        public void setAlpha(float _alpha)
        {
            if (null != _m_mPraticleMono)
                _m_mPraticleMono.setAlpha(_alpha);
        }

        #endregion
    }
}

