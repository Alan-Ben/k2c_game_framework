using UnityEngine;

namespace GOE
{
    /// <summary>
    /// showcase resObj的通用mono
    /// </summary>
    public class ShowCaseCommonResObjMono : MonoBehaviour
    {
        [ALHeader("特效父节点")]
        public Transform sfxParent;
        [ALHeader("动画相关表现接口，有需要加对应mono托")]
        [ALInfo("ShowCaseCommonResObjAniEffect_Animation 动画资源是Animation用这个" +
                "\n ShowCaseCommonResObjAniEffect_Animator 动画资源是Animator用这个" +
                "\n ShowCaseCommonResObjAniEffect_Spine 动画资源是Spine用这个")]
        public _AShowCaseCommonResObjAniEffect aniShowInterface;
        [ALHeader("渲染相关表现接口，有需要加对应mono托")]
        [ALInfo("ShowCaseCommonResObjRenderEffect_Common 资源是SpriteRenderer用这个" +
                "\n ShowCaseCommonResObjRenderEffect_MeshRendererRes 资源是MeshRendererRes用这个")]
        public _AShowCaseCommonResObjRenderEffect renderShowInterface;
        
        
        //播放指定动画
        public void playAni(string _aniName)
        {
            if(null == aniShowInterface)
                return;
            
            aniShowInterface.playAni(_aniName);
        }

        //设置动画触发器
        public void setAnimTrigger(string _triggerName)
        {
            if(null == aniShowInterface)
                return;
            
            aniShowInterface.setAnimTrigger(_triggerName);
        }
        
        //设置动画速度
        public void setSpeed(float _speed)
        {
            if(null == aniShowInterface)
                return;
            
            aniShowInterface.setSpeed(_speed);
        }
        
        //设置透明度
        public void setAlpha(float _alpha)
        {
            if(null == renderShowInterface)
                return;
            
            renderShowInterface.setAlpha(_alpha);
        }

        //设置ZSpacing
        public void setZSpacing(float _value)
        {
            if(null == renderShowInterface)
                return;
            
            renderShowInterface.setZSpacing(_value);
        }
        
        //强制切换动画
        public void forceSetAni(string _aniName)
        {
            if(null == aniShowInterface)
                return;

            aniShowInterface.forceSetAni(_aniName);
        }

        /// <summary>
        /// 是否正在播放动画
        /// </summary>
        /// <param name="_aniName"></param>
        /// <returns></returns>
        public bool isPlayingAni(string _aniName)
        {
            if(null == aniShowInterface)
                return false;

            return aniShowInterface.isPlayingAni(_aniName);
        }
        
        public void enableRender(bool _isEnable)
        {
            if(null == aniShowInterface)
                return;

            aniShowInterface.enableRender(_isEnable);
        }
    }
}