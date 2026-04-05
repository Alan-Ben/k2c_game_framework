package GS2GC.p010_BuildingOp;

import java.nio.ByteBuffer;
/*********
 * 推送经营建筑数据变更
 **/
public class GS2GC_010_052_OnBusinessChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 经营建筑建筑数据 */
private Common.BuildingObj.Building_Business business;


public GS2GC_010_052_OnBusinessChg() {
	business = new Common.BuildingObj.Building_Business();
}

public GS2GC_010_052_OnBusinessChg(
	 Common.BuildingObj.Building_Business _business
) {	business = _business;
}

public final byte getMainOrder() { return (byte)10; }

public final byte getSubOrder() { return (byte)52; }

/** 经营建筑建筑数据 */
public Common.BuildingObj.Building_Business getBusiness() { return business; }
/** 经营建筑建筑数据 */
public void setBusiness(Common.BuildingObj.Building_Business _business) { business = _business; }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + business.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + business.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _businessCustLen = _buf.getInt();
	int _businessCurPos = _buf.position();
	business.ReadUnzipBuf(_buf, _businessCurPos + _businessCustLen);
	_buf.position(_businessCurPos + _businessCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(business.GetBufSize());
	business.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)10);
	_buf.put((byte)52);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)10);
	_recBuf.put((byte)52);
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

