package NPGameRes.GameObjs.NPPlayerProperty;

import NPCommon.Property._ATNPBasicPropertyMgr;
import NPEnum.ENPPlayerPropertyType;

/****************
 * 总的属性管理对象
 **/
public class NPPlayerPropertyMgr extends _ATNPBasicPropertyMgr<ENPPlayerPropertyType, NPPlayerPropertyModifier, NPPlayerPropertyContainer>
{
    public NPPlayerPropertyMgr()
    {
        super(ENPPlayerPropertyType.class);
    }

}