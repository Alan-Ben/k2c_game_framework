package GC2GS.p032_GuildOp;

import java.nio.ByteBuffer;
/*********
 * 切换联盟加入类型
 **/
public class GC2GS_032_012_ReqSetGuildJoinType implements ALBasicProtocolPack._IALProtocolStructure {
private Common.GuildEnum.EGuildJoinType type;
private java.util.ArrayList<Common.GuildObj.Guild_JoinLimitInfo> joinLimitInfo;


public GC2GS_032_012_ReqSetGuildJoinType() {
	type = Common.GuildEnum.EGuildJoinType.values()[0];
	joinLimitInfo = new java.util.ArrayList<Common.GuildObj.Guild_JoinLimitInfo>();
}

public GC2GS_032_012_ReqSetGuildJoinType(
	 Common.GuildEnum.EGuildJoinType _type
	, java.util.ArrayList<Common.GuildObj.Guild_JoinLimitInfo> _joinLimitInfo
) {	type = _type;
	joinLimitInfo = _joinLimitInfo;
}

public final byte getMainOrder() { return (byte)32; }

public final byte getSubOrder() { return (byte)12; }

public Common.GuildEnum.EGuildJoinType getType() { return type; }
public void setType(Common.GuildEnum.EGuildJoinType _type) { type = _type; }
public java.util.ArrayList<Common.GuildObj.Guild_JoinLimitInfo> getJoinLimitInfo() { return joinLimitInfo; }
public void addJoinLimitInfo(Common.GuildObj.Guild_JoinLimitInfo _joinLimitInfo) { joinLimitInfo.add(_joinLimitInfo); }


public final int GetBufSize() {
	int _size = 4;
	_size += 2 + (joinLimitInfo.size() * 16);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 6;
	_size += 2 + (joinLimitInfo.size() * 16);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) type = Common.GuildEnum.EGuildJoinType.EGuildJoinType_FromInt(_buf.getInt());
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
	_buf.putInt(type.ordinal());

	_buf.putShort((short)joinLimitInfo.size());
	for(int _i = 0; _i < joinLimitInfo.size(); _i++) { 
		_buf.putInt(joinLimitInfo.get(_i).GetBufSize());
	joinLimitInfo.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)12);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
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

