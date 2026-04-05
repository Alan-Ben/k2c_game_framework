package NPCommon.DB;

import NPCommon.Enum.NPCommonEnum;

/***********
 * 用于调用BM的时候，可以根据不同情况选择不同DB的接口对象，方便一个进程可以设置多个服务器的处理
 */
public interface _ISelectDBInterface {
    /**********
     * 允许服务器对数据进行转换
     * @param _srcTag
     * @return
     */
    NPCommonEnum.EDBTag switchDBTag(NPCommonEnum.EDBTag _srcTag);
}
