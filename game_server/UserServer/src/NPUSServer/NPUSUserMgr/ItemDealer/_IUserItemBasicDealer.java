package NPUSServer.NPUSUserMgr.ItemDealer;

import NPEnum.ENPItemType;
import NPUSServer.Common.Context.NPPlayerContext;

/*****************
 * 通用的Item需要的处理类
 * @author mj
 *
 */
public interface _IUserItemBasicDealer
{
    /**********
     * 对应处理的类型
     * @return
     */
    ENPItemType getItemType();

    /*************
     * 获取数量
     * @param _itemId
     * @return
     */
    long getItemCount(long _itemId);

    /***************
     * 是否有足够的数量
     * @param _itemId
     * @param _count
     * @return
     */
    boolean hasItem(long _itemId, long _count);

    /****************
     * 初始化的获取物品处理
     * @param _itemId
     * @param _count
     * @param _context
     * @return
     */
    void initGainItem(long _itemId, long _count, NPPlayerContext _context);

    /****************
     * 获取物品的处理，返回值表示是否触发gameevent事件
     * @param _itemId
     * @param _count
     * @param _isNotMerge
     * @param _context
     */
    void gainItem(long _itemId, long _count, boolean _isNotMerge, NPPlayerContext _context);
    
    /****************
     * 消耗物品的处理
     * @param _itemId
     * @param _count
     * @param _context
     * @return
     */
    boolean spendItem(long _itemId, long _count, NPPlayerContext _context);
}
