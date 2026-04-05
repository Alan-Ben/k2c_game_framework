package GC2GS.p021_PlayerInfo;

import java.nio.ByteBuffer;
/*********
 * 变更好友分组的名称
 **/
public class GC2GS_021_039_ReqChgFriendGroupName implements ALBasicProtocolPack._IALProtocolStructure {
/** 目标分组数据id */
private long groupDbId;
private String name;


public GC2GS_021_039_ReqChgFriendGroupName() {
	groupDbId = (long)0;
	name = "";
}

public GC2GS_021_039_ReqChgFriendGroupName(
	 long _groupDbId
	, String _name
) {	groupDbId = _groupDbId;
	name = _name;
}

public final byte getMainOrder() { return (byte)21; }

public final byte getSubOrder() { return (byte)39; }

/** 目标分组数据id */
public long getGroupDbId() { return groupDbId; }
/** 目标分组数据id */
public void setGroupDbId(long _groupDbId) { groupDbId = _groupDbId; }
public String getName() { return name; }
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
	_buf.put((byte)39);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)21);
	_recBuf.put((byte)39);
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

