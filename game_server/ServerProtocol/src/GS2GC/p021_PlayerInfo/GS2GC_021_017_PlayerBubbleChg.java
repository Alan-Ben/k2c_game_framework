package GS2GC.p021_PlayerInfo;

import java.nio.ByteBuffer;
public class GS2GC_021_017_PlayerBubbleChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 气泡框唯一Id */
private long id;
/** 超时时间标记，单位秒。-1表示永久 */
private int expireTimeTagS;


public GS2GC_021_017_PlayerBubbleChg() {
	id = (long)0;
	expireTimeTagS = 0;
}

public GS2GC_021_017_PlayerBubbleChg(
	 long _id
	, int _expireTimeTagS
) {	id = _id;
	expireTimeTagS = _expireTimeTagS;
}

public final byte getMainOrder() { return (byte)21; }

public final byte getSubOrder() { return (byte)17; }

/** 气泡框唯一Id */
public long getId() { return id; }
/** 气泡框唯一Id */
public void setId(long _id) { id = _id; }
/** 超时时间标记，单位秒。-1表示永久 */
public int getExpireTimeTagS() { return expireTimeTagS; }
/** 超时时间标记，单位秒。-1表示永久 */
public void setExpireTimeTagS(int _expireTimeTagS) { expireTimeTagS = _expireTimeTagS; }


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
	if(_buf.remaining() > 0) id = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) expireTimeTagS = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(id);
	_buf.putInt(expireTimeTagS);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)21);
	_buf.put((byte)17);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)21);
	_recBuf.put((byte)17);
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

