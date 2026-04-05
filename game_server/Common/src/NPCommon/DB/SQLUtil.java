/**
 * License THE WORK (AS DEFINED BELOW) IS PROVIDED UNDER THE TERMS OF THIS
 * CREATIVE COMMONS PUBLIC LICENSE ("CCPL" OR "LICENSE"). THE WORK IS PROTECTED
 * BY COPYRIGHT AND/OR OTHER APPLICABLE LAW. ANY USE OF THE WORK OTHER THAN AS
 * AUTHORIZED UNDER THIS LICENSE OR COPYRIGHT LAW IS PROHIBITED.
 * <p>
 * BY EXERCISING ANY RIGHTS TO THE WORK PROVIDED HERE, YOU ACCEPT AND AGREE TO
 * BE BOUND BY THE TERMS OF THIS LICENSE. TO THE EXTENT THIS LICENSE MAY BE
 * CONSIDERED TO BE A CONTRACT, THE LICENSOR GRANTS YOU THE RIGHTS CONTAINED
 * HERE IN CONSIDERATION OF YOUR ACCEPTANCE OF SUCH TERMS AND CONDITIONS.
 */
package NPCommon.DB;

import java.sql.Connection;
import java.sql.ResultSet;
import java.sql.Statement;

public class SQLUtil
{

    public static Exception close(java.lang.AutoCloseable rs)
    {
        try
        {
            if (rs != null)
            {
                rs.close();
            }
        } catch (Exception e)
        {
            return e;
        }
        return null;
    }

    public static void close(ResultSet rs, Statement pstm, Connection con)
    {
        close(rs);
        close(pstm);
        close(con);
    }
}
