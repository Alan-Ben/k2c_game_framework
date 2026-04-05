using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星任务预览页签
    /// </summary>
    public class GGUIWndMarsMissionPreviewTab : _ATNPGGUIWndCommonTab<EMarsMissionType, GGUIWndMarsMissionPreviewTab>
    {
        private List<GameObject> _m_lGoList;

        public GGUIWndMarsMissionPreviewTab(NPGGUIMonoCommonTab _wnd, EMarsMissionType _bagItemType, List<GameObject> _goList) : base(_wnd, _bagItemType)
        {
            _m_lGoList = _goList;
        }

        /// <summary>
        /// 设置点击tab
        /// </summary>
        public void setClickTab()
        {
            _onClickSelectButton(null);
        }

        public override void setSelected(bool _isSelect)
        {
            base.setSelected(_isSelect);

            ALUGUICommon.setGameObjEnable(_m_lGoList, _isSelect);
        }
    }
}
