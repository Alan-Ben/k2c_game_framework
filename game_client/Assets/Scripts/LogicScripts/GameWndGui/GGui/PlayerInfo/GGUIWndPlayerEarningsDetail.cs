using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 玩家赚速详情弹窗
    /// </summary>
    public class GGUIWndPlayerEarningsDetail : _ATNPGGUIWndCommonItemToolTip<GGUIMonoPlayerEarningsDetail>
    {

        public GGUIWndPlayerEarningsDetail() : base(GGUIMonoPlayerEarningsDetail.assetPath, GGUIMonoPlayerEarningsDetail.objName)
        {
        }

        protected override void _onClose()
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_PlayerInfo.C_ADD_PLAYER_EARNINGS_DETAIL_NODE);
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

            //额外收益加成
            long additionValue = NPPlayer.instance.playerPropertyMgr.getValue(ENPPlayerPropertyType.EXTRA_EARNINGS_ADD_PER);
            string additionValueStr = TextTranslate.instance.getLanguage(TransKeyConst.common_addPropPer_num, additionValue/100f);
            string targetStr = string.IsNullOrEmpty(wnd.additionTransKey) ? additionValueStr:TextTranslate.instance.getLanguage(wnd.additionTransKey, additionValueStr);
            ALUGUICommon.setLabelTxt(wnd.txtAddition, targetStr);

            //当前总收益
            ALUGUICommon.setLabelTxt(wnd.txtCurEarnings, TextTranslate.instance.getLanguage(TransKeyConst.main_earningsShow_num,
                NPPlayer.instance.specialItemComp.goldData.earnings.ToLargeString(PrimitiveExtension.ELargeStringType.GOLD)));
            //当前建筑总收益
            ALUGUICommon.setLabelTxt(wnd.txtCurBuildingEarnings, TextTranslate.instance.getLanguage(TransKeyConst.main_earningsShow_num, 
                NPPlayer.instance.buildingComp.getTotalBusinessBuildingEarningsPerS().ToLargeString(PrimitiveExtension.ELargeStringType.GOLD)));
            //当前学生总收益
            ALUGUICommon.setLabelTxt(wnd.txtCurChildEarnings, TextTranslate.instance.getLanguage(TransKeyConst.main_earningsShow_num,
                NPPlayer.instance.childComp.totalChildEarnings.ToLargeString(PrimitiveExtension.ELargeStringType.GOLD)));

            //历史最高总收益
            ALUGUICommon.setLabelTxt(wnd.txtMaxEarnings, TextTranslate.instance.getLanguage(TransKeyConst.main_earningsShow_num,
                NPPlayer.instance.playerInfo.getValue(ENPPlayerParam.EARNINGS_MAX_RECORD).ToLargeString(PrimitiveExtension.ELargeStringType.GOLD)));
            //历史最高建筑总收益
            ALUGUICommon.setLabelTxt(wnd.txtMaxBuildingEarnings, TextTranslate.instance.getLanguage(TransKeyConst.main_earningsShow_num,
                NPPlayer.instance.playerInfo.getValue(ENPPlayerParam.BUILDINGS_EARNINGS_MAX_RECORD).ToLargeString(PrimitiveExtension.ELargeStringType.GOLD)));
            //历史最高学生总收益
            ALUGUICommon.setLabelTxt(wnd.txtMaxChildEarnings, TextTranslate.instance.getLanguage(TransKeyConst.main_earningsShow_num,
                NPPlayer.instance.playerInfo.getValue(ENPPlayerParam.CHILD_EARNINGS_MAX_RECORD).ToLargeString(PrimitiveExtension.ELargeStringType.GOLD)));
        }
    }
}