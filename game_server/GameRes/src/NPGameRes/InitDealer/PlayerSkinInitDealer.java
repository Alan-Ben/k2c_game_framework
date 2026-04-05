package NPGameRes.InitDealer;

import NPCommon.Log.CommLog;
import NPGameRes.GameObjs.CommonObj.LevelObj._TLevelMapMgr;
import NPGameRes.Refs.PlayerSkin.RefPlayerSkin;
import NPGameRes.Refs.PlayerSkin.RefPlayerSkinLevel;

import java.util.HashMap;
import java.util.List;
import java.util.Map;

/**
 * 初始化玩家皮肤对象
 */
public class PlayerSkinInitDealer extends _ABasicInitDealer
{
    @Override
    public void dealInit()
    {
        // 预整理：按 player_skin_id 分组皮肤等级数据
        Map<Long, _TLevelMapMgr<RefPlayerSkinLevel>> skinLevelMap = new HashMap<>();
        List<RefPlayerSkinLevel> skinLevelList = RefPlayerSkinLevel.getMgr().getList();
        for (int i = 0; i < skinLevelList.size(); i++)
        {
            RefPlayerSkinLevel ref = skinLevelList.get(i);
            if (null == ref)
                continue;
            if (null == RefPlayerSkin.getMgr().get(ref.player_skin_id))
            {
                CommLog.error("PlayerSkinInitDealer.dealInit - player skin ref not found: player_skin_id={}, skin_level={}", ref.player_skin_id, ref.skin_level);
                continue;
            }
            skinLevelMap.computeIfAbsent(ref.player_skin_id, k -> new _TLevelMapMgr<>())._initAddLevelData(ref);
        }

        // 统一赋值
        List<RefPlayerSkin> skinList = RefPlayerSkin.getMgr().getList();
        for (int i = 0; i < skinList.size(); i++)
        {
            RefPlayerSkin ref = skinList.get(i);
            if (null == ref)
                continue;
            _TLevelMapMgr<RefPlayerSkinLevel> mgr = skinLevelMap.get(ref.id);
            ref.setLevelMapMgr(mgr != null ? mgr : new _TLevelMapMgr<>());
        }
    }
}
