using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.BuildingObj
{

/// <summary>
/// 农田建筑数据
/// </summary>
public class Building_Farm : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 建筑ID
/// </summary>
private long buildingId;
/// <summary>
/// 等级
/// </summary>
private int lvl;
/// <summary>
/// 上次点击时间点（毫秒）
/// </summary>
private long lastClickMS;


public Building_Farm() {
	buildingId = (long)0;
	lvl = 0;
	lastClickMS = (long)0;
}

public Building_Farm(
	long _buildingId
	, int _lvl
	, long _lastClickMS
) {	buildingId = _buildingId;
	lvl = _lvl;
	lastClickMS = _lastClickMS;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 建筑ID
/// </summary>
public long getBuildingId() { return buildingId; }
/// <summary>
/// 建筑ID
/// </summary>
public void setBuildingId(long _buildingId) { buildingId = _buildingId; }
/// <summary>
/// 等级
/// </summary>
public int getLvl() { return lvl; }
/// <summary>
/// 等级
/// </summary>
public void setLvl(int _lvl) { lvl = _lvl; }
/// <summary>
/// 上次点击时间点（毫秒）
/// </summary>
public long getLastClickMS() { return lastClickMS; }
/// <summary>
/// 上次点击时间点（毫秒）
/// </summary>
public void setLastClickMS(long _lastClickMS) { lastClickMS = _lastClickMS; }


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
	buildingId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	lvl = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	lastClickMS = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(buildingId);
	_buf.putInt(lvl);
	_buf.putLong(lastClickMS);
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
	builder.Append("buildingId").Append(":").Append(buildingId.ToString()).Append(", ");
	builder.Append("lvl").Append(":").Append(lvl.ToString()).Append(", ");
	builder.Append("lastClickMS").Append(":").Append(lastClickMS.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

