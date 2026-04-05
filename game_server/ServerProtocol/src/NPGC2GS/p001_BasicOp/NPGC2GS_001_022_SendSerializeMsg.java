package NPGC2GS.p001_BasicOp;

import java.nio.ByteBuffer;
public class NPGC2GS_001_022_SendSerializeMsg implements ALBasicProtocolPack._IALProtocolStructure {
private int msgSerialize;
private long clientRequestSerialize;
private byte[] msg;


public NPGC2GS_001_022_SendSerializeMsg() {
	msgSerialize = 0;
	clientRequestSerialize = (long)0;
	msg = null;
}

public NPGC2GS_001_022_SendSerializeMsg(
	 int _msgSerialize
	, long _clientRequestSerialize
	, byte[] _msg
) {	msgSerialize = _msgSerialize;
	clientRequestSerialize = _clientRequestSerialize;
	msg = _msg;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)22; }

public int getMsgSerialize() { return msgSerialize; }
public void setMsgSerialize(int _msgSerialize) { msgSerialize = _msgSerialize; }
public long getClientRequestSerialize() { return clientRequestSerialize; }
public void setClientRequestSerialize(long _clientRequestSerialize) { clientRequestSerialize = _clientRequestSerialize; }
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
	if(_buf.remaining() > 0) msgSerialize = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) clientRequestSerialize = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _msgCount = _buf.getInt();
	if(0 < _msgCount){
		msg = new byte[_msgCount];
		_buf.get(msg);
	}

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(msgSerialize);
	_buf.putLong(clientRequestSerialize);
	_buf.putInt((msg == null ? 0 : msg.length));
	if(null != msg){_buf.put(msg);}

}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)22);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)22);
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

