package WCGCommon.Enum;


/********************
 * 枚举定义类
 * @author Administrator
 *
 */
public class NPEnum
{
    /***************
     * 游戏服务器类型
     * @author Administrator
     *
     */
    public enum EServerType
    {
        NONE,           //0无，不使用本枚举
        SINGLE,         //1独立服务器（全服务器架构只存在一个的特殊类型服务器）
        LOGIN,          //2登录服
        USER,           //3用户服
        GATE,           //4网关
        CROSS_GAME,     //5跨服游戏
        CROSS_RANK,     //7跨服排行榜
        GAME_LOGIC,     //8游戏逻辑服
        ;

        public static final EServerType[] EServerType_Values = EServerType.values();
        public static final int ENPGameEvent_Length = EServerType_Values.length;

        public static EServerType EServerType_FromInt(int _iValue)
        {
            if (_iValue < 0 || _iValue >= ENPGameEvent_Length)
            {
                return null;
            }
            return EServerType_Values[_iValue];
        }
    }

    /***************
     * 特殊服务器类型
     * @author Administrator
     *
     */
    public enum ENPSingleServerType
    {
        NONE,
        LOGIN_CHECK,         //1登录检测的处理服务器
        COMMON,              //2通用服务器，一般用于匹配处理等通用操作
        HTTP,                //3HTTP服务器，一般用于与PHP对接操作
        INTERFACE,           //4接口服务器
        RECORD,              //5记录服务器
        SCHEDULE,            //6排期服务器
        MARRY_MATCH,         //7联姻匹配服务器
        DINNER,              //8宴会服务器
        SHARE_CODE,          //9分享码服务器
        PAY,                 //10支付服务器
        CROSS_DATA,          //11跨服数据
        CROSS_TEAM,          //12跨服队伍
        ;
        public static final ENPSingleServerType[] ENPSingleServerType_Values = ENPSingleServerType.values();
        public static final int ENPGameEvent_Length = ENPSingleServerType_Values.length;

        public static ENPSingleServerType ENPSingleServerType_FromInt(int _ivalue)
        {
            if (_ivalue < 0 || _ivalue >= ENPGameEvent_Length)
            {
                return null;
            }
            return ENPSingleServerType_Values[_ivalue];
        }
    }

    public enum EClientLoginType
    {
        NONE,
        LOCAL_CLIENT,//本地客户端连接
    }

    /***************
     * 游戏国家枚举类型
     * @author Administrator
     *
     */
    public enum EWCGCountryType
    {
        MECHANIST,
        CHINA,
    }

    /***************
     * 踢出Gate的类型
     * @author Administrator
     *
     */
    public enum EWCGKickOutGateType
    {
        NONE,
        DEVICE, //玩家登陆额外设备踢人
        OPERATE, //运营封号踢人
        SYSTEM,//系统原因清理
    }

    /***************
     * 战斗房间类型
     * @author Administrator
     *
     */
    public enum EWCGRoomType
    {
        NONE,
        PVE,//PVE关卡
    }


    //匹配类型
    public enum EWCGCustomMatchType
    {
        NONE,
        ONE_TO_ONE,//1V1
        TWO_TO_TWO,//2V2
        THREE_TO_THREE,
    }


    /************
     * 战斗当前的状态
     * @author Administrator
     *
     */
    public enum EWCGBattleState
    {
        WAITING,
        ING,
        DONE,
        INTERRUPTED,//强行中断比赛
    }


    public enum EProfileEvent
    {
        eProfile_loadUserData,
        eProfile_verifyIdentity,
        eProfile_matching,
    }


//==============================服務器端枚举分割线==============================   


    public enum DayOfWeek
    {
        SUNDAY,
        MONDAY,
        TUESDAY,
        WEDNESDAY,
        THURSDAY,
        FRIDAY,
        SATURDAY
    }

    //==============================客户端端枚举分割线==============================
///////////////////////////////////////////////////////////////////
//client battle enum  
///////////////////////////////////////////////////////////////////
    public enum EWCGLayer
    {
        DEFAULT,
        TRANSPARENT_FX,
        IGNORE_RAYCAST,
        LAYER_3,
        WATER,
        UI,
        LAYER_6,
        LAYER_7,

        //--- 以下可修改
        LAYER_8,
        LAYER_9,
        GO_TRIGGER,
        GAME_UNIT,
        GAME_UNIT_SHADOW,
        GAME_ACTOR,
        GAME_SCENE,
        LAYER_15,
        LAYER_16,
        LAYER_17,
        LAYER_18,
        LAYER_19,
        GAME_IGNORE_LAYER,
        LAYER_21,
        LAYER_22,
        LAYER_23,
        LAYER_24,
        LAYER_25,
        LAYER_26,
        LAYER_27,
        LAYER_28,
        LAYER_29,
        GAME_FIELD_OF_VIEW,
        LAYER_31,
    }

    /**
     * 游戏内Scene的类型
     */
    public enum EWCGSceneType
    {
        NONE,
        UI_SCENE,       //UI视图
        TD_SCENE,       //3D视图
    }


    /*************
     * MMC(聊天)的客户端连接类型
     **/
    public enum EClientChatLoginType
    {
        NONE,
        NORMAL,
    }

    public enum EWCGPosState
    {
        NONE,
        IDLE,
        MOVE,
        TELEPORT,
        PUSH,
    }

    //单位动画状态
    public enum EWCGActorState
    {
        NONE,
        ATTACK,
        MOVE,
        RUN,
        IDLE,
        CONTROL,
        DYING,//临死状态
        DEATH,
    }

    /**
     * 本对象当前是出战状态还是默认状态的枚举
     */
    public enum EWCGActorOpState
    {
        DEFENCE,
        BATTLE,
    }

    public enum EWCGControlState
    {
        NORMAL,//正常
        STUN,//昏迷
        FROZEN,//冰冻
    }

    public enum EWCGAnimDirection
    {
        NONE,
        DOWN,               //1
        DOWN_RIGHT,         //2
        RIGHT,              //3
        UP_RIGHT,           //4
        UP,                 //5
        UP_LEFT,
        LEFT,
        DOWN_LEFT,
    }

    public enum EWCGSkillTriggerType
    {
        NONE,
        KILL,//击杀
        DEATH,//死亡

        HEAL,//造成治疗
        BE_HEAL,//被治疗

        DAMAGE,//造成伤害
        BE_DAMAGE,//被伤害

        ATK_SUC,//攻击成功
        HIT_SUC,//命中
        BE_HIT_SUC,//被命中

        SUMMON_SUC,//召唤成功

        BIRTH,//出场

        SKILL_T,//任意技能

        DYING,//将要死亡

        DEF_ATT,    //默认攻击

        CRIT,       //暴击
        BE_CRIT,    //被暴击
        DUDGE,      //闪避
        BE_DUDGE,   //被闪避

        SKILL_NOT_DEF,  //非普攻技能触发的时候
    }

    //效果有效类型  
    public enum ENPRelationType
    {
        NONE,  //无  一般单体效果目标填着个
        SELF,//自己，释放者自己
        ALLY,//同盟，和自己同一个group的对象
        ENEMY,//敌人，敌人
        FRIEND,//队友，和自己友好的group对象
    }

    //效果范围类型
    public enum EWCGEffectRangeType
    {
        NONE,
        SINGLE,//单体
        CIRCLE,//圆形
        RECT, //矩形  根据距离判断
        LIMIT_RECT,//矩形  最大距离或者以目标距离结束
        SECTOR,//扇形
        LIMIT_SECTOR,
        ALL,//所有单位
        TEAM,//指挥官技能范围区别于其他
    }

    public class EWCGEffectRangeTypeComparer extends IEqualityComparer<EWCGEffectRangeType>
    {
        public boolean Equals(EWCGEffectRangeType x, EWCGEffectRangeType y)
        {
            return x == y;
        }

        public int GetHashCode(EWCGEffectRangeType obj)
        {
            return (int) obj.ordinal();
        }
    }

    public class EWCGFlyGoTypeComparer extends IEqualityComparer<EWCGFlyGoType>
    {
        public boolean Equals(EWCGFlyGoType x, EWCGFlyGoType y)
        {
            return x == y;
        }

        public int GetHashCode(EWCGFlyGoType obj)
        {
            return (int) obj.ordinal();
        }
    }

    //范围效果选择位置
    public enum EWCGEffectRangeSelectPos
    {
        NONE,
        INS,//释放者
        INS_POS,//释放者的位置
        TAR,//作用效果对象
        TAR_POS,//作用效果对象的位置
    }

    //范围效果目标排序条件
    public enum EWCGSortCondition
    {
        DIS_FAR,// - 最远单位
        DIS_NEAR,// - 最近单位
        MAX_HP,// - 最大当前血量
        MIN_HP,// - 最小当前血量
        MAX_SP,// - 最大当前能量
        MIN_SP,// - 最小当前能量
        MAX_M_HP,// - 最大最大血量
        MIN_M_HP,// - 最小最大血量
        MAX_M_SP,// - 最大最大能量
        MIN_M_SP,// - 最小最大能量
        MAX_HP_PER,// - 最大当前血量百分比
        MIN_HP_PER,// - 最小当前血量百分比
        MAX_SP_PER,// - 最大当前能量百分比
        MIN_SP_PER,// - 最小当前能量百分比
        MAX_ARMOR,// - 最大当前防御值
        MIN_ARMOR,// - 最小当前防御值
        MAX_MAGIC_ARMOR,// - 最大当前魔法防御值
        MIN_MAGIC_ARMOR,// - 最小当前魔法防御值
        MAX_ATK,// - 最大当前攻击力
        MIN_ATK,// - 最小当前攻击力

        RND,//   - 随机处理
    }

    /*****************
     * 技能触发效果的类型
     **/
    public enum EWCGEffectType
    {
        NONE,
        DMG,                    //伤害  有命中  无吸血
        HEAL,                   //治疗
        RMV_SP,                 //扣除能量值
        ADD_SP,                 //添加能量值
        SET_HP,                 //设置血量
        SET_SP,                 //设置能量值
        ADD_BUF,                //增加buf
        SET_BUF,                //设置buf状态
        SET_G_BUF,           //设置buf类型的状态
        DEL_BUF,                //删除一个BuffId 或者一组(BuffId列表)对应的Buff
        DEL_G_BUF,              //删除一个groupId或者一组(groupId)对应的Buff
        MOVE,                   //移动效果
        INTERRUPT,              //控制状态效果

        ADD_RES,                //增加队伍资源
        DEL_RES,                //减少队伍资源

        SUMMON,                 //召唤
        CHG_ACTOR,              //角色替换
        PLAY_SFX,               //播放特效
        CHG_MODEL,              //模型变化

        COLL_RES,               //采集资源(受人口数量阶段百分比影响)

        SPECIAL,    // 特殊效果
        DEL_T_BUF,              //驱散效果 删除目标指定 极性的BUFF

        TAKE_AREA,  //区域归属转换

        RM_BUF,                 //移除buf指定层数
        RM_T_BUF,               //移除bufGroupId的指定层数

        DMG_N,          //普通无命中 无吸血效果伤害
        DMG_X,          //普通有命中 有吸血效果伤害

        SHOW_NOTICE,    //展示策划配置的特殊提示UI
        FORCE_ACTION,      //使用指定技能

        SHOW_SINGLE_NOTICE,    //展示唯一的策划配置的特殊提示UI

        CREATE,         //创造一个对象
        DO_TRIG,        //触发一个AItrigger效果
        SET_AI_V,      //设置AI特殊值
        SHOW_MINIMAP_NOTICE, //小地图提示

        SUMMON_POINT,   //指定地图point_id召唤单位
        COLL_RES_SHARE,               //采集资源并分享给友方(受人口数量阶段百分比影响)

        EFFECT,     //对对象执行指定Id的效果
        SINGLE_MOVE, //从发起点向目标点击退指定距离
        ROUND_MOVE,//从发起点将周围指定范围内的符合关系的对象击退到指定距离之外
        SOUND_PLAY,//播放单位出场音效
        HIT_SOUND_PLAY,//攻击音效
        CHG_BK_MUSIC,//切换背景音乐
        ADD_RACE_SKILL,//增加指挥官技能

        SUMMON_SHARE,   //召唤  共享
        SUMMON_POINT_SHARE,  //指定地图point_id召唤单位  共享
        CREATE_SHARE,   //创造一个对象 共享

        SHOW_TUTORIAL,//展示教程窗口UI

        TELEPORT,   //传送
        RESET_CD,//重置技能CD     

        POS_EFF,//触发位置效果
        PLAY_POS_SFX,               //播放位置特效

        CLOSE_AUDIO,//关闭所有指定AudioID的音效

        SEND_BULLET,//发射一个子弹
        ADD_CD,//增加CD

        PICK_BUF,//增加CD

        ADD_BUF_S,                //增加buf高级方式


        SHAKE_SCREEN,//震屏

        ADD_CAMP_RES,//增加阵营资源
        DEL_CAMP_RES,//减少阵营资源
        CHG_CTL_T,// 修改控制时间

        DEAL_EFF,   //由指定的目标对目标触发effect效果，一般用于目标和释放者之间的切换处理

        TELEPORT_POS,   //传送到指定位置

        ADD_SHARE_RES,                //给共享组增加队伍资源

        C_TIME_SCALE,       //客户端事件缩放，服务端不处理

        C_DO_TIMER,         //处理一个事情一定时间
    }

    //触发效果的特殊效果的类型
    public enum EWCGEffectSpecialType
    {
        NONE,
        CLE_OPER,   //清除目标操作信息
        EVOLVE, ///进化
        DEFEAT,         //失败效果
        VICTORY,        //胜利效果
        DIE,        //死亡 
        DYING,      //进入临死状态
        IDLE,       //重置状态
        LOSE_AREA,  //丢失区域  返回最初归属权
        TELEPORT_OWNER, //传送回拥有者身边
        RECORD_CACHE_POS,   //设置ai中的cache位置信息
        DRAW,    //平局效果

        SHARE_EVOLVE, ///进化为共享组的处理
        HIDE_TUTORIAL,//隐藏教程窗口UI
        STOP_ALL_AUDIO,//停止所有音乐的播放

        LORD_ATK,//领主出击

        REMOVE_ACTOR,//移除对象
    }

    /*****************
     * 技能触发位置的效果的类型
     **/
    public enum EWCGPosEffectType
    {
        NONE,
        TRIGGER_EFFECT,     //以位置作为目标，触发单体的效果
        SUMMON,             //召唤怪物
        SPECIAL,            //特殊效果

        ROUND_MOVE,         //范围移动效果
        TELEPORT,           //瞬移

        PLAY_SFX,           //播放特效

        TIME_LAND,          //定时降落
    }

    //触发效果的特殊效果的类型
    public enum EWCGPosEffectSpecialType
    {
        NONE,
        TELEPORT,           //传送
        MOVEMENT,           //前冲等位移处理
        RECORD_CACHE_POS,   //记录目标位置位置信息到cache中
        RECAL_PATH,         //以此位置重新计算对象的路径信息

    }

    public enum EWCGDmgType
    {
        NONE,
        PHYS,
        MAGIC,
        CHAOS,
    }

    /*****************
     * 效果计算中的参数类型
     **/
    public enum EWCGEffectVariableType
    {
        NONE,
        DMG,            //伤害值
        HEAL,           //治疗值
        TAR_COUNT,      //作用数量
        ACT_TIME,       //动作时间
    }

    //条件筛选目标
    public enum EWCGEffectTargetType
    {
        NONE,
        INS,//释放者
        TAR,//作用效果对象
        INS_OWNER,//释放者的召唤者
        TAR_OWNER,//目标的召唤者
    }

    //比较结果类型
    public enum EWCGCompareResult
    {
        NONE,
        BIG,//前者较大
        EQUAL,//相等
        SMA,//前者较小
        BIG_E,//前者较大或相等
        SMA_E,//前者较小或相等
    }

    /*****************
     * 属性类型枚举
     **/
    public enum __EWCGPropertyType
    {
        NONE,
        HP,                     //生命 
        HP_PER,                 //生命万分比
        SP,                     //能量值
        SP_PER,                 //能量值万分比
        ATK,                    //物理攻击力
        ATK_PER,                //物理攻击力万分比
        POW,                    //法术强度
        POW_PER,                //法术强度万分比
        ARMOR,                  //护甲
        ARMOR_PER,              //护甲万分比
        MAGIC_ARMOR,              //魔抗
        MAGIC_ARMOR_PER,          //魔抗万分比
        ARMOR_IGNORE,           //护甲穿透
        ARMOR_IGNORE_PER,       //护甲穿透万分比
        MAGIC_ARMOR_IGNORE,       //魔抗穿透
        MAGIC_ARMOR_IGNORE_PER,   //魔抗穿透万分比
        HEALED,                   //被治疗效果加强值
        HEALED_PER,               //被治疗效果加强万分比
        DMG_REDUCE,             //伤害减免绝对值
        DMG_REDUCE_RATE,        //伤害减免系数 REALDMG = DMG * 10000 / (10000 + 系数)
        PHYS_REDUCE,            //物理伤害减免绝对值
        PHYS_REDUCE_RATE,       //物理伤害减免系数 REALDMG = DMG * 10000 / (10000 + 系数)
        MAGIC_REDUCE,           //魔法伤害减免绝对值
        MAGIC_REDUCE_RATE,      //魔法伤害减免系数 REALDMG = DMG * 10000 / (10000 + 系数)
        ATK_SPEED,              //攻击速度加成万分比
        MOVE_SPEED,             //移动速度
        MOVE_SPEED_PER,         //移动速度加成万分比
        HP_STEAL,               //生命偷取绝对值
        HP_STEAL_PER,           //生命偷取百分比
        ATK_RANGE,              //攻击范围（100 = 1米）

        MODEL_SCALE,        //模型缩放万分比
        LOCK_RANGE,        //锁敌范围固定加成
        PRODUCE_PER,       //生产速度加成

        CRIT_V,               //暴击值
        CRIT_PER,               //暴击率
        CRIT_DEF_V,            //抗暴值
        CRIT_DEF_PER,           //抗暴率
        CRIT_DMG_PER,           //暴击伤害
        CRIT_DMG_DEF_PER,         //暴击伤害减免

        DODGE_V,              //闪避值
        DODGE_PER,          //闪避率
        HIT_V,                //命中值
        HIT_PER,            //命中率

        WEIGHT,              //重量

        ADD_MOVE_SPEED,         //移动速度实际数值加成，只用于速度的计算，方便策划调整
        SUMMON_TIME,        //召唤时间
        DEPLOY_TIME,        //放置时间

        RELIVE_TIME,//复活时间额外值
        RELIVE_TIME_PER,//复活时间额外百分比

    }

    /*************
     * 条件类型
     **/
    public enum EWCGBothConditionType
    {
        NONE,

        P_COMP,//属性比较
        V_COMP,//状态值比较
        A_COND,//单体条件  -  指向SingleCondition
        BUF_INS_STK,
        BUF_INS_T_STK,
        BELONG, //归属关系  后者是(否)前者的召唤物
        RELATION,//战斗关系

        V_RNG_S,//状态值范围高级处理方法

        S_COMP,//高级公式比较

        S_RNG,//高级公式范围

        BUF_INS_STK_S, //某对象释放的buf信息判断
    }

    //角色特殊标记类型
    public enum EWCGActorSpecialType
    {
        NONE,
        MECHINE,//机械
        HUGE,//巨型
        UNDEAD,//死灵
        SUMMON,//召唤
        ELEMENT,//元素
        ENERGY,//能量生物
        SOUL,//灵体生命
        NONORGANIC,//非有机
        NEUTRAL,//中立单位标记
        DRAGON,//大龙
        NEED_OP,    //需要操作的对象，用于自动战斗
        ACTOR_2117,//标记一个2117类型的单位
    }

    /*************
     * 状态值类型
     **/
    public enum EWCGValueType
    {
        NONE,
        LVL,
        HP,
        SP,
        HP_PER,
        SP_PER,
        MAX_HP,
        MAX_SP,
        ATTACK,
        POW,
        ARMOR,
        MAGIC_ARMOR,
        MOVE_SPEED,
        RADIUS,//半径
        POS_X,//x位置
        POS_Z,//z位置
        MODEL_SCALE,//模型缩放
        FORWARD_X,//前方x位置
        FORWARD_Z,//前方z位置
        WEIGHT,//重量
        DISTANCE2OWNER,//与召唤者的距离
        PATROL_RADIUS,//巡逻半径
        DISTANCE2BIRTH_POS,//表示对象距离出生点的距离 cm
        ATK_RNG,    //攻击距离
        SID,//实例ID
        COST_POPU,//使用人口数
        BATTLE_PROC_TIME,//战斗经过的毫秒数
    }

    /*************
     * 参数类型  , 高级计算公式类型
     **/
    public enum EWCGVariableType
    {
        NONE,
        NUM,//单纯的数值
        PRO,//属性数值
        VALUE,//状态值
        BUF_STACK,// buf叠加层数
        BUF_TYPE_STACK,//buf类型的叠加层数
        SPECIAL,//特殊变量类型
        TEAM_PRO,//队伍属性
        TEAM_VALUE,//队伍值
        RND,//随机数
        AI_VALUE,//AI中的某个状态值
        SKILL_LVL,//技能等级

        SPE_GROUP_PRO,   //指定队伍的属性值
        SPE_GROUP_VALUE, //指定队伍的状态值
        SPE_GROUP_RES,  //指定队伍的资源值
        CAMP_PRO,//阵营属性
        CAMP_RES,//阵营资源

        VAR_VALUE,//动态带入的变量值

        OWNER_V,//所有者的相关数值

        ACTOR_VALUE,    //对应对象队伍中的对应actor的状态值
        SP_ACTOR_VALUE, //对应对象队伍中特殊acotor的状态值
        BATTLE_VALUE,   //战场中对应的状态值
        PLAYER_PARAM,//玩家数值
    }

    /*************
     * 特殊变量类型
     **/
    public enum EWCGSpecialVariableType
    {
        NONE,
        DISTANCE,       //双方距离
        TIME,           //获取当前战斗持续时间
    }

    //小地图Icon类型
    public enum EWCGMiniMapIconType
    {
        NONE,//没有类型
        UNIT,//单位
        HERO,//英雄
        BUILDING_MINI,//建筑
        BUILDING_MID,//建筑
        BUILDING_BIG,//建筑
        STRONGHOLD,//据点 
        CREEPS,//野怪
        MINERALBUILDING,//野矿
        CITY,//主城
        TOWER_BUILDING,//积分塔
        HEROBUILDING,//英雄祭坛
        ROUGH_FACTORY,//原石场
        CREEPS_1,//小野怪
        CREEPS_2,//中野怪
        CREEPS_3,//大野怪
        CREEPS_4,//史诗野
        CREEPS_5,//公共野
        CREEPS_6,//大龙
    }

    public class EWCGMiniMapIconTypeComparer extends IEqualityComparer<EWCGMiniMapIconType>
    {
        public boolean Equals(EWCGMiniMapIconType x, EWCGMiniMapIconType y)
        {
            return x == y;
        }

        public int GetHashCode(EWCGMiniMapIconType obj)
        {
            return (int) obj.ordinal();
        }
    }

    //角色类型
    public enum EWCGActorType
    {
        NONE,
        NORMAL,//普通
        HERO,//英雄
        BUILDING,//建筑
    }

    //角色的分类类型
    public enum EWCGActorUnitType
    {
        NONE,
        MINERAL,//矿
        CORPSE,//死尸单位
        CITY,//主城
        MAIN_BUILDING,  //主力建筑 - 一般用于是否可升级主城的判断
        TOWER_BUILDING,  //守护塔
        STRONGHOLD,//据点
        CREEPS,//野怪
        MINERALBUILDING,//野矿
        HEROBUILDING,//英雄祭坛
        ROUGH_FACTORY,//原石场
        HOUSE,      //民居
        BOW_TOWER,  //炮塔
        FRAMER,     //农民
        BUILDING_CORPSE,//建筑尸体
        MAGIC,//指挥官技能马甲
        ARMY,   //作战单位
        UNBUILTMINERAL,//没建好的野矿
    }

    //单位类型，本类型在summon效果中使用，根据不同类型会获取不同的对象Id
    public enum EWCGUnitType
    {
        NONE,
        ACTOR,//角色

        PRE_MOB,//预设的对象
    }

    //AI的触发类型
    public enum EWCGAITriggerType
    {
        NONE,
        SELF_TRIGGER,//对自己触发对应的效果队列
        SET_AI_V,//设置ai中对应索引的值(idx索引暂定0-99)
        CHG_AI_V,//修改ai中对应索引的值(idx索引暂定0-99)
        START_TIMER,//开启对应的定时AI，并在指定时间后执行（时间为毫秒）
        START_OP,//触发一个3D操作
        SPECIAL,  //特殊事件
        SELF_EFFECT,  //自身效果
        PRINT,      //输出信息到控制台
        SHOW_NOTICE,    //展示特殊提示UI
        SHOW_SINGLE_NOTICE,    //展示唯一的特殊提示UI
        SOUND_PLAY,//播放单位出场音效
        CAMP_WIN,//指定阵营胜利
        CAMP_LOSE,//指定阵营出局
        TEAM_WIN,//指定队伍胜利
        TEAM_LOSE,//指定队伍出局

        SHOW_ADD_NOTICE,//显示附加的提示信息窗口
        SUM_T_POS,//在对应位置召唤对应队伍的对象
        SUM_T_POINT,//在对应索引位置召唤对应队伍的对象

        SET_TUTORIAL_DONE,//设置引导完成
        SHOW_TXT_NOTICE,  //展示文本提示信息
        TEAM_TRIGGER,   //队伍触发效果

        SHOW_TIP,       //展示对应的效果类型

        SET_ACTOR_V,
        CHG_ACTOR_V,        //修改AI状态值
        SET_SP_ACTOR_V,
        CHG_SP_ACTOR_V,     //修改AI状态值
        SET_BATTLE_V,       //设置战场状态值
        CHG_BATTLE_V,

        ADD_CAMP_RES,//增加阵营资源
        DEL_CAMP_RES,//减少阵营资源

        RMV_CONTROL,//移除控制状态
    }

    /******************
     * 提示类型
     **/
    public enum EWCGActorTipType
    {
        NONE,
        CLOCK,          //闹钟
        TIME_BAR,       //时间条
    }

    public enum EWCGAITriggerSpecialType
    {
        NONE,
        STATE_IDLE,//强制设置ai对象状态
        DIE,//死亡
        DRAW_GAME,//平局，游戏结束
        RESET_AI_TAR,// 重置AI目标对象
        REMOVE,//移除
        DIE_WOUT_T,//不触发死亡效果的死亡 
    }

    public enum EWCGTeamTriggerType
    {
        NONE,
        SUMMON, //队伍召唤
        EFFECT_ID,//选中位置触发对应效果列表
        COMPLETE_BATTLE_TASK,//指定PVE战斗内任务id的任务完成
    }

    //AI的条件类型
    public enum EWCGSingleConditionType
    {
        NONE,
        RANGE,//根据距离，关系和是飞行还是地面单位有效来判断是否有敌人在范围内
        AI_V_RNG,//ai对应索引值的范围(idx索引暂定0-99)，min,max如为-1表示无限制
        SK_OK,//根据指定技能id 冷却时间已经完成
        SK_IDX_OK,//根据技能位置索引，判断AI主体对应索引技能是否还在cd中
        RNG_S_COND,//根据与技能效果条件中相同格式的条件，对范围内对象进行判断
        RNG_NUM_S_COND,//根据距离和关系和条件判断范围内符合条件的关系对象数量
        RNG_D_COND,//根据与技能效果条件中相同格式的条件，对范围内对象进行判断
        RNG_NUM_D_COND,//根据距离和关系和条件判断范围内符合条件的关系对象数量
        P_RNG,//属性范围
        V_RNG,//状态值范围
        BUF_STK,//某个buffid的层数
        BUF_T_STK,//某个类型buff的层数
        SPECIAL,//特效条件
        SPE_TAG_T,//角色特殊标记   目标若是（XX里头的任何一个）则生效
        RACE_ID,//种族ID
        ACT_TYPE,//角色类型
        MOVE_TYPE,//移动类型
        TEAM_P_R,//队伍属性
        TEAM_V_R,//队伍状态值
        SPE_TAG_F,//角色特殊标记   目标若是（XX里头的任何一个）则不生效

        SELF_CNT,//判断单位数量
        RNG_FT,   //范围内存在其他队伍是敌对
        UNIT_TYP_T,//特殊标记符合判断
        UNIT_TYP_F,//特殊标记不符合判断
        ACT_ID, //判断单位的Id

        RALL,       //根据距离，关系和是飞行还是地面单位有效来判断是否有敌人在范围内,会检索无法索敌对象
        RALL_S_COND,//根据与技能效果条件中相同格式的条件，对范围内对象进行判断,会检索无法索敌对象
        RALL_NUM_S_COND,//根据距离和关系和条件判断范围内符合条件的关系对象数量,会检索无法索敌对象
        RALL_D_COND,//根据与技能效果条件中相同格式的条件，对范围内对象进行判断,会检索无法索敌对象
        RALL_NUM_D_COND,//根据距离和关系和条件判断范围内符合条件的关系对象数量,会检索无法索敌对象

        BUF_MAX,//某个buffid是否满层
        BUF_T_MAX,//某个类型buff是否满层

        V_RNG_S,//状态值范围高级处理方法
        AREA_RELATION,//判断当前所在区域的关系

        S_COMP,//高级公式比较
        TEAM_COND,//队伍条件
        SPE_TEAM_COND,//判断指定队伍的队伍条件

        OWNER_S_COND,//判断所有者的条件
        ACT_SID,//判断单位的SID
        ACTOR_OPERATION_FEASIBILITY,//Actor操作可行性

        RANGE_O,//在所有者周围，根据距离，关系和是飞行还是地面单位有效来判断是否有敌人在范围内
        RNG_S_O_COND,//在所有者周围，根据与技能效果条件中相同格式的条件，对范围内对象进行判断
        RNG_NUM_S_O_COND,//在所有者周围，根据距离和关系和条件判断范围内符合条件的关系对象数量
        RNG_D_O_COND,//在所有者周围，根据与技能效果条件中相同格式的条件，对范围内对象进行判断
        RNG_NUM_D_O_COND,//在所有者周围，根据距离和关系和条件判断范围内符合条件的关系对象数量
        BUF_SPE_TAG,//BUFF特殊标记
        DNG_SPE_TAG,//副本特殊标记
        S_RNG,//高级公式范围
        CTL_STAT,//控制状态
        BUF_STK_S,//某个buffid的层数高级条件方式
    }

    /*************
     * 特殊条件的类型
     **/
    public enum EWCGSpecialConditionType
    {
        NONE,
        GOD,//目前是无敌状态
        BKB,//目前不是魔免状态
        PHYS_IMM,//是否免疫物理攻击
        CONTROL,//是否被控制
        IS_OPING,   //是否正在进行3D操作
        HAS_P_BUF,  //有可偷取的buf

        IS_DEAD,        //是否死亡
        IS_SHARE_TEAM,  //是否共享队伍

        HAS_BUF,        //是否有buf
        HAS_DEBUF,      //是否有debuf
    }

    //AI的目标选择类型
    public enum EWCGAITarType
    {
        NONE,
        NEAR_TAR,//根据距离，关系和是否地面才有效选择最近的目标对象
        NEAR_TAR_S_COND,//在距离内，搜寻匹配关系和条件的最近单位
        NEAR_TAR_D_COND,//在距离内，搜寻匹配关系和条件的最近单位
        SPECIAL,// 进行特殊目标的处理   特殊目标如：SELF，SELF_POS
        NEAR_ALL,//根据距离，关系和是否地面才有效选择最近的目标对象 (范围为所有对象）
        NEAR_ALL_S_COND,//在距离内，搜寻匹配关系和条件的最近单位 (范围为所有对象）
        NEAR_ALL_D_COND,//在距离内，搜寻匹配关系和条件的最近单位 (范围为所有对象）
        SORT_TAR,// - 范围内根据排序方式最优先的对象
        SORT_TAR_S_COND,// - 范围内根据排序方式最优先单体条件对象
        SORT_TAR_D_COND,// - 范围内根据排序方式最优先双向条件对象
        SORT_ALL,// - 范围内根据排序方式最优先对象 (范围为所有对象）
        SORT_ALL_S_COND,// - 范围内根据排序方式最优先单体条件对象 (范围为所有对象）
        SORT_ALL_D_COND,// - 范围内根据排序方式最优先双向条件对象(范围为所有对象）
    }

    public enum EWCGTarInfoType
    {
        NONE,
        POS,
        INFO,
        ALL_INFO,
    }


    //AI的目标选择类型
    public enum EWCGAITarSpecialType
    {
        NONE,
        SELF,//选择自己
        SELF_POS,//自己所在位置
        OWNER,//ai本体召唤者
        BIRTH_POS,//出生点
        CACHE_POS,//缓存位置，默认为出生位置

    }

    //AI的行为类型
    public enum EWCGAIActionType
    {
        NONE,
        SKILL,//无技能等级则从技能集合中选择最高技能等级
        SKILL_IDX,// 从配置的技能Id集合中选择对应下标的技能Id进行释放
        SKILL_CD,//无技能等级则从技能集合中选择最高技能等级,判断是否在cd中，在则不释放，不在则释放
        SKILL_CD_IDX,//从配置的技能Id集合中选择对应下标的技能Id,判断是否在cd中，在则不释放，不在则释放
        SPECIAL,//特殊处理行为
        MOV_IDX,//根据配置的索引值，以及AI主体配置的坐标队列，移动到对应索引的坐标位置
        EFFECT,//对目标直接执行一个效果
        EFFECT_ID,//直接执行id列表
        PRINT,//输出日志
    }

    //AI的特殊行为类型
    public enum EWCGAIActionSpecialType
    {
        NONE,
        MOV_NEXT_POINT,//移动到下一个路点
        DEFAULT_ATK,//使用默认技能攻击
        DEFAULT_ATK_OK,//使用默认技能攻击(需要判断技能cd是否完成)
        MOV,//移动到对应目标位置
        RND_MOV_OWNER,  //围绕着所有者进行移动，在范围内则随机，范围外则向范围内移动
    }

    //角色移动类型
    public enum EWCGMoveType
    {
        NONE,
        GROUND,
        FLY,
    }


    //战斗资源类型
    public enum ENPResouceType
    {
        GOLD,//金币
        SCORE,//积分
        SOUL,//战魂
        ROUGH,//原石
    }

    //阵营资源类型
    public enum EWCGCampResouceType
    {
        SCORE,//积分
    }

    public enum EWCGHandleType
    {
        ADD_PRODUCE,//增加产出
        DEL_PRODUCE,//减少产出
        CREATE_UNIT,//创建单位
    }

    public enum ENPTeamPropertyType
    {
        NONE,
        COLLECT_PER,//(金币)采集资源加成,默认为0 万分比,公式为 现采集数量 = 原采集数量 * (1 + 采集资源加成/10000)
        PRODUCE_PER,//生产加速
        OPERATION_PER,//操作加速
        RECYCLE_PER,//操作加速
        TEAMSKILL_TIME_PER,
        TEAMSKILL_COST_PER,
        BATTLE_SPEED,//战斗速度加成
        ROUGH_COLLECT_PER,//(原石)采集资源加成,默认为0 万分比,公式为 现采集数量 = 原采集数量 * (1 + 采集资源加成/10000)
    }

    public enum EWCGTeamValueType
    {
        GOLD,//金币数量
        SCORE, //队伍积分
        TIME,   //开局时间
        ROUGH,//原石
        SOUL,  //战魂数量
        ID,//队伍Id

        TEAM_SKILL_NUM,     //指挥官技能数量
    }

    public enum EWCGTeamBoolType
    {
        IS_THERE_HERO_TB,//是否可以建造英雄
        IS_THERE_HERO_UP,//是否可以升级英雄
    }

    public enum EWCGCampPropertyType
    {
        NONE,
        BE_DEFEAT_TOWER_NUM,//被摧毁防御塔数量
        BE_DEFEAT_BASE_NUM,//被摧毁基地塔数量
    }

    /**************
     * 人口阶段枚举
     **/
    public enum EWCGPopulationStepType
    {
        NONE,
        LIGHT,      //轻度负载
        MID,        //中度负载
        HEAVY,      //重度负载
        NO_REWARD,  //无收获
    }

    //子弹类型
    public enum EWCGBulletType
    {
        NONE,
        LINE,//直线
        PARABOLA,//抛物线
        DIRECTION,//往一个方向的子弹
        BACKFORTH,//往复的子弹
    }

    public enum EWCGBuffType
    {
        NONE,
        BUFF,
        DEBUFF,
    }

    public enum EWCGOperationBattleTagType
    {
        NONE,
        BIG,
        SMALL,
    }

    public enum EWCGActorChgType
    {
        NONE,
        ADD,
        REMOVE,
        UPDATE,
    }

    public enum EWCGModelType
    {
        NONE,//没有模型的单位
        MODEL_2D,  //2d
        MODEL_3D, //3d
    }

    public enum EWCGHeroState
    {
        NONE,
        CAN_DISPATCH,   //已解锁,可派遣 -1
        DISPATCHED,     //已派遣，已登场战斗 -2  人口不在生产管理器中
        DEATH,          //可复活 -3
        RELIVE_ING,     //复活中 -4
        RECYCLE_ING,     //回收中  不可派遣- 5
    }

    //队伍战斗状态
    public enum EWCGTeamState
    {
        PREPARE,//准备
        FIGHTING,//战斗中
        WIN,//胜利
        LOSE,//失败
        DRAW,//平局
    }

    //队伍投降状态
    public enum EWCGTeamSurrenderState
    {
        NONE,//没投
        SURRENDER,//投降
        UNSURRENDER,//不投降
    }


    public enum EWCGPingState
    {
        Hight, //高Ping值
        Mid,//中Ping值
        Low,//低Ping值
    }

    //战场内提示类型
    public enum EWCGTipType
    {
        WARNING,//警告提示
        HYPERLINK,//超链接
    }

    //战场内事件类型
    public enum EWCGEventType
    {
        ATTACKED,//被攻击
        IMPORTANT_ATTACKED,//标记为重点单位被攻击
        MINERAL_OCCUPIED,//占领野矿成功
        VIEW_ENEMY,//发现敌人
        ALLY_ATTACKED,//友军受击
        HERO_KILLED,//英雄击杀
        HERO_DEAD,//英雄死亡
        HERO_UNLOCKED,//英雄解锁完成
        HERO_RELIVE,//我方英雄复活完成
        BUILDING_3DOPERATION,//建筑3Doperation操作完成
        UNIT_PRODUCED,//单位生产完成
        UNIT_RECYLE,//回收成功
        NORMAL_BEKILL,//带通知标记的普通单位被击杀
    }

    public class EWCGEventTypeComparer extends IEqualityComparer<EWCGEventType>
    {
        public boolean Equals(EWCGEventType x, EWCGEventType y)
        {
            return x == y;
        }

        public int GetHashCode(EWCGEventType obj)
        {
            return (int) obj.ordinal();
        }
    }

    //头顶操作条样式
    public enum EWCGOperationGridType
    {
        NORMAL,
        HERO,
    }

    public class EWCGOperationGridTypeComparer extends IEqualityComparer<EWCGOperationGridType>
    {
        public boolean Equals(EWCGOperationGridType x, EWCGOperationGridType y)
        {
            return x == y;
        }

        public int GetHashCode(EWCGOperationGridType obj)
        {
            return (int) obj.ordinal();
        }
    }

    //资源变化提示类型(飘金币)
    public enum EWCGResourceTipType
    {
        NORMAL,
        ADD,
        REDUCE,
        ADD_BAD,
    }

    //特殊伤害提示枚举类型
    public enum EWCGSpecialHurtTipType
    {
        CRIT,       //暴击
        DODGE,      //闪避
        BECrit,     //被暴击
        BEDODGE,    //被闪避
    }

    public enum EWCGCommandType
    {
        NONE,
        ACTIVE_SKILL,
        EFFECT,
        CLIENT_OP,
        ACTOR_OP,
        CANCEL_OP,
        TIMER_AI,
        SKILL_TO_ACTOR,
        SKILL_TO_POS,

        REPLACE_SEAT_ACTOR, //替换占位对象

        USE_HERO_SKILL,    //使用英雄技能

        SURRENDER_OP,    //发起投降
        ACCEPTSURRENDER_OP, //同意投降

        SHARE_ACTOR_OP,//共享3D操作
        CANCEL_SHARE_ACTOR_OP,//取消共享操作

        POS_EFFECT,//位置效果
        BATTLE_TEAM_ADD_CARD,//战斗阵容添加卡牌

        SURRENDER_DONE, //投降成功的处理
    }

    //战斗Bar上actor item 的UI状态
    public enum EWCGGUIBattleActorUIState
    {
        HIDE,   // 0
        ADD,     // 1
        IDLE,   // 2
        REMOVE, // 3
    }

    //目标类型枚举
    public enum AttackTargetType
    {
        GROUND,
        FLY,
        BUILDING,
    }

    //新手引导运动遮罩Element类型
    public enum TutorialMoveMaskElementType
    {
        TOP,
        BOTTOM,
        LEFT,
        RIGHT,
        MID,
    }


    //战斗单位属性面板显示的属性类型
    public enum EWCGActorPanelPropertyType
    {
        NONE,
        HP,
        HP_PER,
        SP,
        SP_PER,
        ATK,
        ATK_PER,
        POW,
        POW_PER,
        ARMOR,
        ARMOR_PER,
        MAGIC_ARMOR,
        MAGIC_ARMOR_PER,
        ARMOR_IGNORE,
        ARMOR_IGNORE_PER,
        MAGIC_ARMOR_IGNORE,
        MAGIC_ARMOR_IGNORE_PER,
        HEALED,
        HEALED_PER,
        DMG_REDUCE,
        DMG_REDUCE_RATE,
        PHYS_REDUCE,
        PHYS_REDUCE_RATE,
        MAGIC_REDUCE,
        MAGIC_REDUCE_RATE,
        ATK_SPEED,
        MOVE_SPEED,
        MOVE_SPEED_PER,
        HP_STEAL,
        HP_STEAL_PER,
        CRIT_V,
        CRIT_PER,
        CRIT_DEF_V,
        CRIT_DEF_PER,
        CRIT_DMG_PER,
        CRIT_DMG_DEF_PER,
        DODGE_V,
        DODGE_PER,
        HIT_V,
        HIT_PER,
        _ATTACK_INTERVAL,
        _ATTACK_RANGE,
        _TARGET_TYPE_LIST,

        SUMMON_TIME,        //召唤时间
        DEPLOY_TIME,        //放置时间

    }

    //血条样式枚举
    public enum EWCGBloodBarType
    {
        NONE,
        ARM_BLOOD,//我方普通兵种  血条
        BUILDING_BLOOD,//我方建筑  血条
        HERO_BLOOD,//我方英雄 血条
        FRI_ARM_BLOOD,//友方普通兵种  血条
        FRI_BUILDING_BLOOD,//友方建筑  血条
        FRI_HERO_BLOOD,//友方英雄 血条
        ENEMY_ARM_BLOOD,//敌方 普通兵种血条
        ENEMY_BUILDING_BLOOD,//敌方建筑血条
        ENEMY_HERO_BLOOD,//敌方英雄血条
    }

    public class EWCGBloodBarTypeComparer extends IEqualityComparer<EWCGBloodBarType>
    {
        public boolean Equals(EWCGBloodBarType x, EWCGBloodBarType y)
        {
            return x == y;
        }

        public int GetHashCode(EWCGBloodBarType obj)
        {
            return (int) obj.ordinal();
        }
    }

    //单位类型，本类型在受击提示中使用
    public enum EWCGActorEventType
    {
        NONE,
        NORMAL,//普通战斗单位
        IMPORTANT,//重点战斗单位
    }

    //小地图提示类型
    public enum EWCGMiniMapNoticeType
    {
        BUILDINGDONE,   //我方建筑建造完成
        MONSTERSHOW,    //出现野怪
        POINTGET,       //据点占领
        BUILDINGUPGRADE,//主城升级，建筑升级
        MINERGET,       //野矿占领
        MINERDONE,      //野矿建造完成
        FAMERDONE,      //农民生产完成
        HEROBUILDINGDONE,//英雄祭坛建造完成
        HERODEATH_ALLY, //我方英雄死亡
        HERODEATH_ENEMY,//敌方英雄死亡
        MONSTERSHOW_MINI,
        MONSTERSHOW_MID,
        MONSTERSHOW_BIG,
        HEROBIRTH,      //英雄出生
    }

    public class EWCGMiniMapNoticeTypeComparer extends IEqualityComparer<EWCGMiniMapNoticeType>
    {
        public boolean Equals(EWCGMiniMapNoticeType x, EWCGMiniMapNoticeType y)
        {
            return x == y;
        }

        public int GetHashCode(EWCGMiniMapNoticeType obj)
        {
            return (int) obj.ordinal();
        }
    }

    //资源统计数据查询类型
    public enum EWCGResourceDataAnalysisType
    {
        TOTALNUM,//总量
        CUR_NUM,//当前资源量
        CREEPSNUM,//野怪处取得资源
        UTILIZATION, //利用率
        ACTOR_ABOUT, //资源相关
    }

    //Actor统计数据查询类型
    public enum EWCGActorDataAnalysisType
    {
        //------单数据-------
        ACTOR_PRODUCE_NUM,//总生产量
        ACTOR_GOLD_COST,//金币花费
        ACTOR_ROUGH_COST,//原石花费
        HEAL,//治疗量

        //---------总伤害
        BEDODGE,//被闪避的伤害
        DODGE, //闪避掉的伤害

        CRIT,//造成的暴击伤害
        BECRIT,//被暴击承受的伤害
        NORMAL_ATTACK,//造成的普攻伤害
        BE_NORMAL_ATTACK, //被普攻承受的伤害

        //-----------实际输出
        PARRY_DAMAGE,//格挡的伤害
        BE_PARRY_DAMAGE,//被格挡的伤害

        SHIELD_OFFSET,//护盾抵消伤害
        BE_SHIELD_OFFSET,//被护盾抵消伤害

        REMOVE_BLOOD, //造成面板伤害
        BE_REMOVE_BLOOD, //被造成面板伤害

        //-----复合数据
        TOTALDAMAGE,//总输出
        DAMAGE,//输出

        TOTALINJURED,//总承伤
        INJURED,//承伤

        LEVEL,//等级
        ACTOR_RELIVE_NUM,//总复活数
    }

    public enum EWCGBattleClickType
    {
        LIFT,//抬起
        PRESS,//按下
    }

    //小地图的附属功能
    public enum EWCGMapRemark
    {
        NONE,
        SCALE,//缩放
        ATTACK,//攻击
        DEFENSE,//防御
        GOBACK,//撤退
    }

    public class EWCGMapRemarkComparer extends IEqualityComparer<EWCGMapRemark>
    {
        public boolean Equals(EWCGMapRemark x, EWCGMapRemark y)
        {
            return x == y;
        }

        public int GetHashCode(EWCGMapRemark obj)
        {
            return (int) obj.ordinal();
        }
    }

    //ActorLevelOperation的类别 以后应该还会新增
    public enum EWCGOperationType
    {
        NONE,
        BUILDING,//普通建筑
        HEROBUILDING,//英雄建筑
        NORMAL,//3D操作
        MAGIC,//指挥官技能
    }


    //材质类型
    public enum EWCGDefenseMaterialType
    {
        NONE,
        WOOD,//木头
        METAL,//金属
        BODY,//血肉
        STONE,//石头
    }

    public enum EWCGHitMaterialType
    {
        NONE,
        WOOD_HEAVY,//木头--重
        METAL_HEAVY,//金属--重
        STONE_HEAVY,//石头--重
        BODY_HEAVY,//血肉--重
        METALBLADE_HEAVY,//金属刀刃--重
        WOOD_LIGHT,//木头--轻
        METAL_LIGHT,//金属--轻
        STONE_LIGHT,//石头--轻
        BODY_LIGHT,//血肉--轻
        METALBLADE_LIGHT,//金属刀刃--轻
        BOW_LIGHT,//弓箭--轻
        BOW_HEAVY,//弓箭-重
        MAGIC_LIGHT,//魔法--轻
        MAGIC_HEAVY,//魔法--重
        CLAW_LIGHT,//爪击--轻
        CLAW_HEAVY,//爪击--重

    }

    //技能追踪方法
    public enum EWCGSkillTrackingMethod
    {
        NONE,
        POSITION,       //释放目标是位置
        DIRECTION,      //释放目标是方向，按照指定距离计算位置

    }

    //副本类型枚举
    public enum EWCGGUIDungeonType
    {
        RACE_TEST,//兵种测试副本
        PVE_TEST,//单人PVE副本
        QUALIFYING,//排位赛
        PVP,//匹配
        PVE_AI_TEST,//人机对战测试
        CUSTOM_BATTLE,//自定义战役
        NEW_PVE,
    }

    //窗口事件类型
    public enum WinMsgType
    {
        NONE,
        _DISABLE_1,
        _DISABLE_2,
        ON_LANGUAGE_CHAGE,//语言更改事件
        ON_SELECT_NUM_CHANGE,//选择数量变化
        CARD_ATLAS_CHIP_CHG,//购买碎片
        CARD_ATLAS_UNLOCK,//卡牌解锁
        CARD_ATLAS_STATE_CHANGE,//卡牌图鉴状态发生改变

        POP_UP_ITEM_NOT_ENOUGH,//弹窗提示物品不足
        GEM_COMMAND_CONSOLE_LOG_CHANGE,//作弊命令
        GET_KEY_DOWN_ENTER,

        ON_FRIEND_ONLINE_CHANGE,//好友在线状态改变
        ON_USER_STATE_CHANGE,//好友状态改变
        ON_FRIEND_INFO_CHANGE,//好友基础信息改变
        ON_ADD_FRIEND,//添加好友
        ON_ADD_APP,//添加申请
        ON_ADD_RECOMMAND,//添加推荐
        ON_REMOVE_FRIEND,//删除好友
        ON_REMOVE_APP,//删除申请
        ON_REMOVE_RECOMMAND,//删除推荐
        ON_SELECT_FRIEND,//设置选中好友
        ON_DIS_SELECT_FRIEND,//取消选中
        ON_FRIEND_OP_MODE_CHG,//好友操作模式改变
        ON_REQ_FRIEND_CHAT,//请求好友聊天数据回包
        ON_FRIEND_CHAT_NOTIFY,//好友新聊天通知

        ON_GRADES_INFO_UPDATE,//玩家段位信息更新
        ON_GET_SEASON_INFO,//获取到赛季信息
        ON_GET_SEASON_RANKING,//获取到赛季排行信息
        ON_PLAYER_NAME_CHANGE,//玩家改名
        ON_PLAYER_PARAM_CHANGE,//玩家信息变化
        ON_PLAYER_STATE_CHANGE,//玩家状态变化
        ON_PLAYER_SPEAKER_CHANGE,//玩家扬声器状态发生变化
        ON_PLAYER_MICRO_CHANGE,//玩家麦克风状态发生变化

        TRIGGER_TUTORIAL,//触发战斗外引导
        SET_TUTORIAL_DONE,//设置引导完成
        COMPLETE_ALL_TUTORIAL,//完成所有引导
        RESET_ALL_TUTORIAL,//重置所有引导
        QUIT_CURRENT_TUTORIAL,//退出当前引导

        CONTROL_TUROTIAL_SETP,//控制引导步骤

        ON_SELECT_NOTICE,//选择阅读公告

        ON_GET_BATTLE_RECORD,//获取战报成功

        ON_TASK_UPDATE,//任务更新

        ON_OPERATION_STATE_CHG,//操作状态改变
        ON_ACTOR_OPERATION_COMPLETE,//操作条完成
        ON_ACTOR_OPERATION_CANCEL,//操作取消

        ON_RET_PLAYER_GUILD_INFO,//获取公会数据
        ON_RET_UPDATE_GUILD_INFO,//更新公会数据
        ON_RET_QUERY_GUILD_LIST,//获取到推荐公会列表
        ON_RET_SEARCH_GUILD_LIST,//获取查找的公会列表
        ON_RET_REQUEST_JOIN_GUILD_LIST,//已经申请加入到公会列表的回包

        ON_QUIT_GUILD,//退出公会/被踢/解散
        ON_GUILD_MEMBER_LIST_CHANGE,//公会成员列表变化
        ON_GUILD_CHAT_LIST_CHANGE,//公会成员聊天列表变化
        ON_GUILD_APPLY_LIST_CHANGE,//公会申请列表列表变化

        //ON_APPLY_JOIN_GUILD_LIST_CHANGE,//发送申请加入的公会的列表发送变化
        ON_REQ_APPLY_JOIN_GUILD,//发送申请加入公会

        ON_REFRESH_GUILD_MEMBER,//刷新公会成员
        ON_MEMBER_ONLINE_STATE_CHANGE,//公会成员在线状态改变
        ON_MEMBER_USER_STATE_CHANGE,//公会成员战斗状态改变
        ON_GUILD_ICON_SELECT,//公会-设置图标
        ON_GUILD_NATIONAL_ICON_SELECT,//公会-设置国籍图标

        ON_GUILD_CREATE,//公会创建
        ON_GUILD_NAME_CHANGE,//改名
        ON_GUILD_ICON_CHANGE,//公会设置改图标
        ON_GUILD_NATIONAL_ICON_CHANGE,//改国籍图标
        ON_GUILD_JOIN_LIMIT_CHANGE,//改加入限制类型
        ON_GUILD_JOIN_CONDITION_CHANGE,//改加入条件类型
        on_GUILD_MANIFESTO_CHANGE,//改宣言
        ON_RET_COUNTER_LIST,//获取计数器列表
        ON_COUNTER_UPDATE,//计数器更新


        DAILY_FIRST_WIN_STATE_CHG,//每日首胜奖励状态改变

        DAILY_GRADE_REWARD_STATE_CHG,//每日段位奖励状态改变

        SUMMON_STONE_STATE_CHG,//召唤石状态变更

        SUMMON_STONE_TIME_LAPSE,//召唤石时间流逝

        SUMMON_STONE_SCORE_CHG,//召唤石战斗积分增加

        ON_RET_GUILD_DO_DONATE,

        ON_REC_CHAT_MSG, //收到全服聊天信息
    }

    /*****************
     * 引导触发的类型
     **/
    public enum EWCGTutorialTriggerType
    {
        NONE,
        RENAME_DONE,        //重命名结束
        MOVE_FOCUS_DONE,    //摄像头移动完成
        FAIL,//操作失败
        BATTLE_RESULT_DONE,//结算界面展示完毕
        CUSTOM_BATTLE_READY,//自定义房间引导触发
        SELECE_RACE_REWARD,//获取选择种族后的奖励
        CLOSE_LEVEL_UP,//关卡升级解锁窗口触发引导
    }

    public class WinMsgTypeComparer extends IEqualityComparer<WinMsgType>
    {
        public boolean Equals(WinMsgType x, WinMsgType y)
        {
            return x == y;
        }

        public int GetHashCode(WinMsgType obj)
        {
            return (int) obj.ordinal();
        }
    }

    //针对队伍定义的条件类型
    public enum EWCGTeamConditionType
    {
        NONE,
        TEAM_P_R,//队伍属性
        TEAM_V_R,//队伍状态值
        TEAM_S_R,//队伍中满足单体条件的Actor数量范围
        TEAM_S_COMP,//队伍高级公式变量比较
        TEAM_BATTLE_TASK,//队伍任务完成状态
        //== 0.36
        ALL_ACT_S_R,//所有单位中满足单体条件的数量范围
        SHARE_T_S_R,//共享组中的单位满足单体条件的数量范围
    }

    /****************
     * 教程获取对应信息窗口位置的处理
     **/
    public enum EWCGTutorialNoticeRectType
    {
        NONE,
        CARD_RECT,          //指定卡牌位置
        CARD_TYPE_RECT,     //指定卡牌类型的第一章卡牌的位置
        ATLAS_CARD_RECT,//卡牌池卡牌位置
    }


    //建筑类型
    public enum EWCGBuildingType
    {
        NormalBuilding,//普通建筑
        SoldierBuilding,//兵营建筑
        HeroBuilding//英雄建筑
    }

    /*************
     * 队伍的参数类型  , 高级计算公式类型
     **/
    public enum EWCGTeamVariableType
    {
        NONE,
        NUM,//单纯的数值
        TEAM_PRO,//队伍属性
        TEAM_VALUE,//队伍值
        RND,//随机数
        AB_VALUE,//AI中的某个状态值

        SPE_GROUP_PRO,   //指定队伍的属性值
        SPE_GROUP_VALUE, //指定队伍的状态值
        SPE_GROUP_RES,  //指定队伍的资源值
        CAMP_PRO,//阵营属性
        CAMP_RES,//阵营资源

        ALL_TEAM_VALUE,//所有队伍的指定类型数值的总值
        AVG_TEAM_VALUE,//所有队伍的指定类型数值平均值

        ALL_CAMP_VALUE,//所有陣營的隊伍指定類型數值的總值
        AVG_CAMP_VALUE,//所有陣營的隊伍指定類型數值的平均值

        ALL_TEAM_NUM,//本场战斗所有队伍数量

    }

    /*************
     * 玩家的参数类型  , 高级计算公式类型
     **/
    public enum EWCGPlayerVariableType
    {
        NONE,
        NUM,                    //指定数字
        VALUE,                  //指定类型的值，如等级等
        RND,                    //随机值
        PROPERTY,               //玩家属性
        VAR_V,                  //对应的动态值
        CARD,                   //卡牌
        BUF_L,                  //buf层级
        BUF_T,                  //buf时间
        ITEM_COUNT,                //指定物品类型数量
        ACT_STEP,               //当前活动步骤
    }

    /*****************
     * 玩家的值类型
     **/
    public enum EWCGPlayerValueType
    {
        NONE,
        LVL,                    //等级
        RMB_NUM,                //充值金额
        GEM_NUM,                //钻石数量
        GRADE,//段位
        MAXGRADES,//最高段位
    }

    /*****************
     * 玩家属性类型
     **/
    public enum EWCGPlayerPropertyType
    {
        NONE,
        EXP_BONUS_PER,          //属性加成百分比
        COIN_BONUS_PER,         //银币加成百分比
    }

    public enum EWCGTutorialEffectType
    {
        NONE,
        MOVE_FOCUS,//视野焦点缓慢移动
        SWITCH_FOCUS_IMMEDIATELY,//视野焦点快速跳转
        PLAY_MUSIC,//播放指定声音
        SPECIAL,//特殊效果

        BEGIN_DRAG, //拖拽开始
        ON_DRAG,  //拖拽
        DRAG_FAILURE,//拖拽失败后的处理
        DRAG_SUCCESS,//拖拽成功后的处理

        PUT_ACTOR,//指定位置派遣士兵
        PUT_HERO,//指定位置派遣英雄
        PUT_BUILDING,//指定位置放置普通建筑
        PUT_ACTOR_BUILDING,//指定位置放置士兵建筑
        PUT_HERO_BUILDING,//指定位置放置英雄建筑
        USE_TEAM_SKILL,//使用指定队伍技能
        USE_HERO_SKILL,//使用指定英雄技能
        LOCK_PLAYER_OPERATION,//锁定屏幕
        UNLOCK_PLAYER_OPERATION,//接触锁定屏幕
        MOVE_FOCUS_TO_HERO,//移动焦点到指定英雄处
        PERFORM_OPERATION,//指定单体条件的Actor执行指定Operation
        PERFORM_SHARD_OPERATION,//共享队伍指定单体条件的Actor执行指定Operation
        PRODUCE_ACTOR,//生产指定兵种
        MOVE_FOCUS_TO_OPERATION_BAR,//移动焦点到指定单位的OperationBar上
        MOVE_FOCUS_TO_SHARE_OPERATION_BAR,//移动焦点到指定单位的OperationBar上
        BEGIN_DRAG_SCREEN,   //开始拖拽屏幕
        DRAG_SCREEN,      //拖动屏幕
        DEAG_SCREEN_END,  //结束拖动屏幕
        CLICK_ARMS_UI,    //点击普通兵种卡牌UI
        SCALE_CAMERA,//设置正交视角大小
        CLICK_HERO_UI,//点击英雄卡牌UI
        CLICK_BUILDING_UI,//点击建筑卡牌UI

        //战斗外引导相关
        SET_TUTORIAL_DONE,//强制引导完成
        SWITCH_STAGE_SCENE,//挑转stage和scene
        SWITCH_STAGE_TEAM_SCENE,//挑转stage�uisceneTeam
        ENTER_PVE_LEVEL,//进入PVE关卡
        USE_CARD,//选择卡牌上阵
        ENTER_CUSTOM_READY,//进入自定义副本准备界面
        ENTER_CUSTOM_ROOM,//进入自定义副本房间
        ADD_CUSTOM_ROBOT,//添加自定义副本机器人
        SET_TRIGGER_WITH_ACTOR,//给指定Actor触发Trigger
        BATTLE_TEAM_ADD_CARD,//添加新卡牌到战斗阵容
        OPEN_EQUIT_TYPE_DETAIL,//打开挑战赛类型详情界面
        BEGIN_FIGHT_EQUIT_ARENA,//开始挑战赛
        OPEN_MATCHING,//打开搜索匹配界面
        OPEN_MATCH_SUCCEED_CASUAL,//打开休闲匹配成功界面
        OPEN_SUMMON_GOODS_BUY,//打开召唤商店购买弹窗
        CONFIRM_BUY_GOODS,//购买商品
        OPEN_TEAM_MEMBER,//打开队伍界面
        SWITCH_SHOP_TAB,//商店页签切换（适用于商店已经打开，切换页签）

        C_D_TASK_RECEIVE, //领取每日任务
        C_D_TASK_GO,//跳转到做对应每日任务的界面


        BATTLE_DONE_ENTER,//战斗结束并进入另一个战役- 枚举:是否胜利:下一个pve战斗Id
        ENTER_DUNGEON,//进入对应地下城
        RELIVE_HERO,//复活英雄


        C_BUY_GIFT,//客户端购买礼包处理
        C_USE_R_EFF,//客户端尝试使用远程效果
        C_GET_ACT_REW,//领取对应活动的奖励
        C_RECEIVE_SUMMON_REWARD,//领取召唤书奖励
        C_START_SUMMON_BOOK,//开始解锁消耗时间的召唤书
        C_OPEN_SUMMON_BOOK,//打开消耗时间的召唤书界面

        C_OPEN_CARD_INFO,//打开卡牌详情界面
        C_OPEN_CARD_SKILL_TIP,//打开卡牌技能详情界面
        C_OPEN_RESTRAIN_ATTACK,//打开某个卡牌的属性克制关系
        C_CUSTOM_ROOM_SELECT_DUNGEON,//创建自定义战斗房间时选择地图类型
        C_OPEN_SUMMON_BOOK_INDEX,// 打开哪一个位置的召唤书
        C_START_SUMMON_BOOK_INDEX,// 开始解锁哪一个位置的召唤书
        STOP_MUSIC,//停止音乐

        C_ENTER_PVE_CHAPTER,//进入某个种族的章节列表界面

        C_BATTLE_TASK_ADD_STAR,//pve 战斗内任务加星，加星只是客户端自己处理的，不需要服务器同步
        C_COMPLETE_BATTLE_TASK,//指定战斗内任务下标idx的任务完成
        CLICK_HERO_SKILL,//单击使用英雄技能
        C_CLICK_TEAM_SKILL,//点击指挥官技能
        C_SELECT_RACE,//引导选择某个种族


        //====================
        ADD_C,//增加计数,带计数类型
        ADD_C_FROM,//用参数值增加计数，带计数类型
        SET_C,//设置活动计数，带计数类型
        SET_C_FROM,//用参数值设置计数，带计数类型
        SET_C_FROM_GT,//当参数值更大是设置计数，带计数类型

        ADDFIX_VALUE,//增加参数修正器，按值修正
        ADDFIX_PERCENT,//增加参数修正器，按百分比修正
        ADDFIX_LIMIT,//增加参数修正器，限制最终值

        GET_REWARD, //获得指定rewardid的奖励
        GET_UNIFORM, //获得指定的物品
        CHOOSE_REWARD, //获得N选1的reward的奖励
        CHOOSE_UNIFORM, //获得N选1的物品

        SET_AUTO_UNLOCK_SUMMON, //设置自动解锁召唤效果


        S_ADD_BUF,// 服务器添加buf
        S_ADD_BUF_TIME,//服务器延长buf时间
        S_ADD_BUF_LAYER, // 服务器增加buf层级
        S_ADD_DAILY_BUF,//
        S_REMOVE_BUF,//
        S_REMOVE_BUF_LAYER,//
        S_SET_BUF_LAYER, // 服务器设置buf层级
        S_SET_BUF_LAYER_S, // 服务器设置buf层级支持高级计算公式

        S_TAKE_UNI_ITEM,

        S_GET_D_TASK,//给予每日任务
        S_D_TASK_DONE,//设置每日任务完成

        S_GIVE_SUMMON,

        GAIN_SKIN,//獲得皮膚
        GAIN_ICON_BG,//獲得頭像框


        S_S_EFFECT,// 直接在服务器执行server effect
        S_R_EFFECT,// - 直接在服务器执行remote effect

        S_COMPLETE_PVE_LEVEL,// 完成某个或者多个pve关卡，并设置星数
        S_RND_SE,//- 随机执行服务器效果
        S_RESET_ACT,//重置活动数据。
        S_GET_UNIFORM_S,// 获得指定的物品高级处理
        S_ACT_DONE,//设置活动已完成。
        S_GET_ICON,//获得头像
    }

    //触发效果的特殊效果的类型
    public enum EWCGTutorialEffectSpecialType
    {
        NONE,
        PAUSE_GAME,//暂停战场
        RECOVERY_GAME,//恢复战场
        WIN,//战斗胜利
        LOSE,//战斗失败
        GET_DAILY_VIC,//获取每日首胜奖励
        OPEN_SUMMON,//打开一个召唤书
        GUILD_CHECK,//进行公会签到

        REFRESH_DAILY_T,//刷新每日任务
        FORCE_CLOSE_LAST_NODE,//关闭最后一个节点
        DO_UI_ROLL_BACK,//UI 回退操作的处理

        OPEN_ALL_SUM_R,//开启所有召唤书奖励
        BATTLE_WIN_CONTINUE,//点击战斗胜利的继续按钮.
        BATTLE_SETTLE_REWARD_TO_MAIN,//战斗结算奖励界面返回主界面
        C_CREATE_CUSTOM_BATTLE_ROOM,//创建自定义战斗房间
        C_SET_TEAM_DONE,//设置阵容完成

        C_REFRESH_CP,//刷新condition prefab

        GET_VERSION_UP_REWARD,//领取版本更新奖励
        C_OPEN_RENAME,//打开玩家名修改界面
        C_CANCEL_SEARCH_MATCH,//取消搜索匹配
        C_CHANGE_HEAD_EDGE,//主界面打开更换头像框
        C_OPEN_SELECT_RACE,//打开种族选择界面
        C_BACK_MAIN_S,  //回到主界面
    }

    public enum EClientGSLoginType
    {
        NORMAL,
        WITH_RELOGIN_KEY,
    }

    //物品类型


    //物品使用类型
    public enum EWCGItemUseType
    {
        NONE,
        CHOOSE,
        REWARD,
    }

    //难度
    public enum EWCGDifficulty
    {
        NONE,
        EASY,       //简单
        NORMAL,     //普通
        CRAZY,      //疯狂
    }

    //聊天类型
    public enum EWCGChatType
    {
        NONE,
        Sys,        //系统
        ROOM,       //房间
        TEAM,       //队伍
    }

    public enum EWCGFriendChatSender
    {
        NONE,
        PLAYER, //自己发的
        FRIEND, //好友发的
    }

    /************
     * 奖励掉落类型
     * @author Administrator
     *
     */
    public enum EWCGRewardDropType
    {
        NONE,
        PRO, // 概率掉落，列表中格子随机获得
        WEI,// 权重掉落，列表中取一个
    }


    //触发效果的窗口拖拽类型
    public enum EWCGTutorialEffectDragWndType
    {
        NONE,
        ARM,//兵种
        HERO,//英雄
        BUILDING,//建筑
        HERO_SKILL,//英雄技能
        TEAM_SKILL,//队伍技能
    }

    //针对自动战斗定义的条件类型
    public enum EWCGTutorialConditionType
    {
        NONE,
        TUTORIAL_PLAYER_LEVEL,//玩家等级
        TUTORIAL_DONE_TUTORIAL_ID,//引导完成的id
        TUTORIAL_STAGE_SCENE,//在某个stage的某个scene
        CHANGE_NAME,//改名
        UNLOCK_FUNC,//解锁功能
        PLAYER_PARAM,//玩家身上的参数数值
        EVENT_PARAM,//事件参数
        COLLECT_CARD_COUNT,//拥有某品质的和等级卡牌的数量满足条件
        ACTIVITY_COUNTER,
        SPECIAL,//特殊
        EQUITABLE_FREE,//公平竞技是否免费
        C_CAN_GET_ACT_REW,//是否可以获得活动奖励
        P_V_RNG,//玩家值范围
        BUF_LAYER,//层数判断
        BUF_TIME,// BUF剩余时间（秒）判断
        GIFT_CAN_BUY,//是否可购买对应礼包
        CAN_USE_R_EFF,//是否可使用远程效果
        CARD_CAN_UPGRADE,//卡牌可升级
        CARD_NEW_UNLOCK,//新卡解锁
        C_LANG,//客户端语言判断
        C_NEW_VERSION,//客户端最新版本判断
        VARIA_RNG,//高级计算公式范围
        C_IN_ACT,//是否在活动中
        CLEAR_MAIN_VIEW_T,//判断当前节点是否是主界面(无其他弹窗)
        //===============================
        ASSIGN_DAY_OF_WEEK, //指定周天
        ACHIEVE_COUNTER,//成就
        C_PVE_LEVEL_PASSED,//某个PVE章节关卡通关
        SKIN_TIME,//皮肤剩余时间
        HAS_SKIN,//是否永久拥有列表中的所有皮肤

        C_PLAT,//客户端执行的平台
        C_P_NAME,//客户端包名判断函数.
        C_SELECT_RACE,//判断玩家的初始选择

        C_FUNC_ENABLE,//判断功能是否开启
        C_BUY_TIMES, // 商品购买次数的条件判断
        ACTIVITY_DONE_ID_LIST,//活动是否完成
        CARD_LEVEL,//玩家身上指定卡牌等级条件判断
        ACTIVITY_CAN_JOIN,//活动是否可以参加
        HAS_U_ITEM_C,//指定物品数量是否足够
        VARIA_RNG_S,//高级计算公式范围，高级用法
    }

    /**
     * 客户端执行平台
     */
    public enum EWCGClientPlat
    {
        NONE,
        IOS,
        ANDROID,
        PC,
    }

    //ui stage 类型
    public enum EWCGUIStageType
    {
        NONE,
        MAIN,
        BATTLE,
        PREPARE,
    }

    //ui scene 类型
    public enum EWCGUISceneType
    {
        NONE,
        EMPTY,
        BATTLE,
        CUSTOM_BATTLE,
        PVE_BATTLE,
        TEST_BATTLE,

        LOGIN,
        MAIN,
        FRIEND,
        QUALIFYING,
        REPLAY,
        SHOP,
        TEAM,
        TEAM_MEMBER,
        CARD_ATLAS,
        SDK,
        MATCH_CONFIRMED,

        LIVE,
        AUDIENCE,
        PREPARE,
        EDIT_QUICK_MESSAGE,//局内快捷消息编辑
        RANDOM_CARDS_TEAM,//随机卡牌选择界面

    }

    //客户端存储在服务器的数据类型
    public enum EWCGClientDataType
    {
        OUT_COMBAT_TUTORIAL,
    }

    //换位结束类型枚举
    public enum EWCGCusstomGameExchangeClose
    {
        CANCEL,     //取消换位
        AGREE,      //同意换位
        REJECT,     //拒绝换位
        TIMEOUT,    //超时
    }

    //功能类型
    public enum EWCGFunctionType
    {
        QUALIFYING,//排位赛
        QUALIFYING_1V1,//排位赛1v1
        QUALIFYING_2V2,//排位赛2v2
        CUSSTOM_BATTLE,//自定义战役
        PVE_BATTLE,//战役
        FRIEND,//好友
        SHOP,//商店
        CARD_ATLAS,//单位/卡牌池
    }

    /**************
     * 应用登录平台类型 注意与SDK的EWCGAppLoginPlatformType配置要一�每次改动需要同步wiki
     **/
    public enum EWCGAppLoginPlatformType
    {
        NONE,
        GUEST,
        VK,
        FACEBOOK,
        GOOGLE,
        GAMECENTER,
        WECHAT,
        QQ,
        XIANYU,
        UC,
        XIAOMI,
        QIHOO,
        YYBQQ,
        YYBWX,
        JGQQ,
        JGWX,
        HUAWEI,
        BAIDU,
        VIVO,
        LENOVO,
        OPPO,
        JINLI,
        COOLPAD,
        MZ,
        BILIBILI,
        DOUYU,
        WUFAN,
        M4399,
        ZHANQI,
        PPTV,
    }


    /**************
     * 分享平台类型 注意与SDK的EWCGAppSharePlatformType配置要一样,每次改动需要同步wiki
     * http://www.wcg.com/doku.php?id=wiki:wcg:shareplatformtype&do=edit
     **/
    public enum EWCGAppSharePlatformType
    {
        NONE,
        FACEBOOK,               //Facebook分享
        VK,                     //vk分享
        INSTAGRAM,              //instagram分享
        WECHAT_FRIEND,          //微信好友分享
        WECHAT_MOMENTS,         //微信朋友圈分享
        QQ_FRIEND,              //QQ好友分享
        SINAWEIBO,              //新浪微博分享
    }

    /**************
     * 操作系统类型
     **/
    public enum EWCGDeviceOSType
    {
        PC,
        Android,
        IOS,
    }

    //语言枚举 注意该枚举需要跟PHP的枚举对应，即PHP语言枚举�1=>"English",2=>"China",3=>"Taiwan",4=>"Russia",5=>"Korea",6=>"Japan",7=>"Arabic",8=>"Turkey",9=>"German",10=>"French",11=>"Spanish",12=>"Portuguese");
    public enum EWCGLanguage
    {
        NONE,
        EN_US,//英语(美国)
        ZH_CN,//2简体中文
        ZH_TW,//3中国台湾
        RU_RU,//4俄语(俄罗斯)
        KO_KR,//5韩国
        JA_JP,//6日文
        AR_AR,//7阿拉伯
        TR_TR,//8土耳其语
        DE_DE,//9德语
        FR_FR,//10法语
        ES_ES,//11西班牙语
        PT_PT,//12葡萄牙语
    }

    //窗口功能类型枚举
    public enum EWCGWndType
    {
        TECHNOLOGY, //科技面板
        BATTLE_MENU,//设置界面
        GOLD_ORE,  //金矿原石
        POPULATION,//人口
        MINIMAP_NOOP,//小地图
        MINIMAP_OP,//小地图操作部分
        SCORE,//积分展示部分
        PLAYER_SKILL,//指挥官技能
        CARD, //卡牌界面
        OPERATION,//3D操作部分
        HERO_OPERATION,//英雄操作部分
        AUTO_ADD_MAN,//自动补兵
        TECHNOLOGY_UI,//科技面板UI
        BATTLE_MENU_UI,//设置界面UI
        POPULATION_TIP,//人口提示
        OPERATION_PROGRESS_CLOSTBTN,//Operation的取消按钮
        BATTLE_TASK,//战场任务
        EXPRESSION,//表情按钮

        BUILDING_GRID,//建筑栏
        ARM_GRID,//兵种栏
        SPECIAL_GRID,//特殊单位栏

        QUICK_COMMUNICATION,//快捷交互
        BATTLE_TASK_GOAL,//PVE 战斗任务目标进度小窗口入口
    }

    //段位变化类型
    public enum EWCGGradeChange
    {
        NONE,//段位无变化
        UP,//升段
        DOWN,//降段
    }

    //段位变化时星的动画的类型
    public enum EWCGGradeStarAnim
    {
        SHOW,
        HIDE,
        UP,
        DOWN,
    }

    //公告内容类型
    public enum EWCGNoticeContentType
    {
        TEXT,//文字公告
        IMAGE,//图片公告
    }

    //公告显示位置类型
    public enum EWCGNoticeClientType
    {
        NONE,//0-显示在聊天和跑马灯窗口
        CHAT_WIN,//1-世界频道聊天窗口
        MARQUEE,//2-跑马灯
        TIP,//3-悬浮提示
    }

    //卡牌图鉴操作状态
    public enum EWCGCradItemState
    {
        NONE,
        CHIP_CAN_NOT_COMPOSITE,//碎卡不可合成
        CHIP_CAN_COMPOSITE,//碎卡可合成
        //WHOLE_CARD_ON_CARD_UP,//整卡升级中
        WHOLE_CARD_NOT_CARD_UP,//整卡可升级未升级
        WHOLE_CARD_CHIP_NOT_ENOUGH,//整卡碎片不够
        WHOLE_CARD_RES_NOT_ENOUGH,//整卡资源不够
        FULL_LEVEL,//满级
        WHOLE_CARD_ALL_NOT_ENOUGH,//整卡资源及碎片都不够
        //WHOLE_CARD_CARD_UP_COMPLETE,//整卡升级完成，等待确认升级
    }

    //精彩视频类型类型
    public enum EWCGLiveCompetitionType
    {
        NONE,
        ONE_TO_ONE,//1V1
        TWO_TO_TWO,//2V2
        THREE_TO_THREE,//3V3
    }

    //集锦条件类型
    public enum EWCElitConditionType
    {
        NONE,
        GRADE, //段位要求
        BATTLE_LAST,//战斗持续时长（秒）
        SECOND_INJURY,//综合秒伤
        ROOM_TYPE,//房间类型
        IS_CONTAIN_AI,//是否允许包含AI
    }

    public class EWCGLiveCompetitionTypeComparer extends IEqualityComparer<EWCGLiveCompetitionType>
    {
        public boolean Equals(EWCGLiveCompetitionType x, EWCGLiveCompetitionType y)
        {
            return x == y;
        }

        public int GetHashCode(EWCGLiveCompetitionType obj)
        {
            return (int) obj.ordinal();
        }
    }

    //战斗结束状态
    public enum EWCGBattleResult
    {
        WIN,//胜利
        LOSE,//失败
        DRAW,//平局
    }

    //ui view类型
    public enum EWCGUIViewType
    {
        NONE,
        DEFAULT,
        CUSTON_BATTLE_VIEWTYPE_READY,
        CUSTON_BATTLE_VIEWTYPE_SELECT,
        TEAM_MEMBER,
        QUALIFYING,
        PVE_BATTLE,
        LIVE,
        MATCH_CONFIRM,
        REPLAY,
        SDK,
        SHOP,
        TEAM,
        LOGIN,
        TEST_BATTLE,
        FRIEND,
        EMPTY,
        BATTLE,
        CARD_ATLAS,
        TASK,
        EMAIL,
        AUDIENCE,
        GUILD,//公会
        BOXSHOP,//抽卡（宝箱商店）
        EQUITABLE_ARENA,//公平竞技
        ACTIVITY,//活动
        EDIT_QUICK_MESSAGE,//局内快捷消息编辑
        CASUAL,//休闲玩法
        RANDOM_CARDS_TEAM,//随机卡牌选择界面
        PREFAB_SHOW,//仅展示加载的prefab，而不进行跳转（成长历程用）
        PREFAB_CARD,//展示卡牌详情页面，成长历程用
    }

    //战斗结果
    public enum EWCGCampBattleResults
    {
        NONE,
        WIN,
        LOSE,
        DRAW,
    }

    //任务状态枚举
    public enum EWCGTaskState
    {
        UNKNOWN,
        UNREACH, // 未达成
        REACH,   // 达成
        RECIEVED, // 已领取
    }

    //服务器状态
    public enum EWCGServerState
    {
        HIDE,       //隐藏
        MAINTAIN,   //维护
        FULL,       //爆满
        NEW,        //新服
        BUSY,       //繁忙
    }

    //回访类型
    public enum EWCGReplayType
    {
        NONE,
        RECORD,
        TV_LIVE,
    }

    //公会入会类型
    public enum EWCGGuildJoinLimit
    {
        NONE, //任何人都可以加入
        REVIEW, //需要审核
        REFUSE, //拒绝任何人加入
    }

    //公会日志基础类型（不需要额外记录其他数据的日志）
    public enum EWCGGuildLogBaseOpType
    {
        NONE,
        JOIN, //加入
        LEAVE,//离开
        DISBAND,//解散
    }

    //公会日志修改类型
    public enum EWCGGuildLogChgType
    {
        NONE,
        CHG_ICON, //公会图标
        CHG_JOIN_GRADES, //公会加入段位
        CHG_JOIN_LIMIT, //公会加入等级
        CHG_MANIFESTO, //公会宣言
        CHG_NAME, //公会名称
        CHG_NATIONAL, //公会国家
    }

    //Operation统计数据查询类型
    public enum EWCGOperationDataAnalysisType
    {
        OPERATION_GOLD_COST,//金币花费
        OPERATION_ROUGH_COST,//原石花费

        TIME,               //完成时间
    }


    public class EWCGMatchTypeComparer extends IEqualityComparer<EWCGCustomMatchType>
    {
        public boolean Equals(EWCGCustomMatchType x, EWCGCustomMatchType y)
        {
            return x == y;
        }

        public int GetHashCode(EWCGCustomMatchType obj)
        {
            return (int) obj.ordinal();
        }
    }

    public enum EWCGLockFuncType
    {
        NONE,
        CARD,//卡牌
        FRIEND,//好友
        BATTLE_RECORD,//战斗记录
        VIDEO,//精彩集锦
        SHOP,//商店
        DAILY_TASK,//每日任务
        PVE,//战役
        CUSTOM_1V1,//自定义战役
        QUALIFYING_1V1,//排位赛1v1
        QUALIFYING_2V2,//排位赛2V2
        QUALIFYING_3V3,//排位赛3V3

        HERO,//英雄
        HOUSE,//民居
        MINIMAP,//小地图
        WILD_MONSTER,//野怪
        STRONG_HOLD,//据点
        TECHNOLOGY,//科技面板
        POINT_TOWER,//积分塔
        CARD_UP,//卡牌升级
        BASE_LV2,//二级基地
        BASE_LV3,//三级基地
        MAGIC_SPY,//侦察
        WAR_FOG,//迷雾
        MAGIC_HOME,//回城
        CUSTOM_2V2,//组队对战
        GUILD,//公会
        CARD_CHEST,//抽卡
        EQUITABLE_ARENA,//竞技场
        ACTIVITY,//活动系统
        CASUAL_1V1,//休闲模式1v1
        CASUAL_2V2,//休闲模式2v2
        CASUAL_3V3,//休闲模式3v3

        ACHIEVEMENT,//成就
        MAGIC_CONVERGE,//集火
        MAGIC_GATHER,//集合
        GROWTH,//成长历程
        BAG,//背包
        FIRST_WIN,//首胜
        DAILY_BATTLE,//每日对战
    }

    public enum EWCGUnlockType
    {
        NONE,
        LOCK,//未解锁
        SHOW_LOCK,//显示未解锁
        FIRST_UNLOCK,//首次解锁
        UNLOCK,//解锁
    }

    public enum EWCGDeployBranchType
    {
        MAIN,//主分支
        DEPUTY,//副分支
        ALL,//全部随机
    }

    //自定义房间状态
    public enum EWCGCustomRoomState
    {
        FULL,//房间满，啥也干不了
        CAN_FIGHT,//可以参战
        CAN_OB,//可观战
    }

    //指挥官技能效果枚举枚举
    public enum EWCGTeamSkillEffectType
    {
        DEFAULT,
        INCREASE,
        REDUCE,
    }


    //野怪类型
    public enum EWCGMapCreeps
    {
        CREEPS_1,//小野怪
        CREEPS_2,//中野怪
        CREEPS_3,//大野怪
        CREEPS_4,//史诗野
        CREEPS_5,//公共野
        CREEPS_6,//大龙
    }


    public enum EWCGTeamSkillClass
    {
        ATK,//攻击型
        DEF,//防御型
        SUPPORT,//辅助型
        CONTROL,//控制型
    }

    public class EWCGTeamSkillClassComparer extends IEqualityComparer<EWCGTeamSkillClass>
    {
        public boolean Equals(EWCGTeamSkillClass x, EWCGTeamSkillClass y)
        {
            return x == y;
        }

        public int GetHashCode(EWCGTeamSkillClass obj)
        {
            return (int) obj.ordinal();
        }
    }


    //副本阵容保存方式
    public enum EWCGBattleTeamSaveType
    {
        READ_ONLY,//读取服务端保存的阵容，并且不保存客户端当前阵容
        READ_SAVE,//读取服务端保存的阵容，保存客户端当前阵容
        EMPTY_ONLY,//显示空阵容，不保存当前阵容(到服务器)
        CONFIG_ONLY,//直接读取配置，不保存
        CONFIG_SAVE,//直接读取配置，保存到服务器
    }

    //战斗内任务枚举
    public enum EWCGBattleTaskType
    {
        NONE,

        //实际任务枚举
        CREATE_ACTOR,//建造建筑
        PRODUCE_ARMY,//生产单位
        DISPATCH_ARMY,//派遣士兵
        OPERATION,//3D操作
        KILL_ENEMY,//击杀敌人
        HERO_SKILL,//英雄技能
        TEAM_SKILL,//使用魔法
        GET_SKILL,//拾取魔法
        RES_ENOUGH,//资源
        TEAM_V_RNG,//队伍属性
        PRODUCE_ARMY_DONE,//手动点击生成单位的数量
        //马甲任务枚举
        COMPETELE_RETASK,//完成前置任务
    }

    //BUFF特殊标记
    public enum EWCGBuffSpecialType
    {
        SHIELD,//护盾
    }

    //飞行对象类型
    public enum EWCGFlyGoType
    {
        TASK_ACTIVE,
        GOLD,
    }

    //公平竞技场挑战类型
    public enum EWCGEquitableArenaType
    {
        NONE,
        EQ_NORMAL,//普通挑战
        EQ_SUPER,//超级挑战
    }

    //宝箱类型 涉及UI
    public enum EWCGBoxType
    {
        NONE,               //其他类型
        BOX_GOLD,           //黄金宝箱
        BOX_EXTREME,        //至尊宝箱
        BOX_LEGEND,         //传奇宝箱
    }

    public enum EWCGChatUIView
    {
        NONE,
        QUALIFYING_CHAT,//排位赛匹配成功后聊天
        TEAM_CHAT,//多人组队时聊天
        CUSTOM_BATTLE_CHAT,//	自定义战役聊天
    }

    public enum EWCGEquitableArenaItemShowType
    {
        NONE,//无表现
        WIN,//加一胜利表现
        LOSE,//加一负场表现
    }

    //战场任务完成状态
    public enum EWCGBattleTaskState
    {
        NOT_IMPLEMENTED,//未进行
        ON_THE_WAY,// 进行中
        COMPELETE,//已完成
    }

    //卡牌选择模式
    public enum EWCGCardSelector
    {
        SELECT_IN_OWNED, //选择自己的卡片阵容，天梯和平衡竞技场都是这种
        SELECT_IN_ALL, //可以选择所有卡牌
        RANDOM_ALL,//系统一次性自动生成随机种族随机卡牌
        RANDOM_STEP, //系统逐步生成随机卡牌，3选一
    }


    //战斗内交流类型
    public enum EWCGBattleCommunicationType
    {
        FACE,//表情
        TEXT_MESSAGE,//文本消息
    }


    //战斗内快捷消息类型
    public enum EWCGQuickMessageType
    {
        ATTACK,//攻击类型
        DEFENSE,//防御
        COMMUNICATION,//其他交互
    }

    //局内快捷消息编辑状态
    public enum EWCGEditQuickMessageState
    {
        NORMAL,
        EDIT,
    }

    //每日首胜状态枚举
    public enum EWCGSkillRollingTimeType
    {
        NONE,
        HERO,
        NORMAL,
    }

    //玩家相关统计数据查询类型
    public enum EWCGBattlePlayerDataAnalysisType
    {
        ACTOR_KILL,//单位击杀
        HERO_KILL,//英雄击杀次数
        HERO_DIE,//英雄死亡次数
        KILL_DRAGON,//大龙击杀

        TOWER_SCORE,// 拆塔数
        BATTLE_SCORE,//战斗积分

        SKILL_BREAK_DOWN,//技能打断
        SKILL_BE_BREAK_DOWN,//技能被打断
    }

    //结算荣誉类型
    public enum EWCGBattleSettlementHonor
    {
        PUSH_TOWER,//推塔
        KILL_ALL,//总击杀人口
        KILL_HERO,//总英雄击杀
        HURT,//伤害
        DEFENSE,//防御
        RESOURCE,//资源
        HEAL,//资源
        MVP,//MVP
        RESOURCE_ROUGH,
        NONE_1,
        NONE_2,
        NONE_3,
        NONE_4,
        NONE_5,
        NONE_6,
        NONE_7,
        NONE_8,
        NONE_9,
        NONE_10,
    }

    //技能前摇展示样式类型枚举
    public enum ECardRandSelectStep
    {
        SELECTING_RACE,
        SELECTING_CARD,
        FINISHED,
    }


    //数据结算标签类别
    public enum DataAnalysisType
    {
        RESOURCE,  //资源
        HERO,  //英雄
        ARM, //兵种
        OPERATION,//科技
        BATTLE_DATA,//战斗关键数据
    }

    //召唤师状态枚举
    public enum EWCGSummonStoneState
    {
        NONE,
        EMPTY,
        WAIT_UNLOCK,
        UNLOCKING,
        WAIT_OPEN,
        PAUSE,

    }

    //地图特殊类型标记
    public enum EWCGDungeonSpecialType
    {
        ONE_V_ONE,
        TWO_V_TWO,
        THREE_V_THREE,
    }

    public enum EWCGActivitySystemType
    {
        TIME_LIMIT,//限时
        ACHIEVEMENT,//成就
    }

    public enum EWCGAchieveType
    {
        NONE,
        COMMON,//通用
        NE,//暗夜
        ORC,//兽族
        UD,//不死
    }

    public enum EWCGCounterType
    {
        ACTIVITY, //活动计数
        TASK,//任务计数
        GROWTH,//成长计数
        ACHIEVE,//成就计数
    }

    //匹配确认提示类型
    public enum EWCGCheckReadyFaildTipType
    {
        NONE,
        SELF,
        FIREND,
        ENEMY,
    }

    /**
     * 每日首胜状态枚举
     * @author mj
     */
    public enum EWCGDailyFirstWinState
    {
        NONE,
        NO_FINISHED, //未完成
        FINISHED, //已完成
        REWARDED, //已领奖
    }

    public enum EWCGSettingsType
    {
        NONE,
        BASIC, // 基础设置
        PUSH, // 推送设置
        QUICK_MSG, // 快捷消息设置
        VIDEO, // 录像设置
        USER_CENTER, // 用户中心
    }

    public enum EWCGTutorialConSpecialType
    {
        SUMMON_EMPTY,// 无召唤书
        NO_SUMMON_OPEN,// 无召唤书开启状态
        SUMMON_FULL,//  召唤书位置已满
        CAN_STORY,// 是否有战役可挑战
        CAN_GET_VIC,//  是否可获取首胜奖励
        HAS_GET_VIC,// 是否已经领取了首胜奖励
        HAS_GET_GRADE_D,//  是否已经领取了每日段位奖励
        HAS_ACTIVITY,//是否有参与任意活动

        DAILY_TASK_NEW,//新任务可领取
        DAILY_TASK_REACH,//任务已达成，可领取奖励
        DAILY_TASK_ACTIVE_BOX_REACH,//每日任务活跃度宝箱可以领奖
        CARD_CAN_UPGRADE,//卡牌可升级
        CARD_NEW_UNLOCK,//新卡解锁
        HAS_GUILD_CHECK,//是否在公会签到

        EQUITABLE_ARENA_FIGHTING,//挑战赛进行中
        EQUITABLE_REWARD,//挑战赛有奖励可领取
        C_NEW_VERSION,//是否可以领取版本更新奖励
        FALSE,//永远不通过
        C_RECHARGE_FUNCTION,//充值功能
        IS_IDLE, //是否在匹配过程中
    }

    public enum EWCGTutorialConPVRngType
    {
        DAILY_T_C,// 每日任务数
        DAILY_T_RESET_T,//每日任务刷新次数
        SUMMON_C,//召唤书个数
        ACHIEVE_P,//总成就点数
        SUMMON_ALL_T,//召唤书的总开启时间
    }

    // 统计类型枚举
    public enum EBattleStastic
    {
        LADDER_ONE2ONE_ALL, //1V1排位全部数据
        LADDER_TWO2TWO_ALL,//2V2排位全部数据
        LADDER_THREE2THREE_ALL,//3V3排位全部数据
        LADDER_THREE2THREE_ORC,//3v3排位兽族
        LADDER_THREE2THREE_ELF,//3v3排位精灵
        LADDER_THREE2THREE_DEATH,//3v3排位不死
    }

    // 玩家详情页面页签分类
    public enum EPlayerDetailsTabType
    {
        NONE,
        SEASON_RECORD, // 赛季战绩
        LINEUP, // 常用阵容
        RACE_WIN_RATE, // 种族胜率
        GIFT_KEY, // 兑换礼物
    }

    //推送时间类型
    public enum EWCGLocalPushTimeType
    {
        FIXED,//固定时间点
        DYNAMIC,//变化时间点的
        SERVER,//服务端控制的
    }

    //推送内容类型（唯一）
    public enum EWCGLocalPushContentType
    {
        NONE,
        ALL,//全部
        ACTIVITY_OPEN,//活动开启
        ACTIVITY_OVER,//活动结束
        SUMMON_WAIT_OPEN,//召唤石待开启
        SUMMON_WAIT_UNLCOK,//召唤石待解锁
        SEASON_OVER,//赛季结束
        SEASON_NEW,//赛季开始
        DAILY_SHOP_REFRESH,//每日商店刷新
        GUILD_DONATE,//公会可请求捐献
        DAILY_TASK,//每日任务
    }

    //举报类型
    public enum EReportType
    {
        NONE,
        HOOK,//挂机
        NEGATIVE,//消极比赛
        SNAP,//谩骂
        CHEAT,//作弊
        ESCAPE,//逃跑
        NOOB,//玩家水平过低
        OTHER,//其他
    }

    //指挥官技能标记类型
    public enum EWCGTeamSkillSignType
    {
        NONE,
        SPY,//侦察
        BACK,//回城
        FIRE,//集火
        MOVE,//集合
    }

    //分享内容类型
    public enum EWCGShareType
    {
        NONE,
        SHARE_URL,//分享链接
        SHARE_IMAGE_CAPTURE,//分享当前截屏图片
        SHARE_IMAGE_LOCAL,//分享本地图片
    }

    // PVE章节滑动方向
    public enum EWCGPVEChapterSlide
    {
        LEFT_TO_RIGHT,
        RIGHT_TO_LEFT,
    }

    //聊天内容样式
    public enum EWCGChatContentAssetType
    {
        NONE,//默认样式
    }

    //好友关系
    public enum EWCGPlayerFriendType
    {
        NONE,
        IS_APPLYED,//已经申请
        IS_FRIEND,//已经是好友
    }

    public enum EWCGChatMsgFlag
    {
        NONE,
        SMALL_HORN, //小喇叭
        BIG_HORN,//大喇叭
        SYS,//系统消息
    }

    /**
     * 聊房间类型
     * @author mj
     */
    public enum EWCGSessionType
    {
        WORLD,
    }

    /**
     * 消息类型
     * @author mj
     */
    public enum EWCGMsgType
    {
        TEXT,//文字
        VOICE,//语音
    }

    /**
     * 展示效果类型
     * @author mj
     */
    public enum EWCGGainRewardSource
    {
        SELECT_RACE,
        TASK_REWARD_RECEIVE,
    }

    public enum SelectMode
    {
        NONE,
        HERO,
        CONDTION,
    }

    //战斗结算交互类型
    public enum EWCGBattleSettlementInteractive
    {
        ADD_FRIEND,//加好友
        REPORT,//举报
        //LIKE,//点赞
    }

    public enum EWCGWildAudioType
    {
        BG_MUSIC,   //野生背景音乐
        NORMAL_MUSIC,//野生音效
    }

    //活动类型
    public enum EWCGActivityType
    {
        NONE,//无类型活动，不会显示在UI上
        NORMAL,//普通活动
        GIFT,//礼包活动
        CUSTOM,//自定义活动
    }

    //PHP登录操作类型
    public enum EWCGPHPLoginOperationType
    {
        NONE,
        LOGIN,              //登录
        REGISTERED,         //注册
        BIND,               //绑定
        UNBIND,             //解绑
        FORCEBIND,          //强制绑定
        RESETGUESTLOGIN,    //重置游客账号数据
    }

    public enum EWCGOnlineLogType
    {
        ALL,
        NATION,
        ADFROM,
    }

    /***************
     * 战斗房间类型
     * @author Administrator
     *
     */
    public enum EWCGCardChooseStat
    {
        NOT_GAIN, //未获得
        NOT_CHOOSED,//没有被使用
        CHOOSED,//已选择使用
    }
}
