package NPGameRes.InitDealer;

import Common.ArenaEnum.EArenaBuffType;
import NPCommon.Log.CommLog;
import NPGameRes.Refs.Arena.RefArenaBuff;
import NPGameRes.Refs.RefGeneral;

public class ArenaInitDealer extends _ABasicInitDealer
{
    @Override
    public void dealInit()
    {
		RefArenaBuff[] arenaBuffList = new RefArenaBuff[EArenaBuffType.EArenaBuffType_Length];
		for (Long buffId : RefGeneral.Ref().arena_choose_buff_list)
		{
			RefArenaBuff buffRef = RefArenaBuff.getMgr().get(buffId);
			if (null == buffRef)
			{
				CommLog.error("init arena buff ref fail, not find buff:{} ref.", buffId);
				continue;
			}

			arenaBuffList[buffRef.type.ordinal()] =buffRef;
		}
		RefGeneral.Ref().arenaChooseBuffListRef = arenaBuffList;

    }
}
