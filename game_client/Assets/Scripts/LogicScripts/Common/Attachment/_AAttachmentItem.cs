using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    internal abstract class _AAttachmentItem
    {   
        // 附加物配置
        [NotNull] private readonly AttachmentItemRefObj _m_refObj;
        private readonly AttachmentAnimatorAgent _m_animatorAgent;
        
        private int _m_initSerialize;
        private bool _m_isInit;
        
        protected _AAttachmentItem([NotNull] AttachmentItemRefObj _refObj, AttachmentAnimatorAgent _animatorAgent)
        {
            _m_refObj = _refObj;
            _m_animatorAgent = _animatorAgent;
            
            _m_isInit = false;
            _m_initSerialize = ALSerializeOpMgr.next();
        }
        
        /// <summary>
        /// 初始化序列号
        /// </summary>
        public int initSerialize { get { return _m_initSerialize; } }
        /// <summary>
        /// 是否初始化了
        /// </summary>
        public bool isInit { get { return _m_isInit; } }
        /// <summary>
        /// 附加物的配置
        /// </summary>
        [NotNull] public AttachmentItemRefObj refObj { get { return _m_refObj; } }
        /// <summary>
        /// 动画代理
        /// </summary>
        public AttachmentAnimatorAgent animatorAgent { get { return _m_animatorAgent; } }
        
        /// <summary>
        /// 初始化
        /// </summary>
        public void init()
        {
            if (_m_isInit)
                return;

            _m_isInit = true;
            _onInit();
        }
        /// <summary>
        /// 把这个 item 附加到对应的目标上
        /// </summary>
        public abstract void attachTo(AttachmentTarget _target);
        /// <summary>
        /// 销毁这个 item
        /// </summary>
        public void discard()
        {
            if (!_m_isInit)
                return;

            _onDiscard();
            _m_isInit = false;
            _m_initSerialize = ALSerializeOpMgr.next();
        }

        protected abstract void _onInit();
        protected abstract void _onDiscard();
    }
}