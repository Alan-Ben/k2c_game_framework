package Common.GuildObj;

import java.nio.ByteBuffer;
/*********
 * 联盟建造信息列表
 **/
public class Guild_ConstructList implements ALBasicProtocolPack._IALProtocolStructure {
/** 日期 */
private int date;
/** 建造信息列表 */
private java.util.ArrayList<Common.GuildObj.Guild_ConstructInfo> constructList;
/** 捐赠进度 */
private int rewardPoint;


public Guild_ConstructList() {
	date = 0;
	constructList = new java.util.ArrayList<Common.GuildObj.Guild_ConstructInfo>();
	rewardPoint = 0;
}

public Guild_ConstructList(
	 int _date
	, java.util.ArrayList<Common.GuildObj.Guild_ConstructInfo> _constructList
	, int _rewardPoint
) {	date = _date;
	constructList = _constructList;
	rewardPoint = _rewardPoint;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 日期 */
public int getDate() { return date; }
/** 日期 */
public void setDate(int _date) { date = _date; }
/** 建造信息列表 */
public java.util.ArrayList<Common.GuildObj.Guild_ConstructInfo> getConstructList() { return constructList; }
/** 建造信息列表 */
public void addConstructList(Common.GuildObj.Guild_ConstructInfo _constructList) { constructList.add(_constructList); }
/** 捐赠进度 */
public int getRewardPoint() { return rewardPoint; }
/** 捐赠进度 */
public void setRewardPoint(int _rewardPoint) { rewardPoint = _rewardPoint; }


public final int GetBufSize() {
	int _size = 8;
	_size += 2 + (constructList.size() * 28);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += 2 + (constructList.size() * 28);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) date = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _constructListCount = _buf.getShort();
	for(int _i = 0; _i < _constructListCount; _i++) { 
		Common.GuildObj.Guild_ConstructInfo _constructList = new Common.GuildObj.Guild_ConstructInfo();
		if(_buf.remaining() <= 0) return;
	int __constructListCustLen = _buf.getInt();
	int __constructListCurPos = _buf.position();
	_constructList.ReadUnzipBuf(_buf, __constructListCurPos + __constructListCustLen);
	_buf.position(__constructListCurPos + __constructListCustLen);

		constructList.add(_constructList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) rewardPoint = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(date);
	_buf.putShort((short)constructList.size());
	for(int _i = 0; _i < constructList.size(); _i++) { 
		_buf.putInt(constructList.get(_i).GetBufSize());
	constructList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putInt(rewardPoint);
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

