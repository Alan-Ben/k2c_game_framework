package Common.ServerObj;

import java.nio.ByteBuffer;
/*********
 * 子嗣联姻池请求数据
 **/
public class ServerObj_AdultMarryGroupApplyInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 申请玩家CID */
private long applyCid;
/** 发起请求的子嗣实例ID */
private long applyAdultId;
/** 匹配ID */
private int matchId;
/** 子嗣收益 */
private long bonus;
/** 对方子嗣允许最小收益 */
private long minBonus;


public ServerObj_AdultMarryGroupApplyInfo() {
	applyCid = (long)0;
	applyAdultId = (long)0;
	matchId = 0;
	bonus = (long)0;
	minBonus = (long)0;
}

public ServerObj_AdultMarryGroupApplyInfo(
	 long _applyCid
	, long _applyAdultId
	, int _matchId
	, long _bonus
	, long _minBonus
) {	applyCid = _applyCid;
	applyAdultId = _applyAdultId;
	matchId = _matchId;
	bonus = _bonus;
	minBonus = _minBonus;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 申请玩家CID */
public long getApplyCid() { return applyCid; }
/** 申请玩家CID */
public void setApplyCid(long _applyCid) { applyCid = _applyCid; }
/** 发起请求的子嗣实例ID */
public long getApplyAdultId() { return applyAdultId; }
/** 发起请求的子嗣实例ID */
public void setApplyAdultId(long _applyAdultId) { applyAdultId = _applyAdultId; }
/** 匹配ID */
public int getMatchId() { return matchId; }
/** 匹配ID */
public void setMatchId(int _matchId) { matchId = _matchId; }
/** 子嗣收益 */
public long getBonus() { return bonus; }
/** 子嗣收益 */
public void setBonus(long _bonus) { bonus = _bonus; }
/** 对方子嗣允许最小收益 */
public long getMinBonus() { return minBonus; }
/** 对方子嗣允许最小收益 */
public void setMinBonus(long _minBonus) { minBonus = _minBonus; }


public final int GetBufSize() {
	int _size = 36;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 38;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) applyCid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) applyAdultId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) matchId = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) bonus = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) minBonus = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(applyCid);
	_buf.putLong(applyAdultId);
	_buf.putInt(matchId);
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

