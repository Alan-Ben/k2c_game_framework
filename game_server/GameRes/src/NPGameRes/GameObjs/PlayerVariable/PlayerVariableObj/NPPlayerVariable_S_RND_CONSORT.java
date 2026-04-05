package NPGameRes.GameObjs.PlayerVariable.PlayerVariableObj;

import NPEnum.ENPPlayerVariableType;
import NPGameRes.GameObjs.PlayerVariable._ANPBasicPlayerVariableObj;

public class NPPlayerVariable_S_RND_CONSORT extends _ANPBasicPlayerVariableObj
{
    protected NPPlayerVariable_S_RND_CONSORT()
    {
    }

    /******************
     * 获取条件类型
     */
    @Override
    public ENPPlayerVariableType variableType()
    {
        return ENPPlayerVariableType.S_RND_CONSORT;
    }


    public static NPPlayerVariable_S_RND_CONSORT readVariable()
    {
        return new NPPlayerVariable_S_RND_CONSORT();
    }
}
