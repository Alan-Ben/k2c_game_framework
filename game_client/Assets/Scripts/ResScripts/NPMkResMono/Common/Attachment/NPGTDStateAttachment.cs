using System;
using UnityEngine;

namespace GOE
{
    public class NPGTDStateAttachment : StateMachineBehaviour
    {
        public bool resetAllWhenEnter;
        public AttachmentConfig enterConfig;
        public AttachmentConfig exitConfig;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);
            
            if (animator == null)
                return;
            
            GTDMonoAttachment attachment = animator.GetComponent<GTDMonoAttachment>();
            if (attachment == null)
                return;
            
            if (resetAllWhenEnter)
                attachment.clearAllAttachment();
            
            _dealAttachmentConfig(attachment, enterConfig);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);
            
            if (animator == null)
                return;
            
            GTDMonoAttachment attachment = animator.GetComponent<GTDMonoAttachment>();
            if (attachment == null)
                return;
            
            _dealAttachmentConfig(attachment, exitConfig);
        }

        private void _dealAttachmentConfig(GTDMonoAttachment _attachmentMono, AttachmentConfig _config)
        {
            if (_attachmentMono == null)
                return;
            
            switch (_config.type)
            {
                case AttachType.ADD:
                    _attachmentMono.addAttachment(_config.data);     
                    break;
                case AttachType.REMOVE:
                    _attachmentMono.removeAttachment(_config.data);
                    break;
            }
        }
        
        [Serializable]
        public struct AttachmentConfig
        {
            public AttachType type;
            public AttachmentData data;
        }

        public enum AttachType
        {
            NONE,
            ADD,
            REMOVE,
        }
    }

}