using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 附加物的 Mono，主要提供给 AnimationEvent 使用
    /// </summary>
    public class GTDMonoAttachment : MonoBehaviour
    {
        // 附加物的列表
        [ItemNotNull][NotNull] private readonly List<PackedData> _m_attachmentDataList = new List<PackedData>();

        // 附加目标的 animator
        private Animator _m_animator;
        
        public void Awake()
        {
            _m_animator = GetComponent<Animator>();
        }

#if NP_GAME
      
        // 附加物的 animator agent
        private AttachmentAnimatorAgent _m_itemAnimatorAgent;
        public void setAnimatorAgent(AttachmentAnimatorAgent _animatorAgent)
        {
            _m_itemAnimatorAgent = _animatorAgent;
        }
        
#endif

        /// <summary>
        /// 添加一个附加物
        /// </summary>
        public void addAttachment(AnimationEvent _event)
        {
            if (_event == null)
                return;

            if (_m_animator == null)
                return;

            // 如果 animator 正在切换，判断下一个状态是不是自己，如果不是自己，就忽视
            if (_m_animator.IsInTransition(0))
            {
                AnimatorStateInfo stateInfo = _m_animator.GetNextAnimatorStateInfo(0);
                if (stateInfo.fullPathHash != _event.animatorStateInfo.fullPathHash)
                    return;
            }
            
            string parentName = _event.stringParameter;
            int attachId = _event.intParameter;
            addAttachment(parentName, attachId);
        }
        /// <summary>
        /// 添加一个附加物
        /// </summary>
        public void addAttachment(AttachmentData _data)
        {
            addAttachment(_data.parentName, _data.attachId);
        }
        /// <summary>
        /// 添加一个附加物
        /// </summary>
        public void addAttachment(string _parentName, int _attachId)
        {
#if NP_GAME
            Transform attachParent = transform.findTransform(_trans => _trans.name == _parentName, false);
            if (AttachmentMgr.instance.addAttachment(attachParent, _attachId, _m_itemAnimatorAgent))
            {
                _m_attachmentDataList.Add(new PackedData(attachParent, _attachId));
            }
#endif
        }
        /// <summary>
        /// 移除一个附加物
        /// </summary>
        public void removeAttachment(AnimationEvent _event)
        {
            if (_event == null)
                return;
            
            string parentName = _event.stringParameter;
            int attachId = _event.intParameter;
            removeAttachment(parentName, attachId);
        }
        /// <summary>
        /// 移除一个附加物
        /// </summary>
        public void removeAttachment(AttachmentData _data)
        {
            removeAttachment(_data.parentName, _data.attachId);
        }
        /// <summary>
        /// 移除一个附加物
        /// </summary>
        public void removeAttachment(string _parentName, int _attachId)
        {
#if NP_GAME
            Transform attachParent = transform.findTransform(_trans => _trans.name == _parentName, false);
            if (_m_attachmentDataList.FindAndRemove(_data => _data.attachParent == attachParent && _data.attachId == _attachId) != null)
            {
                AttachmentMgr.instance.removeAttachment(attachParent, _attachId);
            }
#endif
        }
        /// <summary>
        /// 清空所有的附加物
        /// </summary>
        public void clearAllAttachment()
        {
#if NP_GAME
            foreach (PackedData data in _m_attachmentDataList)
            {
                AttachmentMgr.instance.removeAttachment(data.attachParent, data.attachId);
            }
            _m_attachmentDataList.Clear();
#endif
        }

        public void OnDestroy()
        {
            clearAllAttachment();

            _m_animator = null;
        }

        private class PackedData
        {
            public readonly Transform attachParent;
            public readonly int attachId;

            public PackedData(Transform _attachParent, int _attachId)
            {
                attachParent = _attachParent;
                attachId = _attachId;
            }
        }
    }
}