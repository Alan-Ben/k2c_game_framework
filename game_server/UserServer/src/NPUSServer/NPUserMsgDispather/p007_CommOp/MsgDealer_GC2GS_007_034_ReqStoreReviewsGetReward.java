package NPUSServer.NPUserMsgDispather.p007_CommOp;

import Common.MailObj.Mail_Data;
import GC2GS.p007_CommOp.GC2GS_007_034_ReqStoreReviewsGetReward;
import NPCommon.ErrMain.PlayerErr;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPGameEvent;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.GameSystem.MailSystem.MailSystem;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_007_CommOp;

public class MsgDealer_GC2GS_007_034_ReqStoreReviewsGetReward extends NPUserMsgDealer<GC2GS_007_034_ReqStoreReviewsGetReward> 
{
	@Override
	protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_007_034_ReqStoreReviewsGetReward _msg) 
	{
		NPUSUserData userData = _commiter.getUserData();
		if (null == userData)
			return;
		
		if(!userData.getPlayerComponent().setStoreReviews())
		{
			_commiter.commitFailRes(PlayerErr.STORE_REVIEWS_DONE.getCode());
			return;
		}
		
		//发送奖励邮件
		Mail_Data mailData = new Mail_Data();
        mailData.setMailRefId(RefGeneral.Ref().store_reviews_reward_mail_id);
        mailData.getItemList().getItemList().addAll(CommonFunc.costItemListToProto(RefGeneral.Ref().store_reviews_reward_item_list));
        MailSystem.addMail(userData.getUSServer(), userData.getCid(), mailData, NPPlayerContext.createNew(ENPGameEvent.STORE_REVIEWS_ADD));

		_commiter.commitSucRes(US2GCWriter_007_CommOp.make_034_RetStoreReviewsGetReward());
	}
}
