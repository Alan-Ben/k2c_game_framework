package NPHttpServer.Http.HttpService.AutoDecoder;

import NPCommon.Log.CommLog;
import NPHttpServer.Http.HttpService.Decoder._ANPPlatFormHttpDataDecoder;

/**
 * @description: 基于注解的通用JSON解码器
 * 特性：
 * 1. 泛型支持，适用于任何带JsonField注解的实体类
 * 2. 自动字段映射，无需手写解析代码
 * 3. 详细的错误处理和调试日志
 * 4. 高性能字段缓存机制
 * 5. 完全向后兼容现有架构
 * <p>
 * 使用方式：
 * 1. 在Entity类字段上添加@JsonField注解
 * 2. 创建解码器实例：new AutoJsonDecoder<>(YourEntity.class)
 * 3. 在SubDealer中使用该解码器实例
 * @author: claude
 * @date: 2025-08-06
 */
public class AutoJsonDecoder<T> extends _ANPPlatFormHttpDataDecoder<T>
{

    private final Class<T> targetClass;
    private final String decoderName;

    /**
     * 构造函数
     * @param targetClass 目标实体类，必须包含JsonField注解
     */
    public AutoJsonDecoder(Class<T> targetClass)
    {
        if (targetClass == null)
        {
            throw new IllegalArgumentException("目标类型不能为null");
        }

        this.targetClass = targetClass;
        this.decoderName = "AutoJsonDecoder<" + targetClass.getSimpleName() + ">";

        // 验证目标类是否有无参构造函数
        try
        {
            targetClass.getDeclaredConstructor();
        } catch (NoSuchMethodException e)
        {
            throw new IllegalArgumentException("目标类必须有无参构造函数: " + targetClass.getName());
        }

        CommLog.debug("[{}] 初始化完成", decoderName);
    }

    @Override
    public T decode(String data)
    {
        if (data == null || data.trim().isEmpty())
        {
            logError("输入数据为空或null");
            return null;
        }

        logDebug("开始解析JSON数据，长度: " + data.length());
        // 详细的JSON内容调试信息
        CommLog.debug("[{}] JSON内容: {}", decoderName, data);

        try
        {
            T result = JsonMappingUtil.fromJson(data, targetClass);

            if (result != null)
            {
                logDebug("JSON解析成功");
                return result;
            } else
            {
                logError("JSON映射返回null，可能是数据格式不符合Entity定义");
                return null;
            }

        } catch (Exception e)
        {
            logError("JSON解析过程中发生异常: " + e.getMessage(), e);
            return null;
        }
    }

    /**
     * 获取目标类型
     * @return 目标类型
     */
    public Class<T> getTargetClass()
    {
        return targetClass;
    }

    /**
     * 获取解码器名称
     * @return 解码器名称
     */
    public String getDecoderName()
    {
        return decoderName;
    }

    /**
     * 记录错误日志
     */
    private void logError(String message)
    {
        CommLog.error("[{}] {}", decoderName, message);
    }

    /**
     * 记录错误日志（带异常）
     */
    private void logError(String message, Throwable e)
    {
        CommLog.error("[{}] {}", decoderName, message, e);
    }

    /**
     * 记录调试日志
     */
    private void logDebug(String message)
    {
        CommLog.debug("[{}] {}", decoderName, message);
    }

    /**
     * 记录警告日志
     */
    private void logWarn(String message)
    {
        CommLog.warn("[{}] {}", decoderName, message);
    }

    @Override
    public String toString()
    {
        return decoderName;
    }
}