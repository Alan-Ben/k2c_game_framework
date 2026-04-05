package NPCrossGameServer.NPGeneralListener.RequestDispather;

import NP2CGS_R.p001_BasicOp.NP2CGS_R_001_001_SendCrossGameRequest;
import NP2CGS_R.p001_BasicOp.NP2CGS_R_001_002_ReqCreateCrossGameInstance;
import NP2CGS_R.p001_BasicOp.NP2CGS_R_001_003_ReqDiscardCrossGameInstance;
import NP2CGS_R.p001_BasicOp.NP2CGS_R_001_004_ReqGM;
import NP2CGS_RB.p001_BasicOp.NP2CGS_RB_001_002_RetCreateCrossGameInstance;
import NPCommon.Dispather.NPRequestDispatcher;
import NPCommon.ErrMain.CGSErr;
import NPCommon.GMCommand.GmCommandMgr;
import NPCommon.Util.CallBack._IRunCallBack;
import NPCrossGameServer.NPCrossGameContext.NPCrossGameContext;
import NPCrossGameServer.NPCrossGameCore.CrossGameCategoryMgr;
import NPCrossGameServer.NPCrossGameCore._ACrossGameInstanceCategory;
import NPEnum.ENPGameEvent;
import NPServerProtocolWriter.NP2CGS.RequestBack.NP2CGS_RB_Writer_001_BasicOp;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

public class NPCGSGeneral_001_RequestDispather_BasicOp extends NPRequestDispatcher
{
    public static void init(NPCGSGeneralRequestDispather _dispather)
    {
        _dispather.regHandler(new NPRequestDealer<NP2CGS_R_001_001_SendCrossGameRequest>()
        {
            @SuppressWarnings("rawtypes")
            @Override
            protected void _dealMessage(final _IWCGBasicRequestCommiter _committer, final NP2CGS_R_001_001_SendCrossGameRequest _msg)
            {
                _ACrossGameInstanceCategory category = CrossGameCategoryMgr.getInstance().getCategory(_msg.getCategory().ordinal());
                if (null == category)
                {
                    _committer.commitFailRes(CGSErr.NO_CROSSGAME_CATEGORY_ERROR.getCode());
                    return;
                }

                category.getMsgMgr().addRequest(_msg.getInstanceId(), _msg.getCid(), _msg.get_buffer_ReqMsg(), _committer);
            }
        });

        _dispather.regHandler(new NPRequestDealer<NP2CGS_R_001_002_ReqCreateCrossGameInstance>()
        {
            @SuppressWarnings("rawtypes")
            @Override
            protected void _dealMessage(final _IWCGBasicRequestCommiter _committer, final NP2CGS_R_001_002_ReqCreateCrossGameInstance _msg)
            {
                _ACrossGameInstanceCategory category = CrossGameCategoryMgr.getInstance().getCategory(_msg.getCategory().ordinal());
                if (null == category)
                {
                    _committer.commitFailRes(CGSErr.NO_CROSSGAME_CATEGORY_ERROR.getCode());
                    return;
                }

                NP2CGS_RB_001_002_RetCreateCrossGameInstance ret = new NP2CGS_RB_001_002_RetCreateCrossGameInstance();

                if (!category.createNewInstance(_msg.getInstanceId(), _msg.getExtData(), ret))
                {
                    _committer.commitFailRes(CGSErr.NO_CROSSGAME_CATEGORY_ERROR.getCode());
                    return;
                }

                _committer.commitSucRes(ret);
            }
        });

        _dispather.regHandler(new NPRequestDealer<NP2CGS_R_001_003_ReqDiscardCrossGameInstance>()
        {
            @SuppressWarnings("rawtypes")
            @Override
            protected void _dealMessage(final _IWCGBasicRequestCommiter _committer, final NP2CGS_R_001_003_ReqDiscardCrossGameInstance _msg)
            {
                _ACrossGameInstanceCategory category = CrossGameCategoryMgr.getInstance().getCategory(_msg.getCategory().ordinal());
                if (null == category)
                {
                    _committer.commitFailRes(CGSErr.NO_CROSSGAME_CATEGORY_ERROR.getCode());
                    return;
                }

                category.unregGameInstance(_msg.getInstanceId());

                _committer.commitSucRes(NP2CGS_RB_Writer_001_BasicOp.make_003_RetDiscardCrossGameInstance());
            }
        });
        _dispather.regHandler(new NPRequestDealer<NP2CGS_R_001_004_ReqGM>()
        {
            @Override
            protected void _dealMessage(final _IWCGBasicRequestCommiter _committer, final NP2CGS_R_001_004_ReqGM _msg)
            {
                NPCrossGameContext context = NPCrossGameContext.createNew(ENPGameEvent.GM_CMD);
                GmCommandMgr.getInstance().run(null, _msg.getGm(), context, new _IRunCallBack()
                {
                    @Override
                    public void onRunOver(boolean _bSucc, String _msg)
                    {
                        _committer.commitSucRes(NP2CGS_RB_Writer_001_BasicOp.make_004_RetGM(_bSucc, _msg));
                    }
                });
            }
        });
    }
}
