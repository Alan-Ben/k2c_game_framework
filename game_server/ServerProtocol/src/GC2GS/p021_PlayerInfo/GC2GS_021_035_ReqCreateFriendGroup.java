package GC2GS.p021_PlayerInfo;

import java.nio.ByteBuffer;
/*********
 * 创建好友分组
 **/
public class GC2GS_021_035_ReqCreateFriendGroup implements ALBasicProtocolPack._IALProtocolStructure {
private String groupName;
/** 好友列表 */
private java.util.ArrayList<Long> cidList;


public GC2GS_021_035_ReqCreateFriendGroup() {
	groupName = "";
	cidList = new java.util.ArrayList<Long>();
}

public GC2GS_021_035_ReqCreateFriendGroup(
	 String _groupName
	, java.util.ArrayList<Long> _cidList
) {	groupName = _groupName;
	cidList = _cidList;
}

public final byte getMainOrder() { return (byte)21; }

public final byte getSubOrder() { return (byte)35; }

public String getGroupName() { return groupName; }
public void setGroupName(String _groupName) { groupName = _groupName; }
/** 好友列表 */
public java.util.ArrayList<Long> getCidList() { return cidList; }
/** 好友列表 */
public void addCidList(long _cidList) { cidList.add(_cidList); }


public final int GetBufSize() {
	int _size = 0;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(groupName);
	_size += 2 + (cidList.size() * 8);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(groupName);
	_size += 2 + (cidList.size() * 8);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) groupName = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
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
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, groupName);
	_buf.putShort((short)cidList.size());
	for(int _i = 0; _i < cidList.size(); _i++) { 
		_buf.putLong(cidList.get(_i));
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)21);
	_buf.put((byte)35);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)21);
	_recBuf.put((byte)35);
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

