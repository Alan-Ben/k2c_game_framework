package NPCommon.Util;

/**
 * 订单号解析器 - 解析订单号中包含的各种信息
 * <p>
 * 订单号格式：序列号-用户ID-商品类型-商品ID-时间戳(秒)-平台ID-区服ID-服务器ID
 * <p>
 * 主要功能：
 * 1. 解析订单号字符串，提取各个字段信息
 * 2. 提供便捷的字段访问方法
 * 3. 验证订单号格式的有效性
 * <p>
 * 线程安全：解析后的对象是不可变的，可安全并发访问
 */
public class OrderIdParser
{

    // 订单号分隔符
    private static final String DELIMITER = "-";

    // 订单号字段数量
    private static final int FIELD_COUNT = 7;

    // 订单号各字段
    private final long orderSerial;      // 序列号
    private final long cid;           // 用户ID
    private final long goodsId;          // 商品ID
    private final long timestamp;        // 时间戳(秒)
    private final int platformId;        // 平台ID
    private final int areaId;           // 区服ID
    private final int serverTypeId;      // 服务器ID

    // 原始订单号字符串
    private final String originalOrderId;

    /**
     * 私有构造函数 - 通过解析方法创建实例
     */
    private OrderIdParser(String orderId, long orderSerial, long cid,
                          long goodsId, long timestamp, int platformId, int areaId, int serverTypeId)
    {
        this.originalOrderId = orderId;
        this.orderSerial = orderSerial;
        this.cid = cid;
        this.goodsId = goodsId;
        this.timestamp = timestamp;
        this.platformId = platformId;
        this.areaId = areaId;
        this.serverTypeId = serverTypeId;
    }

    /**
     * 解析订单号字符串
     * @param orderId 订单号字符串
     * @return 解析结果，失败时返回null
     */
    public static OrderIdParser parse(String orderId)
    {
        if (orderId == null || orderId.trim().isEmpty())
        {
            return null;
        }

        String[] parts = orderId.split(DELIMITER);
        if (parts.length != FIELD_COUNT)
        {
            return null;
        }

        try
        {
            long orderSerial = Long.parseLong(parts[0]);
            long userId = Long.parseLong(parts[1]);
            long goodsId = Long.parseLong(parts[2]);
            long timestamp = Long.parseLong(parts[3]);
            int platformId = Integer.parseInt(parts[4]);
            int areaId = Integer.parseInt(parts[5]);
            int serverTypeId = Integer.parseInt(parts[6]);

            return new OrderIdParser(orderId, orderSerial, userId,
                    goodsId, timestamp, platformId, areaId, serverTypeId);
        } catch (NumberFormatException e)
        {
            return null;
        }
    }

    /**
     * 验证订单号格式是否有效
     * @param orderId 订单号字符串
     * @return true表示格式有效
     */
    public static boolean isValidFormat(String orderId)
    {
        return parse(orderId) != null;
    }

    /**
     * 获取订单序列号
     */
    public long getOrderSerial()
    {
        return orderSerial;
    }

    /**
     * 获取用户ID
     */
    public long getCid()
    {
        return cid;
    }

    /**
     * 获取商品ID
     */
    public long getGoodsId()
    {
        return goodsId;
    }

    /**
     * 获取时间戳(秒)
     */
    public long getTimestamp()
    {
        return timestamp;
    }

    /**
     * 获取平台ID
     */
    public int getPlatformId()
    {
        return platformId;
    }

    /**
     * 获取区服ID
     */
    public int getAreaId()
    {
        return areaId;
    }

    /**
     * 获取服务器类型ID
     */
    public int getServerTypeId()
    {
        return serverTypeId;
    }

    /**
     * 获取原始订单号字符串
     */
    public String getOriginalOrderId()
    {
        return originalOrderId;
    }

    /**
     * 格式化显示订单信息
     * @return 格式化的订单信息字符串
     */
    public String toDetailString()
    {
        return String.format("订单详情 [%s]:" +
                        "  序列号: %d" +
                        "  用户ID: %d" +
                        "  商品ID: %d" +
                        "  时间戳: %d" +
                        "  平台ID: %d" +
                        "  区服ID: %d" +
                        "  服务器ID: %d",
                originalOrderId, orderSerial, cid,
                goodsId, timestamp, platformId, areaId, serverTypeId);
    }

    @Override
    public String toString()
    {
        return originalOrderId;
    }

    @Override
    public boolean equals(Object obj)
    {
        if (this == obj) return true;
        if (obj == null || getClass() != obj.getClass()) return false;

        OrderIdParser that = (OrderIdParser) obj;
        return originalOrderId.equals(that.originalOrderId);
    }

    @Override
    public int hashCode()
    {
        return originalOrderId.hashCode();
    }
}