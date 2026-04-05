using System;
using ALBasicProtocolPack;
using ALPackage;
using Common.ActivityEnum;
using CommonEnum;
using GOE;

namespace Hotfix
{
    /// <summary>
    /// 基于活动的组件基类 - 处理活动生命周期和初始化
    /// </summary>
    public abstract class HotfixActivityBasedComponent<T_C2S_INIT, T_S2C_INIT> : HotfixBaseComponent
        where T_C2S_INIT : _IALProtocolStructure, new()
        where T_S2C_INIT : _IALProtocolStructure, new()
    {
        // 当前的异步活动序列号
        private int _m_activitySerialize;
        // 当前组件内数据生效的活动对象
        private _ABaseActivityInfo _m_playingActivityInfo;


        /// <summary>
        /// 子类必须指定对应的活动类型
        /// </summary>
        protected abstract ECommonActivityType activityType { get; }
        /// <summary>
        /// 当前组件内数据生效的活动实例ID，没有数据时返回0
        /// </summary>
        public long activityInstanceId { get { return _m_playingActivityInfo?.instanceId ?? 0; } }


        protected override void _dealInit()
        {
            // 尝试获取活动初始化数据后绑定活动状态变更消息
            _tryGetActivityInitData(setInitDone);
            WinMsg.RegisterMsg(WinMsgType.ON_COMMON_ACTIVITY_STATE_CHG, _onActivityStateChg);
        }
        protected override void _onDiscard()
        {
            // 解绑活动状态变更消息后清除活动数据
            WinMsg.UnregisterMsg(WinMsgType.ON_COMMON_ACTIVITY_STATE_CHG, _onActivityStateChg);
            _clearActivityInitData();
        }
        protected override void _onInitDone()
        {
        }
        protected override void _onInitFail()
        {
        }


        /// <summary>
        /// 子类实现：从服务器数据初始化组件数据
        /// </summary>
        protected abstract void _initData(T_S2C_INIT _data);
        /// <summary>
        /// 子类实现：清除组件数据
        /// </summary>
        protected abstract void _clearData();


        /// <summary>
        /// 尝试获取活动初始化数据
        /// </summary>
        private void _tryGetActivityInitData(Action _complete = null)
        {
            // 如果当前活动还 OK 就不用尝试获取了
            if (null != _m_playingActivityInfo && _m_playingActivityInfo.isPlaying)
            {
                _complete?.Invoke();
                return;
            }

            // 如果当前没有活动数据，说明还没有进行初始化，或是活动关闭了，先清空上一次的数据，准备重新获取
            _clearActivityInitData();
            // 找到对应的活动数据
            _ABaseActivityInfo activityInfo = NPPlayer.instance.commonActivityComp.getValidActivityInfoByType(activityType);
            // 如果活动处于进行中状态，尝试获取初始化数据
            if (null != activityInfo && activityInfo.isPlaying)
            {
                // 记录当前组件内数据的活动对象
                _m_playingActivityInfo = activityInfo;
                // 用当前的活动序列号去取数据
                int serialize = _m_activitySerialize;
                // 取数据中
                NPGSClientListener.sendRequestByLog(new T_C2S_INIT(),
                    new HotfixCommonRequestSucFailSameCallbackProtocolDealer<T_S2C_INIT>((_isSuc, _msg) =>
                    {
                        // 如果活动序列号变了，说明数据已经无效，直接返回
                        if (serialize != _m_activitySerialize)
                        {
                            _complete?.Invoke();
                            return;
                        }

                        // 子类处理数据更新
                        _initData(_msg);
                        _complete?.Invoke();
                    }));
            }
            // 活动不处于进行中状态，直接返回，等待事件监听出现变化
            else
                _complete?.Invoke();
        }
        /// <summary>
        /// 清除活动初始化数据
        /// </summary>
        private void _clearActivityInitData()
        {
            // 如果当前没有活动数据，直接返回(该方法不关心活动是否还在进行中，直接清除内部数据)
            if (_m_playingActivityInfo == null)
                return;

            _clearData();
            // 清除当前组件内数据的活动对象，并且序列号增加表示上一个数据已经无效
            _m_playingActivityInfo = null;
            _m_activitySerialize = ALSerializeOpMgr.next();
        }
        /// <summary>
        /// 活动状态变更
        /// </summary>
        private void _onActivityStateChg(params object[] _objects)
        {
            if (_objects == null || _objects.Length < 4 ||
                _objects[1] is not long instanceId ||
                _objects[3] is not EActivityState nowActivityState)
                return;

            // 如果当前数据为空，判断产生变化的新活动是否要进行数据初始化
            if (_m_playingActivityInfo == null)
            {
                _ABaseActivityInfo lastActivity = NPPlayer.instance.commonActivityComp.getValidActivityInfoByType(activityType);
                // 如果新变化的活动就是最新的活动，继续往下判断
                if (lastActivity != null && instanceId == lastActivity.instanceId && nowActivityState == EActivityState.PLAYING)
                    _tryGetActivityInitData();
            }
            else if (instanceId == _m_playingActivityInfo.instanceId && nowActivityState != EActivityState.PLAYING)
                _clearActivityInitData();
        }
    }
}
