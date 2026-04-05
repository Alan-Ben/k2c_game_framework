using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 附加物管理器
    /// </summary>
    /// <remarks>
    /// 给 Transform 添加上对应的附加物，不可以把同一个 attachItemId 附加在同一个 Transform 上
    /// </remarks>
    public class AttachmentMgr
    {
        /// <summary>
        /// 单例
        /// </summary>
        [NotNull]
        public static AttachmentMgr instance
        {
            get
            {
                if (_g_instance == null) _g_instance = new AttachmentMgr();
                return _g_instance;
            }
        }
        private static AttachmentMgr _g_instance;

        /// <summary>
        /// 对应的附加物目标
        /// </summary>
        [NotNull] private readonly Dictionary<Transform, AttachmentTarget> _m_attachmentTargets;

        private AttachmentMgr()
        {
            _m_attachmentTargets = new Dictionary<Transform, AttachmentTarget>();
        }

        /// <summary>
        /// 给对应 Transform 添加附加物
        /// </summary>
        public bool addAttachment(Transform _targetTransform, int _attachItemId, AttachmentAnimatorAgent _animatorAgent)
        {
            if (_targetTransform == null)
                return false;

            // 尝试获取对应的配置
            AttachmentItemRefObj attachRef = GRefdataCoreMgr.instance.attachmentItemRefCore.getRef(_attachItemId);
            if (attachRef == null)
                return false;

            // 尝试获取对应的附加物目标，如果获取不到，就新建一个
            if (!_m_attachmentTargets.TryGetValue(_targetTransform, out AttachmentTarget attachTarget) || attachTarget == null)
            {
                attachTarget = new AttachmentTarget();
                attachTarget.init(_targetTransform);
                _m_attachmentTargets[_targetTransform] = attachTarget;
            }

            // 添加附加物
            if (!attachTarget.addAttachment(attachRef, _animatorAgent))
            {
                // 如果添加失败，判断是不是当前这个附加物目标是空的，就认为不被需要了，discard 后移除
                checkIsEmptyAndRemove(attachTarget);
                return false;
            }

            return true;
        }
        /// <summary>
        /// 从 Transform 上移除一个附加物
        /// </summary>
        public void removeAttachment(Transform _targetTransform, int _attachItemId)
        {
            if (_targetTransform == null)
                return;

            // 尝试获取对应的附加物目标
            if (_m_attachmentTargets.TryGetValue(_targetTransform, out AttachmentTarget attachTarget) && attachTarget != null)
            {
                attachTarget.removeAttachment(_attachItemId);
                checkIsEmptyAndRemove(attachTarget);
            }
        }
        /// <summary>
        /// 判断是不是当前这个附加物目标是空的，就认为不被需要了，discard 后移除
        /// </summary>
        internal void checkIsEmptyAndRemove(AttachmentTarget _attachTarget)
        {
            if (_attachTarget == null || _attachTarget.trans == null)
                return;
            
            if (_attachTarget.isEmpty)
            {
                _m_attachmentTargets.Remove(_attachTarget.trans);
                _attachTarget.discard();
            }
        }
        /// <summary>
        /// 强制删除一个附加物目标
        /// </summary>
        internal void forceRemoveAttachmentTarget(AttachmentTarget _attachTarget)
        {
            if (_attachTarget == null || _attachTarget.trans == null)
                return;

            _m_attachmentTargets.Remove(_attachTarget.trans);
            _attachTarget.discard();
        }
    }
}