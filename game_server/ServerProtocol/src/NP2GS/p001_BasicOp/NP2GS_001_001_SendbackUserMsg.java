package NP2GS.p001_BasicOp;

import java.nio.ByteBuffer;
public class NP2GS_001_001_SendbackUserMsg implements ALBasicProtocolPack._IALProtocolStructure {
private long sessionId;
private int dealerSerialize;
private byte[] msg;


public NP2GS_001_001_SendbackUserMsg() {
	sessionId = (long)0;
	dealerSerialize = 0;
	msg = null;
}

public NP2GS_001_001_SendbackUserMsg(
	 long _sessionId
	, int _dealerSerialize
	, byte[] _msg
) {	sessionId = _sessionId;
	dealerSerialize = _dealerSerialize;
	msg = _msg;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)1; }

public long getSessionId() { return sessionId; }
public void setSessionId(long _sessionId) { sessionId = _sessionId; }
public int getDealerSerialize() { return dealerSerialize; }
public void setDealerSerialize(int _dealerSerialize) { dealerSerialize = _dealerSerialize; }
public byte[] getMsg() { return msg; }
public java.nio.ByteBuffer get_buffer_Msg() { if(null == msg)return null; else return ByteBuffer.wrap(msg); }

public void setMsg(byte[] _msg) { msg = _msg; }
public void setMsg(java.nio.ByteBuffer _msg) 
{
	if(null == _msg){return;}
	int _oldPos = _msg.position();
	int _bufLength = _msg.remaining();
	msg = new byte[_bufLength];
	_msg.get(msg);
	_msg.position(_oldPos);
}



public final int GetBufSize() {
	int _size = 12;
	_size += 4 + (msg == null ? 0 : msg.length);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 14;
	_size += 4 + (msg == null ? 0 : msg.length);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) sessionId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) dealerSerialize = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _msgCount = _buf.getInt();
	if(0 < _msgCount){
		msg = new byte[_msgCount];
		_buf.get(msg);
	}

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(sessionId);
	_buf.putInt(dealerSerialize);
	_buf.putInt((msg == null ? 0 : msg.length));
	if(null != msg){_buf.put(msg);}

}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)1);
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

