
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    [RequireComponent(typeof(Collider))]
    public class NPGTDMonoAnimatorTrigger : MonoBehaviour
    {
        [ALHeader("动画组件")] 
        public Animator animator;
        [ALHeader("进入动画触发器")] 
        public string enterAnimationTrigger;
        [ALHeader("退出动画触发器")] 
        public string exitAnimationTrigger;

        [ALHeader("进入特效id")] 
        public long enterSfxId;
        [ALHeader("退出特效id")] 
        public long exitSfxId;
        [ALHeader("特效父对象")] 
        public Transform sfxParent;

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
            if (animator != null)
                animator.SetTrigger(enterAnimationTrigger);

#if NP_GAME
            CommonTDSfxObj commonTdSfxObj = PlaySfxMgr.instance.playTDSfx(enterSfxId, sfxParent);
            _m_sfxContainer.addSfxObj(commonTdSfxObj);
#endif
        }

        private void OnTriggerExit(Collider other)
        {
            if (animator != null)
                animator.SetTrigger(exitAnimationTrigger);

#if NP_GAME
            CommonTDSfxObj commonTdSfxObj = PlaySfxMgr.instance.playTDSfx(exitSfxId, sfxParent);
            _m_sfxContainer.addSfxObj(commonTdSfxObj);
#endif
        }
    }
}