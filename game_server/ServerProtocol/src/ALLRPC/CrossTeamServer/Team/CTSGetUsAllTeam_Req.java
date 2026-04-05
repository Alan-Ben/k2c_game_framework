package ALLRPC.CrossTeamServer.Team;

import java.nio.ByteBuffer;
public class CTSGetUsAllTeam_Req implements ALBasicProtocolPack._IALProtocolStructure {
private int usId;
/** 分组实例ID列表 */
private java.util.ArrayList<Long> groupIdList;


public CTSGetUsAllTeam_Req() {
	usId = 0;
	groupIdList = new java.util.ArrayList<Long>();
}

public CTSGetUsAllTeam_Req(
	 int _usId
	, java.util.ArrayList<Long> _groupIdList
) {	usId = _usId;
	groupIdList = _groupIdList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public int getUsId() { return usId; }
public void setUsId(int _usId) { usId = _usId; }
/** 分组实例ID列表 */
public java.util.ArrayList<Long> getGroupIdList() { return groupIdList; }
/** 分组实例ID列表 */
public void addGroupIdList(long _groupIdList) { groupIdList.add(_groupIdList); }


public final int GetBufSize() {
	int _size = 4;
	_size += 2 + (groupIdList.size() * 8);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 6;
	_size += 2 + (groupIdList.size() * 8);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) usId = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _groupIdListCount = _buf.getShort();
	for(int _i = 0; _i < _groupIdListCount; _i++) { 
		long _groupIdList = (long)0;
		if(_buf.remaining() > 0) _groupIdList = _buf.getLong();
		groupIdList.add(_groupIdList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(usId);
	_buf.putShort((short)groupIdList.size());
	for(int _i = 0; _i < groupIdList.size(); _i++) { 
		_buf.putLong(groupIdList.get(_i));
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

