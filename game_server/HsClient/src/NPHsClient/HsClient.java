package NPHsClient;

import ALBasicClient.ALBasicClientConf;
import ALBasicProtocolPack.BasicObj._IALProtocolReceiver;
import ALBasicProtocolPack._IALProtocolStructure;
import ALServerLog.ALServerLog;
import BasicServer.C2S_BasicClientVerifyInfo;
import BasicServer.S2C_BasicClientVerifyResult;
import NPCommon.Util.Delegate.ADelegateOne;
import WCGCommon.Enum.NPEnum.EClientLoginType;

import java.io.IOException;
import java.net.InetSocketAddress;
import java.nio.BufferOverflowException;
import java.nio.ByteBuffer;
import java.nio.channels.SelectionKey;
import java.nio.channels.Selector;
import java.nio.channels.SocketChannel;
import java.util.Iterator;
import java.util.Set;

public class HsClient implements _IALProtocolReceiver
{
    private int _m_sBufferLen;
    private ByteBuffer _m_bByteBuffer;
    private ByteBuffer _m_bTmpByteBuffer;
    private boolean _m_bLoged = false;
    private SocketChannel _m_SocketChannel;
    private boolean _m_bExitLoop = false;

    public ADelegateOne<String> OnLoginSucc = new ADelegateOne<>(this);
    public ADelegateOne<ByteBuffer> OnReceiveMsg = new ADelegateOne<>(this);
    public ADelegateOne<String> OnError = new ADelegateOne<>(this);

    public HsClient()
    {
        _m_sBufferLen = 0;
        _m_bByteBuffer = ByteBuffer.allocate(10240);
        _m_bTmpByteBuffer = ByteBuffer.allocate(10240);
        _m_bByteBuffer.clear();


    }

    public void login(String _m_sServerIP, int _m_iServerPort, String _m_sUserName, String _m_sUserPassword, String _m_sCustomMsg)
    {
        Selector clientSelector = null;
        try
        {
            clientSelector = Selector.open();
            _m_SocketChannel = SocketChannel.open();

            InetSocketAddress address = new InetSocketAddress(_m_sServerIP, _m_iServerPort);
            if (!_m_SocketChannel.connect(address))
            {
                OnError.onEvent(String.format("cann't connect to %s:%d", _m_sServerIP, _m_iServerPort));
                clientSelector.close();
                _m_SocketChannel.close();
                return;
            }

            _m_SocketChannel.configureBlocking(false);
            _m_SocketChannel.socket().setTcpNoDelay(true);
            _m_SocketChannel.socket().setKeepAlive(true);
            _m_SocketChannel.register(clientSelector, SelectionKey.OP_READ);

            C2S_BasicClientVerifyInfo msg = new C2S_BasicClientVerifyInfo();
            msg.setClientType(EClientLoginType.LOCAL_CLIENT.ordinal());
            msg.setUserName(_m_sUserName);
            msg.setUserPassword(_m_sUserPassword);
            if (_m_sCustomMsg == null)
            {
                msg.setCustomMsg("");
            } else
            {
                msg.setCustomMsg(_m_sCustomMsg);
            }
            sendBytes(msg.makePackage());

        } catch (Exception ex)
        {

            OnError.onEvent(String.format("login to %s:%d failed:%s", _m_sServerIP, _m_iServerPort, ex.getMessage()));
            ex.printStackTrace();
            return;
        }

        ByteBuffer recBuffer = ByteBuffer.allocate(ALBasicClientConf.getInstance().getRecBufferLen() * 2);

        while (!_m_bExitLoop)
        {
            try
            {
                clientSelector.select();
            } catch (Exception e)
            {
                ALServerLog.Fatal("Client port select event error!!");
                e.printStackTrace();
            }

            try
            {
                Set<SelectionKey> readyKeySet = clientSelector.selectedKeys();
                Iterator<SelectionKey> iter = readyKeySet.iterator();
                while (iter.hasNext())
                {
                    SelectionKey key = (SelectionKey) iter.next();

                    iter.remove();

                    if (key.isReadable())
                    {

                        SocketChannel socketChannel = (SocketChannel) key.channel();

                        try
                        {
                            recBuffer.clear();

                            int recLen = socketChannel.read(recBuffer);

                            recBuffer.flip();

                            if (recLen < 0)
                            {
                                clientSelector.close();
                                _m_SocketChannel.close();
                                return;
                            }

                            if (recLen > 0)
                            {
                                if (!_socketReceivingMessage(recBuffer))
                                {
                                    clientSelector.close();
                                    _m_SocketChannel.close();
                                    return;
                                }
                            }
                        } catch (IOException e)
                        {
                            clientSelector.close();
                            _m_SocketChannel.close();

                            OnError.onEvent(e.getMessage());
                            e.printStackTrace();

                            return;
                        } catch (Exception e)
                        {
                            clientSelector.close();
                            _m_SocketChannel.close();

                            OnError.onEvent(e.getMessage());
                            e.printStackTrace();
                            return;
                        }
                    }
                }
            } catch (Exception e)
            {

                try
                {
                    clientSelector.close();
                    _m_SocketChannel.close();
                } catch (IOException e1)
                {

                }

                OnError.onEvent(e.getMessage());
                e.printStackTrace();
            }
        }
    }

    protected boolean _socketReceivingMessage(ByteBuffer _buf)
    {
        try
        {
            _m_bByteBuffer.put(_buf);
        } catch (BufferOverflowException e)
        {
            ALServerLog.Error("_socketReceivingMessage length is too long, Socket Buffer need more!");
            _m_bByteBuffer.put(_buf.array(), 0, _m_bByteBuffer.remaining());

            _buf.position(_m_bByteBuffer.remaining());
        }

        if (_m_sBufferLen == 0)
        {

            if (_m_bByteBuffer.position() >= 4)
            {

                _m_sBufferLen = _m_bByteBuffer.getInt(0);
            }
        }

        int bufLen = _m_bByteBuffer.position();
        int startPos = 0;
        while ((_m_sBufferLen != 0) && (bufLen >= startPos + _m_sBufferLen + 4))
        {
            ByteBuffer message = ByteBuffer.allocate(_m_sBufferLen);
            message.put(_m_bByteBuffer.array(), startPos + 4, _m_sBufferLen);
            message.flip();

            startPos = startPos + _m_sBufferLen + 4;

            if (_m_bLoged)
            {
                receiveMes(message);

            } else
            {
                if (!_checkLoginMes(message))
                    return false;
            }

            if (bufLen - startPos > 4)
            {

                _m_sBufferLen = _m_bByteBuffer.getInt(startPos);
            } else
            {
                _m_sBufferLen = 0;
                break;
            }
        }

        if (startPos != 0)
        {
            _m_bTmpByteBuffer.clear();
            _m_bTmpByteBuffer.put(_m_bByteBuffer.array(), startPos, bufLen - startPos);
            _m_bTmpByteBuffer.flip();

            _m_bByteBuffer.clear();
            _m_bByteBuffer.put(_m_bTmpByteBuffer);
        }

        if (_buf.remaining() > 0)
        {
            _m_bByteBuffer.put(_buf);
        }
        return true;
    }

    private void receiveMes(ByteBuffer message)
    {
        OnReceiveMsg.onEvent(message);
    }

    public boolean sendProto(_IALProtocolStructure _proto)
    {
        return sendBytes(_proto.makeFullPackage());
    }

    public boolean sendBytes(ByteBuffer _buf)
    {
        if ((_buf == null) || (_buf.remaining() == 0))
        {
            return false;
        }
        ByteBuffer fullBuffer = ByteBuffer.allocate(4 + _buf.remaining());
        fullBuffer.putInt(_buf.remaining());
        fullBuffer.put(_buf);
        fullBuffer.flip();
        try
        {
            _m_SocketChannel.write(fullBuffer);
        } catch (IOException e)
        {
            OnError.onEvent("sendBytes exception:" + e.getMessage());
            e.printStackTrace();
            return false;
        }
        return true;
    }

    boolean _checkLoginMes(ByteBuffer _mes)
    {
        try
        {
            S2C_BasicClientVerifyResult msg = new S2C_BasicClientVerifyResult();
            msg.readPackage(_mes);
            _m_bLoged = true;
            OnLoginSucc.onEvent(msg.getCustomRetMsg());
            return true;
        } catch (Exception e)
        {
            OnError.onEvent(e.getMessage());
            e.printStackTrace();
        }
        return false;
    }

    public void exit()
    {
        _m_bExitLoop = true;
    }
}
