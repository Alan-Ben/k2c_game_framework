package NPGameRes.UsHotRefDataMgr;

import NPCommon.Log.CommLog;
import NPCommon.RefData.Ref.RefBase;
import NPCommon.RefData.RefContainer.RefContainerBase;
import NPGameRes.UsHotRefDataMgr.HotLoaders.*;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

/**
 * 热更配表加载管理器
 *
 * 职责：
 * 1. 管理所有表的硬编码加载器
 * 2. 从 ActivityHotRefTableInfo 加载数据到 RefContainerBase
 * 3. 支持热更新（替换现有数据）
 * 4. 统一的错误处理和日志记录
 *
 * 线程安全：单例模式，内部无状态操作，线程安全
 */
public class ActivityHotRefLoaderMgr
{
    private static ActivityHotRefLoaderMgr _g_instance = new ActivityHotRefLoaderMgr();

    public static ActivityHotRefLoaderMgr getInstance()
    {
        return _g_instance;
    }

    // 表名 → 加载器映射
    private Map<String, _IHotRefTableLoader<?>> _m_loaderMap;

    public ActivityHotRefLoaderMgr()
    {
        _m_loaderMap = new HashMap<>();

        registerLoader(new RefActivityFundStepHotLoader());
        registerLoader(new RefActivityRankRewardHotLoader());
        registerLoader(new RefActivityShopItemHotLoader());
        registerLoader(new RefActivityStepRewardHotLoader());
        registerLoader(new RefGiftPackHotLoader());
        registerLoader(new RefCrystalGiftPackHotLoader());
        registerLoader(new RefRankGiftPackHotLoader());
        registerLoader(new RefRechargeRebateStepHotLoader());
        registerLoader(new RefRewardHotLoader());
        registerLoader(new RefRewardSubHotLoader());
    }

    /**
     * 注册表加载器
     *
     * 调用时机：
     * 在服务器启动时，各活动初始化阶段调用
     *
     * @param loader 加载器实例
     */
    public void registerLoader(_IHotRefTableLoader<?> loader)
    {
        String tableName = loader.getTableName();

        if (_m_loaderMap.containsKey(tableName))
        {
            CommLog.warn("ActivityHotRefLoaderMgr.registerLoader - loader already registered and will be replaced: tableName={}",
                    tableName);
        }

        _m_loaderMap.put(tableName, loader);
        CommLog.info("ActivityHotRefLoaderMgr.registerLoader - register success: tableName={}", tableName);
    }

    /**
     * 加载单个表数据
     *
     * 执行流程：
     * 1. 查找对应的加载器
     * 2. 获取静态容器
     * 3. 遍历每一行数据，调用加载器创建对象
     * 4. 判断是热更新还是新增
     * 5. 批量添加新数据
     * 6. 重新排序和回调
     *
     * @param tableInfo 表数据
     * @return 加载成功的数据行数，失败返回-1
     */
    public <T extends RefBase> int loadTable(ActivityHotRefTableInfo tableInfo)
    {
        // 查找对应的加载器
        _IHotRefTableLoader<?> loader = _m_loaderMap.get(tableInfo.getTableName());
        if (loader == null)
        {
            CommLog.warn("ActivityHotRefLoaderMgr.loadTable - loader not found: no loader registered for table, tableName={}",
                    tableInfo.getTableName());
            return -1;
        }

        return loadTableWithLoader((_IHotRefTableLoader<T>) loader, tableInfo);
    }

    /**
     * 使用指定加载器加载表数据
     *
     * 执行逻辑：
     * 1. 获取配表的静态容器
     * 2. 遍历热更数据的每一行
     * 3. 先获取行的主键字段，判断该ID的配表对象是否已存在
     * 4. 如果存在，遍历热更数据中的字段列表，逐个调用 updateSingleField 更新
     * 5. 如果不存在，报错并跳过（不支持新增数据）
     *
     * 性能优化：
     * - 外层循环只遍历热更数据中传入的字段（O(n)）
     * - 避免方案B中每个字段都调用 contains() 的 O(n²) 复杂度
     *
     * 限制：
     * - 只支持修改现有配表数据，不支持新增
     * - 如果热更数据中包含不存在的ID，会记录错误日志并跳过
     *
     * @param loader 加载器
     * @param tableInfo 表数据
     * @return 加载成功的数据行数
     */
    private <T extends RefBase> int loadTableWithLoader(
        _IHotRefTableLoader<T> loader,
        ActivityHotRefTableInfo tableInfo)
    {
        try
        {
            // 获取静态容器
            T tempInstance = loader.getRefClass().newInstance();
            RefContainerBase<T> container = (RefContainerBase<T>) tempInstance.getStaticContainer();

            int updateCount = 0;
            List<Long> updatedIdList = new ArrayList<>();

            // 遍历每一行数据
            for (int i = 0; i < tableInfo.getRowCount(); i++)
            {
                try
                {
                    // 先获取主键字段，判断对象是否已存在
                    String primaryKeyField = loader.getPrimaryKeyFieldName();
                    long refId = tableInfo.getLongByFieldName(i, primaryKeyField, 0L);
                    if (refId == 0L)
                    {
                        CommLog.error("ActivityHotRefLoaderMgr.loadTableWithLoader - row validation failed: primary key is 0, tableName={}, row={}, primaryKeyField={}",
                                    tableInfo.getTableName(), i, primaryKeyField);
                        continue;
                    }

                    // 检查是否已存在
                    T existingRef = container.get(refId);
                    if (existingRef != null)
                    {
                        // 遍历热更数据中的字段列表，逐个更新
                        for (String fieldName : tableInfo.getFields())
                        {
                            loader.updateSingleField(existingRef, fieldName, tableInfo, i);
                        }
                        updateCount++;
                        updatedIdList.add(refId);
                    }
                    else
                    {
                        // 不支持新增数据，报错
                        CommLog.error("ActivityHotRefLoaderMgr.loadTableWithLoader - ref not found: hot update does not support adding new data, tableName={}, {}={}, row={}",
                                    tableInfo.getTableName(), primaryKeyField, refId, i);
                    }
                }
                catch (Exception e)
                {
                    CommLog.error("ActivityHotRefLoaderMgr.loadTableWithLoader - load row failed: exception occurred, tableName={}, row={}, error={}",
                               tableInfo.getTableName(), i, e.getMessage());
                }
            }

            // 数据加载完成后的处理
            container.onLoaded();

            CommLog.info("ActivityHotRefLoaderMgr.loadTableWithLoader - load success: tableName={}, updateCount={}, updatedIds={}",
                    tableInfo.getTableName(), updateCount, updatedIdList);

            return updateCount;
        }
        catch (Exception e)
        {
            CommLog.error("ActivityHotRefLoaderMgr.loadTableWithLoader - load table failed: exception occurred, tableName={}, error={}",
                    tableInfo.getTableName(), e.getMessage());
            return -1;
        }
    }

    /**
     * 批量加载配表组中的所有表
     *
     * 执行流程：
     * 1. 获取配表组数据
     * 2. 遍历所有表
     * 3. 逐个加载
     * 4. 统计结果
     *
     * @param groupInfo 配表组信息
     * @return 成功加载的表数量
     */
    public int loadGroupTables(ActivityHotRefGroupInfo groupInfo)
    {
        ActivityHotRefTableGroup tableGroup = groupInfo.getFileData();
        if (tableGroup == null)
        {
            CommLog.error("ActivityHotRefLoaderMgr.loadGroupTables - load group failed: file data is null, groupId={}",
                    groupInfo.getGroupId());
            return 0;
        }

        int successCount = 0;
        int totalRows = 0;

        // 遍历所有表
        for (String tableName : tableGroup.getTableNames())
        {
            ActivityHotRefTableInfo tableInfo = tableGroup.getTable(tableName);
            if (tableInfo == null)
                continue;

            int rowCount = loadTable(tableInfo);
            if (rowCount >= 0)
            {
                successCount++;
                totalRows += rowCount;
            }
        }

        CommLog.info("ActivityHotRefLoaderMgr.loadGroupTables - load group done: groupId={}, successTables={}/{}, totalUpdateRows={}",
                   groupInfo.getGroupId(), successCount, tableGroup.getTableCount(), totalRows);

        return successCount;
    }
}