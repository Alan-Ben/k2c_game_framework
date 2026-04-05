package Common;

import java.nio.ByteBuffer;
public class Common_GuildDonateInfo implements ALBasicProtocolPack._IALProtocolStructure {
private long id;
private long reqId;
private Common.Common_GuildMemberInfo guildMemberInfo;
private long cardId;
private int donateCount;
private int timeTag;


public Common_GuildDonateInfo() {
	id = (long)0;
	reqId = (long)0;
	guildMemberInfo = new Common.Common_GuildMemberInfo();
	cardId = (long)0;
	donateCount = 0;
	timeTag = 0;
}

public Common_GuildDonateInfo(
	 long _id
	, long _reqId
	, Common.Common_GuildMemberInfo _guildMemberInfo
	, long _cardId
	, int _donateCount
	, int _timeTag
) {	id = _id;
	reqId = _reqId;
	guildMemberInfo = _guildMemberInfo;
	cardId = _cardId;
	donateCount = _donateCount;
	timeTag = _timeTag;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getId() { return id; }
public void setId(long _id) { id = _id; }
public long getReqId() { return reqId; }
public void setReqId(long _reqId) { reqId = _reqId; }
public Common.Common_GuildMemberInfo getGuildMemberInfo() { return guildMemberInfo; }
public void setGuildMemberInfo(Common.Common_GuildMemberInfo _guildMemberInfo) { guildMemberInfo = _guildMemberInfo; }
public long getCardId() { return cardId; }
public void setCardId(long _cardId) { cardId = _cardId; }
public int getDonateCount() { return donateCount; }
public void setDonateCount(int _donateCount) { donateCount = _donateCount; }
public int getTimeTag() { return timeTag; }
public void setTimeTag(int _timeTag) { timeTag = _timeTag; }


public final int GetBufSize() {
	int _size = 32;
	_size += 4 + guildMemberInfo.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 34;
	_size += 4 + guildMemberInfo.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) id = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) reqId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _guildMemberInfoCustLen = _buf.getInt();
	int _guildMemberInfoCurPos = _buf.position();
	guildMemberInfo.ReadUnzipBuf(_buf, _guildMemberInfoCurPos + _guildMemberInfoCustLen);
	_buf.position(_guildMemberInfoCurPos + _guildMemberInfoCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cardId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) donateCount = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) timeTag = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(id);
	_buf.putLong(reqId);
	_buf.putInt(guildMemberInfo.GetBufSize());
	guildMemberInfo.PutUnzipBuf(_buf);
	_buf.putLong(cardId);
	_buf.putInt(donateCount);
	_buf.putInt(timeTag);
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

