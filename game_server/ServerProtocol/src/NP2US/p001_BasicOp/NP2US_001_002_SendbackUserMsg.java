package NP2US.p001_BasicOp;

import java.nio.ByteBuffer;
public class NP2US_001_002_SendbackUserMsg implements ALBasicProtocolPack._IALProtocolStructure {
private long cid;
private byte[] msg;


public NP2US_001_002_SendbackUserMsg() {
	cid = (long)0;
	msg = null;
}

public NP2US_001_002_SendbackUserMsg(
	 long _cid
	, byte[] _msg
) {	cid = _cid;
	msg = _msg;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)2; }

public long getCid() { return cid; }
public void setCid(long _cid) { cid = _cid; }
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
	int _size = 8;
	_size += 4 + (msg == null ? 0 : msg.length);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += 4 + (msg == null ? 0 : msg.length);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _msgCount = _buf.getInt();
	if(0 < _msgCount){
		msg = new byte[_msgCount];
		_buf.get(msg);
	}

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(cid);
	_buf.putInt((msg == null ? 0 : msg.length));
	if(null != msg){_buf.put(msg);}

}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)2);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)2);
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

