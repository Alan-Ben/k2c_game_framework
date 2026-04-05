package Common.NpPlayerInfoObj;

import java.nio.ByteBuffer;
/*********
 * 普通称号
 **/
public class PlayerInfo_Title implements ALBasicProtocolPack._IALProtocolStructure {
/** 称号唯一Id */
private long id;
/** 超时时间标记，单位秒。0表示永久 */
private int expireTimeTagS;
/** 是否查看过 */
private boolean viewed;
/** 最近一次获得时间 */
private int lastGainTs;
/** 获得次数 */
private int gainCount;


public PlayerInfo_Title() {
	id = (long)0;
	expireTimeTagS = 0;
	viewed = false;
	lastGainTs = 0;
	gainCount = 0;
}

public PlayerInfo_Title(
	 long _id
	, int _expireTimeTagS
	, boolean _viewed
	, int _lastGainTs
	, int _gainCount
) {	id = _id;
	expireTimeTagS = _expireTimeTagS;
	viewed = _viewed;
	lastGainTs = _lastGainTs;
	gainCount = _gainCount;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 称号唯一Id */
public long getId() { return id; }
/** 称号唯一Id */
public void setId(long _id) { id = _id; }
/** 超时时间标记，单位秒。0表示永久 */
public int getExpireTimeTagS() { return expireTimeTagS; }
/** 超时时间标记，单位秒。0表示永久 */
public void setExpireTimeTagS(int _expireTimeTagS) { expireTimeTagS = _expireTimeTagS; }
/** 是否查看过 */
public boolean getViewed() { return viewed; }
/** 是否查看过 */
public void setViewed(boolean _viewed) { viewed = _viewed; }
/** 最近一次获得时间 */
public int getLastGainTs() { return lastGainTs; }
/** 最近一次获得时间 */
public void setLastGainTs(int _lastGainTs) { lastGainTs = _lastGainTs; }
/** 获得次数 */
public int getGainCount() { return gainCount; }
/** 获得次数 */
public void setGainCount(int _gainCount) { gainCount = _gainCount; }


public final int GetBufSize() {
	int _size = 21;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 23;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) id = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) expireTimeTagS = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) viewed = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) lastGainTs = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) gainCount = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(id);
	_buf.putInt(expireTimeTagS);
	_buf.put(viewed?(byte)1:(byte)0);
	_buf.putInt(lastGainTs);
	_buf.putInt(gainCount);
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

