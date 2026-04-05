using ALPackage;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星到达新节点确认弹窗
    /// </summary>
    public class GGUIWndMarsNewStageConfirm : _ANPGGUIBasicWnd<GGUIMonoMarsNewStageConfirm>
    {
        private static GGUIWndMarsNewStageConfirm _g_instance;
        public static GGUIWndMarsNewStageConfirm instance
        {
            get
            {
                if(_g_instance == null)
                    _g_instance = new GGUIWndMarsNewStageConfirm();
                return _g_instance;
            }
        }

        // 确认按钮点击回调
        private Action _m_aOnClickConfirm;
        // banner图标
        private NPGGuiWndTexture _m_wBannerTex;
        // 当前随机消息key
        private string _m_sRandomMsgKey;

        public GGUIWndMarsNewStageConfirm() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoMarsNewStageConfirm.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoMarsNewStageConfirm.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override bool isShowAniPlayOnlyOne { get { return true; } }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wBannerTex?.hideWnd();
            _m_sRandomMsgKey = null;
        }

        protected override void _onReset()
        {
            _m_wBannerTex?.discardTexture();
        }

        protected override void _onDiscard()
        {
            _m_wBannerTex?.discard();
            _m_wBannerTex = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnClose2, _onBtnCloseClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnConfirm, _onBtnConfirmClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnRandomMsg, _onBtnRandomMsgClick);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.imgBanner != null)
                _m_wBannerTex = new NPGGuiWndTexture(wnd.imgBanner);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClick);
            ALUGUICommon.combineBtnClick(wnd.btnClose2, _onBtnCloseClick);
            ALUGUICommon.combineBtnClick(wnd.btnConfirm, _onBtnConfirmClick);
            ALUGUICommon.combineBtnClick(wnd.btnRandomMsg, _onBtnRandomMsgClick);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_onClickConfirm">点击确认回调</param>
        public void setInfo(Action _onClickConfirm)
        {
            _m_aOnClickConfirm = _onClickConfirm;
            _refreshWnd();
        }

        // 刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null)
                return;

            MarsStageInfo curMarsStageInfo = NPPlayer.instance.marsComp.goToSubComponent.curArrivedMarsStageInfo;
            if (curMarsStageInfo == null || curMarsStageInfo.marsGoRouteRef == null)
                return;

            // 下一个节点信息
            MarsGoRouteRefObj nextGoRouteRefObj = GRefdataCoreMgr.instance.getMarsGoRouteRefByStageId(curMarsStageInfo.marsGoRouteRef.stage_id + 1);
            if (nextGoRouteRefObj == null)
                return;

            // 设置banner图 
            _m_wBannerTex?.showWnd();
            _m_wBannerTex?.setTexture(nextGoRouteRefObj.banner_tex);

            // 设置描述
            ALUGUICommon.setLabelTxt(wnd.txtDesc, TextTranslate.instance.getLanguage(nextGoRouteRefObj.desc, nextGoRouteRefObj.desc_args));
            ALUGUICommon.setLabelTxt(wnd.txtArriveDesc, TextTranslate.instance.getLanguage(nextGoRouteRefObj.arrive_desc, nextGoRouteRefObj.arrive_desc_args));
            ALUGUICommon.setLabelTxt(wnd.txtConfirm, TextTranslate.instance.getLanguage(nextGoRouteRefObj.arrive_confirm_btn_desc));

            // 设置到达时间
            long endTimeMs = curMarsStageInfo.endTimeMs;
            ALUGUICommon.setLabelTxt(wnd.txtArriveTime, TimeUtil.DateTime2StringMDYHM(TimeUtil.FromUTCMilliseconds(endTimeMs)));

            // 设置当前随机消息key
            if (wnd.msgInputField != null)
            {
                _m_sRandomMsgKey = null;
                if(GRefdataCoreMgr.instance.npGeneral.mars_go_to_random_msg_list != null)
                    _m_sRandomMsgKey = GRefdataCoreMgr.instance.npGeneral.mars_go_to_random_msg_list.GetRandomItem();
    
                ALUGUICommon.setLabelTxt(wnd.msgInputField, TextTranslate.instance.getLanguage(_m_sRandomMsgKey));
            }
        }

        /// <summary>
        /// 点击关闭按钮
        /// </summary>
        private void _onBtnCloseClick(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_MARS_ARRIVE_NEW_STAGE_CONFIRM);
        }

        /// <summary>
        /// 点击确认按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onBtnConfirmClick(GameObject _go)
        {
            if (NPPlayer.instance.marsComp.goToSubComponent.curArrivedMarsStageInfo == null)
                return;

            //记录留言信息
            if(wnd != null && wnd.msgInputField != null)
            {
                MarsStageInfo curMarsStageInfo = NPPlayer.instance.marsComp.goToSubComponent.curArrivedMarsStageInfo;
                long stageId = curMarsStageInfo != null && curMarsStageInfo.marsGoRouteRef != null ? curMarsStageInfo.marsGoRouteRef.stage_id : 0;
                string content = wnd.msgInputField.text;

                //如果有输入文本，发送留言信息
                if (!string.IsNullOrEmpty(content) && stageId > 0)
                {
                    //过滤敏感词
                    CharacterDetermineMgr.instance.replaceIllegalCharacter(true, ref content);
                    //请求发送留言
                    NPPlayer.instance.marsComp.goToSubComponent.reqSendStageMsg((int)stageId, content);
                }
            }

            //关闭窗口执行回调
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_MARS_ARRIVE_NEW_STAGE_CONFIRM);
            _m_aOnClickConfirm?.Invoke();
            _m_aOnClickConfirm = null;
        }

        /// <summary>
        /// 点击随机消息按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onBtnRandomMsgClick(GameObject _go)
        {
            if (wnd == null)
                return;

            List<string> randomMsgKeyList = GRefdataCoreMgr.instance.npGeneral.mars_go_to_random_msg_list;
            if (randomMsgKeyList == null)
                return;

            List<string> targetMsgKeyList = new List<string>();
            string targetKey = null;

            if (randomMsgKeyList.Count <= 1)
            {
                targetMsgKeyList.AddRange(randomMsgKeyList);
            }
            else
            {
                for (int i = 0; i < randomMsgKeyList.Count; i++)
                {
                    //去除掉当前显示的随机消息key，保证每次点击随机消息按钮都会切换消息
                    if (!string.IsNullOrEmpty(_m_sRandomMsgKey) && randomMsgKeyList[i] != _m_sRandomMsgKey)
                    {
                        targetMsgKeyList.Add(randomMsgKeyList[i]);
                    }
                }
            }

            targetKey = targetMsgKeyList.GetRandomItem();
            ALUGUICommon.setLabelTxt(wnd.msgInputField, TextTranslate.instance.getLanguage(targetKey));
        }
    }
}