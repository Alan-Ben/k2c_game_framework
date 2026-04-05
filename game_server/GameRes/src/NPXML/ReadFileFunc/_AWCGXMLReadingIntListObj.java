package NPXML.ReadFileFunc;

import java.util.ArrayList;


/*****************
 * 基本读取操作函数部分
 *
 * @author alzq.z
 * @email zhuangfan@vip.163.com
 * @time Aug 12, 2013 10:22:21 PM
 */
public abstract class _AWCGXMLReadingIntListObj extends _AWCGXMLReadingObj<ArrayList<Integer>>
{
    /**
     * 标识名称
     */
    private String _m_sNodeName;

    public _AWCGXMLReadingIntListObj(String _nodeName)
    {
        _m_sNodeName = _nodeName;
    }

    public String getNodeName()
    {
        return _m_sNodeName;
    }
}
