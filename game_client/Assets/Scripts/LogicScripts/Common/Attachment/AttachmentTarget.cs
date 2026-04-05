using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 附加物的附加目标
    /// </summary>
    internal class AttachmentTarget
    {
        // 想要附加到的 Transform
        private Transform _m_transform;
        // 是否初始化
        private bool _m_isInit;
        
        // 检测 Transform 是否被删除的脚本
        private NPMonoDestroyFunc _m_destroyFuncMono;

        /// <summary>
        /// 附加物字典
        /// </summary>
        [NotNull] private readonly Dictionary<long, _AAttachmentItem> _m_attachmentItems;

        public AttachmentTarget()
        {
            _m_attachmentItems = new Dictionary<long, _AAttachmentItem>();
            _m_isInit = false;
        }
        
        /// <summary>
        /// 是否没有任何附加物
        /// </summary>
        public bool isEmpty { get { return !_m_isInit || _m_attachmentItems.Count == 0; } }
        /// <summary>
        /// 想要附加到的 Transform
        /// </summary>
        public Transform trans { get { return _m_transform; } }

        /// <summary>
        /// 初始化数据
        /// </summary>
        public void init([NotNull] Transform _targetTrans)
        {
            if (_m_isInit)
                return;
            
            _m_transform = _targetTrans;
            // 给附加物目标的 transform 附加上销毁方法脚本，确保被挂在上面的附加物不会跟着一起被销毁掉
            _m_destroyFuncMono = _m_transform.gameObject.AddComponent<NPMonoDestroyFunc>();
            _m_destroyFuncMono.setDestroyFunc(() =>
            {
                // transform 在 destroy 的时候强制移除自己
                AttachmentMgr.instance.forceRemoveAttachmentTarget(this);
            });

            _m_isInit = true;
        }
        /// <summary>
        /// 清空所有内容
        /// </summary>
        public void discard()
        {
            if (!_m_isInit)
                return;

            // 清空所有附加物
            foreach (_AAttachmentItem item in _m_attachmentItems.Values)
            {
                item?.discard();
            }
            _m_attachmentItems.Clear();

            if (_m_destroyFuncMono != null)
            {
                _m_destroyFuncMono.setDestroyFunc(null);
                Object.Destroy(_m_destroyFuncMono);
            }
            _m_destroyFuncMono = null;
            
            _m_transform = null;
            
            _m_isInit = false;
        }

        /// <summary>
        /// 添加一个附加物
        /// </summary>
        public bool addAttachment(long _refId, AttachmentAnimatorAgent _animatorAgent)
        {
            if (!_m_isInit)
                return false;
            
            AttachmentItemRefObj attachRef = GRefdataCoreMgr.instance.attachmentItemRefCore.getRef(_refId);
            if (attachRef == null)
                return false;

            return addAttachment(attachRef, _animatorAgent);
        }
        /// <summary>
        /// 添加一个附加物
        /// </summary>
        public bool addAttachment(AttachmentItemRefObj _refObj, AttachmentAnimatorAgent _animatorAgent)
        {
            if (!_m_isInit || _refObj == null)
                return false;

            if (_m_attachmentItems.ContainsKey(_refObj.id))
                return false;
            
            _AAttachmentItem attachmentItem = _createAttachmentItem(_refObj, _animatorAgent);
            if (attachmentItem == null)
                return false;
            
            attachmentItem.init();
            attachmentItem.attachTo(this);
            _m_attachmentItems.Add(_refObj.id, attachmentItem);
            return true;
        }
        
        public void removeAttachment(long _refId)
        {
            if (!_m_isInit)
                return;

            if (_m_attachmentItems.TryGetValue(_refId, out _AAttachmentItem item) && item != null)
                item.discard();

            _m_attachmentItems.Remove(_refId);
            AttachmentMgr.instance.checkIsEmptyAndRemove(this);
        }

        private _AAttachmentItem _createAttachmentItem([NotNull] AttachmentItemRefObj _refObj, AttachmentAnimatorAgent _animatorAgent)
        {
            switch (_refObj.attachment_item_type)
            {
                case EAttachmentItemType.NONE:
                    return null;
                case EAttachmentItemType.GAME_OBJECT:
                    return new AttachmentItem_GameObject(_refObj, _animatorAgent);
                case EAttachmentItemType.SFX:
                    return new AttachmentItem_SFX(_refObj, _animatorAgent);
            }

            return null;
        }
    }
}