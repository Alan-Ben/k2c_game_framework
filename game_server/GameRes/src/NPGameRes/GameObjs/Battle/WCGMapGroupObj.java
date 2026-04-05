package NPGameRes.GameObjs.Battle;

import NPGameRes.Refs.Battle.RefMapGroup;

import java.util.ArrayList;

public class WCGMapGroupObj
{
    public int groupId;//组id
    public long friend;//友方组
    public long default_race;//默认种族
    public ArrayList<Integer> share_res_group;//共享资源的group

    public boolean isFriend(int _groupId)
    {
        return (friend & (1 << _groupId)) != 0;
    }

    public void adapt(RefMapGroup refMapGroup)
    {
        this.groupId = refMapGroup.group_id;
        this.friend = 0;
        for (int i = 0; i < refMapGroup.friend_group.size(); i++)
        {
            if (refMapGroup.friend_group.get(i) >= 64)
            {
                System.err.println("err friend group id: " + refMapGroup.friend_group.get(i) + " in map: " + refMapGroup.map_id);
                continue;
            }

            this.friend = this.friend | (1L << refMapGroup.friend_group.get(i));
        }

        this.default_race = refMapGroup.default_race;
        this.share_res_group = new ArrayList<Integer>();
        this.share_res_group.addAll(refMapGroup.share_res_group);
    }
}