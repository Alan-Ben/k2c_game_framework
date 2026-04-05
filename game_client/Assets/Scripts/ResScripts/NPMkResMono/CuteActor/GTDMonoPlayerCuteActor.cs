using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    public enum EPlayerCuteActorTagType
    {
        [InspectorName("通用 COMMON")]
        COMMON,
        [InspectorName("关卡 CHAPTER")]
        CHAPTER,
        [InspectorName("宴会 DINNER")]
        DINNER,
    }
    
    [Serializable]
    public class PlayerCuteActorTagAnimatorController
    {
        public EPlayerCuteActorTagType type;
        public RuntimeAnimatorController controller;
    }
    
    public class GTDMonoPlayerCuteActor : MonoBehaviour
    {
        [ALHeader("玩家的动画机")]
        public Animator animator;
        [ALHeader("不同Q版形象tag对应的animationController")]
        public List<PlayerCuteActorTagAnimatorController> animationControllerList;
        
        [ALHeader("这个单位旋转的目标")]
        public Transform rotateTarget;
        
        [ALHeader("UI跟随的偏移")]
        public Vector3 uiFollowOffset;

        [ALHeader("特效挂载的父节点")]
        public Transform sfxParent;
        
        [ALHeader("身体材质")]
        [SerializeField]private Material bodyMat;
        [ALHeader("身体的SkinnedMeshRenderer,改变肤色用")]
        public List<Renderer> bodyRenders;
        private Material m_bodyMat;
        
        private void Awake()
        {
            if (null != bodyMat)
            {
                m_bodyMat = new Material(bodyMat);
                if (bodyRenders != null)
                    foreach (var render in bodyRenders)
                        if (render != null) render.sharedMaterial = m_bodyMat;   
            }
        }
        
        
        /// <summary>
        /// 获得对应的 animator controller
        /// </summary>
        public RuntimeAnimatorController getAnimatorController(EPlayerCuteActorTagType _type)
        {
            if (animationControllerList == null)
                return null;

            foreach (PlayerCuteActorTagAnimatorController data in animationControllerList)
            {
                if (data != null && data.type == _type)
                    return data.controller;
            }
            
            return null;
        }
        
        /// <summary>
        /// 设置肤色
        /// </summary>
        /// <param name="_skinColor"></param>
        public void setSkinColor(Color _skinColor)
        {
            if (m_bodyMat != null)
                m_bodyMat.SetColor(ShaderPropertyMgr.g_SkinColor, _skinColor);
        }
    }
}