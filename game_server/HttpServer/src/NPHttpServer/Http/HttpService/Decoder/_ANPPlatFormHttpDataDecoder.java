package NPHttpServer.Http.HttpService.Decoder;

/**
 * @description: 后台发送的data数据解析器
 * @author: ricci
 * @date: 2023-03-24 17:25:15
 */
public abstract class _ANPPlatFormHttpDataDecoder<T>
{
    public abstract T decode(String _data);
}
