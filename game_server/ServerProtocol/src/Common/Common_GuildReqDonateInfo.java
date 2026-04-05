package Common;

import java.nio.ByteBuffer;
public class Common_GuildReqDonateInfo implements ALBasicProtocolPack._IALProtocolStructure {
private long id;
private Common.Common_GuildMemberInfo reqGuildMemberInfo;
private long cardId;
private int donateCount;
private int reqDonateCountLimit;
private int reqTimeTag;
private int reqExpireTimeTag;
private java.util.ArrayList<Common.Common_GuildReqDonateCountInfo> guildReqDonateCountInfoList;


public Common_GuildReqDonateInfo() {
	id = (long)0;
	reqGuildMemberInfo = new Common.Common_GuildMemberInfo();
	cardId = (long)0;
	donateCount = 0;
	reqDonateCountLimit = 0;
	reqTimeTag = 0;
	reqExpireTimeTag = 0;
	guildReqDonateCountInfoList = new java.util.ArrayList<Common.Common_GuildReqDonateCountInfo>();
}

public Common_GuildReqDonateInfo(
	 long _id
	, Common.Common_GuildMemberInfo _reqGuildMemberInfo
	, long _cardId
	, int _donateCount
	, int _reqDonateCountLimit
	, int _reqTimeTag
	, int _reqExpireTimeTag
	, java.util.ArrayList<Common.Common_GuildReqDonateCountInfo> _guildReqDonateCountInfoList
) {	id = _id;
	reqGuildMemberInfo = _reqGuildMemberInfo;
	cardId = _cardId;
	donateCount = _donateCount;
	reqDonateCountLimit = _reqDonateCountLimit;
	reqTimeTag = _reqTimeTag;
	reqExpireTimeTag = _reqExpireTimeTag;
	guildReqDonateCountInfoList = _guildReqDonateCountInfoList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getId() { return id; }
public void setId(long _id) { id = _id; }
public Common.Common_GuildMemberInfo getReqGuildMemberInfo() { return reqGuildMemberInfo; }
public void setReqGuildMemberInfo(Common.Common_GuildMemberInfo _reqGuildMemberInfo) { reqGuildMemberInfo = _reqGuildMemberInfo; }
public long getCardId() { return cardId; }
public void setCardId(long _cardId) { cardId = _cardId; }
public int getDonateCount() { return donateCount; }
public void setDonateCount(int _donateCount) { donateCount = _donateCount; }
public int getReqDonateCountLimit() { return reqDonateCountLimit; }
public void setReqDonateCountLimit(int _reqDonateCountLimit) { reqDonateCountLimit = _reqDonateCountLimit; }
public int getReqTimeTag() { return reqTimeTag; }
public void setReqTimeTag(int _reqTimeTag) { reqTimeTag = _reqTimeTag; }
public int getReqExpireTimeTag() { return reqExpireTimeTag; }
public void setReqExpireTimeTag(int _reqExpireTimeTag) { reqExpireTimeTag = _reqExpireTimeTag; }
public java.util.ArrayList<Common.Common_GuildReqDonateCountInfo> getGuildReqDonateCountInfoList() { return guildReqDonateCountInfoList; }
public void addGuildReqDonateCountInfoList(Common.Common_GuildReqDonateCountInfo _guildReqDonateCountInfoList) { guildReqDonateCountInfoList.add(_guildReqDonateCountInfoList); }


public final int GetBufSize() {
	int _size = 32;
	_size += 4 + reqGuildMemberInfo.GetBufSize();
	_size += 2 + (guildReqDonateCountInfoList.size() * 14);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 34;
	_size += 4 + reqGuildMemberInfo.GetBufSize();
	_size += 2 + (guildReqDonateCountInfoList.size() * 14);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) id = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _reqGuildMemberInfoCustLen = _buf.getInt();
	int _reqGuildMemberInfoCurPos = _buf.position();
	reqGuildMemberInfo.ReadUnzipBuf(_buf, _reqGuildMemberInfoCurPos + _reqGuildMemberInfoCustLen);
	_buf.position(_reqGuildMemberInfoCurPos + _reqGuildMemberInfoCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cardId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) donateCount = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) reqDonateCountLimit = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) reqTimeTag = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) reqExpireTimeTag = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _guildReqDonateCountInfoListCount = _buf.getShort();
	for(int _i = 0; _i < _guildReqDonateCountInfoListCount; _i++) { 
		Common.Common_GuildReqDonateCountInfo _guildReqDonateCountInfoList = new Common.Common_GuildReqDonateCountInfo();
		if(_buf.remaining() <= 0) return;
	int __guildReqDonateCountInfoListCustLen = _buf.getInt();
	int __guildReqDonateCountInfoListCurPos = _buf.position();
	_guildReqDonateCountInfoList.ReadUnzipBuf(_buf, __guildReqDonateCountInfoListCurPos + __guildReqDonateCountInfoListCustLen);
	_buf.position(__guildReqDonateCountInfoListCurPos + __guildReqDonateCountInfoListCustLen);

		guildReqDonateCountInfoList.add(_guildReqDonateCountInfoList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(id);
	_buf.putInt(reqGuildMemberInfo.GetBufSize());
	reqGuildMemberInfo.PutUnzipBuf(_buf);
	_buf.putLong(cardId);
	_buf.putInt(donateCount);
	_buf.putInt(reqDonateCountLimit);
	_buf.putInt(reqTimeTag);
	_buf.putInt(reqExpireTimeTag);
	_buf.putShort((short)guildReqDonateCountInfoList.size());
	for(int _i = 0; _i < guildReqDonateCountInfoList.size(); _i++) { 
		_buf.putInt(guildReqDonateCountInfoList.get(_i).GetBufSize());
	guildReqDonateCountInfoList.get(_i).PutUnzipBuf(_buf);
	}
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

