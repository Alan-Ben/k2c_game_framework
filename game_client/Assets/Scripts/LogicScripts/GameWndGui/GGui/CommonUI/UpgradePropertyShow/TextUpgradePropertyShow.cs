
using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    public class TextUpgradePropertyShow<T> : _AUpgradePropertyShow<Text, T>
    {
        private string _m_formatKey;
        
        
        public TextUpgradePropertyShow(CommonUpgradePropertyShow<Text> _propertyShow, string _formatKey) 
            : base(_propertyShow)
        {
            _m_formatKey = _formatKey;
        }

        protected override void _setValue(Text _property, T _value)
        {
            if (string.IsNullOrEmpty(_m_formatKey))
                ALUGUICommon.setLabelTxt(_property, _value?.ToString());
            else
                ALUGUICommon.setLabelTxt(_property, TextTranslate.instance.getLanguage(_m_formatKey, _value?.ToString()));
        }
    }	
}