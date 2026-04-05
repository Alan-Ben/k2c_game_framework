using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    internal class AttachmentItem_GameObject : _AAttachmentItem
    {
        private GameObject _m_attachmentGo;
        private Animator _m_attachmentGoAnimator;
        private AttachmentTarget _m_target;
        
        public AttachmentItem_GameObject([NotNull] AttachmentItemRefObj _refObj, AttachmentAnimatorAgent _animatorAgent) : base(_refObj, _animatorAgent)
        {
        }

        /// <summary>
        /// 把这个 item 附加到对应的目标上
        /// </summary>
        public override void attachTo(AttachmentTarget _target)
        {
            _m_target = _target;
            _refreshAttachToTarget();
        }

        protected override void _onInit()
        {
            int serialize = initSerialize;
            GGoIndexCacheMgr.instance.popItem(refObj.attach_go_index, _go =>
            {
                if (_go == null)
                    return;
                
                if (serialize != initSerialize)
                {
                    GGoIndexCacheMgr.instance.pushbackItem(refObj.attach_go_index, _go);
                    return;
                }

                _m_attachmentGo = _go;
                
                _m_attachmentGoAnimator = null;
                NPGTDMonoAttachmentGoItem mono = _go.GetComponent<NPGTDMonoAttachmentGoItem>();
                if (mono != null) _m_attachmentGoAnimator = mono.animator;
                
                animatorAgent?.addAnimator(_m_attachmentGoAnimator);
                _refreshAttachToTarget();
            });
        }

        protected override void _onDiscard()
        {
            if (_m_attachmentGo != null)
                GGoIndexCacheMgr.instance.pushbackItem(refObj.attach_go_index, _m_attachmentGo);
            
            if (_m_attachmentGoAnimator != null)
                animatorAgent?.removeAnimator(_m_attachmentGoAnimator);
            
            _m_attachmentGo = null;
            _m_attachmentGoAnimator = null;
        }

        private void _refreshAttachToTarget()
        {
            if (_m_attachmentGo == null || _m_target == null || _m_target.trans == null)
                return;
            
            switch (refObj.attach_type)
            {
                case EAttachType.NONE:
                    return;
                case EAttachType.BE_CHILD:
                    _m_attachmentGo.transform.SetParent(_m_target.trans, false);
                    _m_attachmentGo.transform.localScale = Vector3.one;
                    _m_attachmentGo.transform.localRotation = Quaternion.identity;
                    _m_attachmentGo.SetActive(true);
                    break;
                case EAttachType.SAME_POSITION:
                    _m_attachmentGo.transform.SetParent(null);
                    _m_attachmentGo.transform.position = _m_target.trans.position;
                    _m_attachmentGo.transform.localScale = Vector3.one;
                    _m_attachmentGo.transform.localRotation = Quaternion.identity;
                    _m_attachmentGo.SetActive(true);
                    break;
            }
        }
    }
}