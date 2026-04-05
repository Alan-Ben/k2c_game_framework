package NPUSServer.UserOfflineTmpDataMgr;

/**
 * 每个橘色的离线临时数据对象，仅支持查询
 */
public interface _IUserOfflineTmpDataInfo {
    /**
     * 返回数据Id，用于在管理器中校验数据匹配
     * @return
     */
    long getDataId();
}
