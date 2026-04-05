package GS2GC.p037_GuildDungeonOp;

import java.nio.ByteBuffer;
public class GS2GC_037_051_OnDungeonInstanceChg implements ALBasicProtocolPack._IALProtocolStructure {
private Common.GuildDungeonObj.GuildDungeon_InstanceInfo instanceInfo;


public GS2GC_037_051_OnDungeonInstanceChg() {
	instanceInfo = new Common.GuildDungeonObj.GuildDungeon_InstanceInfo();
}

public GS2GC_037_051_OnDungeonInstanceChg(
	 Common.GuildDungeonObj.GuildDungeon_InstanceInfo _instanceInfo
) {	instanceInfo = _instanceInfo;
}

public final byte getMainOrder() { return (byte)37; }

public final byte getSubOrder() { return (byte)51; }

public Common.GuildDungeonObj.GuildDungeon_InstanceInfo getInstanceInfo() { return instanceInfo; }
public void setInstanceInfo(Common.GuildDungeonObj.GuildDungeon_InstanceInfo _instanceInfo) { instanceInfo = _instanceInfo; }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + instanceInfo.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + instanceInfo.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _instanceInfoCustLen = _buf.getInt();
	int _instanceInfoCurPos = _buf.position();
	instanceInfo.ReadUnzipBuf(_buf, _instanceInfoCurPos + _instanceInfoCustLen);
	_buf.position(_instanceInfoCurPos + _instanceInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(instanceInfo.GetBufSize());
	instanceInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)37);
	_buf.put((byte)51);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)37);
	_recBuf.put((byte)51);
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

