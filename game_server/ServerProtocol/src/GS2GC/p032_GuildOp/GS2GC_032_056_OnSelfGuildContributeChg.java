package GS2GC.p032_GuildOp;

import java.nio.ByteBuffer;
public class GS2GC_032_056_OnSelfGuildContributeChg implements ALBasicProtocolPack._IALProtocolStructure {
private Common.GuildObj.Guild_MemberContributeInfo contributeInfo;


public GS2GC_032_056_OnSelfGuildContributeChg() {
	contributeInfo = new Common.GuildObj.Guild_MemberContributeInfo();
}

public GS2GC_032_056_OnSelfGuildContributeChg(
	 Common.GuildObj.Guild_MemberContributeInfo _contributeInfo
) {	contributeInfo = _contributeInfo;
}

public final byte getMainOrder() { return (byte)32; }

public final byte getSubOrder() { return (byte)56; }

public Common.GuildObj.Guild_MemberContributeInfo getContributeInfo() { return contributeInfo; }
public void setContributeInfo(Common.GuildObj.Guild_MemberContributeInfo _contributeInfo) { contributeInfo = _contributeInfo; }


public final int GetBufSize() {
	int _size = 28;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 30;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _contributeInfoCustLen = _buf.getInt();
	int _contributeInfoCurPos = _buf.position();
	contributeInfo.ReadUnzipBuf(_buf, _contributeInfoCurPos + _contributeInfoCustLen);
	_buf.position(_contributeInfoCurPos + _contributeInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(contributeInfo.GetBufSize());
	contributeInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)56);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
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

