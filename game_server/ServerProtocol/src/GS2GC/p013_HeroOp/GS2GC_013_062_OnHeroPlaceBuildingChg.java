package GS2GC.p013_HeroOp;

import java.nio.ByteBuffer;
/*********
 * 大臣驻扎建筑变更推送
 **/
public class GS2GC_013_062_OnHeroPlaceBuildingChg implements ALBasicProtocolPack._IALProtocolStructure {
private long heroId;
private Common.HeroObj.Hero_PlaceInfo placeInfo;


public GS2GC_013_062_OnHeroPlaceBuildingChg() {
	heroId = (long)0;
	placeInfo = new Common.HeroObj.Hero_PlaceInfo();
}

public GS2GC_013_062_OnHeroPlaceBuildingChg(
	 long _heroId
	, Common.HeroObj.Hero_PlaceInfo _placeInfo
) {	heroId = _heroId;
	placeInfo = _placeInfo;
}

public final byte getMainOrder() { return (byte)13; }

public final byte getSubOrder() { return (byte)62; }

public long getHeroId() { return heroId; }
public void setHeroId(long _heroId) { heroId = _heroId; }
public Common.HeroObj.Hero_PlaceInfo getPlaceInfo() { return placeInfo; }
public void setPlaceInfo(Common.HeroObj.Hero_PlaceInfo _placeInfo) { placeInfo = _placeInfo; }


public final int GetBufSize() {
	int _size = 28;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 30;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) heroId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _placeInfoCustLen = _buf.getInt();
	int _placeInfoCurPos = _buf.position();
	placeInfo.ReadUnzipBuf(_buf, _placeInfoCurPos + _placeInfoCustLen);
	_buf.position(_placeInfoCurPos + _placeInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(heroId);
	_buf.putInt(placeInfo.GetBufSize());
	placeInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)13);
	_buf.put((byte)62);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)13);
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

