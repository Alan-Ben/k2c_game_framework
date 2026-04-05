package NPGameRes.GameObjs.PlayerBonusProperty;

import CommonEnum.EBonusPropertyType;
import NPCommon.Property._ATNPBasicPropertyModifier;

public class PlayerBonusPropertyModifier extends _ATNPBasicPropertyModifier<EBonusPropertyType, PlayerBonusPropertyModifier>
{
    public PlayerBonusPropertyModifier()
    {
        super(EBonusPropertyType.class);
    }

    /************
     * 创建一个编辑器对象
     * @return
     */
    @Override
    protected PlayerBonusPropertyModifier _createModifier()
    {
        return new PlayerBonusPropertyModifier();
    }
}