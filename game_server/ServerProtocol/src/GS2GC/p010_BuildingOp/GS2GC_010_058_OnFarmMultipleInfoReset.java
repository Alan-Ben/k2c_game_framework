package GS2GC.p010_BuildingOp;

import java.nio.ByteBuffer;
/*********
 * 农田暴击信息重置
 **/
public class GS2GC_010_058_OnFarmMultipleInfoReset implements ALBasicProtocolPack._IALProtocolStructure {
/** 农田建筑暴击信息 */
private Common.BuildingObj.Building_FarmMultipleInfo farmMultipleInfo;


public GS2GC_010_058_OnFarmMultipleInfoReset() {
	farmMultipleInfo = new Common.BuildingObj.Building_FarmMultipleInfo();
}

public GS2GC_010_058_OnFarmMultipleInfoReset(
	 Common.BuildingObj.Building_FarmMultipleInfo _farmMultipleInfo
) {	farmMultipleInfo = _farmMultipleInfo;
}

public final byte getMainOrder() { return (byte)10; }

public final byte getSubOrder() { return (byte)58; }

/** 农田建筑暴击信息 */
public Common.BuildingObj.Building_FarmMultipleInfo getFarmMultipleInfo() { return farmMultipleInfo; }
/** 农田建筑暴击信息 */
public void setFarmMultipleInfo(Common.BuildingObj.Building_FarmMultipleInfo _farmMultipleInfo) { farmMultipleInfo = _farmMultipleInfo; }


public final int GetBufSize() {
	int _size = 32;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 34;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _farmMultipleInfoCustLen = _buf.getInt();
	int _farmMultipleInfoCurPos = _buf.position();
	farmMultipleInfo.ReadUnzipBuf(_buf, _farmMultipleInfoCurPos + _farmMultipleInfoCustLen);
	_buf.position(_farmMultipleInfoCurPos + _farmMultipleInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(farmMultipleInfo.GetBufSize());
	farmMultipleInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)10);
	_buf.put((byte)58);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)10);
	_recBuf.put((byte)58);
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

