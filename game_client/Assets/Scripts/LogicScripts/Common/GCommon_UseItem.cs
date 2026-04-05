using ALPackage;
using Common.BagItemUseEnum;
using Common.BagItemUseObj;
using GS2GC.p006_BagItemOp;
using NPEnum;
using System;
using System.Collections.Generic;

namespace GOE
{
    public static partial class GCommon
    {
        // 使用物品
        public static void showBagUseItemsMono(ENPItemType _itemType, long _itemId, AccessAdditionData _additionData = null)
        {
            if (!isItemEnough(_itemType, _itemId, 1, true))
                return;

            BagItem bagItem = NPPlayer.instance.bagComp.getItem(_itemId);
            showBagUseItemsMono(bagItem, _additionData);
        }

        /// <summary>
        /// 批量使用道具 这里没有判断道具的批量使用条件
        /// </summary>
        /// <param name="_itemType">物品类型</param>
        /// <param name="_itemId">物品id</param>
        /// <param name="_useMaxCount">最大使用限制</param>
        /// <param name="_useAction">使用回调</param>
        /// <param name="_specialTitle">翻译后的标题</param>
        /// <param name="_specialDesc">翻译后的描述</param>
        public static void showBagBatchUse(ENPItemType _itemType, long _itemId, long _useMaxCount = -1, Action<long> _useAction = null, string _specialTitle = null, string _specialDesc = null, bool _forceShowTip = false)
        {
            if (!isItemEnough(_itemType, _itemId, 1, true))
                return;
            
            BagItem bagItem = NPPlayer.instance.bagComp.getItem(_itemId);

            //如果不能使用则返回
            if (null == bagItem || bagItem.itemUseRefObj == null || !NPPlayer.instance.bagComp.isItemCanUse(bagItem, 1))
                return;

            //批量使用
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndBagItemUse.instance, () =>
            {
                GGUIWndBagItemUse.instance.showWnd();
                GGUIWndBagItemUse.instance.init(bagItem, (_count) =>
                {
                    if (null != _useAction)
                    {
                        _useAction(_count);
                        return;
                    }

                    NPPlayer.instance.bagComp.reqUseItems(bagItem, _count, null, _msg =>
                    {
                        showUseItemResult(_msg,_forceShowTip);
                    });
                }, _useMaxCount, _specialTitle, _specialDesc);
            }, UINodeTagConst.C_ADD_Bag_UseItemNode);
        }

        // 使用物品-统一走这里  根据不同的道具类型枚举弹出不同的使用弹窗
        public static void showBagUseItemsMono(BagItem _item, AccessAdditionData _additionData = null, bool _forceShowTip = false)
        {
            if (null == _item)
                return;

            //如果不能使用则返回
            if (_item.itemUseRefObj == null || !NPPlayer.instance.bagComp.isItemCanUse(_item, 1))
                return;

            //如果物品有可选列表 展示多选一弹窗
            if (_item.itemUseRefObj.isNx)
            {
                QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndBagItemUseSelect.instance, () =>
                {
                    GGUIWndBagItemUseSelect.instance.showWnd();
                    GGUIWndBagItemUseSelect.instance.init(_item, (_useItem, _useCount, _selectIdList) =>
                    {
                        _useBagItem(_useItem, _useCount, (_count) => {
                            NPPlayer.instance.bagComp.reqUseItems(_useItem, _count, _selectIdList, (_msg) =>
                            {
                                QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_ADD_Bag_Item_Use_Select);
                                showUseItemResult(_msg, _forceShowTip);
                            });
                        });
                    });
                }, UINodeTagConst.C_ADD_Bag_Item_Use_Select);
            }
            //如果物品有奖励列表 弹出奖励展示弹窗
            else if (_item.itemUseRefObj.get_reward_id > 0)
            {
                QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndBagItemUsePercent.instance, () =>
                {
                    GGUIWndBagItemUsePercent.instance.showWnd();
                    GGUIWndBagItemUsePercent.instance.init(_item, (_useItem, _useCount) =>
                    {
                        _useBagItem(_useItem, _useCount, (_count) =>
                        {
                            NPPlayer.instance.bagComp.reqUseItems(_useItem, _count, null, (_msg) =>
                            {
                                QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_ADD_Bag_Item_Use_Percent);
                                showUseItemResult(_msg, _forceShowTip);
                            });
                        });
                     });
                }, UINodeTagConst.C_ADD_Bag_Item_Use_Percent);
            }
            //对骑士用的物品走骑士特殊弹窗
            else if (null != GRefdataCoreMgr.instance.bagItemHeroCore.getRef(_item.itemId))
            {
                //如果没有骑士弹出tip
                if (NPPlayer.instance.heroComponent.getTotalHeroCount() == 0)
                {
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.bag_noHeroTip_str);
                    return;
                }

                //如果骑士道具表
                BagItemHeroRefObj heroRefObj = GRefdataCoreMgr.instance.bagItemHeroCore.getRef(_item.itemId);
                if (null == heroRefObj)
                {
                    ALLog.Error($"bag_item_hero can not find id {_item.itemId}");
                    return;
                }

                //如果道具类型是选择骑士
                if (heroRefObj.select_type == EBagItemUse_TargetType.SELECT)
                {
                    //如果已经选择的骑士id
                    if (_additionData != null && _additionData.useItemSelectHeroId > 0)
                    {
                        _useBagItem(_item, _item.count, (_count) =>{
                            NPPlayer.instance.bagComp.reqBagUseItemForSelectHero(_additionData.useItemSelectHeroId, _item.itemId, _count,
                                _msg =>
                                {
                                    if(_msg == null)
                                        return;

                                    _showHeroConsortUseItemResult(_msg.getItemList());
                                }
                             );
                        },true);
                    }
                    else
                    {
                        QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndBagItemUseHero.instance, () =>
                        {
                            GGUIWndBagItemUseHero.instance.showWnd();
                            GGUIWndBagItemUseHero.instance.init(_item, (_useItem,_heroId) => {
                                _useBagItem(_useItem, _useItem.count, (_count) =>
                                {
                                    NPPlayer.instance.bagComp.reqBagUseItemForSelectHero(_heroId, _useItem.itemId, _count,
                                     _msg =>
                                     {
                                         if(_msg == null)
                                             return;

                                         _showHeroConsortUseItemResult(_msg.getItemList());
                                     }
                                 );
                                }, true);
                            });
                        }, UINodeTagConst.C_BAG_ITEM_USE_HERO);
                    }
                }
                //针对随机骑士使用物品
                else if (heroRefObj.select_type == EBagItemUse_TargetType.RAND)
                {
                    //是否有对应属性的伙伴
                    bool haveHero = false;
                    NPPlayer.instance.heroComponent.dealAllHero(_hero =>
                    {
                        if(_hero != null && _hero.specAttrType == heroRefObj.attr_type)
                            haveHero = true;
                    });
                    if (!haveHero)
                    {
                        //没有对应属性伙伴弹出tip
                        BasicAttrRefObj basicAttrRef = GRefdataCoreMgr.instance.basicAttrRefCore.getRef((long)heroRefObj.attr_type);
                        NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.bag_heroRandomUseFail_str,TextTranslate.instance.getLanguage(basicAttrRef?.name)));
                        return;
                    }

                    _useBagItem(_item,_item.count, (_count) =>
                    {
                        _reqUseRandomHeroItem(_item.itemId, _count);
                    },true);
                }
            }

            //对妃子用的物品走妃子特殊弹窗
            else if (null != GRefdataCoreMgr.instance.bagItemConsortCore.getRef(_item.itemId))
            {
                //如果没有妃子弹出tip
                if (NPPlayer.instance.consortComp.getConsortCount() == 0)
                {
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.bag_noConsortTip_str);
                    return;
                }

                //如果是给妃子使用的物品
                BagItemConsortRefObj consortRefObj = GRefdataCoreMgr.instance.bagItemConsortCore.getRef(_item.itemId);
                if (null == consortRefObj)
                {
                    ALLog.Error($"bag_item_consort can not find id {_item.itemId}");
                    return;
                }

                if (consortRefObj.select_type == EBagItemUse_TargetType.SELECT)
                {
                    QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndBagItemUseConsort.instance, () =>
                    {
                        GGUIWndBagItemUseConsort.instance.showWnd();
                        GGUIWndBagItemUseConsort.instance.init(_item, (_useItem, _consortId) => {
                            _useBagItem(_useItem, _useItem.count, (_count) =>
                            {
                                NPPlayer.instance.bagComp.reqBagUseItemForSelectConsort(_consortId, _useItem.itemId, (int)_count,
                                    _msg =>
                                    {
                                        if (_msg == null || _msg.getItemList() == null)
                                            return;

                                        _showHeroConsortUseItemResult(_msg.getItemList());
                                    });
                            }, true);
                        });
                    }, UINodeTagConst.C_BAG_ITEM_USE_CONSORT);
                }
                //针对随机妃子使用物品
                else if (consortRefObj.select_type == EBagItemUse_TargetType.RAND)
                {
                    _useBagItem(_item, _item.count, (_count) =>
                    {
                        _reqUseRandomConsortItem(_item.itemId, _count);
                    }, true);
                }
            }

            //其他物品根据能否批量使用走统一弹窗
            else
            {
                _useBagItem(_item, _item.count, (_count) =>
                {
                    NPPlayer.instance.bagComp.reqUseItems(_item, _count, null, (_msg) =>
                    {
                        showUseItemResult(_msg, _forceShowTip);
                    });
                }, true);
            }
        }

        /// <summary>
        /// 直接根据数量使用物品
        /// </summary>
        /// <param name="_item"></param>
        /// <param name="_count"></param>
        /// <param name="_additionData"></param>
        /// <param name="_onUseDone"></param>
        public static void useItemByCount(BagItem _item, long _count, AccessAdditionData _additionData = null, Action _onUseDone = null, bool _forceShowTip = false)
        {
            if (null == _item)
                return;

            //如果不能使用则返回
            if (_item.itemUseRefObj == null || !NPPlayer.instance.bagComp.isItemCanUse(_item, 1))
                return;

            //判断是否超出了最大值
            if (_count > GRefdataCoreMgr.instance.npGeneral.batch_use_item_max_count)
                _count = GRefdataCoreMgr.instance.npGeneral.batch_use_item_max_count;

            //判断数量是否超出了当前拥有数量
            if(_count > _item.count)
                _count = _item.count;

            //如果物品有可选列表 展示多选一弹窗
            if (_item.itemUseRefObj.isNx)
            {
                QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndBagItemUseSelect.instance, () =>
                {
                    GGUIWndBagItemUseSelect.instance.showWnd();
                    GGUIWndBagItemUseSelect.instance.init(_item, (_useItem, _useCount, _selectIdList) =>
                    {
                        _useBagItem(_useItem, _useCount, (_count) => {
                            NPPlayer.instance.bagComp.reqUseItems(_useItem, _count, _selectIdList, (_msg) =>
                            {
                                QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_ADD_Bag_Item_Use_Select);
                                showUseItemResult(_msg, _forceShowTip);
                                _onUseDone?.Invoke();
                            });
                        });
                    });
                }, UINodeTagConst.C_ADD_Bag_Item_Use_Select);
            }
            //对骑士用的物品走骑士特殊弹窗
            else if (null != GRefdataCoreMgr.instance.bagItemHeroCore.getRef(_item.itemId))
            {
                //如果没有骑士弹出tip
                if (NPPlayer.instance.heroComponent.getTotalHeroCount() == 0)
                {
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.bag_noHeroTip_str);
                    return;
                }

                BagItemHeroRefObj heroRefObj = GRefdataCoreMgr.instance.bagItemHeroCore.getRef(_item.itemId);
                if (null == heroRefObj)
                {
                    ALLog.Error($"bag_item_hero can not find id {_item.itemId}");
                    return;
                }
                //如果道具类型是选择骑士
                if (heroRefObj.select_type == EBagItemUse_TargetType.SELECT)
                {
                    //如果已经选择的骑士id
                    if (_additionData != null && _additionData.useItemSelectHeroId > 0)
                    {
                        _useBagItem(_item, _count, (_count) => {
                            NPPlayer.instance.bagComp.reqBagUseItemForSelectHero(_additionData.useItemSelectHeroId, _item.itemId, _count,
                                _msg =>
                                {
                                    if (_msg == null)
                                        return;

                                    _showHeroConsortUseItemResult(_msg.getItemList());
                                    _onUseDone?.Invoke();
                                }
                            );
                        });
                    }
                    else
                    {
                        QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndBagItemUseHero.instance, () =>
                        {
                            GGUIWndBagItemUseHero.instance.showWnd();
                            GGUIWndBagItemUseHero.instance.init(_item, (_useItem, _heroId) => {
                                _useBagItem(_useItem, _count, (_count) =>
                                {
                                    NPPlayer.instance.bagComp.reqBagUseItemForSelectHero(_heroId, _useItem.itemId, _count,
                                        _msg =>
                                        {
                                            if (_msg == null)
                                                return;

                                            _showHeroConsortUseItemResult(_msg.getItemList());
                                            _onUseDone?.Invoke();
                                        }
                                    );
                                });
                            });
                        }, UINodeTagConst.C_BAG_ITEM_USE_HERO);
                    }
                }
                //针对随机骑士使用物品
                else if (heroRefObj.select_type == EBagItemUse_TargetType.RAND)
                {
                    //是否有对应属性的伙伴
                    bool haveHero = false;
                    NPPlayer.instance.heroComponent.dealAllHero(_hero =>
                    {
                        if (_hero != null && _hero.specAttrType == heroRefObj.attr_type)
                            haveHero = true;
                    });
                    if (!haveHero)
                    {
                        //没有对应属性伙伴弹出tip
                        BasicAttrRefObj basicAttrRef = GRefdataCoreMgr.instance.basicAttrRefCore.getRef((long)heroRefObj.attr_type);
                        NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.bag_heroRandomUseFail_str, TextTranslate.instance.getLanguage(basicAttrRef?.name)));
                        return;
                    }

                    _useBagItem(_item, _count, (_count) =>
                    {
                        _reqUseRandomHeroItem(_item.itemId, _count, _onUseDone);
                    });
                }
            }
            //对妃子用的物品走妃子特殊弹窗
            else if (null != GRefdataCoreMgr.instance.bagItemConsortCore.getRef(_item.itemId))
            {
                //如果没有妃子弹出tip
                if (NPPlayer.instance.consortComp.getConsortCount() == 0)
                {
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.bag_noConsortTip_str);
                    return;
                }

                //如果是给妃子使用的物品
                BagItemConsortRefObj consortRefObj = GRefdataCoreMgr.instance.bagItemConsortCore.getRef(_item.itemId);
                if (null == consortRefObj)
                {
                    ALLog.Error($"bag_item_consort can not find id {_item.itemId}");
                    return;
                }
                if (consortRefObj.select_type == EBagItemUse_TargetType.SELECT)
                {
                    QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndBagItemUseConsort.instance, () =>
                    {
                        GGUIWndBagItemUseConsort.instance.showWnd();
                        GGUIWndBagItemUseConsort.instance.init(_item, (_useItem, _consortId) => {
                            _useBagItem(_useItem, _count, (_count) =>
                            {
                                NPPlayer.instance.bagComp.reqBagUseItemForSelectConsort(_consortId, _useItem.itemId, (int)_count,
                                    _msg =>
                                    {
                                        if (_msg == null || _msg.getItemList() == null)
                                            return;

                                        _showHeroConsortUseItemResult(_msg.getItemList());
                                        _onUseDone?.Invoke();
                                    });
                            });
                        });
                    }, UINodeTagConst.C_BAG_ITEM_USE_CONSORT);
                }
                //针对随机妃子使用物品
                else if (consortRefObj.select_type == EBagItemUse_TargetType.RAND)
                {
                    _useBagItem(_item, _count, (_count) =>
                    {
                        _reqUseRandomConsortItem(_item.itemId, _count, _onUseDone);
                    });
                }
            }
            //其他物品根据能否批量使用走统一弹窗
            else
            {
                _useBagItem(_item, _count, (_count) =>
                {
                    NPPlayer.instance.bagComp.reqUseItems(_item, _count,null, (_msg) =>
                    {
                        showUseItemResult(_msg, _forceShowTip);
                        _onUseDone?.Invoke();
                    });
                });
            }
        }

        //伙伴家人使用道具完成界面
        private static void _showHeroConsortUseItemResult(List<BagItemUse_ConsortShowInfo> _infoList)
        {
            if (_infoList == null || _infoList.Count == 0)
                return;

            //合并数据
            Dictionary<EBagItemUse_ConsortDrawShowType, List<BagItemUse_ConsortShowInfo>> targetInfoDic = new Dictionary<EBagItemUse_ConsortDrawShowType, List<BagItemUse_ConsortShowInfo>>();
            for (int i = 0; i < _infoList.Count; i++)
            {
                BagItemUse_ConsortShowInfo temp = _infoList[i];
                if (null == temp)
                    continue;

                if (targetInfoDic.TryGetValue(temp.getType(), out List<BagItemUse_ConsortShowInfo> _showList))
                {
                    bool isFind = false;
                    for (int j = 0; j < _showList.Count; j++)
                    {
                        if (_showList[j].getType() == temp.getType() &&
                            _showList[j].getConsortId() == temp.getConsortId())
                        {
                            isFind = true;
                            _showList[j].setCount(_showList[j].getCount() + temp.getCount());
                            break;
                        }
                    }
                    if (!isFind)
                        _showList.Add(temp);
                }
                else
                {
                    List<BagItemUse_ConsortShowInfo> tempList = new List<BagItemUse_ConsortShowInfo>();
                    tempList.Add(temp);
                    targetInfoDic[temp.getType()] = tempList;
                }
            }

            foreach (KeyValuePair<EBagItemUse_ConsortDrawShowType, List<BagItemUse_ConsortShowInfo>> keyValuePair in targetInfoDic)
            {
                NPUINoticeMgr.instance.addDealer(new NoticeDealer_BagItemHeroConsortUseResult(keyValuePair.Key, keyValuePair.Value));
            }
        }

        //伙伴家人使用道具完成界面
        private static void _showHeroConsortUseItemResult(List<BagItemUse_HeroShowInfo> _infoList)
        {
            if (_infoList == null || _infoList.Count == 0)
                return;

            //合并数据
            Dictionary<EBagItemUse_HeroDrawShowType, List<BagItemUse_HeroShowInfo>> targetInfoDic = new Dictionary<EBagItemUse_HeroDrawShowType, List<BagItemUse_HeroShowInfo>>();
            for (int i = 0; i < _infoList.Count; i++)
            {
                BagItemUse_HeroShowInfo temp = _infoList[i];
                if (null == temp)
                    continue;

                if (targetInfoDic.TryGetValue(temp.getType(), out List<BagItemUse_HeroShowInfo> _showList))
                {
                    bool isFind = false;
                    for (int j = 0; j < _showList.Count; j++)
                    {
                        if (_showList[j].getType() == temp.getType() &&
                            _showList[j].getHeroId() == temp.getHeroId())
                        {
                            isFind = true;
                            _showList[j].setCount(_showList[j].getCount() + temp.getCount());
                            break;
                        }
                    }
                    if (!isFind)
                        _showList.Add(temp);
                }
                else
                {
                    List<BagItemUse_HeroShowInfo> tempList = new List<BagItemUse_HeroShowInfo>();
                    tempList.Add(temp);
                    targetInfoDic[temp.getType()] = tempList;
                }
            }

            foreach (KeyValuePair<EBagItemUse_HeroDrawShowType, List<BagItemUse_HeroShowInfo>> keyValuePair in targetInfoDic)
            {
                NPUINoticeMgr.instance.addDealer(new NoticeDealer_BagItemHeroConsortUseResult(keyValuePair.Key, keyValuePair.Value));
            }
        }

        /// <summary>
        /// 使用物品
        /// </summary>
        /// <param name="_item"></param>
        /// <param name="_count"></param>
        /// <param name="_reqAction"></param>
        /// <param name="_isShowBatch">是否弹出批量选择弹窗</param>
        private static void _useBagItem(BagItem _item, long _count, Action<long> _reqAction,bool _isShowBatch = false)
        {
            if (null == _item || null == _item.itemUseRefObj || null == _reqAction)
                return;

            //物品不足弹获取途径
            if (!isItemEnough(_item.itemType, _item.itemId, 1, true))
                return;

            //如果有客户端条件 先执行
            Action effectUse = () =>
            {
                if (null != _item.itemUseRefObj.c_effects && !_item.itemUseRefObj.c_effects.isEmpty)
                {
                    _item.itemUseRefObj.c_effects.dealEffect();
                }
            };

            if (_isShowBatch)
            {
                // 不能批量使用则直接请求使用物品 
                if (_item.itemUseRefObj.can_batch_use == false ||
                    _item.count < GRefdataCoreMgr.instance.npGeneral.bag_auto_use_max_count)
                {
                    effectUse();
                    _reqAction(1);
                }
                else
                {
                    //显示实际消耗的使用弹窗
                    if (null != _item.itemUseRefObj.real_gain_item_count && null != _item.itemUseRefObj.real_gain_item_count.variable && _item.itemUseRefObj.real_gain_item_count.variable.hasVariable())
                    {
                        QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndBagItemUseShowRealGainItem.instance, () =>
                        {
                            GGUIWndBagItemUseShowRealGainItem.instance.showWnd();
                            GGUIWndBagItemUseShowRealGainItem.instance.init(_item, (_useCount) =>
                            {
                                effectUse();
                                _reqAction(_useCount);
                            });
                        }, UINodeTagConst.C_ADD_Bag_UseShowRealGainItemNode);
                    }
                    else
                    {
                        //批量使用
                        QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndBagItemUse.instance, () =>
                        {
                            GGUIWndBagItemUse.instance.showWnd();
                            GGUIWndBagItemUse.instance.init(_item, (_useCount) =>
                            {
                                effectUse();
                                _reqAction(_useCount);
                            });
                        }, UINodeTagConst.C_ADD_Bag_UseItemNode);
                    }
                }
            }
            else
            {
                effectUse();
                _reqAction(_count);
            }
        }

        //针对随机妃子使用物品
        private static void _reqUseRandomConsortItem(long _itemId, long _count, Action _onDone = null)
        {
            NPPlayer.instance.bagComp.reqBagUseItemForSelectConsort(0, _itemId, (int)_count, _msg =>
            {
                if (null == _msg || null == _msg.getItemList())
                    return;

                _showHeroConsortUseItemResult(_msg.getItemList());
                _onDone?.Invoke();
            });
        }

        //针对随机骑士使用物品
        private static void _reqUseRandomHeroItem(long _itemId, long _count, Action _onDone = null)
        {
            NPPlayer.instance.bagComp.reqBagUseItemForSelectHero(0, _itemId, (int)_count,
            _msg =>
            {
                if (null == _msg || null == _msg.getItemList())
                    return;

                _showHeroConsortUseItemResult(_msg.getItemList());
                _onDone?.Invoke();
            });
        }

        /// <summary>
        /// 使用物品结果展示
        /// </summary>
        /// <param name="_msg">回包数据</param>
        /// <param name="_forceShowTip">是否强制显示tip</param>
        public static void showUseItemResult(GS2GC_006_002_RetBagUseItem _msg, bool _forceShowTip)
        {
            if (_msg == null || _msg.getItemList() == null)
                return;
            
            BagItemUseRefObj bagItemUseRef = GRefdataCoreMgr.instance.bagItemUseCore.getRef(_msg.getItemId());
            if (bagItemUseRef == null)
                return;

            long useCount = _msg.getCount();
            //获取奖励展示类型
            ENpRewardShowType showType = useCount >= GRefdataCoreMgr.instance.npGeneral.bag_auto_use_max_count ? bagItemUseRef.tip_reward_multiple : bagItemUseRef.tip_reward;
            
            if (_forceShowTip && showType != ENpRewardShowType.NOT_DISPLAY)
                GCommon.showGainRewardTip(_msg.getItemList(), true);
            else
            {
                //走统一函数处理
                switch (showType)
                {
                    case ENpRewardShowType.TIP:
                        GCommon.showGainRewardTip(_msg.getItemList(), true);
                        break;
                    case ENpRewardShowType.DEFAULT:
                        GCommon.dealGainItem(_msg.getItemList());
                        break;
                    case ENpRewardShowType.NOT_DISPLAY:
                        //不表现
                        break;
                    default:
                        GCommon.dealGainItem(_msg.getItemList());
                        break;
                }
            }
        }
    }
}