using System;
using System.Collections.Generic;
using ALPackage;
using Common.ActivityEnum;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 阶段任务冲榜活动界面
    /// </summary>
    public class GGUIWndPersonalStepTaskRushRankActivity : _AGGUIWndStepTaskRushRankActivity<GGUIMonoPersonalStepTaskRushRankActivity>
    {
        public GGUIWndPersonalStepTaskRushRankActivity(long _activityId, NPCommonAssetPathInfo _assetPathInfo, Transform _parent) : base(_activityId, _assetPathInfo, _parent)
        {
        }

        protected override void _getRankInfo(Action<long, long> _onGetRankInfo)
        {
            if(_onGetRankInfo == null)
                return;
            
            NPPlayer.instance.commonActivityComp.reqActivityRankBaseInfoByKey(
                _m_rankRushInfo.activityInfo.instanceId,
                _m_rankRushInfo.activityRankRushRefObj.rank_id,
                NPPlayer.instance.playerInfo.CID, _m_rankRushInfo.activityInfo.isCross,
                _msg =>
                {
                    // 回调校验：窗口已关闭或序列号不匹配则忽略
                    if (_msg == null || _msg.getBaseItem() == null || wnd == null || !isShow)
                        return;

                    _onGetRankInfo(_msg.getBaseItem().getRank(), _msg.getBaseItem().getScore());
                });
        }
    }
}