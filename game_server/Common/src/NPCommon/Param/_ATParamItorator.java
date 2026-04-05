package NPCommon.Param;

/********************
 * 可以使用本对象对NPParamMgr中的参数进行迭代处理
 * @author mj
 *
 * @param <D>
 * @param <P>
 */
public abstract class _ATParamItorator<D, P extends _ATNPParamBase<D>>
{
    /*****************
     * 逐个处理参数对象的操作
     * @param _param
     */
    public abstract void dealParam(P _param);
}