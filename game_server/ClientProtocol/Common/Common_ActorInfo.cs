using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common
{

public class Common_ActorInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 宠物实例Id，虚构的使用负数
/// </summary>
private long petInstanceId;
/// <summary>
/// 宠物卡牌Id
/// </summary>
private long petId;
private int lvl;
private short star;
private short quality;
private long skin_id;
private byte[] cardExInfo;
/// <summary>
/// 宠物资质品质
/// </summary>
private NPEnum.ENPAttrGrade attrGrade;


public Common_ActorInfo() {
	petInstanceId = (long)0;
	petId = (long)0;
	lvl = 0;
	star = (short)0;
	quality = (short)0;
	skin_id = (long)0;
	cardExInfo = null;
	attrGrade = 0;
}

public Common_ActorInfo(
	long _petInstanceId
	, long _petId
	, int _lvl
	, short _star
	, short _quality
	, long _skin_id
	, byte[] _cardExInfo
	, NPEnum.ENPAttrGrade _attrGrade
) {	petInstanceId = _petInstanceId;
	petId = _petId;
	lvl = _lvl;
	star = _star;
	quality = _quality;
	skin_id = _skin_id;
	cardExInfo = _cardExInfo;
	attrGrade = _attrGrade;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 宠物实例Id，虚构的使用负数
/// </summary>
public long getPetInstanceId() { return petInstanceId; }
/// <summary>
/// 宠物实例Id，虚构的使用负数
/// </summary>
public void setPetInstanceId(long _petInstanceId) { petInstanceId = _petInstanceId; }
/// <summary>
/// 宠物卡牌Id
/// </summary>
public long getPetId() { return petId; }
/// <summary>
/// 宠物卡牌Id
/// </summary>
public void setPetId(long _petId) { petId = _petId; }
public int getLvl() { return lvl; }
public void setLvl(int _lvl) { lvl = _lvl; }
public short getStar() { return star; }
public void setStar(short _star) { star = _star; }
public short getQuality() { return quality; }
public void setQuality(short _quality) { quality = _quality; }
public long getSkin_id() { return skin_id; }
public void setSkin_id(long _skin_id) { skin_id = _skin_id; }
public byte[] getCardExInfo() { return cardExInfo; }

public void setCardExInfo(byte[] _cardExInfo) { cardExInfo = _cardExInfo; }

/// <summary>
/// 宠物资质品质
/// </summary>
public NPEnum.ENPAttrGrade getAttrGrade() { return attrGrade; }
/// <summary>
/// 宠物资质品质
/// </summary>
public void setAttrGrade(NPEnum.ENPAttrGrade _attrGrade) { attrGrade = _attrGrade; }


public int GetBufSize() {
	int _size = 36;
	_size += 4 + (cardExInfo == null ? 0 : cardExInfo.Length);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 38;
	_size += 4 + (cardExInfo == null ? 0 : cardExInfo.Length);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	petInstanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	petId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	lvl = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	star = _buf.getShort();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	quality = _buf.getShort();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	skin_id = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	cardExInfo = _buf.getByteBuffer();

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	attrGrade = (NPEnum.ENPAttrGrade)_buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(petInstanceId);
	_buf.putLong(petId);
	_buf.putInt(lvl);
	_buf.putShort(star);
	_buf.putShort(quality);
	_buf.putLong(skin_id);
	_buf.putByteBuffer(cardExInfo);

	_buf.putInt((int)attrGrade);

}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
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
	builder.Append("petInstanceId").Append(":").Append(petInstanceId.ToString()).Append(", ");
	builder.Append("petId").Append(":").Append(petId.ToString()).Append(", ");
	builder.Append("lvl").Append(":").Append(lvl.ToString()).Append(", ");
	builder.Append("star").Append(":").Append(star.ToString()).Append(", ");
	builder.Append("quality").Append(":").Append(quality.ToString()).Append(", ");
	builder.Append("skin_id").Append(":").Append(skin_id.ToString()).Append(", ");
	builder.Append("cardExInfo").Append(":").Append(cardExInfo == null ? "null" : cardExInfo.ToString()).Append(", ");
	builder.Append("attrGrade").Append(":").Append(attrGrade.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

