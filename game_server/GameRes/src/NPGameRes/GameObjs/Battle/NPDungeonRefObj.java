package NPGameRes.GameObjs.Battle;

import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPGameRes.NPGRefdataCoreMgr;
import NPGameRes.Refs.Battle.RefDungeon;
import NPGameRes.Refs.Battle.RefMapMob;
import WCGCommon.Enum.NPEnum.EWCGDungeonSpecialType;

import java.util.ArrayList;
import java.util.List;

public class NPDungeonRefObj extends _IALBasicRefObj
{
    public long _refId()
    {
        return id;
    }

    public long id;

    public long mapId;//地图id

    public boolean check;//该副本是否需要验证战斗逻辑合法性

    public float time_scale;    //时间倍率

    public long battle_music;//副本音效索引

    public int audio_to_screen_center_dis_for_stop;//副本管理音效关闭检测的距离屏幕中心点的距离


    public List<Long> race_sk_list;//默认携带的指挥官技能
    public int add_race_sk_num;//额外添加的指挥官技能数量

    public String icon;//段位图标 

    public long hide_wnd_type_list; //副本初始化时禁用的功能UI类型枚举列表

    public boolean is_fog_disable;//迷雾是否无效
    public boolean close_bgm;//战斗结束是否关闭背景音乐

    public List<WCGMobInstanceRef> mobInstanceList = new ArrayList<>();//初始生物列表

    public String dungeon_name;//dungeon名字
    public String dungeon_icon;//Dungeon图标

    public long special_type;//地图特殊类型

    //后续初始化部分
    private WCGMapRefObj _m_mrMapRef;

    public WCGMapRefObj mapRef()
    {
        if (null == _m_mrMapRef)
            _m_mrMapRef = NPGRefdataCoreMgr.getInstance().mapMap.getRef(mapId);

        return _m_mrMapRef;
    }

    public static List<WCGSimpleActorInfo> readSimpleActorInfoList(String _str)
    {
        List<WCGSimpleActorInfo> simpleInfoList = new ArrayList<WCGSimpleActorInfo>();
        if (_str.trim().length() <= 0)
            return simpleInfoList;

        if (_str.isEmpty())
            return simpleInfoList;

        String[] strs = CommonFunc.charSplit(_str, ';');

        for (int i = 0; i < strs.length; i++)
        {
            String[] temp = CommonFunc.charSplit(strs[i], ':');

            try
            {
                WCGSimpleActorInfo tempActor = new WCGSimpleActorInfo();
                tempActor.actorId = Long.parseLong(temp[0].trim());
                tempActor.actorLevel = Integer.parseInt(temp[1].trim());

                simpleInfoList.add(tempActor);
            } catch (Exception e)
            {
                CommLog.error("WCGSimpleActorInfo refObj init error, check DungeonFrom:" + _str, e);
            }
        }
        return simpleInfoList;

    }

    public WCGMobInstanceRef getMobInstanceRef(long _instanceId)
    {
        for (int i = 0; i < mobInstanceList.size(); i++)
        {
            WCGMobInstanceRef mobObj = mobInstanceList.get(i);
            if (mobObj.mob_instance_id == _instanceId)
                return mobObj;
        }
        return null;
    }

    public void adapt(RefDungeon ref)
    {
        this.id = ref.id;
        this.mapId = ref.map_id;
        this.time_scale = ref.time_scale;
        //this.select_actor =ref.select_actor;
        //this.select_hero =ref.select_hero;
        this.check = ref.check;
        this.battle_music = ref.battle_music;//副本音效索引
        this.race_sk_list = ref.race_sk_list;
        this.add_race_sk_num = ref.add_race_sk_num;
        this.dungeon_name = ref.dungeon_name;//dungeon名字
        this.dungeon_icon = ref.dungeon_icon;//Dungeon图标

        if (ref.special_type != null && !ref.special_type.isEmpty())
        {
            String[] strs = CommonFunc.charSplit(ref.special_type, ':');
            try
            {
                for (int i = 0; i < strs.length; i++)
                {
                    if (!strs[i].trim().isEmpty())
                    {
                        EWCGDungeonSpecialType specialType = EWCGDungeonSpecialType.valueOf(strs[i].trim().toUpperCase());
                        this.special_type |= 1L << (int) specialType.ordinal();
                    }

                }
            } catch (Exception e)
            {
                CommLog.error("副本特殊类型配置错误 - WCGConditionDungeonSpeicalType example: enum:EWCGDungeonSpecialType:EWCGDungeonSpecialType str:" + ref.special_type, e);
            }
        }


        // this.mobList = ref.mobList;
        for (RefMapMob refMobInstance : RefMapMob.getMgr().getList())
        {
            if (refMobInstance.dungeon_id == this.id)
            {
                WCGMobInstanceRef obj = new WCGMobInstanceRef();
                obj.adapt(refMobInstance);
                this.mobInstanceList.add(obj);
            }
        }
    }
}