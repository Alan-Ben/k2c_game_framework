package NPGameRes.InitDealer;

import NPCommon.Log.CommLog;
import NPGameRes.Refs.Activity.RefActivityBagItem;
import NPGameRes.Refs.BagItem.RefBagItem;

public class BagItemInitDealer extends _ABasicInitDealer
{
    @Override
    public void dealInit()
    {
        for (RefActivityBagItem refActivityBagItem : RefActivityBagItem.getMgr().getList())
        {
            RefBagItem refBagItem = RefBagItem.getMgr().get(refActivityBagItem.bag_item_id);
            if (refBagItem == null)
            {
                CommLog.error("BagItemInitDealer dealInit RefActivityBagItem relative refBagItem not found, bagItemId:{}", refActivityBagItem.Id());
                continue;
            }

            refBagItem.relativeActivityBagItemRef = refActivityBagItem;
        }

    }
}
