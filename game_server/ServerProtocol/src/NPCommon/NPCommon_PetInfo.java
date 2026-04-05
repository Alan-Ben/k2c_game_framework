package NPCommon;

import java.nio.ByteBuffer;
/*********
 * 宠物通用数据
 **/
public class NPCommon_PetInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 宠物配置ID */
private long petRefId;
/** 宠物等级 */
private int petLevel;
/** 宠物皮肤 */
private long petSkin;


public NPCommon_PetInfo() {
	petRefId = (long)0;
	petLevel = 0;
	petSkin = (long)0;
}

public NPCommon_PetInfo(
	 long _petRefId
	, int _petLevel
	, long _petSkin
) {	petRefId = _petRefId;
	petLevel = _petLevel;
	petSkin = _petSkin;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 宠物配置ID */
public long getPetRefId() { return petRefId; }
/** 宠物配置ID */
public void setPetRefId(long _petRefId) { petRefId = _petRefId; }
/** 宠物等级 */
public int getPetLevel() { return petLevel; }
/** 宠物等级 */
public void setPetLevel(int _petLevel) { petLevel = _petLevel; }
/** 宠物皮肤 */
public long getPetSkin() { return petSkin; }
/** 宠物皮肤 */
public void setPetSkin(long _petSkin) { petSkin = _petSkin; }


public final int GetBufSize() {
	int _size = 20;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 22;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) petRefId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) petLevel = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) petSkin = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(petRefId);
	_buf.putInt(petLevel);
	_buf.putLong(petSkin);
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

