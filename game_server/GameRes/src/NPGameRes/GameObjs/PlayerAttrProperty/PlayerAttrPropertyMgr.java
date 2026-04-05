package NPGameRes.GameObjs.PlayerAttrProperty;

import Common.Common_LongList;
import CommonEnum.EBasicAttrType;
import NPCommon.Property._ATNPBasicPropertyMgr;

/****************
 * 总的属性管理对象
 **/
public class PlayerAttrPropertyMgr extends _ATNPBasicPropertyMgr<EBasicAttrType, PlayerAttrPropertyModifier, PlayerAttrPropertyContainer>
{
    public PlayerAttrPropertyMgr()
    {
        super(EBasicAttrType.class);
    }

    /**
     * 构造属性列表
     */
    public Common_LongList makeAttrList()
    {
        Common_LongList attrList = new Common_LongList();
        for (EBasicAttrType attrType : EBasicAttrType.EBasicAttrType_Values)
        {
            attrList.addValueList(getValue(attrType));
        }
        return attrList;
    }
}