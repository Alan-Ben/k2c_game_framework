package GS2GC.p023_ArenaOp;

import java.nio.ByteBuffer;
public class GS2GC_023_062_OnTowerResearchActivePosChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 研究位置信息 */
private Common.TowerObj.Tower_PosInfo pos;
/** 建筑收益提升万分比 */
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

public final byte getMainOrder() { return (byte)23; }

public final byte getSubOrder() { return (byte)62; }

/** 研究位置信息 */
public Common.TowerObj.Tower_PosInfo getPos() { return pos; }
/** 研究位置信息 */
public void setPos(Common.TowerObj.Tower_PosInfo _pos) { pos = _pos; }
/** 建筑收益提升万分比 */
public int getBuildingProfitAddPer() { return buildingProfitAddPer; }
/** 建筑收益提升万分比 */
public void setBuildingProfitAddPer(int _buildingProfitAddPer) { buildingProfitAddPer = _buildingProfitAddPer; }


public final int GetBufSize() {
	int _size = 20;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 22;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _posCustLen = _buf.getInt();
	int _posCurPos = _buf.position();
	pos.ReadUnzipBuf(_buf, _posCurPos + _posCustLen);
	_buf.position(_posCurPos + _posCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) buildingProfitAddPer = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(pos.GetBufSize());
	pos.PutUnzipBuf(_buf);
	_buf.putInt(buildingProfitAddPer);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)23);
	_buf.put((byte)62);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)23);
	_recBuf.put((byte)62);
	PutUnzipBuf(_recBuf);
}
public final ByteBuffer makePackage() {
	int _bufSize = GetBufSize();
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void readPackage(ByteBuffer _buf) {
	ReadUnzipBuf(_buf, -1);
}
}

