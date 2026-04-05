package NPGameRes.UsHotRefDataMgr;

import NPCommon.RefData.Ref.RefBase;

/**
 * 热更配表加载器接口
 *
 * 设计目标：
 * 1. 为每个需要热更的配表提供统一的加载规范
 * 2. 通过硬编码实现高性能数据解析
 * 3. 确保类型安全和编译期检查
 *
 * 使用方式：
 * 每个需要支持热更的配表都需要实现此接口，提供硬编码的数据加载逻辑
 *
 * @param <T> RefBase子类类型
 */
public interface _IHotRefTableLoader<T extends RefBase>
{
    /**
     * 获取支持的表名
     *
     * 注意：
     * - 表名必须与JSON文件中的表名完全一致（区分大小写）
     * - 表名与 @RefTable(tableName = "xxx") 注解中的名称保持一致
     *
     * @return 表名
     */
    String getTableName();

    /**
     * 获取表的主键字段名
     * <p>
     * 用途：
     * - 用于在热更数据中查找对应ID的配表对象
     * - 大部分配表使用 "id" 作为主键，少数表可能使用其他字段
     * @return 主键字段名
     */
    String getPrimaryKeyFieldName();

    /**
     * 更新单个字段
     *
     * 核心方法：硬编码解析逻辑，直接修改现有对象的单个字段，避免反射
     *
     * 设计目的：
     * - 热更数据可能不完整，只包含需要修改的字段
     * - 外层遍历热更数据中的字段列表，逐个调用此方法更新
     * - 性能优化：避免方案B中每个字段都调用 contains() 的 O(n²) 复杂度
     *
     * 实现要求：
     * 1. 使用 if-else 链判断字段名，第一次匹配后立即返回 true
     * 2. 使用 tableInfo.getXXXByFieldName() 方法获取字段值并赋值
     * 3. 对于复杂类型字段，使用 ParseUtil 或对象的 parseFromString() 方法
     * 4. 不支持的字段返回 false
     *
     * 性能优势：
     * - 外层循环只遍历传入的字段（O(n)），而非所有可能的字段
     * - 硬编码直接赋值，无反射开销
     * - 编译期类型检查，运行时无类型转换
     *
     * @param ref 要更新的RefBase对象
     * @param fieldName 字段名
     * @param tableInfo 表格数据
     * @param rowIndex 行索引（从0开始）
     * @return true=字段已处理, false=不支持该字段
     */
    boolean updateSingleField(T ref, String fieldName, ActivityHotRefTableInfo tableInfo, int rowIndex);

    /**
     * 获取对应的RefBase类
     *
     * 用途：
     * - 获取静态容器 (getStaticContainer)
     * - 创建临时实例
     *
     * @return RefBase类对象
     */
    Class<T> getRefClass();
}