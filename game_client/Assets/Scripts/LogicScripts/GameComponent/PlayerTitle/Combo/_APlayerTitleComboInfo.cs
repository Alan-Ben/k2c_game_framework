using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 组合称号信息基类
    /// </summary>
    public abstract class _APlayerTitleComboInfo
    {
        //是否已经解锁
        private bool _m_bIsUnlock;
        //是否查看
        private bool _m_bIsViewed;
        //监听消息列表
        private List<string> _m_lMsgList;
        /// <summary>
        /// 是否已经解锁
        /// </summary>
        public bool isUnlock { get { return _m_bIsUnlock; } set { _m_bIsUnlock = value;} }
        /// <summary>
        /// 是否是新的称号
        /// </summary>
        public bool isNew { get { return !_m_bIsViewed && isUnlock; } }

        protected _APlayerTitleComboInfo(List<string> _msgList)
        {
            _m_lMsgList = _msgList;
            //监听客户端目标数变动
            _addRegister(_msgList);
        }

        /// <summary>
        /// 监听解锁状态
        /// </summary>
        private void _addRegister(List<string> _msgList)
        {
            if (null == _msgList)
                return;

            WinMsgType temp = 0;
            for (int i = 0; i < _msgList.Count; i++)
            {
                bool isParse = ALCommon.TryEnumParse(typeof(WinMsgType), _msgList[i], out temp);
                if (!isParse || temp == WinMsgType.NONE)
                    continue;

                WinMsg.RegisterMsgAct(temp, _onCheckUnlock);
            }
        }

        /// <summary>
        /// 移除监听
        /// </summary>
        private void _removeRegister(List<string> _msgList)
        {
            if (null == _msgList)
                return;

            WinMsgType temp = 0;
            for (int i = 0; i < _msgList.Count; i++)
            {
                bool isParse = ALCommon.TryEnumParse(typeof(WinMsgType), _msgList[i], out temp);
                if (!isParse || temp == WinMsgType.NONE)
                    continue;

                WinMsg.UnregisterMsgAct(temp, _onCheckUnlock);
            }
        }

        //检查是否解锁
        public void checkIsUnlock(bool _popTip)
        {
            //如果已经解锁就不处理
            if (_m_bIsUnlock)
                return;

            //如果没有解锁条件或者解锁条件满足就解锁
            if (unlockCondition == null || unlockCondition.isEmpty || unlockCondition.IsEnable(null))
            {
                //条件通过，设置为已解锁
                _m_bIsUnlock = true;
                _dealUnlock(_popTip);
            }
        }

        /// <summary>
        /// 设置是否查看
        /// </summary>
        public void setIsViewed(bool _isViewed)
        {
            _m_bIsViewed = _isViewed;
            if(_m_bIsViewed)
                _dealSetIsViewed();
        }

        /// <summary>
        /// 清空数据
        /// </summary>
        public void clear()
        {
            _removeRegister(_m_lMsgList);
        }

        /// <summary>
        /// 监听消息检查解锁
        /// </summary>
        private void _onCheckUnlock()
        {
            checkIsUnlock(true);
        }

        /// <summary>
        /// 解锁条件
        /// </summary>
        protected abstract _NPPlayerConditionSerializeInfo unlockCondition { get; }
        /// <summary>
        /// 处理解锁
        /// </summary>
        protected abstract void _dealUnlock(bool _popTip);
        /// <summary>
        /// 处理已查看
        /// </summary>
        protected abstract void _dealSetIsViewed();
    }
}

