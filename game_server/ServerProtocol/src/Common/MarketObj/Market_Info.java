package Common.MarketObj;

import java.nio.ByteBuffer;
/*********
 * 集市数据
 **/
public class Market_Info implements ALBasicProtocolPack._IALProtocolStructure {
/** 集市ID */
private long marketId;
/** 集市等级 */
private long marketLvl;
/** 当前操作次数 */
private int count;
/** 上次经营时间（毫秒） */
private long lastOperateMs;


public Market_Info() {
	marketId = (long)0;
	marketLvl = (long)0;
	count = 0;
	lastOperateMs = (long)0;
}

public Market_Info(
	 long _marketId
	, long _marketLvl
	, int _count
	, long _lastOperateMs
) {	marketId = _marketId;
	marketLvl = _marketLvl;
	count = _count;
	lastOperateMs = _lastOperateMs;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 集市ID */
public long getMarketId() { return marketId; }
/** 集市ID */
public void setMarketId(long _marketId) { marketId = _marketId; }
/** 集市等级 */
public long getMarketLvl() { return marketLvl; }
/** 集市等级 */
public void setMarketLvl(long _marketLvl) { marketLvl = _marketLvl; }
/** 当前操作次数 */
public int getCount() { return count; }
/** 当前操作次数 */
public void setCount(int _count) { count = _count; }
/** 上次经营时间（毫秒） */
public long getLastOperateMs() { return lastOperateMs; }
/** 上次经营时间（毫秒） */
public void setLastOperateMs(long _lastOperateMs) { lastOperateMs = _lastOperateMs; }


public final int GetBufSize() {
	int _size = 28;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 30;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) marketId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) marketLvl = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) count = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) lastOperateMs = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(marketId);
	_buf.putLong(marketLvl);
	_buf.putInt(count);
	_buf.putLong(lastOperateMs);
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

