package PayCenter;

/**
 * EPayServerAsynEnum - PayServer异步操作枚举
 * 
 * 主要功能：
 * 1. 定义PayServer支持的异步操作类型
 * 2. 用于系统启动时确定异步操作数量
 * 3. 为异步任务管理提供类型标识
 * 
 * 设计特点：
 * - 枚举形式管理异步操作类型
 * - 便于扩展新的异步操作
 * - 与系统启动参数配合使用
 */
public enum EPayServerAsynEnum
{
    PAY_DB,

}