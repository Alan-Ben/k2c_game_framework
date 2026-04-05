package NPGameRes.GameObjs.PlayerVariable.PlayerVariableObj;

import CommonEnum.ESpecAttrType;
import NPCommon.CommonObj.NPStringReader;
import NPCommon.Log.CommLog;
import NPEnum.ENPPlayerVariableType;
import NPGameRes.GameObjs.PlayerVariable._ANPBasicPlayerVariableObj;

public class NPPlayerVariable_S_RND_ATTR_HERO extends _ANPBasicPlayerVariableObj
{
    //大臣特长属性
    private ESpecAttrType _m_attrType = null;

    public ESpecAttrType attrType()
    {
        return _m_attrType;
    }

    /******************
     * 获取条件类型
     */
    @Override
    public ENPPlayerVariableType variableType()
    {
        return ENPPlayerVariableType.S_RND_ATTR_HERO;
    }

    public static NPPlayerVariable_S_RND_ATTR_HERO readVariable(NPStringReader _reader)
    {
        NPPlayerVariable_S_RND_ATTR_HERO variableObj = new NPPlayerVariable_S_RND_ATTR_HERO();
        try
        {
            //解析字符串
            String rawAttrType = _reader.readItem('@');
            //逐个判断
            if (null != rawAttrType)
            {
                variableObj._m_attrType = ESpecAttrType.valueOf(rawAttrType);
            }
            return variableObj;
        } catch (Exception e)
        {
            CommLog.error("高级公式配置错误 - S_RND_ATTR_HERO S_RND_ATTR_HERO@ESpecAttrType Str:{} Exception:{}", _reader.getSrcString(), e);
            return null;
        }
    }
}
