using System;
using System.Collections.Generic;
using ALPackage;
using GOE.U2D;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.U2D;

namespace GOE
{
   
    public class SpriteMeshModifyMgr
    {
        private static SpriteMeshModifyMgr _g_instance = new SpriteMeshModifyMgr();
        public static SpriteMeshModifyMgr instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new SpriteMeshModifyMgr();
                return _g_instance;
            }
        }
        
        private Dictionary<int, List<SpriteModifyInfo> > _m_dSpriteModifyObjs = new Dictionary<int, List<SpriteModifyInfo> >();

        // private Dictionary<int, List<SpriteShadowInfo>> _m_dSpriteShadowInfos = new Dictionary<int, List<SpriteShadowInfo>>();
        
        private Vector3 _m_vLightDir = new Vector3(-0.3213501f, -0.7660446f, 0.5566711f);

        private long _m_lSerializeId = 0;

        public Vector3 lightDir => _m_vLightDir;

        /// <summary>
        /// 初始化灯光角度
        /// </summary>
        /// <param name="_lightDir"></param>
        public void initLightDir(Vector3 _lightDir)
        {
            updateLightDir(_lightDir);
        }

        //TODO
        /// <summary>
        /// 释放
        /// </summary>
        public void discard()
        {
            
        }

        public void closeShadow()
        {
            _m_lSerializeId++;
        }

        /// <summary>
        /// 更新灯光方向
        /// </summary>
        /// <param name="_lightDir"></param>
        public void updateLightDir(Vector3 _lightDir)
        {
            // if (_m_vLightDir != _lightDir)
            // {
            //     _m_vLightDir = _lightDir;
            //     //TODO 有需要动态改变灯光位置的时候开启
            //     foreach (var spriteShadowInfos in _m_dSpriteShadowInfos.Values)
            //     {
            //         foreach (var spriteShadowInfo in spriteShadowInfos)
            //         {
            //             spriteShadowInfo.updateDir(_m_vLightDir);
            //         }
            //     }
            // }
        }

        public Sprite getModifySprite(SpriteModifyInfo spriteModifyInfo)
        {
            int instanceId = spriteModifyInfo.sprite.GetInstanceID();
            
            if (!_m_dSpriteModifyObjs.TryGetValue(instanceId, out List<SpriteModifyInfo> list))
            {
                list = new List<SpriteModifyInfo>();
                _m_dSpriteModifyObjs.Add(instanceId,list);
            }

            SpriteModifyInfo tInfo = null;
            foreach (var obj in list)
            {
                if (obj.isEqual(spriteModifyInfo))
                {
                    tInfo = obj;
                    break;
                }
            }

            if (tInfo == null)
            {
                tInfo = new SpriteModifyInfo(spriteModifyInfo);
                list.Add(tInfo);
            }

            return tInfo.getModifySprite();
        }

        // /// <summary>
        // /// 获取修改后的平面阴影Sprite
        // /// </summary>
        // /// <param name="_info"></param>
        // /// <param name="_rotation"></param>
        // /// <returns></returns>
        // public Sprite getShadowSprite(SpriteShadowInfo _info, Quaternion _rotation)
        // {
        //     int instanceId = _info.sprite.GetInstanceID();
        //     
        //     if (!_m_dSpriteShadowInfos.TryGetValue(instanceId, out List<SpriteShadowInfo> list))
        //     {
        //         list = new List<SpriteShadowInfo>();
        //         _m_dSpriteShadowInfos.Add(instanceId,list);
        //     }
        //
        //     SpriteShadowInfo tInfo = null;
        //     foreach (var obj in list)
        //     {
        //         if (obj.isEqual(_rotation))
        //         {
        //             tInfo = obj;
        //             break;
        //         }
        //     }
        //
        //     if (tInfo == null)
        //     {
        //         tInfo = new SpriteShadowInfo(_info, _rotation);
        //         list.Add(tInfo);
        //     }
        //
        //     return tInfo.getShadowSprite(_m_vLightDir);
        // }
        //
        // public void regShadowSprite(SpritePlanarShadow _planarShadow)
        // {
        //     // _m_lSpritePlanarShadows.Add(_planarShadow);
        //     ALCommonTaskController.CommonActionAddNextFrameLaterTask(() =>
        //     {
        //         updatePlanarShadowSprite(_planarShadow);
        //     });
        //     // updatePlanarShadowSprite(_planarShadow);
        // }
        //
        // public void unRegShadowSprite(SpritePlanarShadow _planarShadow)
        // {
        //     // _m_lSpritePlanarShadows.Remove(_planarShadow);
        // }
        //
        // private void updatePlanarShadowSprite(SpritePlanarShadow _planarShadow)
        // {
        //     if(_planarShadow.spriteShadowInfo.sprite == null)
        //         return;
        //     _planarShadow.setPlanarShadowSprite(getShadowSprite(_planarShadow.spriteShadowInfo, _planarShadow.transform.rotation));
        // }

    }
}