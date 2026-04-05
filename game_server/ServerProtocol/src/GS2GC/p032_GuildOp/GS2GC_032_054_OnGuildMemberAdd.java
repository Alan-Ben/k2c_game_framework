package GS2GC.p032_GuildOp;

import java.nio.ByteBuffer;
public class GS2GC_032_054_OnGuildMemberAdd implements ALBasicProtocolPack._IALProtocolStructure {
/** 成员基础信息 */
private Common.GuildObj.Guild_MemberBaseInfo baseInfo;


public GS2GC_032_054_OnGuildMemberAdd() {
	baseInfo = new Common.GuildObj.Guild_MemberBaseInfo();
}

public GS2GC_032_054_OnGuildMemberAdd(
	 Common.GuildObj.Guild_MemberBaseInfo _baseInfo
) {	baseInfo = _baseInfo;
}

public final byte getMainOrder() { return (byte)32; }

public final byte getSubOrder() { return (byte)54; }

/** 成员基础信息 */
public Common.GuildObj.Guild_MemberBaseInfo getBaseInfo() { return baseInfo; }
/** 成员基础信息 */
public void setBaseInfo(Common.GuildObj.Guild_MemberBaseInfo _baseInfo) { baseInfo = _baseInfo; }


public final int GetBufSize() {
	int _size = 16;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 18;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _baseInfoCustLen = _buf.getInt();
	int _baseInfoCurPos = _buf.position();
	baseInfo.ReadUnzipBuf(_buf, _baseInfoCurPos + _baseInfoCustLen);
	_buf.position(_baseInfoCurPos + _baseInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(baseInfo.GetBufSize());
	baseInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)54);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
	_recBuf.put((byte)54);
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

