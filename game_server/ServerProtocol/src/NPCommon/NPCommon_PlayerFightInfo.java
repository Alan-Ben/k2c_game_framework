package NPCommon;

import java.nio.ByteBuffer;
/*********
 * 玩家战斗初始化数据
 **/
public class NPCommon_PlayerFightInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 阵容位置和血量列表 */
private NPCommon.NPCommon_LineupSettingInfo lineupList;
/** 卡牌属性列表 */
private java.util.ArrayList<NPCommon.NPCommon_BattleCardInfo> petList;


public NPCommon_PlayerFightInfo() {
	lineupList = new NPCommon.NPCommon_LineupSettingInfo();
	petList = new java.util.ArrayList<NPCommon.NPCommon_BattleCardInfo>();
}

public NPCommon_PlayerFightInfo(
	 NPCommon.NPCommon_LineupSettingInfo _lineupList
	, java.util.ArrayList<NPCommon.NPCommon_BattleCardInfo> _petList
) {	lineupList = _lineupList;
	petList = _petList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 阵容位置和血量列表 */
public NPCommon.NPCommon_LineupSettingInfo getLineupList() { return lineupList; }
/** 阵容位置和血量列表 */
public void setLineupList(NPCommon.NPCommon_LineupSettingInfo _lineupList) { lineupList = _lineupList; }
/** 卡牌属性列表 */
public java.util.ArrayList<NPCommon.NPCommon_BattleCardInfo> getPetList() { return petList; }
/** 卡牌属性列表 */
public void addPetList(NPCommon.NPCommon_BattleCardInfo _petList) { petList.add(_petList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + lineupList.GetBufSize();
	_size += 2;
	for(int _i = 0; _i < petList.size(); _i++) {
	_size += 4 + petList.get(_i).GetBufSize();
	}


	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + lineupList.GetBufSize();
	_size += 2;
	for(int _i = 0; _i < petList.size(); _i++) {
	_size += 4 + petList.get(_i).GetBufSize();
	}


	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _lineupListCustLen = _buf.getInt();
	int _lineupListCurPos = _buf.position();
	lineupList.ReadUnzipBuf(_buf, _lineupListCurPos + _lineupListCustLen);
	_buf.position(_lineupListCurPos + _lineupListCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _petListCount = _buf.getShort();
	for(int _i = 0; _i < _petListCount; _i++) { 
		NPCommon.NPCommon_BattleCardInfo _petList = new NPCommon.NPCommon_BattleCardInfo();
		if(_buf.remaining() <= 0) return;
	int __petListCustLen = _buf.getInt();
	int __petListCurPos = _buf.position();
	_petList.ReadUnzipBuf(_buf, __petListCurPos + __petListCustLen);
	_buf.position(__petListCurPos + __petListCustLen);

		petList.add(_petList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(lineupList.GetBufSize());
	lineupList.PutUnzipBuf(_buf);
	_buf.putShort((short)petList.size());
	for(int _i = 0; _i < petList.size(); _i++) { 
		_buf.putInt(petList.get(_i).GetBufSize());
	petList.get(_i).PutUnzipBuf(_buf);
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

