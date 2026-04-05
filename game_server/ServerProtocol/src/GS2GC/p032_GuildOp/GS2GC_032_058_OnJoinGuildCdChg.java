package GS2GC.p032_GuildOp;

import java.nio.ByteBuffer;
public class GS2GC_032_058_OnJoinGuildCdChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 加入联盟CD信息 */
private Common.GuildObj.Guild_JoinCdInfo joinCdInfo;


public GS2GC_032_058_OnJoinGuildCdChg() {
	joinCdInfo = new Common.GuildObj.Guild_JoinCdInfo();
}

public GS2GC_032_058_OnJoinGuildCdChg(
	 Common.GuildObj.Guild_JoinCdInfo _joinCdInfo
) {	joinCdInfo = _joinCdInfo;
}

public final byte getMainOrder() { return (byte)32; }

public final byte getSubOrder() { return (byte)58; }

/** 加入联盟CD信息 */
public Common.GuildObj.Guild_JoinCdInfo getJoinCdInfo() { return joinCdInfo; }
/** 加入联盟CD信息 */
public void setJoinCdInfo(Common.GuildObj.Guild_JoinCdInfo _joinCdInfo) { joinCdInfo = _joinCdInfo; }


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
	int _joinCdInfoCustLen = _buf.getInt();
	int _joinCdInfoCurPos = _buf.position();
	joinCdInfo.ReadUnzipBuf(_buf, _joinCdInfoCurPos + _joinCdInfoCustLen);
	_buf.position(_joinCdInfoCurPos + _joinCdInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(joinCdInfo.GetBufSize());
	joinCdInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)58);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
	_recBuf.put((byte)58);
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

