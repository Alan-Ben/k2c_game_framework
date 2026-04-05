package NPGameRes.UsHotRefDataMgr;

import java.util.HashMap;
import java.util.Map;
import java.util.Set;

/**
 * 活动热更新文件数据容器
 *
 * 包含文件中所有配表的数据
 */
public class ActivityHotRefTableGroup
{
    // 表名到表数据的映射
    private Map<String, ActivityHotRefTableInfo> _m_tables;

    public ActivityHotRefTableGroup()
    {
        _m_tables = new HashMap<>();
    }

    /**
     * 添加表数据
     */
    public void addTable(ActivityHotRefTableInfo _tableData)
    {
        _m_tables.put(_tableData.getTableName(), _tableData);
    }

    /**
     * 根据表名获取表数据
     */
    public ActivityHotRefTableInfo getTable(String _tableName)
    {
        return _m_tables.get(_tableName);
    }

    /**
     * 检查是否包含指定表
     */
    public boolean hasTable(String _tableName)
    {
        return _m_tables.containsKey(_tableName);
    }

    /**
     * 获取所有表名
     */
    public Set<String> getTableNames()
    {
        return _m_tables.keySet();
    }

    /**
     * 获取表数量
     */
    public int getTableCount()
    {
        return _m_tables.size();
    }

    /**
     * 清空所有表数据
     */
    public void clear()
    {
        _m_tables.clear();
    }

    @Override
    public String toString()
    {
        return String.format("FileData[tables=%d]", getTableCount());
    }
}