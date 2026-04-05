package NPCommon.Http.HttpService.server;

import com.sun.net.httpserver.spi.HttpServerProvider;

import java.io.IOException;
import java.net.InetSocketAddress;

public class NPHttpService
{
    public static com.sun.net.httpserver.HttpServer createServer(int port, HttpDispather handler, String path) throws IOException
    {
        com.sun.net.httpserver.HttpServer httpserver = null;
        HttpServerProvider provider = HttpServerProvider.provider();
        httpserver = provider.createHttpServer(new InetSocketAddress(port), 100);//监听端口,能同时接受100个请求
        httpserver.createContext(path, handler);
        return httpserver;
    }

    public static com.sun.net.httpserver.HttpServer createServer(int port, HttpDispather handler) throws IOException
    {
        return createServer(port, handler, "/");
    }
}
