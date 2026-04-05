package GS2GC.p037_GuildDungeonOp;

import java.nio.ByteBuffer;
/*********
 * 公会副本-出战大臣数据变更
 **/
public class GS2GC_037_054_OnDungeonHeroFightChg implements ALBasicProtocolPack._IALProtocolStructure {
private Common.GuildDungeonObj.GuildDungeon_FightHero info;


public GS2GC_037_054_OnDungeonHeroFightChg() {
	info = new Common.GuildDungeonObj.GuildDungeon_FightHero();
}

public GS2GC_037_054_OnDungeonHeroFightChg(
	 Common.GuildDungeonObj.GuildDungeon_FightHero _info
) {	info = _info;
}

public final byte getMainOrder() { return (byte)37; }

public final byte getSubOrder() { return (byte)54; }

public Common.GuildDungeonObj.GuildDungeon_FightHero getInfo() { return info; }
public void setInfo(Common.GuildDungeonObj.GuildDungeon_FightHero _info) { info = _info; }


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
	int _infoCustLen = _buf.getInt();
	int _infoCurPos = _buf.position();
	info.ReadUnzipBuf(_buf, _infoCurPos + _infoCustLen);
	_buf.position(_infoCurPos + _infoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(info.GetBufSize());
	info.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)37);
	_buf.put((byte)54);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)37);
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

