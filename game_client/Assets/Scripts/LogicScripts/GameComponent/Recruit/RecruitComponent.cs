using System;
using System.Collections.Generic;
using ALPackage;
using GS2GC.p002_InitOp;
using GS2GC.p007_CommOp;
using JetBrains.Annotations;
using NPEnum;
using UnityEngine.Pool;

namespace GOE
{
    /// <summary>
    /// 招募组件
    /// </summary>
    public class RecruitComponent : _ANPBasicPlayerComponent
    {
        private List<RecruitShopInfo> _m_lRecruitShopInfoList;//招募商店信息列表

        private bool _m_bHasRefreshRedTipTask;//是否有刷新红点任务
        [NotNull] private ObjectPool<NPCommonItem> _m_commonItemPool = new ObjectPool<NPCommonItem>(() => new NPCommonItem(), null, null, null, false, 1);//通用道具对象池
        [NotNull] private List<NPCommonItem> _m_lChgItemList = new List<NPCommonItem>();//变化的道具列表
        
        public RecruitComponent(NPPlayerComponentMgr _compMgr) : base(_compMgr)
        {
        }

        public override bool isMustInit { get { return true; } }
        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.RECRUIT; } }
        public override ENPPlayerCompType[] dependCompList { get { return null; } }
        public override bool canPreInit { get { return true; } }

        public override void presendInitProtocol()
        {
            _reqRecruitInfo();
        }

        protected override void _dealInit()
        {
            if (_m_lRecruitShopInfoList == null)
                _m_lRecruitShopInfoList = new List<RecruitShopInfo>();
            _m_lRecruitShopInfoList.Clear();
            
            foreach (var shopRefObj in GRefdataCoreMgr.instance.recruitShopRefCore.refList)
            {
                if(shopRefObj != null)
                    _m_lRecruitShopInfoList.Add(new RecruitShopInfo(shopRefObj));
            }
            
            WinMsg.RegisterMsg(WinMsgType.ON_COMMON_ITEM_COUNT_CHG, _onCommonItemChg);
        }

        protected override void _onInitDone()
        {
        }

        public override void onAllCompInited()
        {
            base.onAllCompInited();

            // 所有组件初始化完成后, 刷新红点
            _refreshRedTipAll();
        }

        protected override void _onInitFail()
        {
            ALLog.Error("RecruitComponent init Fail!!!");
        }

        protected override void _discard()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_COMMON_ITEM_COUNT_CHG, _onCommonItemChg);
            
            _m_lRecruitShopInfoList?.Clear();
            _m_lRecruitShopInfoList = null;
            
            _m_bHasRefreshRedTipTask = false;
            _pushBackAllCommonItem();
            _m_commonItemPool.Clear();
        }

        /// <summary>
        /// 获取招募商店信息
        /// </summary>
        /// <param name="_shopId"></param>
        /// <returns></returns>
        public RecruitShopInfo getShopInfo(long _shopId)
        {
            if(_m_lRecruitShopInfoList == null)
                return null;
            
            return _m_lRecruitShopInfoList.Find((_info) => _info != null && _info.shopId == _shopId);
        }
        
        public RecruitShopInfo getShopInfoWithCreate(long _shopId)
        {
            RecruitShopInfo shopInfo = getShopInfo(_shopId);
            if (shopInfo == null)
            {
                RecruitShopRefObj shopRefObj = GRefdataCoreMgr.instance.recruitShopRefCore.getRef(_shopId);
                if (shopRefObj != null)
                {
                    shopInfo = new RecruitShopInfo(shopRefObj);

                    if (_m_lRecruitShopInfoList == null)
                        _m_lRecruitShopInfoList = new List<RecruitShopInfo>();
                    _m_lRecruitShopInfoList.Add(shopInfo);
                }
                else
                {
                    Debug.LogError($"[RecruitComponent getShopInfoWithCreate] shopRefObj is null, shopId: {_shopId}");
                }
            }

            return shopInfo;
        }
        
        /// <summary>
        /// 是否已招募
        /// </summary>
        /// <param name="_recruitItemInfo"></param>
        /// <returns></returns>
        public bool isHadRecruit(_ARecruitItemInfo _recruitItemInfo)
        {
            if (_recruitItemInfo == null || _recruitItemInfo.recruitRefObj == null)
                return false;

            RecruitShopInfo shopInfo = getShopInfoWithCreate(_recruitItemInfo.recruitRefObj.shop_id);
            if(shopInfo == null)
                return false;
            
            // 判断 是否有已招募记录 或 道具数量大于0
            return shopInfo.isHadRecruit(_recruitItemInfo);
        }
        
        /// <summary>
        /// 获取招募状态
        /// </summary>
        /// <returns></returns>
        public ERecruitState getRecruitState(_ARecruitItemInfo _recruitItemInfo)
        {
            if (_recruitItemInfo == null || _recruitItemInfo.recruitRefObj == null)
                return ERecruitState.NONE;

            RecruitShopInfo shopInfo = getShopInfoWithCreate(_recruitItemInfo.recruitRefObj.shop_id);
            if(shopInfo == null)
                return ERecruitState.NONE;

            return shopInfo.getRecruitState(_recruitItemInfo, false);
        }
        
        /// <summary>
        /// 添加已被招募id
        /// </summary>
        private void _addRecruitId(long _id)
        {
            RecruitRefObj recruitRefObj = GRefdataCoreMgr.instance.recruitRefCore.getRef(_id);
            if(recruitRefObj == null)
                return;
            
            RecruitShopInfo shopInfo = getShopInfoWithCreate(recruitRefObj.shop_id);
            if(shopInfo != null)
                shopInfo.addRecruitId(_id);
        }

        #region 红点

        /// <summary>
        /// 创建刷新红点任务
        /// </summary>
        private void _createRefreshRedTipTask()
        {
            if (_m_bHasRefreshRedTipTask)
                return;

            _m_bHasRefreshRedTipTask = true;
            ALCommonTaskController.CommonActionAddMonoTask(()=>
            {
                if(!_m_bHasRefreshRedTipTask)
                    return;
                
                _m_bHasRefreshRedTipTask = false;
                
                _refreshRedTipTask();
            }, 1f);//延迟一会刷新红点, 防止频繁刷新
        }
        
        /// <summary>
        /// 刷新红点
        /// </summary>
        private void _refreshRedTipTask()
        {
            // 根据_m_lChgItemList刷新红点
            if (_m_lChgItemList.Count > 0 && _m_lRecruitShopInfoList != null)
            {
                foreach (var recruitShopInfo in _m_lRecruitShopInfoList)
                {
                    if(recruitShopInfo == null || recruitShopInfo.recruitShopRefObj == null)
                        continue;

                    bool needChgRed = false;
                    bool hasUnRecruitItem = false;

                    if (recruitShopInfo.recruitItemInfoList != null)
                    {
                        foreach (var recruitItemInfo in recruitShopInfo.recruitItemInfoList)
                        {
                            if(recruitItemInfo == null || recruitItemInfo.recruitRefObj == null)
                                continue;
                    
                            // 若招募需要的道具不在变化列表中, 则跳过
                            if(recruitItemInfo.recruitRefObj.cost_item != null && !_m_lChgItemList.Contains(recruitItemInfo.recruitRefObj.cost_item.item))
                                continue;

                            needChgRed = true;
                            // 若已经招募过, 则跳过
                            if(isHadRecruit(recruitItemInfo))
                                continue;
                    
                            if (GCommon.isItemEnough(recruitItemInfo.recruitRefObj.cost_item, false))
                            {
                                hasUnRecruitItem = true;
                                break;
                            }
                        }
                    }
                
                    if(needChgRed)
                        RedTipMgr.instance.setCountByRefRedTipId(recruitShopInfo.recruitShopRefObj.red_tip_id, hasUnRecruitItem ? 1 : 0);
                }
                
                _pushBackAllCommonItem();
            }
        }

        /// <summary>
        /// 刷新红点
        /// </summary>
        private void _refreshRedTipAll()
        {
            if (_m_lRecruitShopInfoList == null)
                return;
            
            foreach (var recruitShopInfo in _m_lRecruitShopInfoList)
            {
                if(recruitShopInfo == null || recruitShopInfo.recruitShopRefObj == null)
                    continue;

                if (recruitShopInfo.recruitItemInfoList == null)
                {
                    RedTipMgr.instance.setCountByRefRedTipId(recruitShopInfo.recruitShopRefObj.red_tip_id, 0);
                    continue;
                }
                
                bool hasUnRecruitItem = false;
                foreach (var recruitItemInfo in recruitShopInfo.recruitItemInfoList)
                {
                    if(recruitItemInfo == null || recruitItemInfo.recruitRefObj == null)
                        continue;
                    
                    if(isHadRecruit(recruitItemInfo))
                        continue;
                    
                    if (GCommon.isItemEnough(recruitItemInfo.recruitRefObj.cost_item, false))
                    {
                        hasUnRecruitItem = true;
                        break;
                    }
                }
                
                RedTipMgr.instance.setCountByRefRedTipId(recruitShopInfo.recruitShopRefObj.red_tip_id, hasUnRecruitItem ? 1 : 0);
            }
        }
        
        /// <summary>
        /// 放回所有通用道具对象
        /// </summary>
        private void _pushBackAllCommonItem()
        {
            foreach (var item in _m_lChgItemList)
            {
                _m_commonItemPool.Release(item);
            }
            _m_lChgItemList.Clear();
        }
        
        #endregion
        
        #region GC2GS

        /// <summary>
        /// 请求初始化协议
        /// </summary>
        private void _reqRecruitInfo()
        {
            NPGSClientListener.sendMsgByLog(GSWriter_002_InitOp.make_002_041_ReqRecruitInfo());
        }

        /// <summary>
        /// 请求招募
        /// </summary>
        public void reqRecruit(long _id, Action<GS2GC_007_028_RetRecruit> _dealDone, Action _dealFail)
        {
            NPGSClientListener.sendRequestByLog(GSWriter_007_CommOp.make_028_ReqRecruit(_id), 
                new CommonRequestCallbackProtocolDealer<GS2GC_007_028_RetRecruit>((_info) =>
                {
                    // 招募成功
                    _dealDone?.Invoke(_info);
                    
                    //发送消息
                    WinMsg.SendMsg(WinMsgType.ON_RECRUIT_EXCHANGE_SUCC, _id);
                }, (_error) =>
                {
                    NPGUIAddSceneCenterTip.instance.showErrorInfo(_error);
                    _dealFail?.Invoke();
                }));
        }

        #endregion

        #region GS2GC

        /// <summary>
        /// 初始化协议回包
        /// </summary>
        /// <param name="_msg"></param>
        public void retRecuritInfo(GS2GC_002_041_RetRecuritInfo _msg)
        {
            if (_msg == null)
            {
                Debug.LogError($"[RecruitComponent retRecuritInfo] _msg is null");
                setInitDone();
                return;
            }

            if (_msg.getHadRecuritIdList() != null)
            {
                foreach (var hadRecruitId in _msg.getHadRecuritIdList())
                {
                    _addRecruitId(hadRecruitId);
                }
            }
            
            setInitDone();
        }

        /// <summary>
        /// 新增已兑换数据
        /// </summary>
        /// <param name="_msg"></param>
        public void onRecuritRecordAdd(GS2GC_007_073_OnRecuritRecordAdd _msg)
        {
            if(_msg == null || !isInited)
                return;

            _addRecruitId(_msg.getRecuritId());
        }
        
        #endregion

        #region 消息监听

        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        private void _onCommonItemChg(params object[] _objs)
        {
            if (_objs == null || _objs.Length < 2 || !(_objs[0] is ENPItemType itemType) || !(_objs[1] is long subId))
                return;

            NPCommonItem chgCommonItem = _m_commonItemPool.Get();
            if(chgCommonItem == null)
                return;
            
            chgCommonItem.itemType = itemType;
            chgCommonItem.itemId = subId;
            _m_lChgItemList.Add(chgCommonItem);
            
            _createRefreshRedTipTask();
        }

        #endregion
    }
}