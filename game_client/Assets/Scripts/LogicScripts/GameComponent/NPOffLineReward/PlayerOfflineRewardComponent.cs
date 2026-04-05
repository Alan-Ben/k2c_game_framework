
using System.Collections.Generic;
using ALPackage;
using Common.OfflineRewardEnum;
using Common.OfflineRewardObj;
using GS2GC.p002_InitOp;
using GS2GC.p007_CommOp;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 离线奖励（信息）组件
    /// </summary>
    /// <remarks>
    /// 所有在玩家离线中也会产生交互的信息都会在这个组件中处理
    /// </remarks>
    public class PlayerOfflineRewardComponent : _ANPBasicPlayerComponent
    {
        /// <summary>
        /// 离线奖励列表
        /// </summary>
        [ItemNotNull, NotNull] private readonly List<_ABasicOfflineRewardInfo> _m_offLineRewardList;
        

        public PlayerOfflineRewardComponent(NPPlayerComponentMgr _compMgr) 
            : base(_compMgr)
        {
            _m_offLineRewardList = new List<_ABasicOfflineRewardInfo>();
        }
        

        public override bool isMustInit { get { return true; } }
        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.OFF_LINE_REWARD; } }
        public override ENPPlayerCompType[] dependCompList { get { return null; } }
        /// <summary>
        /// 是否允许提前初始化。提前初始化的意思是在依赖项没有完成初始化之前就进行初始化操作（一般是提前发送消息）
        /// 在初始化结果消息返回的时候，通过特殊的初始化函数dealPreInitFunc进行处理函数注册，再依赖项完成之后才进行初始化处理
        /// </summary>
        public override bool canPreInit { get { return true; } }
        
        
        public override void presendInitProtocol()
        {
            NPGSClientListener.sendRequestByLog(GSWriter_002_InitOp.make_002_054_ReqOfflineRewardInit(), 
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_002_054_RetOfflineRewardInit>((_isSuc, _msg) =>
                {
                   if (_isSuc)
                       dealPreInitFunc(() => _initData(_msg));
                   else
                       setInitFail();
                }));
        }
        protected override void _dealInit()
        {
        }
        protected override void _onInitDone()
        {
        }
        protected override void _onInitFail()
        {
            ALLog.Error("[NPPlayerOffLineRewardComponent] init failed.");
        }
        protected override void _discard()
        {
        }
        public override void onAllCompInited()
        {
            foreach (_ABasicOfflineRewardInfo rewardInfo in _m_offLineRewardList)
            {
                rewardInfo.dealReward(true);
            }
        }


        /// <summary>
        /// 获取最早的一个离线奖励
        /// </summary>
        public _ABasicOfflineRewardInfo getOffLineRewardFirst(EOfflineRewardEnum _type)
        {
            foreach (_ABasicOfflineRewardInfo info in _m_offLineRewardList)
            {
                if (info.type == _type)
                    return info;
            }

            return null;
        }
        /// <summary>
        /// 获取最新的一个离线奖励
        /// </summary>
        public _ABasicOfflineRewardInfo getOffLineRewardLast(EOfflineRewardEnum _type)
        {
            int l = _m_offLineRewardList.Count;
            while (--l>-1)
            {
                _ABasicOfflineRewardInfo info = _m_offLineRewardList[l];
                if (info.type == _type)
                    return info;
            }
            
            return null;
        }
        /// <summary>
        /// 是否含有这个类型的离线奖励
        /// </summary>
        public bool getHasOfflineReward(EOfflineRewardEnum _type)
        {
            foreach (_ABasicOfflineRewardInfo info in _m_offLineRewardList)
            {
                if (info.type == _type)
                    return true;
            }

            return false;
        }
        /// <summary>
        /// 获取离线奖励列表
        /// </summary>
        public _ABasicOfflineRewardInfo getFirstOffLineRewardList(List<EOfflineRewardEnum> _typeList)
        {
            if (null == _typeList || _typeList.Count == 0)
                return null;

            foreach (_ABasicOfflineRewardInfo info in _m_offLineRewardList)
            {
                if (_typeList.Contains(info.type))
                    return info;
            }
            
            return null;
        }
        /// <summary>
        /// 获取离线奖励列表
        /// </summary>
        public void getOffLineRewardList(EOfflineRewardEnum _type, List<_ABasicOfflineRewardInfo> _list)
        {
            if (null == _list)
                return;

            _list.Clear();
            foreach (_ABasicOfflineRewardInfo info in _m_offLineRewardList)
            {
                if (info.type != _type)
                    continue;
                
                _list.Add(info);
            }
        }


        /// <summary>
        /// 初始化数据
        /// </summary>
        private void _initData(GS2GC_002_054_RetOfflineRewardInit _msg)
        {
            if (_msg == null)
            {
                setInitFail();
                return;
            }

            List<OfflineReward_Info> serverInfoList = _msg.getRewardList();
            if (serverInfoList == null)
            {
                setInitFail();
                return;
            }
            
            foreach (OfflineReward_Info serverInfo in serverInfoList)
            {
                _ABasicOfflineRewardInfo info = _createOffLineReward(serverInfo);
                if (null == info)
                    continue;
                
                _m_offLineRewardList.Add(info);
            }
            
            setInitDone();
        }
        /// <summary>
        /// new 一个离线奖励信息
        /// </summary>
        private _ABasicOfflineRewardInfo _createOffLineReward(OfflineReward_Info _info)
        {
            if (_info == null)
                return null;
            
            EOfflineRewardEnum type = (EOfflineRewardEnum)_info.getRewardType();
            switch (type)
            {
                // add instantiation of new classes here
                case EOfflineRewardEnum.ADULT_MARRY_REWARD:
                    return new OfflineRewardInfo_ChilMarrySuccess(_info);
                case EOfflineRewardEnum.C_ORDER_DELIVERY:
                    return new OfflineRewardInfo_OrderDelivery(_info);
            }

            return null;
        }
        
        /// <summary>
        /// 离线奖励删除
        /// </summary>
        public void onOfflineRewardDel(GS2GC_007_054_OnOfflineRewardDel _msg)
        {
            if (_msg == null)
                return;
            
            for (int i = 0; i < _m_offLineRewardList.Count; i++)
            {
                _ABasicOfflineRewardInfo info = _m_offLineRewardList[i];
                if (info.id != _msg.getId()) 
                    continue;
                
                _m_offLineRewardList.Remove(info);
                break;
            }
        }
        /// <summary>
        /// 离线奖励新增
        /// </summary>
        public void onOfflineRewardAdd(GS2GC_007_055_OnOfflineRewardAdd _msg)
        {
            _ABasicOfflineRewardInfo info = _createOffLineReward(_msg?.getReward());
            if (null == info)
                return;

            _m_offLineRewardList.Add(info);
            info.dealReward(false);
        }
    }
}