using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p017_ActivityOp
{

/// <summary>
/// 活动基金-领取奖励后推送
/// </summary>
public class GS2GC_017_066_OnActivityFundDrawRewardChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 基金ID
/// </summary>
private long fundId;
/// <summary>
/// 已领取免费档阶段
/// </summary>
private int hadDrawFreeStep;
/// <summary>
/// 已领取付费档阶段
/// </summary>
private int hadDrawPayStep;


public GS2GC_017_066_OnActivityFundDrawRewardChg() {
	fundId = (long)0;
	hadDrawFreeStep = 0;
	hadDrawPayStep = 0;
}

public GS2GC_017_066_OnActivityFundDrawRewardChg(
	long _fundId
	, int _hadDrawFreeStep
	, int _hadDrawPayStep
) {	fundId = _fundId;
	hadDrawFreeStep = _hadDrawFreeStep;
	hadDrawPayStep = _hadDrawPayStep;
}

public byte getMainOrder() { return (byte)17; }

public byte getSubOrder() { return (byte)66; }

/// <summary>
/// 基金ID
/// </summary>
public long getFundId() { return fundId; }
/// <summary>
/// 基金ID
/// </summary>
public void setFundId(long _fundId) { fundId = _fundId; }
/// <summary>
/// 已领取免费档阶段
/// </summary>
public int getHadDrawFreeStep() { return hadDrawFreeStep; }
/// <summary>
/// 已领取免费档阶段
/// </summary>
public void setHadDrawFreeStep(int _hadDrawFreeStep) { hadDrawFreeStep = _hadDrawFreeStep; }
/// <summary>
/// 已领取付费档阶段
/// </summary>
public int getHadDrawPayStep() { return hadDrawPayStep; }
/// <summary>
/// 已领取付费档阶段
/// </summary>
public void setHadDrawPayStep(int _hadDrawPayStep) { hadDrawPayStep = _hadDrawPayStep; }


public int GetBufSize() {
	int _size = 16;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 18;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	fundId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	hadDrawFreeStep = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	hadDrawPayStep = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(fundId);
	_buf.putInt(hadDrawFreeStep);
	_buf.putInt(hadDrawPayStep);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)17);
	_buf.put((byte)66);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)17);
	_recBuf.put((byte)66);
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
	builder.Append("fundId").Append(":").Append(fundId.ToString()).Append(", ");
	builder.Append("hadDrawFreeStep").Append(":").Append(hadDrawFreeStep.ToString()).Append(", ");
	builder.Append("hadDrawPayStep").Append(":").Append(hadDrawPayStep.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

