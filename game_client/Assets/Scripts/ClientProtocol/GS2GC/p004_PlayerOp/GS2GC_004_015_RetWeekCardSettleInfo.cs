using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p004_PlayerOp
{

public class GS2GC_004_015_RetWeekCardSettleInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 结算信息
/// </summary>
private Common.WeekCardObj.WeekCard_SettleInfo settleInfo;
/// <summary>
/// 征收粮食离线数据
/// </summary>
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

public byte getMainOrder() { return (byte)4; }

public byte getSubOrder() { return (byte)15; }

/// <summary>
/// 结算信息
/// </summary>
public Common.WeekCardObj.WeekCard_SettleInfo getSettleInfo() { return settleInfo; }
/// <summary>
/// 结算信息
/// </summary>
public void setSettleInfo(Common.WeekCardObj.WeekCard_SettleInfo _settleInfo) { settleInfo = _settleInfo; }
/// <summary>
/// 征收粮食离线数据
/// </summary>
public Common.LevyObj.Levy_FoodOfflineInfo getFoodOfflineInfo() { return foodOfflineInfo; }
/// <summary>
/// 征收粮食离线数据
/// </summary>
public void setFoodOfflineInfo(Common.LevyObj.Levy_FoodOfflineInfo _foodOfflineInfo) { foodOfflineInfo = _foodOfflineInfo; }


public int GetBufSize() {
	int _size = 20;
	_size += 4 + settleInfo.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 22;
	_size += 4 + settleInfo.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _settleInfoCustLen = _buf.getInt();
	int _settleInfoCurPos = _buf.getCurPos();
	settleInfo.ReadUnzipBuf(_buf, _settleInfoCurPos + _settleInfoCustLen);
	_buf.setPosition(_settleInfoCurPos + _settleInfoCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _foodOfflineInfoCustLen = _buf.getInt();
	int _foodOfflineInfoCurPos = _buf.getCurPos();
	foodOfflineInfo.ReadUnzipBuf(_buf, _foodOfflineInfoCurPos + _foodOfflineInfoCustLen);
	_buf.setPosition(_foodOfflineInfoCurPos + _foodOfflineInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(settleInfo.GetBufSize());
	settleInfo.PutUnzipBuf(_buf);
	_buf.putInt(foodOfflineInfo.GetBufSize());
	foodOfflineInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)4);
	_buf.put((byte)15);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)15);
	PutUnzipBuf(_recBuf);
}
public byte[] makePackage() {
	int _bufSize = GetBufSize();
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void readPackage(byte[] _buf) {
	ALProtocolBuf _bufObj = new ALProtocolBuf(_buf);
	ReadUnzipBuf(_bufObj, -1);
}
public void readPackage(ALProtocolBuf _buf) {
	ReadUnzipBuf(_buf, -1);
}
public override string ToString() {
	System.Text.StringBuilder builder = new System.Text.StringBuilder();

	builder.Append("{");
	builder.Append("settleInfo").Append(":").Append(settleInfo == null ? "null" : settleInfo.ToString()).Append(", ");
	builder.Append("foodOfflineInfo").Append(":").Append(foodOfflineInfo == null ? "null" : foodOfflineInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

