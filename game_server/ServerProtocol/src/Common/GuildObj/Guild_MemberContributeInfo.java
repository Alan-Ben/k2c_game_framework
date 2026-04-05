package Common.GuildObj;

import java.nio.ByteBuffer;
/*********
 * 联盟成员贡献信息
 **/
public class Guild_MemberContributeInfo implements ALBasicProtocolPack._IALProtocolStructure {
private long cid;
/** 7日贡献度 */
private long sevenDaysContribute;
/** 总贡献度 */
private long totalContribute;


public Guild_MemberContributeInfo() {
	cid = (long)0;
	sevenDaysContribute = (long)0;
	totalContribute = (long)0;
}

public Guild_MemberContributeInfo(
	 long _cid
	, long _sevenDaysContribute
	, long _totalContribute
) {	cid = _cid;
	sevenDaysContribute = _sevenDaysContribute;
	totalContribute = _totalContribute;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getCid() { return cid; }
public void setCid(long _cid) { cid = _cid; }
/** 7日贡献度 */
public long getSevenDaysContribute() { return sevenDaysContribute; }
/** 7日贡献度 */
public void setSevenDaysContribute(long _sevenDaysContribute) { sevenDaysContribute = _sevenDaysContribute; }
/** 总贡献度 */
public long getTotalContribute() { return totalContribute; }
/** 总贡献度 */
public void setTotalContribute(long _totalContribute) { totalContribute = _totalContribute; }


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
	if(_buf.remaining() > 0) cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) sevenDaysContribute = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) totalContribute = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(cid);
	_buf.putLong(sevenDaysContribute);
	_buf.putLong(totalContribute);
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

