using System;
using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    [Serializable]
    public class TextWithCustomKey
    {
        public Text txtValue;
        public string valueKey;


#if NP_GAME
        public void setValue(string _value)
        {
            ALUGUICommon.setLabelTxt(txtValue, string.IsNullOrEmpty(valueKey) ? _value : TextTranslate.instance.getLanguage(valueKey, _value));
        }
        public void setValue(int _value)
        {
            setValue(_value.ToString());   
        }
        public void setValue(long _value)
        {
            setValue(_value.ToString());   
        }
        public void setValue(float _value)
        {
            setValue(_value.ToString());   
        }
        public void setValue(double _value)
        {
            setValue(_value.ToString());
        }
#endif
    }
}