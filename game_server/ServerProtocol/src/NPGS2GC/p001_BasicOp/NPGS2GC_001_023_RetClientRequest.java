package NPGS2GC.p001_BasicOp;

import java.nio.ByteBuffer;
public class NPGS2GC_001_023_RetClientRequest implements ALBasicProtocolPack._IALProtocolStructure {
private int msgSerialize;
private long clientRequestSerialize;
private boolean res;
private int errCode;
private byte[] msgBuffer;


public NPGS2GC_001_023_RetClientRequest() {
	msgSerialize = 0;
	clientRequestSerialize = (long)0;
	res = false;
	errCode = 0;
	msgBuffer = null;
}

public NPGS2GC_001_023_RetClientRequest(
	 int _msgSerialize
	, long _clientRequestSerialize
	, boolean _res
	, int _errCode
	, byte[] _msgBuffer
) {	msgSerialize = _msgSerialize;
	clientRequestSerialize = _clientRequestSerialize;
	res = _res;
	errCode = _errCode;
	msgBuffer = _msgBuffer;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)23; }

public int getMsgSerialize() { return msgSerialize; }
public void setMsgSerialize(int _msgSerialize) { msgSerialize = _msgSerialize; }
public long getClientRequestSerialize() { return clientRequestSerialize; }
public void setClientRequestSerialize(long _clientRequestSerialize) { clientRequestSerialize = _clientRequestSerialize; }
public boolean getRes() { return res; }
public void setRes(boolean _res) { res = _res; }
public int getErrCode() { return errCode; }
public void setErrCode(int _errCode) { errCode = _errCode; }
public byte[] getMsgBuffer() { return msgBuffer; }
public java.nio.ByteBuffer get_buffer_MsgBuffer() { if(null == msgBuffer)return null; else return ByteBuffer.wrap(msgBuffer); }

public void setMsgBuffer(byte[] _msgBuffer) { msgBuffer = _msgBuffer; }
public void setMsgBuffer(java.nio.ByteBuffer _msgBuffer) 
{
	if(null == _msgBuffer){return;}
	int _oldPos = _msgBuffer.position();
	int _bufLength = _msgBuffer.remaining();
	msgBuffer = new byte[_bufLength];
	_msgBuffer.get(msgBuffer);
	_msgBuffer.position(_oldPos);
}



public final int GetBufSize() {
	int _size = 17;
	_size += 4 + (msgBuffer == null ? 0 : msgBuffer.length);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 19;
	_size += 4 + (msgBuffer == null ? 0 : msgBuffer.length);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) msgSerialize = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) clientRequestSerialize = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) res = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) errCode = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _msgBufferCount = _buf.getInt();
	if(0 < _msgBufferCount){
		msgBuffer = new byte[_msgBufferCount];
		_buf.get(msgBuffer);
	}

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(msgSerialize);
	_buf.putLong(clientRequestSerialize);
	_buf.put(res?(byte)1:(byte)0);
	_buf.putInt(errCode);
	_buf.putInt((msgBuffer == null ? 0 : msgBuffer.length));
	if(null != msgBuffer){_buf.put(msgBuffer);}

}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)23);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)23);
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

