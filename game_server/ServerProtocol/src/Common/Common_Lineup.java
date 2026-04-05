package Common;

import java.nio.ByteBuffer;
public class Common_Lineup implements ALBasicProtocolPack._IALProtocolStructure {
private long raceId;
private java.util.ArrayList<Common.Common_ActorInfo> cardList;
private java.util.ArrayList<Long> cmdSkillIds;


public Common_Lineup() {
	raceId = (long)0;
	cardList = new java.util.ArrayList<Common.Common_ActorInfo>();
	cmdSkillIds = new java.util.ArrayList<Long>();
}

public Common_Lineup(
	 long _raceId
	, java.util.ArrayList<Common.Common_ActorInfo> _cardList
	, java.util.ArrayList<Long> _cmdSkillIds
) {	raceId = _raceId;
	cardList = _cardList;
	cmdSkillIds = _cmdSkillIds;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getRaceId() { return raceId; }
public void setRaceId(long _raceId) { raceId = _raceId; }
public java.util.ArrayList<Common.Common_ActorInfo> getCardList() { return cardList; }
public void addCardList(Common.Common_ActorInfo _cardList) { cardList.add(_cardList); }
public java.util.ArrayList<Long> getCmdSkillIds() { return cmdSkillIds; }
public void addCmdSkillIds(long _cmdSkillIds) { cmdSkillIds.add(_cmdSkillIds); }


public final int GetBufSize() {
	int _size = 8;
	_size += 2;
	for(int _i = 0; _i < cardList.size(); _i++) {
	_size += 4 + cardList.get(_i).GetBufSize();
	}

	_size += 2 + (cmdSkillIds.size() * 8);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += 2;
	for(int _i = 0; _i < cardList.size(); _i++) {
	_size += 4 + cardList.get(_i).GetBufSize();
	}

	_size += 2 + (cmdSkillIds.size() * 8);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) raceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _cardListCount = _buf.getShort();
	for(int _i = 0; _i < _cardListCount; _i++) { 
		Common.Common_ActorInfo _cardList = new Common.Common_ActorInfo();
		if(_buf.remaining() <= 0) return;
	int __cardListCustLen = _buf.getInt();
	int __cardListCurPos = _buf.position();
	_cardList.ReadUnzipBuf(_buf, __cardListCurPos + __cardListCustLen);
	_buf.position(__cardListCurPos + __cardListCustLen);

		cardList.add(_cardList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _cmdSkillIdsCount = _buf.getShort();
	for(int _i = 0; _i < _cmdSkillIdsCount; _i++) { 
		long _cmdSkillIds = (long)0;
		if(_buf.remaining() > 0) _cmdSkillIds = _buf.getLong();
		cmdSkillIds.add(_cmdSkillIds);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(raceId);
	_buf.putShort((short)cardList.size());
	for(int _i = 0; _i < cardList.size(); _i++) { 
		_buf.putInt(cardList.get(_i).GetBufSize());
	cardList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putShort((short)cmdSkillIds.size());
	for(int _i = 0; _i < cmdSkillIds.size(); _i++) { 
		_buf.putLong(cmdSkillIds.get(_i));
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

