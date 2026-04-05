using System;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 通用关于操作遮罩逻辑
    /// </summary>
    public class CommonOpMaskLogic
    {
        // 总输入遮罩的计数，要求 open 和 close 一一对应才行，好处是支持了多套逻辑同时需要屏蔽操作
        private int _m_iOpMaskCounter = 0;
        // 当前 open close 方法的有效序列号，forceClose 会使序列号自增，废弃旧的 close 将不再影响新的 open
        private int _m_iOpMaskSerialize = 0;
        
        private Action _m_aDealOpenOpMask;
        private Action _m_aDealCloseOpMask;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_dealOpenOpMask">打开操作遮罩的具体方法</param>
        /// <param name="_dealCloseOpMask">关闭操作遮罩的具体方法</param>
        public CommonOpMaskLogic(Action _dealOpenOpMask = null, Action _dealCloseOpMask = null)
        {
            _m_aDealOpenOpMask = _dealOpenOpMask;
            _m_aDealCloseOpMask = _dealCloseOpMask;
        }
        
        /// <summary>
        /// 操作是否被屏蔽
        /// </summary>
        public bool isOpMasking { get { return _m_iOpMaskCounter > 0; } }
        
        /// <summary>
        /// 打开操作遮罩
        /// </summary>
        public int openOpMask()
        {
            // 增加计数
            _m_iOpMaskCounter++;
            // 计数为 0 时才通过
            if (_m_iOpMaskCounter > 1)
                return _m_iOpMaskSerialize;
            
            _m_aDealOpenOpMask?.Invoke();
            
            // 返回当前的序列号
            return _m_iOpMaskSerialize;
        }
        /// <summary>
        /// 恢复玩家的正常输入
        /// </summary>
        /// <param name="_serialize">需要带入 open 给到的序列号</param>
        public void closeOpMask(int _serialize)
        {
            // 如果序列号不同，说明当前屏蔽系统已经被 forceClose 过一次，旧序列号的操作不再被响应
            if (_m_iOpMaskSerialize != _serialize)
                return;
            
            // 减少计数
            _m_iOpMaskCounter--;
            // 计数小于等于 0 才进行处理
            if (_m_iOpMaskCounter > 0)
                return;

            // 强行赋值 0 ，防止奇怪的情况出现
            _m_iOpMaskCounter = 0;
         
            _m_aDealCloseOpMask?.Invoke();
        }
        
        /// <summary>
        /// 强制关闭操作遮罩
        /// </summary>
        public void forceCloseOpMask()
        {
            // 自增序列号，让旧序列号的 open 和 close 不再响应
            _m_iOpMaskSerialize = ALSerializeOpMgr.next();
            // 强行赋值 0 ，防止奇怪的情况出现
            _m_iOpMaskCounter = 0;
            
            // 恢复所有玩家的输入权限
            _m_aDealCloseOpMask?.Invoke();
        }
    }
}