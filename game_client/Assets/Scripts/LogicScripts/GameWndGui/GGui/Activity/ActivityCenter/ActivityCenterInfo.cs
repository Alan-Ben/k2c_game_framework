using System;

namespace GOE
{
    /// <summary>
    /// 活动中心信息类
    /// </summary>
    public class ActivityCenterInfo
    {
        //活动中心配置
        private ActivityCenterRefObj _m_refObj;
        //消息监听回调
        private Action<ActivityCenterRefObj> _m_aOnRecMsg;

        /// <summary>
        /// 活动中心配置
        /// </summary>
        public ActivityCenterRefObj refObj { get { return _m_refObj; } set { _m_refObj = value; } }
        /// <summary>
        /// 消息监听回调
        /// </summary>
        public Action<ActivityCenterRefObj> onRecMsg { get { return _m_aOnRecMsg; } set { _m_aOnRecMsg = value; } }
        /// <summary>
        /// 是否可以显示
        /// </summary>
        public bool canShow { get { return _m_refObj != null && (_m_refObj.show_condition == null || _m_refObj.show_condition.isEmpty || _m_refObj.show_condition.IsEnable(null)); } }

        public ActivityCenterInfo(ActivityCenterRefObj _ref)
        {
            _m_refObj = _ref;
        }

        /// <summary>
        /// 注册消息监听
        /// </summary>
        public void regMsg()
        {
            if (_m_refObj == null || _m_refObj.addMsgTypeList == null)
                return;

            for (int i = 0; i < _m_refObj.addMsgTypeList.Count; i++)
            {
                WinMsg.RegisterMsgAct(_m_refObj.addMsgTypeList[i], _onRecMsg);
            }
        }

        /// <summary>
        /// 取消注册消息监听
        /// </summary>
        public void unRegMsg()
        {
            if (_m_refObj == null || _m_refObj.addMsgTypeList == null)
                return;

            for (int i = 0; i < _m_refObj.addMsgTypeList.Count; i++)
            {
                WinMsg.UnregisterMsgAct(_m_refObj.addMsgTypeList[i], _onRecMsg);
            }
        }

        //监听消息需要执行的方法
        private void _onRecMsg()
        {
            onRecMsg?.Invoke(_m_refObj);
        }
    }
}