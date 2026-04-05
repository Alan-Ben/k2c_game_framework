using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.TravelObj
{

/// <summary>
/// 博彩事件额外结果
/// </summary>
public class Travel_GambleResult : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 结果类型
/// </summary>
private Common.TravelEnum.ETravelGambleResult resultType;
/// <summary>
/// 钻石变化量（正获得负损失）
/// </summary>
private long diamondChange;
/// <summary>
/// 下注金额
/// </summary>
private long betAmount;


public Travel_GambleResult() {
	resultType = 0;
	diamondChange = (long)0;
	betAmount = (long)0;
}

public Travel_GambleResult(
	Common.TravelEnum.ETravelGambleResult _resultType
	, long _diamondChange
	, long _betAmount
) {	resultType = _resultType;
	diamondChange = _diamondChange;
	betAmount = _betAmount;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 结果类型
/// </summary>
public Common.TravelEnum.ETravelGambleResult getResultType() { return resultType; }
/// <summary>
/// 结果类型
/// </summary>
public void setResultType(Common.TravelEnum.ETravelGambleResult _resultType) { resultType = _resultType; }
/// <summary>
/// 钻石变化量（正获得负损失）
/// </summary>
public long getDiamondChange() { return diamondChange; }
/// <summary>
/// 钻石变化量（正获得负损失）
/// </summary>
public void setDiamondChange(long _diamondChange) { diamondChange = _diamondChange; }
/// <summary>
/// 下注金额
/// </summary>
public long getBetAmount() { return betAmount; }
/// <summary>
/// 下注金额
/// </summary>
public void setBetAmount(long _betAmount) { betAmount = _betAmount; }


public int GetBufSize() {
	int _size = 20;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 22;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	resultType = (Common.TravelEnum.ETravelGambleResult)_buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	diamondChange = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	betAmount = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt((int)resultType);

	_buf.putLong(diamondChange);
	_buf.putLong(betAmount);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
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
	builder.Append("resultType").Append(":").Append(resultType.ToString()).Append(", ");
	builder.Append("diamondChange").Append(":").Append(diamondChange.ToString()).Append(", ");
	builder.Append("betAmount").Append(":").Append(betAmount.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

