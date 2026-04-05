package Common.FriendObj;

import java.nio.ByteBuffer;
/*********
 * 自定义好友分组数据
 **/
public class Friend_CustomGroupInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 数据库id */
private long dbId;
/** 分组名 */
private String name;
/** 好友列表 */
private java.util.ArrayList<Long> cidList;


public Friend_CustomGroupInfo() {
	dbId = (long)0;
	name = "";
	cidList = new java.util.ArrayList<Long>();
}

public Friend_CustomGroupInfo(
	 long _dbId
	, String _name
	, java.util.ArrayList<Long> _cidList
) {	dbId = _dbId;
	name = _name;
	cidList = _cidList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 数据库id */
public long getDbId() { return dbId; }
/** 数据库id */
public void setDbId(long _dbId) { dbId = _dbId; }
/** 分组名 */
public String getName() { return name; }
/** 分组名 */
public void setName(String _name) { name = _name; }
/** 好友列表 */
public java.util.ArrayList<Long> getCidList() { return cidList; }
/** 好友列表 */
public void addCidList(long _cidList) { cidList.add(_cidList); }


public final int GetBufSize() {
	int _size = 8;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(name);
	_size += 2 + (cidList.size() * 8);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(name);
	_size += 2 + (cidList.size() * 8);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) dbId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) name = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _cidListCount = _buf.getShort();
	for(int _i = 0; _i < _cidListCount; _i++) { 
		long _cidList = (long)0;
		if(_buf.remaining() > 0) _cidList = _buf.getLong();
		cidList.add(_cidList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(dbId);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, name);
	_buf.putShort((short)cidList.size());
	for(int _i = 0; _i < cidList.size(); _i++) { 
		_buf.putLong(cidList.get(_i));
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

