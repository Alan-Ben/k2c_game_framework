package GC2GS.p037_GuildDungeonOp;

import java.nio.ByteBuffer;
/*********
 * 公会副本-设置怪物标签
 **/
public class GC2GS_037_012_ReqSetTagMonsterList implements ALBasicProtocolPack._IALProtocolStructure {
/** 公会实例ID */
private long id;
/** 怪物ID列表 */
private java.util.ArrayList<Long> monsterIdList;


public GC2GS_037_012_ReqSetTagMonsterList() {
	id = (long)0;
	monsterIdList = new java.util.ArrayList<Long>();
}

public GC2GS_037_012_ReqSetTagMonsterList(
	 long _id
	, java.util.ArrayList<Long> _monsterIdList
) {	id = _id;
	monsterIdList = _monsterIdList;
}

public final byte getMainOrder() { return (byte)37; }

public final byte getSubOrder() { return (byte)12; }

/** 公会实例ID */
public long getId() { return id; }
/** 公会实例ID */
public void setId(long _id) { id = _id; }
/** 怪物ID列表 */
public java.util.ArrayList<Long> getMonsterIdList() { return monsterIdList; }
/** 怪物ID列表 */
public void addMonsterIdList(long _monsterIdList) { monsterIdList.add(_monsterIdList); }


public final int GetBufSize() {
	int _size = 8;
	_size += 2 + (monsterIdList.size() * 8);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += 2 + (monsterIdList.size() * 8);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) id = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _monsterIdListCount = _buf.getShort();
	for(int _i = 0; _i < _monsterIdListCount; _i++) { 
		long _monsterIdList = (long)0;
		if(_buf.remaining() > 0) _monsterIdList = _buf.getLong();
		monsterIdList.add(_monsterIdList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(id);
	_buf.putShort((short)monsterIdList.size());
	for(int _i = 0; _i < monsterIdList.size(); _i++) { 
		_buf.putLong(monsterIdList.get(_i));
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)37);
	_buf.put((byte)12);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)37);
	_recBuf.put((byte)12);
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

