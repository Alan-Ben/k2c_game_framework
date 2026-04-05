using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    public class PlaySfxMgr
    {
        private static PlaySfxMgr _g_instance = new PlaySfxMgr();
        public static PlaySfxMgr instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new PlaySfxMgr();
                return _g_instance;
            }
        }

        public PlaySfxMgr()
        {
        }

        /// <summary>
        /// 通用播放ui特效接口
        /// </summary>
        public CommonUISfxObj playUISfx(long _sfxRefId, Transform _parent)
        {
            return playSfx<CommonUISfxObj, NPSfxMono>(_sfxRefId, _parent);
        }
        
        /// <summary>
        /// 通用播放场景特效接口
        /// </summary>
        public CommonTDSfxObj playTDSfx(long _sfxRefId, Transform _parent)
        {
            return playSfx<CommonTDSfxObj, NPSfxMono>(_sfxRefId, _parent);
        }
        
        public T playSfx<T, Y>(long _sfxRefId, Transform _parent)
            where T : _BaseSfxObj<Y>, new() where Y : NPSfxMono
        {
            NPSfxRefObj sfxRefObj = GRefdataCoreMgr.instance.sfxMap.getRef(_sfxRefId);
            return playSfx<T, Y>(sfxRefObj, _parent);
        }
        
        /// <summary>
        /// 播放特效，需要一个父节点
        /// </summary>
        public T playSfx<T, Y>(NPSfxRefObj _sfxRefObj, Transform _parent)
            where T : _BaseSfxObj<Y>, new() where Y : NPSfxMono
        {
            if (_sfxRefObj == null || _parent == null)
                return null;
            
            //构造特效对象结构体
            T sfxObj = new T();
            sfxObj.init(_sfxRefObj);
            sfxObj.setParent(_parent);
            sfxObj.setLocalScale(Vector3.one);
            sfxObj.setLocalPos(Vector3.zero);
            sfxObj.setLocalRotation(Quaternion.identity);
            sfxObj.setLayer(_parent.gameObject.layer);

            //开启加载播放处理
            sfxObj.playSfxObj();
            
            //返回结构体
            return sfxObj;
        }

        public CommonTDSfxObj playSfxByPos(long _sfxRefId, Vector3 _pos)
        {
            NPSfxRefObj sfxRefObj = GRefdataCoreMgr.instance.sfxMap.getRef(_sfxRefId);
            return playSfxByPos<CommonTDSfxObj, NPSfxMono>(sfxRefObj, _pos);
        }
        
        /// <summary>
        /// 直接在某个位置播放特效的接口，兼容旧版本
        /// </summary>
        public T playSfxByPos<T, Y>(NPSfxRefObj _sfxRefObj, Vector3 _pos)
            where T : _BaseSfxObj<Y>, new() where Y : NPSfxMono
        {
            if (_sfxRefObj == null)
                return null;
            
            //构造特效对象结构体
            T sfxObj = new T();
            sfxObj.init(_sfxRefObj);

            //设置坐标
            sfxObj.setWorldPos(_pos);
            
            // 独立父节点固定位置的特效，layer默认设置为默认的layer层级，有特殊需求业务自己处理
            sfxObj.setLayer(WCGResCommon.WCG_C_LAYER_UNIT);
            
            //开启加载播放处理
            sfxObj.playSfxObj();
            
            //返回结构体
            return sfxObj;
        }

        /********************
         * 释放所有资源
         **/
        public void discard()
        {
        }
    }
}
