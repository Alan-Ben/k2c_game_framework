using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common
{

public class Common_Lineup : ALBasicProtocolPack._IALProtocolStructure {
private long raceId;
private List<Common.Common_ActorInfo> cardList;
private List<long> cmdSkillIds;


public Common_Lineup() {
	raceId = (long)0;
	cardList = new List<Common.Common_ActorInfo>();
	cmdSkillIds = new List<long>();
}

public Common_Lineup(
	long _raceId
	, List<Common.Common_ActorInfo> _cardList
	, List<long> _cmdSkillIds
) {	raceId = _raceId;
	cardList = _cardList;
	cmdSkillIds = _cmdSkillIds;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public long getRaceId() { return raceId; }
public void setRaceId(long _raceId) { raceId = _raceId; }
public List<Common.Common_ActorInfo> getCardList() { return cardList; }
public void addCardList(Common.Common_ActorInfo _cardList) { cardList.Add(_cardList); }
public List<long> getCmdSkillIds() { return cmdSkillIds; }
public void addCmdSkillIds(long _cmdSkillIds) { cmdSkillIds.Add(_cmdSkillIds); }


public int GetBufSize() {
	int _size = 8;
	_size += 2;
for(int _i = 0; _i < cardList.Count; _i++) {
	_size += 4 + cardList[_i].GetBufSize();
	}

	_size += 2 + (cmdSkillIds.Count * 8);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;
	_size += 2;
for(int _i = 0; _i < cardList.Count; _i++) {
	_size += 4 + cardList[_i].GetBufSize();
	}

	_size += 2 + (cmdSkillIds.Count * 8);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	raceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _cardListCount = _buf.getShort();
	for(int _i = 0; _i < _cardListCount; _i++) { 
		Common.Common_ActorInfo _cardList = new Common.Common_ActorInfo();
		int __cardListCustLen = _buf.getInt();
	int __cardListCurPos = _buf.getCurPos();
	_cardList.ReadUnzipBuf(_buf, __cardListCurPos + __cardListCustLen);
	_buf.setPosition(__cardListCurPos + __cardListCustLen);

		cardList.Add(_cardList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _cmdSkillIdsCount = _buf.getShort();
	for(int _i = 0; _i < _cmdSkillIdsCount; _i++) { 
		long _cmdSkillIds = (long)0;
		_cmdSkillIds = _buf.getLong();
		cmdSkillIds.Add(_cmdSkillIds);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(raceId);
	_buf.putShort((short)cardList.Count);
	for(int _i = 0; _i < cardList.Count; _i++) { 
		_buf.putInt(cardList[_i].GetBufSize());
	cardList[_i].PutUnzipBuf(_buf);
	}
	_buf.putShort((short)cmdSkillIds.Count);
	for(int _i = 0; _i < cmdSkillIds.Count; _i++) { 
		_buf.putLong(cmdSkillIds[_i]);
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
	builder.Append("raceId").Append(":").Append(raceId.ToString()).Append(", ");
	builder.Append("cardList").Append(":").Append(cardList.ToString()).Append(", ");
	builder.Append("cmdSkillIds").Append(":").Append(cmdSkillIds.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

