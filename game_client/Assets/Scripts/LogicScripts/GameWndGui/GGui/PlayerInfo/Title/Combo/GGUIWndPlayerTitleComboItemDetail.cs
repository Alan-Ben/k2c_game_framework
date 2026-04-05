using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 玩家组合称号详情弹窗
    /// </summary>
    public class GGUIWndPlayerTitleComboItemDetail : _ATNPGGUIWndCommonItemToolTip<GGUIMonoPlayerTitleComboItemDetail>
    {
        private ENPItemType _m_eType;
        private long _m_lId;

        public GGUIWndPlayerTitleComboItemDetail() : base(GGUIMonoPlayerTitleComboItemDetail.assetPath, GGUIMonoPlayerTitleComboItemDetail.objName)
        {
        }

        protected override void _onClose()
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_PlayerInfo.C_MAIN_PLAYERINFO_COMBO_TITLE_ITEM_DETAIL_NODE);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_targetTransRoot"></param>
        /// <param name="_intervalX"></param>
        /// <param name="_intervalY"></param>
        public void setInfo(ENPItemType _type, long _id, RectTransform _targetTransRoot, float _intervalX, float _intervalY)
        {
            _m_eType = _type;
            _m_lId = _id;
            _refreshWnd();
            setPos(_targetTransRoot, _intervalX, _intervalY);
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtName, GCommon.getItemName(_m_eType, _m_lId));
            ALUGUICommon.setLabelTxt(wnd.txtDesc, GCommon.getItemDesc(_m_eType, _m_lId));
            ALUGUICommon.setLabelTxt(wnd.txtSource, GCommon.getItemSource(_m_eType, _m_lId));

            switch (_m_eType)
            {
                case ENPItemType.TITLE_PRE:
                    ALUGUICommon.setLabelTxt(wnd.txtType, TextTranslate.instance.getLanguage(TransKeyConst.playerInfo_comboTitlePrefix_none));
                    break;
                case ENPItemType.TITLE_SFX:
                    ALUGUICommon.setLabelTxt(wnd.txtType, TextTranslate.instance.getLanguage(TransKeyConst.playerInfo_comboTitleSuffix_none));
                    break;
                case ENPItemType.TITLE_BG:
                    ALUGUICommon.setLabelTxt(wnd.txtType, TextTranslate.instance.getLanguage(TransKeyConst.playerInfo_comboTitleBg_none));
                    break;
            }
        }
    }
}