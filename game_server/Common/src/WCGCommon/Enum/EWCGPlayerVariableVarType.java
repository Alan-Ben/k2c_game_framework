package WCGCommon.Enum;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;
import java.util.Map;
import java.util.concurrent.ConcurrentHashMap;

/********************
 * 错误枚举定义类
 * @author Administrator
 *
 */
public enum EWCGPlayerVariableVarType
{
    NONE(0),
    ACTION,//逻辑事件
    ARENA_BATTEL_END, //平衡竞技场战斗结束
    E0_IS_WIN,//是否胜利,true或false
    E0_IS_DRAW,//是否平局,true或false
    E0_IS_ALLWIN,//是否战斗全胜,true或false
    E0_USED_RACE,//使用的种族long
    E0_SERIAL_WIN,//连胜次数 Int

    BEFORE_GAIN_ITEM(10),//获取物品前触发事件
    E10_GAME_EVENT,//获取物品时的行为事件枚举,参见枚举EWCGGameEvent
    E10_UNIFORM_ID, //物品的UniformID
    E10_ITEM_TYPE, //物品类型,参见枚举EWCGItemType
    E10_ITEM_ID, //物品的子ID
    E10_SRC_COUNT,//要获得的数量
    E10_ADD_COUNT,//额外修正数量
    E10_SET_COUNT,//额外设置数量
    E10_NEW_UNIFORM_ID,//允许修改物品为新的ID。

    CONSUME(20),//物品消耗后事件
    E20_GAME_EVENT, //物品消耗是的行为枚举,参见枚举EWCGGameEvent
    E20_UNIFORM_ID, //物品的UniformID
    E20_ITEM_TYPE, //物品类型,参见枚举EWCGItemType
    E20_ITEM_ID,  //物品的子ID
    E20_ITEM_COUNT, //消耗的数量

    DRAW_CARD(30),//抽卡事件
    E30_DRAW_ID,//抽卡对象的ID

    LADDER_BATTEL_END(40),//天梯战斗结束事件
    E40_IS_WIN,//是否胜利,true或false
    E40_IS_DRAW,//是否平局,true或false
    E40_MATCH_SIZE,//比赛人数1--1v1,2--2v2,3--3v3
    E40_SERIAL_WIN,//连胜次数
    E40_USED_RACE,//使用的种族long

    DAY_LOGIN(50),//每日跨天登录事件
    E50_LOING_DATE,//登录日期，如20120821

    RECHARGE(60),//充值事件
    E60_ALL_GEM,//获得的全部宝石
    E60_BASE_GEM,//获得的基础宝石
    E60_EXTRA_GEM,//获得的额外赠送宝石

    GAIN(70),//物品获取后事件
    E70_GAME_EVENT, //物品消耗是的行为枚举,参见枚举EWCGGameEvent
    E70_UNIFORM_ID, //物品的UniformID
    E70_ITEM_TYPE, //物品类型,参见枚举EWCGItemType
    E70_ITEM_ID,  //物品的子ID
    E70_ITEM_COUNT, //获得的数量
    E70_ITEM_BALANCE, //最后数量

    ADD_FRIEND(80),
    E80_CUR_FRIEND_NUM, //当前好友数量

    BATTLE_END(90),
    E90_IS_WIN,//是否胜利,true或false
    E90_IS_DRAW,//是否平局,true或false
    E90_IS_MVP,//是否平局,true或false
    E90_USED_RACE,//使用的种族long
    E90_ROOM_TYPE,//比赛房间类型，参见枚举
    E90_BATTLE_SCORE,//战斗积分
    E90_KILL_HERO,//个人击杀的英雄数量
    E90_KILL_DRAGON,//个人击杀的大龙数量
    E90_PUSH_TOWER,//个人推塔数量
    E90_CRASH_BASE,//个人摧毁基地数量
    E90_KILL_HERO_CAMP,//阵营击杀的英雄数量
    E90_KILL_DRAGON_CAMP,//阵营击杀的大龙数量
    E90_PUSH_TOWER_CAMP,//阵营推塔数量
    E90_CRASH_BASE_CAMP,//阵营摧毁基地数量
    E90_GET_STRONGHOLDE,//累计占领据点
    E90_BASE_LEVEL,//基地等级。
    E90_MAX_SUPPLY,//最大人口。
    E90_MAX_COIN,//最大金币。
    E90_MAX_ROUGH,//最大原石。
    E90_BATTLE_TIME_SEC,//战斗时间，秒。
    E90_HERO_DIE,//英雄被杀次数。
    E90_TOWER_DIE,//被摧毁的塔数量。
    E90_GOT_MINE,//占领的矿数量。


    UPGRADE_CARD(120),//卡牌升级
    E120_CARD_ID,//卡牌id actor表的id
    E120_ACTOR_TYPE,//卡牌类型见枚举 EWCGActorType

    OPEN_CARD_PACK(125),//打开卡包
    E110_DRAW_ID,//卡包id

    VIEW_BATTLE_RECORD(130),//播放战报
    VIEW_ELITE_RECORD(140),//播放精彩战报
    COMPLTE_DAILY_TASK(150),//完成每日任务
    SHOP_BUY(160),//商店购买
    LEVEL_UP(170),//升级
    E170_LEVEL,//等级
    DONATE_CARD(180),//公会捐赠卡牌
    E180_CARD_ID,//卡牌ID
    E180_CARD_QUALITY,//卡牌类型见枚举 EWCGActorType
    E180_CARD_RACE,//

    CASUAL_BATTLE_END(190),//休闲赛结束事件
    E190_IS_WIN,//是否胜利,true或false
    E190_IS_DRAW,//是否平局,true或false
    E190_MATCH_SIZE,//比赛人数1--1v1,2--2v2,3--3v3
    E190_SERIAL_WIN,//连胜次数
    E190_USED_RACE,//使用的种族long
    E190_CASUAL_ID,//休闲赛ID
    GAIN_BUFF(200),//获得buff
    E200_BUFF_ID,//buff id
    E201_IS_NEW,//是否新加buff
    E202_ADD_LAYER,//新加buff 层数
    LOSE_BUFF(210),//获得buff
    E210_BUFF_ID,//buff id
    E210_LAYER,//buff 层数
    RENAME(220),//改名
    E220_ISFREE,//是否免费
    CHG_ICON_BK(230),//更改头像框
    E230_BK_ID,//头像框ID
    SWITCH_TASK(240),//切换每日任务
    E240_ISFREE,//是否免费

    ;
    private static ArrayList<EWCGPlayerVariableVarType> _m_lWriteableVars = new ArrayList<EWCGPlayerVariableVarType>(Arrays.asList(
            E10_ADD_COUNT,
            E10_SET_COUNT,
            E10_NEW_UNIFORM_ID
    ));//连胜加成

    public static boolean isVarWritable(EWCGPlayerVariableVarType _var)
    {
        return _m_lWriteableVars.contains(_var);
    }

    /*************************
     *public methods
     ***************************/
    public int number()
    {
        return _mNumber;
    }

    public static EWCGPlayerVariableVarType valueOf(int _number)
    {
        return _gValueMap.get(_number);
    }

    public static List<Integer> numbers()
    {
        return _gNumbers;
    }

    /*************************
     *private methods
     ***************************/
    private static int _gLastNumber = 0;
    private static Map<Integer, EWCGPlayerVariableVarType> _gValueMap;
    private static ArrayList<Integer> _gNumbers;

    private static void setLastNumber(int _number)
    {
        _gLastNumber = _number;
    }

    private static int getLastNumber()
    {
        return _gLastNumber;
    }

    private static void regItem(int _number, EWCGPlayerVariableVarType e)
    {
        if (null == _gValueMap)
        {
            _gValueMap = new ConcurrentHashMap<Integer, EWCGPlayerVariableVarType>();
        }
        if (null == _gNumbers)
        {
            _gNumbers = new ArrayList<Integer>();
        }
        _gValueMap.put(_number, e);
        _gNumbers.add(_number);
    }

    private int _mNumber;

    private EWCGPlayerVariableVarType(int _number)
    {
        _mNumber = _number;
        setLastNumber(_number);
        regItem(_mNumber, this);
    }

    private EWCGPlayerVariableVarType()
    {
        _mNumber = getLastNumber() + 1;
        setLastNumber(this._mNumber);
        regItem(_mNumber, this);
    }


}
