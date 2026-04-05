using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 游历地点事件item - 妃子好感度类型Wnd
    /// </summary>
    public class GGUIWndTravelPosEventItemConsortLike : _AGGUIWndTravelPosEventItem<GGUIMonoTravelPosEventItemConsortLike>
    {
        private NPGGUIWndProgress _m_wLikeProgress; // 好感度进度条
        private TravelEventConsortLikeRefObj _m_rConsortLikeRefObj; // 好感度事件配表

        public GGUIWndTravelPosEventItemConsortLike(GGUIMonoTravelPosEventItemConsortLike _mono) : base(_mono)
        {
        }

        protected override void _onWndInitDoneSub()
        {
            if (wnd == null)
                return;

            // 构建好感度进度条
            if (wnd.monoLikeProgress != null)
                _m_wLikeProgress = new NPGGUIWndProgress(wnd.monoLikeProgress);

            // 绑定妃子详情按钮
            if (wnd.btnConsortInfo != null)
                ALUGUICommon.combineBtnClick(wnd.btnConsortInfo, _onBtnConsortInfoClick);
        }

        protected override void _onDiscardSub()
        {
            // 解绑妃子详情按钮
            if (wnd != null && wnd.btnConsortInfo != null)
                ALUGUICommon.uncombineBtnClick(wnd.btnConsortInfo, _onBtnConsortInfoClick);

            _m_wLikeProgress?.discard();
            _m_wLikeProgress = null;
            _m_rConsortLikeRefObj = null;
        }

        protected override void _onShowWndSub()
        {
        }

        protected override void _onHideWndSub()
        {
            _m_wLikeProgress?.hideWnd();
        }

        protected override void _onResetSub()
        {
            _m_wLikeProgress?.resetWnd();
        }

        protected override void _onSetData(TravelEventRefObj _eventRefObj)
        {
            // 获取好感度事件配表
            _m_rConsortLikeRefObj = _eventRefObj == null
                ? null
                : GRefdataCoreMgr.instance.travelEventConsortLikeRefCore.getRef(_eventRefObj.event_id);
        }

        protected override void _onRefreshWnd()
        {
            if (wnd == null || _m_rConsortLikeRefObj == null)
                return;

            _refreshLikeProgress();
        }


        /// <summary>
        /// 刷新好感度进度条
        /// </summary>
        private void _refreshLikeProgress()
        {
            if (_m_wLikeProgress == null || _m_rConsortLikeRefObj == null)
                return;

            long consortId = _m_rConsortLikeRefObj.consort_id;

            // 获取游历妃子信息（当前好感度）
            TravelConsortInfo travelConsortInfo = NPPlayer.instance.travelComp.getTravelConsortInfo(consortId);
            int curLike = travelConsortInfo?.like ?? 0;

            // 获取迎娶所需好感度（作为满值）
            TravelConsortRefObj travelConsortRefObj = GRefdataCoreMgr.instance.travelConsortRefCore.getRef(consortId);
            int maxLike = travelConsortRefObj?.marry_need_like ?? 0;

            _m_wLikeProgress.showWnd();
            if (maxLike > 0)
                _m_wLikeProgress.setProgress(curLike, maxLike, EValueFormatType.NORMAL_NOT_LARGE_STR);
            else
                _m_wLikeProgress.setProgress(1);
        }

        /// <summary>
        /// 妃子详情按钮点击
        /// </summary>
        private void _onBtnConsortInfoClick(GameObject _)
        {
            if (_m_rConsortLikeRefObj == null)
                return;

            GGottenConsortInfo gottenConsortInfo = NPPlayer.instance.consortComp.getConsortInfo(_m_rConsortLikeRefObj.consort_id);
            if (gottenConsortInfo != null)
            {
                GNodeUnLockConsortDetail.addConsortNodeInteraction(gottenConsortInfo);
            }
            else
            {
                QueueMgr.instance.AddNode(new GNodeLockConsortDetail(new ConsortRefShowInfo(_m_rConsortLikeRefObj.consort_id)));
            }
        }
    }
}
