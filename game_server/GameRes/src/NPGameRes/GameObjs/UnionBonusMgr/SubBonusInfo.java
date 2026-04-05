package NPGameRes.GameObjs.UnionBonusMgr;

import ALServerLog.ALServerLog;
import CommonEnum.EBonusFilterType;
import CommonEnum.EBonusPropertyType;
import NPGameRes.GameObjs.PlayerBonusProperty.PlayerBonusPropertyModifier;

import java.util.Hashtable;

/********************
 * 根据大类型，可做具体细分某个子类型下的加成信息
 * 格式：大类型:子类型:加成信息:数值
 * @author mj
 *
 */
public class SubBonusInfo
{
    //对应的某个筛选属性的类型
    private EBonusFilterType _m_eBonusFilterType;

    //根据不同类型的加成对象
    private Hashtable<Long, BonusInfoNode> _m_htBonusNodeTable;

    public SubBonusInfo(EBonusFilterType _filterType)
    {
        _m_eBonusFilterType = _filterType;

        _m_htBonusNodeTable = new Hashtable<Long, BonusInfoNode>();
    }

    public EBonusFilterType getBonusFilterType() {return _m_eBonusFilterType;}

    /***********
     * 获取对应的属性加成汇总数值
     */
    protected long _getTotalPropertyBonus(EBonusPropertyType _type, long _id)
    {
        long totalV = 0;

        //获取数据对象
        BonusInfoNode node = _m_htBonusNodeTable.get(_id);
        //有数据则累加
        if(null != node)
            totalV += node._getPropertyBonus(_type);

        return totalV;
    }

    /*******************
     * 所有函数只提供内部调用
     * 在Mgr层增加锁保证安全
      */


    /******************
     * 增加属性的操作
     * @param _modifier
     */
    protected void _addProperty(long _id, PlayerBonusPropertyModifier _modifier, int _stack)
    {
        //获取对应的加成对象
        BonusInfoNode node = _ensureIdBonus(_id);
        if (null == node)
        {
            ALServerLog.Error("SubBonusInfo::_addProperty, node is null, id=" + _id);
            return;
        }

        //累加数据
        node._addProperty(_modifier, _stack);
    }

    /******************
     * 减少属性的操作
     * @param _modifier
     */
    protected void _removeProperty(long _id, PlayerBonusPropertyModifier _modifier, int _stack)
    {
        //获取对应的加成对象
        BonusInfoNode node = _ensureIdBonus(_id);
        if (null == node)
        {
            ALServerLog.Error("SubBonusInfo::_addProperty, node is null, id=" + _id);
            return;
        }

        //累加数据
        node._removeProperty(_modifier, _stack);
    }

    /*****
     * 移除属性列表，并替增加新的属性列表
     */
    protected void _replace(long _id, PlayerBonusPropertyModifier _toRemoved, PlayerBonusPropertyModifier _toAdd)
    {
        //获取对应的加成对象
        BonusInfoNode node = _ensureIdBonus(_id);
        if(null == node)
        {
            ALServerLog.Error("SubBonusInfo::_addProperty, node is null, id=" + _id);
            return;
        }

        //累加数据
        node._replace(_toRemoved, _toAdd);
    }

    /***********
     * 清除所有数据
     */
    protected void _clear()
    {
        for(BonusInfoNode node : _m_htBonusNodeTable.values())
        {
            node._clear();
        }
        _m_htBonusNodeTable.clear();
    }

    /***********
     * 根据Id检查对应的加成对象
     * @param _id
     * @return
     */
    protected BonusInfoNode _ensureIdBonus(long _id)
    {
        BonusInfoNode node = _m_htBonusNodeTable.get(_id);

        if(null == node) {
            node = new BonusInfoNode(_id);

            //添加到集合中
            _m_htBonusNodeTable.put(_id, node);
        }

        return node;
    }

    /***********
     * 根据Id获取对应的加成对象（只获取，不创建）
     * @param _id
     * @return
     */
    protected BonusInfoNode _getBonusNodeById(long _id)
    {
        return _m_htBonusNodeTable.get(_id);
    }


    @Override
    public String toString()
    {
        return "{" +
                "\n  属性加成=\n" + _m_eBonusFilterType +
                '}';
    }
}
