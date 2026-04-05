
using NPCommon;
using System;
using System.Collections.Generic;
using Common.MailObj;
using NPEnum;
using UnityEngine;

namespace GOE
{
    public class GMailDataInfo
    {
        //详细信息初始化完成
        private bool _m_bDetailInited;
        //是否发送了请求
        private bool _m_bSendDetailRequest;
        //是否发送了请求
        private bool _m_bSendTitleRequest;
        //邮件ID
        private long _m_lId;
        //邮件简要数据
        private Mail_BriefInfo _m_briefInfo;

        //列表信息初始化完成
        private bool _m_bTitleInited;
        /// <summary>
        /// 邮件列表展示信息
        /// </summary>
        private Mail_TitleInfo _m_titleInfo;
        //邮件详细数据
        private Mail_DetailInfo _m_mailDetialInfo;

        //获取列表信息数据的回调处理
        private Action<Mail_TitleInfo> _m_dTitleGetDelegate;
        //获取详细数据的回调处理
        private Action<Mail_DetailInfo> _m_dDetailGetDelegate;

        /// <summary>
        /// 邮件的配表数据，可能为空
        /// </summary>
        private GMailRefObj _m_mailRef;
        /// <summary>
        /// 邮件发送者的配置，可能为空
        /// </summary>
        private GMailSenderRefObj _m_mailSenderRef;

        /// <summary>
        /// 邮件发送者的配置，可能为空
        /// </summary>
        private GMailTypeRefObj _m_mailTypeRef;


        public GMailDataInfo(Mail_BriefInfo _info)
        {
            _m_bDetailInited = false;
            _m_bSendDetailRequest = false;

            _m_lId = _info.getMailUid();
            _m_briefInfo = _info;
            _m_mailRef = GRefdataCoreMgr.instance.mailRefCore.getRef(_m_briefInfo.getMailRefId() == 0 ? GRefdataCoreMgr.instance.npGeneral.default_mail_refid : _m_briefInfo.getMailRefId());
            _m_mailDetialInfo = null;
            _m_dDetailGetDelegate = null;
            _m_bTitleInited = false;
            _m_titleInfo = null;
            _m_dTitleGetDelegate = null;

        }

        public GMailDataInfo(long _id)
        {
            _m_bDetailInited = false;
            _m_bSendDetailRequest = false;

            _m_lId = _id;
            _m_briefInfo = null;
            _m_mailDetialInfo = null;
            _m_dDetailGetDelegate = null;
            _m_bTitleInited = false;
            _m_titleInfo = null;
            _m_dTitleGetDelegate = null;

        }

        public long id { get { return _m_lId; } }

        /// <summary>
        /// 详细信息是否初始化完成
        /// </summary>
        public bool detailInited { get { return _m_bDetailInited; } }

        /// <summary>
        /// 邮件详细信息
        /// </summary>
        public Mail_DetailInfo mailDetailInfo { get { return _m_mailDetialInfo; } }

        /// <summary>
        /// 邮件配置，可能为空
        /// </summary>
        public GMailRefObj mailRef { get { return _m_mailRef; } }

        /// <summary>
        /// 邮件类型配置，可能为空
        /// </summary>
        public GMailTypeRefObj mailTypeRef { get { return _m_mailTypeRef; } }

        /// <summary>
        /// 邮件列表信息
        /// </summary>
        /// <param name="_info"></param>
        public void setMailTitleInfo(Mail_TitleInfo _info)
        {
            if (null == _info)
            {
                return;
            }
            _m_bTitleInited = true;
            _m_titleInfo = _info;


            _m_mailRef = GRefdataCoreMgr.instance.mailRefCore.getRef(_m_titleInfo.getMailRefId() == 0 ? GRefdataCoreMgr.instance.npGeneral.default_mail_refid : _m_titleInfo.getMailRefId());
            _m_mailSenderRef = GRefdataCoreMgr.instance.mailSenderRefCore.getRef(_m_titleInfo.getSenderId());

            if (null != _m_mailRef)
            {
                _m_mailTypeRef = GRefdataCoreMgr.instance.mailTypeRefCore.getRef(_m_mailRef.type_id);
            }

            if (null != _m_mailRef && null == _m_mailSenderRef)
            {//服务端给的senderId找不到的时候找配置的senderId
                _m_mailSenderRef = GRefdataCoreMgr.instance.mailSenderRefCore.getRef(_m_mailRef.sender_id);
            }
            if (null != _m_dTitleGetDelegate)
                _m_dTitleGetDelegate(_m_titleInfo);
            _m_dTitleGetDelegate = null;
        }

        /// <summary>
        /// 请求获取邮件列表信息进行处理
        /// </summary>
        /// <param name="_delegate"></param>
        public void reqTitleInfo(Action<Mail_TitleInfo> _delegate)
        {
            //已有数据则直接处理
            if (_m_bTitleInited)
            {
                if (null != _delegate)
                    _delegate(_m_titleInfo);
                return;
            }

            //注册回调
            if (null == _m_dTitleGetDelegate)
                _m_dTitleGetDelegate = _delegate;
            else
                _m_dTitleGetDelegate += _delegate;

            //判断是否发送请求，无则发送 
            if (!_m_bSendTitleRequest)
            {
                //发送请求
                NPPlayer.instance.mailComp.reqMailTitle(id);
                _m_bSendTitleRequest = true;
            }
        }

        /// <summary>
        /// 设置邮件详细信息
        /// </summary>
        /// <param name="_info"></param>
        public void setDetail(Mail_DetailInfo _info)
        {
            if (!_m_bDetailInited)
                _m_bDetailInited = true;

            _m_mailDetialInfo = _info;

            //调用回调
            if (null != _m_dDetailGetDelegate)
                _m_dDetailGetDelegate(_m_mailDetialInfo);
            _m_dDetailGetDelegate = null;
        }


        /// <summary>
        /// 请求获取邮件详情进行处理
        /// </summary>
        /// <param name="_delegate"></param>
        public void reqDetailInfo(Action<Mail_DetailInfo> _delegate)
        {
            //已有数据则直接处理
            if (_m_bDetailInited)
            {
                if (null != _delegate)
                    _delegate(_m_mailDetialInfo);
                return;
            }

            if (null != _delegate)
            {
                //注册回调
                if (null == _m_dDetailGetDelegate)
                    _m_dDetailGetDelegate = _delegate;
                else
                    _m_dDetailGetDelegate += _delegate;
            }

            //判断是否发送请求，无则发送 
            if (!_m_bSendDetailRequest)
            {
                //发送请求
                NPPlayer.instance.mailComp.reqMailDetail(id);
                _m_bSendDetailRequest = true;
            }
        }

        /// <summary>
        /// 展示本邮件的详情窗口
        /// </summary>
        public void showDetailWnd()
        {
            if (null == _m_mailRef)
                return;
                reqDetailInfo((Mail_DetailInfo _detailInfo) =>
                {
                    switch (_m_mailTypeRef.prefab_type)
                    {
                
                        case EMailDetailPrefabType.ITEM_EXPIRED:
                            GGUIWndMailDetail_ItemExpired itemExpiredWnd = new GGUIWndMailDetail_ItemExpired(this);
                            QueueMgr.instance.addNode_InGame_SingleWnd(itemExpiredWnd, itemExpiredWnd.showDetailData, UINodeTagConst.C_Mail_Detail);
                            break;
                
                        case EMailDetailPrefabType.HERO_GET:
                            GGUIWndMailDetail_HeroGet heroGetWnd = new GGUIWndMailDetail_HeroGet(this);
                            QueueMgr.instance.addNode_InGame_SingleWnd(heroGetWnd, heroGetWnd.showDetailData, UINodeTagConst.C_Mail_Detail);
                            break;
                
                        case EMailDetailPrefabType.HERO_UPGRADE_STEP:
                            GGUIWndMailDetail_HeroUpgradeStep heroUpgradeStepWnd = new GGUIWndMailDetail_HeroUpgradeStep(this);
                            QueueMgr.instance.addNode_InGame_SingleWnd(heroUpgradeStepWnd, heroUpgradeStepWnd.showDetailData, UINodeTagConst.C_Mail_Detail);
                            break;
                
                        default:

                            GGUIWndMailDetail_Normal detailNormalWnd = new GGUIWndMailDetail_Normal(this);
                            QueueMgr.instance.addNode_InGame_SingleWnd(detailNormalWnd, detailNormalWnd.showDetailData, UINodeTagConst.C_Mail_Detail);
                            break;
                
                    }
                });
        }


        /// <summary>
        /// 判断是否过期
        /// </summary>
        /// <returns></returns>
        public bool isExceed()
        {
            if (null == _m_titleInfo)
            {
                return false;
            }
            //收藏邮件不会过期
            if (getIsLocked())
                return false;
            return getRemainTimeMs() == 0;
        }


        /// <summary>
        /// 获取邮件标题
        /// </summary>
        /// <returns></returns>
        public string getTitle()
        {
            if (null == _m_titleInfo)
            {
                return "";
            }
            if (!string.IsNullOrEmpty(_m_titleInfo.getTitle()))
            {
                return string.Format(_m_titleInfo.getTitle(), _m_titleInfo.getContentReplace().ToArray());
            }
            if (null == _m_mailRef)
            {
                return "";
            }
            return TextTranslate.instance.getLanguage(_m_mailRef.title);
        }

        /// <summary>
        /// 获取邮件副标题
        /// </summary>
        /// <returns></returns>
        public string getSubTitle()
        {
            // 定死规则, 副标题只读取配表配置字段, 且 参数使用_m_titleInfo.getContentReplace() 【GOB-0】邮件-副标题参数补充单据 https://www.teambition.com/task/691e93298cb392e373019778
            // if (null == _m_titleInfo)
            // {
            //     return "";
            // }
            // if (!string.IsNullOrEmpty(_m_titleInfo.getSubTitle()))
            // {
            //     return TextTranslate.instance.getLanguage(_m_titleInfo.getSubTitle(), _m_titleInfo.getContentReplace());
            // }
            if (null == _m_mailRef)
            {
                return "";
            }
            return TextTranslate.instance.getLanguage(_m_mailRef.sub_title, _m_titleInfo?.getContentReplace());
        }
        
        /// <summary>
        /// 获取邮件内容
        /// </summary>
        /// <returns></returns>
        public string getContent()
        {
            if (null == _m_mailDetialInfo)
            {
                return "";
            }
            if (!string.IsNullOrEmpty(_m_mailDetialInfo.getContent()))
            {
                return TextTranslate.instance.getLanguage(_m_mailDetialInfo.getContent(), _m_mailDetialInfo.getContentReplace().ToArray());
            }
            //服务端如果没给内容，就取配表数据
            if (null == _m_mailRef)
            {
                return "";
            }
            return TextTranslate.instance.getLanguage(_m_mailRef.content, _m_mailDetialInfo.getContentReplace().ToArray());
        }

        /// <summary>
        /// 获取发送者名称
        /// </summary>
        /// <returns></returns>
        public string getSender()
        {
            if (null == _m_mailSenderRef)
            {
                return TextTranslate.instance.getLanguage(TransKeyConst.mail_sender_sys_none);
            }
            return TextTranslate.instance.getLanguage(_m_mailSenderRef.name);
        }

        /// <summary>
        /// 发送已读完请求
        /// </summary>
        public void reqSetReadOver()
        {
            if (getIsReadOver())
            {
                return;
            }
            setIsReadOver();
            NPPlayer.instance.mailComp.reqSetReadOver(id);
        }

        /// <summary>
        /// 获取是否必读
        /// </summary>
        /// <returns></returns>
        public bool getIsNeedRead()
        {
            if (null != _m_titleInfo)
            {
                return _m_titleInfo.getIsMustRead();
            }
            if(_m_briefInfo != null)
            {
                return _m_briefInfo.getIsMustRead();
            }
            return false;
        }

        /// <summary>
        /// 是否有奖励
        /// </summary>
        /// <returns></returns>
        public bool getHasItem()
        {
            //有title以title为准
            if (null != _m_titleInfo)
            {
                return _m_titleInfo.getHasItem();
            }
            if (null != _m_briefInfo)
            {
                NPCommon_ItemInfo rewardItem = _m_briefInfo.getItem();
                return rewardItem != null && rewardItem.getItemType() != (int)ENPItemType.NONE && rewardItem.getSubId() != 0;
            }
            return false;
        }

        /// <summary>
        /// 获取邮件奖励
        /// </summary>
        /// <returns></returns>
        public NPCommon_ItemInfo getMailReward()
        {
            if (null != _m_briefInfo)
            {
                return _m_briefInfo.getItem();
            }
            
            return null;
        }

        /// <summary>
        /// 列表item的额外数据
        /// </summary>
        /// <returns></returns>
        public byte[] getExTitleData()
        {
            //有title以title为准
            if (null != _m_titleInfo)
            {
                return _m_titleInfo.getExTitleData();
            }
            return null;
        }

        /// <summary>
        /// 是否已读
        /// </summary>
        /// <returns></returns>
        public bool getIsRead()
        {
            //有title以title为准
            if (null != _m_titleInfo)
            {
                //已读需求 = 已读，并且如果是必读邮件要已读完才算已读
                return _m_titleInfo.getIsRead() && (!_m_titleInfo.getIsMustRead() || (_m_titleInfo.getIsMustRead() && _m_titleInfo.getIsReadOver()));
            }
            if (null != _m_briefInfo)
            {
                return _m_briefInfo.getIsRead();
            }
            return false;
        }

        /// <summary>
        /// 设置成已读
        /// </summary>
        /// <param name="_isRead"></param>
        public void setIsRead()
        {
            bool isChg = false;
            if (null != _m_briefInfo && !_m_briefInfo.getIsRead())
            {
                _m_briefInfo.setIsRead(true);
                isChg = true;
            }
            if (null != _m_titleInfo && !_m_titleInfo.getIsRead())
            {
                _m_titleInfo.setIsRead(true);
                isChg = true;
            }
            if (isChg)
            {
                WinMsg.SendMsg(WinMsgType.ON_CHG_MAIL, id);
            }
        }

        /// <summary>
        /// 是否收藏
        /// </summary>
        /// <returns></returns>
        public bool getIsLocked()
        {
            if (null != _m_titleInfo)
            {
                return _m_titleInfo.getIsLocked();
            }
            if(_m_briefInfo != null)
            {
                return _m_briefInfo.getIsLocked();
            }
            return false;
        }

        /// <summary>
        /// 设置邮件收藏状态
        /// </summary>
        /// <param name="_isLocked"></param>
        public void setLockStat(bool _isLocked)
        {
            if (null != _m_titleInfo && _m_titleInfo.getIsLocked() != _isLocked)
            {
                _m_titleInfo.setIsLocked(_isLocked);
                WinMsg.SendMsg(WinMsgType.ON_CHG_MAIL, id);
            }
        }

        /// <summary>
        /// 是否已领取
        /// </summary>
        /// <returns></returns>
        public bool getHasTaken()
        {
            //有title以title为准
            if (null != _m_titleInfo)
            {
                return _m_titleInfo.getHasTaken();
            }
            if (null != _m_briefInfo)
            {
                return _m_briefInfo.getHasTaken();
            }
            return false;
        }

        /// <summary>
        /// 设置已领取
        /// </summary>
        public void setHasTaken()
        {
            bool isChg = false;
            if (null != _m_briefInfo && !_m_briefInfo.getHasTaken())
            {
                _m_briefInfo.setHasTaken(true);
                isChg = true;
            }
            if (null != _m_titleInfo && !_m_titleInfo.getHasTaken())
            {
                _m_titleInfo.setHasTaken(true);
                isChg = true;
            }
            if (isChg)
            {
                WinMsg.SendMsg(WinMsgType.ON_CHG_MAIL, id);
            }

        }

        /// <summary>
        /// 是否已经读完
        /// </summary>
        /// <returns></returns>
        public bool getIsReadOver()
        {
            if (null != _m_titleInfo)
            {
                return _m_titleInfo.getIsReadOver();
            }
            return false;
        }

        /// <summary>
        /// 设置已读完
        /// </summary>
        public void setIsReadOver()
        {
            if (null != _m_titleInfo && !_m_titleInfo.getIsReadOver())
            {
                _m_titleInfo.setIsReadOver(true);
                WinMsg.SendMsg(WinMsgType.ON_CHG_MAIL, id);
            }
        }
        
        /// <summary>
        /// 截止时间变化
        /// </summary>
        /// <param name="_expiredTimeSec"></param>
        public void setMailExpiredSec(int _expiredTimeSec)
        {
            if (null != _m_titleInfo)
            {
                _m_titleInfo.setEndTimeSec(_expiredTimeSec);
                WinMsg.SendMsg(WinMsgType.ON_CHG_MAIL, id);
            }
        }

        /// <summary>
        /// 邮件获得时间戳（秒）
        /// </summary>
        /// <returns></returns>
        public int getGainTimeSec()
        {
            //有title以title为准
            if (null != _m_titleInfo)
            {
                return _m_titleInfo.getGainTimeSec();
            }
            if (null != _m_briefInfo)
            {
                return _m_briefInfo.getGainTimeSec();
            }
            return 0;
        }

        /// <summary>
        /// 邮件剩余时间（毫秒）
        /// </summary>
        /// <returns></returns>
        public float getRemainTimeMs()
        {
            if (null != _m_titleInfo)
            {
                if (_m_titleInfo.getEndTimeSec() < 0)
                    return _m_titleInfo.getEndTimeSec();
                long remainTimeMs = (long)_m_titleInfo.getEndTimeSec() * 1000 - FpsAndPingMgr.instance.serverTimeTag;
                return Mathf.Max(0, remainTimeMs);
            }
            return 0;
        }

        /// <summary>
        /// 额外信息变化
        /// </summary>
        /// <param name="_exData"></param>
        public void setExData(byte[] _exData)
        {
            if (null == _m_mailDetialInfo)
            {
                return;
            }
            _m_mailDetialInfo.setExData(_exData);
            WinMsg.SendMsg(WinMsgType.ON_CHG_MAIL, id);
        }

        /// <summary>
        /// 邮件状态
        /// </summary>
        /// <returns></returns>
        public EMailReadStat getMailStat()
        {
            if (!getIsRead())//未读
            {
                return EMailReadStat.NO_READ;
            }
            // 没物品奖励的：已读已领取
            return EMailReadStat.HAS_READ;
        }

        /// <summary>
        /// 领取状态
        /// </summary>
        /// <returns></returns>
        public ENPCommonGetStat getMailGetStat()
        {
            if (getHasItem())
            {
                if (getHasTaken())
                    return ENPCommonGetStat.HAS_GET;
                return ENPCommonGetStat.CAN_GET;
            }

            return ENPCommonGetStat.CAN_NOT_GET;
        }

        /// <summary>
        /// 对列表进行排序 未读未领取 > 已读未领取 > 已读已领取,按获得时间,新获得的排前
        /// </summary>
        /// <param name="_mailList"></param>
        public static void sortMailList(List<GMailDataInfo> _mailList)
        {
            if (null == _mailList)
            {
                return;
            }

            _mailList.Sort((x, y) =>
            {
                if (x.getIsRead() && !y.getIsRead())
                    return 1;
                if (!x.getIsRead() && y.getIsRead())
                    return -1;

                if (x.getHasItem() && !x.getHasTaken() && y.getHasItem() && y.getHasTaken())
                    return -1;
                if (x.getHasItem() && x.getHasTaken() && y.getHasItem() && !y.getHasTaken())
                    return 1;
                if (x.getHasItem() && !x.getHasTaken() && !y.getHasItem())
                    return -1;
                if (!x.getHasItem() && y.getHasItem() && !y.getHasTaken())
                    return 1;
                // if (x.getHasTaken() && !y.getHasTaken())
                //     return 1;
                // if (!x.getHasTaken() && y.getHasTaken())
                //     return -1;
                if (x.getGainTimeSec() > y.getGainTimeSec())
                    return -1;
                if (x.getGainTimeSec() < y.getGainTimeSec())
                    return 1;
                return 0;
            });
        }
    }
}
