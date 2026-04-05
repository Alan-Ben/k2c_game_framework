package GS2GC.p037_GuildDungeonOp;

import java.nio.ByteBuffer;
/*********
 * 公会副本-标签怪物变更
 **/
public class GS2GC_037_055_OnDungeonTagMonsterChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 公会实例ID */
private long id;
private java.util.ArrayList<Long> tagMonsterIdList;


public GS2GC_037_055_OnDungeonTagMonsterChg() {
	id = (long)0;
	tagMonsterIdList = new java.util.ArrayList<Long>();
}

public GS2GC_037_055_OnDungeonTagMonsterChg(
	 long _id
	, java.util.ArrayList<Long> _tagMonsterIdList
) {	id = _id;
	tagMonsterIdList = _tagMonsterIdList;
}

public final byte getMainOrder() { return (byte)37; }

public final byte getSubOrder() { return (byte)55; }

/** 公会实例ID */
public long getId() { return id; }
/** 公会实例ID */
public void setId(long _id) { id = _id; }
public java.util.ArrayList<Long> getTagMonsterIdList() { return tagMonsterIdList; }
public void addTagMonsterIdList(long _tagMonsterIdList) { tagMonsterIdList.add(_tagMonsterIdList); }


public final int GetBufSize() {
	int _size = 8;
	_size += 2 + (tagMonsterIdList.size() * 8);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += 2 + (tagMonsterIdList.size() * 8);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) id = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _tagMonsterIdListCount = _buf.getShort();
	for(int _i = 0; _i < _tagMonsterIdListCount; _i++) { 
		long _tagMonsterIdList = (long)0;
		if(_buf.remaining() > 0) _tagMonsterIdList = _buf.getLong();
		tagMonsterIdList.add(_tagMonsterIdList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(id);
	_buf.putShort((short)tagMonsterIdList.size());
	for(int _i = 0; _i < tagMonsterIdList.size(); _i++) { 
		_buf.putLong(tagMonsterIdList.get(_i));
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)37);
	_buf.put((byte)55);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)37);
	_recBuf.put((byte)55);
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

