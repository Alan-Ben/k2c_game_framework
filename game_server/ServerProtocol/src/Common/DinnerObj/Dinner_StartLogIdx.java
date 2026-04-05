package Common.DinnerObj;

import java.nio.ByteBuffer;
/*********
 * 开宴记录-索引记录
 **/
public class Dinner_StartLogIdx implements ALBasicProtocolPack._IALProtocolStructure {
/** 宴会实例ID */
private long instanceId;
/** 宴会配置ID */
private long dinnerId;
/** 获得的宴会人气 */
private long gainScore;
/** 开始时间戳（秒） */
private int startTs;
/** 赴宴玩家数量 */
private int joinerCount;
/** 凭证类型 */
private Common.DinnerEnum.EDinnerPermitType permitType;
/** 凭证类型ID */
private long permitTypeId;


public Dinner_StartLogIdx() {
	instanceId = (long)0;
	dinnerId = (long)0;
	gainScore = (long)0;
	startTs = 0;
	joinerCount = 0;
	permitType = Common.DinnerEnum.EDinnerPermitType.values()[0];
	permitTypeId = (long)0;
}

public Dinner_StartLogIdx(
	 long _instanceId
	, long _dinnerId
	, long _gainScore
	, int _startTs
	, int _joinerCount
	, Common.DinnerEnum.EDinnerPermitType _permitType
	, long _permitTypeId
) {	instanceId = _instanceId;
	dinnerId = _dinnerId;
	gainScore = _gainScore;
	startTs = _startTs;
	joinerCount = _joinerCount;
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
/** 获得的宴会人气 */
public long getGainScore() { return gainScore; }
/** 获得的宴会人气 */
public void setGainScore(long _gainScore) { gainScore = _gainScore; }
/** 开始时间戳（秒） */
public int getStartTs() { return startTs; }
/** 开始时间戳（秒） */
public void setStartTs(int _startTs) { startTs = _startTs; }
/** 赴宴玩家数量 */
public int getJoinerCount() { return joinerCount; }
/** 赴宴玩家数量 */
public void setJoinerCount(int _joinerCount) { joinerCount = _joinerCount; }
/** 凭证类型 */
public Common.DinnerEnum.EDinnerPermitType getPermitType() { return permitType; }
/** 凭证类型 */
public void setPermitType(Common.DinnerEnum.EDinnerPermitType _permitType) { permitType = _permitType; }
/** 凭证类型ID */
public long getPermitTypeId() { return permitTypeId; }
/** 凭证类型ID */
public void setPermitTypeId(long _permitTypeId) { permitTypeId = _permitTypeId; }


public final int GetBufSize() {
	int _size = 44;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 46;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) dinnerId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) gainScore = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) startTs = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) joinerCount = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) permitType = Common.DinnerEnum.EDinnerPermitType.EDinnerPermitType_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) permitTypeId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(instanceId);
	_buf.putLong(dinnerId);
	_buf.putLong(gainScore);
	_buf.putInt(startTs);
	_buf.putInt(joinerCount);
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

