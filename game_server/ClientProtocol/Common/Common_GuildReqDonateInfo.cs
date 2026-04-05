using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common
{

public class Common_GuildReqDonateInfo : ALBasicProtocolPack._IALProtocolStructure {
private long id;
private Common.Common_GuildMemberInfo reqGuildMemberInfo;
private long cardId;
private int donateCount;
private int reqDonateCountLimit;
private int reqTimeTag;
private int reqExpireTimeTag;
private List<Common.Common_GuildReqDonateCountInfo> guildReqDonateCountInfoList;


public Common_GuildReqDonateInfo() {
	id = (long)0;
	reqGuildMemberInfo = new Common.Common_GuildMemberInfo();
	cardId = (long)0;
	donateCount = 0;
	reqDonateCountLimit = 0;
	reqTimeTag = 0;
	reqExpireTimeTag = 0;
	guildReqDonateCountInfoList = new List<Common.Common_GuildReqDonateCountInfo>();
}

public Common_GuildReqDonateInfo(
	long _id
	, Common.Common_GuildMemberInfo _reqGuildMemberInfo
	, long _cardId
	, int _donateCount
	, int _reqDonateCountLimit
	, int _reqTimeTag
	, int _reqExpireTimeTag
	, List<Common.Common_GuildReqDonateCountInfo> _guildReqDonateCountInfoList
) {	id = _id;
	reqGuildMemberInfo = _reqGuildMemberInfo;
	cardId = _cardId;
	donateCount = _donateCount;
	reqDonateCountLimit = _reqDonateCountLimit;
	reqTimeTag = _reqTimeTag;
	reqExpireTimeTag = _reqExpireTimeTag;
	guildReqDonateCountInfoList = _guildReqDonateCountInfoList;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

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
public List<Common.Common_GuildReqDonateCountInfo> getGuildReqDonateCountInfoList() { return guildReqDonateCountInfoList; }
public void addGuildReqDonateCountInfoList(Common.Common_GuildReqDonateCountInfo _guildReqDonateCountInfoList) { guildReqDonateCountInfoList.Add(_guildReqDonateCountInfoList); }


public int GetBufSize() {
	int _size = 32;
	_size += 4 + reqGuildMemberInfo.GetBufSize();
	_size += 2 + (guildReqDonateCountInfoList.Count * 14);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 34;
	_size += 4 + reqGuildMemberInfo.GetBufSize();
	_size += 2 + (guildReqDonateCountInfoList.Count * 14);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	id = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _reqGuildMemberInfoCustLen = _buf.getInt();
	int _reqGuildMemberInfoCurPos = _buf.getCurPos();
	reqGuildMemberInfo.ReadUnzipBuf(_buf, _reqGuildMemberInfoCurPos + _reqGuildMemberInfoCustLen);
	_buf.setPosition(_reqGuildMemberInfoCurPos + _reqGuildMemberInfoCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	cardId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	donateCount = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	reqDonateCountLimit = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	reqTimeTag = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	reqExpireTimeTag = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _guildReqDonateCountInfoListCount = _buf.getShort();
	for(int _i = 0; _i < _guildReqDonateCountInfoListCount; _i++) { 
		Common.Common_GuildReqDonateCountInfo _guildReqDonateCountInfoList = new Common.Common_GuildReqDonateCountInfo();
		int __guildReqDonateCountInfoListCustLen = _buf.getInt();
	int __guildReqDonateCountInfoListCurPos = _buf.getCurPos();
	_guildReqDonateCountInfoList.ReadUnzipBuf(_buf, __guildReqDonateCountInfoListCurPos + __guildReqDonateCountInfoListCustLen);
	_buf.setPosition(__guildReqDonateCountInfoListCurPos + __guildReqDonateCountInfoListCustLen);

		guildReqDonateCountInfoList.Add(_guildReqDonateCountInfoList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(id);
	_buf.putInt(reqGuildMemberInfo.GetBufSize());
	reqGuildMemberInfo.PutUnzipBuf(_buf);
	_buf.putLong(cardId);
	_buf.putInt(donateCount);
	_buf.putInt(reqDonateCountLimit);
	_buf.putInt(reqTimeTag);
	_buf.putInt(reqExpireTimeTag);
	_buf.putShort((short)guildReqDonateCountInfoList.Count);
	for(int _i = 0; _i < guildReqDonateCountInfoList.Count; _i++) { 
		_buf.putInt(guildReqDonateCountInfoList[_i].GetBufSize());
	guildReqDonateCountInfoList[_i].PutUnzipBuf(_buf);
	}
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
	builder.Append("reqGuildMemberInfo").Append(":").Append(reqGuildMemberInfo == null ? "null" : reqGuildMemberInfo.ToString()).Append(", ");
	builder.Append("cardId").Append(":").Append(cardId.ToString()).Append(", ");
	builder.Append("donateCount").Append(":").Append(donateCount.ToString()).Append(", ");
	builder.Append("reqDonateCountLimit").Append(":").Append(reqDonateCountLimit.ToString()).Append(", ");
	builder.Append("reqTimeTag").Append(":").Append(reqTimeTag.ToString()).Append(", ");
	builder.Append("reqExpireTimeTag").Append(":").Append(reqExpireTimeTag.ToString()).Append(", ");
	builder.Append("guildReqDonateCountInfoList").Append(":").Append(guildReqDonateCountInfoList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

