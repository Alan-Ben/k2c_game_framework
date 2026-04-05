package NPHttpServer.Http.HttpService.AutoDecoder;

import java.util.concurrent.ConcurrentHashMap;

/**
 * @description: JSON解码器工厂
 * 提供通用的解码器实例，无需为每个Entity创建单独的Decoder类
 * <p>
 * 使用方式：
 * 1. 直接获取解码器：JsonDecoderFactory.getDecoder(YourEntity.class)
 * 2. 在SubDealer中使用：return JsonDecoderFactory.getDecoder(NPEntityPushServerList.class);
 * <p>
 * 特性：
 * - 单例缓存：每个Entity类型只创建一次解码器实例
 * - 线程安全：使用ConcurrentHashMap保证并发访问安全
 * - 类型安全：泛型确保类型匹配
 * - 零配置：无需手动创建和管理解码器
 * @author: claude
 * @date: 2025-08-06
 */
public class JsonDecoderFactory
{

    // 解码器缓存，每个类型只创建一次
    private static final ConcurrentHashMap<Class<?>, AutoJsonDecoder<?>> DECODER_CACHE =
            new ConcurrentHashMap<>();

    /**
     * 获取指定类型的JSON解码器（带缓存）
     * @param entityClass 实体类类型
     * @param <T>         泛型类型
     * @return AutoJsonDecoder实例
     */
    @SuppressWarnings("unchecked")
    public static <T> AutoJsonDecoder<T> getDecoder(Class<T> entityClass)
    {
        if (entityClass == null)
            return null;

        // 使用computeIfAbsent确保线程安全的单例创建
        AutoJsonDecoder<?> decoder = DECODER_CACHE.computeIfAbsent(entityClass,
                clazz -> new AutoJsonDecoder<>(entityClass));

        return (AutoJsonDecoder<T>) decoder;
    }

    /**
     * 获取缓存的解码器数量（用于监控）
     * @return 缓存中的解码器数量
     */
    public static int getCachedDecoderCount()
    {
        return DECODER_CACHE.size();
    }

    /**
     * 清空解码器缓存（主要用于测试）
     */
    public static void clearCache()
    {
        DECODER_CACHE.clear();
    }

    /**
     * 检查指定类型是否已缓存解码器
     * @param entityClass 实体类类型
     * @return 是否已缓存
     */
    public static boolean isDecoderCached(Class<?> entityClass)
    {
        return DECODER_CACHE.containsKey(entityClass);
    }

    // 私有构造函数，防止实例化
    private JsonDecoderFactory()
    {
        throw new UnsupportedOperationException("JsonDecoderFactory是工具类，不能实例化");
    }
}