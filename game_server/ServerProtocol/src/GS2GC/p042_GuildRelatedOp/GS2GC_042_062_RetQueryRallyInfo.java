package GS2GC.p042_GuildRelatedOp;

import java.nio.ByteBuffer;
/*********
 * 返回单个集结信息
 **/
public class GS2GC_042_062_RetQueryRallyInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 集结ID */
private long rallyId;
/** 集结类型 */
private Common.GuildEnum.EGuildRallyType rallyType;
/** 创建者CID */
private long leaderCid;
/** 创建者队伍ID */
private long leaderTeamId;
/** 创建时间毫秒 */
private long createTimeMs;
/** 过期时间毫秒 */
private long expireTimeMs;
/** 最低战力限制 */
private long minPowerLimit;
/** 最大成员限制 */
private int maxMemberLimit;
/** 创建者队伍快照 */
private Common.MarsObj.MarsBattleV2_MemberInfo leaderTeamSnapshot;
/** 扩展数据 */
private byte[] extData;
/** 当前成员CID列表 */
private java.util.ArrayList<Long> memberCidList;


public GS2GC_042_062_RetQueryRallyInfo() {
	rallyId = (long)0;
	rallyType = Common.GuildEnum.EGuildRallyType.values()[0];
	leaderCid = (long)0;
	leaderTeamId = (long)0;
	createTimeMs = (long)0;
	expireTimeMs = (long)0;
	minPowerLimit = (long)0;
	maxMemberLimit = 0;
	leaderTeamSnapshot = new Common.MarsObj.MarsBattleV2_MemberInfo();
	extData = null;
	memberCidList = new java.util.ArrayList<Long>();
}

public GS2GC_042_062_RetQueryRallyInfo(
	 long _rallyId
	, Common.GuildEnum.EGuildRallyType _rallyType
	, long _leaderCid
	, long _leaderTeamId
	, long _createTimeMs
	, long _expireTimeMs
	, long _minPowerLimit
	, int _maxMemberLimit
	, Common.MarsObj.MarsBattleV2_MemberInfo _leaderTeamSnapshot
	, byte[] _extData
	, java.util.ArrayList<Long> _memberCidList
) {	rallyId = _rallyId;
	rallyType = _rallyType;
	leaderCid = _leaderCid;
	leaderTeamId = _leaderTeamId;
	createTimeMs = _createTimeMs;
	expireTimeMs = _expireTimeMs;
	minPowerLimit = _minPowerLimit;
	maxMemberLimit = _maxMemberLimit;
	leaderTeamSnapshot = _leaderTeamSnapshot;
	extData = _extData;
	memberCidList = _memberCidList;
}

public final byte getMainOrder() { return (byte)42; }

public final byte getSubOrder() { return (byte)62; }

/** 集结ID */
public long getRallyId() { return rallyId; }
/** 集结ID */
public void setRallyId(long _rallyId) { rallyId = _rallyId; }
/** 集结类型 */
public Common.GuildEnum.EGuildRallyType getRallyType() { return rallyType; }
/** 集结类型 */
public void setRallyType(Common.GuildEnum.EGuildRallyType _rallyType) { rallyType = _rallyType; }
/** 创建者CID */
public long getLeaderCid() { return leaderCid; }
/** 创建者CID */
public void setLeaderCid(long _leaderCid) { leaderCid = _leaderCid; }
/** 创建者队伍ID */
public long getLeaderTeamId() { return leaderTeamId; }
/** 创建者队伍ID */
public void setLeaderTeamId(long _leaderTeamId) { leaderTeamId = _leaderTeamId; }
/** 创建时间毫秒 */
public long getCreateTimeMs() { return createTimeMs; }
/** 创建时间毫秒 */
public void setCreateTimeMs(long _createTimeMs) { createTimeMs = _createTimeMs; }
/** 过期时间毫秒 */
public long getExpireTimeMs() { return expireTimeMs; }
/** 过期时间毫秒 */
public void setExpireTimeMs(long _expireTimeMs) { expireTimeMs = _expireTimeMs; }
/** 最低战力限制 */
public long getMinPowerLimit() { return minPowerLimit; }
/** 最低战力限制 */
public void setMinPowerLimit(long _minPowerLimit) { minPowerLimit = _minPowerLimit; }
/** 最大成员限制 */
public int getMaxMemberLimit() { return maxMemberLimit; }
/** 最大成员限制 */
public void setMaxMemberLimit(int _maxMemberLimit) { maxMemberLimit = _maxMemberLimit; }
/** 创建者队伍快照 */
public Common.MarsObj.MarsBattleV2_MemberInfo getLeaderTeamSnapshot() { return leaderTeamSnapshot; }
/** 创建者队伍快照 */
public void setLeaderTeamSnapshot(Common.MarsObj.MarsBattleV2_MemberInfo _leaderTeamSnapshot) { leaderTeamSnapshot = _leaderTeamSnapshot; }
/** 扩展数据 */
public byte[] getExtData() { return extData; }
public java.nio.ByteBuffer get_buffer_ExtData() { if(null == extData)return null; else return ByteBuffer.wrap(extData); }

/** 扩展数据 */
public void setExtData(byte[] _extData) { extData = _extData; }
public void setExtData(java.nio.ByteBuffer _extData) 
{
	if(null == _extData){return;}
	int _oldPos = _extData.position();
	int _bufLength = _extData.remaining();
	extData = new byte[_bufLength];
	_extData.get(extData);
	_extData.position(_oldPos);
}

/** 当前成员CID列表 */
public java.util.ArrayList<Long> getMemberCidList() { return memberCidList; }
/** 当前成员CID列表 */
public void addMemberCidList(long _memberCidList) { memberCidList.add(_memberCidList); }


public final int GetBufSize() {
	int _size = 88;
	_size += 4 + (extData == null ? 0 : extData.length);
	_size += 2 + (memberCidList.size() * 8);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 90;
	_size += 4 + (extData == null ? 0 : extData.length);
	_size += 2 + (memberCidList.size() * 8);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) rallyId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) rallyType = Common.GuildEnum.EGuildRallyType.EGuildRallyType_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) leaderCid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) leaderTeamId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) createTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) expireTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) minPowerLimit = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) maxMemberLimit = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _leaderTeamSnapshotCustLen = _buf.getInt();
	int _leaderTeamSnapshotCurPos = _buf.position();
	leaderTeamSnapshot.ReadUnzipBuf(_buf, _leaderTeamSnapshotCurPos + _leaderTeamSnapshotCustLen);
	_buf.position(_leaderTeamSnapshotCurPos + _leaderTeamSnapshotCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _extDataCount = _buf.getInt();
	if(0 < _extDataCount){
		extData = new byte[_extDataCount];
		_buf.get(extData);
	}

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _memberCidListCount = _buf.getShort();
	for(int _i = 0; _i < _memberCidListCount; _i++) { 
		long _memberCidList = (long)0;
		if(_buf.remaining() > 0) _memberCidList = _buf.getLong();
		memberCidList.add(_memberCidList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(rallyId);
	_buf.putInt(rallyType.ordinal());

	_buf.putLong(leaderCid);
	_buf.putLong(leaderTeamId);
	_buf.putLong(createTimeMs);
	_buf.putLong(expireTimeMs);
	_buf.putLong(minPowerLimit);
	_buf.putInt(maxMemberLimit);
	_buf.putInt(leaderTeamSnapshot.GetBufSize());
	leaderTeamSnapshot.PutUnzipBuf(_buf);
	_buf.putInt((extData == null ? 0 : extData.length));
	if(null != extData){_buf.put(extData);}

	_buf.putShort((short)memberCidList.size());
	for(int _i = 0; _i < memberCidList.size(); _i++) { 
		_buf.putLong(memberCidList.get(_i));
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)42);
	_buf.put((byte)62);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)42);
	_recBuf.put((byte)62);
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

