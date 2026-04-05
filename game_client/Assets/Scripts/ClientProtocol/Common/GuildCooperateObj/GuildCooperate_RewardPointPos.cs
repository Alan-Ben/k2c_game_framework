using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.GuildCooperateObj
{

/// <summary>
/// 联盟协作奖励点位置
/// </summary>
public class GuildCooperate_RewardPointPos : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 区域ID
/// </summary>
private long areaId;
/// <summary>
/// 奖励点索引，从0开始
/// </summary>
private int index;


public GuildCooperate_RewardPointPos() {
	areaId = (long)0;
	index = 0;
}

public GuildCooperate_RewardPointPos(
	long _areaId
	, int _index
) {	areaId = _areaId;
	index = _index;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 区域ID
/// </summary>
public long getAreaId() { return areaId; }
/// <summary>
/// 区域ID
/// </summary>
public void setAreaId(long _areaId) { areaId = _areaId; }
/// <summary>
/// 奖励点索引，从0开始
/// </summary>
public int getIndex() { return index; }
/// <summary>
/// 奖励点索引，从0开始
/// </summary>
public void setIndex(int _index) { index = _index; }


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
	areaId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	index = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(areaId);
	_buf.putInt(index);
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
	builder.Append("areaId").Append(":").Append(areaId.ToString()).Append(", ");
	builder.Append("index").Append(":").Append(index.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

