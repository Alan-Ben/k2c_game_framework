package NPGameRes.GameObjs.UnionBonusMgr;

import CommonEnum.EBonusPropertyType;
import NPGameRes.GameObjs.PlayerBonusProperty.PlayerBonusPropertyModifier;

/********************
 * 具体一个子类型下的属性加成管理对象
 * @author mj
 *
 */
public class BonusInfoNode
{
    //对应的属性加成的Id
    private long _m_lId;
    //属性加成对象
    private PlayerBonusPropertyModifier _m_pmPropertyBonus;

    public BonusInfoNode(long _id)
    {
        _m_lId = _id;

        _m_pmPropertyBonus = new PlayerBonusPropertyModifier();
    }

    /***********
     * 获取对应的属性加成数值
     */
    protected long _getPropertyBonus(EBonusPropertyType _type)
    {
        return _m_pmPropertyBonus.getPropValue(_type);
    }

    /******************
     * 增加属性的操作
     * @param _modifier
     */
    protected void _addProperty(PlayerBonusPropertyModifier _modifier)
    {
        //累加属性加成
        _m_pmPropertyBonus.addModifier(_modifier);
    }

    /******************
     * 增加属性的操作
     * @param _modifier
     */
    protected void _addProperty(PlayerBonusPropertyModifier _modifier, int _stack)
    {
        //累加属性加成
        _m_pmPropertyBonus.addModifier(_modifier, _stack);
    }

    /******************
     * 减少属性的操作
     * @param _modifier
     */
    protected void _removeProperty(PlayerBonusPropertyModifier _modifier, int _stack)
    {
        _m_pmPropertyBonus.removeModifier(_modifier, _stack);
    }

    /*****
     * 清除属性列表
     */
    protected void _clear()
    {
        //清空本地数据
        _m_pmPropertyBonus.clear();
    }

    /*****
     * 移除属性列表，并替增加新的属性列表
     */
    protected void _replace(PlayerBonusPropertyModifier _toRemoved, PlayerBonusPropertyModifier _toAdd)
    {
        _m_pmPropertyBonus.removeModifier(_toRemoved);
        _m_pmPropertyBonus.addModifier(_toAdd);
    }

    @Override
    public String toString()
    {
        return "{" +
                "\n  属性加成=\n" + _m_pmPropertyBonus +
                '}';
    }
}
