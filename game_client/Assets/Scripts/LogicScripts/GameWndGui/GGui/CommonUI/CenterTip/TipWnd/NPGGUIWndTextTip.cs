using ALPackage;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 文本tip模板
    /// </summary>
    public class NPGGUIWndTextTip : _ATNPGGUIWndTip<NPGGUIMonoTextTip>
    {
        public NPGGUIWndTextTip(NPGGUIMonoTextTip _wnd)
            : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWndEx()
        {

        }

        protected override void _onHideWndEx()
        {

        }

        protected override void _onResetEx()
        {

        }

        protected override void _onDiscardEx()
        {

        }

        protected override void _onWndInitDoneEx()
        {

        }

        /// <summary>
        /// 设置tip数据
        /// </summary>
        /// <param name="_strList"></param>
        public void setTipData(List<string> _strList)
        {
            if (_strList == null || wnd == null || wnd.txtStrList == null)
                return;

            int count = 0;//记录已赋值的text控件数量

            for (; count < _strList.Count; count++)
            {
                if (count >= wnd.txtStrList.Count)
                    break;

                ALUGUICommon.setLabelTxt(wnd.txtStrList[count], _strList[count]);
            }

            //多余的text控件设置文本为空
            for (; count < wnd.txtStrList.Count; count++)
            {
                ALUGUICommon.setLabelTxt(wnd.txtStrList[count], string.Empty);
            }
        }
    }
}