package Common.GuildObj;

import java.nio.ByteBuffer;
/*********
 * 联盟今日建造信息
 **/
public class Guild_ConstructInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 建造类型 */
private Common.GuildEnum.EGuildConstructType type;
/** 已建造次数 */
private int num;
/** 获得联盟经验 */
private long gainGuildExpCount;
/** 获得联盟财富 */
private long gainGuildWealthCount;


public Guild_ConstructInfo() {
	type = Common.GuildEnum.EGuildConstructType.values()[0];
	num = 0;
	gainGuildExpCount = (long)0;
	gainGuildWealthCount = (long)0;
}

public Guild_ConstructInfo(
	 Common.GuildEnum.EGuildConstructType _type
	, int _num
	, long _gainGuildExpCount
	, long _gainGuildWealthCount
) {	type = _type;
	num = _num;
	gainGuildExpCount = _gainGuildExpCount;
	gainGuildWealthCount = _gainGuildWealthCount;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 建造类型 */
public Common.GuildEnum.EGuildConstructType getType() { return type; }
/** 建造类型 */
public void setType(Common.GuildEnum.EGuildConstructType _type) { type = _type; }
/** 已建造次数 */
public int getNum() { return num; }
/** 已建造次数 */
public void setNum(int _num) { num = _num; }
/** 获得联盟经验 */
public long getGainGuildExpCount() { return gainGuildExpCount; }
/** 获得联盟经验 */
public void setGainGuildExpCount(long _gainGuildExpCount) { gainGuildExpCount = _gainGuildExpCount; }
/** 获得联盟财富 */
public long getGainGuildWealthCount() { return gainGuildWealthCount; }
/** 获得联盟财富 */
public void setGainGuildWealthCount(long _gainGuildWealthCount) { gainGuildWealthCount = _gainGuildWealthCount; }


public final int GetBufSize() {
	int _size = 24;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 26;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) type = Common.GuildEnum.EGuildConstructType.EGuildConstructType_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) num = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) gainGuildExpCount = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) gainGuildWealthCount = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(type.ordinal());

	_buf.putInt(num);
	_buf.putLong(gainGuildExpCount);
	_buf.putLong(gainGuildWealthCount);
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

