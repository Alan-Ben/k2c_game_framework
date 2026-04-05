package CrossDataServer.GeneralListener.RequestDispather;

import CrossDataServer.CrossDataMgr.CrossDataController;
import CrossDataServer.GeneralListener.RB_Writer.GOM2CD_RB_Writer_001_DataOp;
import GOM2CD_R.gom_p001_DataOp.*;
import NPCommon.Dispather.NPRequestDispatcher;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

public class GOMCDGeneral_001_RequestDispatcher_DataOp extends NPRequestDispatcher
{
    public static void init(GOMCDGeneralRequestDispather _dispather)
    {
        //001_001
    	_dispather.regHandler(new NPRequestDealer<GOM2CD_R_001_001_InitData>() {
            @Override
            protected void _dealMessage(_IWCGBasicRequestCommiter _commiter, GOM2CD_R_001_001_InitData _msg) {
                CrossDataController.rmvUSData(_msg.getDataType(), _msg.getGroupId(), _msg.getUsId());

                //返回成功
                _commiter.commitSucRes(GOM2CD_RB_Writer_001_DataOp.make_001_RetInitData());
            }
        });

        //001_002
        _dispather.regHandler(new NPRequestDealer<GOM2CD_R_001_002_SyncData>() {
            @Override
            protected void _dealMessage(_IWCGBasicRequestCommiter _commiter, GOM2CD_R_001_002_SyncData _msg)
            {
                CrossDataController.syncDataList(_msg.getDataType(), _msg.getGroupId(), _msg.getUsId(), _msg.getDataList());

                //返回成功
                _commiter.commitSucRes(GOM2CD_RB_Writer_001_DataOp.make_002_RetSyncData());
            }
        });

        //001_003
        _dispather.regHandler(new NPRequestDealer<GOM2CD_R_001_003_RmvData>() {
            @Override
            protected void _dealMessage(_IWCGBasicRequestCommiter _commiter, GOM2CD_R_001_003_RmvData _msg)
            {
                CrossDataController.rmvData(_msg.getDataType(), _msg.getGroupId(), _msg.getUsId(), _msg.getDataId());

                //返回成功
                _commiter.commitSucRes(GOM2CD_RB_Writer_001_DataOp.make_003_RetRmvData());
            }
        });

        //001_005
        _dispather.regHandler(new NPRequestDealer<GOM2CD_R_001_005_ClearUSFromGroup>() {
            @Override
            protected void _dealMessage(_IWCGBasicRequestCommiter _commiter, GOM2CD_R_001_005_ClearUSFromGroup _msg)
            {
                CrossDataController.rmvUSData(_msg.getDataType(), _msg.getGroupId(), _msg.getUsId());

                //返回成功
                _commiter.commitSucRes(GOM2CD_RB_Writer_001_DataOp.make_005_RetClearUSFromGroup());
            }
        });


        //001_010
        _dispather.regHandler(new NPRequestDealer<GOM2CD_R_001_010_CustomDataOp>() {
            @Override
            protected void _dealMessage(_IWCGBasicRequestCommiter _commiter, GOM2CD_R_001_010_CustomDataOp _msg)
            {
                CrossDataController.dealCustomOp(_msg.getDataType(), _msg.getGroupId(), _commiter, _msg.get_buffer_OpData());
            }
        });
    }
}
