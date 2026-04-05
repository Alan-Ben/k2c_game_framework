package NPGameRes.GameObjs.NPPlayerProperty;

import NPCommon.Property._ATNPBasicPropertyModifier;
import NPEnum.ENPPlayerPropertyType;

public class NPPlayerPropertyModifier extends _ATNPBasicPropertyModifier<ENPPlayerPropertyType, NPPlayerPropertyModifier>
{
    public NPPlayerPropertyModifier()
    {
        super(ENPPlayerPropertyType.class);
    }

    /************
     * 创建一个编辑器对象
     * @return
     */
    @Override
    protected NPPlayerPropertyModifier _createModifier()
    {
        return new NPPlayerPropertyModifier();
    }
}