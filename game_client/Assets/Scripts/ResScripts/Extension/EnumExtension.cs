using System;
using System.ComponentModel;
using System.Reflection;

namespace GOE
{
    public static class EnumExtension
    {
        /// <summary>
        /// 获取枚举的描述信息
        /// </summary>
        public static string GetDescription(this Enum em)
        {
            Type type = em.GetType();
            FieldInfo fd = type.GetField(em.ToString());
            if (fd == null)
                return string.Empty;
            object[] attrs = fd.GetCustomAttributes(typeof(DescriptionAttribute), false);
            string name = string.Empty;
            foreach (DescriptionAttribute attr in attrs)
            {
                name = attr.Description;
            }
            return name;
        }
        
        /// <summary>
        /// 转化为大字符串类型
        /// </summary>
        /// <param name="_valueFormatType"></param>
        /// <returns></returns>
        public static PrimitiveExtension.ELargeStringType toLargeStringType(this EValueFormatType _valueFormatType)
        {
            switch (_valueFormatType)
            {
                case EValueFormatType.GOLD:
                    return PrimitiveExtension.ELargeStringType.GOLD;
                
                case EValueFormatType.BATTLE:
                    return PrimitiveExtension.ELargeStringType.BATTLE;
                
                default:
                    return PrimitiveExtension.ELargeStringType.DEFAULT;
            }
        }
        
        public static PrimitiveExtension.ELargeStringType toLargeStringType(this EHarvestType _harvestType)
        {
            switch (_harvestType)
            {
                case EHarvestType.SILVER:
                    return PrimitiveExtension.ELargeStringType.GOLD;
                
                default:
                    return PrimitiveExtension.ELargeStringType.DEFAULT;
            }
            
        }
    }
}