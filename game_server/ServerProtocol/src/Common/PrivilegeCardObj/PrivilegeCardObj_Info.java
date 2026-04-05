package Common.PrivilegeCardObj;

import java.nio.ByteBuffer;
/*********
 * 权益卡数据
 **/
public class PrivilegeCardObj_Info implements ALBasicProtocolPack._IALProtocolStructure {
/** 权益卡类型 */
private Common.PrivilegeCardEnum.EPrivilegeCardType cardType;
/** 生效开始时间戳（秒） */
private long startS;
/** 生效结束时间戳（秒） */
private long endS;
/** 最后一次领取每日奖励的时间戳（秒） */
private long lastGainDailyRewardS;


public PrivilegeCardObj_Info() {
	cardType = Common.PrivilegeCardEnum.EPrivilegeCardType.values()[0];
	startS = (long)0;
	endS = (long)0;
	lastGainDailyRewardS = (long)0;
}

public PrivilegeCardObj_Info(
	 Common.PrivilegeCardEnum.EPrivilegeCardType _cardType
	, long _startS
	, long _endS
	, long _lastGainDailyRewardS
) {	cardType = _cardType;
	startS = _startS;
	endS = _endS;
	lastGainDailyRewardS = _lastGainDailyRewardS;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 权益卡类型 */
public Common.PrivilegeCardEnum.EPrivilegeCardType getCardType() { return cardType; }
/** 权益卡类型 */
public void setCardType(Common.PrivilegeCardEnum.EPrivilegeCardType _cardType) { cardType = _cardType; }
/** 生效开始时间戳（秒） */
public long getStartS() { return startS; }
/** 生效开始时间戳（秒） */
public void setStartS(long _startS) { startS = _startS; }
/** 生效结束时间戳（秒） */
public long getEndS() { return endS; }
/** 生效结束时间戳（秒） */
public void setEndS(long _endS) { endS = _endS; }
/** 最后一次领取每日奖励的时间戳（秒） */
public long getLastGainDailyRewardS() { return lastGainDailyRewardS; }
/** 最后一次领取每日奖励的时间戳（秒） */
public void setLastGainDailyRewardS(long _lastGainDailyRewardS) { lastGainDailyRewardS = _lastGainDailyRewardS; }


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
	if(_buf.remaining() > 0) cardType = Common.PrivilegeCardEnum.EPrivilegeCardType.EPrivilegeCardType_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) startS = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) endS = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) lastGainDailyRewardS = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(cardType.ordinal());

	_buf.putLong(startS);
	_buf.putLong(endS);
	_buf.putLong(lastGainDailyRewardS);
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

