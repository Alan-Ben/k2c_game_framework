using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p036_TreasureHuntOp
{

/// <summary>
/// 太空寻宝-捕捉矿石
/// </summary>
public class GC2GS_036_001_ReqTreasureHuntOreCapture : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 捕捉类型
/// </summary>
private Common.TreasureHuntEnum.ETreasureHuntCaptureType type;
/// <summary>
/// 是否高级
/// </summary>
private bool isAdvance;
/// <summary>
/// 区域ID
/// </summary>
private long areaId;
/// <summary>
/// 距离
/// </summary>
private long distance;


public GC2GS_036_001_ReqTreasureHuntOreCapture() {
	type = 0;
	isAdvance = false;
	areaId = (long)0;
	distance = (long)0;
}

public GC2GS_036_001_ReqTreasureHuntOreCapture(
	Common.TreasureHuntEnum.ETreasureHuntCaptureType _type
	, bool _isAdvance
	, long _areaId
	, long _distance
) {	type = _type;
	isAdvance = _isAdvance;
	areaId = _areaId;
	distance = _distance;
}

public byte getMainOrder() { return (byte)36; }

public byte getSubOrder() { return (byte)1; }

/// <summary>
/// 捕捉类型
/// </summary>
public Common.TreasureHuntEnum.ETreasureHuntCaptureType getType() { return type; }
/// <summary>
/// 捕捉类型
/// </summary>
public void setType(Common.TreasureHuntEnum.ETreasureHuntCaptureType _type) { type = _type; }
/// <summary>
/// 是否高级
/// </summary>
public bool getIsAdvance() { return isAdvance; }
/// <summary>
/// 是否高级
/// </summary>
public void setIsAdvance(bool _isAdvance) { isAdvance = _isAdvance; }
/// <summary>
/// 区域ID
/// </summary>
public long getAreaId() { return areaId; }
/// <summary>
/// 区域ID
/// </summary>
public void setAreaId(long _areaId) { areaId = _areaId; }
/// <summary>
/// 距离
/// </summary>
public long getDistance() { return distance; }
/// <summary>
/// 距离
/// </summary>
public void setDistance(long _distance) { distance = _distance; }


public int GetBufSize() {
	int _size = 21;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 23;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	type = (Common.TreasureHuntEnum.ETreasureHuntCaptureType)_buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isAdvance = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	areaId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	distance = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt((int)type);

	_buf.put(isAdvance?(byte)1:(byte)0);
	_buf.putLong(areaId);
	_buf.putLong(distance);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)36);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)36);
	_recBuf.put((byte)1);
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
	builder.Append("isAdvance").Append(":").Append(isAdvance.ToString()).Append(", ");
	builder.Append("areaId").Append(":").Append(areaId.ToString()).Append(", ");
	builder.Append("distance").Append(":").Append(distance.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

