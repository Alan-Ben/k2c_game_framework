package CommonProto;

import java.nio.ByteBuffer;
/*********
 * 通用的RPC请求
 **/
public class Common_R_255_001_ReqRpc implements ALBasicProtocolPack._IALProtocolStructure {
private int classId;
private byte[] reqBytes;


public Common_R_255_001_ReqRpc() {
	classId = 0;
	reqBytes = null;
}

public Common_R_255_001_ReqRpc(
	 int _classId
	, byte[] _reqBytes
) {	classId = _classId;
	reqBytes = _reqBytes;
}

public final byte getMainOrder() { return (byte)255; }

public final byte getSubOrder() { return (byte)1; }

public int getClassId() { return classId; }
public void setClassId(int _classId) { classId = _classId; }
public byte[] getReqBytes() { return reqBytes; }
public java.nio.ByteBuffer get_buffer_ReqBytes() { if(null == reqBytes)return null; else return ByteBuffer.wrap(reqBytes); }

public void setReqBytes(byte[] _reqBytes) { reqBytes = _reqBytes; }
public void setReqBytes(java.nio.ByteBuffer _reqBytes) 
{
	if(null == _reqBytes){return;}
	int _oldPos = _reqBytes.position();
	int _bufLength = _reqBytes.remaining();
	reqBytes = new byte[_bufLength];
	_reqBytes.get(reqBytes);
	_reqBytes.position(_oldPos);
}



public final int GetBufSize() {
	int _size = 4;
	_size += 4 + (reqBytes == null ? 0 : reqBytes.length);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 6;
	_size += 4 + (reqBytes == null ? 0 : reqBytes.length);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) classId = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _reqBytesCount = _buf.getInt();
	if(0 < _reqBytesCount){
		reqBytes = new byte[_reqBytesCount];
		_buf.get(reqBytes);
	}

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(classId);
	_buf.putInt((reqBytes == null ? 0 : reqBytes.length));
	if(null != reqBytes){_buf.put(reqBytes);}

}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)255);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)255);
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

