package NPGameRes.InitDealer;

/**************
 * 初始化处理类
 * @author Administrator
 *
 */
public abstract class _ABasicInitDealer
{
    /****************
     * 返回优先级，默认为0最低
     *
     * @author alzq.z
     * @time 2021年3月23日 下午11:39:58
     */
    public int getPriority()
    {
        return 0;
    }

    public abstract void dealInit();
}
