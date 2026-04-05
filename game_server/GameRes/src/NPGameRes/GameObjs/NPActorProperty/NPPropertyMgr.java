package NPGameRes.GameObjs.NPActorProperty;

import NPCommon.Enum.NPCommonEnum.ENPPropertyType;
import NPCommon.Property._ATNPBasicPropertyMgr;

/****************
 * 总的属性管理对象
 **/
public class NPPropertyMgr extends _ATNPBasicPropertyMgr<ENPPropertyType, NPPropertyModifier, NPPropertyContainer>
{
    public NPPropertyMgr()
    {
        super(ENPPropertyType.class);
    }
}