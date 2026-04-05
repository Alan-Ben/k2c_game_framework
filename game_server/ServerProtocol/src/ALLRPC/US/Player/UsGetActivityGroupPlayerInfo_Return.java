package ALLRPC.US.Player;

import java.nio.ByteBuffer;
public class UsGetActivityGroupPlayerInfo_Return implements ALBasicProtocolPack._IALProtocolStructure {
/** 玩家群组数据 */
private byte[] obj;


public UsGetActivityGroupPlayerInfo_Return() {
	obj = null;
}

public UsGetActivityGroupPlayerInfo_Return(
	 byte[] _obj
) {	obj = _obj;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 玩家群组数据 */
public byte[] getObj() { return obj; }
public java.nio.ByteBuffer get_buffer_Obj() { if(null == obj)return null; else return ByteBuffer.wrap(obj); }

/** 玩家群组数据 */
public void setObj(byte[] _obj) { obj = _obj; }
public void setObj(java.nio.ByteBuffer _obj) 
{
	if(null == _obj){return;}
	int _oldPos = _obj.position();
	int _bufLength = _obj.remaining();
	obj = new byte[_bufLength];
	_obj.get(obj);
	_obj.position(_oldPos);
}



public final int GetBufSize() {
	int _size = 0;
	_size += 4 + (obj == null ? 0 : obj.length);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + (obj == null ? 0 : obj.length);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _objCount = _buf.getInt();
	if(0 < _objCount){
		obj = new byte[_objCount];
		_buf.get(obj);
	}

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt((obj == null ? 0 : obj.length));
	if(null != obj){_buf.put(obj);}

}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
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

