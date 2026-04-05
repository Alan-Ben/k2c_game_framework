using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 矿石质量排名
    /// </summary>
    public class GGUIWndTreasureHuntOreMassRank : _ANPGGUIBasicWnd<GGUIMonoTreasureHuntOreMassRank>
    {
        private static GGUIWndTreasureHuntOreMassRank _g_instance;
        public static GGUIWndTreasureHuntOreMassRank instance { get { return _g_instance ??= new GGUIWndTreasureHuntOreMassRank(); } }

        private _ITreasureHuntOreInfo _m_oreInfo;
        private List<_ISliderRewardItemInfoPro> _m_lMassRewardInfoList;
        private long _m_lMassMin;
        private long _m_lMassMax;
        
        private List<GGUIWndTreasureHuntOreMassRankItem> _m_lRankItemWndList;
        private GGUIWndTreasureHuntOreInfo _m_wOreInfo;
        private GGUIWndCommonRewardSliderPro _m_wRewardSlider;

        public GGUIWndTreasureHuntOreMassRank() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoTreasureHuntOreMassRank.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoTreasureHuntOreMassRank.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        protected override bool isShowAniPlayOnlyOne { get { return true; } }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            // 初始化矿石信息子窗口
            if (wnd.monoOreInfo != null)
                _m_wOreInfo = new GGUIWndTreasureHuntOreInfo(wnd.monoOreInfo);

            // 初始化奖励进度条
            if (wnd.monoRewardSlider != null)
            {
                _m_wRewardSlider = new GGUIWndCommonRewardSliderPro(wnd.monoRewardSlider);
                _m_wRewardSlider.onClickItem += _onMassRewardSliderItemClick;
            }

            // 初始化排行项目列表
            if (wnd.rankItemList != null)
            {
                _m_lRankItemWndList = new List<GGUIWndTreasureHuntOreMassRankItem>();
                foreach (var rankItemMono in wnd.rankItemList)
                {
                    if (rankItemMono != null)
                        _m_lRankItemWndList.Add(new GGUIWndTreasureHuntOreMassRankItem(rankItemMono));
                }
            }

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }

        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
            }

            _m_wOreInfo?.discard();
            _m_wOreInfo = null;

            if (_m_wRewardSlider != null)
            {
                _m_wRewardSlider.onClickItem -= _onMassRewardSliderItemClick;
                _m_wRewardSlider.discard();
                _m_wRewardSlider = null;                
            }

            if (_m_lRankItemWndList != null)
            {
                foreach (var rankItemWnd in _m_lRankItemWndList)
                {
                    rankItemWnd?.discard();
                }
                _m_lRankItemWndList.Clear();
                _m_lRankItemWndList = null;
            }

            _m_lMassRewardInfoList?.Clear();
            _m_lMassRewardInfoList = null;
            _m_oreInfo = null;
        }

        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_wOreInfo?.hideWnd();
            _m_wRewardSlider?.hideWnd();

            if (_m_lRankItemWndList != null)
            {
                foreach (var rankItemWnd in _m_lRankItemWndList)
                {
                    rankItemWnd?.hideWnd();
                }
            }
        }

        protected override void _onReset()
        {
            _m_wOreInfo?.resetWnd();
            _m_wRewardSlider?.resetWnd();

            if (_m_lRankItemWndList != null)
            {
                foreach (var rankItemWnd in _m_lRankItemWndList)
                {
                    rankItemWnd?.resetWnd();
                }
            }
        }

        public void setData(_ITreasureHuntOreInfo _oreInfo)
        {
            _m_oreInfo = _oreInfo;
            
            // 初始化质量奖励信息列表
            if (_m_lMassRewardInfoList == null)
                _m_lMassRewardInfoList = new List<_ISliderRewardItemInfoPro>();
            _m_lMassRewardInfoList.Clear();
            
            _m_lMassMin = Int64.MaxValue;
            _m_lMassMax = Int64.MinValue;
            if (_m_oreInfo != null && _m_oreInfo.oreRefObj != null && _m_oreInfo.oreRefObj.mass_reward_grade_list != null && wnd != null)
            {
                for (int i = 0, count = _m_oreInfo.oreRefObj.mass_reward_grade_list.Count; i < count; i++)
                {
                    TreasureHuntOreMassRewardGradeInfo rewardGradeInfo = _m_oreInfo.oreRefObj.mass_reward_grade_list[i];
                    if(rewardGradeInfo == null)
                        continue;

                    if (rewardGradeInfo.mass < _m_lMassMin)
                        _m_lMassMin = rewardGradeInfo.mass;
                    if(rewardGradeInfo.mass > _m_lMassMax)
                        _m_lMassMax = rewardGradeInfo.mass;
                   
                    if (rewardGradeInfo.rewardItem != null && rewardGradeInfo.rewardItem.IsValid)
                    {
                        _m_lMassRewardInfoList.Add(new TreasureHuntOreMassRewardInfo(_m_oreInfo.oreId, i, rewardGradeInfo, wnd.rewardSliderItemAssetPath));
                    }
                }
            }

            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if (wnd == null || !isShow || _m_oreInfo == null)
                return;

            // 设置标题文本
            if (wnd.txtTitle != null && _m_oreInfo.oreRefObj != null)
            {
                string titleText = TextTranslate.instance.getLanguage(TransKeyConst.treasureHunt_oreMassRankTitle_str, _m_oreInfo.oreRefObj.name);
                ALUGUICommon.setLabelTxt(wnd.txtTitle, titleText);
            }

            // 刷新矿石信息
            if (_m_wOreInfo != null)
            {
                _m_wOreInfo.showWnd();
                _m_wOreInfo.setData(_m_oreInfo);
            }

            // 刷新奖励进度条
            _refreshOreMassRewardSlider();
            
            // 请求排行榜信息
            _refreshRankInfo();
        }

        private void _refreshRankInfo()
        {
            if (_m_oreInfo == null)
                return;

            // 通过TreasureHuntComponent中的reqTreasureHuntOreRankInfo请求矿石排名信息
            NPPlayer.instance.treasureHuntComponent.reqTreasureHuntOreRankInfo(_m_oreInfo.oreId, (_isSucc, _msg) =>
            {
                if (!_isSucc || _msg == null || wnd == null || !isShow)
                    return;

                // 刷新排行榜列表
                if (_m_lRankItemWndList != null && _msg.getTopThreeList() != null)
                {
                    for (int i = 0; i < _m_lRankItemWndList.Count; i++)
                    {
                        var rankItemWnd = _m_lRankItemWndList[i];
                        var rankInfo = _msg.getTopThreeList().SafeGet(i);
                        
                        if (rankItemWnd != null)
                        {
                            rankItemWnd.showWnd();
                            rankItemWnd.setData(rankInfo);
                        }
                    }
                }

                int selfRank = _msg.getRank();
                // 刷新自己的排行信息
                ALUGUICommon.setLabelTxt(wnd.selfRankText, TextTranslate.instance.getLanguage(TransKeyConst.rankCommon_selfRank_num, selfRank));
                ALUGUICommon.setGameObjEnable(wnd.noRankShow, selfRank <= 0);
            });
        }

        private void _refreshOreMassRewardSlider()
        {
            if (_m_wRewardSlider == null || _m_oreInfo == null)
                return;

            TreasureHuntGotOreInfo gotOreInfo = _m_oreInfo as TreasureHuntGotOreInfo;
            if (gotOreInfo == null)
            {
                _m_wRewardSlider.hideWnd();
                return;
            }

            _m_wRewardSlider.showWnd();
            _m_wRewardSlider.setInfo(_m_lMassMin, _m_lMassMax, _m_oreInfo.mass, _m_lMassRewardInfoList);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_item"></param>
        private void _onMassRewardSliderItemClick(GGUIWndCommonRewardSliderItemPro _item)
        {
            if(_m_oreInfo == null || _item == null || _item.itemInfo == null || !(_item.itemInfo is TreasureHuntOreMassRewardInfo massRewardInfo))
                return;

            ESliderRewardState rewardState = _item.itemInfo.rewardState;
            if (rewardState != ESliderRewardState.CAN_GET)
            {
                if(_item.wnd != null)
                    _item.showRewardItemDetail(_item.wnd.btnClick);
                return;
            }
            
            // 领取质量奖励
            NPPlayer.instance.treasureHuntComponent.reqTreasureHuntDrawOreRecordReward(_m_oreInfo.oreId, massRewardInfo.gradeIndex, (_isSucc, _msg) =>
            {
                if (_isSucc)
                {
                    // 刷新奖励进度条状态
                    _refreshOreMassRewardSlider();
                }
            });
        }

        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_TREASURE_HUNT_ORE_MASS_RANK);
        }
        
        #region 矿石质量奖励进度item

        private class TreasureHuntOreMassRewardInfo : _ISliderRewardItemInfoPro
        {
            public long oreId { get; private set; }
            public int gradeIndex { get; private set; }
            [NotNull] private TreasureHuntOreMassRewardGradeInfo _m_iGradeInfo;
            private string _m_sShowScore;
            private NPCommonAssetPathInfo _m_iAssetPathInfo;
            
            public TreasureHuntOreMassRewardInfo(long _oreId, int _index, [NotNull] TreasureHuntOreMassRewardGradeInfo _gradeInfo, NPCommonAssetPathInfo _assetPathInfo)
            {
                oreId = _oreId;
                gradeIndex = _index;
                _m_iGradeInfo = _gradeInfo;
                _m_sShowScore = TreasureHuntUtil.getOreMassShowStr(_m_iGradeInfo.mass);
                _m_iAssetPathInfo = _assetPathInfo;
            }
            
            public long id { get { return gradeIndex; } }
            public long score { get { return _m_iGradeInfo.mass; } }
            public EValueFormatType valueFormatType { get { return EValueFormatType.NORMAL_NOT_LARGE_STR; } }
            public string showScore { get { return _m_sShowScore; } }

            public ESliderRewardState rewardState
            {
                get
                {
                    TreasureHuntGotOreInfo gotOreInfo = NPPlayer.instance.treasureHuntComponent.getGotOreInfo(oreId);
                    if(gotOreInfo == null || gotOreInfo.mass < _m_iGradeInfo.mass)
                        return ESliderRewardState.CAN_NOT_GET;

                    if (gotOreInfo.hadDrawRecordReward(gradeIndex))
                        return ESliderRewardState.ALREADY_GET;

                    return ESliderRewardState.CAN_GET;
                }
            }

            public NPGTextureIndex icon { get { return null; } }
            public _IItem showRewardItem { get { return _m_iGradeInfo.rewardItem; } }

            public NPCommonAssetPathInfo rewardItemAssetPath { get { return _m_iAssetPathInfo; } }
        }
        
        #endregion
    }
}