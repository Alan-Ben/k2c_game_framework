package NPGameRes.UsHotRefDataMgr;

import java.util.ArrayList;
import java.util.List;

/**
 * 活动热更新配表数据
 *
 * 表示单个配表的数据结构，包含字段定义和数据行
 */
public class ActivityHotRefTableInfo
{
    // 表名
    private String _m_tableName;

    // 字段名列表
    private List<String> _m_fields;

    // 数据行列表，每一行是一个字符串列表
    private List<List<String>> _m_dataRows;

    public ActivityHotRefTableInfo(String _tableName)
    {
        _m_tableName = _tableName;
        _m_fields = new ArrayList<>();
        _m_dataRows = new ArrayList<>();
    }

    /**
     * 添加字段名
     */
    public void addField(String _fieldName)
    {
        _m_fields.add(_fieldName);
    }

    /**
     * 添加数据行
     */
    public void addDataRow(List<String> _row)
    {
        _m_dataRows.add(_row);
    }

    /**
     * 获取表名
     */
    public String getTableName()
    {
        return _m_tableName;
    }

    /**
     * 获取字段列表
     */
    public List<String> getFields()
    {
        return _m_fields;
    }

    /**
     * 获取字段数量
     */
    public int getFieldCount()
    {
        return _m_fields.size();
    }

    /**
     * 获取数据行列表
     */
    public List<List<String>> getDataRows()
    {
        return _m_dataRows;
    }

    /**
     * 获取数据行数量
     */
    public int getRowCount()
    {
        return _m_dataRows.size();
    }

    /**
     * 根据行索引获取数据行
     */
    public List<String> getRow(int _index)
    {
        if (_index < 0 || _index >= _m_dataRows.size())
        {
            return null;
        }
        return _m_dataRows.get(_index);
    }

    /**
     * 获取指定行指定列的值
     *
     * @param _rowIndex 行索引
     * @param _colIndex 列索引
     * @return 字符串值或null
     */
    public String getValue(int _rowIndex, int _colIndex)
    {
        List<String> row = getRow(_rowIndex);
        if (row == null || _colIndex < 0 || _colIndex >= row.size())
        {
            return null;
        }
        return row.get(_colIndex);
    }

    /**
     * 获取指定行指定字段名的值
     *
     * @param _rowIndex 行索引
     * @param _fieldName 字段名
     * @return 字符串值或null
     */
    public String getValueByFieldName(int _rowIndex, String _fieldName)
    {
        int colIndex = _m_fields.indexOf(_fieldName);
        if (colIndex < 0)
        {
            return null;
        }
        return getValue(_rowIndex, colIndex);
    }

    /**
     * 获取指定行指定列的整数值
     */
    public int getInt(int _rowIndex, int _colIndex, int _default)
    {
        String value = getValue(_rowIndex, _colIndex);
        if (value == null || value.isEmpty())
        {
            return _default;
        }
        try
        {
            return Integer.parseInt(value);
        }
        catch (NumberFormatException e)
        {
            return _default;
        }
    }

    /**
     * 获取指定行指定字段名的整数值
     */
    public int getIntByFieldName(int _rowIndex, String _fieldName, int _default)
    {
        String value = getValueByFieldName(_rowIndex, _fieldName);
        if (value == null || value.isEmpty())
        {
            return _default;
        }
        try
        {
            return Integer.parseInt(value);
        }
        catch (NumberFormatException e)
        {
            return _default;
        }
    }

    /**
     * 获取指定行指定列的长整数值
     */
    public long getLong(int _rowIndex, int _colIndex, long _default)
    {
        String value = getValue(_rowIndex, _colIndex);
        if (value == null || value.isEmpty())
        {
            return _default;
        }
        try
        {
            return Long.parseLong(value);
        }
        catch (NumberFormatException e)
        {
            return _default;
        }
    }

    /**
     * 获取指定行指定字段名的长整数值
     */
    public long getLongByFieldName(int _rowIndex, String _fieldName, long _default)
    {
        String value = getValueByFieldName(_rowIndex, _fieldName);
        if (value == null || value.isEmpty())
        {
            return _default;
        }
        try
        {
            return Long.parseLong(value);
        }
        catch (NumberFormatException e)
        {
            return _default;
        }
    }

    /**
     * 获取指定行指定列的字符串值
     */
    public String getString(int _rowIndex, int _colIndex, String _default)
    {
        String value = getValue(_rowIndex, _colIndex);
        return value == null ? _default : value;
    }

    /**
     * 获取指定行指定字段名的字符串值
     */
    public String getStringByFieldName(int _rowIndex, String _fieldName, String _default)
    {
        String value = getValueByFieldName(_rowIndex, _fieldName);
        return value == null ? _default : value;
    }

    /**
     * 获取指定行指定字段名的布尔值
     *
     * 支持的格式：
     * - "true" / "false" (不区分大小写)
     * - 空字符串返回默认值
     */
    public boolean getBooleanByFieldName(int _rowIndex, String _fieldName, boolean _default)
    {
        String value = getValueByFieldName(_rowIndex, _fieldName);
        if (value == null || value.isEmpty())
        {
            return _default;
        }

        // 支持 "true" / "false" 字符串（不区分大小写）
        if ("true".equalsIgnoreCase(value))
        {
            return true;
        }
        if ("false".equalsIgnoreCase(value))
        {
            return false;
        }

        return _default;
    }

    @Override
    public String toString()
    {
        return String.format("Table[%s] fields=%d, rows=%d", _m_tableName, getFieldCount(), getRowCount());
    }
}