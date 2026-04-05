using System.Collections.Generic;
using Common.MarsEnum;

namespace GOE
{
    /// <summary>
    /// 火星属性修改器
    /// </summary>
    [System.Serializable]
    public class MarsPropertyModifier : _ATNPBasicPropertyModifier<EMarsPropertyType, MarsPropertyModifier>
    {
        protected override MarsPropertyModifier _createModifier()
        {
            return new MarsPropertyModifier();
        }

        public override string ToString()
        {
            List<string> strs = new List<string>();
            for (int i = 0; i < propertyObjList.Count; i++)
            {
                strs.Add(propertyObjList[i].type.ToString() + ":" + propertyObjList[i].value.ToString());
            }
            return string.Join("\n", strs.ToArray());
        }
    }
}