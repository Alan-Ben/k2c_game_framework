package Common.TreasureHuntObj;

import java.nio.ByteBuffer;
/*********
 * 太空寻宝-奇物信息
 **/
public class TreasureHunt_TreasureInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 奇物ID */
private long treasureId;
/** 技能等级 */
private int skillLevel;
/** 获得时间 ms */
private long gainTimeMs;


public TreasureHunt_TreasureInfo() {
	treasureId = (long)0;
	skillLevel = 0;
	gainTimeMs = (long)0;
}

public TreasureHunt_TreasureInfo(
	 long _treasureId
	, int _skillLevel
	, long _gainTimeMs
) {	treasureId = _treasureId;
	skillLevel = _skillLevel;
	gainTimeMs = _gainTimeMs;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 奇物ID */
public long getTreasureId() { return treasureId; }
/** 奇物ID */
public void setTreasureId(long _treasureId) { treasureId = _treasureId; }
/** 技能等级 */
public int getSkillLevel() { return skillLevel; }
/** 技能等级 */
public void setSkillLevel(int _skillLevel) { skillLevel = _skillLevel; }
/** 获得时间 ms */
public long getGainTimeMs() { return gainTimeMs; }
/** 获得时间 ms */
public void setGainTimeMs(long _gainTimeMs) { gainTimeMs = _gainTimeMs; }


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
	if(_buf.remaining() > 0) treasureId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) skillLevel = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) gainTimeMs = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(treasureId);
	_buf.putInt(skillLevel);
	_buf.putLong(gainTimeMs);
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

