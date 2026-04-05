package Common.DinnerObj;

import java.nio.ByteBuffer;
/*********
 * 宴会结算信息
 **/
public class Dinner_ResultInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 宴会实例ID */
private long instanceId;
/** 宴会配置ID */
private long dinnerId;
/** 获取的宴会币 */
private long gainCoin;
/** 获得的宴会人气 */
private long gainScore;
/** 宴会人气加成 */
private long scoreAddPer;
/** 凭证类型 */
private Common.DinnerEnum.EDinnerPermitType permitType;
/** 凭证类型ID */
private long permitTypeId;
/** 宾客信息列表 */
private java.util.ArrayList<Common.DinnerObj.Dinner_ResultGuestInfo> guestLog;


public Dinner_ResultInfo() {
	instanceId = (long)0;
	dinnerId = (long)0;
	gainCoin = (long)0;
	gainScore = (long)0;
	scoreAddPer = (long)0;
	permitType = Common.DinnerEnum.EDinnerPermitType.values()[0];
	permitTypeId = (long)0;
	guestLog = new java.util.ArrayList<Common.DinnerObj.Dinner_ResultGuestInfo>();
}

public Dinner_ResultInfo(
	 long _instanceId
	, long _dinnerId
	, long _gainCoin
	, long _gainScore
	, long _scoreAddPer
	, Common.DinnerEnum.EDinnerPermitType _permitType
	, long _permitTypeId
	, java.util.ArrayList<Common.DinnerObj.Dinner_ResultGuestInfo> _guestLog
) {	instanceId = _instanceId;
	dinnerId = _dinnerId;
	gainCoin = _gainCoin;
	gainScore = _gainScore;
	scoreAddPer = _scoreAddPer;
	permitType = _permitType;
	permitTypeId = _permitTypeId;
	guestLog = _guestLog;
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
/** 获取的宴会币 */
public long getGainCoin() { return gainCoin; }
/** 获取的宴会币 */
public void setGainCoin(long _gainCoin) { gainCoin = _gainCoin; }
/** 获得的宴会人气 */
public long getGainScore() { return gainScore; }
/** 获得的宴会人气 */
public void setGainScore(long _gainScore) { gainScore = _gainScore; }
/** 宴会人气加成 */
public long getScoreAddPer() { return scoreAddPer; }
/** 宴会人气加成 */
public void setScoreAddPer(long _scoreAddPer) { scoreAddPer = _scoreAddPer; }
/** 凭证类型 */
public Common.DinnerEnum.EDinnerPermitType getPermitType() { return permitType; }
/** 凭证类型 */
public void setPermitType(Common.DinnerEnum.EDinnerPermitType _permitType) { permitType = _permitType; }
/** 凭证类型ID */
public long getPermitTypeId() { return permitTypeId; }
/** 凭证类型ID */
public void setPermitTypeId(long _permitTypeId) { permitTypeId = _permitTypeId; }
/** 宾客信息列表 */
public java.util.ArrayList<Common.DinnerObj.Dinner_ResultGuestInfo> getGuestLog() { return guestLog; }
/** 宾客信息列表 */
public void addGuestLog(Common.DinnerObj.Dinner_ResultGuestInfo _guestLog) { guestLog.add(_guestLog); }


public final int GetBufSize() {
	int _size = 52;
	_size += 2 + (guestLog.size() * 40);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 54;
	_size += 2 + (guestLog.size() * 40);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) dinnerId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) gainCoin = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) gainScore = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) scoreAddPer = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) permitType = Common.DinnerEnum.EDinnerPermitType.EDinnerPermitType_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) permitTypeId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _guestLogCount = _buf.getShort();
	for(int _i = 0; _i < _guestLogCount; _i++) { 
		Common.DinnerObj.Dinner_ResultGuestInfo _guestLog = new Common.DinnerObj.Dinner_ResultGuestInfo();
		if(_buf.remaining() <= 0) return;
	int __guestLogCustLen = _buf.getInt();
	int __guestLogCurPos = _buf.position();
	_guestLog.ReadUnzipBuf(_buf, __guestLogCurPos + __guestLogCustLen);
	_buf.position(__guestLogCurPos + __guestLogCustLen);

		guestLog.add(_guestLog);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(instanceId);
	_buf.putLong(dinnerId);
	_buf.putLong(gainCoin);
	_buf.putLong(gainScore);
	_buf.putLong(scoreAddPer);
	_buf.putInt(permitType.ordinal());

	_buf.putLong(permitTypeId);
	_buf.putShort((short)guestLog.size());
	for(int _i = 0; _i < guestLog.size(); _i++) { 
		_buf.putInt(guestLog.get(_i).GetBufSize());
	guestLog.get(_i).PutUnzipBuf(_buf);
	}
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

