using UnityEngine;
using System.Collections.Generic;
using ALPackage;
using System;
using System.Text;


namespace GOE
{
    //邮件列表展示窗口
    public class GGUIWndMail : _ATALBasicUIWnd<GGUIMonoMail>
    {
        private static GGUIWndMail _g_instance = new GGUIWndMail();
        public static GGUIWndMail instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndMail();
                return _g_instance;
            }
        }
        //邮件列表
        private GGUIWndMailGrid _m_wMailGrid;
        //收藏筛选按钮
        private NPGGUIWndCommonToggleEx _m_toggleLock;

        //邮件列表
        private List<GMailDataInfo> _m_mailList = new List<GMailDataInfo>();

        protected GGUIWndMail() : base(EALUIWndLayer.ADDITION)
        {

        }

        /********************
        * 获取资源所在资源加载文件名称
        **/
        protected override string _monoAssetPath { get { return GGUIMonoMail.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoMail.objName; } }

        /**************
         * 获取用于加载资源的管理对象
         **/
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        

        /******************
        * 显示窗口的事件函数
        **/
        protected override void _onShowWnd()
        {
            if (null == _m_wMailGrid)
                return;

            //筛选按钮默认状态
            if (null != _m_toggleLock)
            {
                _m_toggleLock.setSelected(false,true);
            }
            //获取数据
            NPPlayer.instance.mailComp.reqMailBreifList(() =>
            {
                _refreshMailList();
                //设置数据对象 
                _m_wMailGrid?.showWnd();
            });
            //邮件数量
            _checkMailCount();
            //邮件数量文本
            _refreshMailCountText();
            // 刷新一键操作显示物体
            _refreshAkeyShow();

            //监听事件
            WinMsg.RegisterMsg(WinMsgType.ON_SET_MAIL_BRIEF, _onSetMailBrief);
            WinMsg.RegisterMsgAct(WinMsgType.ON_ADD_MAIL, _onAddMail);
            WinMsg.RegisterMsgAct(WinMsgType.ON_DEL_MAIL, _onDelMail);
            WinMsg.RegisterMsgAct(WinMsgType.ON_MAIL_STAT_CHG, _onMailStatChg);
            WinMsg.RegisterMsg(WinMsgType.ON_CHG_MAIL, _onChgMail);
        }

        /******************
         * 隐藏窗口的事件函数
         **/
        protected override void _onHideWnd()
        {
            _m_bHasRefreshAkeyShowTask = false;
            
            //解除监听事件
            WinMsg.UnregisterMsg(WinMsgType.ON_SET_MAIL_BRIEF, _onSetMailBrief);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_ADD_MAIL, _onAddMail);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_DEL_MAIL, _onDelMail);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_MAIL_STAT_CHG, _onMailStatChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_CHG_MAIL, _onChgMail);
        }
        /******************
        * 重置窗口数据的事件函数
        **/
        protected override void _onReset()
        {
            _m_bHasRefreshAkeyShowTask = false;
        }

        /******************
        * 释放资源时触发的事件
        **/
        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onCloseBtnClick);
            }
            
            if (null != _m_wMailGrid)
            {
                _m_wMailGrid.discard();
                _m_wMailGrid = null;
            }
            if(null != _m_toggleLock)
            {
                _m_toggleLock.discard();
                _m_toggleLock = null;
            }
         
            _m_bHasRefreshAkeyShowTask = false;
        }

        /*************
        * 窗口初始化完成调用的函数
        * */
        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;

            if (null != wnd.monoMailGrid)
                _m_wMailGrid = new GGUIWndMailGrid(wnd.monoMailGrid);

            //监听UI
            ALUGUICommon.combineBtnClick(wnd.btnAkeyDel, _onClickAkeyDelAll);
            ALUGUICommon.combineBtnClick(wnd.btnAkeyFinish, _onClickAkeyFinishAll);
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onCloseBtnClick);

            if (null != wnd.toggleLock)
            {
                _m_toggleLock = new NPGGUIWndCommonToggleEx(wnd.toggleLock);
                _m_toggleLock.showWnd();
                _m_toggleLock.clickDelegate = _onClickLock;
            }
        }

        /// <summary>
        /// 根据邮件数量 刷新按钮以及没有邮件的提示
        /// </summary>
        private void _checkMailCount()
        {
            EMailMainStat stat = EMailMainStat.ALL_EMPTY;
            
            if (NPPlayer.instance.mailComp.getTotalMailCount() > 0)
            {
                stat = EMailMainStat.ALL_NO_EMPTY;
            }
            else
            {
                stat = EMailMainStat.ALL_EMPTY;
            }
            
            if (NPPlayer.instance.mailComp.getTotalLockMailCount() > 0)
            {
                if (null != _m_toggleLock && _m_toggleLock.isOn)
                {
                    stat = EMailMainStat.LOCK_NO_EMPTY;
                }
            }
            else
            {
                if (null != _m_toggleLock && _m_toggleLock.isOn)
                {
                    stat = EMailMainStat.LOCK_EMPTY;
                }
            }
            
            NPCommonEnumStatInfo<EMailMainStat>.setStat(wnd.statInfos, stat);
            if (null != _m_toggleLock && _m_toggleLock.isOn)
            {
                ALUGUICommon.setGameObjEnable(wnd.isLockOnShow, true);
                ALUGUICommon.setGameObjEnable(wnd.isLockOffShow, false);
            }
            else
            {
                ALUGUICommon.setGameObjEnable(wnd.isLockOnShow, false);
                ALUGUICommon.setGameObjEnable(wnd.isLockOffShow, true);
            }
        }

        /// <summary>
        /// 刷新邮件数量的文本显示
        /// </summary>
        private void _refreshMailCountText()
        {
            bool isShowLock = (null == _m_toggleLock) ? false : _m_toggleLock.isOn;
            if (isShowLock)
            {
                
                string mes = TextTranslate.instance.getLanguage(TextTranslate.instance.getLanguage(TransKeyConst.mail_mail_lock_count_num, 
                    NPPlayer.instance.mailComp.getTotalLockMailCount()
                    , GRefdataCoreMgr.instance.npGeneral.mail_max_lock_num));
                //当前收藏邮件数 / 最大收藏邮件数
                ALUGUICommon.setLabelTxt(wnd.textMailCount, mes);
            }
            else
            {
                
                string mes = TextTranslate.instance.getLanguage(TextTranslate.instance.getLanguage(TransKeyConst.mail_mail_count_num, 
                    NPPlayer.instance.mailComp.getTotalMailCount()
                    , GRefdataCoreMgr.instance.npGeneral.mail_max_num));
                //当前邮件数 / 最大邮件数
                ALUGUICommon.setLabelTxt(wnd.textMailCount, mes);
            }
        }

        /// <summary>
        /// 刷新一键删除和一键领取的显示
        /// </summary>
        private void _refreshAkeyShow()
        {
            if(wnd == null)
                return;
            
            bool canAKeyDealMail = NPPlayer.instance.mailComp.hasCanAKeyDealMail(_m_mailList);
            bool canAKeyDelMail = NPPlayer.instance.mailComp.hasCanAKeyDeleteMail(_m_mailList);
            bool hasUnReadNeedReadMail = NPPlayer.instance.mailComp.hasUnReadNeedReadMail(_m_mailList);
            if (canAKeyDealMail)
            {
                ALUGUICommon.setGameObjEnable(wnd.canAkeyFinishShowGoList, true);
                ALUGUICommon.setGameObjEnable(wnd.canAkeyDelShowGoList, false);
                ALUGUICommon.setGameObjEnable(wnd.hasUnReadNeedReadMailShow, false);
            }
            else if(canAKeyDelMail)
            {
                ALUGUICommon.setGameObjEnable(wnd.canAkeyFinishShowGoList, false);
                ALUGUICommon.setGameObjEnable(wnd.canAkeyDelShowGoList, true);
                ALUGUICommon.setGameObjEnable(wnd.hasUnReadNeedReadMailShow, false);
            }
            else if(hasUnReadNeedReadMail)
            {
                ALUGUICommon.setGameObjEnable(wnd.canAkeyFinishShowGoList, false);
                ALUGUICommon.setGameObjEnable(wnd.canAkeyDelShowGoList, false);
                ALUGUICommon.setGameObjEnable(wnd.hasUnReadNeedReadMailShow, true);
            }
            else
            {
                ALUGUICommon.setGameObjEnable(wnd.canAkeyFinishShowGoList, false);
                ALUGUICommon.setGameObjEnable(wnd.canAkeyDelShowGoList, false);
                ALUGUICommon.setGameObjEnable(wnd.hasUnReadNeedReadMailShow, false);
            }
        }
        
        /// <summary>
        /// 一键删除
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickAkeyDelAll(GameObject _go)
        {
            bool canAKeyDelMail = NPPlayer.instance.mailComp.hasCanAKeyDeleteMail(_m_mailList);
            if (!canAKeyDelMail)
            {
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.mail_not_akey_delTip_none));   
                return;
            }
            string mes = TextTranslate.instance.getLanguage(TransKeyConst.mail_delTip_none);
            string rightTxt = TextTranslate.instance.getLanguage(TransKeyConst.confirm);
            string leftTxt = TextTranslate.instance.getLanguage(TransKeyConst.cancel);
            NPMesMgr.instance.showTwoBtnMes(mes, leftTxt,
                () =>
                {

                }, rightTxt,
                () =>
                {
                    if(wnd !=null)
                        PlayAudioMgr.instance.playClip(wnd.delAudioId);
                    
                    NPPlayer.instance.mailComp.reqAkeyDelAll();
                });

        }

        /// <summary>
        /// 一键领取
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickAkeyFinishAll(GameObject _go)
        {
            bool canAKeyDealMail = NPPlayer.instance.mailComp.hasCanAKeyDealMail(_m_mailList);
            if (!canAKeyDealMail)
            {
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.mail_not_akey_dealTip_none));   
                return;
            }
            NPPlayer.instance.mailComp.reqAKeyTakeAll();
        }

        /// <summary>
        /// 收藏筛选按钮点击
        /// </summary>
        /// <param name="_go"></param>

        private void _onClickLock(NPGGUIWndCommonToggleEx _commonToggle)
        {
            if(null == _commonToggle)
                return;
            
            _commonToggle.setSelected(!_commonToggle.isOn);
            
            _checkMailCount();
            _refreshMailList();
        }

        /// <summary>
        /// 刷新邮件列表显示
        /// </summary>
        private void _refreshMailList()
        {
            if (null == _m_wMailGrid)
                return;

            if (null != _m_mailList)
            {
                _m_mailList.Clear();
            }

            //获取是否收藏的列表
            if (_m_toggleLock != null && _m_toggleLock.isOn)
            {
                NPPlayer.instance.mailComp.getLockedMailList(_m_mailList);
            }
            else
            {
                NPPlayer.instance.mailComp.getMailList(_m_mailList);
            }

            //刷新数据队列
            _m_wMailGrid.refreshGridList(_m_mailList);
            
            //邮件数量文本
            _refreshMailCountText();

            // 刷新一键操作显示物体
            _refreshAkeyShow();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_go"></param>
        private void _onCloseBtnClick(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_Main_MailNode);
        }
        
        #region WIN_MSG

        /// <summary>
        /// 某个邮件简要信息刷新
        /// </summary>
        /// <param name="_objs"></param>
        private void _onSetMailBrief(params object[] _objs)
        {
            if (null == _m_wMailGrid)
                return;

            long id = (long)_objs[0];//邮件数据实例ID
            _m_wMailGrid.refreshItemBy(id);
        }

        /// <summary>
        /// 有新邮件获得
        /// </summary>
        private void _onAddMail()
        {
            _checkMailCount();
            _refreshMailList();
        }

        /// <summary>
        /// 删除 邮件
        /// </summary>
        private void _onDelMail()
        {
            _checkMailCount();
            _refreshMailList();
        }

        /// <summary>
        /// 邮件总状态更新
        /// </summary>
        private void _onMailStatChg()
        {
            _refreshMailCountText();
            _refreshAkeyShow();
        }

        /// <summary>
        /// 有邮件信息变化
        /// </summary>
        /// <param name="_objs"></param>
        private void _onChgMail(params object[] _objs)
        {
            if (null == _m_wMailGrid)
                return;

            long id = (long)_objs[0];//邮件数据实例ID
            _m_wMailGrid.refreshItemBy(id);

            if (!_m_bHasRefreshAkeyShowTask)
            {
                _m_bHasRefreshAkeyShowTask = true;
                ALCommonTaskController.CommonActionAddNextFrameTask(() =>
                {
                    _refreshAkeyShow();
                    _m_bHasRefreshAkeyShowTask = false;
                });
            }
        }

        /// <summary>
        /// 是否有刷新一键操作显示物体的任务
        /// </summary>
        private bool _m_bHasRefreshAkeyShowTask;

        #endregion
    }
}
