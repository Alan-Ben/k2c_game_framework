/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package NPCommon.DB;

import java.io.PrintWriter;
import java.sql.Connection;
import java.sql.SQLException;

public interface IDBConnectionFactory
{

    Connection getConnection();

    PrintWriter getLogWriter() throws SQLException;

    void setLogWriter(PrintWriter out) throws SQLException;

    void shutdown();

    String getCatalog();

    int getUsedConnectCount();

}
