using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.AchieveObj
{

/// <summary>
/// 成就点数据
/// </summary>
public class Achieve_AchievePointInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 成就点ID EAchieveType
/// </summary>
private int type;
/// <summary>
/// 数量
/// </summary>
private long count;
/// <summary>
/// 已领取最大成就步骤
/// </summary>
private int hadDrawMaxStep;


public Achieve_AchievePointInfo() {
	type = 0;
	count = (long)0;
	hadDrawMaxStep = 0;
}

public Achieve_AchievePointInfo(
	int _type
	, long _count
	, int _hadDrawMaxStep
) {	type = _type;
	count = _count;
	hadDrawMaxStep = _hadDrawMaxStep;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 成就点ID EAchieveType
/// </summary>
public int getType() { return type; }
/// <summary>
/// 成就点ID EAchieveType
/// </summary>
public void setType(int _type) { type = _type; }
/// <summary>
/// 数量
/// </summary>
public long getCount() { return count; }
/// <summary>
/// 数量
/// </summary>
public void setCount(long _count) { count = _count; }
/// <summary>
/// 已领取最大成就步骤
/// </summary>
public int getHadDrawMaxStep() { return hadDrawMaxStep; }
/// <summary>
/// 已领取最大成就步骤
/// </summary>
public void setHadDrawMaxStep(int _hadDrawMaxStep) { hadDrawMaxStep = _hadDrawMaxStep; }


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
	type = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	count = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	hadDrawMaxStep = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(type);
	_buf.putLong(count);
	_buf.putInt(hadDrawMaxStep);
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
	builder.Append("type").Append(":").Append(type.ToString()).Append(", ");
	builder.Append("count").Append(":").Append(count.ToString()).Append(", ");
	builder.Append("hadDrawMaxStep").Append(":").Append(hadDrawMaxStep.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

