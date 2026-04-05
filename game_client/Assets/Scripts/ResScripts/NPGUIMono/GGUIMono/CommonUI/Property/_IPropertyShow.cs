using System;
using System.Text;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 一种属性应该只存在一种展示实例(例如 火星实例属性展示:MarsPowerPropertyShow)
    /// MarsPropertyShowRefObj 和 PlayerPropertyShowRefObj 并非通过单例类式存在, 是因为他们的每一个实例都是一种类型 
    /// </summary>
    public interface _IPropertyShow
    {
        /// <summary>
        /// 简易名称
        /// </summary>
        public string simpleName { get; }
        
        /// <summary>
        /// 是否万分比加成
        /// </summary>
        public bool isAddPer { get; }
        
        public string getValueStr(long _value, int _foreceDecimalPlaces = 0, int _optionalDecimalPlaces = 2)
        {
            if (isAddPer)
            {
                float perValue = _value / 100f;
                if (_foreceDecimalPlaces == 0 && _optionalDecimalPlaces == 0)
                {
                    return ((long) perValue).ToString();
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("0.");
                    for (int i = 0; i < _foreceDecimalPlaces; i++)
                    {
                        sb.Append("0");
                    }
                    for (int i = 0; i < _optionalDecimalPlaces; i++)
                    {
                        sb.Append("#");
                    }
                    return perValue.ToString(sb.ToString());
                }
            }
            else
            {
                return _value.ToString();
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_value"></param>
        /// <param name="_decimalPlacesShowFormat">小数位显示格式</param>
        /// <returns></returns>
        public string getValueStr(long _value, string _decimalPlacesShowFormat)
        {
            if (isAddPer)
            {
                float perValue = _value / 100f;
                
                return string.IsNullOrEmpty(_decimalPlacesShowFormat) ? perValue.ToString() : perValue.ToString(_decimalPlacesShowFormat);
            }
            else
            {
                return _value.ToString();
            }
        }
    }
    
    /// <summary>
    /// 属性展示接口
    /// </summary>
    public interface _IEnumPropertyShow : _IPropertyShow
    {
        public Enum propertyEnum { get; }

        /// <summary>
        /// 转化为具体属性枚举
        /// </summary>
        /// <param name="_enum"></param>
        /// <typeparam name="T_Enum"></typeparam>
        /// <returns></returns>
        public bool toEnum<T_Enum>(out T_Enum _enum) where T_Enum : Enum
        {
            _enum = default;
            if(propertyEnum is T_Enum enumValue)
            {
                _enum = enumValue;
                return true;
            }

            return false;
        }
    }
}