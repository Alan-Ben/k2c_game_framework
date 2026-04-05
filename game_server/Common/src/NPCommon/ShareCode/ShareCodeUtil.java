package NPCommon.ShareCode;

import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.ResultOne;
import NPCommon.Util.Random;

/**
 * 分享码工具类
 * 支持传入: 类型值(8位 0-255) + 序列号(48位 2^48-1)
 * 分享码格式 16位Base32字符组成 前4位为校验码 后12位为载荷(新鲜值+类型值+序列号)
 * https://blog.csdn.net/m0_62943934/article/details/137241204
 */
public class ShareCodeUtil
{
    /**
     * fresh值的偏移位数
     */
    private final static int FRESH_BIT_OFFSET = 56;

    /**
     * 类型值的偏移位数
     */
    private final static int TYPE_BIT_OFFSET = 48;

    /**
     * fresh值的掩码，4位
     */
    private final static int FRESH_MASK = 0xF;

    /**
     * 类型值的掩码，8位
     */
    private final static int TYPE_MASK = 0xFF;

    /**
     * 序列号掩码，48位
     */
    private final static long SERIAL_NUM_MASK = 0xFFFFFFFFFFFFL;

    /**
     * 载荷的掩码，60位
     */
    private final static long PAYLOAD_MASK = 0xFFFFFFFFFFFFFFFL;

    /**
     * 验证码的掩码，20位
     */
    private final static int CHECK_CODE_MASK = 0b11111111111111111111;

    /**
     * 优惠券兑换码模板
     */
    private final static String SHARE_CODE_PATTERN = "^[23456789ABCDEFGHJKLMNPQRSTUVWXYZ]{16}$";

    /**
     * 序列号加权运算的秘钥表
     */
    private final static int[][] PRIME_TABLE = {
            {8978, 7053, 564, 6173, 9593, 528, 3467, 5245, 7087, 1332, 3405, 4797, 9043, 2691, 6701},
            {4710, 4707, 1312, 9647, 5766, 3179, 9151, 9099, 9730, 5428, 1145, 5334, 3118, 3991, 6612},
            {4707, 453, 6543, 1417, 5149, 9296, 8945, 1495, 8900, 6281, 753, 6363, 58, 2756, 1813},
            {324, 1645, 2040, 8600, 4182, 3851, 8449, 2745, 4475, 3745, 3147, 3781, 8442, 9494, 1431},
            {3947, 8025, 9369, 6405, 3871, 2956, 2652, 4757, 713, 9969, 2492, 3139, 6843, 1007, 3522},
            {4673, 9040, 9090, 3022, 6022, 1838, 2438, 3858, 3187, 6105, 5868, 8251, 6267, 476, 908},
            {4080, 8373, 269, 8602, 1979, 3584, 2151, 6277, 1952, 4715, 600, 4487, 1767, 9033, 4497},
            {3412, 5375, 4112, 8, 6306, 3148, 8895, 1213, 7936, 5009, 2907, 4875, 6617, 7629, 6622},
            {2423, 3242, 803, 7969, 4240, 2462, 6152, 4692, 6627, 6119, 9931, 4083, 7724, 4268, 1651},
            {4159, 7714, 27, 8228, 3239, 2810, 7904, 7943, 4063, 7515, 2598, 1085, 387, 7717, 4508},
            {9510, 6035, 248, 1964, 7187, 1271, 7660, 1218, 8909, 5311, 8719, 7403, 5388, 1704, 2353},
            {4102, 889, 3864, 1349, 9212, 1911, 5608, 2991, 5820, 9796, 6090, 4048, 9134, 8221, 1911},
            {799, 8128, 7810, 3034, 470, 389, 908, 7880, 1081, 2277, 7163, 9096, 7654, 7095, 8130},
            {5457, 7645, 874, 5367, 7385, 5783, 661, 434, 7938, 7229, 3224, 2417, 5731, 2346, 5284},
            {6644, 9079, 2285, 9117, 5945, 2773, 5185, 7985, 4710, 3424, 1367, 9338, 6014, 6323, 4199},
            {1877, 3945, 7580, 3235, 7111, 7004, 8692, 8328, 1783, 1476, 9766, 3991, 1976, 3152, 5948}
    };

    /**
     * 异或密钥表，用于最后的数据混淆
     */
    private final static long[] XOR_TABLE = {
            2921504606846976L, 6192552361925523L, 6912720369127203L, 3178621931786219L,
            6992719969927199L, 6912694369126943L, 3128620931286209L, 8222734982227349L,
            6912706369127063L, 6912698769126987L, 8220093982200939L, 6192573961925739L,
            3128656331286563L, 3178642731786427L, 6912707769127077L, 1186500111865001L,
            8221676382216763L, 6192566361925663L, 6912711369127113L, 3928211939282119L,
            3128647931286479L, 6992723369927233L, 9025166190251661L, 6912712169127121L,
            6992732169927321L, 3928217939282179L, 1186488111864881L, 6912703169127031L,
            6912722169127221L, 6192552361925523L, 6912694369126943L, 6992736369927363L,
    };

    /**
     * 生成兑换码
     * @param serialNum 递增序列号
     * @return 兑换码
     */
    public static String generateCode(int type, long serialNum)
    {
        // 0.校验序列号和类型是否在范围内
        if ((serialNum & SERIAL_NUM_MASK) != serialNum || (type & TYPE_MASK) != type)
            return null;
        // 1.计算新鲜值
        long fresh = Random.nextInt(16) & FRESH_MASK;
        // 2.拼接payload，fresh(4位) + type(8位) + serialNum(48位)
        long freshWithType = fresh << 8 | type;
        long payload = freshWithType << TYPE_BIT_OFFSET | serialNum;
        // 3.计算验证码
        long checkCode = calcCheckCode(payload, (int) fresh);
        // 4.payload做大质数异或运算，混淆数据
        payload ^= XOR_TABLE[(int) (checkCode & 0b11111)];
        // 6.转码
        return Base32.encode(checkCode, 20) + Base32.encode(payload, 60);
    }

    /**
     * 计算校验码
     * @param payload
     * @param fresh
     * @return
     */
    private static long calcCheckCode(long payload, int fresh)
    {
        // 1.获取码表
        int[] table = PRIME_TABLE[fresh];
        // 2.生成校验码，payload每4位乘加权数，求和，取最后13位结果
        long sum = 0;
        int index = 0;
        while (payload > 0)
        {
            sum += (payload & 0xf) * table[index++];
            payload >>>= 4;
        }
        return sum & CHECK_CODE_MASK;
    }

    /**
     * 计算加密前数据
     * @param code
     * @return
     */
    public static ResultOne<ShareCodeData> parseCode(String code)
    {
        // 0.校验兑换码格式
        if (code == null || !code.matches(SHARE_CODE_PATTERN))
            return ResultOne.failed(CommErr.PARAM_ERROR);
        // 1.Base32解码
        // 计算checkCode的长度 因为checkCode是20位/5，所以前4位是checkCode
        long rawCheckCode = Base32.decode(code.substring(0, 4));
        long rawPayload = Base32.decode(code.substring(4));
        // 2.获取60位，payload
        long payload = rawPayload & PAYLOAD_MASK;
        // 3.获取校验码
        long checkCode = rawCheckCode & CHECK_CODE_MASK;
        // 4.载荷异或大质数，解析出原来的payload
        payload ^= XOR_TABLE[(int) (checkCode & 0b11111)];
        // 5.获取高4位，fresh
        int fresh = (int) (payload >>> FRESH_BIT_OFFSET & FRESH_MASK);
        // 6.验证格式：
        if (calcCheckCode(payload, fresh) != checkCode)
            return ResultOne.failed(CommErr.PARAM_ERROR);
        // 7.计算类型和序列号
        int type = (int) (payload >>> TYPE_BIT_OFFSET & TYPE_MASK);
        long serial = payload & SERIAL_NUM_MASK;
        // 8.返回结果
        return ResultOne.succ(new ShareCodeData(type, serial));
    }

}