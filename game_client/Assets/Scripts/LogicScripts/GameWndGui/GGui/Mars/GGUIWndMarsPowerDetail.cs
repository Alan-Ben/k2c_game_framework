using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星实力详情弹窗
    /// </summary>
    public class GGUIWndMarsPowerDetail : _ATNPGGUIWndCommonItemToolTip<GGUIMonoMarsPowerDetail>
    {

        public GGUIWndMarsPowerDetail() : base(GGUIMonoMarsPowerDetail.assetPath, GGUIMonoMarsPowerDetail.objName)
        {
        }

        protected override void _onClose()
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_MARS_POWER_DETAIL);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_targetTransRoot"></param>
        /// <param name="_intervalX"></param>
        /// <param name="_intervalY"></param>
        public void setInfo(RectTransform _targetTransRoot, float _intervalX, float _intervalY)
        {
            _refreshWnd();
            setPos(_targetTransRoot, _intervalX, _intervalY);
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null)
                return;

            //当前建筑实力
            long buildingPower = 0;
            foreach (MarsBuildingInfo buildingInfo in NPPlayer.instance.marsComp.buildingSubComponent._getBuildingInfos())
                buildingPower += buildingInfo.powerProperty.value;
            //当前队伍总实力
            long teamPower = NPPlayer.instance.recordComp.getValue(ENPPlayerRecordParam.MARS_TEAM_MAX_POWER);
            //当前科研总实力
            long technologyPower = NPPlayer.instance.marsComp.technologySubComponent.marsPower;
            //百分比加成
            float per = 1 + NPPlayer.instance.playerPropertyMgr.getValue(ENPPlayerPropertyType.MARS_POWER_PER) / 10000f;
            long curPower = (long)((buildingPower + teamPower + technologyPower) * per);
            
            //========当前========
            //当前总实力
            ALUGUICommon.setLabelTxt(wnd.txtCurPower, curPower.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
            //当前建筑总实力
            ALUGUICommon.setLabelTxt(wnd.txtCurBuildingPower, buildingPower.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
            //当前队伍总实力
            ALUGUICommon.setLabelTxt(wnd.txtCurTeamPower, teamPower.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
            //当前科研总实力
            ALUGUICommon.setLabelTxt(wnd.txtCurTechnologyPower, technologyPower.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));


            //========历史========
            //历史最高总实力
            ALUGUICommon.setLabelTxt(wnd.txtMaxPower, NPPlayer.instance.recordComp.getValue(ENPPlayerRecordParam.MARS_MAX_POWER).ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
            //历史最高建筑总实力
            ALUGUICommon.setLabelTxt(wnd.txtMaxBuildingPower, NPPlayer.instance.recordComp.getValue(ENPPlayerRecordParam.MARS_BUILDING_MAX_POWER).ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
            //历史最高队伍总实力
            ALUGUICommon.setLabelTxt(wnd.txtMaxTeamPower, NPPlayer.instance.recordComp.getValue(ENPPlayerRecordParam.MARS_TEAM_MAX_POWER).ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
            //历史最高科研总实力
            ALUGUICommon.setLabelTxt(wnd.txtMaxTechnologyPower, NPPlayer.instance.recordComp.getValue(ENPPlayerRecordParam.MARS_TECH_MAX_POWER).ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));

            //额外百分比加成
            float addPowerPer = NPPlayer.instance.playerPropertyMgr.getValue(ENPPlayerPropertyType.MARS_POWER_PER) / 100f;
            string addPowerPerKey = wnd.addPowerPerKey;
            if (string.IsNullOrEmpty(addPowerPerKey))
                addPowerPerKey = TransKeyConst.common_percentage_num;
            ALUGUICommon.setLabelTxt(wnd.txtAddPowerPer, TextTranslate.instance.getLanguage(addPowerPerKey, addPowerPer));
        }
    }
}