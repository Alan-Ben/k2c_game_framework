using ALPackage;
using Common.StageGoalObj;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 阶段目标到达时代之巅详情弹窗列表item
    /// </summary>
    public class GGUIWndStageGoalReachPeakDetailContainerItem : _ATALBasicUISubWnd<GGUIMonoStageGoalReachPeakDetailContainerItem>
    {
        // 显示操作序列号
        private long _m_lShowSerialize;

        public GGUIWndStageGoalReachPeakDetailContainerItem(GGUIMonoStageGoalReachPeakDetailContainerItem _mono) : base(_mono)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
        }

        protected override void _onWndInitDone()
        {
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_id"></param>
        public void setInfo(StageGoal_BigStepFirstReachInfo _info, long _rank)
        {
            if (wnd == null || _info == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtRank, _rank);
            ALUGUICommon.setLabelTxt(wnd.txtTime, TimeUtil.DateTime2StringMDYHMS(TimeUtil.FromUTCMilliseconds(_info.getReachTimeMs())));
            ALUGUICommon.setLabelTxt(wnd.txtName ,"");
            ALUGUICommon.setGameObjEnable(wnd.goSelfHideList, _info.getCid() != NPPlayer.instance.playerInfo.CID);
            ALUGUICommon.setGameObjEnable(wnd.goSelfShowList, _info.getCid() == NPPlayer.instance.playerInfo.CID);

            //请求获取玩家信息，展示名字
            _m_lShowSerialize = ALSerializeOpMgr.next();
            long curSerialize = _m_lShowSerialize;
            GCommon.reqPlayerInfo(_info.getCid(), _playerInfo =>
            {
                if (_playerInfo == null || curSerialize != _m_lShowSerialize || !isShow)
                    return;

                ALUGUICommon.setLabelTxt(wnd.txtName, _playerInfo.name);
            });
        }
    }
}