package NPGC2GS.p001_BasicOp;

import java.nio.ByteBuffer;
public class NPGC2GS_001_006_QuitUS implements ALBasicProtocolPack._IALProtocolStructure {
/** 客户端用于识别的序列号 */
private long clientSerialize;
/** 用户服务器Id */
private int usId;


public NPGC2GS_001_006_QuitUS() {
	clientSerialize = (long)0;
	usId = 0;
}

public NPGC2GS_001_006_QuitUS(
	 long _clientSerialize
	, int _usId
) {	clientSerialize = _clientSerialize;
	usId = _usId;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)6; }

/** 客户端用于识别的序列号 */
public long getClientSerialize() { return clientSerialize; }
/** 客户端用于识别的序列号 */
public void setClientSerialize(long _clientSerialize) { clientSerialize = _clientSerialize; }
/** 用户服务器Id */
public int getUsId() { return usId; }
/** 用户服务器Id */
public void setUsId(int _usId) { usId = _usId; }


public final int GetBufSize() {
	int _size = 12;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 14;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) clientSerialize = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) usId = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(clientSerialize);
	_buf.putInt(usId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)6);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)6);
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

