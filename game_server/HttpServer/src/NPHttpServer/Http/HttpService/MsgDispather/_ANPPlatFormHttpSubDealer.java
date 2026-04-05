package NPHttpServer.Http.HttpService.MsgDispather;

import NPCommon.ErrMain.HttpErr;
import NPCommon.Http.HttpService.server._IResponse;
import NPHttpServer.Http.HttpService.Decoder._ANPPlatFormHttpDataDecoder;
import NPHttpServer.Http.HttpService.NPPlatFormCommiter;

/**
 * @description: 后台http实际处理抽象类
 * @author: ricci
 * @date: 2023-03-24 17:27:01
 */
public abstract class _ANPPlatFormHttpSubDealer<T>
{

    /**
     * 解析并处理消息
     * @param _serial   请求序列号
     * @param _data     数据内容
     * @param _response 响应体
     */
    public void dealMsg(String _serial, String _data, _IResponse _response)
    {
        //构造消息响应体
        NPPlatFormCommiter commiter = new NPPlatFormCommiter(_serial, _response);

        T decodeObj;
        try
        {
            _ANPPlatFormHttpDataDecoder<T> decoder = getDecoder();
            if (decoder == null)
            {
                commiter.commitFail(HttpErr.PLATFORM_DECODE_DATA_ERROR);
                return;
            }

            //调用decoder解析消息内容
            decodeObj = decoder.decode(_data);
            if (decodeObj == null)
            {
                commiter.commitFail(HttpErr.PLATFORM_DECODE_DATA_ERROR);
                return;
            }

        } catch (Exception e)
        {
            commiter.commitFail(HttpErr.PLATFORM_DECODE_DATA_ERROR);
            return;
        }

        try
        {
            //处理消息
            _doDealMsg(commiter, decodeObj);
        } catch (Exception e)
        {
            commiter.commitFail(HttpErr.PLATFORM_DEAL_MSG_ERROR);
        }
    }


    /**
     * 次协议号
     * @return int
     */
    public abstract int subOrder();

    /**
     * 消息解析器
     * @return _ANPPlatFormHttpDataDecoder
     */
    public abstract _ANPPlatFormHttpDataDecoder<T> getDecoder();

    /**
     * 交给子类处理消息内容
     * @param _commiter  消息响应体
     * @param _decodeObj 消息对象
     */
    protected abstract void _doDealMsg(NPPlatFormCommiter _commiter, T _decodeObj);

}
