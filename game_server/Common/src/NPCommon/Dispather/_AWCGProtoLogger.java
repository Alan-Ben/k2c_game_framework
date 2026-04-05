package NPCommon.Dispather;

import ALBasicProtocolPack.BasicObj._IALProtocolReceiver;
import ALBasicProtocolPack._IALProtocolStructure;

/******
 * 协议日志输出类
 */
public abstract class _AWCGProtoLogger
{
    private static _AWCGProtoLogger _instance;

    public static _AWCGProtoLogger getLogger()
    {
        return _instance;
    }

    /***
     * 设置协议输出对象
     * @param _logger
     */
    public static void setLogger(_AWCGProtoLogger _logger)
    {
        _instance = _logger;
    }

    /****
     * 记录日志。
     * @param _receiver
     * @param _msg
     */
    public abstract void logProto(_IALProtocolReceiver _receiver, _IALProtocolStructure _msg);

}