package NPUSServer.NPUSUserMgr.UserComp.RedDotComp;

import Common.Common_RedDotInfo;
import CommonEnum.ERedDotType;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPCommon.Util.Pair.WCGPair;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_007_CommOp;

import java.util.ArrayList;
import java.util.List;

/**
 * 红点组件 - 管理玩家所有红点状态
 * <p>
 * 主要功能：
 * 1. 维护当前激活的红点集合（内存存储）
 * 2. 提供红点的增删查接口
 * 3. 管理红点处理器（支持扩展）
 * 4. 自动推送红点状态变化到客户端
 * <p>
 * 设计特点：
 * - 数组索引模式：通过枚举ordinal()快速访问处理器
 * - 扩展性：新增红点类型只需添加枚举和处理器
 * - 轻量级：只在内存维护，无数据库开销
 * - 自动推送：状态变化时自动通知客户端
 * <p>
 * 线程安全：所有公开方法都使用玩家锁保护
 */
public class RedDotComponent extends _ANPUserComponent
{

    // 红点处理器数组（通过枚举ordinal索引）
    private _ARedDotDealer[] _m_dealerList;

    // 当前激活的红点集合
    private List<RedDotInfo> _m_redDotList;

    /**
     * 构造函数 - 初始化组件和注册处理器
     * @param _userData 玩家数据对象
     */
    public RedDotComponent(NPUSUserData _userData)
    {
        super(_userData, ENPPlayerCompType.RED_DOT_COMP);

        // 初始化处理器数组
        _m_dealerList = new _ARedDotDealer[ERedDotType.ERedDotType_Length];

        // 初始化红点集合
        _m_redDotList = new ArrayList<>();

        // 注册红点处理器
        //regDealer(new RedDotDealer_GuildGreatReward(this));
    }

    /**
     * 注册红点处理器
     * @param _dealer 处理器实例
     */
    private void regDealer(_ARedDotDealer _dealer)
    {
        _m_dealerList[_dealer.getRedDotType().ordinal()] = _dealer;
    }

    /**
     * 获取红点处理器
     * @param _type 红点类型
     * @return 对应的处理器，如果未注册则返回null
     */
    public _ARedDotDealer getDealer(ERedDotType _type)
    {
        if (_type == null || _type == ERedDotType.NONE)
        {
            return null;
        }
        return _m_dealerList[_type.ordinal()];
    }

    /**
     * 查找红点信息
     * @param _type
     * @return
     */
    public RedDotInfo lookupRedDotInfo(ERedDotType _type)
    {
        for (RedDotInfo redDotInfo : _m_redDotList)
        {
            if (redDotInfo.getRedDotType() == _type)
            {
                return redDotInfo;
            }
        }
        return null;
    }

    /**
     * 移除红点
     * <p>
     * 执行流程：
     * 1. 检查参数有效性
     * 2. 加锁保护并发访问
     * 3. 从激活集合移除
     * 4. 推送到客户端
     * @param _type     红点类型
     * @param _needPush
     */
    public void removeRedDot(ERedDotType _type, boolean _needPush)
    {
        if (_type == null || _type == ERedDotType.NONE)
        {
            return;
        }

        getUserData().lockUser();
        try
        {
            RedDotInfo redDotInfo = lookupRedDotInfo(_type);
            if (redDotInfo == null)
                return;

            // 如果红点存在且被移除，推送到客户端
            if (_m_redDotList.remove(redDotInfo)&& _needPush)
            {
                getUserData().pushMsgToGC(US2GCWriter_007_CommOp.make_060_PushRedDotRemove(_type));
            }
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 清除红点（客户端已读后调用）
     * @param _type 红点类型
     */
    public void clearRedDot(ERedDotType _type, boolean _needPush)
    {
        removeRedDot(_type, _needPush);
    }

    /**
     * 批量清除红点
     * @param _types 红点类型列表
     */
    public void clearRedDots(List<ERedDotType> _types, boolean _needPush)
    {
        if (_types == null || _types.isEmpty())
        {
            return;
        }

        for (ERedDotType type : _types)
        {
            clearRedDot(type, _needPush);
        }
    }

    /**
     * 获取所有激活的红点（用于初始化协议）
     * @return 红点类型列表
     * <p>
     * 线程安全：使用玩家锁保护
     */
    public List<Common_RedDotInfo> getAllRedDots()
    {
        getUserData().lockUser();
        try
        {
            List<Common_RedDotInfo> list = new ArrayList<>();
            for (RedDotInfo redDotInfo : _m_redDotList)
            {
                list.add(redDotInfo.makeRedDotInfo());
            }
            return list;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 通过处理器检查并更新红点状态
     * <p>
     * 执行流程：
     * 1. 获取对应的处理器
     * 2. 调用处理器的checkRedDot()方法
     * 3. 获取当前红点状态
     * 4. 对比状态并自动推送变化：
     * - 应该显示但当前没有 → 推送添加
     * - 不应该显示但当前有 → 推送清除
     * - 状态一致 → 不推送
     * @param _type     红点类型
     * @param _needPush
     */
    public void checkAndUpdateRedDot(ERedDotType _type, boolean _needPush)
    {
        _ARedDotDealer dealer = getDealer(_type);
        if (dealer == null)
        {
            return;
        }

        // 通过处理器检查红点是否应该显示
        WCGPair<Boolean, Long> showInfo = dealer.checkRedDot();
        if (showInfo == null || !showInfo.first)
        {
            removeRedDot(_type, _needPush);
        }else
        {
            RedDotInfo redDotInfo = lookupRedDotInfo(_type);
            if (redDotInfo == null)
            {
                // 红点不存在，创建新红点
                redDotInfo = new RedDotInfo(this, _type, showInfo.second);
                _m_redDotList.add(redDotInfo);
            } else
            {
                // 红点已存在，更新过期时间
                redDotInfo.setExpireTimeMs(showInfo.second);
            }

            // 推送红点添加或更新到客户端
            if (_needPush)
                getUserData().pushMsgToGC(US2GCWriter_007_CommOp.make_059_PushRedDotChg(redDotInfo.makeRedDotInfo()));
        }
    }

    /**
     * 批量检查并更新所有已注册的红点
     */
    public void checkAndUpdateAllRedDots(boolean _needPush)
    {
        for (_ARedDotDealer dealer : _m_dealerList)
        {
            if (dealer != null)
            {
                checkAndUpdateRedDot(dealer.getRedDotType(), _needPush);
            }
        }
    }

    @Override
    protected void _init()
    {
        // 内存维护，无需从数据库加载数据
        // 直接标记为初始化完成
        setInited();
    }

    @Override
    public ENPPlayerCompType[] getDependCompList()
    {
        // 声明依赖的组件
        return null;
    }

    @Override
    public void onInited()
    {
        // 首次检查所有红点状态
        checkAndUpdateAllRedDots(false);
    }

    @Override
    public void dispose()
    {
    }
}
