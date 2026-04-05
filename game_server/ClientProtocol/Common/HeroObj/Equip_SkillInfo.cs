using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.HeroObj
{

/// <summary>
/// 藏品技能信息
/// </summary>
public class Equip_SkillInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 技能索引
/// </summary>
private int index;
/// <summary>
/// 当前加成值
/// </summary>
private int value;
/// <summary>
/// 上次重塑的加成值
/// </summary>
private int pendingValue;
/// <summary>
/// 普通重塑次数
/// </summary>
private int normalRebuildNum;


public Equip_SkillInfo() {
	index = 0;
	value = 0;
	pendingValue = 0;
	normalRebuildNum = 0;
}

public Equip_SkillInfo(
	int _index
	, int _value
	, int _pendingValue
	, int _normalRebuildNum
) {	index = _index;
	value = _value;
	pendingValue = _pendingValue;
	normalRebuildNum = _normalRebuildNum;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 技能索引
/// </summary>
public int getIndex() { return index; }
/// <summary>
/// 技能索引
/// </summary>
public void setIndex(int _index) { index = _index; }
/// <summary>
/// 当前加成值
/// </summary>
public int getValue() { return value; }
/// <summary>
/// 当前加成值
/// </summary>
public void setValue(int _value) { value = _value; }
/// <summary>
/// 上次重塑的加成值
/// </summary>
public int getPendingValue() { return pendingValue; }
/// <summary>
/// 上次重塑的加成值
/// </summary>
public void setPendingValue(int _pendingValue) { pendingValue = _pendingValue; }
/// <summary>
/// 普通重塑次数
/// </summary>
public int getNormalRebuildNum() { return normalRebuildNum; }
/// <summary>
/// 普通重塑次数
/// </summary>
public void setNormalRebuildNum(int _normalRebuildNum) { normalRebuildNum = _normalRebuildNum; }


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
	index = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	value = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	pendingValue = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	normalRebuildNum = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(index);
	_buf.putInt(value);
	_buf.putInt(pendingValue);
	_buf.putInt(normalRebuildNum);
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
	builder.Append("index").Append(":").Append(index.ToString()).Append(", ");
	builder.Append("value").Append(":").Append(value.ToString()).Append(", ");
	builder.Append("pendingValue").Append(":").Append(pendingValue.ToString()).Append(", ");
	builder.Append("normalRebuildNum").Append(":").Append(normalRebuildNum.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

