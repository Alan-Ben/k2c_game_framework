using System;
using System.Collections.Generic;
using ALPackage;
using ChatPackage;
using Common.BagItemUseEnum;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public partial class GConsortComponent
    {
        private class RedTipDealer
        {
            private GConsortComponent _m_component;

            [NotNull] private Dictionary<string, _ARedTipNode> _m_dRefRedTipDic = new Dictionary<string, _ARedTipNode>();//红点数字典
            private ConsortRedTipSaver _m_redTipSaver;//红点存储器
            [NotNull] private Dictionary<long, Action> _m_dSimpleUnlockTriggerDic = new Dictionary<long, Action>();//功能解锁消息监听字典
            
            public RedTipDealer(GConsortComponent _component)
            {
                _m_component = _component;
            }

            /// <summary>
            /// 初始化聊天红点
            /// </summary>
            public void init()
            {
                _m_redTipSaver = new ConsortRedTipSaver();
                _m_redTipSaver.init();
                
                _initSendGiftRedTip();
                _regSimpleUnlockMsg();
                _refreshRed();
                
                // WinMsg.RegisterMsgAct(WinMsgType.ON_CONSORT_SKIN_INFO_ADD, _refreshRed);
                // WinMsg.RegisterMsgAct(WinMsgType.ON_CONSORT_SKIN_INFO_CHG, _refreshRed);
                // WinMsg.RegisterMsgAct(WinMsgType.ON_CONSORT_SKILL_INFO_ADD, _refreshRed);
                // WinMsg.RegisterMsgAct(WinMsgType.ON_CONSORT_SKILL_INFO_CHG, _refreshRed);
                // WinMsg.RegisterMsgAct(WinMsgType.ON_CONSORT_SKILL_POINT_CHG, _refreshRed);
                
                WinMsg.RegisterMsg(WinMsgType.ON_CONSORT_ADD, _onConsortAdd);
                WinMsg.RegisterMsg(WinMsgType.ON_CONSORT_INTIMACY_CHG, _onConsortIntimacyChg);
                WinMsg.RegisterMsg(WinMsgType.ON_CONSORT_CHARM_CHG, _onConsortCharmChg);
                WinMsg.RegisterMsg(WinMsgType.ON_CONSORT_CHARM_POINT_CHG, _onConsortCharmPointChg);
                WinMsg.RegisterMsgAct(WinMsgType.ON_LAZY_CD_CHG, _refreshCanInviteRed);
                WinMsg.RegisterMsg(WinMsgType.ON_CONSORT_BUSINESS_SKILL_INFO_CHG, _onConsortBusinessSkillChg);
                WinMsg.RegisterMsg(WinMsgType.ON_CONSORT_BLESS_SKILL_INFO_CHG, _onConsortBlessSkillChg);
                WinMsg.RegisterMsg(WinMsgType.ON_CONSORT_FETTER_LEVEL_CHG, _onConsortFetterLevelChg);
                WinMsg.RegisterMsg(WinMsgType.ON_CONSORT_CG_CHG, _onConsortCgChg); // 注册CG变化监听
                WinMsg.RegisterMsg(WinMsgType.ON_CONSORT_ADD_CG, _onConsortAddCg); // 注册CG新增监听
                WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_COUNT_ADD, _onBagItemCountAdd);
            }

            public void clear()
            {
                _unRegSimpleUnlockMsg();
                _m_dRefRedTipDic.Clear();
                
                // WinMsg.UnregisterMsgAct(WinMsgType.ON_CONSORT_ADD, _refreshRed);
                // WinMsg.UnregisterMsgAct(WinMsgType.ON_CONSORT_SKIN_INFO_ADD, _refreshRed);
                // WinMsg.UnregisterMsgAct(WinMsgType.ON_CONSORT_SKIN_INFO_CHG, _refreshRed);
                // WinMsg.UnregisterMsgAct(WinMsgType.ON_CONSORT_SKILL_INFO_ADD, _refreshRed);
                // WinMsg.UnregisterMsgAct(WinMsgType.ON_CONSORT_SKILL_INFO_CHG, _refreshRed);
                // WinMsg.UnregisterMsgAct(WinMsgType.ON_CONSORT_SKILL_POINT_CHG, _refreshRed);
                
                WinMsg.UnregisterMsg(WinMsgType.ON_CONSORT_ADD, _onConsortAdd);
                WinMsg.UnregisterMsg(WinMsgType.ON_CONSORT_INTIMACY_CHG, _onConsortIntimacyChg);
                WinMsg.UnregisterMsg(WinMsgType.ON_CONSORT_CHARM_CHG, _onConsortCharmChg);
                WinMsg.UnregisterMsg(WinMsgType.ON_CONSORT_CHARM_POINT_CHG, _onConsortCharmPointChg);
                WinMsg.UnregisterMsgAct(WinMsgType.ON_LAZY_CD_CHG, _refreshCanInviteRed);
                WinMsg.UnregisterMsg(WinMsgType.ON_CONSORT_BUSINESS_SKILL_INFO_CHG, _onConsortBusinessSkillChg);
                WinMsg.UnregisterMsg(WinMsgType.ON_CONSORT_BLESS_SKILL_INFO_CHG, _onConsortBlessSkillChg);
                WinMsg.UnregisterMsg(WinMsgType.ON_CONSORT_FETTER_LEVEL_CHG, _onConsortFetterLevelChg);
                WinMsg.UnregisterMsg(WinMsgType.ON_CONSORT_CG_CHG, _onConsortCgChg); // 注销CG变化监听
                WinMsg.UnregisterMsg(WinMsgType.ON_CONSORT_ADD_CG, _onConsortAddCg); // 注销CG新增监听
                WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_COUNT_ADD, _onBagItemCountAdd);

                _m_redTipSaver = null;
            }

            /// <summary>
            /// 注册功能解锁消息监听(模仿RedTipMgr._regSimpleUnlockMsg)
            /// </summary>
            private void _regSimpleUnlockMsg()
            {
                _m_dSimpleUnlockTriggerDic.Clear();
                
                // 经营技能红点监听
                _regSimpleUnlockMsgByRedId(RedTipConst.RED_CONSORT_BUSINESS_SKILL);
                // 加护技能红点监听
                _regSimpleUnlockMsgByRedId(RedTipConst.RED_CONSORT_BLESS_SKILL);
            }

            /// <summary>
            /// 根据红点ID注册功能解锁消息监听
            /// </summary>
            private void _regSimpleUnlockMsgByRedId(long _redId)
            {
                RedMonitorRefObj redMonitorRefObj = GRefdataCoreMgr.instance.redMonitorRefCore.getRef(_redId);
                if (redMonitorRefObj == null || redMonitorRefObj.msgTypeList == null || redMonitorRefObj.msgTypeList.Count <= 0)
                    return;

                Action msgRecAction = () => { _onSimpleUnlockRefresh(_redId); };
                for (int i = 0; i < redMonitorRefObj.msgTypeList.Count; i++)
                {
                    WinMsg.RegisterMsgAct(redMonitorRefObj.msgTypeList[i], msgRecAction);
                }
                _m_dSimpleUnlockTriggerDic[_redId] = msgRecAction;
            }

            /// <summary>
            /// 注销功能解锁消息监听(模仿RedTipMgr._unRegSimpleUnlockMsg)
            /// </summary>
            private void _unRegSimpleUnlockMsg()
            {
                if (_m_dSimpleUnlockTriggerDic.Count <= 0)
                    return;

                foreach (var kvp in _m_dSimpleUnlockTriggerDic)
                {
                    RedMonitorRefObj redMonitorRefObj = GRefdataCoreMgr.instance.redMonitorRefCore.getRef(kvp.Key);
                    if (redMonitorRefObj == null || redMonitorRefObj.msgTypeList == null || redMonitorRefObj.msgTypeList.Count <= 0)
                        continue;

                    for (int i = 0; i < redMonitorRefObj.msgTypeList.Count; i++)
                    {
                        WinMsg.UnregisterMsgAct(redMonitorRefObj.msgTypeList[i], kvp.Value);
                    }
                }
                _m_dSimpleUnlockTriggerDic.Clear();
            }

            /// <summary>
            /// 功能解锁消息回调，刷新对应红点
            /// </summary>
            private void _onSimpleUnlockRefresh(long _redId)
            {
                if (_m_component == null)
                    return;

                if (_redId == RedTipConst.RED_CONSORT_BUSINESS_SKILL)
                {
                    foreach (GConsortRefObj consortRefObj in GRefdataCoreMgr.instance.consortRefCore.refList)
                    {
                        if (consortRefObj != null)
                            _refreshConsortBusinessSkillRed(consortRefObj.id);
                    }
                }
                else if (_redId == RedTipConst.RED_CONSORT_BLESS_SKILL)
                {
                    foreach (GConsortRefObj consortRefObj in GRefdataCoreMgr.instance.consortRefCore.refList)
                    {
                        if (consortRefObj != null)
                            _refreshConsortBlessSKillRed(consortRefObj.id);
                    }
                }
            }
            
            /// <summary>
            /// 玩家信息详细红点
            /// </summary>
            private void _refreshRed()
            {
                _refreshCanInviteRed();//刷新可邀约红点

                foreach (GConsortRefObj consortRefObj in GRefdataCoreMgr.instance.consortRefCore.refList)
                {
                    if(consortRefObj != null)
                        _refreshConsortRed(consortRefObj.id);
                }
                
                _refreshCgRewardRed(); // 刷新CG奖励红点
            }

            /// <summary>
            /// 刷新妃子红点
            /// </summary>
            private void _refreshConsortRed(long _consortId)
            {
                if(_m_component == null)
                    return;
                
                GGottenConsortInfo consortInfo = _m_component.getConsortInfo(_consortId);
                
                _refreshConsortBusinessSkillRed(_consortId, consortInfo);//刷新妃子经营技能红点
                _refreshConsortBlessSKillRed(_consortId, consortInfo);//刷新妃子加护技能红点
                _refreshConsortFetterRed(_consortId, consortInfo);//刷新妃子羁绊红点
            }

            /// <summary>
            /// 皮肤红点
            /// </summary>
            /// <param name="gottenConsortInfo"></param>
            private void _addSkinRed(GGottenConsortInfo gottenConsortInfo)
            {
                if(gottenConsortInfo == null || gottenConsortInfo.consortRefObj == null)
                    return;
                
                _ARedTipNode consortRedTipNode = null;
                //妃子解锁新皮肤
                _ARedTipNode consortSkinTipNode = RedTipMgr.instance.getNodeBySaveKeyRecursive(getConsortSaveKey(RedTipConst.RED_CONSORT_SKIN_ENTER, gottenConsortInfo.consortId));
                if (null == consortSkinTipNode)
                {
                    consortSkinTipNode = new CommonForceRedTipNode(getConsortSaveKey(RedTipConst.RED_CONSORT_SKIN_ENTER, gottenConsortInfo.consortId));
                    RedTipMgr.instance.addRedTipNodeWithParent(consortSkinTipNode, RedTipConst.RED_CONSORT_SKIN_ENTER);
                }

                foreach (GConsortSkinInfo skinInfo in gottenConsortInfo.skinList)
                {
                    bool isLooked = _m_component.remarkInfo.isConsortSkinUnlockLooked(gottenConsortInfo.consortId, skinInfo.skinId);
                    consortRedTipNode = RedTipMgr.instance.getNodeBySaveKeyRecursive(getConsortSubSaveKey(RedTipConst.RED_CONSORT_SKIN_ENTER, gottenConsortInfo.consortId, skinInfo.skinId));
                    if (null == consortRedTipNode)
                    {
                        consortRedTipNode = new CommonForceRedTipNode(getConsortSubSaveKey(RedTipConst.RED_CONSORT_SKIN_ENTER, gottenConsortInfo.consortId, skinInfo.skinId));
                        RedTipMgr.instance.addRedTipNodeWithParent(consortRedTipNode, consortSkinTipNode);
                    }

                    if (isLooked || skinInfo.skinId == gottenConsortInfo.consortRefObj.default_skin_id)//已经看过或者默认皮肤，不显示红点
                    {
                        consortRedTipNode.setCount(0);
                    }
                    else
                    {
                        consortRedTipNode.setCount(1);
                    }
                }
            }

            /// <summary>
            /// 妃子系统红点根节点
            /// </summary>
            /// <returns></returns>
            [NotNull] public _ARedTipNode getConsortSystemRootRedTipNode()
            {
                if (_m_dRefRedTipDic.TryGetValue(RedTipConst.RED_CONSORT_ENTER.ToString(), out _ARedTipNode _nodeTip) && _nodeTip != null)
                    return _nodeTip;
                
                _nodeTip = RedTipMgr.instance.getNodeByRefRedTipId(RedTipConst.RED_CONSORT_ENTER);
                if (_nodeTip == null)//若找不到, 创建一个红点根节点
                {
                    _nodeTip = new RefdataRedTipNode(RedTipConst.RED_CONSORT_ENTER);
                    RedTipMgr.instance.addRootRedTipNode(_nodeTip);
                }
                
                _m_dRefRedTipDic[RedTipConst.RED_CONSORT_ENTER.ToString()] = _nodeTip;
                return _nodeTip;
            }
            
            /// <summary>
            /// 获取妃子item红点Node
            /// </summary>
            /// <returns></returns>
            [NotNull] public _ARedTipNode getConsortRedTipNode(long _consortId)
            {
                // 妃子item红点保存key
                string saveKey = getConsortSaveKey(RedTipConst.RED_CONSORT_ENTER, _consortId);
                
                if (_m_dRefRedTipDic.TryGetValue(saveKey, out _ARedTipNode _nodeTip) && _nodeTip != null)
                    return _nodeTip;
                
                _ARedTipNode consortSystemRootRedTipNode = getConsortSystemRootRedTipNode();
                _nodeTip = consortSystemRootRedTipNode.getNodeBySaveKeyRecursive(saveKey);
                if (_nodeTip == null)//若找不到, 创建一个妃子红点节点加入到妃子系统根节点下
                {
                    _nodeTip = new CommonForceRedTipNode(saveKey);
                    RedTipMgr.instance.addRedTipNodeWithParent(_nodeTip, RedTipConst.RED_CONSORT_ENTER);
                }
                
                _m_dRefRedTipDic[saveKey] = _nodeTip;
                return _nodeTip;
            }
            
            /// <summary>
            /// 获取单妃子红点下子红点节点
            /// </summary>
            /// <param name="_consortId"></param>
            /// <param name="_subId"></param>
            /// <returns></returns>
            [NotNull] public _ARedTipNode getConsortSubRedTipNode(long _consortId, long _subId)
            {
                string saveKey = getConsortSubSaveKey(RedTipConst.RED_CONSORT_ENTER, _consortId, _subId);
                
                if (_m_dRefRedTipDic.TryGetValue(saveKey, out _ARedTipNode _nodeTip) && _nodeTip != null)
                    return _nodeTip;
                
                _ARedTipNode consortRedTipNode = getConsortRedTipNode(_consortId);
                _nodeTip = consortRedTipNode.getNodeBySaveKeyRecursive(saveKey);
                if (_nodeTip == null)//若找不到, 创建一个妃子红点节点加入到妃子系统根节点下
                {
                    _nodeTip = new CommonForceRedTipNode(saveKey);
                    RedTipMgr.instance.addRedTipNodeWithParent(_nodeTip, consortRedTipNode);
                }
                
                _m_dRefRedTipDic[saveKey] = _nodeTip;
                return _nodeTip;
            }

            #region 可邀约红点

            /// <summary>
            /// 获取妃子可邀约红点Node
            /// </summary>
            /// <returns></returns>
            [NotNull] public _ARedTipNode getCanInviteRedTipNode()
            {
                // 妃子可邀约红点保存key
                string saveKey = RedTipConst.RED_CONSORT_HAS_INTITE_CD.ToString();
                
                if (_m_dRefRedTipDic.TryGetValue(saveKey, out _ARedTipNode _nodeTip) && _nodeTip != null)
                    return _nodeTip;
                
                // 获取妃子系统红点根节点Node
                _ARedTipNode consortSystemRootRedTipNode = getConsortSystemRootRedTipNode();
                _nodeTip = consortSystemRootRedTipNode.getNodeBySaveKeyRecursive(saveKey);
                if (_nodeTip == null)
                {
                    _nodeTip = new CommonForceRedTipNode(saveKey);
                    RedTipMgr.instance.addRedTipNodeWithParent(_nodeTip, consortSystemRootRedTipNode);
                }
                
                _m_dRefRedTipDic[saveKey] = _nodeTip;
                return _nodeTip;
            }
            
            /// <summary>
            /// 刷新可邀约红点
            /// </summary>
            private void _refreshCanInviteRed()
            {
                if(_m_component == null)
                    return;

                int count = NPPlayer.instance.lazyCdComp.getCount(GRefdataCoreMgr.instance.npGeneral.consort_rand_call_cd);
                _ARedTipNode consortCanInviteRedTipNode = getCanInviteRedTipNode();
                consortCanInviteRedTipNode.setCount(count);
            }
            
            #endregion

            #region CG奖励红点

            /// <summary>
            /// 获取妃子CG奖励红点Node
            /// </summary>
            /// <returns></returns>
            [NotNull] public _ARedTipNode getConsortCgRewardRedTipNode()
            {
                // 妃子CG奖励红点保存key
                string saveKey = RedTipConst.RED_CONSORT_CG_REWARD.ToString();
                
                if (_m_dRefRedTipDic.TryGetValue(saveKey, out _ARedTipNode _nodeTip) && _nodeTip != null)
                    return _nodeTip;
                
                // 获取妃子系统红点根节点Node
                _nodeTip = RedTipMgr.instance.getNodeByRefRedTipId(RedTipConst.RED_CONSORT_CG_REWARD);
                if (_nodeTip == null)
                {
                    // 获取妃子系统红点根节点Node
                    _ARedTipNode consortSystemRootRedTipNode = getConsortSystemRootRedTipNode();
                    _nodeTip = consortSystemRootRedTipNode.getNodeBySaveKeyRecursive(saveKey);
                    if (_nodeTip == null)
                    {
                        _nodeTip = new CommonForceRedTipNode(saveKey);
                        RedTipMgr.instance.addRedTipNodeWithParent(_nodeTip, consortSystemRootRedTipNode);
                    }
                }
                
                _m_dRefRedTipDic[saveKey] = _nodeTip;
                return _nodeTip;
            }
            
            /// <summary>
            /// 刷新妃子CG奖励红点
            /// </summary>
            private void _refreshCgRewardRed()
            {
                if(_m_component == null)
                    return;

                _ARedTipNode consortCgRewardRedTipNode = getConsortCgRewardRedTipNode();
                
                // 检查是否有未领取奖励的CG
                bool hasUnclaimedCgReward = false;
                if (_m_component.consortCgInfoList != null)
                {
                    foreach (ConsortCgInfo cgInfo in _m_component.consortCgInfoList)
                    {
                        // 如果CG未领取奖励，则显示红点
                        if (cgInfo != null && !cgInfo.rewarded)
                        {
                            hasUnclaimedCgReward = true;
                            break;
                        }
                    }
                }
                
                consortCgRewardRedTipNode.setCount(hasUnclaimedCgReward ? 1 : 0);
            }
            
            #endregion

            #region 获取新妃子红点

            /// <summary>
            /// 获取新妃子红点Node
            /// </summary>
            /// <returns></returns>
            [NotNull] public _ARedTipNode getGainConsortRedTipNode(long _consortId)
            {
                _ARedTipNode nodeTip = getConsortSubRedTipNode(_consortId, RedTipConst.RED_CONSORT_GAIN);
                return nodeTip;
            }

            /// <summary>
            /// 设置获取新妃子红点
            /// </summary>
            private void _setGainConsortRedTip(long _consortId)
            {
                _ARedTipNode gainConsortRedTipNode = getGainConsortRedTipNode(_consortId);
                gainConsortRedTipNode.setCount(1);
            }

            /// <summary>
            /// 设置已读获取新妃子红点
            /// </summary>
            /// <param name="_consortId"></param>
            public void setReadGainConsortRed(long _consortId)
            {
                _ARedTipNode gainConsortRedTipNode = getGainConsortRedTipNode(_consortId);
                if (gainConsortRedTipNode.getCount() != 0)
                    gainConsortRedTipNode.setCount(0);
            }

            #endregion
            
            #region 经营技能红点

            /// <summary>
            /// 获取妃子经营技能红点RootNode
            /// </summary>
            /// <param name="_consortId"></param>
            /// <returns></returns>
            [NotNull] public _ARedTipNode getConsortBusinessSkillRedTipRootNode(long _consortId)
            {
                // 妃子经营技能红点保存key
                _ARedTipNode consortBusinessSkillRootRedTipNode = getConsortSubRedTipNode(_consortId, RedTipConst.RED_CONSORT_BUSINESS_SKILL);
                return consortBusinessSkillRootRedTipNode;
            }
            
            /// <summary>
            /// 获取妃子经营技能红点Node
            /// </summary>
            /// <param name="_consortId"></param>
            /// <returns></returns>
            [NotNull] public _ARedTipNode getConsortBusinessSkillRedTipNode(long _consortId, long _skillId)
            {
                // 妃子经营技能红点保存key
                string saveKey = getConsortSubExtraSaveKey(RedTipConst.RED_CONSORT_ENTER, _consortId, RedTipConst.RED_CONSORT_BUSINESS_SKILL, _skillId.ToString());
                if (_m_dRefRedTipDic.TryGetValue(saveKey, out _ARedTipNode _nodeTip) && _nodeTip != null)
                    return _nodeTip;
                
                // 获取 妃子item - 经营技能红点 Node
                _ARedTipNode consortBusinessSkillRootRedTipNode = getConsortBusinessSkillRedTipRootNode(_consortId);
                _nodeTip = consortBusinessSkillRootRedTipNode.getNodeBySaveKeyRecursive(saveKey);
                if (_nodeTip == null)
                {
                    _nodeTip = new CommonForceRedTipNode(saveKey);
                    RedTipMgr.instance.addRedTipNodeWithParent(_nodeTip, consortBusinessSkillRootRedTipNode);
                }
                
                _m_dRefRedTipDic[saveKey] = _nodeTip;
                return _nodeTip;
            }
            
            /// <summary>
            /// 刷新妃子经营技能红点
            /// </summary>
            private void _refreshConsortBusinessSkillRed(long _consortId)
            {
                if(_m_component == null)
                    return;

                _refreshConsortBusinessSkillRed(_consortId, _m_component.getConsortInfo(_consortId));
            }

            /// <summary>
            /// 刷新妃子经营技能红点
            /// </summary>
            /// <param name="_consortInfo"></param>
            private void _refreshConsortBusinessSkillRed(long _consortId, GGottenConsortInfo _consortInfo)
            {
                RedMonitorRefObj redMonitorRefObj = GRefdataCoreMgr.instance.redMonitorRefCore.getRef(RedTipConst.RED_CONSORT_BUSINESS_SKILL);
                bool businessSkillIsUnlock = redMonitorRefObj == null || GCommon.isSimpleUnlock(redMonitorRefObj.simple_unlock_id);
                
                foreach (ConsortBusinessSkillRefObj businessSkillRefObj in GRefdataCoreMgr.instance.consortBusinessSkillRefCore.refList)
                {
                    if (businessSkillRefObj == null)
                        continue;

                    ConsortUtil.getBusinessSkillState(businessSkillRefObj, _consortInfo, (EConsortBusinessSkillItemState _itemState) =>
                    {
                        _ARedTipNode consortBusinessSkillRedTipNode = getConsortBusinessSkillRedTipNode(_consortId, businessSkillRefObj.id);
                        // 若技能未解锁且未读过该技能解锁红点，则显示红点
                        if (businessSkillIsUnlock && _itemState == EConsortBusinessSkillItemState.UNLOCK_NO_OP && 
                            !(_m_redTipSaver?.hasReadUnlockBusinessSkill(_consortId, businessSkillRefObj.id) ?? false))
                        {
                            consortBusinessSkillRedTipNode.setCount(1);
                        }
                        else
                        {
                            consortBusinessSkillRedTipNode.setCount(0);
                        }
                    });
                }
            }
            
            /// <summary>
            /// 设置已读经营技能红点
            /// </summary>
            /// <param name="_consortId"></param>
            /// <param name="_skillId"></param>
            public void setReadUnlockBusinessSkill(long _consortId, long _skillId)
            {
                _m_redTipSaver?.setReadUnlockBusinessSkill(_consortId, _skillId);

                _ARedTipNode consortBusinessSkillRedTipNode = getConsortBusinessSkillRedTipNode(_consortId, _skillId);
                consortBusinessSkillRedTipNode.setCount(0);
            }
            
            #endregion

            #region 加护技能红点

            /// <summary>
            /// 获取妃子加护技能红点Node
            /// </summary>
            /// <param name="_consortId"></param>
            /// <returns></returns>
            [NotNull] public _ARedTipNode getConsortBlessSkillRedTipNode(long _consortId)
            {
                _ARedTipNode nodeTip = getConsortSubRedTipNode(_consortId, RedTipConst.RED_CONSORT_BLESS_SKILL);
                return nodeTip;
            }
            
            /// <summary>
            /// 刷新妃子加护技能红点
            /// </summary>
            /// <param name="_consortId"></param>
            private void _refreshConsortBlessSKillRed(long _consortId)
            {
                if(_m_component == null)
                    return;

                _refreshConsortBlessSKillRed(_consortId, _m_component.getConsortInfo(_consortId));
            }
            
            /// <summary>
            /// 刷新妃子加护技能红点
            /// </summary>
            /// <param name="_consortId"></param>
            private void _refreshConsortBlessSKillRed(long _consortId, GGottenConsortInfo _consortInfo)
            {
                _ARedTipNode consortBlessSKillRedTipNode = getConsortBlessSkillRedTipNode(_consortId);
                RedMonitorRefObj redMonitorRefObj = GRefdataCoreMgr.instance.redMonitorRefCore.getRef(RedTipConst.RED_CONSORT_BLESS_SKILL);
                bool blessSkillIsUnlock = redMonitorRefObj == null || GCommon.isSimpleUnlock(redMonitorRefObj.simple_unlock_id);
                
                // 没有加护技能 或 加护技能功能未解锁时不显示红点
                if (_consortInfo == null || _consortInfo.blessSkillInfoList == null || !blessSkillIsUnlock)
                {
                    consortBlessSKillRedTipNode.setCount(0);
                    return;
                }

                long charmPointChgTime = _m_redTipSaver?.getConsortCharmPointChgTime(_consortId) ?? 0;// 获取加护点变化时间
                long blessSkillRedReadTime = _m_redTipSaver?.getConsortBlessSkillRedReadTime(_consortId) ?? 0;// 获取加护技能红点已读时间
                // 若加护点变化时间早于已读时间，则不显示红点
                if (blessSkillRedReadTime >= charmPointChgTime)
                {
                    consortBlessSKillRedTipNode.setCount(0);
                    return;
                }
                
                bool needShowRed = false;
                foreach (ConsortBlessSkillInfo blessSkillInfo in _consortInfo.blessSkillInfoList)
                {
                    if (blessSkillInfo == null)
                        continue;

                    // 若技能等级未满级且技能点足够升级
                    if (blessSkillInfo.checkHasCanLevelUp(_consortInfo))
                    {
                        needShowRed = true;
                        break;
                    }
                }
                
                consortBlessSKillRedTipNode.setCount(needShowRed ? 1 : 0);
            }
            
            /// <summary>
            /// 设置已读加护技能红点
            /// </summary>
            /// <param name="_consortId"></param>
            public void setReadConsortBlessSkillRed(long _consortId)
            {
                // 设置
                _m_redTipSaver?.setConsortBlessSkillRedReadTime(_consortId, FpsAndPingMgr.instance.serverTimeTag);
                
                _ARedTipNode consortBlessSKillRedTipNode = getConsortBlessSkillRedTipNode(_consortId);
                consortBlessSKillRedTipNode.setCount(0);
            }

            /// <summary>
            /// 设置妃子加护点变化时间
            /// </summary>
            private void _setConsortCharmPointChgTime(long _consortId)
            {
                _m_redTipSaver?.setConsortCharmPointChgTime(_consortId, FpsAndPingMgr.instance.serverTimeTag);
             
                // 加护点变化，刷新加护技能红点
                _refreshConsortBlessSKillRed(_consortId);
            }
            
            #endregion

            #region 羁绊红点

            /// <summary>
            /// 获取妃子羁绊红点Node
            /// </summary>
            /// <param name="_consortId"></param>
            /// <returns></returns>
            [NotNull] public _ARedTipNode getConsortFetterRedTipNode(long _consortId)
            {
                // 妃子羁绊技能红点保存key
                _ARedTipNode nodeTip = getConsortSubRedTipNode(_consortId, RedTipConst.RED_CONSORT_FETTER);
                return nodeTip;
            }
            
            /// <summary>
            /// 刷新妃子羁绊红点
            /// </summary>
            private void _refreshConsortFetterRed(long _consortId)
            {
                if(_m_component == null)
                    return;

                _refreshConsortFetterRed(_consortId, _m_component.getConsortInfo(_consortId));
            }
            
            /// <summary>
            /// 刷新妃子羁绊红点
            /// </summary>
            private void _refreshConsortFetterRed(long _consortId, GGottenConsortInfo _consortInfo)
            {
                _ARedTipNode consortFetterRedTipNode = getConsortFetterRedTipNode(_consortId);
                RedMonitorRefObj redMonitorRefObj = GRefdataCoreMgr.instance.redMonitorRefCore.getRef(RedTipConst.RED_CONSORT_FETTER);
                bool fetterIsUnlock = redMonitorRefObj == null || GCommon.isSimpleUnlock(redMonitorRefObj.simple_unlock_id);
                
                if (_consortInfo == null || _consortInfo.fetterInfo == null || !fetterIsUnlock)
                {
                    consortFetterRedTipNode.setCount(0);
                    return;
                }
                
                bool canLevelUp = ConsortFetterInfo.checkCanLevelUp(_consortInfo, _consortInfo.fetterInfo, false);
                consortFetterRedTipNode.setCount(canLevelUp ? 1 : 0);
            }
            
            #endregion

            #region 送礼红点(送礼红点和道具有关与单妃子无关)

            /// <summary>
            /// 获取妃子送礼红点保存key
            /// </summary>
            /// <param name="_bagItemId"></param>
            /// <returns></returns>
            public string getSendGiftRedTipSaveKey(long _bagItemId)
            {
                return $"{RedTipConst.RED_CONSORT_SEND_GIFT}_{_bagItemId}";
            }

            /// <summary>
            /// 获取 妃子送礼红点根节点
            /// </summary>
            /// <returns></returns>
            [NotNull] public _ARedTipNode getSendGiftRedTipRootNode()
            {
                if (_m_dRefRedTipDic.TryGetValue(RedTipConst.RED_CONSORT_SEND_GIFT.ToString(), out _ARedTipNode _nodeTip) && _nodeTip != null)
                    return _nodeTip;
                
                _nodeTip = RedTipMgr.instance.getNodeByRefRedTipId(RedTipConst.RED_CONSORT_SEND_GIFT);
                if(_nodeTip == null)
                {
                    _nodeTip = new RefdataRedTipNode(RedTipConst.RED_CONSORT_SEND_GIFT);
                    RedTipMgr.instance.addRedTipNodeWithParent(_nodeTip, RedTipConst.RED_CONSORT_ENTER);
                }
                _m_dRefRedTipDic[RedTipConst.RED_CONSORT_SEND_GIFT.ToString()] = _nodeTip;

                return _nodeTip;
            }
            
            /// <summary>
            /// 获取送礼红点Node
            /// </summary>
            /// <param name="_bagItemId"></param>
            /// <returns></returns>
            [NotNull] public _ARedTipNode getSendGiftBagItemRedTipNode(long _bagItemId)
            {
                string saveKey = getSendGiftRedTipSaveKey(_bagItemId);
                if (_m_dRefRedTipDic.TryGetValue(saveKey, out _ARedTipNode _nodeTip) && _nodeTip != null)
                    return _nodeTip;
                
                // 获取 妃子送礼红点根节点
                _ARedTipNode sendGiftRedTipRootNode = getSendGiftRedTipRootNode();
                _nodeTip = sendGiftRedTipRootNode.getNodeBySaveKeyRecursive(saveKey);
                if (_nodeTip == null)
                {
                    _nodeTip = new CommonForceRedTipNode(saveKey);
                    RedTipMgr.instance.addRedTipNodeWithParent(_nodeTip, sendGiftRedTipRootNode);
                }
                
                _m_dRefRedTipDic[saveKey] = _nodeTip;
                return _nodeTip;
            }

            /// <summary>
            /// 初始化妃子送礼红点
            /// </summary>
            private void _initSendGiftRedTip()
            {
                NPPlayer.instance.bagComp.dealAllItem((_bagItem) =>
                {
                    if(_bagItem == null || _bagItem.count <= 0)
                        return;
                    
                    BagItemConsortRefObj bagItemConsortRefObj = GRefdataCoreMgr.instance.bagItemConsortCore.getRef(_bagItem.itemId);
                    if(bagItemConsortRefObj != null && bagItemConsortRefObj.isConsortDetailSendGiftBagItem())
                        getSendGiftBagItemRedTipNode(_bagItem.itemId);
                });
            }
            
            /// <summary>
            /// 显示送礼道具红点
            /// </summary>
            /// <param name="_bagItemId"></param>
            public void showSendGiftBagItemRedTip(long _bagItemId)
            {
                BagItem bagItem = NPPlayer.instance.bagComp.getItem(_bagItemId);
                if(bagItem == null || bagItem.count <= 0)
                    return;
                
                _ARedTipNode sendGiftBagItemRedTipNode = getSendGiftBagItemRedTipNode(_bagItemId);
                sendGiftBagItemRedTipNode.setCount(1);
            }

            /// <summary>
            /// 设置送礼道具红点已读
            /// </summary>
            public void setSendGiftBagItemRedTipRead(long _bagItemId)
            {
                _ARedTipNode sendGiftBagItemRedTipNode = getSendGiftBagItemRedTipNode(_bagItemId);
                sendGiftBagItemRedTipNode.setCount(0);
            }
            
            #endregion
            
            #region 消息监听

            /// <summary>
            /// 当前获取新妃子
            /// </summary>
            private void _onConsortAdd(params object[] _objs)
            {
                if(_objs == null || _objs.Length < 1 || !(_objs[0] is long _consortId) || _m_component == null)
                    return;
                
                // 设置获取妃子红点
                _setGainConsortRedTip(_consortId);
                // 刷新妃子红点
                _refreshConsortRed(_consortId);
            }
            
            /// <summary>
            /// 妃子经营技能信息变化
            /// </summary>
            private void _onConsortBusinessSkillChg(params object[] _objs)
            {
                if(_objs == null || _objs.Length < 1 || !(_objs[0] is long _consortId) || _m_component == null)
                    return;

                _refreshConsortBusinessSkillRed(_consortId);
            }

            /// <summary>
            /// 妃子加护技能信息变化
            /// </summary>
            private void _onConsortBlessSkillChg(params object[] _objs)
            {
                if(_objs == null || _objs.Length < 1 || !(_objs[0] is long _consortId) || _m_component == null)
                    return;

                // _refreshConsortBlessSKillRed(_consortId);
            }

            /// <summary>
            /// 妃子羁绊技能信息变化
            /// </summary>
            private void _onConsortFetterLevelChg(params object[] _objs)
            {
                if(_objs == null || _objs.Length < 1 || !(_objs[0] is long _consortId) || _m_component == null)
                    return;

                _refreshConsortFetterRed(_consortId);
            }
            
            /// <summary>
            /// 妃子亲密度变化
            /// </summary>
            /// <param name="_objs"></param>
            private void _onConsortIntimacyChg(params object[] _objs)
            {
                if(_objs == null || _objs.Length < 1 || !(_objs[0] is long _consortId) || _m_component == null)
                    return;

                _refreshConsortRed(_consortId);
            }
            
            /// <summary>
            /// 妃子加护力变化
            /// </summary>
            /// <param name="_objs"></param>
            private void _onConsortCharmChg(params object[] _objs)
            {
                if(_objs == null || _objs.Length < 1 || !(_objs[0] is long _consortId) || _m_component == null)
                    return;

                _refreshConsortRed(_consortId);
            }
            
            /// <summary>
            /// 妃子加护点变化
            /// </summary>
            /// <param name="_objs"></param>
            private void _onConsortCharmPointChg(params object[] _objs)
            {
                if(_objs == null || _m_component == null || _objs.Length < 3 || !(_objs[0] is long _consortId) ||
                   !(_objs[1] is long _oldCharmPoint) || !(_objs[2] is long _newCharmPoint) || _oldCharmPoint >= _newCharmPoint)
                    return;

                _setConsortCharmPointChgTime(_consortId);//设置加护点变化时间
            }
            
            /// <summary>
            /// 妃子CG变化
            /// </summary>
            private void _onConsortCgChg(params object[] _objs)
            {
                if(_objs == null || _objs.Length < 1 || !(_objs[0] is ConsortCgInfo cgInfo) || _m_component == null)
                    return;
                
                // 刷新CG奖励红点
                _refreshCgRewardRed();
            }
            
            /// <summary>
            /// 妃子新增CG
            /// </summary>
            private void _onConsortAddCg(params object[] _objs)
            {
                if(_objs == null || _objs.Length < 1 || !(_objs[0] is ConsortCgInfo cgInfo) || _m_component == null)
                    return;
                
                // 刷新CG奖励红点
                _refreshCgRewardRed();
            }
            
            private void _onBagItemCountAdd(params object[] _objs)
            {
                if(_objs == null || _objs.Length < 1 || !(_objs[0] is BagItem bagItem) || bagItem.count <= 0)
                    return;

                BagItemConsortRefObj bagItemConsortRefObj = GRefdataCoreMgr.instance.bagItemConsortCore.getRef(bagItem.itemId);
                // 是送礼道具，显示送礼道具红点
                if (bagItemConsortRefObj != null && bagItemConsortRefObj.isConsortDetailSendGiftBagItem())
                {
                    showSendGiftBagItemRedTip(bagItem.itemId);
                }
            }
            
            #endregion
        }

        /// <summary>
        /// 妃子解锁的皮肤是否已经看过
        /// </summary>
        /// <param name="_consortId"></param>
        /// <param name="_skinId"></param>
        /// <returns></returns>
        public bool isConsortSkinUnlockLooked(long _consortId, long _skinId)
        {
            return remarkInfo.isConsortSkinUnlockLooked(_consortId, _skinId);
        }


        /// <summary>
        /// 妃子解锁的皮肤是否已经看过
        /// </summary>
        /// <param name="_consortId"></param>
        /// <param name="_skinId"></param>
        /// <returns></returns>
        public void setConsortSkinUnlockLooked(long _consortId, long _skinId)
        {
            remarkInfo.setConsortSkinUnlockLooked(_consortId, _skinId);
        }

        [NotNull] public static string getConsortSaveKey(long _saveMainId, long _consortId)
        {
            return $"{_saveMainId}_{_consortId}";
        }

        [NotNull] public static string getConsortSubSaveKey(long _saveMainId, long _consortId, long _subId)
        {
            return $"{_saveMainId}_{_consortId}_{_subId}";
        }

        [NotNull] public static string getConsortSubExtraSaveKey(long _saveMainId, long _consortId, long _subId, string _extra)
        {
            return $"{_saveMainId}_{_consortId}_{_subId}_{_extra}";
        }
        
        /// <summary>
        /// 设置已读获取新妃子红点
        /// </summary>
        public void setReadGainConsortRedTip(long _consortId)
        {
            _m_redTip?.setReadGainConsortRed(_consortId);
        }
        
        /// <summary>
        /// 获取妃子显示红点数
        /// </summary>
        /// <param name="_consortId"></param>
        /// <returns></returns>
        public long getConsortRedCount(long _consortId)
        {
            return _m_redTip?.getConsortRedTipNode(_consortId).getCount() ?? 0;
        }

        #region 经营技能

        /// <summary>
        /// 是否需要显示妃子经营技能红点
        /// </summary>
        /// <param name="_consortId"></param>
        /// <returns></returns>
        public bool needShowConsortBusinessSkillRed(long _consortId)
        {
            _ARedTipNode consortBusinessSkillRedTipRootNode = _m_redTip?.getConsortBusinessSkillRedTipRootNode(_consortId);
            return consortBusinessSkillRedTipRootNode != null && consortBusinessSkillRedTipRootNode.getCount() > 0;
        }
        
        /// <summary>
        /// 是否需要显示妃子经营技能红点
        /// </summary>
        /// <param name="_consortId"></param>
        /// <param name="_skillId"></param>
        /// <returns></returns>
        public bool needShowConsortBusinessSkillRed(long _consortId, long _skillId)
        {
            _ARedTipNode consortBusinessSkillRedTipNode = _m_redTip?.getConsortBusinessSkillRedTipNode(_consortId, _skillId);
            return consortBusinessSkillRedTipNode != null && consortBusinessSkillRedTipNode.getCount() > 0;
        }
        
        /// <summary>
        /// 设置已读经营技能红点
        /// </summary>
        /// <param name="_consortId"></param>
        /// <param name="_skillId"></param>
        public void setReadBusinessSkillRed(long _consortId, long _skillId)
        {
            _m_redTip?.setReadUnlockBusinessSkill(_consortId, _skillId);
        }

        #endregion

        #region 加护技能

        /// <summary>
        /// 获取是否需要显示妃子加护技能红点
        /// </summary>
        /// <param name="_consortId"></param>
        /// <returns></returns>
        public bool needShowConsortBlessSkillRed(long _consortId)
        {
            _ARedTipNode consortBlessSkillRedTipNode = _m_redTip?.getConsortBlessSkillRedTipNode(_consortId);
            return consortBlessSkillRedTipNode?.needShow() ?? false;
        }
        
        /// <summary>
        /// 设置已读加护技能红点
        /// </summary>
        /// <param name="_consortId"></param>
        public void setReadConsortBlessSkillRed(long _consortId)
        {
            _m_redTip?.setReadConsortBlessSkillRed(_consortId);
        }

        #endregion
        
        
        /// <summary>
        /// 获取是否需要显示妃子羁绊红点
        /// </summary>
        /// <param name="_consortId"></param>
        /// <returns></returns>
        public long getConsortFetterRedCount(long _consortId)
        {
            return _m_redTip?.getConsortFetterRedTipNode(_consortId).getCount() ?? 0;
        }
        
        /// <summary>
        /// 获取妃子CG奖励红点数
        /// </summary>
        /// <returns></returns>
        public long getConsortCgRewardRedCount()
        {
            return _m_redTip?.getConsortCgRewardRedTipNode().getCount() ?? 0;
        }

        #region 妃子送礼道具红点

        /// <summary>
        /// 设置送礼道具红点已读
        /// </summary>
        /// <param name="_bagItemId"></param>
        public void setConsortSendGiftBagItemRedTipRead(long _bagItemId)
        {
            _m_redTip?.setSendGiftBagItemRedTipRead(_bagItemId);
        }
        
        /// <summary>
        /// 是否需要显示妃子送礼道具红点
        /// </summary>
        /// <param name="_bagItemId"></param>
        public bool needShowConsortSendGiftBagItemRedTip(long _bagItemId)
        {
            _ARedTipNode sendGiftBagItemRedTipNode = _m_redTip?.getSendGiftBagItemRedTipNode(_bagItemId);
            return sendGiftBagItemRedTipNode?.needShow() ?? false;
        }

        public bool needShowConsortSendGiftRootRedTip()
        {
            _ARedTipNode sendGiftRedTipRootNode = _m_redTip?.getSendGiftRedTipRootNode();
            return sendGiftRedTipRootNode != null && sendGiftRedTipRootNode.getCount() > 0;
        }
        
        #endregion
    }
}