package MGClient.ProtocolFactory;

import ALBasicProtocolPack._IALProtocolStructure;
import NPCommon.Log.CommLog;
import NPCommon.Util.CommClass;

import java.lang.reflect.Constructor;
import java.util.List;
import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;

/**
 * 回包协议工厂类 - 根据主协议号和副协议号创建协议实例
 * <p>
 * 主要功能：
 * 1. 自动扫描指定路径下的GS2GC协议类
 * 2. 根据协议号快速创建协议实例
 * 3. 支持运行时动态注册新协议
 * <p>
 * 设计特点：
 * - 使用双重映射结构：主协议号 -> 副协议号 -> 协议类
 * - 线程安全的ConcurrentHashMap实现
 * - 延迟加载机制，减少启动时间
 * <p>
 * 线程安全：使用ConcurrentHashMap保证多线程环境下的安全性
 */
public class ResponseProtocolFactory
{
    // 协议映射表：主协议号 -> 副协议号 -> 协议类
    private final Map<Byte, Map<Byte, Class<? extends _IALProtocolStructure>>> protocolMap =
            new ConcurrentHashMap<>();

    // 构造器缓存，提高创建性能
    private final Map<Class<? extends _IALProtocolStructure>, Constructor<? extends _IALProtocolStructure>> constructorCache =
            new ConcurrentHashMap<>();

    // 单例实例
    private static final ResponseProtocolFactory _g_instance = new ResponseProtocolFactory();

    // 私有构造函数
    private ResponseProtocolFactory()
    {
        // 根据需要扫描不同的协议包
        String[] protocolPackages = {
                "GS2GC",
                "NPGS2GC",
        };

        for (String packagePath : protocolPackages)
        {
            scanAndRegisterByPackage(packagePath);
        }

        CommLog.info("客户端协议工厂初始化完成: {}", getRegistrationStats());
    }

    /**
     * 获取单例实例
     * @return 工厂实例
     */
    public static ResponseProtocolFactory getInstance()
    {
        return _g_instance;
    }

    /**
     * 根据主协议号和副协议号创建协议实例
     * <p>
     * 执行流程：
     * 1. 检查协议是否已注册
     * 2. 获取或缓存协议类构造器
     * 3. 创建并返回协议实例
     * @param mainOrder 主协议号
     * @param subOrder  副协议号
     * @return 协议实例，如果未找到对应协议则返回null
     */
    public _IALProtocolStructure createProtocol(byte mainOrder, byte subOrder)
    {
        try
        {
            Map<Byte, Class<? extends _IALProtocolStructure>> subMap = protocolMap.get(mainOrder);
            if (subMap == null)
            {
                CommLog.warn("主协议号未注册: {}", mainOrder);
                return null;
            }

            Class<? extends _IALProtocolStructure> protocolClass = subMap.get(subOrder);
            if (protocolClass == null)
            {
                CommLog.warn("协议未注册: 主协议号={}, 副协议号={}", mainOrder, subOrder);
                return null;
            }

            // 获取或缓存构造器
            Constructor<? extends _IALProtocolStructure> constructor = constructorCache.get(protocolClass);
            if (constructor == null)
            {
                constructor = protocolClass.getDeclaredConstructor();
                constructor.setAccessible(true);
                constructorCache.put(protocolClass, constructor);
            }

            return constructor.newInstance();

        } catch (Exception e)
        {
            CommLog.error("创建协议实例失败: 主协议号={}, 副协议号={}, 错误={}",
                    mainOrder, subOrder, e.getMessage());
            return null;
        }
    }

    /**
     * 手动注册协议类
     * @param protocolClass 协议类
     */
    public void registerProtocol(Class<? extends _IALProtocolStructure> protocolClass)
    {
        try
        {
            _IALProtocolStructure instance = protocolClass.getDeclaredConstructor().newInstance();
            byte mainOrder = instance.getMainOrder();
            byte subOrder = instance.getSubOrder();

            protocolMap.computeIfAbsent(mainOrder, k -> new ConcurrentHashMap<>())
                    .put(subOrder, protocolClass);

//            CommLog.info("协议注册成功: {} - 主协议号={}, 副协议号={}",
//                    protocolClass.getSimpleName(), mainOrder, subOrder);

        } catch (Exception e)
        {
            CommLog.error("协议注册失败: {}, 错误={}", protocolClass.getName(), e.getMessage());
        }
    }

    /**
     * 根据包路径扫描并注册协议类
     * <p>
     * 使用示例：
     * - scanAndRegisterByPackage("GS2GC.p032_GuildOp") - 扫描指定包下的所有协议
     * - scanAndRegisterByPackage("GS2GC") - 扫描GS2GC包下的所有协议
     * @param packagePath 包路径（如"GS2GC.p032_GuildOp"）
     */
    public void scanAndRegisterByPackage(String packagePath)
    {
        try
        {
            // 使用CommClass工具类扫描指定包下实现_IALProtocolStructure接口的所有类
            List<Class<?>> classes = CommClass.getAllClassByInterface(_IALProtocolStructure.class, packagePath);

            int registeredCount = 0;
            for (Class<?> clazz : classes)
            {
                if (_IALProtocolStructure.class.isAssignableFrom(clazz))
                {
                    @SuppressWarnings("unchecked")
                    Class<? extends _IALProtocolStructure> protocolClass =
                            (Class<? extends _IALProtocolStructure>) clazz;
                    registerProtocol(protocolClass);
                    registeredCount++;
                }
            }

            CommLog.info("包扫描注册完成: {}, 注册协议数量: {}, 总主协议号: {}",
                    packagePath, registeredCount, protocolMap.size());

        } catch (Exception e)
        {
            CommLog.error("包扫描注册失败: {}, 错误={}", packagePath, e.getMessage());
        }
    }

    /**
     * 获取已注册的协议统计信息
     * @return 统计信息字符串
     */
    public String getRegistrationStats()
    {
        int totalProtocols = 0;
        for (Map<Byte, Class<? extends _IALProtocolStructure>> subMap : protocolMap.values())
        {
            totalProtocols += subMap.size();
        }

        return String.format("已注册主协议号数量: %d, 总协议数量: %d",
                protocolMap.size(), totalProtocols);
    }

    /**
     * 检查指定协议是否已注册
     * @param mainOrder 主协议号
     * @param subOrder  副协议号
     * @return 是否已注册
     */
    public boolean isRegistered(byte mainOrder, byte subOrder)
    {
        Map<Byte, Class<? extends _IALProtocolStructure>> subMap = protocolMap.get(mainOrder);
        return subMap != null && subMap.containsKey(subOrder);
    }

    /**
     * 清空所有已注册的协议（主要用于测试）
     */
    public void clearAll()
    {
        protocolMap.clear();
        constructorCache.clear();
        CommLog.info("协议工厂已清空");
    }
}