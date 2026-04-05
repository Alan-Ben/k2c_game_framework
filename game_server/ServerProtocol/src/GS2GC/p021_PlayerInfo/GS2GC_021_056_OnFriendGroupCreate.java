package GS2GC.p021_PlayerInfo;

import java.nio.ByteBuffer;
/*********
 * 创建分组
 **/
public class GS2GC_021_056_OnFriendGroupCreate implements ALBasicProtocolPack._IALProtocolStructure {
/** 分组数据id */
private long groupDbId;
/** 分组名 */
private String name;


public GS2GC_021_056_OnFriendGroupCreate() {
	groupDbId = (long)0;
	name = "";
}

public GS2GC_021_056_OnFriendGroupCreate(
	 long _groupDbId
	, String _name
) {	groupDbId = _groupDbId;
	name = _name;
}

public final byte getMainOrder() { return (byte)21; }

public final byte getSubOrder() { return (byte)56; }

/** 分组数据id */
public long getGroupDbId() { return groupDbId; }
/** 分组数据id */
public void setGroupDbId(long _groupDbId) { groupDbId = _groupDbId; }
/** 分组名 */
public String getName() { return name; }
/** 分组名 */
public void setName(String _name) { name = _name; }


public final int GetBufSize() {
	int _size = 8;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(name);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(name);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) groupDbId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) name = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(groupDbId);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, name);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)21);
	_buf.put((byte)56);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)21);
	_recBuf.put((byte)56);
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

