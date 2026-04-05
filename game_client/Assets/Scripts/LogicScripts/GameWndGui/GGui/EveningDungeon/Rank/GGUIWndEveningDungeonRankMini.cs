using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 晚间活动排行小窗口
    /// </summary>
    public class GGUIWndEveningDungeonRankMini : _ANPGGUIBasicSubWnd<GGUIMonoEveningDungeonRankMini>
    {
        //排行榜列表
        private List<EveningDungeonRankInfo> _m_lRankShowInfoList;
        // 自己排行数据
        private Common.RankObj.Rank_BaseItem _m_SelfRankShowInfo;
        
        private long _m_lShowSerialize = 0;
        
        public GGUIWndEveningDungeonRankMini(GGUIMonoEveningDungeonRankMini _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;
            
            ALUGUICommon.combineBtnClick(wnd.openRankBtn, _onOpenRankDetailBtnClick);
        }
        
        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.openRankBtn, _onOpenRankDetailBtnClick);
            }

            _m_lRankShowInfoList?.Clear();
            _m_lRankShowInfoList = null;

            _m_SelfRankShowInfo = null;
        }
        
        protected override void _onShowWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
        }

        protected override void _onHideWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
        }

        protected override void _onReset()
        {
        }

        /// <summary>
        /// 请求排行数据
        /// </summary>
        private void _reqRankInfo(bool _needResetRankInfo, Action _reqDone)
        {
            List<EveningDungeonRankInfo> oldRankInfoList = _m_lRankShowInfoList;
            long serialize = _m_lShowSerialize;

            //请求排行数据
            NPPlayer.instance.eveningDungeonComp.reqEveningDungeonRankList((_msg) =>
            {
                if (wnd == null || !isShow || serialize != _m_lShowSerialize)
                    return;
                
                _m_lRankShowInfoList = new List<EveningDungeonRankInfo>();
                EveningDungeonRankInfo rankInfo = null;
                Common.RankObj.Rank_BaseItem rankBaseItem = null;

                if (_msg != null && _msg.getRankList() != null)
                {
                    for (int i = 0; i < _msg.getRankList().Count; i++)
                    {
                        rankBaseItem = _msg.getRankList()[i];
                        if(rankBaseItem == null)
                            continue;

                        if (_needResetRankInfo)
                        {
                            rankInfo = oldRankInfoList?.SafeGet(i);
                            rankInfo?.update(rankBaseItem);
                        }
                        else
                        {
                            rankInfo = oldRankInfoList?.FindAndRemove((_info) =>
                            {
                                if (_info != null && _info.cid == rankBaseItem.getKey())
                                    return true;

                                return false;
                            });
                            rankInfo?.updateRankInfo(rankBaseItem.getRank(), rankBaseItem.getScore());
                        }

                        if (rankInfo == null)
                            rankInfo = new EveningDungeonRankInfo(rankBaseItem);
                        
                        _m_lRankShowInfoList.Add(rankInfo);
                    }
                }

                _m_lRankShowInfoList.Sort((_a,_b)=>_a.rankSortId.CompareTo(_b.rankSortId));
                _m_SelfRankShowInfo = _msg?.getSelfRankItem();
                
                _reqDone?.Invoke();
            });
        }
        
        /// <summary>
        /// 刷新排序数据
        /// </summary>
        public void refreshRank(bool _needReqRankInfo)
        {
            if(wnd == null)
                return;
            
            if (_m_lRankShowInfoList == null || _m_lRankShowInfoList.Count <= 0)
            {
                ALUGUICommon.setGameObjEnable(wnd.hasRankInfoShow, false);
            }
            else
            {
                EveningDungeonRankInfo firstRankInfo = _m_lRankShowInfoList[0];
                ALUGUICommon.setGameObjEnable(wnd.hasRankInfoShow, true);
                firstRankInfo?.getInfo(true, (_rankInfo) =>
                {
                    if(_rankInfo == null || _rankInfo.playerInfo == null)
                        return;

                    ALUGUICommon.setLabelTxt(wnd.firstRankInfoDesc,
                        TextTranslate.instance.getLanguage(TransKeyConst.eveningDungeon_rankInfoDesc_str_str,
                            _rankInfo.playerInfo.name,
                            _rankInfo.rankScore.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT)));
                });
            }

            if (_needReqRankInfo)
            {
                _reqRankInfo(true, () =>
                {
                    // 请求完后在刷新一遍
                    refreshRank(false);
                });
            }
        }

        /// <summary>
        /// 打开排行详情按钮被点击
        /// </summary>
        private void _onOpenRankDetailBtnClick(GameObject _go)
        {
            // 请求排行数据
            _reqRankInfo(true, () =>
            {
                refreshRank(false);
                GGUIWndEveningDungeonRankDetail.instance.setData(_m_lRankShowInfoList, _m_SelfRankShowInfo);
            });

            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndEveningDungeonRankDetail.instance, () =>
            {
                GGUIWndEveningDungeonRankDetail.instance.showWnd();
                GGUIWndEveningDungeonRankDetail.instance.setData(_m_lRankShowInfoList, _m_SelfRankShowInfo);
            }, EUIQueueStageType.MAIN, UINodeTagConst.C_EVENING_DUNGEON_RANK_DETAIL, false, false, false);
        }
    }
}