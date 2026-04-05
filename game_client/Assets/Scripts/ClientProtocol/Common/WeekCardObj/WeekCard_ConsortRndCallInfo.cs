using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.WeekCardObj
{

/// <summary>
/// 周卡-妃子倾诉结算信息
/// </summary>
public class WeekCard_ConsortRndCallInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 妃子id
/// </summary>
private long consortId;
/// <summary>
/// 获得势力值
/// </summary>
private long skillPoint;


public WeekCard_ConsortRndCallInfo() {
	consortId = (long)0;
	skillPoint = (long)0;
}

public WeekCard_ConsortRndCallInfo(
	long _consortId
	, long _skillPoint
) {	consortId = _consortId;
	skillPoint = _skillPoint;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 妃子id
/// </summary>
public long getConsortId() { return consortId; }
/// <summary>
/// 妃子id
/// </summary>
public void setConsortId(long _consortId) { consortId = _consortId; }
/// <summary>
/// 获得势力值
/// </summary>
public long getSkillPoint() { return skillPoint; }
/// <summary>
/// 获得势力值
/// </summary>
public void setSkillPoint(long _skillPoint) { skillPoint = _skillPoint; }


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
	consortId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	skillPoint = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(consortId);
	_buf.putLong(skillPoint);
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
	builder.Append("consortId").Append(":").Append(consortId.ToString()).Append(", ");
	builder.Append("skillPoint").Append(":").Append(skillPoint.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

