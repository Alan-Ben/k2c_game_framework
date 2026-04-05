using System.Collections.Generic;
using System;
using ALPackage;
using Common.MailObj;
using GS2GC.p009_MailOp;

namespace GOE
{
    //管理器
    public partial class GPlayerMailComponent : _ANPBasicPlayerComponent
    {
        //邮件未读数量（其它旧界面有用到，后续如果不用再删除）
        private int _m_iNoReadCount;

        /// <summary>
        /// 检测过期邮件删除的间隔
        /// </summary>
        private int _m_checkDelMailIntervalTime = 1;
        /// <summary>
        /// 检测过期邮件task
        /// </summary>
        private ALCommonEnableTaskController _m_iCheckDelTask;
        //邮件列表
        private List<GMailDataInfo> _m_lDataList;
        //邮件总状态信息
        private Mail_StatInfo _m_mailStatInfo;


        //请求简要列表的是否已经请求
        private bool _m_bListIsInit;
        //是否已经发送列表请求
        private bool _m_bListSended;
        //请求邮件列表的回调
        private Action _m_dReqListDelegate;
        private RedTipDealer _m_redDealer;

        //构造函数
        public GPlayerMailComponent(NPPlayerComponentMgr _compMgr) : base(_compMgr)
        {
            _m_lDataList = new List<GMailDataInfo>();
            _m_mailStatInfo = null;

            _m_bListIsInit = false;
            _m_bListSended = false;
            _m_dReqListDelegate = null;
            _m_redDealer = new RedTipDealer(this);
        }

        public int noReadCount { get { return _m_iNoReadCount; } }
        //事件

        //属性
        public override bool isMustInit { get { return true; } }

        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.MAIL; } }

        public override ENPPlayerCompType[] dependCompList { get { return null; } }

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
            //请求邮件信息
            reqMailStatInfo();
        }


        protected override void _dealInit()
        {
        }

        //组件加载完成时的调用
        protected override void _onInitDone()
        {
            //开启定时检测
            _m_iCheckDelTask.setDisable();
            _m_iCheckDelTask = ALCommonTaskController.CommonEnableDurationActionAddMonoTask(_doDelMailPerSecond, _m_checkDelMailIntervalTime);
        }

        //组件初始化失败的处理
        protected override void _onInitFail()
        {
            ALLog.Error("NPPlayerChatComponent init Fail!!!");
        }

        //释放资源函数
        protected override void _discard()
        {
            _m_iCheckDelTask.setDisable();

            if (null != _m_dReqListDelegate)
                _m_dReqListDelegate();
            _m_dReqListDelegate = null;
        }

        /// <summary>
        /// 每秒删除过期邮件
        /// </summary>
        private void _doDelMailPerSecond()
        {
            if (null == _m_lDataList)
                return;

            int judgeIndex = _m_lDataList.Count;
            //默认设置无删除处理
            bool isRemove = false;
            long removeId = 0;
            GMailDataInfo _info = null;
            while (--judgeIndex > -1)
            {
                _info = _m_lDataList[judgeIndex];
                if (null == _info)
                {
                    _m_lDataList.RemoveAt(judgeIndex);
                    continue;
                }

                //过期邮件，本地删除
                if (_info.isExceed())
                {
                    //记录id，防止移除后info为空取不到id
                    removeId = _info.id;
                    _m_lDataList.RemoveAt(judgeIndex);
                    //自动移除不发消息，等重开列表或者有啥其它操作的时候才刷新列表
                    //WinMsg.SendMsg(WinMsgType.ON_DEL_MAIL, removeId);
                    isRemove = true;
                }
            }
            _info = null;

            if (isRemove)
            {
                WinMsg.SendMsg(WinMsgType.ON_MAIL_STAT_CHG);
            }
        }

        /// <summary>
        /// 获取邮件列表
        /// </summary>
        /// <param name="_list"></param>
        public void getMailList(List<GMailDataInfo> _list)
        {
            if (null == _list)
            {
                return;
            }
            _list.AddRange(_m_lDataList);
        }


        /// <summary>
        /// 未领取数量
        /// </summary>
        /// <returns></returns>
        public int getUnGetRewardCount()
        {
            if (null == _m_mailStatInfo)
                return 0;

            if (_m_bListIsInit)
            {
                int unGetRewardCount = 0;
                for (int i = 0; i < _m_lDataList.Count; i++)
                {
                    if (_m_lDataList[i].getHasItem() && !_m_lDataList[i].getHasTaken())
                    {
                        unGetRewardCount++;
                    }
                }
                return unGetRewardCount;
            }
            return _m_mailStatInfo.getUnTakeCount();
        }
        
        /// <summary>
        /// 未读数量
        /// </summary>
        /// <returns></returns>
        public int getUnReadCount()
        {
            if (null == _m_mailStatInfo)
                return 0;

            if (_m_bListIsInit)
            {
                int unReadCount = 0;
                for (int i = 0; i < _m_lDataList.Count; i++)
                {
                    if (!_m_lDataList[i].getIsRead())
                    {
                        unReadCount++;
                    }
                }
                return unReadCount;
            }
            return _m_mailStatInfo.getUnReadCount();
        }

        /// <summary>
        /// 现有邮件数量
        /// </summary>
        /// <returns></returns>
        public int getTotalMailCount()
        {
            if (null == _m_mailStatInfo)
                return 0;

            if (_m_bListIsInit)
            {
                return _m_lDataList.Count;
            }

            return _m_mailStatInfo.getTotalMailCount();
        }


        /// <summary>
        /// 现有收藏邮件数量
        /// </summary>
        /// <returns></returns>
        public int getTotalLockMailCount()
        {
            if (null == _m_mailStatInfo)
                return 0;

            if (_m_bListIsInit)
            {
                int lockCount = 0;
                foreach (GMailDataInfo mailInfo in _m_lDataList)
                {
                    lockCount += mailInfo.getIsLocked() ? 1 : 0;
                }
                return lockCount;
            }

            return _m_mailStatInfo.getTotalMailCount();
        }


        /// <summary>
        /// 根据id获取邮件信息
        /// </summary>
        /// <param name="_mailUid"></param>
        /// <returns></returns>
        public GMailDataInfo getMailDataInfo(long _mailUid)
        {
            GMailDataInfo tmpInfo = null;
            for (int i = 0; i < _m_lDataList.Count; i++)
            {
                tmpInfo = _m_lDataList[i];
                if (null == tmpInfo)
                    continue;

                if (tmpInfo.id == _mailUid)
                {
                    return tmpInfo;
                }
            }

            return null;
        }

        /// <summary>
        /// 根据邮件id获取第一封匹配邮件Id的邮件信息
        /// </summary>
        /// <param name="_mailUid"></param>
        /// <returns></returns>
        public GMailDataInfo getFirstMailByMailRefId(long _mailRefId)
        {
            GMailDataInfo tmpInfo = null;
            for (int i = 0; i < _m_lDataList.Count; i++)
            {
                tmpInfo = _m_lDataList[i];
                if (null == tmpInfo)
                    continue;

                if (tmpInfo.mailRef != null && tmpInfo.mailRef.id == _mailRefId)
                {
                    return tmpInfo;
                }
            }

            return null;
        }

        /// <summary>
        /// 获取邮件收藏列表
        /// </summary>
        /// <param name="_list"></param>
        public void getLockedMailList(List<GMailDataInfo> _list)
        {
            if (null == _list)
                return;

            _list.AddRange(_m_lDataList.FindAll((a) => { return a.getIsLocked(); }));
        }


        public void clear()
        {
            _m_bListIsInit = false;
            _m_bListSended = false;
            
            _m_lDataList.Clear();

            if (null != _m_dReqListDelegate)
                _m_dReqListDelegate();
            _m_dReqListDelegate = null;
            
            _m_redDealer?.clear();
        }
        #region S2C
        //通用邮件

        /// <summary>
        /// 邮件总状态信息
        /// </summary>
        /// <param name="_msg"></param>
        public void retMailStatInfo(GS2GC.p002_InitOp.GS2GC_002_014_RetMailStatInfo _msg)
        {
            clear();
            _m_mailStatInfo = _msg.getMailStatInfo();
            _m_redDealer?.init();
            setInitDone();
        }

        /// <summary>
        /// 邮件简要信息列表
        /// </summary>
        /// <param name="_msg"></param>
        private void _retMailBriefList(GS2GC.p009_MailOp.GS2GC_009_002_RetMailBriefList _msg)
        {
            GMailDataInfo _info = null;
            for (int i = 0; i < _msg.getBriefList().Count; i++)
            {
                _info = new GMailDataInfo(_msg.getBriefList()[i]);
                _m_lDataList.Add(_info);
            }

            _m_bListIsInit = true;
            WinMsg.SendMsg(WinMsgType.ON_INIT_MAIL_LIST);

            //调用回调
            if (null != _m_dReqListDelegate)
                _m_dReqListDelegate();
            _m_dReqListDelegate = null;
        }

        /// <summary>
        /// 邮件详情
        /// </summary>
        /// <param name="_msg"></param>
        private void _retMailDetail(GS2GC.p009_MailOp.GS2GC_009_003_RetMailDetail _msg)
        {
            Mail_DetailInfo info = _msg.getMailDetail();
            for (int i = 0; i < _m_lDataList.Count; i++)
            {
                if (info.getMailUid() == _m_lDataList[i].id)
                {
                    _m_lDataList[i].setDetail(info);
                    //打开就是已读
                    _m_lDataList[i].setIsRead();
                    WinMsg.SendMsg(WinMsgType.ON_SET_MAIL_DETAIL, info.getMailUid());
                    break;
                }
            }
        }

        /// <summary>
        /// 列表展示信息返回
        /// </summary>
        /// <param name="msg"></param>
        private void _retMailTitle(GS2GC_009_010_RetMailTitleInfo _msg)
        {
            List<Mail_TitleInfo> titleList = _msg.getTitleList();
            GMailDataInfo _info = null;
            Mail_TitleInfo _titleInfo = null;
            for (int i = 0; i < titleList.Count; i++)
            {
                _titleInfo = titleList[i];
                for (int k = 0; k < _m_lDataList.Count; k++)
                {
                    _info = _m_lDataList[k];
                    if (_titleInfo.getMailUid() == _info.id)
                    {
                        _info.setMailTitleInfo(_titleInfo);
                        break;
                    }
                }
            }
        }

        
        /// <summary>
        /// 有新邮件
        /// </summary>
        /// <param name="_msg"></param>
        public void onMailAdded(GS2GC_009_051_OnMailAdded _msg)
        {
            if (!_m_bListIsInit)
            {
                //没有简要列表的情况下维护 StatInfo
                if (null != _m_mailStatInfo)
                {
                    int unreadCount = _m_mailStatInfo.getUnReadCount();
                    int totalMailCount = _m_mailStatInfo.getTotalMailCount();
                    _m_mailStatInfo.setTotalMailCount(totalMailCount + 1); //新邮件总数加1
                    _m_mailStatInfo.setUnReadCount(unreadCount + 1); //新邮件未读加1
                    WinMsg.SendMsg(WinMsgType.ON_MAIL_STAT_CHG);
                }
                return;
            }

            GMailDataInfo _info = new GMailDataInfo(_msg.getBriefInfo());
            _info.setMailTitleInfo(_msg.getTitleInfo());
            _m_lDataList.Insert(0, _info);

            WinMsg.SendMsg(WinMsgType.ON_ADD_MAIL, _info.id);
            WinMsg.SendMsg(WinMsgType.ON_MAIL_STAT_CHG);
        }

        /// <summary>
        /// 邮件移除
        /// </summary>
        /// <param name="_msg"></param>
        public void onMailRemoved(GS2GC_009_052_OnMailRemoved _msg)
        {
            if (!_m_bListIsInit)
            {          
                //没有简要列表的情况下维护 StatInfo
                if (null != _m_mailStatInfo)
                {
                    int unreadCount = _m_mailStatInfo.getUnReadCount();
                    int totalMailCount = _m_mailStatInfo.getTotalMailCount();
                    _m_mailStatInfo.setTotalMailCount(totalMailCount - 1);
                    if (!_msg.getIsRead())//如果删除的是未读邮件，未读数量-1
                    {
                        _m_mailStatInfo.setUnReadCount(unreadCount - 1);
                    }
                    WinMsg.SendMsg(WinMsgType.ON_MAIL_STAT_CHG);
                }
                return;
            }

            for (int i = 0; i < _m_lDataList.Count; i++)
            {
                if (_m_lDataList[i].id == _msg.getMailUid())
                {
                    _m_lDataList.RemoveAt(i);
                    WinMsg.SendMsg(WinMsgType.ON_DEL_MAIL, _msg.getMailUid());
                    WinMsg.SendMsg(WinMsgType.ON_MAIL_STAT_CHG);
                    break;
                }
            }
        }

        /// <summary>
        /// 邮件收藏状态变化
        /// </summary>
        /// <param name="_msg"></param>
        public void onMailLockedUpdated(GS2GC_009_053_OnMailLockedUpdated _msg)
        {
            if (!_m_bListIsInit)
            {
                return;
            }
            for (int i = 0; i < _m_lDataList.Count; i++)
            {
                if (_m_lDataList[i].id == _msg.getMailUid())
                {
                    _m_lDataList[i].setLockStat(_msg.getIsLocked());
                    break;
                }
            }
        }

        /// <summary>
        /// 邮件已读的时候
        /// </summary>
        /// <param name="_msg"></param>
        public void onMailReaded(GS2GC_009_054_OnMailReaded _msg)
        {
            if (!_m_bListIsInit)
            {
                return;
            }
            List<long> mailIdLists = _msg.getMailUidList();
            if (null == mailIdLists)
            {
                return;
            }
            for (int i = 0; i < _m_lDataList.Count; i++)
            {
                if (mailIdLists.Contains(_m_lDataList[i].id))
                {
                    _m_lDataList[i].setIsRead();
                }
            }
        }

        /// <summary>
        /// 邮件附件奖励领取的时候
        /// </summary>
        /// <param name="_msg"></param>
        public void onMailRewardTaken(GS2GC_009_055_OnMailRewardTaken _msg)
        {
            if (!_m_bListIsInit)
            {
                return;
            }
            List<long> mailIdLists = _msg.getMailUidList();
            if (null == mailIdLists)
            {
                return;
            }
            for (int i = 0; i < _m_lDataList.Count; i++)
            {
                if (mailIdLists.Contains(_m_lDataList[i].id))
                {
                    if (!_m_lDataList[i].getHasTaken())
                    {
                        _m_lDataList[i].setHasTaken();
                    }
                    //break;
                }
            }
        }

        /// <summary>
        /// 邮件额外信息变化
        /// </summary>
        /// <param name="_msg"></param>
        public void onMailExDataUpdated(GS2GC_009_056_OnMailExDataUpdated _msg)
        {
            if (!_m_bListIsInit)
            {
                return;
            }
            for (int i = 0; i < _m_lDataList.Count; i++)
            {
                if (_m_lDataList[i].id == _msg.getMailUid())
                {
                    _m_lDataList[i].setExData(_msg.getExData());
                    break;
                }
            }
        }

        /// <summary>
        /// 邮件已完整读完
        /// </summary>
        /// <param name="_msg"></param>
        public void onMailReadOver(GS2GC_009_057_OnMailReadOver _msg)
        {
            if (!_m_bListIsInit)
            {
                return;
            }
            for (int i = 0; i < _m_lDataList.Count; i++)
            {
                if (_m_lDataList[i].id == _msg.getMailUid())
                {
                    _m_lDataList[i].setIsReadOver();
                    break;
                }
            }
        }

        /// <summary>
        /// 截至时间变化
        /// </summary>
        /// <param name="_msg"></param>
        public void onMailExpiredSecChg(GS2GC_009_058_OnMailExpiredSecChg _msg)
        {
            if (!_m_bListIsInit)
            {
                return;
            }
            for (int i = 0; i < _m_lDataList.Count; i++)
            {
                if (_m_lDataList[i].id == _msg.getMailUid())
                {
                    _m_lDataList[i].setMailExpiredSec(_msg.getExpiredTimeSec());
                    break;
                }
            }
        }


        #endregion

        #region C2S
        /// <summary>
        /// 请求邮件的总的状态信息
        /// </summary>
        public void reqMailStatInfo()
        {
            NPGSClientListener.sendMsgByLog(GSWriter_002_InitOp.make_014_ReqMailStatInfo());
        }
        /// <summary>
        /// 请求邮件的简要信息列表
        /// </summary>
        public void reqMailBreifList(Action _delegate)
        {
            if (_m_bListIsInit) //说明已经请求过列表
            {
                if (null != _delegate)
                    _delegate();
                return;
            }

            if (null == _m_mailStatInfo)
            {
                if (null != _delegate)
                    _delegate();
                return;
            }

            if (null != _delegate)
            {
                //注册回调
                if (null == _m_dReqListDelegate)
                    _m_dReqListDelegate = _delegate;
                else
                    _m_dReqListDelegate += _delegate;
            }

            if (!_m_bListSended)
            {
                NPGSClientListener.sendRequestByLog(GSWriter_009_MailOp.make_002_ReqBriefList(),
                    new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_009_002_RetMailBriefList>((_info) =>
                    {
                        _retMailBriefList(_info);
                    }));
                _m_bListSended = true;
            }
        }
        /// <summary>
        /// 请求邮件详情
        /// </summary>
        /// <param name="_mailUid"></param>
        public void reqMailDetail(long _mailUid, Action _dealDone = null)
        {
            NPGSClientListener.sendRequestByLog(GSWriter_009_MailOp.make_003_ReqMailDetail(_mailUid),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_009_003_RetMailDetail>((_info) =>
                {
                    _retMailDetail(_info);
                    _dealDone?.Invoke();
                }));
        }

        /// <summary>
        /// 请求邮件列表的展示数据
        /// </summary>
        /// <param name="_mailUid"></param>
        public void reqMailTitle(long _mailUid,Action _dealDone = null)
        {
            NPGSClientListener.sendRequestByLog(GSWriter_009_MailOp.make_010_ReqMailTitleInfo(_mailUid),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_009_010_RetMailTitleInfo>((_info) =>
                {
                    _retMailTitle(_info);
                    _dealDone?.Invoke();
                }));
        }

        /// <summary>
        /// 请求领取附件奖励
        /// </summary>
        /// <param name="_mailUid"></param>
        public void reqTakeMailItems(long _mailUid,Action _dealDone = null)
        {

            NPGSClientListener.sendRequestByLog(GSWriter_009_MailOp.make_004_ReqTakeMailItems(_mailUid),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_009_004_RetTakeMailItems>((_info) =>
                {
                    _dealDone?.Invoke();
                }));
        }

        /// <summary>
        /// 请求设置邮件收藏状态
        /// </summary>
        /// <param name="_mailUid"></param>
        /// <param name="_isLocked"></param>
        public void reqSetMailLockState(long _mailUid, bool _isLocked,Action _dealDone = null)
        {
            NPGSClientListener.sendRequestByLog(GSWriter_009_MailOp.make_005_ReqSetMailLockState(_mailUid, _isLocked),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_009_005_RetSetMailLockState>((_info) =>
                {
                    _dealDone?.Invoke();
                }));
        }

        /// <summary>
        /// 请求一键领取
        /// </summary>
        public void reqAKeyTakeAll(Action _dealDone = null)
        {
            NPGSClientListener.sendRequestByLog(GSWriter_009_MailOp.make_006_ReqAKeyTakeAll(),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_009_006_RetAKeyTakeAll>((_info) =>
                {
                    _dealDone?.Invoke();
                }));
        }

        /// <summary>
        /// 请求删除邮件
        /// </summary>
        /// <param name="_mailUid"></param>
        public void reqDelMail(long _mailUid,Action _dealDone = null)
        {
            NPGSClientListener.sendRequestByLog(GSWriter_009_MailOp.make_007_ReqDelMail(_mailUid),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_009_007_RetDelMail>((_info) =>
                {
                    _dealDone?.Invoke();
                }));
        }

        /// <summary>
        /// 一键删除邮件
        /// </summary>
        public void reqAkeyDelAll(Action _dealDone = null)
        {
            NPGSClientListener.sendRequestByLog(GSWriter_009_MailOp.make_008_ReqAkeyDelAll(),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_009_008_RetAKeyDelAll>((_info) =>
                {
                    _dealDone?.Invoke();
                }));
        }

        /// <summary>
        /// 设置邮件已完整读完
        /// </summary>
        /// <param name="_mailUid"></param>
        public void reqSetReadOver(long _mailUid, Action _dealDone = null)
        {
            NPGSClientListener.sendRequestByLog(GSWriter_009_MailOp.make_009_ReqSetReadOver(_mailUid),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_009_009_RetSetReadOver>((_info) =>
                {
                    _dealDone?.Invoke();
                }));
        }

        #endregion

        /// <summary>
        /// 获取是否有可以一键删除邮件
        /// </summary>
        /// <returns></returns>
        public bool hasCanAKeyDeleteMail()
        {
            return hasCanAKeyDeleteMail(_m_lDataList);
        }

        public bool hasCanAKeyDeleteMail(List<GMailDataInfo> _mailList)
        {
            if (!_m_bListIsInit || _mailList == null)
                return false;
            
            foreach (GMailDataInfo dataInfo in _mailList)
            {
                // 存在已读，且(无奖励需要领取 或 奖励已领取)，且非收藏的邮件
                if (dataInfo != null && dataInfo.getIsRead() && (!dataInfo.getHasItem() || dataInfo.getHasTaken()) && !dataInfo.getIsLocked())
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 获取是否有可以一键处理邮件
        /// </summary>
        /// <returns></returns>
        public bool hasCanAKeyDealMail()
        {
            return hasCanAKeyDealMail(_m_lDataList);
        }

        public bool hasCanAKeyDealMail(List<GMailDataInfo> _mailList)
        {
            if (!_m_bListIsInit || _mailList == null)
                return false;
            foreach (GMailDataInfo dataInfo in _mailList)
            {
                if(dataInfo == null)
                    continue;
                
                // 非必读邮件，未读或者有奖励未领取
                if(!dataInfo.getIsNeedRead() && (!dataInfo.getIsRead() || (dataInfo.getHasItem() && !dataInfo.getHasTaken())))
                    return true;
                
                // 必读邮件, 已读 且 有奖励可领取
                if(dataInfo.getIsNeedRead() && dataInfo.getIsRead() && dataInfo.getHasItem() && !dataInfo.getHasTaken())
                    return true;
            }
            return false;
        }
        
        /// <summary>
        /// 是否存在未读的必读邮件
        /// </summary>
        /// <returns></returns>
        public bool hasUnReadNeedReadMail()
        {
            return hasUnReadNeedReadMail(_m_lDataList);
        }

        public bool hasUnReadNeedReadMail(List<GMailDataInfo> _mailList)
        {
            if (!_m_bListIsInit || _mailList == null)
                return false;
            foreach (GMailDataInfo dataInfo in _mailList)
            {
                if(dataInfo == null)
                    continue;
                
                // 必读邮件, 未读
                if(dataInfo.getIsNeedRead() && !dataInfo.getIsRead())
                    return true;
            }
            return false;
        }
    }

}

