
using ALPackage;
using GOE;
using UnityEngine;

namespace Hotfix
{
    /// <summary>
    /// 2048 宝箱奖励子窗口
    /// </summary>
    public class GGUIWndNumMergeBox : _AHotfixBaseSubWnd<GGUIMonoNumMergeBox>
    {
        private NPGGoIndex _m_boxGoIndex;
        private GameObject _m_boxGo;
        private int _m_boxGoSerialize;
        
        
        public GGUIWndNumMergeBox(GGUIHotfixCommonMono _wnd) : base(_wnd)
        {
            initWnd();
        }


        protected override void _onShowWnd()
        {
            refreshWnd();

            HotfixNPPlayer.instance.numMergeComponent.onBoxDataChg += refreshWnd;
        }
        protected override void _onHideWnd()
        {
            HotfixNPPlayer.instance.numMergeComponent.onBoxDataChg -= refreshWnd;

            _tryDiscardBoxGo();
        }
        protected override void _onReset()
        {
        }
        protected override void _onDiscard()
        {
            if (hotfixWnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(hotfixWnd.btnDetail, _onClickDetail);
            ALUGUICommon.uncombineBtnClick(hotfixWnd.btnGet, _onClickGet);

            _tryDiscardBoxGo();
        }
        protected override void _onWndInitDoneHotfix()
        {
            if (hotfixWnd == null)
                return;

            ALUGUICommon.combineBtnClick(hotfixWnd.btnDetail, _onClickDetail);
            ALUGUICommon.combineBtnClick(hotfixWnd.btnGet, _onClickGet);
        }


        public void refreshWnd()
        {
            if (hotfixWnd == null || !_m_bIsShow)
                return;

            NumMergeBoxInfo boxInfo = HotfixNPPlayer.instance.numMergeComponent.boxInfo;

            int claimableCount = boxInfo.getClaimableCount();
            bool hasReward = claimableCount > 0;

            ALUGUICommon.setLabelTxt(hotfixWnd.txtRewardCount, claimableCount);
            hotfixWnd.setHasReward(hasReward);

            // 进度百分比
            float progress = boxInfo.getCurrentProgress();
            if (hotfixWnd.sldProgress != null)
                hotfixWnd.sldProgress.value = progress;

            // 进度文本：当前分数 / 需要分数
            long currentScore = boxInfo.currentScore;
            long needScore = boxInfo.currentBoxRef?.upgrade_need_score ?? 0;
            ALUGUICommon.setLabelTxt(hotfixWnd.txtValue, TextTranslate.instance.getLanguage(HotfixTransKeyConst.numMerge_boxSld_value2, currentScore, needScore));

            _tryDiscardBoxGo();
            _tryLoadBoxGo();
        }


        private void _onClickDetail(GameObject _go)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndNumMergeBoxDetail.instance, GGUIWndNumMergeBoxDetail.instance.showWnd, HotfixUINodeTagConst.NUMMERGE_BOX_DETAIL);
        }
        private void _onClickGet(GameObject _go)
        {
            NumMergeBoxInfo boxInfo = HotfixNPPlayer.instance.numMergeComponent.boxInfo;

            // 检查是否有可领取的宝箱
            if (!boxInfo.hasClaimableBox())
            {
                // 计算还差多少积分
                long scoreNeeded = boxInfo.getScoreNeededForNext();
                if (scoreNeeded > 0)
                {
                    // 显示提示：还差{0}积分可开启宝箱
                    string tipText = TextTranslate.instance.getLanguage(HotfixTransKeyConst.numMerge_boxScoreNotEnough_count, scoreNeeded);
                    NPGUIAddSceneCenterTip.instance.showTextInfo(tipText);
                }
                return;
            }

            // 请求领取宝箱奖励
            HotfixNPPlayer.instance.numMergeComponent.reqBoxReward(null);
        }

        private void _tryLoadBoxGo()
        {
            if (hotfixWnd?.transBoxGoParent == null)
                return;
            
            NumMergeBoxInfo boxInfo = HotfixNPPlayer.instance.numMergeComponent.boxInfo;
            NumMergeBoxRefObj boxRefObj = boxInfo.currentBoxRef;
            if (boxRefObj == null)
                return;

            _m_boxGoIndex = boxRefObj.box_go_index;
            
            int serialize = _m_boxGoSerialize;
            NPGGoIndex goIndex = _m_boxGoIndex;
            GGoIndexCacheMgr.instance.popItem(goIndex, _go =>
            {
                if (_go == null)
                    return;
                
                if (serialize != _m_boxGoSerialize)
                {
                    // 已经被丢弃了
                    GGoIndexCacheMgr.instance.pushbackItem(goIndex, _go);
                    return;
                }
                
                _go.transform.SetParent(hotfixWnd.transBoxGoParent);
                _go.transform.localPosition = Vector3.zero;
                _go.transform.localScale = Vector3.one;
                _m_boxGo = _go;
            });
        }
        private void _tryDiscardBoxGo()
        {
            if (_m_boxGo != null)
            {
                GGoIndexCacheMgr.instance.pushbackItem(_m_boxGoIndex, _m_boxGo);
                _m_boxGo = null;
            }
            _m_boxGoSerialize = ALSerializeOpMgr.next();
        }
    }
}
