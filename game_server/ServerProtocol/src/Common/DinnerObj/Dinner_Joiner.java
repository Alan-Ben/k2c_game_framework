package Common.DinnerObj;

import java.nio.ByteBuffer;
/*********
 * 赴宴玩家数据
 **/
public class Dinner_Joiner implements ALBasicProtocolPack._IALProtocolStructure {
/** 赴宴对象类型 */
private Common.DinnerEnum.EDinnerJoinerType joinerType;
/** 赴宴对象ID */
private long joinerId;
/** 赴宴花费配置ID */
private long costId;
/** 玩家宴会人气 */
private long score;
/** 赴宴时间戳（毫秒） */
private long joinTimeMs;


public Dinner_Joiner() {
	joinerType = Common.DinnerEnum.EDinnerJoinerType.values()[0];
	joinerId = (long)0;
	costId = (long)0;
	score = (long)0;
	joinTimeMs = (long)0;
}

public Dinner_Joiner(
	 Common.DinnerEnum.EDinnerJoinerType _joinerType
	, long _joinerId
	, long _costId
	, long _score
	, long _joinTimeMs
) {	joinerType = _joinerType;
	joinerId = _joinerId;
	costId = _costId;
	score = _score;
	joinTimeMs = _joinTimeMs;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 赴宴对象类型 */
public Common.DinnerEnum.EDinnerJoinerType getJoinerType() { return joinerType; }
/** 赴宴对象类型 */
public void setJoinerType(Common.DinnerEnum.EDinnerJoinerType _joinerType) { joinerType = _joinerType; }
/** 赴宴对象ID */
public long getJoinerId() { return joinerId; }
/** 赴宴对象ID */
public void setJoinerId(long _joinerId) { joinerId = _joinerId; }
/** 赴宴花费配置ID */
public long getCostId() { return costId; }
/** 赴宴花费配置ID */
public void setCostId(long _costId) { costId = _costId; }
/** 玩家宴会人气 */
public long getScore() { return score; }
/** 玩家宴会人气 */
public void setScore(long _score) { score = _score; }
/** 赴宴时间戳（毫秒） */
public long getJoinTimeMs() { return joinTimeMs; }
/** 赴宴时间戳（毫秒） */
public void setJoinTimeMs(long _joinTimeMs) { joinTimeMs = _joinTimeMs; }


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
	if(_buf.remaining() > 0) joinerType = Common.DinnerEnum.EDinnerJoinerType.EDinnerJoinerType_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) joinerId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) costId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) score = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) joinTimeMs = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(joinerType.ordinal());

	_buf.putLong(joinerId);
	_buf.putLong(costId);
	_buf.putLong(score);
	_buf.putLong(joinTimeMs);
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

