package NPGameRes.GameObjs.PlayerBonusProperty;

import CommonEnum.EBonusPropertyType;
import NPCommon.Property._ATNPBasicPropertyMgr;

/****************
 * 总的属性管理对象
 **/
public class PlayerBonusPropertyMgr extends _ATNPBasicPropertyMgr<EBonusPropertyType, PlayerBonusPropertyModifier, PlayerBonusPropertyContainer>
{
    public PlayerBonusPropertyMgr()
    {
        super(EBonusPropertyType.class);
    }
}