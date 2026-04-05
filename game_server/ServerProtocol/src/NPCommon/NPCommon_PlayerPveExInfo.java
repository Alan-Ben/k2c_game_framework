package NPCommon;

import java.nio.ByteBuffer;
/*********
 * PVE关卡其他信息结构体
 **/
public class NPCommon_PlayerPveExInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 怪物血量列表 */
private java.util.ArrayList<NPCommon.NPCommon_BattleMobHp> mobHpList;


public NPCommon_PlayerPveExInfo() {
	mobHpList = new java.util.ArrayList<NPCommon.NPCommon_BattleMobHp>();
}

public NPCommon_PlayerPveExInfo(
	 java.util.ArrayList<NPCommon.NPCommon_BattleMobHp> _mobHpList
) {	mobHpList = _mobHpList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 怪物血量列表 */
public java.util.ArrayList<NPCommon.NPCommon_BattleMobHp> getMobHpList() { return mobHpList; }
/** 怪物血量列表 */
public void addMobHpList(NPCommon.NPCommon_BattleMobHp _mobHpList) { mobHpList.add(_mobHpList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (mobHpList.size() * 18);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (mobHpList.size() * 18);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _mobHpListCount = _buf.getShort();
	for(int _i = 0; _i < _mobHpListCount; _i++) { 
		NPCommon.NPCommon_BattleMobHp _mobHpList = new NPCommon.NPCommon_BattleMobHp();
		if(_buf.remaining() <= 0) return;
	int __mobHpListCustLen = _buf.getInt();
	int __mobHpListCurPos = _buf.position();
	_mobHpList.ReadUnzipBuf(_buf, __mobHpListCurPos + __mobHpListCustLen);
	_buf.position(__mobHpListCurPos + __mobHpListCustLen);

		mobHpList.add(_mobHpList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)mobHpList.size());
	for(int _i = 0; _i < mobHpList.size(); _i++) { 
		_buf.putInt(mobHpList.get(_i).GetBufSize());
	mobHpList.get(_i).PutUnzipBuf(_buf);
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

