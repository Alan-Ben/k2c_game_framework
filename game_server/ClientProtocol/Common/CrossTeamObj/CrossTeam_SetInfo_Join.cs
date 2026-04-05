using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.CrossTeamObj
{

/// <summary>
/// 组队设置-加入条件
/// </summary>
public class CrossTeam_SetInfo_Join : ALBasicProtocolPack._IALProtocolStructure {
private Common.CrossTeamEnum.ENPCrossTeamJoinCond cond;
private long value;


public CrossTeam_SetInfo_Join() {
	cond = 0;
	value = (long)0;
}

public CrossTeam_SetInfo_Join(
	Common.CrossTeamEnum.ENPCrossTeamJoinCond _cond
	, long _value
) {	cond = _cond;
	value = _value;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public Common.CrossTeamEnum.ENPCrossTeamJoinCond getCond() { return cond; }
public void setCond(Common.CrossTeamEnum.ENPCrossTeamJoinCond _cond) { cond = _cond; }
public long getValue() { return value; }
public void setValue(long _value) { value = _value; }


public int GetBufSize() {
	int _size = 12;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 14;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	cond = (Common.CrossTeamEnum.ENPCrossTeamJoinCond)_buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	value = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt((int)cond);

	_buf.putLong(value);
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
	builder.Append("cond").Append(":").Append(cond.ToString()).Append(", ");
	builder.Append("value").Append(":").Append(value.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

