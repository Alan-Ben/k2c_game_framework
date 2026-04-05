package NPGameRes.GameObjs.NPActorProperty;

import NPCommon.Enum.NPCommonEnum.ENPPropertyType;
import NPCommon.Property._ATNPBasicPropertyModifier;

public class NPPropertyModifier extends _ATNPBasicPropertyModifier<ENPPropertyType, NPPropertyModifier>
{
    public NPPropertyModifier()
    {
        super(ENPPropertyType.class);
    }

    /************
     * 创建一个编辑器对象
     * @return
     */
    @Override
    protected NPPropertyModifier _createModifier()
    {
        return new NPPropertyModifier();
    }

    /**************
     * 全局的读取函数
     * @param _str
     * @param _fieldName
     * @return
     */
    public static NPPropertyModifier readPropertyModifier(String _str, String _fieldName)
    {
        NPPropertyModifier modifier = new NPPropertyModifier();

        //读取
        modifier.readStr(_str, _fieldName);

        if (modifier.isEmpty())
            return null;

        return modifier;
    }
}