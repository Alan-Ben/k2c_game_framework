package NPUSServer.NPUSUserMgr.CommonEvent;

import ALBasicProtocolPack._IALProtocolStructure;
import Common.EventObj.CommonEvent_DoneInfo;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.ResultOne;
import NPCommon.Log.CommLog;
import NPGameRes.Refs.CommonEvent._ARefCommonEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.USLog;

import java.nio.ByteBuffer;

/**
 * 通用事件处理器基类（带额外处理信息基类，需要传入事件处理过程数据）
 */
public abstract class _ACommonEventDealerWithExtra<T extends _ARefCommonEvent, E extends _IALProtocolStructure> extends _ACommonEventDealer<T>
{
    /**
     * 创建事件处理过程数据实例
     * @return 事件处理过程数据实例
     */
    public abstract E createDealInfoInstance();

    /**
     * 解析事件处理过程数据
     * @param _dealInfo 事件处理过程数据的字节流
     * @return 事件处理过程数据实例
     */
    public E readDealInfo(byte[] _dealInfo){
        //创建事件处理过程数据实例
        E dealInfo = createDealInfoInstance();
        if (dealInfo == null)
        {
            CommLog.error("CommonEventDealerMgr readDealInfo createDealInfoInstance failed, eventType:{}", eventType());
            return null;
        }

        //尝试解析事件处理过程数据
        try{
            dealInfo.readPackage(ByteBuffer.wrap(_dealInfo));
        }catch (Exception e){
            CommLog.error("CommonEventDealerMgr readDealInfo readPackage has exception", e);
            return null;
        }

        return dealInfo;
    }

    @Override
    public ResultOne<CommonEvent_DoneInfo> _dealInfo(NPUSUserData _userData, T _eventRef, byte[] _dealInfo, NPPlayerContext _context)
    {
        //解析事件处理过程数据
        E dealInfo = readDealInfo(_dealInfo);
        if (dealInfo == null)
        {
            USLog.error(_userData.getUSServer(), "CommonEventDealerMgr _dealInfo readDealInfo failed eventRefId:{} ", _eventRef.getCommonEventRefId());
            return ResultOne.failed(CommErr.OBJ_ERR);
        }

        //处理事件
        return _dealEvent(_userData, _eventRef, dealInfo, _context);
    }

    public abstract ResultOne<CommonEvent_DoneInfo> _dealEvent(NPUSUserData _userData, T _eventRef, E _dealInfo, NPPlayerContext _context);
}
