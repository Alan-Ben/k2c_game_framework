using System;
using System.Collections.Generic;
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 排行榜item
    /// </summary>
    public class NPGGUIWndRankFixedItem : _ANPGGUIBasicSubWnd<NPGGUIMonoRankFixedItem>
    {
        //请求第一名的刷新操作序列号
        private long _m_lRefreshSerialize;
        //展示信息
        private NPRankFixedShowInfo _m_fixedInfo;
        //常驻排行榜id列表
        private List<long> _m_fixedRankIdList;
        //记录是否有联盟在排行榜中完成
        private Action _m_aOnRecordGuildInRankDone;

        /// <summary>
        /// 常驻排行榜id
        /// </summary>
        public long rankId { get { return wnd == null ? 0 : wnd.fixedRankId; } }

        public NPGGUIWndRankFixedItem(NPGGUIMonoRankFixedItem _wnd)
           : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
            _refresh();
        }

        protected override void _onHideWnd()
        {
            _m_lRefreshSerialize = ALSerializeOpMgr.next();
            _m_fixedInfo?.clearData();

            //重置动画
            wnd?.needResetAni?.resetAni();
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            if (null == wnd)
                return;

            _m_fixedInfo?.clearData();
            _m_fixedInfo = null;

            ALUGUICommon.uncombineBtnClick(wnd.clickBtn, _clickBtnDidClick);
        }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;

            _m_fixedInfo = new NPRankFixedShowInfo();

            ALUGUICommon.combineBtnClick(wnd.clickBtn, _clickBtnDidClick);
        }

        public void setFixedRankIdList(List<long> _fixedRankIdList, Action _onRecordGuildInRankDone)
        {
            _m_fixedRankIdList = _fixedRankIdList;
            _m_aOnRecordGuildInRankDone = _onRecordGuildInRankDone;
            _refresh();
        }

        /// <summary>
        /// 处理点击item
        /// </summary>
        public void dealClickItem()
        {
            _clickBtnDidClick(null);
        }

        /// <summary>
        /// 刷新
        /// </summary>
        private void _refresh()
        {
            if (null == wnd)
                return;

            NPRankFixedRefObj refObj = GRefdataCoreMgr.instance.rankFixedRefCore.getRef(wnd.fixedRankId);
            if (null == refObj)
                return;

            NPRankRefObj comRefObj = GRefdataCoreMgr.instance.rankCommonRefCore.getRef(refObj.rank_id);
            if (null == comRefObj)
                return;

            ALUGUICommon.setLabelTxt(wnd.rankNameTxt, comRefObj.nameStr);

            //请求序列号
            _m_lRefreshSerialize = ALSerializeOpMgr.next();
            long opSerialize = _m_lRefreshSerialize;

            ALUGUICommon.setGameObjEnable(wnd.canLikeShowGoList, NPPlayer.instance.fixedCdComp.getCount(refObj.like_fixed_cd_id) > 0);

            //请求第一名信息
            _m_fixedInfo?.reqFirstRankInfo(wnd.fixedRankId, (_showinfo) =>
            {
                if (opSerialize != _m_lRefreshSerialize || null == wnd || null == comRefObj || null == _showinfo || refObj == null)
                    return;

                //判断是不是联盟排行榜，设置是否有联盟在排行榜中，并刷新红点
                bool isGuildRank = comRefObj.rank_type == ERankType.GUILD;
                if (isGuildRank)
                {
                    NPPlayer.instance.rankCommonComp.recordHaveGuildInRank(refObj.id, _showinfo.cid > 0);
                    _m_aOnRecordGuildInRankDone?.Invoke();
                }

                //cid为0表示没有第一名
                if (_showinfo.cid <= 0)
                {
                    ALUGUICommon.setGameObjEnable(wnd.canLikeShowGoList, false); 
                    ALUGUICommon.setGameObjEnable(wnd.noFirstShowGo, true); 
                    ALUGUICommon.setGameObjEnable(wnd.noFirstHideGo, false);
                }
                else
                {
                    ALUGUICommon.setGameObjEnable(wnd.noFirstShowGo, false);
                    ALUGUICommon.setGameObjEnable(wnd.noFirstHideGo, true);
                    ALUGUICommon.setGameObjEnable(wnd.canLikeShowGoList, NPPlayer.instance.fixedCdComp.getCount(refObj.like_fixed_cd_id) > 0);

                    if (isGuildRank)
                        ALUGUICommon.setLabelTxt(wnd.firstNameTxt, _showinfo.firstGuildInfo?.name);
                    else
                        ALUGUICommon.setLabelTxt(wnd.firstNameTxt, _showinfo.firstPlayerInfo?.name);

                    ALUGUICommon.setLabelTxt(wnd.firstScoreTxt, GCommon.getValueFormatStr(comRefObj.process_num_format, _showinfo.score));
                }
            });
        }

        private void _clickBtnDidClick(GameObject _go)
        {
            if (null == wnd)
                return;

            //是否是联盟排行榜
            NPRankFixedRefObj rankFixRef = GRefdataCoreMgr.instance.rankFixedRefCore.getRef(wnd.fixedRankId);
            NPRankRefObj rankRef = GRefdataCoreMgr.instance.rankCommonRefCore.getRef(rankFixRef != null ? rankFixRef.rank_id : 0);
            bool isGuild = rankRef != null && rankRef.rank_type == ERankType.GUILD;

            if (null == _m_fixedRankIdList || _m_fixedRankIdList.Count == 0)
            {
                if(isGuild)
                    QueueMgr.instance.AddNode(new GNodeGuildRank(wnd.fixedRankId));
                else
                    QueueMgr.instance.AddNode(new GNodeRank(wnd.fixedRankId));
            }
            else
            {
                if(isGuild)
                    QueueMgr.instance.AddNode(new GNodeGuildRank(_m_fixedRankIdList, _m_fixedRankIdList.IndexOf(wnd.fixedRankId)));
                else
                    QueueMgr.instance.AddNode(new GNodeRank(_m_fixedRankIdList, _m_fixedRankIdList.IndexOf(wnd.fixedRankId)));

            }
        }
    }
}

