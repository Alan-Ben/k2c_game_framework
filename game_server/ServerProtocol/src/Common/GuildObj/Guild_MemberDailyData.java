package Common.GuildObj;

import java.nio.ByteBuffer;
/*********
 * 联盟成员当日信息
 **/
public class Guild_MemberDailyData implements ALBasicProtocolPack._IALProtocolStructure {
/** 日期 用于每天重置数据 */
private int date;
/** 当天已领取建设奖励列表 */
private java.util.ArrayList<Integer> todayDrawConstructRewardList;


public Guild_MemberDailyData() {
	date = 0;
	todayDrawConstructRewardList = new java.util.ArrayList<Integer>();
}

public Guild_MemberDailyData(
	 int _date
	, java.util.ArrayList<Integer> _todayDrawConstructRewardList
) {	date = _date;
	todayDrawConstructRewardList = _todayDrawConstructRewardList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 日期 用于每天重置数据 */
public int getDate() { return date; }
/** 日期 用于每天重置数据 */
public void setDate(int _date) { date = _date; }
/** 当天已领取建设奖励列表 */
public java.util.ArrayList<Integer> getTodayDrawConstructRewardList() { return todayDrawConstructRewardList; }
/** 当天已领取建设奖励列表 */
public void addTodayDrawConstructRewardList(int _todayDrawConstructRewardList) { todayDrawConstructRewardList.add(_todayDrawConstructRewardList); }


public final int GetBufSize() {
	int _size = 4;
	_size += 2 + (todayDrawConstructRewardList.size() * 4);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 6;
	_size += 2 + (todayDrawConstructRewardList.size() * 4);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) date = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _todayDrawConstructRewardListCount = _buf.getShort();
	for(int _i = 0; _i < _todayDrawConstructRewardListCount; _i++) { 
		int _todayDrawConstructRewardList = 0;
		if(_buf.remaining() > 0) _todayDrawConstructRewardList = _buf.getInt();
		todayDrawConstructRewardList.add(_todayDrawConstructRewardList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(date);
	_buf.putShort((short)todayDrawConstructRewardList.size());
	for(int _i = 0; _i < todayDrawConstructRewardList.size(); _i++) { 
		_buf.putInt(todayDrawConstructRewardList.get(_i));
	}
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

