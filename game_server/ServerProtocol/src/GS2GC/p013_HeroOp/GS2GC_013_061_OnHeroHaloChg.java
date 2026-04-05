package GS2GC.p013_HeroOp;

import java.nio.ByteBuffer;
/*********
 * 大臣光环变更推送
 **/
public class GS2GC_013_061_OnHeroHaloChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 光环信息 */
private Common.HeroObj.Hero_HaloInfo haloInfo;


public GS2GC_013_061_OnHeroHaloChg() {
	haloInfo = new Common.HeroObj.Hero_HaloInfo();
}

public GS2GC_013_061_OnHeroHaloChg(
	 Common.HeroObj.Hero_HaloInfo _haloInfo
) {	haloInfo = _haloInfo;
}

public final byte getMainOrder() { return (byte)13; }

public final byte getSubOrder() { return (byte)61; }

/** 光环信息 */
public Common.HeroObj.Hero_HaloInfo getHaloInfo() { return haloInfo; }
/** 光环信息 */
public void setHaloInfo(Common.HeroObj.Hero_HaloInfo _haloInfo) { haloInfo = _haloInfo; }


public final int GetBufSize() {
	int _size = 17;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 19;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _haloInfoCustLen = _buf.getInt();
	int _haloInfoCurPos = _buf.position();
	haloInfo.ReadUnzipBuf(_buf, _haloInfoCurPos + _haloInfoCustLen);
	_buf.position(_haloInfoCurPos + _haloInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(haloInfo.GetBufSize());
	haloInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)13);
	_buf.put((byte)61);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)13);
	_recBuf.put((byte)61);
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

