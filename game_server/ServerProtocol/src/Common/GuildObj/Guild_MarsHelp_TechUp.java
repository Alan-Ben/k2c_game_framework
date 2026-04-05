package Common.GuildObj;

import java.nio.ByteBuffer;
/*********
 * 联盟火星求助展示信息-火星科技升级
 **/
public class Guild_MarsHelp_TechUp implements ALBasicProtocolPack._IALProtocolStructure {
private long techId;
private int lvl;


public Guild_MarsHelp_TechUp() {
	techId = (long)0;
	lvl = 0;
}

public Guild_MarsHelp_TechUp(
	 long _techId
	, int _lvl
) {	techId = _techId;
	lvl = _lvl;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getTechId() { return techId; }
public void setTechId(long _techId) { techId = _techId; }
public int getLvl() { return lvl; }
public void setLvl(int _lvl) { lvl = _lvl; }


public final int GetBufSize() {
	int _size = 12;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 14;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) techId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) lvl = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(techId);
	_buf.putInt(lvl);
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

