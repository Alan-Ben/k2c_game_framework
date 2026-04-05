using System;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 火星科技详情属性展示数据结构
    /// </summary>
    public struct MasrPropertyShowInfo
    {
        public _IPropertyShow propertyShow;

        public long nowValue;//当前值

        public long nextValue;//下一值
    }
    
    public class GGUIWndMarsPropertyShowItem : _ANPGGUIBasicSubWnd<GGUIMonoMarsPropertyShowItem>
    {
        private MasrPropertyShowInfo _m_showData;
        private string _m_sDecimalPlacesShowFormat;//小数位数显示格式
        
        public GGUIWndMarsPropertyShowItem(GGUIMonoMarsPropertyShowItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
        }

        protected override void _onDiscard()
        {
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
        }

        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="_data">属性展示数据</param>
        public void setData(MasrPropertyShowInfo _data, string _decimalPlacesShowFormat = "")
        {
            _m_showData = _data;
            _m_sDecimalPlacesShowFormat = _decimalPlacesShowFormat;
            
            _refreshWnd();
        }


        /// <summary>
        /// 刷新窗口
        /// </summary>
        private void _refreshWnd()
        {
            if (wnd == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtPropertyName, TextTranslate.instance.getLanguage(_m_showData.propertyShow?.simpleName));

            // 设置当前属性值
            string nowValueStr = _m_showData.propertyShow?.getValueStr(_m_showData.nowValue, string.IsNullOrEmpty(_m_sDecimalPlacesShowFormat) ? "F2": _m_sDecimalPlacesShowFormat) ?? 
                                 _m_showData.nowValue.ToString();
            //若大于等于0, 前面添加+号
            if (_m_showData.nowValue >= 0 && wnd.nonnegativeNeedShowPlusSign)
            {
                nowValueStr = TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, nowValueStr);
            }
            // 是百分比加成，加上百分号
            if (_m_showData.propertyShow?.isAddPer ?? false)
            {
                nowValueStr = TextTranslate.instance.getLanguage(TransKeyConst.common_percentage_num, nowValueStr);
            }
            if (wnd.txtNowPropertyValueList != null)
            {
                foreach (var txt in wnd.txtNowPropertyValueList)
                {
                    ALUGUICommon.setLabelTxt(txt, nowValueStr);
                }
            }
            
            // 设置变化属性值
            long chgValue = _m_showData.nextValue - _m_showData.nowValue;
            string chgValueStr = _m_showData.propertyShow?.getValueStr(chgValue, string.IsNullOrEmpty(_m_sDecimalPlacesShowFormat) ? "F2": _m_sDecimalPlacesShowFormat) ?? 
                                 chgValue.ToString();
            //若大于等于0, 前面添加+号
            if (chgValue >= 0)
            {
                chgValueStr = TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, chgValueStr);
            }
            // 是百分比加成，加上百分号
            if (_m_showData.propertyShow?.isAddPer ?? false)
            {
                chgValueStr = TextTranslate.instance.getLanguage(TransKeyConst.common_percentage_num, chgValueStr);
            }
            if (wnd.txtChgPropertyValueList != null)
            {
                foreach (var txt in wnd.txtChgPropertyValueList)
                {
                    ALUGUICommon.setLabelTxt(txt, chgValueStr);
                }
            }
            
            ALUGUICommon.setGameObjEnable(wnd.hasChgValueShowList, chgValue != 0);
            ALUGUICommon.setGameObjEnable(wnd.noChgValueShowList, chgValue == 0);
            
            // 设置下一级属性值
            string nextValueStr = _m_showData.propertyShow?.getValueStr(_m_showData.nextValue, string.IsNullOrEmpty(_m_sDecimalPlacesShowFormat) ? "F2": _m_sDecimalPlacesShowFormat) ?? 
                                  _m_showData.nextValue.ToString();
            //若大于等于0, 前面添加+号
            if (_m_showData.nextValue >= 0 && wnd.nonnegativeNeedShowPlusSign)
            {
                nextValueStr = TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, nextValueStr);
            }
            // 是百分比加成，加上百分号
            if (_m_showData.propertyShow?.isAddPer ?? false)
            {
                nextValueStr = TextTranslate.instance.getLanguage(TransKeyConst.common_percentage_num, nextValueStr);
            }
            if (wnd.txtNextPropertyValueList != null)
            {
                foreach (var txt in wnd.txtNextPropertyValueList)
                {
                    ALUGUICommon.setLabelTxt(txt, nextValueStr);
                }
            }
        }
    }
}