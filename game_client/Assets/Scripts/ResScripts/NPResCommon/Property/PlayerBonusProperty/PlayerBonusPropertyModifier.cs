using System.Collections.Generic;
using CommonEnum;

namespace GOE
{
    /// <summary>
    /// 玩家四大基础属性修改器
    /// </summary>
    [System.Serializable]
    public class PlayerBonusPropertyModifier : _ATNPBasicPropertyModifier<EBonusPropertyType, PlayerBonusPropertyModifier>
    {
        protected override PlayerBonusPropertyModifier _createModifier()
        {
            return new PlayerBonusPropertyModifier();
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
