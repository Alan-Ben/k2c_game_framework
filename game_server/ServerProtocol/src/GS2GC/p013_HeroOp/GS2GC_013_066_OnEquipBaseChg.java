package GS2GC.p013_HeroOp;

import java.nio.ByteBuffer;
/*********
 * 藏品基础信息变更
 **/
public class GS2GC_013_066_OnEquipBaseChg implements ALBasicProtocolPack._IALProtocolStructure {
private Common.HeroObj.Equip_BaseInfo equipInfo;


public GS2GC_013_066_OnEquipBaseChg() {
	equipInfo = new Common.HeroObj.Equip_BaseInfo();
}

public GS2GC_013_066_OnEquipBaseChg(
	 Common.HeroObj.Equip_BaseInfo _equipInfo
) {	equipInfo = _equipInfo;
}

public final byte getMainOrder() { return (byte)13; }

public final byte getSubOrder() { return (byte)66; }

public Common.HeroObj.Equip_BaseInfo getEquipInfo() { return equipInfo; }
public void setEquipInfo(Common.HeroObj.Equip_BaseInfo _equipInfo) { equipInfo = _equipInfo; }


public final int GetBufSize() {
	int _size = 41;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 43;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _equipInfoCustLen = _buf.getInt();
	int _equipInfoCurPos = _buf.position();
	equipInfo.ReadUnzipBuf(_buf, _equipInfoCurPos + _equipInfoCustLen);
	_buf.position(_equipInfoCurPos + _equipInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(equipInfo.GetBufSize());
	equipInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)13);
	_buf.put((byte)66);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)13);
	_recBuf.put((byte)66);
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

