package NPUSServer.NPUserMsgDispather.p004_PlayerOp;

import ALBasicServer.ALProcess.ALProcess;
import ALBasicServer.ALProcess._AALProcess;
import ALBasicServer.ALProcess._IALProcessMonitor;
import GC2GS.p004_PlayerOp.GC2GS_004_024_ReqRefreshChatBoxStatus;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CallBack._ICallBackBool;
import NPCommon.Util.CallBack._ICallBackResultT;
import NPEnum.ENPBoxChatStatus;
import NPGameRes.Refs.Share.RefBoxComm;
import NPUSServer.NPUSUserMgr.GameSystem.BoxSystem.BoxSystem;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_004_PlayerOp;
import NPUSServer.USLog;
import WCGCS2US_RB.p003_CommOp.NP2US_RB_003_008_RetGetBoxInfo;

public class MsgDealer_GC2GS_004_024_ReqRefreshChatBoxStatus extends NPUserMsgDealer<GC2GS_004_024_ReqRefreshChatBoxStatus>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_004_024_ReqRefreshChatBoxStatus _msg)
    {
        /***************************************************
         * 领取宝箱步骤
         * 1. 获取对应userServer宝箱数据
         *
         **************************************************/

        //开启Process
        final ALProcess process = ALProcess.CreateProcess("box_refresh");

        process.addResDelegateProcess(_doneAction ->
        { //获取对应userServer宝箱数据
            __getBox(_commiter, _msg.getInstanceId(), _commiter.getUserData(), _isSuc ->
            {
                _doneAction.dealAction(_isSuc);
            });
        }, "get_box");

        //开启执行
        process.dealProcess(new _IALProcessMonitor()
        {
            @Override
            public void onTimeoutDone(long _processTimeMS, String _processTag, String _exInfo)
            {
            }

            @Override
            public void onTimeout(long _processTimeMS, String _processTag, String _exInfo)
            {
            }

            //异常终止的事件函数
            @Override
            public void onRootProecssStop()
            {
            }

            //正常结束的事件函数
            @Override
            public void onRootProecssSuc()
            {
            }

            @Override
            public void onProcessFailStop(_AALProcess _process)
            {
            }

            @Override
            public void onRootProecssDone()
            {
            }

            @Override
            public void onErr(long _processTimeMS, String _processTag, String _exInfo, Exception _ex)
            {
                USLog.error(getUSServer(), "box refresh err: " + _ex.getMessage());
            }

            @Override
            public long monitorTimeMS(String _processTag)
            {
                return 0;
            }
        });
    }

    /**********
     * 步骤1：获取并检查宝箱
     * @param _commiter
     * @param _instanceId
     * @param _userData
     * @param _handler
     */
    private void __getBox(_ANPUSUserBasicMsgItem _commiter, long _instanceId, NPUSUserData _userData, final _ICallBackBool _handler)
    {
        BoxSystem.getBox(getUSServer(), _instanceId, _userData.getCid(), new _ICallBackResultT<NP2US_RB_003_008_RetGetBoxInfo>()
        {

            @Override
            public void onRunOver(Result _result, NP2US_RB_003_008_RetGetBoxInfo _boxInfo)
            {
                //服务器连接失败
                if (!_result.isSucc())
                {
                    //返回错误
                    _commiter.commitFailRes(_result.getCode());
                    //中断Process
                    _handler.onRunOver(false);
                    return;
                }

                //检查宝箱
                if (_boxInfo.getBoxId() <= 0)
                {
                    _commiter.commitSucRes(US2GCWriter_004_PlayerOp.make_024_RetRefreshChatBoxStatus(_instanceId, ENPBoxChatStatus.IS_INVAILD));
                    //中断Process
                    _handler.onRunOver(false);
                    return;
                }
                //检查本服宝箱对应配置
                RefBoxComm ref = RefBoxComm.getMgr().get(_boxInfo.getBoxId());
                if (null == ref)
                {
                    USLog.error(getUSServer(), "NPGC2GS_004_024_ReqRefreshChatBoxStatus ref not found refId:{}", _boxInfo.getBoxId());
                    //返回错误
                    _commiter.commitFailRes(CommErr.REF_NOT_FOUND.getCode());
                    //中断Process
                    _handler.onRunOver(false);
                    return;
                }

                //检查宝箱状态
                if (ENPBoxChatStatus.NONE != _boxInfo.getBoxStatus())
                {
                    _commiter.commitSucRes(US2GCWriter_004_PlayerOp.make_024_RetRefreshChatBoxStatus(
                            _instanceId, _boxInfo.getBoxStatus(), _boxInfo.getGainedCidList()));
                    //中断Process
                    _handler.onRunOver(false);
                    return;
                }

                //设置玩家领取状态
                if (!_commiter.getUserData().hasCostItemList(ref.cost_item_list))
                {
                    _commiter.commitSucRes(US2GCWriter_004_PlayerOp.make_024_RetRefreshChatBoxStatus(
                            _instanceId, ENPBoxChatStatus.IS_LIMIT, _boxInfo.getGainedCidList()));
                    //中断Process
                    _handler.onRunOver(false);
                    return;
                }

                //返回数据
                _commiter.commitSucRes(US2GCWriter_004_PlayerOp.make_024_RetRefreshChatBoxStatus(
                        _instanceId, _boxInfo.getBoxStatus(), _boxInfo.getGainedCidList()));
                //进行Process下一步
                _handler.onRunOver(true);
            }
        });
    }
}
