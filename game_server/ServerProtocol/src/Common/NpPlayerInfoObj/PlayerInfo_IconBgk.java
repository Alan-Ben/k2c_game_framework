package Common.NpPlayerInfoObj;

import java.nio.ByteBuffer;
public class PlayerInfo_IconBgk implements ALBasicProtocolPack._IALProtocolStructure {
/** 头像框唯一Id */
private long id;
/** 超时时间标记，单位秒。0表示永久 */
private int expireTimeTagS;
/** 是否查看过 */
private boolean viewed;


public PlayerInfo_IconBgk() {
	id = (long)0;
	expireTimeTagS = 0;
	viewed = false;
}

public PlayerInfo_IconBgk(
	 long _id
	, int _expireTimeTagS
	, boolean _viewed
) {	id = _id;
	expireTimeTagS = _expireTimeTagS;
	viewed = _viewed;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 头像框唯一Id */
public long getId() { return id; }
/** 头像框唯一Id */
public void setId(long _id) { id = _id; }
/** 超时时间标记，单位秒。0表示永久 */
public int getExpireTimeTagS() { return expireTimeTagS; }
/** 超时时间标记，单位秒。0表示永久 */
public void setExpireTimeTagS(int _expireTimeTagS) { expireTimeTagS = _expireTimeTagS; }
/** 是否查看过 */
public boolean getViewed() { return viewed; }
/** 是否查看过 */
public void setViewed(boolean _viewed) { viewed = _viewed; }


public final int GetBufSize() {
	int _size = 13;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 15;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) id = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) expireTimeTagS = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) viewed = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(id);
	_buf.putInt(expireTimeTagS);
	_buf.put(viewed?(byte)1:(byte)0);
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

