package NPCommon.Http.HttpService.server;

import java.util.HashMap;
import java.util.Map;

public class MethodAdapater
{

    private final Map<String, HttpAdapter> requests = new HashMap<>();

    public HttpAdapter getAdapter(String path)
    {
        return requests.get(path);
    }

    public void addAdapter(String path, HttpAdapter httpAdapter)
    {
        requests.put(path, httpAdapter);
    }
}
