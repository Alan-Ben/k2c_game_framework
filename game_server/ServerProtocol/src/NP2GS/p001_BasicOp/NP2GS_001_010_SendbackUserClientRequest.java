package NP2GS.p001_BasicOp;

import java.nio.ByteBuffer;
public class NP2GS_001_010_SendbackUserClientRequest implements ALBasicProtocolPack._IALProtocolStructure {
private long sessionId;
private int dealerSerialize;
private long clientRequestSerialize;
private boolean res;
private int errCode;
private byte[] retMsg;


public NP2GS_001_010_SendbackUserClientRequest() {
	sessionId = (long)0;
	dealerSerialize = 0;
	clientRequestSerialize = (long)0;
	res = false;
	errCode = 0;
	retMsg = null;
}

public NP2GS_001_010_SendbackUserClientRequest(
	 long _sessionId
	, int _dealerSerialize
	, long _clientRequestSerialize
	, boolean _res
	, int _errCode
	, byte[] _retMsg
) {	sessionId = _sessionId;
	dealerSerialize = _dealerSerialize;
	clientRequestSerialize = _clientRequestSerialize;
	res = _res;
	errCode = _errCode;
	retMsg = _retMsg;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)10; }

public long getSessionId() { return sessionId; }
public void setSessionId(long _sessionId) { sessionId = _sessionId; }
public int getDealerSerialize() { return dealerSerialize; }
public void setDealerSerialize(int _dealerSerialize) { dealerSerialize = _dealerSerialize; }
public long getClientRequestSerialize() { return clientRequestSerialize; }
public void setClientRequestSerialize(long _clientRequestSerialize) { clientRequestSerialize = _clientRequestSerialize; }
public boolean getRes() { return res; }
public void setRes(boolean _res) { res = _res; }
public int getErrCode() { return errCode; }
public void setErrCode(int _errCode) { errCode = _errCode; }
public byte[] getRetMsg() { return retMsg; }
public java.nio.ByteBuffer get_buffer_RetMsg() { if(null == retMsg)return null; else return ByteBuffer.wrap(retMsg); }

public void setRetMsg(byte[] _retMsg) { retMsg = _retMsg; }
public void setRetMsg(java.nio.ByteBuffer _retMsg) 
{
	if(null == _retMsg){return;}
	int _oldPos = _retMsg.position();
	int _bufLength = _retMsg.remaining();
	retMsg = new byte[_bufLength];
	_retMsg.get(retMsg);
	_retMsg.position(_oldPos);
}



public final int GetBufSize() {
	int _size = 25;
	_size += 4 + (retMsg == null ? 0 : retMsg.length);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 27;
	_size += 4 + (retMsg == null ? 0 : retMsg.length);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) sessionId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) dealerSerialize = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) clientRequestSerialize = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) res = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) errCode = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _retMsgCount = _buf.getInt();
	if(0 < _retMsgCount){
		retMsg = new byte[_retMsgCount];
		_buf.get(retMsg);
	}

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(sessionId);
	_buf.putInt(dealerSerialize);
	_buf.putLong(clientRequestSerialize);
	_buf.put(res?(byte)1:(byte)0);
	_buf.putInt(errCode);
	_buf.putInt((retMsg == null ? 0 : retMsg.length));
	if(null != retMsg){_buf.put(retMsg);}

}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)10);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)10);
	PutUnzipBuf(_recBuf);
}
public final ByteBuffer makePackage() {
	int _bufSize = GetBufSize();
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void readPackage(ByteBuffer _buf) {
	ReadUnzipBuf(_buf, -1);
}
}

