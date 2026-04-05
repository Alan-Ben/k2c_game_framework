using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common
{

public class Common_GuildDonateInfo : ALBasicProtocolPack._IALProtocolStructure {
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

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

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


public int GetBufSize() {
	int _size = 32;
	_size += 4 + guildMemberInfo.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 34;
	_size += 4 + guildMemberInfo.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	id = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	reqId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _guildMemberInfoCustLen = _buf.getInt();
	int _guildMemberInfoCurPos = _buf.getCurPos();
	guildMemberInfo.ReadUnzipBuf(_buf, _guildMemberInfoCurPos + _guildMemberInfoCustLen);
	_buf.setPosition(_guildMemberInfoCurPos + _guildMemberInfoCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	cardId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	donateCount = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	timeTag = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(id);
	_buf.putLong(reqId);
	_buf.putInt(guildMemberInfo.GetBufSize());
	guildMemberInfo.PutUnzipBuf(_buf);
	_buf.putLong(cardId);
	_buf.putInt(donateCount);
	_buf.putInt(timeTag);
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
	builder.Append("id").Append(":").Append(id.ToString()).Append(", ");
	builder.Append("reqId").Append(":").Append(reqId.ToString()).Append(", ");
	builder.Append("guildMemberInfo").Append(":").Append(guildMemberInfo == null ? "null" : guildMemberInfo.ToString()).Append(", ");
	builder.Append("cardId").Append(":").Append(cardId.ToString()).Append(", ");
	builder.Append("donateCount").Append(":").Append(donateCount.ToString()).Append(", ");
	builder.Append("timeTag").Append(":").Append(timeTag.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

