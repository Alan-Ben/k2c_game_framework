package GS2GC.p013_HeroOp;

import java.nio.ByteBuffer;
/*********
 * 大臣套系变更推送
 **/
public class GS2GC_013_060_OnHeroSuitChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 套系信息 */
private Common.HeroObj.Hero_SuitInfo suitInfo;


public GS2GC_013_060_OnHeroSuitChg() {
	suitInfo = new Common.HeroObj.Hero_SuitInfo();
}

public GS2GC_013_060_OnHeroSuitChg(
	 Common.HeroObj.Hero_SuitInfo _suitInfo
) {	suitInfo = _suitInfo;
}

public final byte getMainOrder() { return (byte)13; }

public final byte getSubOrder() { return (byte)60; }

/** 套系信息 */
public Common.HeroObj.Hero_SuitInfo getSuitInfo() { return suitInfo; }
/** 套系信息 */
public void setSuitInfo(Common.HeroObj.Hero_SuitInfo _suitInfo) { suitInfo = _suitInfo; }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + suitInfo.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + suitInfo.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _suitInfoCustLen = _buf.getInt();
	int _suitInfoCurPos = _buf.position();
	suitInfo.ReadUnzipBuf(_buf, _suitInfoCurPos + _suitInfoCustLen);
	_buf.position(_suitInfoCurPos + _suitInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(suitInfo.GetBufSize());
	suitInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)13);
	_buf.put((byte)60);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)13);
	_recBuf.put((byte)60);
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

