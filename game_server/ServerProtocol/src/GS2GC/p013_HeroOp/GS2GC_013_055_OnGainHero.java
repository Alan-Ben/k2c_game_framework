package GS2GC.p013_HeroOp;

import java.nio.ByteBuffer;
/*********
 * 获得大臣
 **/
public class GS2GC_013_055_OnGainHero implements ALBasicProtocolPack._IALProtocolStructure {
/** 大臣信息 */
private Common.HeroObj.Hero_Info heroInfo;


public GS2GC_013_055_OnGainHero() {
	heroInfo = new Common.HeroObj.Hero_Info();
}

public GS2GC_013_055_OnGainHero(
	 Common.HeroObj.Hero_Info _heroInfo
) {	heroInfo = _heroInfo;
}

public final byte getMainOrder() { return (byte)13; }

public final byte getSubOrder() { return (byte)55; }

/** 大臣信息 */
public Common.HeroObj.Hero_Info getHeroInfo() { return heroInfo; }
/** 大臣信息 */
public void setHeroInfo(Common.HeroObj.Hero_Info _heroInfo) { heroInfo = _heroInfo; }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + heroInfo.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + heroInfo.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _heroInfoCustLen = _buf.getInt();
	int _heroInfoCurPos = _buf.position();
	heroInfo.ReadUnzipBuf(_buf, _heroInfoCurPos + _heroInfoCustLen);
	_buf.position(_heroInfoCurPos + _heroInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(heroInfo.GetBufSize());
	heroInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)13);
	_buf.put((byte)55);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)13);
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

