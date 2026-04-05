using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using NPEnum;
using UnityEngine;

namespace GOE
{
    public class GGUIWndLoverCollectRescue : _ATALBasicUIWnd<GGUIMonoLoverCollectRescue>
    {
        [NotNull] public static GGUIWndLoverCollectRescue instance { get { return _g_instance ??= new GGUIWndLoverCollectRescue(); } }
        private static GGUIWndLoverCollectRescue _g_instance;

        private ALCommonEnableTaskController _m_autoPlayTask;
        private float _m_fAutoPlayTimer;
        private float _m_fAutoPlayDuration;


        public GGUIWndLoverCollectRescue()
            : base(EALUIWndLayer.ADDITION)
        {
        }


        protected override string _monoAssetPath { get { return GGUIMonoLoverCollectRescue.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoLoverCollectRescue.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        protected override void _onShowWnd()
        {
            refreshWnd();
            _playDialogBubbleAnim();
            _startAutoPlayTimer();
        }
        protected override void _onHideWnd()
        {
            _stopAutoPlayTimer();
        }
        protected override void _onReset()
        {
        }
        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnRescue, _onBtnRescueClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnGoEarn, _onBtnGoEarnClick);

            GGUIMonoLoverCollectDialogData dialogData = wnd.dialogData;
            if (dialogData != null)
                ALUGUICommon.uncombineBtnClick(dialogData.btnDialogBubble, _onBtnDialogBubbleClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClick);
            ALUGUICommon.combineBtnClick(wnd.btnRescue, _onBtnRescueClick);
            ALUGUICommon.combineBtnClick(wnd.btnGoEarn, _onBtnGoEarnClick);

            GGUIMonoLoverCollectDialogData dialogData = wnd.dialogData;
            if (dialogData != null)
                ALUGUICommon.combineBtnClick(dialogData.btnDialogBubble, _onBtnDialogBubbleClick);
        }


        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            long curEarnings = NPPlayer.instance.getValue(ENPPlayerValueType.EARNINGS);
            long needEarnings = GRefdataCoreMgr.instance.npGeneral.lover_collect_need_earn_speed;
            bool canRescue = curEarnings >= needEarnings;

            if (wnd.sliderProgress != null)
                wnd.sliderProgress.value = needEarnings > 0 ? Mathf.Clamp01((float)curEarnings / needEarnings) : 0;

            ALUGUICommon.setLabelTxt(wnd.txtProgress, TextTranslate.instance.getLanguage(TransKeyConst.loverCollect_earnProgress_num_num, curEarnings.ToLargeString(PrimitiveExtension.ELargeStringType.GOLD), needEarnings.ToLargeString(PrimitiveExtension.ELargeStringType.GOLD)));

            long diff = needEarnings - curEarnings;
            ALUGUICommon.setLabelTxt(wnd.txtDiffEarnings, TextTranslate.instance.getLanguage(TransKeyConst.loverCollect_needEarnDiff_num, diff.ToLargeString(PrimitiveExtension.ELargeStringType.GOLD)));

            wnd.setCanRescueState(canRescue);
        }


        #region Dialog

        private void _playDialogBubbleAnim()
        {
            if (wnd == null || wnd.dialogData == null)
                return;

            GGUIMonoLoverCollectDialogData dialogData = wnd.dialogData;
            List<GGUIMonoLoverCollectDialogKeys> keysList = dialogData.listDialogKeys;
            if (keysList == null)
                return;

            long curEarnings = NPPlayer.instance.getValue(ENPPlayerValueType.EARNINGS);
            long needEarnings = GRefdataCoreMgr.instance.npGeneral.lover_collect_need_earn_speed;
            bool canRescue = curEarnings >= needEarnings;

            for (int i = 0; i < keysList.Count; i++)
            {
                GGUIMonoLoverCollectDialogKeys keys = keysList[i];
                if (keys.use_condition != null && keys.use_condition.hasCondition && !keys.use_condition.IsEnable(null))
                    continue;

                List<string> dialogKeys = canRescue ? keys.listDialogKeyCanRescue : keys.listDialogKeyNotCanRescue;
                if (dialogKeys == null || dialogKeys.Count <= 0)
                    break;

                string randomKey = dialogKeys.GetRandomItem();
                ALUGUICommon.setLabelTxt(dialogData.txtDialog, TextTranslate.instance.getLanguage(randomKey));
                break;
            }
            
            if (dialogData.animDialogBubble != null)
                dialogData.animDialogBubble.ForcePlay(dialogData.animNameDialogBubble);
        }

        private void _tickAutoPlay()
        {
            if (_m_fAutoPlayDuration <= 0)
                return;

            _m_fAutoPlayTimer -= Time.deltaTime;
            if (_m_fAutoPlayTimer > 0)
                return;

            _m_fAutoPlayTimer = _m_fAutoPlayDuration;
            _playDialogBubbleAnim();
        }

        private void _startAutoPlayTimer()
        {
            _stopAutoPlayTimer();

            if (wnd == null || wnd.dialogData == null)
                return;

            _m_fAutoPlayDuration = wnd.dialogData.autoPlayDialogBubbleAnimTime;
            _m_fAutoPlayTimer = _m_fAutoPlayDuration;

            if (_m_fAutoPlayDuration > 0)
                _m_autoPlayTask = ALCommonTaskController.CommonEnableTickActionAddMonoTask(_tickAutoPlay);
        }

        private void _stopAutoPlayTimer()
        {
            _m_autoPlayTask.setDisable();
            _m_fAutoPlayDuration = 0;
        }

        #endregion


        private void _onBtnCloseClick(GameObject _obj)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_LOVER_COLLECT_RESCUE);
        }

        private void _onBtnRescueClick(GameObject _obj)
        {
            if (wnd == null)
                return;

            long performGroupId = wnd.performGroupId;
            NPPlayer.instance.loverCollectComp.reqClaimLover((_isSuc) =>
            {
                if (!_isSuc)
                    return;

                QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_LOVER_COLLECT_RESCUE);
                GCommon.playPerformGroup(performGroupId, () =>
                {
                    long consortId = NPPlayer.instance.loverCollectComp.targetLoverId;
                    GGottenConsortInfo gottenConsortInfo = NPPlayer.instance.consortComp.getConsortInfo(consortId);
                    if (gottenConsortInfo?.consortRefObj == null)
                        return;

                    AccountSettingMgr.instance.accountSetting.addAlreadyShowGainConsort(consortId);

                    NPUINoticeMgr.instance.addDealer(new NPNoticeDealer_GetConsort(gottenConsortInfo));
                    NPPlayer.instance.dinnerComp.showConsortPermitGetNotice(consortId);
                });
            });
        }

        private void _onBtnGoEarnClick(GameObject _obj)
        {
            NPGGUIWndEffectCustomAddUI effectCustomAddUIWnd = new NPGGUIWndEffectCustomAddUI(2402);
            QueueMgr.instance.addNode_InGame_SingleWnd(effectCustomAddUIWnd, effectCustomAddUIWnd.showWnd, EUIQueueStageType.MAIN, UINodeTagConst.C_EFFECT_CUSTOM_ADD_UI, false, true);
            hideWnd(); // blur 效果有点问题，这里 hack 处理一下
        }

        private void _onBtnDialogBubbleClick(GameObject _obj)
        {
            _playDialogBubbleAnim();
            _m_fAutoPlayTimer = _m_fAutoPlayDuration;
        }
    }
}
