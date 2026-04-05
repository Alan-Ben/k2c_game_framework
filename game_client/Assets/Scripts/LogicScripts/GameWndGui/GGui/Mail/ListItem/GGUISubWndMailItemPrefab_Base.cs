using ALPackage;
using UnityEngine;
using System;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 邮件列表item的基类
    /// </summary>
    /// <typeparam name="T_MONO"></typeparam>
    public abstract class GGUISubWndMailItemPrefab_Base<T_MONO> : _ATALBasicLoadPrefabSubUIWnd<T_MONO>, IGGUISubWndMailItemPrefab
    where T_MONO : GGUISubMonoMailItemPrefab
    {
        protected GMailDataInfo _m_miMailDataInfo; //数据对象
        private long _m_uiPathId;//UI资源路径id

        private GGUIWndCommonRewardContainerItem _m_wRewardItem;//奖励item
        private NPGGuiWndTexture _m_wSenderIcon;//发送者图标

        protected override string _monoAssetPath { get { return UIResPathAssistant.getAssetPath(_m_uiPathId); } }

        protected override string _monoObjName { get { return UIResPathAssistant.getObjName(_m_uiPathId); } }

        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        

        public GGUISubWndMailItemPrefab_Base(long _uiPathId, Transform _parent)
            : base(_parent)
        {
            _m_uiPathId = _uiPathId;
        }

        protected override void _onDiscard()
        {
            _onDiscardEx();
         
            _m_wRewardItem?.discard();
            _m_wRewardItem = null;
            
            _m_wSenderIcon?.discard();
            _m_wSenderIcon = null;
        }

        protected abstract void _onDiscardEx();

        protected override void _onHideWnd()
        {
            _m_wRewardItem?.hideWnd();
            _m_wSenderIcon?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wRewardItem?.resetWnd();
            _m_wSenderIcon?.discardTexture();
        }


        protected override void _onShowWnd()
        {
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;
            
            if(wnd.rewardItemMono != null)
                _m_wRewardItem = new GGUIWndCommonRewardContainerItem(wnd.rewardItemMono);
            
            if(wnd.senderIcon != null)
                _m_wSenderIcon = new NPGGuiWndTexture(wnd.senderIcon);
            
            //监听UI
            ALUGUICommon.combineBtnClick(wnd.btnRead, _onClickReadMail);
        }

        /// <summary>
        /// 设置邮件数据，刷新显示
        /// </summary>
        /// <param name="_dataInfo"></param>
        public void setMailItem(GMailDataInfo _dataInfo)
        {
            if (null == _dataInfo || null == wnd)
                return;

            _m_miMailDataInfo = _dataInfo;
            _onSetDataInfo(_m_miMailDataInfo);
            bool hasRead = _m_miMailDataInfo.getIsRead();
            bool hasGain = _m_miMailDataInfo.getHasTaken();
            bool isRewardMail = _m_miMailDataInfo.getHasItem(); // 是否奖励邮件

            //设置标题
            ALUGUICommon.setLabelTxt(wnd.txtTitle,_getTitle());
            ALUGUICommon.setLabelTxt(wnd.txtSubTitle, _getSubTitle());
            //设置发送者
            ALUGUICommon.setLabelTxt(wnd.txtSender, _getSender());
            if (_m_wSenderIcon != null && _m_miMailDataInfo != null && _m_miMailDataInfo.mailRef != null)
            {
                _m_wSenderIcon.showWnd();
                _m_wSenderIcon.setTexture(_m_miMailDataInfo.mailRef.sender_icon);
            }
            //设置时间
            ALUGUICommon.setLabelTxt(wnd.txtValidityTime, _m_miMailDataInfo.getRemainTimeMs() < 0 ? 
                TextTranslate.instance.getLanguage(TransKeyConst.time_forever) 
                : TimeUtil.millisecondsToTime_Two(_m_miMailDataInfo.getRemainTimeMs()));

            //奖励标志图标
            ALUGUICommon.setGameObjEnable(wnd.goRewardTip, isRewardMail && !hasGain);//奖励标注图标显示

            ALUGUICommon.setGameObjEnable(wnd.goRedPoint, !hasRead && !hasGain);//展示红点
            ALUGUICommon.setGameObjEnable(wnd.goLockShow, _m_miMailDataInfo.getIsLocked());
            ALUGUICommon.setGameObjEnable(wnd.goLockHide, !_m_miMailDataInfo.getIsLocked());
            
            ALUGUICommon.setGameObjEnable(wnd.goNeedRead,_m_miMailDataInfo.getIsNeedRead());

            EMailReadStat readStat = _m_miMailDataInfo.getMailStat();
            ENPCommonGetStat getStat = _m_miMailDataInfo.getMailGetStat();
            
            NPCommonEnumStatInfo<EMailReadStat>.setStat(wnd.mailStatInfos, readStat);
            NPCommonEnumStatInfo<ENPCommonGetStat>.setStat(wnd.mailGetStatInfos, getStat);

            if (_m_wRewardItem != null)
            {
                NPCommon.NPCommon_ItemInfo rewardItem = _m_miMailDataInfo.getMailReward(); 
                if (isRewardMail && rewardItem != null && rewardItem.getItemType() != (int)ENPItemType.NONE && rewardItem.getSubId() != 0)
                {
                    _m_wRewardItem.showWnd();
                    ECommonRewardType rewardType = ECommonRewardType.NONE;
                    switch (getStat)
                    {
                        case ENPCommonGetStat.CAN_GET:
                            rewardType = ECommonRewardType.CAN_GET_REWARD;
                            break;
                        
                        case ENPCommonGetStat.HAS_GET:
                            rewardType = ECommonRewardType.HAS_GET_REWARD;
                            break;
                        
                        case ENPCommonGetStat.CAN_NOT_GET:
                            rewardType = ECommonRewardType.NOT_GET_REWARD;
                            break;
                    }
                    _m_wRewardItem.setItem(rewardItem.toCommonItemData(), rewardType);
                }
                else
                {
                    _m_wRewardItem.hideWnd();
                }
            }
            
            //置灰
            if (readStat == EMailReadStat.HAS_READ && getStat != ENPCommonGetStat.CAN_GET)
            {
                GGameCommonInfo.grayImage(wnd.hasReadHasGetGrayList);
            }
            else
            {
                GGameCommonInfo.disgrayImage(wnd.hasReadHasGetGrayList);
            }

            _refreshWndEx();
        }

        protected abstract void _onSetDataInfo(GMailDataInfo _mailDataInfo);

        protected virtual string _getSender()
        {
            return TextTranslate.instance.getLanguage(TransKeyConst.mail_sender_str, _m_miMailDataInfo.getSender());
        }

        protected virtual string _getTitle()
        {
            if(null != _m_miMailDataInfo)
                return _m_miMailDataInfo.getTitle();
            return null;
        }

        protected virtual string _getSubTitle()
        {
            if(null != _m_miMailDataInfo)
                return _m_miMailDataInfo.getSubTitle();
            return null;
        }
        
        protected virtual void _refreshWndEx()
        {
            
        }

        /// <summary>
        /// 点击展示详情
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickReadMail(GameObject _go)
        {
            if (null == _m_miMailDataInfo)
            {
                return;
            }

            if(_m_miMailDataInfo.isExceed())
            {
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.mail_exceedDel_none));
                WinMsg.SendMsg(WinMsgType.ON_DEL_MAIL, _m_miMailDataInfo.id);
                return;
            }


            //打开邮件详情窗口
            // NPGUIWndMailDetail detailWnd = new NPGUIWndMailDetail(_m_miMailDataInfo);
            // NPQueueMgr.instance.addNode_InGame_SingleWnd(detailWnd
            //     , detailWnd.showDetailData
            //     , NPUINodeTagConst.C_Mail_Detail);

            //展示详情窗口
            _m_miMailDataInfo.showDetailWnd();
        }
    }
}
