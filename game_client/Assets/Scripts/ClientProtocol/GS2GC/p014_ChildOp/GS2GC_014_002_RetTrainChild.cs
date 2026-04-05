using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p014_ChildOp
{

/// <summary>
/// 训练子嗣（未成年）
/// </summary>
public class GS2GC_014_002_RetTrainChild : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 子嗣上课金币消耗
/// </summary>
private long costValue;
/// <summary>
/// 子嗣上课获得伙伴经验
/// </summary>
private long gainValue;
/// <summary>
/// 子嗣收益加成增加
/// </summary>
private long addBonus;


public GS2GC_014_002_RetTrainChild() {
	costValue = (long)0;
	gainValue = (long)0;
	addBonus = (long)0;
}

public GS2GC_014_002_RetTrainChild(
	long _costValue
	, long _gainValue
	, long _addBonus
) {	costValue = _costValue;
	gainValue = _gainValue;
	addBonus = _addBonus;
}

public byte getMainOrder() { return (byte)14; }

public byte getSubOrder() { return (byte)2; }

/// <summary>
/// 子嗣上课金币消耗
/// </summary>
public long getCostValue() { return costValue; }
/// <summary>
/// 子嗣上课金币消耗
/// </summary>
public void setCostValue(long _costValue) { costValue = _costValue; }
/// <summary>
/// 子嗣上课获得伙伴经验
/// </summary>
public long getGainValue() { return gainValue; }
/// <summary>
/// 子嗣上课获得伙伴经验
/// </summary>
public void setGainValue(long _gainValue) { gainValue = _gainValue; }
/// <summary>
/// 子嗣收益加成增加
/// </summary>
public long getAddBonus() { return addBonus; }
/// <summary>
/// 子嗣收益加成增加
/// </summary>
public void setAddBonus(long _addBonus) { addBonus = _addBonus; }


public int GetBufSize() {
	int _size = 24;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 26;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	costValue = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	gainValue = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	addBonus = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(costValue);
	_buf.putLong(gainValue);
	_buf.putLong(addBonus);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)14);
	_buf.put((byte)2);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)14);
	_recBuf.put((byte)2);
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
	builder.Append("costValue").Append(":").Append(costValue.ToString()).Append(", ");
	builder.Append("gainValue").Append(":").Append(gainValue.ToString()).Append(", ");
	builder.Append("addBonus").Append(":").Append(addBonus.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

