package NPGameRes.GameObjs.Battle;

import WCGCommon.Enum.NPEnum.EWCGCampPropertyType;

import java.util.ArrayList;
import java.util.LinkedList;
import java.util.List;

/*************
 * 属性修改的管理对象
 **/
public class WCGCampPropertyChgRecorder
{
    /**
     * 修改的属性列表
     */
    private LinkedList<EWCGCampPropertyType> _m_lChgPropertyList;

    /**
     * 属性修改状态标记
     */
    private List<Boolean> _m_lPropertyChgStatList;

    public WCGCampPropertyChgRecorder()
    {
        _m_lChgPropertyList = new LinkedList<EWCGCampPropertyType>();
        _m_lPropertyChgStatList = new ArrayList<Boolean>();

        //逐个设置初始值
        for (int i = 0; i < EWCGCampPropertyType.values().length; i++)
        {
            _m_lPropertyChgStatList.add(false);
        }
    }

    /*************
     * 添加修改的属性
     *
     * @author alzq.z
     * @time May 8, 2013 1:51:51 AM
     */
    public void addPropertyChg(EWCGCampPropertyType _type)
    {
        //设置属性被修改
        _m_lPropertyChgStatList.set(_type.ordinal(), true);

        //娣诲姞璇ュ睘鎬у埌宸茬粡鏇存敼灞炴�т腑
        _m_lChgPropertyList.addLast(_type);
    }

    /**************
     * 取出修改的属性
     *
     * @author alzq.z
     * @time May 8, 2013 1:53:44 AM
     */
    public EWCGCampPropertyType popChgProperty()
    {
        EWCGCampPropertyType chgType;

        while (true)
        {
            if (_m_lChgPropertyList.size() <= 0)
                return EWCGCampPropertyType.NONE;

            //取出第一个属性对象
            chgType = _m_lChgPropertyList.peek();
            _m_lChgPropertyList.removeFirst();

            if (_m_lPropertyChgStatList.get(chgType.ordinal()))
            {
                //设置未修改
                _m_lPropertyChgStatList.set((int) chgType.ordinal(), false);

                return chgType;
            }
        }
    }
}
