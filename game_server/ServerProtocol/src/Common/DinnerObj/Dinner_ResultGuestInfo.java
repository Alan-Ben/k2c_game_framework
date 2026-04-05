package Common.DinnerObj;

import java.nio.ByteBuffer;
/*********
 * 宴会结算宾客信息
 **/
public class Dinner_ResultGuestInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 赴宴对象类型 */
private Common.DinnerEnum.EDinnerJoinerType joinerType;
/** 赴宴对象ID */
private long joinerId;
/** 赴宴花费配置ID */
private long costId;
/** 赴宴玩家宴会币 */
private long coin;
/** 宴玩家宴会人气 */
private long score;


public Dinner_ResultGuestInfo() {
	joinerType = Common.DinnerEnum.EDinnerJoinerType.values()[0];
	joinerId = (long)0;
	costId = (long)0;
	coin = (long)0;
	score = (long)0;
}

public Dinner_ResultGuestInfo(
	 Common.DinnerEnum.EDinnerJoinerType _joinerType
	, long _joinerId
	, long _costId
	, long _coin
	, long _score
) {	joinerType = _joinerType;
	joinerId = _joinerId;
	costId = _costId;
	coin = _coin;
	score = _score;
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
/** 赴宴玩家宴会币 */
public long getCoin() { return coin; }
/** 赴宴玩家宴会币 */
public void setCoin(long _coin) { coin = _coin; }
/** 宴玩家宴会人气 */
public long getScore() { return score; }
/** 宴玩家宴会人气 */
public void setScore(long _score) { score = _score; }


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
	if(_buf.remaining() > 0) coin = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) score = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(joinerType.ordinal());

	_buf.putLong(joinerId);
	_buf.putLong(costId);
	_buf.putLong(coin);
	_buf.putLong(score);
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

