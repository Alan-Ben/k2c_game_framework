using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 通用带自定义标题和文本的跟随窗口
    /// </summary>
    public class GGUIWndCommonItemToolTip_Title_Text :_ATNPGGUIWndCommonItemToolTip<GGUIMonoCommonToolTip_Title_Text>
    {
        private string _m_textContent;
        
        public GGUIWndCommonItemToolTip_Title_Text(string _assetPath, string _assetName) : base(_assetPath, _assetName)
        {
            
        }
        

        protected override float getSelfHeight()
        {
            if (null == wnd)
                return 0;
            
            return wnd.heightWithoutText + wnd.getTextHeight(_m_textContent);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo(string _textTitle, string _textValue, RectTransform _targetTransRoot, float _intervalX,float _intervalY)
        {
            if(null == wnd)
                return;

            _m_textContent = _textValue;
            ALUGUICommon.setLabelTxt(wnd.txtTitle, _textTitle);
            ALUGUICommon.setLabelTxt(wnd.txtDesc, _m_textContent);
            if (wnd.txtDesc != null) LayoutRebuilder.ForceRebuildLayoutImmediate(wnd.txtDesc.rectTransform);

            //设置位置
            setPos(_targetTransRoot, _intervalX,_intervalY);
        }
        
        protected override void _onClose()
        {
            QueueMgr.instance.forceCloseNodeByType(typeof(GNodeCommonToolTip_Title_Text));
        }
    }
}