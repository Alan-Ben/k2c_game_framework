package NPCommon.Util;

import java.util.UUID;

public class Random
{

    /**
     * 取得指定范围内整數
     * @param n 范围值, 取值[0,n)之间的随机数, n可取负数
     * @return
     */
    public static int nextInt(int n)
    {
        if (n == 0)
        {
            return 0;
        }
        double randf = Math.random();//[0,1)
        long max = (long) (randf * 2d * n);
        int res = (int) (max % n);
        return res;
    }

    /**
     * 取得指定范围内整數(自定义随机数生成器)
     * @param n       范围值, 取值[0,n)之间的随机数, n可取负数
     * @param _random 随机数生成器
     * @return
     */
    public static int nextInt(int n, java.util.Random _random)
    {
        if (n == 0)
        {
            return 0;
        }
        double randf = _random.nextDouble();//[0,1)
        long max = (long) (randf * 2d * n);
        return (int) (max % n);
    }

    // 取得整數+偏移

    /**
     * 取得指定范围内整數, 并加上 offset
     * @param n
     * @param offset
     * @return
     */
    public static int nextInt(int n, int offset)
    {
        if (n == 0)
        {
            return offset;
        }
        int res = Math.abs(UUID.randomUUID().hashCode()) % n;
        return res + offset;
    }
    // 取得整數+偏移

    /**
     * 取得指定范围内整數
     * @param min
     * @param max
     * @return
     */
    public static int nextIntBetween(int min, int max)
    {
        return nextInt(max - min, min);
    }

    /**
     * 真假随机
     * @return
     */
    public static boolean nextBoolean()
    {
        return (nextInt(2) == 1);
    }

    /**
     * 256范围内随机取值
     * @return
     */
    public static byte nextByte()
    {
        return (byte) nextInt(256);
    }

    /**
     * 64位长整数随机[]
     * @param n 范围值, 取值[0,n)之间的随机数,n只能为正数
     * @return
     */
    public static long nextLong(long n)
    {
        if (n == 0)
        {
            return 0;
        }
        long head = nextInt(Integer.MAX_VALUE);
        long l = nextInt(Integer.MAX_VALUE);

        long dividend = ((head << 32) + l);

        long remain = dividend - (dividend / n) * n;

        if (n < 0)
        {
            return 0 - remain;
        } else
        {
            return remain;
        }
    }

    public static boolean isTrue(float ratio)
    {
        float fl = (float) Math.random();
        if (fl <= ratio)
            return true;
        return false;
    }

    /************************
     * 获取一个随机的战斗随机数种子
     * @return
     */
    public static long popBattleRandomSeed()
    {
        return nextLong(9999999999L);
    }
}
