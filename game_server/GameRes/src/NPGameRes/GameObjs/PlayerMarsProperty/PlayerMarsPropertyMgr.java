package NPGameRes.GameObjs.PlayerMarsProperty;

import Common.Common_LongList;
import Common.MarsEnum.EMarsPropertyType;
import NPCommon.Property._ATNPBasicPropertyMgr;

/****************
 * 总的属性管理对象
 **/
public class PlayerMarsPropertyMgr extends _ATNPBasicPropertyMgr<EMarsPropertyType, PlayerMarsPropertyModifier, PlayerMarsPropertyContainer>
{
    public PlayerMarsPropertyMgr()
    {
        super(EMarsPropertyType.class);
    }

    /**
     * 构造属性列表
     */
    public Common_LongList makeAttrList()
    {
        Common_LongList attrList = new Common_LongList();
        for (EMarsPropertyType attrType : EMarsPropertyType.EMarsPropertyType_Values)
        {
            attrList.addValueList(getValue(attrType));
        }
        return attrList;
    }
}