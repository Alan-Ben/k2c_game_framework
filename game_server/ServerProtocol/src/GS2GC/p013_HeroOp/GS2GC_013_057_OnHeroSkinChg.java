package GS2GC.p013_HeroOp;

import java.nio.ByteBuffer;
/*********
 * 大臣皮肤变化推送
 **/
public class GS2GC_013_057_OnHeroSkinChg implements ALBasicProtocolPack._IALProtocolStructure {
private long heroId;
private Common.HeroObj.Hero_SkinInfo skinInfo;


public GS2GC_013_057_OnHeroSkinChg() {
	heroId = (long)0;
	skinInfo = new Common.HeroObj.Hero_SkinInfo();
}

public GS2GC_013_057_OnHeroSkinChg(
	 long _heroId
	, Common.HeroObj.Hero_SkinInfo _skinInfo
) {	heroId = _heroId;
	skinInfo = _skinInfo;
}

public final byte getMainOrder() { return (byte)13; }

public final byte getSubOrder() { return (byte)57; }

public long getHeroId() { return heroId; }
public void setHeroId(long _heroId) { heroId = _heroId; }
public Common.HeroObj.Hero_SkinInfo getSkinInfo() { return skinInfo; }
public void setSkinInfo(Common.HeroObj.Hero_SkinInfo _skinInfo) { skinInfo = _skinInfo; }


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
	if(_buf.remaining() > 0) heroId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _skinInfoCustLen = _buf.getInt();
	int _skinInfoCurPos = _buf.position();
	skinInfo.ReadUnzipBuf(_buf, _skinInfoCurPos + _skinInfoCustLen);
	_buf.position(_skinInfoCurPos + _skinInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(heroId);
	_buf.putInt(skinInfo.GetBufSize());
	skinInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)13);
	_buf.put((byte)57);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)13);
	_recBuf.put((byte)57);
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

