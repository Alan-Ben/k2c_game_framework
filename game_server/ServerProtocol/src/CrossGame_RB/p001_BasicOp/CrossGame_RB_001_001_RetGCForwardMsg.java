package CrossGame_RB.p001_BasicOp;

import java.nio.ByteBuffer;
public class CrossGame_RB_001_001_RetGCForwardMsg implements ALBasicProtocolPack._IALProtocolStructure {
/** 转发协议 */
private byte[] msg;


public CrossGame_RB_001_001_RetGCForwardMsg() {
	msg = null;
}

public CrossGame_RB_001_001_RetGCForwardMsg(
	 byte[] _msg
) {	msg = _msg;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)1; }

/** 转发协议 */
public byte[] getMsg() { return msg; }
public java.nio.ByteBuffer get_buffer_Msg() { if(null == msg)return null; else return ByteBuffer.wrap(msg); }

/** 转发协议 */
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
	int _size = 0;
	_size += 4 + (msg == null ? 0 : msg.length);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + (msg == null ? 0 : msg.length);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _msgCount = _buf.getInt();
	if(0 < _msgCount){
		msg = new byte[_msgCount];
		_buf.get(msg);
	}

}

public final void PutUnzipBuf(ByteBuffer _buf) {
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

