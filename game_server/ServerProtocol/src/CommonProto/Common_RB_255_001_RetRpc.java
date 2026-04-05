package CommonProto;

import java.nio.ByteBuffer;
/*********
 * 通用的RPC返回
 **/
public class Common_RB_255_001_RetRpc implements ALBasicProtocolPack._IALProtocolStructure {
private byte[] retBytes;


public Common_RB_255_001_RetRpc() {
	retBytes = null;
}

public Common_RB_255_001_RetRpc(
	 byte[] _retBytes
) {	retBytes = _retBytes;
}

public final byte getMainOrder() { return (byte)255; }

public final byte getSubOrder() { return (byte)1; }

public byte[] getRetBytes() { return retBytes; }
public java.nio.ByteBuffer get_buffer_RetBytes() { if(null == retBytes)return null; else return ByteBuffer.wrap(retBytes); }

public void setRetBytes(byte[] _retBytes) { retBytes = _retBytes; }
public void setRetBytes(java.nio.ByteBuffer _retBytes) 
{
	if(null == _retBytes){return;}
	int _oldPos = _retBytes.position();
	int _bufLength = _retBytes.remaining();
	retBytes = new byte[_bufLength];
	_retBytes.get(retBytes);
	_retBytes.position(_oldPos);
}



public final int GetBufSize() {
	int _size = 0;
	_size += 4 + (retBytes == null ? 0 : retBytes.length);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + (retBytes == null ? 0 : retBytes.length);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _retBytesCount = _buf.getInt();
	if(0 < _retBytesCount){
		retBytes = new byte[_retBytesCount];
		_buf.get(retBytes);
	}

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt((retBytes == null ? 0 : retBytes.length));
	if(null != retBytes){_buf.put(retBytes);}

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

