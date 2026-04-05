
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    internal class AttachmentItem_SFX : _AAttachmentItem
    {
        // 附加物配置
        private CommonTDSfxObj _m_sfxObj;
        
        public AttachmentItem_SFX([NotNull] AttachmentItemRefObj _refObj, AttachmentAnimatorAgent _animatorAgent) : base(_refObj, _animatorAgent)
        {
        }

        /// <summary>
        /// 把这个 item 附加到对应的目标上
        /// </summary>
        public override void attachTo(AttachmentTarget _target)
        {
            if (_target == null || _target.trans == null || _target.trans == null)
                return;
            
            if (_m_sfxObj != null)
                _m_sfxObj.forceDiscard();
            
            switch (refObj.attach_type)
            {
                case EAttachType.NONE:
                    return;
                case EAttachType.BE_CHILD:
                    _m_sfxObj = PlaySfxMgr.instance.playTDSfx(refObj.attach_sfx_id, _target.trans);
                    break;
                case EAttachType.SAME_POSITION:
                    _m_sfxObj = PlaySfxMgr.instance.playSfxByPos(refObj.attach_sfx_id, _target.trans.position);
                    break;
            }

            if (null != _m_sfxObj)
            {
                _m_sfxObj.regPlayCompleteDelegate(() =>
                {
                    _target.removeAttachment(refObj.id); 
                });
            }
            else
            {
                _target.removeAttachment(refObj.id);
            }
        }

        protected override void _onInit()
        {
        }

        protected override void _onDiscard()
        {
            _m_sfxObj?.discardNoTiming();
            _m_sfxObj = null;
        }
    }
}