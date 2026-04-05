package Common.DinnerObj;

import java.nio.ByteBuffer;
/*********
 * 宴会索引信息
 **/
public class Dinner_Idx implements ALBasicProtocolPack._IALProtocolStructure {
/** 宴会实例ID */
private long instanceId;
/** 宴会配置ID */
private long dinnerId;
/** 开宴玩家CID */
private long ownerCid;
/** 当前赴宴玩家数量 */
private int joinerCount;
/** 当前宴会人气 */
private long score;
/** 结束时间戳（秒） */
private int endTs;
/** 是否加入宴会 */
private boolean isJoined;
/** 凭证类型 */
private Common.DinnerEnum.EDinnerPermitType permitType;
/** 凭证类型ID */
private long permitTypeId;


public Dinner_Idx() {
	instanceId = (long)0;
	dinnerId = (long)0;
	ownerCid = (long)0;
	joinerCount = 0;
	score = (long)0;
	endTs = 0;
	isJoined = false;
	permitType = Common.DinnerEnum.EDinnerPermitType.values()[0];
	permitTypeId = (long)0;
}

public Dinner_Idx(
	 long _instanceId
	, long _dinnerId
	, long _ownerCid
	, int _joinerCount
	, long _score
	, int _endTs
	, boolean _isJoined
	, Common.DinnerEnum.EDinnerPermitType _permitType
	, long _permitTypeId
) {	instanceId = _instanceId;
	dinnerId = _dinnerId;
	ownerCid = _ownerCid;
	joinerCount = _joinerCount;
	score = _score;
	endTs = _endTs;
	isJoined = _isJoined;
	permitType = _permitType;
	permitTypeId = _permitTypeId;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 宴会实例ID */
public long getInstanceId() { return instanceId; }
/** 宴会实例ID */
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
/** 宴会配置ID */
public long getDinnerId() { return dinnerId; }
/** 宴会配置ID */
public void setDinnerId(long _dinnerId) { dinnerId = _dinnerId; }
/** 开宴玩家CID */
public long getOwnerCid() { return ownerCid; }
/** 开宴玩家CID */
public void setOwnerCid(long _ownerCid) { ownerCid = _ownerCid; }
/** 当前赴宴玩家数量 */
public int getJoinerCount() { return joinerCount; }
/** 当前赴宴玩家数量 */
public void setJoinerCount(int _joinerCount) { joinerCount = _joinerCount; }
/** 当前宴会人气 */
public long getScore() { return score; }
/** 当前宴会人气 */
public void setScore(long _score) { score = _score; }
/** 结束时间戳（秒） */
public int getEndTs() { return endTs; }
/** 结束时间戳（秒） */
public void setEndTs(int _endTs) { endTs = _endTs; }
/** 是否加入宴会 */
public boolean getIsJoined() { return isJoined; }
/** 是否加入宴会 */
public void setIsJoined(boolean _isJoined) { isJoined = _isJoined; }
/** 凭证类型 */
public Common.DinnerEnum.EDinnerPermitType getPermitType() { return permitType; }
/** 凭证类型 */
public void setPermitType(Common.DinnerEnum.EDinnerPermitType _permitType) { permitType = _permitType; }
/** 凭证类型ID */
public long getPermitTypeId() { return permitTypeId; }
/** 凭证类型ID */
public void setPermitTypeId(long _permitTypeId) { permitTypeId = _permitTypeId; }


public final int GetBufSize() {
	int _size = 53;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 55;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) dinnerId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) ownerCid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) joinerCount = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) score = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) endTs = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isJoined = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) permitType = Common.DinnerEnum.EDinnerPermitType.EDinnerPermitType_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) permitTypeId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(instanceId);
	_buf.putLong(dinnerId);
	_buf.putLong(ownerCid);
	_buf.putInt(joinerCount);
	_buf.putLong(score);
	_buf.putInt(endTs);
	_buf.put(isJoined?(byte)1:(byte)0);
	_buf.putInt(permitType.ordinal());

	_buf.putLong(permitTypeId);
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

