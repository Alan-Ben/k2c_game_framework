using UnityEngine;
using System.Collections.Generic;
using ALPackage;
using Common.MailObj;

namespace GOE
{
    public class GGUIWndMailDetail_Base : GGUIWndMailDetail_Base<GGUIMonoMailDetail_Base>
    {
        public GGUIWndMailDetail_Base(GMailDataInfo _info) : base(_info)
        {
        }
    }
    
    public class GGUIWndMailDetail_Base<T_MONO> : _ANPGGUIBasicWnd<T_MONO>
    where T_MONO :GGUIMonoMailDetail_Base
    {
        protected GMailDataInfo _m_miMailDataInfo; //邮件信息
        protected int _m_serialize = 0; //判断是否已读完用的序列号
        protected bool _m_isReadEnd; //是否已读完
        protected bool _m_isNeedAni; //需要动画

        public GGUIWndMailDetail_Base(GMailDataInfo _info)
            : base(EALUIWndLayer.ADDITION)
        {
            _m_miMailDataInfo = _info;
            _m_isNeedAni = true;
            //构造的时候就开始请求
            _m_miMailDataInfo.reqDetailInfo(null);
        }

        /// <summary>
        /// 在窗口作为Scene中的主展示窗口的时候，在切换时是否会需要释放
        /// 一般不释放，如果子类有需要可以重载函数处理
        /// </summary>
        public override bool needDiscardOnSwitch
        {
            get { return true; }
        }

        protected override bool isShowAniPlayOnlyOne { get { return true; } }

        /********************
        * 获取资源所在资源加载文件名称
        **/
        protected override string _monoAssetPath
        {
            get
            {
                if (null == _m_miMailDataInfo || null == _m_miMailDataInfo.mailTypeRef)
                    return string.Empty;

                return UIResPathAssistant.getAssetPath(_m_miMailDataInfo.mailTypeRef.content_ui_path_id);
            }
        }

        protected override string _monoObjName
        {
            get
            {
                if (null == _m_miMailDataInfo || null == _m_miMailDataInfo.mailTypeRef)
                    return string.Empty;

                return UIResPathAssistant.getObjName(_m_miMailDataInfo.mailTypeRef.content_ui_path_id);
            }
        }

        /**************
         * 获取用于加载资源的管理对象
         **/
        protected override _AALResourceCore _resourceCore
        {
            get { return GameResCore.instance; }
        }

        /******************
        * 显示窗口的事件函数
        **/
        protected override void _onShowWnd()
        {
            _m_isNeedAni = false;
            //监听事件
            WinMsg.RegisterMsg(WinMsgType.ON_SET_MAIL_DETAIL, _onSetMailDetail);
            WinMsg.RegisterMsg(WinMsgType.ON_DEL_MAIL, _delMailSuc);
            WinMsg.RegisterMsg(WinMsgType.ON_CHG_MAIL, _onChgMail);
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_GAIN_SHOW_MAIL_REWARD, _onSimulateGainShowMailReward);
            _onShowWndEx();
        }

        protected virtual void _onShowWndEx()
        {
            
        }

        /******************
         * 隐藏窗口的事件函数
         **/
        protected override void _onHideWnd()
        {
            _m_serialize = ALSerializeOpMgr.next();
            //解除监听事件
            WinMsg.UnregisterMsg(WinMsgType.ON_SET_MAIL_DETAIL, _onSetMailDetail);
            WinMsg.UnregisterMsg(WinMsgType.ON_DEL_MAIL, _delMailSuc);
            WinMsg.UnregisterMsg(WinMsgType.ON_CHG_MAIL, _onChgMail);
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_GAIN_SHOW_MAIL_REWARD, _onSimulateGainShowMailReward);
            _onHideWndEx();
        }

        protected virtual void _onHideWndEx()
        {
            
        }

        /******************
         * 重置窗口数据的事件函数
         **/
        protected override void _onReset()
        {
            _onResetEx();
        }

        protected virtual void _onResetEx()
        {
            
        }

        /******************
         * 释放资源时触发的事件
         **/
        protected override void _onDiscard()
        {
            _onDiscardEx();
            _m_isNeedAni = true;
            if (null != wnd.transScrollView)
            {
                wnd.transScrollView.onValueChanged.RemoveListener(_onScrollValueChanged);
            }
        }

        protected virtual void _onDiscardEx()
        {
            
        }

        /*************
         * 窗口初始化完成调用的函数
         **/
        protected override void _onWndInitDone()
        {
            if (null == wnd)
            {
                return;
            }
            //监听UI
            ALUGUICommon.combineBtnClick(wnd.btnGain, _onClickGain);
            ALUGUICommon.combineBtnClick(wnd.btnDel, _onClickDel);
            ALUGUICommon.combineBtnClick(wnd.btnLock, _onClickLock);
            ALUGUICommon.combineBtnClick(wnd.btnUnLock, _onClickUnLock);
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
            if (null != wnd.transScrollView)
            {
                wnd.transScrollView.onValueChanged.AddListener(_onScrollValueChanged);
            }
            _onWndInitDoneEx();
        }

        protected virtual void _onWndInitDoneEx()
        {
            
        }

        /// <summary>
        /// 刷新显示数据
        /// </summary>
        /// <param name="_info"></param>
        public void showDetailData()
        {
            if (null == _m_miMailDataInfo)
                return;

            _m_serialize = ALSerializeOpMgr.next();
            _onRetMailDataInfo(_m_miMailDataInfo);
            _refreshWnd();

            //检测是否到底
            ALCommonActionMonoTask.addNextFrameTask(_startCheckIsReadEnd);
            if (_m_isNeedAni)
            {
                //显示窗口
                showWnd();
            }
            else
            {
                showWndWithoutAni();
            }
        }

        protected virtual void _onRetMailDataInfo(GMailDataInfo _mailDataInfo)
        {
            
        }


        /// <summary>
        /// 刷新窗口
        /// </summary>
        private void _refreshWnd()
        {
            if (null == _m_miMailDataInfo || !_m_miMailDataInfo.detailInited || wnd == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtTitle, _m_miMailDataInfo.getTitle());
            ALUGUICommon.setLabelTxt(wnd.txtSubTitle, _m_miMailDataInfo.getSubTitle());

            ALUGUICommon.setLabelTxt(wnd.txtSender, _getSender());
            if (null != wnd.txtContent)
            {
                wnd.txtContent.setUrlColor(wnd.urlColor);
                wnd.txtContent.setIsShowUnderLine(wnd.isShowUnderLine);
            }

            ALUGUICommon.setLabelTxt(wnd.txtContent, _getMailContent());
            ALUGUICommon.setLabelTxt(wnd.txtCreateTime,
                TimeUtil.DateTime2StringMDYHMS(TimeUtil.FromUTCSeconds(_m_miMailDataInfo.getGainTimeSec())));
            // ALUGUICommon.setLabelTxt(wnd.txtRemainTime,
            //     TimeUtil.millisecondsToTime_Two(_m_miMailDataInfo.getRemainTimeMs()));
            
            ALUGUICommon.setLabelTxt(wnd.txtRemainTime, _m_miMailDataInfo.getRemainTimeMs() < 0 ? 
                TextTranslate.instance.getLanguage(TransKeyConst.time_forever) 
                : TimeUtil.millisecondsToTime_Two(_m_miMailDataInfo.getRemainTimeMs()));
            bool hasItem = _m_miMailDataInfo.getHasItem();
            bool hasTaken = _m_miMailDataInfo.getHasTaken();
            bool isLocked = _m_miMailDataInfo.getIsLocked();

            ALUGUICommon.setGameObjEnable(wnd.btnGain, !hasTaken && hasItem); //奖励未领取
            ALUGUICommon.setGameObjEnable(wnd.btnDel, hasTaken || !hasItem); //已领取 或者非奖励邮件
            ALUGUICommon.setGameObjEnable(wnd.btnLock, !isLocked); //未收藏
            ALUGUICommon.setGameObjEnable(wnd.btnUnLock, isLocked); //已收藏
            ALUGUICommon.setGameObjEnable(wnd.goLockStat, isLocked); //已收藏
            ALUGUICommon.setGameObjEnable(wnd.goNeedRead, _m_miMailDataInfo.getIsNeedRead()); //是否必读

            if (_m_miMailDataInfo.getIsNeedRead() && !_m_isReadEnd) //必读 && 未读完
            {
                GGameCommonInfo.grayImage(wnd.grayObjectList);
            }
            else
            {
                GGameCommonInfo.disgrayImage(wnd.grayObjectList);
            }
            _refreshWndEx();
        }

        protected virtual string _getSender()
        {
            return _m_miMailDataInfo.getSender();
        }

        /// <summary>
        /// 邮件内容
        /// </summary>
        /// <returns></returns>
        protected virtual string _getMailContent()
        {
            return _m_miMailDataInfo.getContent();
        }

        protected virtual void _refreshWndEx()
        {
            
        }

        /// <summary>
        /// 开始检查是否已经读完
        /// </summary>
        private void _startCheckIsReadEnd()
        {
            if (null == _m_miMailDataInfo)
                return;

            _m_isReadEnd = _m_miMailDataInfo.getIsReadOver();
            _checkIsReadEnd(_m_serialize, 0.2f);
            _onScrollValueChanged(Vector2.zero);
        }
        
        /// <summary>
        /// 拖动列表变化
        /// </summary>
        /// <param name="arg0"></param>
        private void _onScrollValueChanged(Vector2 arg0)
        {
            if (null == wnd)
                return;
            bool isScrollEnd = true;
            if (null != wnd.transScrollView)
            {
                //已经到底了
                bool isOverView = false;
                //判断有没有超出显示范围
                if(null !=  wnd.transScrollView.content && null !=  wnd.transScrollView.viewport)
                    isOverView = wnd.transScrollView.content.rect.height > wnd.transScrollView.viewport.rect.height;
                isScrollEnd = !isOverView || (wnd.transScrollView.verticalNormalizedPosition <= 0.01f); //0.01的误差容错
                // ALLog.Error($"----------isScrollEnd:{isScrollEnd},isOverView:{isOverView},verticalNormalizedPosition:{wnd.transScrollView.verticalNormalizedPosition}");
            }
            ALUGUICommon.setGameObjEnable(wnd.goScrollEndShow, isScrollEnd);
            ALUGUICommon.setGameObjEnable(wnd.goScrollEndHide, !isScrollEnd);
        }

        /// <summary>
        /// 检查是否已经读完
        /// </summary>
        /// <param name="_serialize"></param>
        /// <param name="_dalayTime"></param>
        private void _checkIsReadEnd(int _serialize, float _dalayTime)
        {
            if (_m_isReadEnd) //已经是读完状态
                return;

            int serialize = _serialize;
            if (serialize != _m_serialize) //序列号不对，跳过
                return;

            //已经到底了
            if (wnd.transScrollView == null || wnd.transScrollView.verticalNormalizedPosition <= 0.01f) //0.01的误差容错
            {
                _m_isReadEnd = true;
            }
            else
            {
                _m_isReadEnd = false;
            }

            if (!_m_isReadEnd)
            {
                //还没读完，开启下一次检查
                ALCommonActionMonoTask.addScaleTimeDelayMonoTask(() => { _checkIsReadEnd(serialize, _dalayTime); },
                    _dalayTime);
            }
            else //读完，发消息
            {
                _m_miMailDataInfo.reqSetReadOver();
            }
        }

        /// <summary>
        /// 检测邮件是否过期删除了
        /// </summary>
        /// <returns></returns>
        private bool _checkMailIsDeled()
        {
            if (null != _m_miMailDataInfo && _m_miMailDataInfo.isExceed())
            {
                //已经过期自动删除的时候，给提示，然后删除
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.mail_exceedDel_none));
                WinMsg.SendMsg(WinMsgType.ON_DEL_MAIL, _m_miMailDataInfo.id);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 点击领取
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickGain(GameObject _go)
        {
            if (null == _m_miMailDataInfo || !_m_miMailDataInfo.detailInited)
                return;
            if (_checkMailIsDeled())
            {
                return;
            }

            if (_m_miMailDataInfo.getHasTaken())
                return;

            NPPlayer.instance.mailComp.reqTakeMailItems(_m_miMailDataInfo.id);
        }

        /// <summary>
        /// 点击删除
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickDel(GameObject _go)
        {
            if (wnd == null || null == _m_miMailDataInfo)
                return;
            
            bool isNeedReadEnd = _m_miMailDataInfo.getIsNeedRead();
            if (isNeedReadEnd && !_m_isReadEnd) //必读 && 未读完
            {
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.mail_hasToReadEnd_none));
                return;
            }
            
            if (_m_miMailDataInfo.getIsLocked())
            {
                //# 该信件为收藏邮件，若需要删除邮件请先取消收藏#
                NPGUIAddSceneCenterTip.instance.showTextInfo(
                    TextTranslate.instance.getLanguage(TransKeyConst.mail_lockTip_none));
                return;
            }

            NPMesMgr.instance.showTwoBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.mail_delMail_none), TextTranslate.instance.getLanguage(TransKeyConst.cancel),
                () => { }, TextTranslate.instance.getLanguage(TransKeyConst.confirm),
                () =>
                {
                    if (wnd == null || _m_miMailDataInfo == null)
                        return;

                    PlayAudioMgr.instance.playClip(wnd.delAudioId);
                    NPPlayer.instance.mailComp.reqDelMail(_m_miMailDataInfo.id);
                });
        }


        /// <summary>
        /// 点击关闭按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.DoUIRollBackByEscByTag(UINodeTagConst.C_Mail_Detail);
        }

        /// <summary>
        /// 点击收藏
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickLock(GameObject _go)
        {
            if (null == _m_miMailDataInfo)
                return;
            if (_checkMailIsDeled())
            {
                return;
            }

            //是否客户端拦截最大值判断

            List<GMailDataInfo> _lockList = new List<GMailDataInfo>();
            NPPlayer.instance.mailComp.getLockedMailList(_lockList);
            if (_lockList.Count >= GRefdataCoreMgr.instance.npGeneral.mail_max_lock_num)
            {
                //已达最大收藏数量
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.mail_lockMax_none));
            }
            else
            {
                NPPlayer.instance.mailComp.reqSetMailLockState(_m_miMailDataInfo.id, true);
            }

            _lockList.Clear();
            _lockList = null;
        }

        /// <summary>
        /// 点击取消收藏
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickUnLock(GameObject _go)
        {
            if (null == _m_miMailDataInfo)
                return;
            if (_checkMailIsDeled())
            {
                return;
            }

            NPPlayer.instance.mailComp.reqSetMailLockState(_m_miMailDataInfo.id, false);
        }

        /// <summary>
        /// 设置邮件详情信息
        /// </summary>
        /// <param name="_objs"></param>
        private void _onSetMailDetail(params object[] _objs)
        {
            if (null == _m_miMailDataInfo)
                return;

            long id = (long) _objs[0];
            if (id != _m_miMailDataInfo.id)
                return;
            _refreshWnd();
        }

        /// <summary>
        /// 邮件信息变化的时候
        /// </summary>
        /// <param name="_objs"></param>
        private void _onChgMail(params object[] _objs)
        {
            if (null == _m_miMailDataInfo)
                return;

            long id = (long) _objs[0];
            if (id != _m_miMailDataInfo.id)
                return;

            _refreshWnd();
        }

        /// <summary>
        /// 邮件删除
        /// </summary>
        /// <param name="_objs"></param>
        private void _delMailSuc(params object[] _objs)
        {
            if (null == _m_miMailDataInfo)
                return;

            long id = (long) _objs[0];
            if (id != _m_miMailDataInfo.id)
                return;

            _m_miMailDataInfo = null;
            QueueMgr.instance.DoUIRollBackByEscByTag(UINodeTagConst.C_Mail_Detail);
        }

        /// <summary>
        /// 模拟领取邮件奖励
        /// </summary>
        private void _onSimulateGainShowMailReward()
        {
            _onClickGain(null);
        }
    }
}