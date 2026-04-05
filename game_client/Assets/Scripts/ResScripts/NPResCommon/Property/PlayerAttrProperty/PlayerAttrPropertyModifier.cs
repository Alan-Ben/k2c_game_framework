using System.Collections.Generic;
using CommonEnum;

namespace GOE
{
    /// <summary>
    /// 玩家四大基础属性修改器
    /// </summary>
    [System.Serializable]
    public class PlayerAttrPropertyModifier : _ATNPBasicPropertyModifier<EBasicAttrType, PlayerAttrPropertyModifier>
    {
        protected override PlayerAttrPropertyModifier _createModifier()
        {
            return new PlayerAttrPropertyModifier();
        }

        public override string ToString()
        {
            List<string> strs = new List<string>();
            for (int i = 0; i < propertyObjList.Count; i++)
            {
                strs.Add(((EBasicAttrType)i).ToString() + ":" + propertyObjList[i].ToString());
            }
            return string.Join("\n", strs.ToArray());
        }
    }
}
