using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p008_TravelOp
{

/// <summary>
/// 处理博彩游历事件
/// </summary>
public class GC2GS_008_011_ReqDealGamblingTravel : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 待处理事件实例ID
/// </summary>
private long instanceId;
/// <summary>
/// 押注钻石数量
/// </summary>
private int betAmount;
/// <summary>
/// 是否放弃事件
/// </summary>
private bool isAbandon;


public GC2GS_008_011_ReqDealGamblingTravel() {
	instanceId = (long)0;
	betAmount = 0;
	isAbandon = false;
}

public GC2GS_008_011_ReqDealGamblingTravel(
	long _instanceId
	, int _betAmount
	, bool _isAbandon
) {	instanceId = _instanceId;
	betAmount = _betAmount;
	isAbandon = _isAbandon;
}

public byte getMainOrder() { return (byte)8; }

public byte getSubOrder() { return (byte)11; }

/// <summary>
/// 待处理事件实例ID
/// </summary>
public long getInstanceId() { return instanceId; }
/// <summary>
/// 待处理事件实例ID
/// </summary>
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
/// <summary>
/// 押注钻石数量
/// </summary>
public int getBetAmount() { return betAmount; }
/// <summary>
/// 押注钻石数量
/// </summary>
public void setBetAmount(int _betAmount) { betAmount = _betAmount; }
/// <summary>
/// 是否放弃事件
/// </summary>
public bool getIsAbandon() { return isAbandon; }
/// <summary>
/// 是否放弃事件
/// </summary>
public void setIsAbandon(bool _isAbandon) { isAbandon = _isAbandon; }


public int GetBufSize() {
	int _size = 13;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 15;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	betAmount = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isAbandon = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(instanceId);
	_buf.putInt(betAmount);
	_buf.put(isAbandon?(byte)1:(byte)0);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)8);
	_buf.put((byte)11);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)8);
	_recBuf.put((byte)11);
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
	builder.Append("instanceId").Append(":").Append(instanceId.ToString()).Append(", ");
	builder.Append("betAmount").Append(":").Append(betAmount.ToString()).Append(", ");
	builder.Append("isAbandon").Append(":").Append(isAbandon.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

