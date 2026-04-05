using ALPackage;
using Common.FriendObj;
using GS2GC.p002_InitOp;
using GS2GC.p004_PlayerOp;
using GS2GC.p021_PlayerInfo;
using JetBrains.Annotations;
using NPEnum;
using System;
using System.Collections.Generic;
using NPCommon;

namespace GOE
{
    // 好友模块管理类
    public partial class PlayerFriendsComponent : _ANPBasicPlayerComponent
    {
        //好友申请列表
        [NotNull] private List<PlayerFriendApplyItem> _m_applyList;

        //好友列表
        [NotNull] private List<PlayerFriendItem> _m_friendsList;
        //好友分组列表
        [NotNull] private List<PlayerFriendGroup> _m_friendGroupList;
        //屏蔽列表
        private List<PlayerShieldItem> _m_shieldIdList;
 
        private RedTipDealer _m_refMgr;

        //发送申请的好友时间戳
        private Dictionary<long, long> _m_requestDic;

        //当前拜访的好友
        private NPCommonSimplePlayerInfo _m_visitFriendInfo;

        private ALStepCounter _m_initStepCounter;

        //构造函数
        public PlayerFriendsComponent(NPPlayerComponentMgr _compMgr) : base(_compMgr)
        {
            _m_shieldIdList = new List<PlayerShieldItem>();
            _m_applyList = new List<PlayerFriendApplyItem>();
            _m_friendsList = new List<PlayerFriendItem>();
            _m_friendGroupList = new List<PlayerFriendGroup>();
            _m_refMgr = new RedTipDealer(this);
            _m_initStepCounter = new ALStepCounter();
        }
      
        #region override
        public override bool isMustInit { get { return true; } }
        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.FRIEND; } }
        public override ENPPlayerCompType[] dependCompList { get { return null; } }
        #endregion

        //发送申请的好友时间戳
        public Dictionary<long,long> requestDic { get { return _m_requestDic; } }

        public NPCommonSimplePlayerInfo visitFriendInfo { get { return _m_visitFriendInfo; } }

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
            _m_initStepCounter.resetAll();
            _m_initStepCounter.chgTotalStepCount(2);
            _m_initStepCounter.regAllDoneDelegate(setInitDone);
            _reqFriendInitData();
            _reqShieldInit();
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
            ALLog.Error("NPPlayerFriendsComponent init Fail!!!");
        }

        //释放资源函数
        protected override void _discard()
        {
            _m_applyList.Clear();
            _m_friendsList.Clear();
            _m_friendGroupList.Clear();
            if (null != _m_requestDic)
                _m_requestDic.Clear();
            
            _m_refMgr?.clear();
        }

        /// <summary>
        /// 当所有组件完成之后会调用此函数
        /// 如果组件有特殊刷新需要，如高级公式计算需要。可以在这里进行统一刷新计算处理
        /// </summary>
        public override void onAllCompInited()
        {
            _m_refMgr?.init();
        }
        
        #endregion

        public void setVisitFriendInfo(NPCommonSimplePlayerInfo _info)
        {
            _m_visitFriendInfo = _info;
        }

        //添加发送好友申请时间戳
        public void addSendRequestFriends(long _cid, long _timeS)
        {
            if (null == _m_requestDic)
                _m_requestDic = new Dictionary<long, long>();

            if (_m_requestDic.ContainsKey(_cid))
                _m_requestDic[_cid] = _timeS;
            else
                _m_requestDic.Add(_cid, _timeS);
        }

        /// <summary>
        /// 好友数量已经达到上限
        /// </summary>
        /// <returns></returns>
        public bool isFriendsCountMax()
        {
            return _m_friendsList.Count >= NPPlayer.instance.playerPropertyMgr.getValue(ENPPlayerPropertyType.FRIEND_NUM);
        }

        /// <summary>
        /// 根据cid获取好友
        /// </summary>
        /// <param name="_cid"></param>
        /// <returns></returns>
        public PlayerFriendItem getFriendItem(long _cid)
        {
            foreach (PlayerFriendItem item in _m_friendsList)
            {
                if (null == item)
                    continue;

                if (item.cid == _cid)
                    return item;
            }
            return null;
        }

        /// <summary>
        /// 获取好友分组信息
        /// </summary>
        /// <param name="_dbId"></param>
        /// <returns></returns>
        public PlayerFriendGroup getFriendGroup(long _dbId)
        {
            foreach (PlayerFriendGroup friendGroup in _m_friendGroupList)
            {
                if(null == friendGroup)
                    continue;
                if (friendGroup.dbId == _dbId)
                    return friendGroup;
            }

            return null;
        }
        
        /// <summary>
        /// 自定义分组数量
        /// </summary>
        /// <returns></returns>
        public long getCustomFriendGroupCount()
        {
            return _m_friendGroupList.Count - 1;
        }

        /// <summary>
        /// 添加好友
        /// </summary>
        /// <param name="_info"></param>
        private PlayerFriendItem _addFriendItem(Friend_Info _info)
        {
            if (null != getFriendItem(_info.getFcid()))
            {
                UnityEngine.Debug.LogError($"重复好友cid{_info.getFcid()}");
                return null;
            }

            PlayerFriendItem item = new PlayerFriendItem(_info);
            _m_friendsList.Add(item);
            _addFriendToDefaultGroup(_info.getFcid());
            return item;
        }

        /// <summary>
        /// 移除好友
        /// </summary>
        /// <param name="_cid"></param>
        private void _removeFriendItem(long _cid)
        {
            PlayerFriendItem temp = null;
            for (int i = 0; i < _m_friendsList.Count; i++)
            {
                temp = _m_friendsList[i];
                if (null == temp)
                    continue;

                if (temp.cid == _cid)
                {
                    _m_friendsList.RemoveAt(i);
                    break;
                }
            }

            _removeFriendFormGroup(_cid);
        }

        /// <summary>
        /// 从分组移除好友
        /// </summary>
        /// <param name="_cid"></param>
        private void _removeFriendFormGroup(long _cid)
        {
            foreach (PlayerFriendGroup friendGroup in _m_friendGroupList)
            {
                friendGroup.removePlayer(_cid);
            }
        }

        /// <summary>
        /// 添加好友到默认分组
        /// </summary>
        /// <param name="_cid"></param>
        private void _addFriendToDefaultGroup(long _cid)
        {
            PlayerFriendGroup defaultGroup = getFriendGroup(0);
            defaultGroup?.addPlayer(_cid);
        }

        /// <summary>
        /// 根据cid获取好友申请
        /// </summary>
        /// <param name="_cid"></param>
        /// <returns></returns>

        private PlayerFriendApplyItem _getApplyItem(long _cid)
        {
            foreach (PlayerFriendApplyItem item in _m_applyList)
            {
                if (null == item)
                    continue;

                if (item.cid == _cid)
                    return item;
            }
            return null;
        }

        /// <summary>
        /// 好友申请变动 添加/修改
        /// </summary>
        /// <param name="_info"></param>
        private void _chgApplyItem(Friend_ApplyInfo _info)
        {
            PlayerFriendApplyItem item = _getApplyItem(_info.getApplyCid());
            if (null != item)
                item.updateApplyTimeS(_info.getApplyTimeS());
            else
            {
                item = new PlayerFriendApplyItem(_info);
                _m_applyList.Add(item);
            }
        }

        /// <summary>
        /// 移除好友申请
        /// </summary>
        /// <param name="_cid"></param>
        private void _removeApplyItem(long _cid)
        {
            PlayerFriendApplyItem temp = null;
            for (int i = 0; i < _m_applyList.Count; i++)
            {
                temp = _m_applyList[i];
                if (null == temp)
                    continue;

                if (temp.cid == _cid)
                {
                    _m_applyList.RemoveAt(i);
                    break;
                }
            }
        }

        /// <summary>
        /// 获取当前好友列表 取详情
        /// </summary>
        /// <param name="_doneAction"></param>
        /// <param name="_isForce">是否强制刷新</param>
        public void getFriendsDataList(Action<List<PlayerFriendItemData>> _doneAction,bool _isForce = false)
        {
            List<PlayerFriendItemData> list = new List<PlayerFriendItemData>();
            if (_m_friendsList.Count == 0)
            {
                _doneAction(list);
                return;
            }
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(_m_friendsList.Count);
            stepCounter.regAllDoneDelegate(()=> {
                _doneAction(list);
            });

            PlayerFriendItem temp = null;
            for (int i = 0; i < _m_friendsList.Count; i++)
            {
                temp = _m_friendsList[i];
                if (null == temp)
                {
                    stepCounter.addDoneStepCount();
                    continue;
                }

                if (_isForce)
                {
                    //这里在线状态会变动 所以每次都要找服务端重新拿数据
                    temp.forceGetValue(_itemData =>
                    {
                        list.Add(_itemData);
                        stepCounter.addDoneStepCount();
                    });
                }
                else
                {
                    temp.getValue(_itemData =>
                    {
                        list.Add(_itemData);
                        stepCounter.addDoneStepCount();
                    });
                }
            }
        }

        /// <summary>
        /// 获取好友分组列表
        /// </summary>
        /// <param name="_list"></param>
        /// <param name="filtGroup"></param>
        public void getFriendGroupList(List<PlayerFriendGroup> _list, Func<PlayerFriendGroup,bool> _filtGroup = null)
        {
            if(null == _list)
                return;
            if (null != _filtGroup)
            {
                foreach (PlayerFriendGroup friendGroup in _m_friendGroupList)
                {
                    if(_filtGroup(friendGroup))
                        continue;
                    _list.Add(friendGroup);
                }
            }
            else
            {
                _list.AddRange(_m_friendGroupList);
            }
            _list.Sort((_x, _y) =>
            {
                if (_x.groupIndex > _y.groupIndex)
                    return 1;
                if (_x.groupIndex < _y.groupIndex)
                    return -1;
                return 0;
            });
        }

        /// <summary>
        /// 获取申请列表
        /// </summary>
        public void getApplyList(List<PlayerFriendApplyItem> _list)
        {
            if (null == _list)
                return;

            PlayerFriendApplyItem temp = null;
            for (int i = _m_applyList.Count - 1; i >= 0; i--) 
            {
                temp = _m_applyList[i];
                if (null == temp || !temp.isAvaiable)
                {
                    _m_applyList.RemoveAt(i);
                    //告诉服务器删除
                    if (null != temp)
                        reqFriendApplyExpired(temp.cid);
                }
            }

            _list.AddRange(_m_applyList);
        }

        /// <summary>
        /// 获取好友数量
        /// </summary>
        public int getFriendsListCount()
        {
            return _m_friendsList.Count;
        }

        /// <summary>
        /// 获取好友申请数量
        /// </summary>
        public int getApplyListCount()
        {
            return _m_applyList.Count;
        }

        /// <summary>
        /// 是否为好友
        /// </summary>
        /// <param name="_cid"></param>
        /// <returns></returns>
        public bool isFriend(long _cid)
        {
            PlayerFriendItem temp = null;
            for (int i = 0; i < _m_friendsList.Count; i++)
            {
                temp = _m_friendsList[i];
                if (null == temp)
                    continue;
                if (temp.cid == _cid)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 获取屏蔽item
        /// </summary>
        public PlayerShieldItem getShieldItem(long _cid)
        {
            PlayerShieldItem item = null;
            for (int i = 0; i < _m_shieldIdList.Count; i++)
            {
                item = _m_shieldIdList[i];
                if(null == item)
                    continue;
                if (item.cid == _cid)
                    return item;
            }

            return null;
        }

        /// <summary>
        /// 获取屏蔽列表
        /// </summary>
        public void getShieldList(List<PlayerShieldItem> _list)
        {
            if (null == _list)
                return;

            _list.AddRange(_m_shieldIdList);
        }
        
        /// <summary>
        /// 是否被屏蔽
        /// </summary>
        /// <param name="_cid"></param>
        /// <returns></returns>
        public bool isShield(long _cid)
        {
            PlayerShieldItem temp = null;
            for (int i = 0; i < _m_shieldIdList.Count; i++)
            {
                temp = _m_shieldIdList[i];
                
                if (null == temp)
                    continue;
                if (temp.cid == _cid)
                    return true;
            }
            return false;
        }

        #region 红点

        //刷新申请好友列表红点
        private void _refreshApplyFriendRedTip()
        {
            long count = _m_applyList != null ? _m_applyList.Count : 0;
            // RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_FRIEND_APPLY_PAGE, count);
        }

        #endregion

        #region S2C
        public void retFriendsInitData(GS2GC_002_049_RetFriendInit _msg)
        {
            if (null == _msg)
                return;
            
            _m_friendGroupList.Clear();
            //先创建默认分组
            PlayerFriendGroup defaultGroup = new PlayerFriendGroup();
            int groupIndex = _msg.getGroupOrderList().IndexOf(defaultGroup.dbId);
            defaultGroup.setIndex(groupIndex);
            _m_friendGroupList.Add(defaultGroup);
            
            _retFriendsList(_msg.getFriendList(),defaultGroup);
            
            //更新服务端分组信息
            _initFriendGroupList(defaultGroup,_msg.getGroupList(), _msg.getGroupOrderList());

            _retApplyList(_msg.getApplyList());

            _m_initStepCounter.addDoneStepCount();
        }

        public void retShieldInit(GS2GC_002_061_RetShieldInit _msg)
        {
            _m_shieldIdList.Clear();
            foreach (long cid in _msg.getShieldCidList())
            {
                _m_shieldIdList.Add(new PlayerShieldItem(cid));
            }
            
            _m_initStepCounter.addDoneStepCount();
        }

        /// <summary>
        /// 初始化分组列表
        /// </summary>
        /// <param name="_defaultGroup"></param>
        /// <param name="_groupList"></param>
        /// <param name="_groupOrderList"></param>
        private void _initFriendGroupList(PlayerFriendGroup _defaultGroup, List<Friend_CustomGroupInfo> _groupList, List<long> _groupOrderList)
        {
            PlayerFriendGroup groupInfo = null;
            int groupIndex;
            foreach (Friend_CustomGroupInfo customGroupInfo in _groupList)
            {
                groupInfo = new PlayerFriendGroup(customGroupInfo);
                groupIndex = _groupOrderList.IndexOf(groupInfo.dbId);
                groupInfo.setIndex(groupIndex);
                _m_friendGroupList.Add(groupInfo);
                //把其它分组的cid从默认分组中移除
                _defaultGroup.removePlayer(groupInfo.playerList);
            }
        }

        /// <summary>
        /// 初始化好友申请列表
        /// </summary>
        private void _retApplyList(List<Friend_ApplyInfo> _list)
        {
            if (null == _list)
                return;

            _m_applyList.Clear();
            Friend_ApplyInfo temp = null;
            for (int i = 0; i < _list.Count; i++)
            {
                temp = _list[i];
                if (null == temp)
                    continue;

                PlayerFriendApplyItem item = new PlayerFriendApplyItem(temp);
                _m_applyList.Add(item);
            }

            //刷新好友申请红点
            _refreshApplyFriendRedTip();
        }

        /// <summary>
        /// 好友申请变动
        /// </summary>
        public void retOnFriendApplyChg(Friend_ApplyInfo _apply)
        {
            if (null == _apply)
                return;

            _chgApplyItem(_apply);
            //刷新好友申请红点
            _refreshApplyFriendRedTip();

            WinMsg.SendMsg(WinMsgType.FRIENDS_APPLY_CHG);
        }

        /// <summary>
        /// 好友申请移除
        /// </summary>
        public void retOnFriendApplyRemove(long _cid)
        {
            _removeApplyItem(_cid);
            //刷新好友申请红点
            _refreshApplyFriendRedTip();

            WinMsg.SendMsg(WinMsgType.FRIENDS_APPLY_CHG);
        }

        /// <summary>
        /// 初始化好友列表
        /// </summary>
        private void _retFriendsList(List<Friend_Info> _list, PlayerFriendGroup defaultGroup)
        {
            if (null == _list)
                return;

            _m_friendsList.Clear();
            Friend_Info temp = null;
            for (int i = 0; i < _list.Count; i++)
            {
                temp = _list[i];
                if (null == temp)
                    continue;

                PlayerFriendItem item = new PlayerFriendItem(temp);
                _m_friendsList.Add(item);
                //初始化好友先都丢到默认分组中去
                defaultGroup.addPlayer(item.cid);
            }
        }

        /// <summary>
        /// 好友变动
        /// </summary>
        public void retOnFriendChg(GS2GC_021_063_OnFriendChg _msg)
        {
            if (null == _msg || null == _msg.getFriend())
                return;

            PlayerFriendItem item = _addFriendItem(_msg.getFriend());
            if (null == item)
                return;

            // 侧边提示  同意方不需要
            // if (!_msg.getIsAgree())
            // {
            //     item.getValue(_itemData =>
            //     {
            //         NPGUIAddSceneCenterTip.instance.showTextTip(TextTranslate.instance.getLanguage(TransKeyConst.friends_add_suc_str, _itemData.playerInfo.name), NPConst.C_FRIENDS_TEXT_TIP_REF_ID);
            //     });
            // }
            WinMsg.SendMsg(WinMsgType.FRIENDS_CHG);
        }

        /// <summary>
        /// 好友移除
        /// </summary>
        public void retOnFriendRemove(long _cid)
        {
            if (0 == _cid)
                return;

            _removeFriendItem(_cid);
            WinMsg.SendMsg(WinMsgType.FRIENDS_CHG);
        }

        /// <summary>
        /// 好友分组顺序变更
        /// </summary>
        /// <param name="_msg"></param>
        public void onFriendGroupOrderListChg(GS2GC_021_055_OnFriendGroupOrderListChg _msg)
        {
            foreach (PlayerFriendGroup friendGroup in _m_friendGroupList)
            {
                friendGroup.setIndex(_msg.getFriendGroupOrderList().IndexOf(friendGroup.dbId));
            }
            WinMsg.SendMsg(WinMsgType.FRIENDS_GROUP_CHG);
        }

        /// <summary>
        /// 分组创建
        /// </summary>
        /// <param name="_msg"></param>
        public void onFriendGroupCreate(GS2GC_021_056_OnFriendGroupCreate _msg)
        {
            PlayerFriendGroup friendGroup = new PlayerFriendGroup();
            friendGroup.setIndex(_m_friendGroupList.Count);
            friendGroup.setName(_msg.getName());
            friendGroup.setDBId(_msg.getGroupDbId());
            _m_friendGroupList.Add(friendGroup);
            
            //把其它分组的cid从默认分组中移除
            PlayerFriendGroup defaultGroup = getFriendGroup(0);
            defaultGroup?.removePlayer(friendGroup.playerList);
            
            WinMsg.SendMsg(WinMsgType.FRIENDS_GROUP_CHG);
        }

        /// <summary>
        /// 删除分组
        /// </summary>
        /// <param name="_msg"></param>
        public void onFriendGroupDelete(GS2GC_021_057_OnFriendGroupDelete _msg)
        {
            long removeGroupId = _msg.getGroupDbId();
            PlayerFriendGroup removeFriendGroup = getFriendGroup(removeGroupId);
            if (null == removeFriendGroup)
                return;
            PlayerFriendGroup defaultFriendGroup = null;
            //设置排序
            foreach (PlayerFriendGroup friendGroup in _m_friendGroupList)
            {
                if (friendGroup.isDefault)
                {
                    defaultFriendGroup = friendGroup;
                }
                if (friendGroup.groupIndex <= removeFriendGroup.groupIndex)
                    continue;
                
                friendGroup.setIndex(friendGroup.groupIndex - 1);

            }
            //转移好友到默认分组
            if(null != defaultFriendGroup)
                defaultFriendGroup.addPlayer(removeFriendGroup.playerList);
            //移除数据
            _m_friendGroupList.Remove(removeFriendGroup);
            
            WinMsg.SendMsg(WinMsgType.FRIENDS_GROUP_CHG);
        }

        /// <summary>
        /// 好友归属分组变更
        /// </summary>
        /// <param name="_msg"></param>
        public void onFriendBelongGroupChg(GS2GC_021_058_OnFriendBelongGroupChg _msg)
        {
            foreach (PlayerFriendGroup friendGroup in _m_friendGroupList)
            {
                if (friendGroup.dbId == _msg.getTargetGroupDbId())//加到新分组中
                {
                    friendGroup.addPlayer(_msg.getCidList());
                }
                else//从旧分组中移除
                {
                    friendGroup.removePlayer(_msg.getCidList());
                }

            }
            WinMsg.SendMsg(WinMsgType.FRIENDS_GROUP_CHG);
        }

        /// <summary>
        /// 好友分组名变更
        /// </summary>
        /// <param name="_msg"></param>
        public void onFriendGroupNameChg(GS2GC_021_059_OnFriendGroupNameChg _msg)
        {
            PlayerFriendGroup friendGroup = getFriendGroup(_msg.getGroupDbId());
            if(null != friendGroup)
                friendGroup.setName(_msg.getName());
            WinMsg.SendMsg(WinMsgType.FRIENDS_GROUP_CHG);
        }

        /// <summary>
        /// 屏蔽玩家CID新增推送
        /// </summary>
        /// <param name="_msg"></param>
        public void onShieldCidAdd(GS2GC_004_071_OnShieldCidAdd _msg)
        {
            
            NPPrivateChatInfo privateChatInfo = NPPlayer.instance.chatComp.getPrivateChatInfo(_msg.getCid());
            //如果有私聊，删除私聊频道
            if(null != privateChatInfo)
                NPPlayer.instance.chatComp.removePrivateChannel(privateChatInfo);  
                    
            _m_shieldIdList.Add(new PlayerShieldItem(_msg.getCid()));
            WinMsg.SendMsg(WinMsgType.FRIENDS_SHIELD_CHG);

        }

        /// <summary>
        /// 屏蔽玩家CID移除推送
        /// </summary>
        /// <param name="_msg"></param>
        public void onShieldCidRemove(GS2GC_004_072_OnShieldCidRemove _msg)
        {
            PlayerShieldItem item = getShieldItem(_msg.getCid());
            if (null != item)
            {
                _m_shieldIdList.Remove(item);
            }
            WinMsg.SendMsg(WinMsgType.FRIENDS_SHIELD_CHG);

        }

        #endregion

        #region C2S

        /// <summary>
        /// 好友组件初始化
        /// </summary>
        private void _reqFriendInitData()
        {
            NPGSClientListener.sendMsgByLog(GSWriter_002_InitOp.make_002_049_ReqFriendInit());
        }

        private void _reqShieldInit()
        {
            NPGSClientListener.sendMsgByLog(GSWriter_002_InitOp.make_002_061_ReqShieldInit());
        }

        /// <summary>
        /// 申请添加好友
        /// </summary>
        public void reqSendFriendApply(long _cid, Action _backAction = null, Action _failAction = null)
        {
            NPGSClientListener.sendRequestByLog(NPGSWriter_021_PlayerInfoOp.make_030_ReqSendFriendApply(_cid),
                 new CommonRequestCallbackProtocolDealer<GS2GC_021_030_RetSendFriendApply>((info) =>
                 {
                     if (null != _backAction)
                         _backAction();
                 }, (_errCode) =>
                 {
                     NPGUIAddSceneCenterTip.instance.showErrorInfo(_errCode);
                     _failAction?.Invoke();
                 }));
        }

        /// <summary>
        /// 同意/拒绝好友申请
        /// </summary>
        /// <param name="_cid"></param>
        /// <param name="_backAction"></param>
        public void reqDealFriendApply(bool _isAgree, long _cid, Action _backAction = null)
        {
            //数量已达到上限
            if (_isAgree && isFriendsCountMax())
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.friends_countIsMax_none);
                return;
            }

            NPGSClientListener.sendRequestByLog(NPGSWriter_021_PlayerInfoOp.make_031_ReqDealFriendApply(_isAgree, _cid),
                 new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_021_031_RetDealFriendApply>((info) =>
                 {
                     if (null != _backAction)
                         _backAction();
                 }));
        }

        /// <summary>
        /// 好友申请过期
        /// </summary>
        /// <param name="_applyInstanceId"></param>
        public void reqFriendApplyExpired(long _applyCid, Action _backAction = null)
        {
            NPGSClientListener.sendRequestByLog(NPGSWriter_021_PlayerInfoOp.make_033_ReqFriendApplyExpired(_applyCid),
                 new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_021_031_RetDealFriendApply>((info) =>
                 {
                     if (null != _backAction)
                         _backAction();
                 }));
        }

        /// <summary>
        /// 删除好友
        /// </summary>
        public void reqRemoveFriend(long _cid, Action _backAction = null)
        {
            NPGSClientListener.sendRequestByLog(NPGSWriter_021_PlayerInfoOp.make_032_ReqRemoveFriend(_cid),
               new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_021_032_RetRemoveFriend>((info) =>
               {
                   if (null != _backAction)
                       _backAction();
               }));
        }

        /// <summary>
        /// 推荐好友
        /// </summary>
        public void reqFriendRecommend(Action<GS2GC_021_034_RetFriendRecommend> _backAction = null)
        {
            NPGSClientListener.sendRequestByLog(NPGSWriter_021_PlayerInfoOp.make_034_ReqFriendRecommend(),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_021_034_RetFriendRecommend>((info) =>
                {
                    if (null != _backAction)
                        _backAction(info);
                }));
        }

        /// <summary>
        /// 创建分组
        /// </summary>
        public void reqCreateFriendGroup(string _groupName, List<long> _cidList, Action<GS2GC_021_035_RetCreateFriendGroup> _backAction = null)
        {
            // if (GRefdataCoreMgr.instance.npGeneral.friends_group_count_limit < _m_friendGroupList.Count)
            // {
            //     NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.friends_group_count_max));
            //     return;
            // }
            
            NPGSClientListener.sendRequestByLog(NPGSWriter_021_PlayerInfoOp.make_035_ReqCreateFriendGroup(_groupName, _cidList),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_021_035_RetCreateFriendGroup>((info) =>
                {
                    if (null != _backAction)
                        _backAction(info);
                }));
        }

        /// <summary>
        /// 修改所属分组
        /// </summary>
        public void reqChgBelongFriendGroup(List<long> _cidList, long _groupDbId, Action<GS2GC_021_036_RetChgBelongFriendGroup> _backAction = null)
        {
            NPGSClientListener.sendRequestByLog(NPGSWriter_021_PlayerInfoOp.make_036_ReqChgBelongFriendGroup(_cidList, _groupDbId),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_021_036_RetChgBelongFriendGroup>((info) =>
                {
                    if (null != _backAction)
                        _backAction(info);
                }));
        }

        /// <summary>
        /// 删除分组
        /// </summary>
        public void reqDeleteFriendGroup(long _groupDbId, Action<GS2GC_021_037_RetDeleteFriendGroup> _backAction = null)
        {
            NPGSClientListener.sendRequestByLog(NPGSWriter_021_PlayerInfoOp.make_037_ReqDeleteFriendGroup(_groupDbId),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_021_037_RetDeleteFriendGroup>((info) =>
                {
                    if (null != _backAction)
                        _backAction(info);
                }));
        }

        /// <summary>
        /// 分组排序
        /// </summary>
        public void reqChgFriendGroupOrderList(List<long> _groupIdList, Action<GS2GC_021_038_RetChgFriendGroupOrderList> _backAction = null)
        {
            NPGSClientListener.sendRequestByLog(NPGSWriter_021_PlayerInfoOp.make_038_ReqChgFriendGroupOrderList(_groupIdList),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_021_038_RetChgFriendGroupOrderList>((info) =>
                {
                    if (null != _backAction)
                        _backAction(info);
                }));
        }

        /// <summary>
        /// 修改分组名字
        /// </summary>
        public void reqChgFriendGroupName(long _groupDbId, string _name, Action<GS2GC_021_039_RetChgFriendGroupName> _backAction = null)
        {
            NPGSClientListener.sendRequestByLog(NPGSWriter_021_PlayerInfoOp.make_039_ReqChgFriendGroupName(_groupDbId, _name),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_021_039_RetChgFriendGroupName>((info) =>
                {
                    if (null != _backAction)
                        _backAction(info);
                }));
        }

        /// <summary>
        /// 庭院拜访
        /// </summary>
        public void reqGainVisitOtherPlayerReward(Action<List<NPCommon.NPCommon_ItemInfo>> _doneAction, Action _failAction)
        {
            NPGSClientListener.sendRequestByLog(NPGSWriter_004_PlayerOp.make_004_014_ReqGainVisitOtherPlayerReward(),
                new CommonRequestCallbackProtocolDealer<GS2GC_004_014_RetGainVisitOtherPlayerReward>((info) =>
                {
                    if (null == info)
                        return;

                    if (null != _doneAction)
                        _doneAction(info.getItemList());

                    WinMsg.SendMsg(WinMsgType.FRIENDS_VISIT_REWARD_GET);
                }, (_errCode) =>
                {
                    if (null != _failAction)
                        _failAction();
                }));
        }

        /// <summary>
        /// 设置屏蔽玩家
        /// </summary>
        public void reqSetShieldPlayer(long _cid, Action<GS2GC_004_031_RetSetShieldPlayer> _doneAction = null)
        {
            if (_m_shieldIdList.Count >= GRefdataCoreMgr.instance.npGeneral.shield_cid_limit)
            {
                //已达上限
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.friend_shield_cid_limit));
                return;
            }
            
            if (isShield(_cid))
            {
                //已经在屏蔽列表
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.friend_shield_isShield_none));
                return;
            }
            
            NPGSClientListener.sendRequestByLog(NPGSWriter_004_PlayerOp.make_004_031_ReqSetShieldPlayer(_cid),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_004_031_RetSetShieldPlayer>((info) =>
                {
                    if (null == info)
                        return;
                    if (null != _doneAction)
                        _doneAction(info);
                }));
        }

        /// <summary>
        /// 取消屏蔽玩家
        /// </summary>
        public void reqUnsetShieldPlayer(long _cid, Action<GS2GC_004_032_RetUnsetShieldPlayer> _doneAction = null)
        {
            NPGSClientListener.sendRequestByLog(NPGSWriter_004_PlayerOp.make_004_032_ReqUnsetShieldPlayer(_cid),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_004_032_RetUnsetShieldPlayer>((info) =>
                {
                    if (null == info)
                        return;
                    if (null != _doneAction)
                        _doneAction(info);
                }));
        }

        #endregion
    }
}
