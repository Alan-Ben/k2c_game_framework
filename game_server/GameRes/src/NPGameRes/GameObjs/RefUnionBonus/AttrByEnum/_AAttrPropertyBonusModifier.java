package NPGameRes.GameObjs.RefUnionBonus.AttrByEnum;

import NPCommon.Util.CommonFunc;
import NPGameRes.GameObjs.PlayerBonusProperty.PlayerBonusPropertyModifier;
import NPGameRes.GameObjs.RefUnionBonus._IBonusReader;

public abstract class _AAttrPropertyBonusModifier implements _IBonusReader
{
    //属性加成对象
    private final PlayerBonusPropertyModifier _m_pbpModifier;

    public _AAttrPropertyBonusModifier()
    {
        _m_pbpModifier = new PlayerBonusPropertyModifier();
    }

    //endregion

    @Override
    public boolean parseFromString(String sValue)
    {
        if (sValue == null || sValue.isEmpty())
            return true;

        return readStr(sValue);
    }

    /**
     * 读取字符串初始化本对象数据
     * 加成类型枚举|属性枚举:数量;属性枚举:数量
     * @param _str 配置字符串
     */
    private boolean readStr(String _str)
    {
        String[] stringList = CommonFunc.charSplit(_str, '|');
        if (stringList.length != 2)
            return false;

        //读取加成类型
        String typeStr = stringList[0];

        //通过存储对象的readStr接口添加属性
        _readStr(typeStr);

        //读取加成信息
        _m_pbpModifier.readStr(stringList[1], "");

        return true;
    }

    /*********
     * 获取实际加成数据
     * @return
     */
    @Override
    public PlayerBonusPropertyModifier getBonusPropertyModifier()
    {
        return _m_pbpModifier;
    }

    /***************
     * 根据筛选出来的数据进行读取，如果无二级筛选则第一个字符串为空
     * @param _subIdInfo
     */
    protected abstract void _readStr(String _subIdInfo);
}