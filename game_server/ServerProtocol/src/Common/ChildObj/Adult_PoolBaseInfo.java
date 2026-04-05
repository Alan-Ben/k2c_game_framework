package Common.ChildObj;

import java.nio.ByteBuffer;
/*********
 * 联姻池待匹配子嗣（成年未婚）基础数据
 **/
public class Adult_PoolBaseInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 发起请求的玩家CID */
private long applyCid;
/** 发起请求的玩家子嗣ID */
private long applyAdultId;
/** 子嗣收益 */
private long bonus;
/** 对方子嗣允许联姻的最低收益 */
private long minBonus;


public Adult_PoolBaseInfo() {
	applyCid = (long)0;
	applyAdultId = (long)0;
	bonus = (long)0;
	minBonus = (long)0;
}

public Adult_PoolBaseInfo(
	 long _applyCid
	, long _applyAdultId
	, long _bonus
	, long _minBonus
) {	applyCid = _applyCid;
	applyAdultId = _applyAdultId;
	bonus = _bonus;
	minBonus = _minBonus;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 发起请求的玩家CID */
public long getApplyCid() { return applyCid; }
/** 发起请求的玩家CID */
public void setApplyCid(long _applyCid) { applyCid = _applyCid; }
/** 发起请求的玩家子嗣ID */
public long getApplyAdultId() { return applyAdultId; }
/** 发起请求的玩家子嗣ID */
public void setApplyAdultId(long _applyAdultId) { applyAdultId = _applyAdultId; }
/** 子嗣收益 */
public long getBonus() { return bonus; }
/** 子嗣收益 */
public void setBonus(long _bonus) { bonus = _bonus; }
/** 对方子嗣允许联姻的最低收益 */
public long getMinBonus() { return minBonus; }
/** 对方子嗣允许联姻的最低收益 */
public void setMinBonus(long _minBonus) { minBonus = _minBonus; }


public final int GetBufSize() {
	int _size = 32;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 34;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) applyCid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) applyAdultId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) bonus = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) minBonus = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(applyCid);
	_buf.putLong(applyAdultId);
	_buf.putLong(bonus);
	_buf.putLong(minBonus);
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

