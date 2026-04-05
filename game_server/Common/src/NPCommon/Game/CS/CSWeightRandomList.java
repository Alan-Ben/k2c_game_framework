package NPCommon.Game.CS;

import NPCommon.Log.CommLog;
import NPCommon.RefData._IParseFromStringable;

import java.util.ArrayList;
import java.util.List;

/**
 * 权重随机对象
 */
public class CSWeightRandomList implements _IParseFromStringable
{
    private List<CSWeightItem> _m_items;
    private int _m_totalWeight;

    public CSWeightRandomList()
    {
        this._m_items = new ArrayList<>();
        this._m_totalWeight = 0;
    }

    /**
     * 从字符串解析权重配置
     * 格式: "数据1id:权重1;数据2id:权重2"
     */
    public boolean parseFromString(String _config)
    {
        _m_items.clear();
        _m_totalWeight = 0;

        if (_config == null || _config.trim().isEmpty())
        {
            return true;
        }

        String[] _pairs = _config.split(";");
        for (String _pair : _pairs)
        {
            if (_pair.trim().isEmpty())
            {
                continue;
            }

            String[] _parts = _pair.split(":");
            if (_parts.length != 2)
            {
                return false;
            }

            try
            {
                long _dataId = Long.parseLong(_parts[0].trim());
                int _weight = Integer.parseInt(_parts[1].trim());

                addItem(_dataId, _weight);
            } catch (NumberFormatException e)
            {
                return false;
            }
        }

        return true;
    }

    /**
     * 添加权重项
     */
    public void addItem(long _dataId, int _weight)
    {
        if (_weight <= 0)
        {
            CommLog.warn("WeightRandomList addItem Invalid weight:{}", _weight, new Exception());
            return;
        }

        _m_items.add(new CSWeightItem(_dataId, _weight));
        _m_totalWeight += _weight;
    }

    /**
     * 使用权重随机选择一个数据ID
     */
    public Long randomSelect(CSSyncRandom _random)
    {
        if (_m_items.isEmpty())
            return null;

        int _randomValue = _random.nextInt(_m_totalWeight);
        int _currentWeight = 0;

        for (CSWeightItem _item : _m_items)
        {
            _currentWeight += _item.getWeight();
            if (_randomValue < _currentWeight)
            {
                return _item.getDataId();
            }
        }

        // 理论上不应该到达这里
        return _m_items.get(_m_items.size() - 1).getDataId();
    }

    /**
     * 获取所有权重项
     */
    public List<CSWeightItem> getItems()
    {
        return new ArrayList<>(_m_items);
    }

    /**
     * 获取总权重
     */
    public int getTotalWeight()
    {
        return _m_totalWeight;
    }

    /**
     * 检查是否为空
     */
    public boolean isEmpty()
    {
        return _m_items.isEmpty();
    }

    /**
     * 清空所有权重项
     */
    public void clear()
    {
        _m_items.clear();
        _m_totalWeight = 0;
    }
}