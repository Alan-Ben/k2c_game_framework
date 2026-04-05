using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    [RequireComponent(typeof(Collider))]
    public class NPGTDMonoAnimationTrigger : MonoBehaviour
    {
        [ALHeader("动画组件")] public new Animation animation;
        [ALHeader("进入动画名字")] public string enterAnimationName;
        [ALHeader("退出动画名字")] public string exitAnimationName;

        [ALHeader("进入特效id")] public long enterSfxId;
        [ALHeader("退出特效id")] public long exitSfxId;
        [ALHeader("特效父对象")] public Transform sfxParent;

#if NP_GAME
        [NotNull]private CommonSfxCtrlContainer _m_sfxContainer = new CommonSfxCtrlContainer();
#endif

        private void Awake()
        {
            
        }

        private void OnDestroy()
        {
#if NP_GAME
            _m_sfxContainer?.clear();
            _m_sfxContainer = null;
#endif
        }

        private void OnTriggerEnter(Collider other)
        {
            if (animation != null)
                animation.Play(enterAnimationName);

#if NP_GAME
            CommonTDSfxObj commonTdSfxObj = PlaySfxMgr.instance.playTDSfx(enterSfxId, sfxParent);
            _m_sfxContainer.addSfxObj(commonTdSfxObj);
#endif
        }

        private void OnTriggerExit(Collider other)
        {
            if (animation != null)
                animation.Play(exitAnimationName);

#if NP_GAME
            
            CommonTDSfxObj commonTdSfxObj = PlaySfxMgr.instance.playTDSfx(exitSfxId, sfxParent);
            _m_sfxContainer.addSfxObj(commonTdSfxObj);
#endif
        }
    }
}