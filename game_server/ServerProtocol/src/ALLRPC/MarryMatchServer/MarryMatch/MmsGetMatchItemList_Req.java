package ALLRPC.MarryMatchServer.MarryMatch;

import java.nio.ByteBuffer;
public class MmsGetMatchItemList_Req implements ALBasicProtocolPack._IALProtocolStructure {
/** 分组ID */
private long groupId;
/** 匹配子嗣ID */
private long adultId;
/** 匹配玩家CID */
private long cid;
/** 匹配ID */
private int matchId;
/** 子嗣收益 */
private long bonus;
/** 获取符合匹配的数量 */
private int count;


public MmsGetMatchItemList_Req() {
	groupId = (long)0;
	adultId = (long)0;
	cid = (long)0;
	matchId = 0;
	bonus = (long)0;
	count = 0;
}

public MmsGetMatchItemList_Req(
	 long _groupId
	, long _adultId
	, long _cid
	, int _matchId
	, long _bonus
	, int _count
) {	groupId = _groupId;
	adultId = _adultId;
	cid = _cid;
	matchId = _matchId;
	bonus = _bonus;
	count = _count;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 分组ID */
public long getGroupId() { return groupId; }
/** 分组ID */
public void setGroupId(long _groupId) { groupId = _groupId; }
/** 匹配子嗣ID */
public long getAdultId() { return adultId; }
/** 匹配子嗣ID */
public void setAdultId(long _adultId) { adultId = _adultId; }
/** 匹配玩家CID */
public long getCid() { return cid; }
/** 匹配玩家CID */
public void setCid(long _cid) { cid = _cid; }
/** 匹配ID */
public int getMatchId() { return matchId; }
/** 匹配ID */
public void setMatchId(int _matchId) { matchId = _matchId; }
/** 子嗣收益 */
public long getBonus() { return bonus; }
/** 子嗣收益 */
public void setBonus(long _bonus) { bonus = _bonus; }
/** 获取符合匹配的数量 */
public int getCount() { return count; }
/** 获取符合匹配的数量 */
public void setCount(int _count) { count = _count; }


public final int GetBufSize() {
	int _size = 40;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 42;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) groupId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) adultId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) matchId = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) bonus = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) count = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(groupId);
	_buf.putLong(adultId);
	_buf.putLong(cid);
	_buf.putInt(matchId);
	_buf.putLong(bonus);
	_buf.putInt(count);
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

