using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p013_HeroOp
{

/// <summary>
/// 大臣驻扎建筑变更推送
/// </summary>
public class GS2GC_013_062_OnHeroPlaceBuildingChg : ALBasicProtocolPack._IALProtocolStructure {
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

public byte getMainOrder() { return (byte)13; }

public byte getSubOrder() { return (byte)62; }

public long getHeroId() { return heroId; }
public void setHeroId(long _heroId) { heroId = _heroId; }
public Common.HeroObj.Hero_PlaceInfo getPlaceInfo() { return placeInfo; }
public void setPlaceInfo(Common.HeroObj.Hero_PlaceInfo _placeInfo) { placeInfo = _placeInfo; }


public int GetBufSize() {
	int _size = 28;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 30;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	heroId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _placeInfoCustLen = _buf.getInt();
	int _placeInfoCurPos = _buf.getCurPos();
	placeInfo.ReadUnzipBuf(_buf, _placeInfoCurPos + _placeInfoCustLen);
	_buf.setPosition(_placeInfoCurPos + _placeInfoCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(heroId);
	_buf.putInt(placeInfo.GetBufSize());
	placeInfo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)13);
	_buf.put((byte)62);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)13);
	_recBuf.put((byte)62);
	PutUnzipBuf(_recBuf);
}
public byte[] makePackage() {
	int _bufSize = GetBufSize();
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void readPackage(byte[] _buf) {
	ALProtocolBuf _bufObj = new ALProtocolBuf(_buf);
	ReadUnzipBuf(_bufObj, -1);
}
public void readPackage(ALProtocolBuf _buf) {
	ReadUnzipBuf(_buf, -1);
}
public override string ToString() {
	System.Text.StringBuilder builder = new System.Text.StringBuilder();

	builder.Append("{");
	builder.Append("heroId").Append(":").Append(heroId.ToString()).Append(", ");
	builder.Append("placeInfo").Append(":").Append(placeInfo == null ? "null" : placeInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

