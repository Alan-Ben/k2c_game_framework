package GS2GC.p010_BuildingOp;

import java.nio.ByteBuffer;
/*********
 * 推送农田建筑建造
 **/
public class GS2GC_010_055_OnFarmBuilt implements ALBasicProtocolPack._IALProtocolStructure {
/** 农田建筑数据 */
private Common.BuildingObj.Building_Farm farm;


public GS2GC_010_055_OnFarmBuilt() {
	farm = new Common.BuildingObj.Building_Farm();
}

public GS2GC_010_055_OnFarmBuilt(
	 Common.BuildingObj.Building_Farm _farm
) {	farm = _farm;
}

public final byte getMainOrder() { return (byte)10; }

public final byte getSubOrder() { return (byte)55; }

/** 农田建筑数据 */
public Common.BuildingObj.Building_Farm getFarm() { return farm; }
/** 农田建筑数据 */
public void setFarm(Common.BuildingObj.Building_Farm _farm) { farm = _farm; }


public final int GetBufSize() {
	int _size = 24;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 26;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _farmCustLen = _buf.getInt();
	int _farmCurPos = _buf.position();
	farm.ReadUnzipBuf(_buf, _farmCurPos + _farmCustLen);
	_buf.position(_farmCurPos + _farmCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(farm.GetBufSize());
	farm.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)10);
	_buf.put((byte)55);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)10);
	_recBuf.put((byte)55);
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

