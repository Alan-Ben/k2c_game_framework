using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p023_ArenaOp
{

public class GS2GC_023_062_OnTowerResearchActivePosChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 研究位置信息
/// </summary>
private Common.TowerObj.Tower_PosInfo pos;
/// <summary>
/// 建筑收益提升万分比
/// </summary>
private int buildingProfitAddPer;


public GS2GC_023_062_OnTowerResearchActivePosChg() {
	pos = new Common.TowerObj.Tower_PosInfo();
	buildingProfitAddPer = 0;
}

public GS2GC_023_062_OnTowerResearchActivePosChg(
	Common.TowerObj.Tower_PosInfo _pos
	, int _buildingProfitAddPer
) {	pos = _pos;
	buildingProfitAddPer = _buildingProfitAddPer;
}

public byte getMainOrder() { return (byte)23; }

public byte getSubOrder() { return (byte)62; }

/// <summary>
/// 研究位置信息
/// </summary>
public Common.TowerObj.Tower_PosInfo getPos() { return pos; }
/// <summary>
/// 研究位置信息
/// </summary>
public void setPos(Common.TowerObj.Tower_PosInfo _pos) { pos = _pos; }
/// <summary>
/// 建筑收益提升万分比
/// </summary>
public int getBuildingProfitAddPer() { return buildingProfitAddPer; }
/// <summary>
/// 建筑收益提升万分比
/// </summary>
public void setBuildingProfitAddPer(int _buildingProfitAddPer) { buildingProfitAddPer = _buildingProfitAddPer; }


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
	int _posCustLen = _buf.getInt();
	int _posCurPos = _buf.getCurPos();
	pos.ReadUnzipBuf(_buf, _posCurPos + _posCustLen);
	_buf.setPosition(_posCurPos + _posCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	buildingProfitAddPer = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(pos.GetBufSize());
	pos.PutUnzipBuf(_buf);
	_buf.putInt(buildingProfitAddPer);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)23);
	_buf.put((byte)62);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)23);
	_recBuf.put((byte)62);
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
	builder.Append("pos").Append(":").Append(pos == null ? "null" : pos.ToString()).Append(", ");
	builder.Append("buildingProfitAddPer").Append(":").Append(buildingProfitAddPer.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

