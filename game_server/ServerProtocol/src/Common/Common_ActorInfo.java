package Common;

import java.nio.ByteBuffer;
public class Common_ActorInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 宠物实例Id，虚构的使用负数 */
private long petInstanceId;
/** 宠物卡牌Id */
private long petId;
private int lvl;
private short star;
private short quality;
private long skin_id;
private byte[] cardExInfo;
/** 宠物资质品质 */
private NPEnum.ENPAttrGrade attrGrade;


public Common_ActorInfo() {
	petInstanceId = (long)0;
	petId = (long)0;
	lvl = 0;
	star = (short)0;
	quality = (short)0;
	skin_id = (long)0;
	cardExInfo = null;
	attrGrade = NPEnum.ENPAttrGrade.values()[0];
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

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 宠物实例Id，虚构的使用负数 */
public long getPetInstanceId() { return petInstanceId; }
/** 宠物实例Id，虚构的使用负数 */
public void setPetInstanceId(long _petInstanceId) { petInstanceId = _petInstanceId; }
/** 宠物卡牌Id */
public long getPetId() { return petId; }
/** 宠物卡牌Id */
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
public java.nio.ByteBuffer get_buffer_CardExInfo() { if(null == cardExInfo)return null; else return ByteBuffer.wrap(cardExInfo); }

public void setCardExInfo(byte[] _cardExInfo) { cardExInfo = _cardExInfo; }
public void setCardExInfo(java.nio.ByteBuffer _cardExInfo) 
{
	if(null == _cardExInfo){return;}
	int _oldPos = _cardExInfo.position();
	int _bufLength = _cardExInfo.remaining();
	cardExInfo = new byte[_bufLength];
	_cardExInfo.get(cardExInfo);
	_cardExInfo.position(_oldPos);
}

/** 宠物资质品质 */
public NPEnum.ENPAttrGrade getAttrGrade() { return attrGrade; }
/** 宠物资质品质 */
public void setAttrGrade(NPEnum.ENPAttrGrade _attrGrade) { attrGrade = _attrGrade; }


public final int GetBufSize() {
	int _size = 36;
	_size += 4 + (cardExInfo == null ? 0 : cardExInfo.length);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 38;
	_size += 4 + (cardExInfo == null ? 0 : cardExInfo.length);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) petInstanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) petId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) lvl = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) star = _buf.getShort();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) quality = _buf.getShort();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) skin_id = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _cardExInfoCount = _buf.getInt();
	if(0 < _cardExInfoCount){
		cardExInfo = new byte[_cardExInfoCount];
		_buf.get(cardExInfo);
	}

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) attrGrade = NPEnum.ENPAttrGrade.ENPAttrGrade_FromInt(_buf.getInt());
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(petInstanceId);
	_buf.putLong(petId);
	_buf.putInt(lvl);
	_buf.putShort(star);
	_buf.putShort(quality);
	_buf.putLong(skin_id);
	_buf.putInt((cardExInfo == null ? 0 : cardExInfo.length));
	if(null != cardExInfo){_buf.put(cardExInfo);}

	_buf.putInt(attrGrade.ordinal());

}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
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

