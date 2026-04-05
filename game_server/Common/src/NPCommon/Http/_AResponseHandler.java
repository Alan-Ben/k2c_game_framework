package NPCommon.Http;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALBasicServer.ALTask._IALSynTask;
import NPCommon.Log.CommLog;
import org.apache.http.Header;
import org.apache.http.HttpResponse;
import org.apache.http.concurrent.FutureCallback;
import org.apache.http.util.EntityUtils;

import java.io.BufferedReader;
import java.io.InputStreamReader;

/**
 * @author Abe
 */
public abstract class _AResponseHandler implements FutureCallback<HttpResponse>
{

    public static class CancelledException extends Exception
    {
        private static final long serialVersionUID = -421378063733917547L;

    }

    public abstract void onComplete(Header[] headers, int _code, String _response);

    public abstract void onFailed(Exception e);

    @Override
    public void completed(HttpResponse httpResponse)
    {
        try
        {
            int code = httpResponse.getStatusLine().getStatusCode();
            if (code == 200)
            {
                String s = EntityUtils.toString(httpResponse.getEntity(), "UTF-8");
                BufferedReader reader = new BufferedReader(new InputStreamReader(httpResponse.getEntity().getContent(), "UTF-8"));
                final StringBuilder sb = new StringBuilder();
                String line = null;
                while ((line = reader.readLine()) != null)
                {
                    sb.append(line);
                }
                ALSynTaskManager.getInstance().regTask(new _IALSynTask()
                {
                    @Override
                    public void run()
                    {
                        onComplete(httpResponse.getAllHeaders(), 200, s);
                    }
                });
            } else
            {
                CommLog.error("Err Http Code" + httpResponse.getStatusLine().toString());
                ALSynTaskManager.getInstance().regTask(() -> onComplete(httpResponse.getAllHeaders(), code, ""));
            }


        } catch (Exception e)
        {
            CommLog.error("read httpresponse EntityString error : ", e.getMessage(), e);
        }
    }


    @Override
    public void failed(final Exception exception)
    {
        ALSynTaskManager.getInstance().regTask(new _IALSynTask()
        {
            @Override
            public void run()
            {
                onFailed(exception);
            }
        });
    }

    @Override
    public void cancelled()
    {
        ALSynTaskManager.getInstance().regTask(new _IALSynTask()
        {
            @Override
            public void run()
            {
                onFailed(new CancelledException());
            }
        });
    }
}