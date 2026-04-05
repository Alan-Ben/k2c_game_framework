using System.Collections.Generic;
using System;
using ALPackage;
using ClientEnum;
using GS2GC.p006_BagItemOp;
using NPEnum;

namespace GOE
{
    // 背包管理类
    public class PlayerBagComponent : _ANPBasicPlayerComponent
    {

        #region override 属性
        public override bool isMustInit { get { return true; } }

        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.BAG; } }

        public override ENPPlayerCompType[] dependCompList { get { return null; } }
        #endregion

        private List<BagItem> _m_lItemList = new List<BagItem>();   // 物品列表

        //构造函数
        public PlayerBagComponent(NPPlayerComponentMgr _compMgr) : base(_compMgr)
        {
        }
        #region override 方法
        /// <summary>
        /// 是否允许提前初始化。提前初始化的意思是在依赖项没有完成初始化之前就进行初始化操作（一般是提前发送消息）
        /// 在初始化结果消息返回的时候，通过特殊的初始化函数dealPreInitFunc进行处理函数注册，再依赖项完成之后才进行初始化处理
        /// </summary>
        public override bool canPreInit { get { return true; } }

        /// <summary>
        /// 发送初始化协议提前申请内容
        /// </summary>
        public override void presendInitProtocol()
        {
            //请求列表
            reqItemList();
        }

        protected override void _dealInit()
        {
        }

        //组件加载完成时的调用
        protected override void _onInitDone()
        {
        }

        //组件初始化失败的处理
        protected override void _onInitFail()
        {
            ALLog.Error("PlayerBagComponent init Fail!!!");
        }
        //释放资源函数
        protected override void _discard()
        {
            clear();
        }

        //所有组件初始化完以后
        public override void onAllCompInited()
        {
            refreshRedTip();
            _initRedTipSetting();
        }

        #endregion

        public void clear()
        {
            _m_lItemList.Clear();

            BagRedTipMgr.instance.clear();
        }

        /// <summary>
        /// 设置背包已经被查看过
        /// </summary>
        public void setBagViewed()
        {
            //同步服务器
            NPGSClientListener.sendMsg(NPGSWriter_006_BagItemOp.make_003_ReqRefreshQuitBagTime());
            //直接设置本地参数
            NPPlayer.instance.playerInfo.updatePlayerParam((int)ENPPlayerParam.LAST_LEFT_BAG_TIME, FpsAndPingMgr.instance.serverTimeTagS);
        }

        /// <summary>
        /// 设置物品被点击过
        /// </summary>
        public void setItemIsViewed(long _itemId)
        {
            BagItem _item = getItem(_itemId);
            if (null != _item)
                _item.updateClickTimeS((int)FpsAndPingMgr.instance.serverTimeTagS);
            //同步服务器
            NPGSClientListener.sendMsgByLog(NPGSWriter_006_BagItemOp.make_004_ReqClickBagTime(_itemId));
        }


        /// <summary>
        /// 移除物品
        /// </summary>
        /// <param name="_itemId"></param>
        public void removeItem(long _itemId)
        {
            BagItem tmpItem = null;
            for (int i = 0; i < _m_lItemList.Count; i++)
            {
                tmpItem = _m_lItemList[i];
                if (null == tmpItem || null == tmpItem.itemRefObj)
                    continue;

                if (tmpItem.itemRefObj.id == _itemId)
                {
                    _m_lItemList.RemoveAt(i);

                    tmpItem.updateCount(0);

                    // 更新小红点
                    BagRedTipMgr.instance.onRemoveItem(tmpItem);
                    //事件广播
                    WinMsg.SendMsg(WinMsgType.ON_BAG_ITEM_REMOVE, tmpItem);

                    //发送消息刷新自定义加载prefab
                    GCommon.reloadCustomLoadPrefab();
                    return;
                }
            }
        }

        /// <summary>
        /// 获取背包数据
        /// </summary>
        /// <param name="_getList"></param>
        public void getItemList(List<BagItem> _getList)
        {
            if (null == _getList)
                return;
            _getList.AddRange(_m_lItemList);
        }

        /// <summary>
        /// 获取背包指定类型数据
        /// </summary>
        /// <param name="_type"></param>
        /// <param name="_getList"></param>
        public void getItemList(ENPBagItemType _type, List<BagItem> _getList)
        {
            if (null == _getList)
                return;

            // 遍历查找物品
            BagItem item = null;
            for (int i = 0, count = _m_lItemList.Count; i < count; i++)
            {
                item = _m_lItemList[i];
                if (null == item)
                    continue;
                if (item.bagItemType == _type)
                    _getList.Add(item);
            }
        }

        /// <summary>
        /// 查找物品
        /// </summary>
        public BagItem getItem(long _itemId)
        {
            // 遍历查找物品
            BagItem item = null;
            for (int i = 0, count = _m_lItemList.Count; i < count; i++)
            {
                item = _m_lItemList[i];
                if (null == item)
                    continue;
                if (item.itemId == _itemId)
                    return item;
            }
            return null;
        }

        /// <summary>
        /// 根据条件获取物品列表
        /// </summary>
        /// <param name="_resultList"></param>
        /// <param name="_match"></param>
        /// <param name="_includeZero"></param>
        public void addRangeItemList(List<BagItem> _resultList, Predicate<BagItem> _match, bool _includeZero = false)
        {
            if (_resultList == null)
                return;

            //不需要匹配条件，就把所有物品加到结果列表
            if (_match == null && _includeZero)
            {
                _resultList.AddRange(_m_lItemList);
            }
            else
            {
                BagItem tmpBagItem = null;
                for (int i = 0; i < _m_lItemList.Count; ++i)
                {
                    tmpBagItem = _m_lItemList[i];
                    if (tmpBagItem == null)
                        continue;

                    // 判断是否过滤数量为0的物品
                    if (!_includeZero && tmpBagItem.count == 0)
                        continue;

                    if (_match == null)
                    {
                        _resultList.Add(tmpBagItem);
                    }
                    else if (_match(tmpBagItem))
                    {
                        _resultList.Add(tmpBagItem);
                    }
                }
            }
        }

        public void dealAllItem(Action<BagItem> _dealFunc)
        {
            if(_m_lItemList == null || _dealFunc == null)
                return;

            _m_lItemList.ForEach(_dealFunc);
        }
        
        /// <summary>
        /// 获取物品的数量
        /// </summary>
        public long getItemCount(long _itemId)
        {
            // 遍历查找物品
            BagItem item = getItem(_itemId);
            if (null == item)
                return 0L;
            return item.count;
        }

        /// <summary>
        /// 判断能否使用/// </summary>
        /// <param name="_item"></param>
        /// <param name="_useCount">使用数量</param>
        /// <param name="showTips">是否展示tip</param>
        /// <returns></returns>
        public bool isItemCanUse(BagItem _item, long _useCount, bool showTips = true)
        {
            if (null == _item)
                return false;
            
            if (null == _item.itemUseRefObj)
            {
                return _useCount <= _item.count;
            }

            // 判断是否符合使用条件
            if (!_item.itemUseRefObj.use_cond.IsEnable(null))
            {
                if (showTips)
                    // 提示不符使用条件
                    NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(_item.itemUseRefObj.cond_not_fix_tip, _item.itemUseRefObj.cond_not_fix_tip_args));
                return false;
            }

            if (_useCount > _item.count)
            {
                return false;
            }

            if (_item.count <= 0)
            {
                if (showTips)
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.bag_itemNotEnough_none);
                return false;
            }

            // 判断是否满足资源消耗条件
            List<NPCommonCostItem> tempList = _item.itemUseRefObj.cost_item_list;
            if (_useCount > 1)
            {
                tempList = new List<NPCommonCostItem>();
                NPCommonCostItem temp = null;
                for (int i = 0; i < _item.itemUseRefObj.cost_item_list.Count; i++)
                {
                    temp = _item.itemUseRefObj.cost_item_list[i];
                    if (null == temp)
                        continue;
                    NPCommonCostItem item = new NPCommonCostItem(temp.item, temp.count * _useCount);
                    tempList.Add(item);
                }
            }
            if (!GCommon.isItemEnough(tempList, false))
            {
                if (showTips)
                    NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(_item.itemUseRefObj.cost_not_fix_tip, _item.itemUseRefObj.cost_not_fix_tip_args));
                return false;
            }
            return true;
        }


        /// <summary>
        /// 获取物品是否可合成
        /// </summary>
        /// <param name="_item"></param>
        /// <returns></returns>
        public bool judgeItemCanCombineMaxCount(BagItem _item)
        {
            if (_item == null)
                return false;

            return GCommon.getItemCanCombineMaxCount(_item.itemId) > 0;
        }

        #region 红点相关

        //刷新红点
        public void refreshRedTip()
        {
            long count = 0;

            //是否有可被合成的物品
            GRefdataCoreMgr.instance.itemConvertCore.dealAllRef(_ref =>
            {
                if (_ref != null && _ref.target_item != null && _ref.target_item.itemType == ENPItemType.BAG_ITEM && GCommon.isItemCanBeCombine(_ref.target_item.itemType, _ref.target_item.itemId))
                {
                    BagItem bagItem = getItem(_ref.bag_item_id);
                    if(bagItem != null && (bagItem.isNew || bagItem.isNewAddItem) && bagItem.itemRefObj != null && !bagItem.itemRefObj.is_hiding)
                        count++;
                }
            });
            _ARedTipNode convertNode = RedTipMgr.instance.getNodeByRefRedTipId(RedTipConst.RED_ITEM_CONVERT_PAGE);
            RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_ITEM_CONVERT_PAGE, convertNode != null && convertNode.needShow() ? 1 : count);

            //可使用页签是否有新物品
            count = 0;
            if (_m_lItemList != null)
            {
                for (int i = 0; i < _m_lItemList.Count; i++)
                {
                    if(_m_lItemList[i] == null || _m_lItemList[i].itemRefObj == null)
                        continue;

                    if (_m_lItemList[i].itemRefObj.show_type == EBagClickShowView.USE && !_m_lItemList[i].itemRefObj.is_hiding && (_m_lItemList[i].isNew || _m_lItemList[i].isNewAddItem))
                    {
                        count++;
                    }
                }
            }
            RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_ITEM_CAN_USE_PAGE, count);

            //道具页签是否有新物品
            count = 0;
            if (_m_lItemList != null)
            {
                for (int i = 0; i < _m_lItemList.Count; i++)
                {
                    if (_m_lItemList[i] == null || _m_lItemList[i].itemRefObj == null)
                        continue;

                    if (_m_lItemList[i].itemRefObj.show_type != EBagClickShowView.USE && !_m_lItemList[i].itemRefObj.is_hiding && (_m_lItemList[i].isNew || _m_lItemList[i].isNewAddItem))
                    {
                        count++;
                    }
                }
            }
            RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_ITEM_PAGE, count);
        }

        //初始化窗口红点设置
        private void _initRedTipSetting()
        {
            for (int i = 0; i < _m_lItemList.Count; i++)
            {
                if (_m_lItemList[i] == null)
                    continue;

                //如果已经查看过，但本地缓存还是标记为新的或者新增的，则更新状态
                if (AccountSettingMgr.instance.bagItemWndRedTipSaver.getCanShowRedTip(_m_lItemList[i].itemId, EBagItemRedTipType.NEW) && !_m_lItemList[i].isNew)
                    AccountSettingMgr.instance.bagItemWndRedTipSaver.setReadRedTipByType(_m_lItemList[i].itemId, EBagItemRedTipType.NEW);
                if (AccountSettingMgr.instance.bagItemWndRedTipSaver.getCanShowRedTip(_m_lItemList[i].itemId, EBagItemRedTipType.ADD) && !_m_lItemList[i].isNewAddItem)
                    AccountSettingMgr.instance.bagItemWndRedTipSaver.setReadRedTipByType(_m_lItemList[i].itemId, EBagItemRedTipType.ADD);

                //记录新的或者新增的物品
                if (_m_lItemList[i] != null && _m_lItemList[i].itemRefObj != null && !_m_lItemList[i].itemRefObj.is_hiding && (_m_lItemList[i].isNew || _m_lItemList[i].isNewAddItem))
                    AccountSettingMgr.instance.bagItemWndRedTipSaver.recordItem(_m_lItemList[i]);
            }
        }

        #endregion

        #region S2C

        // 初始化物品列表
        public void retBagItemList(List<NPCommon.NPCommon_BagItemInfo> _infoList)
        {

            if (_infoList == null)
            {
                //即便加载失败,也要设置完成进入游戏
                setInitDone();
                return;
            }

            _m_lItemList.Clear();

            // 重置小红点
            BagRedTipMgr.instance.clear();

            BagItem temp = null;
            for (int i = 0, count = _infoList.Count; i < count; i++)
            {
                // 物品对象
                temp = new BagItem(_infoList[i]);
                if (null == temp || null == temp.itemRefObj)
                    continue;

                _m_lItemList.Add(temp);

                // 更新小红点
                if (temp.isNew || temp.isNewAddItem)
                    BagRedTipMgr.instance.onAddItem(temp);
            }

            //初始化完成，设置状态
            setInitDone();
        }

        #region 物品增删改
        /// <summary>
        /// 更新物品 (包括:新增物品、更新物品)
        /// </summary>
        /// <param name="_info"></param>
        public void updateItem(NPCommon.NPCommon_BagItemInfo _info)
        {
            BagItem target = getItem(_info.getItemId());

            // 若此物品存在,则视为更新物品
            if (target != null)
            {
                bool addCount = _info.getItemCount() - target.count > 0;

                // 更新数量
                target.updateCount(_info.getItemCount());

                // 物品数量增加且需要红点
                if (addCount && target.itemRefObj.need_redtip_when_add)
                {
                    // 更新物品获取的时间
                    target.updateGetTime(_info.getLastGetTimeS());
                    //管理器增加红点
                    BagRedTipMgr.instance.onAddItem(target);
                }

                // 刷新红点
                refreshRedTip();
                // 记录窗口红点状态
                AccountSettingMgr.instance.bagItemWndRedTipSaver?.recordItem(target);

                if (addCount)
                    WinMsg.SendMsg(WinMsgType.ON_BAG_ITEM_COUNT_ADD, target);
                //事件广播
                WinMsg.SendMsg(WinMsgType.ON_BAG_ITEM_UPDATE, target);

                //发送消息刷新自定义加载prefab
                GCommon.reloadCustomLoadPrefab();
            }
            // 否则视为新增物品
            else
            {
                target = new BagItem(_info);

                if (target.itemRefObj == null)
                    return;

                //如果该物品是自动使用
                if (null != target.itemUseRefObj && target.itemUseRefObj.is_auto_use)
                {
                    reqUseItems(target, target.count, null, _msg =>
                    {
                        GCommon.showUseItemResult(_msg, false);
                    });
                    return;
                }

                _m_lItemList.Add(target);

                // 管理器添加小红点
                BagRedTipMgr.instance.onAddItem(target);
                
                // 刷新红点
                refreshRedTip();
                // 记录窗口红点状态
                AccountSettingMgr.instance.bagItemWndRedTipSaver?.recordItem(target);

                WinMsg.SendMsg(WinMsgType.ON_BAG_ITEM_COUNT_ADD, target);
                //事件广播
                WinMsg.SendMsg(WinMsgType.ON_BAG_ITEM_ADD, target);

                //发送消息刷新自定义加载prefab
                GCommon.reloadCustomLoadPrefab();
            }
        }
        #endregion

        #endregion


        #region C2S

        // 请求物品列表
        public void reqItemList()
        {
            NPGSClientListener.sendMsgByLog(GSWriter_002_InitOp.make_007_ReqBagItemList());
        }

        //关闭输入序列
        private int _m_inputMaskSerialize;
        // 请求使用物品
        public void reqUseItems(BagItem _item, long _count, List<int> _selectedIdxList = null, Action<GS2GC_006_002_RetBagUseItem> _sucAction = null)
        {
            if (null == _item || _count <= 0 || !isItemCanUse(_item, _count))
                return;

            //避免多次请求，先打开遮罩
            _m_inputMaskSerialize = MainCameraMono.selfInstance.openAllInputMask();
            // 发送使用请求
            NPGSClientListener.sendRequestByLog(NPGSWriter_006_BagItemOp.make_002_ReqBagUseItem(_item.itemId, (int)_count, _selectedIdxList),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_006_002_RetBagUseItem>(
                    (_isSuc, _msg) =>
                    {
                        MainCameraMono.selfInstance.closeAllInputMask(_m_inputMaskSerialize);

                        if (_isSuc)
                            _sucAction?.Invoke(_msg);
                    }));
        }

        /// <summary>
        /// 请求合成兑换物品
        /// </summary>
        /// <param name="_bagItemId"></param>
        /// <param name="_bagItemCount"></param>
        public void reqItemConvert(long _bagItemId, long _bagItemCount, Action _sucAction)
        {
            NPGSClientListener.sendRequestByLog(NPGSWriter_006_BagItemOp.make_007_ReqConvert(_bagItemId, _bagItemCount),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_006_007_RetConvert>((info) =>
                {
                    if (null == info)
                        return;

                    if (_sucAction != null)
                        _sucAction();
                }));
        }

        public void reqItemOnceCombine(List<BagOnceCombineItem> _list)
        {
            NPGSClientListener.sendRequestByLog(NPGSWriter_006_BagItemOp.make_008_ReqAKeyConvert(_onceCombineToSingleConvert(_list)),
               new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_006_008_RetAKeyConvert>((info) =>
               {
                   if (null == info)
                       return;
               }));
        }

        private List<NPCommon.NPCommon_SingleItemConvert> _onceCombineToSingleConvert(List<BagOnceCombineItem> _list)
        {
            if (null == _list)
                return null;

            List<NPCommon.NPCommon_SingleItemConvert> convertList = new List<NPCommon.NPCommon_SingleItemConvert>();
            BagOnceCombineItem temp = null;
            for (int i = 0; i < _list.Count; i++)
            {
                temp = _list[i];
                if (null == temp)
                    continue;
                convertList.Add(temp.toSingleConvert());
            }

            return convertList;
        }

        /// <summary>
        /// 请求对骑士使用物品 随机使用heroid传0
        /// </summary>
        /// <param name="_heroId"></param>
        /// <param name="_itemId"></param>
        /// <param name="_count"></param>
        public void reqBagUseItemForSelectHero(long _heroId, long _itemId, long _count, Action<GS2GC_006_009_RetBagUseItemForSelectHero> _callback)
        {
            TipQueueMgr.instance.pauseShowType(ETipQueueType.NATION_POWER);
            NPGSClientListener.sendRequestByLog(NPGSWriter_006_BagItemOp.make_009_ReqBagUseItemForSelectHero(_heroId, _itemId, _count),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_006_009_RetBagUseItemForSelectHero>((info) =>
                {
                    if (null == info)
                        return;

                    TipQueueMgr.instance.resumeShowType(ETipQueueType.NATION_POWER);
                    if (_callback != null)
                        _callback(info);
                }));
        }
        /// <summary>
        /// 请求对妃子使用物品
        /// </summary>
        /// <param name="_consortId"></param>
        /// <param name="_itemId"></param>
        /// <param name="_count"></param>
        public void reqBagUseItemForSelectConsort(long _consortId, long _itemId, int _count, Action<GS2GC_006_010_RetBagUseItemForSelectConsort> _callback)
        {
            NPGSClientListener.sendRequestByLog(NPGSWriter_006_BagItemOp.make_010_ReqBagUseItemForSelectConsort(_consortId, _itemId, _count),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_006_010_RetBagUseItemForSelectConsort>((info) =>
                {
                    if (null == info)
                        return;

                    if (_callback != null)
                        _callback(info);
                }));
        }
        #endregion

    }
}
