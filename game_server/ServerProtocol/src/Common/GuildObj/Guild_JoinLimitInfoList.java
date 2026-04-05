package Common.GuildObj;

import java.nio.ByteBuffer;
/*********
 * 加入限制信息列表
 **/
public class Guild_JoinLimitInfoList implements ALBasicProtocolPack._IALProtocolStructure {
/** 加入限制信息 */
private java.util.ArrayList<Common.GuildObj.Guild_JoinLimitInfo> joinLimitInfo;


public Guild_JoinLimitInfoList() {
	joinLimitInfo = new java.util.ArrayList<Common.GuildObj.Guild_JoinLimitInfo>();
}

public Guild_JoinLimitInfoList(
	 java.util.ArrayList<Common.GuildObj.Guild_JoinLimitInfo> _joinLimitInfo
) {	joinLimitInfo = _joinLimitInfo;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 加入限制信息 */
public java.util.ArrayList<Common.GuildObj.Guild_JoinLimitInfo> getJoinLimitInfo() { return joinLimitInfo; }
/** 加入限制信息 */
public void addJoinLimitInfo(Common.GuildObj.Guild_JoinLimitInfo _joinLimitInfo) { joinLimitInfo.add(_joinLimitInfo); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (joinLimitInfo.size() * 16);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (joinLimitInfo.size() * 16);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _joinLimitInfoCount = _buf.getShort();
	for(int _i = 0; _i < _joinLimitInfoCount; _i++) { 
		Common.GuildObj.Guild_JoinLimitInfo _joinLimitInfo = new Common.GuildObj.Guild_JoinLimitInfo();
		if(_buf.remaining() <= 0) return;
	int __joinLimitInfoCustLen = _buf.getInt();
	int __joinLimitInfoCurPos = _buf.position();
	_joinLimitInfo.ReadUnzipBuf(_buf, __joinLimitInfoCurPos + __joinLimitInfoCustLen);
	_buf.position(__joinLimitInfoCurPos + __joinLimitInfoCustLen);

		joinLimitInfo.add(_joinLimitInfo);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)joinLimitInfo.size());
	for(int _i = 0; _i < joinLimitInfo.size(); _i++) { 
		_buf.putInt(joinLimitInfo.get(_i).GetBufSize());
	joinLimitInfo.get(_i).PutUnzipBuf(_buf);
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

