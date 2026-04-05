using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

/*************************
 * 刷新滑动区域内的窗口尺寸的mono任务对象
 **/
namespace ALPackage
{
    public class ALUGUIWndContainerRefreshGridMonoTask : _IALBaseMonoTask
    {
        /** 数据存放布局对象 */
        private LayoutGroup _m_lgGrid;
        private GridLayoutGroup.Axis _m_eAxis;

        public ALUGUIWndContainerRefreshGridMonoTask(LayoutGroup _grid, GridLayoutGroup.Axis _axis)
        {
            _m_lgGrid = _grid;
            _m_eAxis = _axis;
        }

        public void deal()
        {
            if (null == _m_lgGrid || null == _m_lgGrid.gameObject)
                return;

            RectTransform rectTrans = _m_lgGrid.GetComponent<RectTransform>();
            if (null == rectTrans)
                return;

            if (GridLayoutGroup.Axis.Horizontal == _m_eAxis && _m_lgGrid.preferredWidth > 0)
                rectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _m_lgGrid.preferredWidth);
            else if (GridLayoutGroup.Axis.Vertical == _m_eAxis && _m_lgGrid.preferredHeight > 0)
                rectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _m_lgGrid.preferredHeight);
        }
    }
}
