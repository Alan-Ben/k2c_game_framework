package NPGameRes.InitDealer;

import NPCommon.Log.CommLog;
import NPEnum.ENPShareItemType;
import NPGameRes.Refs.Share.RefBoxComm;
import NPGameRes.Refs.Share.RefShare;

/*******************
 * 初始化mission中怪物实例信息
 * @author Administrator
 *
 */
public class NPShareInitDealer extends _ABasicInitDealer
{
    @Override
    public void dealInit()
    {
        for (RefBoxComm refBox : RefBoxComm.getMgr().getList())
        {
            RefShare shareRef = RefShare.getMgr().get(refBox.id);
            if (null == shareRef)
            {
                CommLog.error("RefShare not find for box, id:{}", refBox.id);
                continue;
            }

            if (ENPShareItemType.NONE != shareRef.share_item_type)
            {
                CommLog.error("RefShare alreay seted, id:{} oriType:{}", refBox.id, shareRef.share_item_type);
                continue;
            }

            shareRef.share_item_type = ENPShareItemType.BOX;
        }
    }
}
