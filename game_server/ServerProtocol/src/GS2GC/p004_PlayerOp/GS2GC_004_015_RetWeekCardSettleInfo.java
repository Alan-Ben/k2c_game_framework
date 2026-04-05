package GS2GC.p004_PlayerOp;

import java.nio.ByteBuffer;
public class GS2GC_004_015_RetWeekCardSettleInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 结算信息 */
private Common.WeekCardObj.WeekCard_SettleInfo settleInfo;
/** 征收粮食离线数据 */
private Common.LevyObj.Levy_FoodOfflineInfo foodOfflineInfo;


public GS2GC_004_015_RetWeekCardSettleInfo() {
	settleInfo = new Common.WeekCardObj.WeekCard_SettleInfo();
	foodOfflineInfo = new Common.LevyObj.Levy_FoodOfflineInfo();
}

public GS2GC_004_015_RetWeekCardSettleInfo(
	 Common.WeekCardObj.WeekCard_SettleInfo _settleInfo
	, Common.LevyObj.Levy_FoodOfflineInfo _foodOfflineInfo
) {	settleInfo = _settleInfo;
	foodOfflineInfo = _foodOfflineInfo;
}

public final byte getMainOrder() { return (byte)4; }

public final byte getSubOrder() { return (byte)15; }

/** 结算信息 */
public Common.WeekCardObj.WeekCard_SettleInfo getSettleInfo() { return settleInfo; }
/** 结算信息 */
public void setSettleInfo(Common.WeekCardObj.WeekCard_SettleInfo _settleInfo) { settleInfo = _settleInfo; }
/** 征收粮食离线数据 */
public Common.LevyObj.Levy_FoodOfflineInfo getFoodOfflineInfo() { return foodOfflineInfo; }
/** 征收粮食离线数据 */
public void setFoodOfflineInfo(Common.LevyObj.Levy_FoodOfflineInfo _foodOfflineInfo) { foodOfflineInfo = _foodOfflineInfo; }


public final int GetBufSize() {
	int _size = 20;
	_size += 4 + settleInfo.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 22;
	_size += 4 + settleInfo.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _settleInfoCustLen = _buf.getInt();
	int _settleInfoCurPos = _buf.position();
	settleInfo.ReadUnzipBuf(_buf, _settleInfoCurPos + _settleInfoCustLen);
	_buf.position(_settleInfoCurPos + _settleInfoCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _foodOfflineInfoCustLen = _buf.getInt();
	int _foodOfflineInfoCurPos = _buf.position();
	foodOfflineInfo.ReadUnzipBuf(_buf, _foodOfflineInfoCurPos + _foodOfflineInfoCustLen);
	_buf.position(_foodOfflineInfoCurPos + _foodOfflineInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(settleInfo.GetBufSize());
	settleInfo.PutUnzipBuf(_buf);
	_buf.putInt(foodOfflineInfo.GetBufSize());
	foodOfflineInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)4);
	_buf.put((byte)15);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)15);
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

