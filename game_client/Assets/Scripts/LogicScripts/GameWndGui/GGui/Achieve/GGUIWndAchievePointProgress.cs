using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using CommonEnum;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 成就进度附加窗口
    /// </summary>
    public class GGUIWndAchievePointProgress : _ANPGGUIBasicSubWnd<GGUIMonoAchievePointProgress>
    {
        private EAchieveType _m_eType;//成就类型
        private NPGGUIWndCommonItemContainer _m_stepRewardItemContainer;//奖励列表
        private AchievePointStepRefObj _m_rCurStepRef;//当前成就阶段数据
        private NPGGuiWndTexture _m_wIcon;//图标
        private NPGGUISubOutSetHarvestWnd _m_harvestWnd;//粒子动画wnd
        private long _m_fLastPointValue;//上个成就点
        private long _m_lAudioInstanceId;//音效实例id

        //预览弹窗
        private NPGGUIWndCommonRewardPreview _m_previewWnd;
        private List<CommonUISfxObj> _m_lParticleSfxList;//粒子触发的特效列表

        public GGUIWndAchievePointProgress(GGUIMonoAchievePointProgress _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
            _m_fLastPointValue = -1;
        }

        protected override void _onHideWnd()
        {
            if (null != _m_stepRewardItemContainer)
                _m_stepRewardItemContainer.hideWnd();

            if (null != _m_wIcon)
                _m_wIcon.hideWnd();

            if (_m_lParticleSfxList != null)
            {
                for (int i = 0; i < _m_lParticleSfxList.Count; i++)
                {
                    if (_m_lParticleSfxList[i] != null)
                        _m_lParticleSfxList[i].forceDiscard();
                }
                _m_lParticleSfxList.Clear();
            }

            if(_m_lAudioInstanceId > 0)
                PlayAudioMgr.instance.stopClip(_m_lAudioInstanceId);
        }

        protected override void _onReset()
        {
            if (null != _m_stepRewardItemContainer)
                _m_stepRewardItemContainer.resetWnd();

            if (null != _m_wIcon)
                _m_wIcon.discardTexture();
        }

        protected override void _onDiscard()
        {
            if (null != _m_stepRewardItemContainer)
                _m_stepRewardItemContainer.discard();
            _m_stepRewardItemContainer = null;

            if (null != _m_harvestWnd)
                _m_harvestWnd.discard();
            _m_harvestWnd = null;

            if (null != _m_wIcon)
                _m_wIcon.discard();
            _m_wIcon = null;

            if (null != _m_previewWnd)
                _m_previewWnd.discard();
            _m_previewWnd = null;

            if (_m_lParticleSfxList != null)
            {
                for (int i = 0; i < _m_lParticleSfxList.Count; i++)
                {
                    if (_m_lParticleSfxList[i] != null)
                        _m_lParticleSfxList[i].forceDiscard();
                }
                _m_lParticleSfxList.Clear();
            }
            _m_lParticleSfxList = null;
            
            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnOpenPreview, _onClickOpenPreviewBtn);
            ALUGUICommon.uncombineBtnClick(wnd.btnGetStepReward, _onClickGetRewardBtn);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (null != wnd.stepRewardItemContainer)
                _m_stepRewardItemContainer = new NPGGUIWndCommonItemContainer(wnd.stepRewardItemContainer);

            if (null != wnd.imgIcon)
                _m_wIcon = new NPGGuiWndTexture(wnd.imgIcon);

            if (null != wnd.particleEndRect)
            {
                _m_harvestWnd = new NPGGUISubOutSetHarvestWnd(wnd.particleEndRect, EHarvestType.ACHIEVE_POINT, _particleResChgDelegate, _particleStartDelegate, _particleFirstItemDoneDelegate, _particlePerItemDoneDelegate, _particleAllItemDoneDelegate);
                _m_harvestWnd.init();
            }

            _m_lParticleSfxList = new List<CommonUISfxObj>();

            ALUGUICommon.combineBtnClick(wnd.btnOpenPreview, _onClickOpenPreviewBtn);
            ALUGUICommon.combineBtnClick(wnd.btnGetStepReward, _onClickGetRewardBtn);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_type"></param>
        public void setInfo(EAchieveType _type)
        {
            _m_eType = _type;
            _m_rCurStepRef = NPPlayer.instance.achieveComp.getCurAchievePointRef(_type);
            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            _refreshState();
            _refreshProgress();
            _refreshReward();
        }

        //刷新状态
        private void _refreshState()
        {
            if (wnd == null)
                return;

            //获取状态
            EAchievePointProgressState state = EAchievePointProgressState.CAN_NOT_GET;
            long curPoint = NPPlayer.instance.achieveComp.getAchievePoint(_m_eType);
            if (_m_rCurStepRef == null)
                state = EAchievePointProgressState.ALL_DONE;
            else if(GCommon.isItemEnough(_m_rCurStepRef.need_point,false))
                state = EAchievePointProgressState.CAN_GET;
            else
                state = EAchievePointProgressState.CAN_NOT_GET;

            //设置状态
            if (wnd.showStateList != null)
            {
                GGUIAchievePointProgressState curState = null;
                for (int i = 0; i < wnd.showStateList.Count; i++)
                {
                    if (wnd.showStateList[i] != null && wnd.showStateList[i].stete == state)
                    {
                        curState = wnd.showStateList[i];
                        break;
                    }
                }

                if (curState != null)
                {
                    ALUGUICommon.setGameObjEnable(curState.goShowList, true);
                    ALUGUICommon.setGameObjEnable(curState.goHideList, false);
                }
            }
        }

        //刷新进度
        private void _refreshProgress()
        {
            if(wnd == null || _m_rCurStepRef == null || _m_rCurStepRef.need_point == null)
                return;

            //设置进度条
            long curPoint = NPPlayer.instance.achieveComp.getAchievePoint(_m_eType);
            if (null != _m_harvestWnd)
                _m_harvestWnd.setResChg(curPoint);

            //设置图标
            if (_m_wIcon != null)
            {
                _m_wIcon.showWnd();
                _m_wIcon.setTexture(GCommon.getItemTexIcon(ENPItemType.ACHIEVE_POINT,(long)_m_eType));
            }

            ALUGUICommon.setLabelTxt(wnd.txtStep, _m_rCurStepRef.step_id);
        }

        //刷新奖励
        private void _refreshReward()
        {
            if (_m_rCurStepRef == null)
                return;

            if (_m_stepRewardItemContainer != null)
            {
                _m_stepRewardItemContainer.showWnd();
                _m_stepRewardItemContainer.showItemList(_m_rCurStepRef.reward_item_list);
            }
        }

        #region 点击事件

        //点击打开预览按钮
        private void _onClickOpenPreviewBtn(GameObject _go)
        {
            if (wnd == null || wnd.rewardPreviewResPathId == 0 || null == _m_rCurStepRef)
                return;

            if(null == _m_previewWnd)
            {
                _m_previewWnd = new NPGGUIWndCommonRewardPreview(wnd.rewardPreviewResPathId);
                _m_previewWnd.load();
            }

            string titleStr = TextTranslate.instance.getLanguage(TransKeyConst.achieve_rewardPreview_num, _m_rCurStepRef.need_point.count);
            QueueMgr.instance.addNode_InGame_SingleWnd(_m_previewWnd, () =>
            {
                _m_previewWnd.showWnd();
                _m_previewWnd.setData(_m_rCurStepRef.reward_item_list, titleStr, null, ECommonRewardType.NOT_GET_REWARD, wnd.notGetBoxIcon, wnd.alreadyGetBoxIcon);
            });
        }

        //点击领取奖励按钮
        private void _onClickGetRewardBtn(GameObject _go)
        {
            if (_m_rCurStepRef == null || !GCommon.isItemEnough(_m_rCurStepRef.need_point, false))
                return;

            NPPlayer.instance.achieveComp.reqGainAchievePointReward(_m_rCurStepRef.id, () =>
            {
                setInfo(_m_eType);
            });
        }

        #endregion

        #region 消息事件

        #endregion

        #region 粒子事件

        /// <summary>
        /// 资源变动回调
        /// </summary>
        private void _particleResChgDelegate()
        {
            _setCurPoint();
        }

        /// <summary>
        /// 粒子动画开始回调
        /// </summary>
        private void _particleStartDelegate()
        {
            if (wnd != null && wnd.particleAni != null)
                wnd.particleAni.play(EAchieveParticleAniType.START_PARTICLE);
        }

        /// <summary>
        /// 第一个粒子结束回调
        /// </summary>
        /// <param name="_itemCount"></param>
        private void _particleFirstItemDoneDelegate(long _itemCount)
        {
            //播放特效
            if (wnd != null && wnd.particleSfxId > 0)
            {
                CommonUISfxObj sfxObj = null;
                sfxObj = PlaySfxMgr.instance.playUISfx(wnd.particleSfxId, wnd.particleSfxParent);

                if (sfxObj != null)
                    _m_lParticleSfxList.Add(sfxObj);

                //销毁多余的特效
                if (_m_lParticleSfxList.Count > wnd.particleSfxMaxNum)
                {
                    while (_m_lParticleSfxList.Count > wnd.particleSfxMaxNum)
                    {
                        if (_m_lParticleSfxList.Count <= 0)
                            break;

                        _m_lParticleSfxList[0]?.forceDiscard();
                        _m_lParticleSfxList.RemoveAt(0);
                    }
                }
            }
        }

        /// <summary>
        /// 单个粒子结束回调
        /// </summary>
        private void _particlePerItemDoneDelegate(long _itemCount)
        {
            if (!isShow)
                return;

            _setCurPoint();

            if(wnd != null && wnd.particleAni != null)
                wnd.particleAni.play(EAchieveParticleAniType.SINGLE_PARTICLE);
        }

        private void _particleAllItemDoneDelegate()
        {
            if (!isShow)
                return;

            //动画结束后刷新，当前阶段变化
            _refreshWnd();
        }


        /// <summary>
        /// 设置当前分数
        /// </summary>
        private void _setCurPoint()
        {
            if (_m_harvestWnd == null || null == _m_rCurStepRef)
                return;

            //设置进度条
            float curValue = 0;
            long curPoint = _m_harvestWnd.getCurCount();
            long targetPoint = _m_rCurStepRef.need_point.count;
            string curPointStr = GCommon.addSizeForRichText(curPoint.ToString(), wnd.curScoreTextSize);
            curValue = curPoint * 1.0f / targetPoint;
            if (curValue < 0f)
                curValue = 0f;
            if (curValue > 1f)
                curValue = 1f;

            //播放可领取音效
            //如果还没记录上次的值，先设置一下
            if(_m_fLastPointValue < 0)
                _m_fLastPointValue = curPoint;

            //如果这次达成了目标值，播放音效
            if (_m_fLastPointValue < targetPoint && curPoint >= targetPoint)
            {
                //先销毁旧的音效
                if (_m_lAudioInstanceId > 0)
                    PlayAudioMgr.instance.stopClip(_m_lAudioInstanceId);

                if(wnd.canGetAudioId > 0)
                    _m_lAudioInstanceId = PlayAudioMgr.instance.playClip(wnd.canGetAudioId);
            }
            _m_fLastPointValue = curPoint;

            ALUGUICommon.setSliderScale(wnd.sldProgress, curValue);
            ALUGUICommon.setLabelTxt(wnd.txtProgress, TextTranslate.instance.getLanguage(TransKeyConst.common_useNum_num_num, curPointStr, targetPoint));
        }

        #endregion
    }
}
