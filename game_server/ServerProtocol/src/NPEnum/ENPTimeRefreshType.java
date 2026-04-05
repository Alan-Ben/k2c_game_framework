package NPEnum;

/*********
 * 时间刷新类型
 **/
public enum ENPTimeRefreshType {
	NONE, //0 ==== 
	REF_BY_LIVE, //1 ==== 按存在时间到期刷新
	REF_IF_DIS, //2 ==== 消失立马刷新
	REF_DURATION, //3 ==== 按照间隔时间刷新 REF_DURATION:MIN（S）:MAX（S）
	REF_CLOCK, //4 ==== 按照定时时间刷新 REF_CLOCK:24进制小时数
	REF_WEEK_CLOCK, //5 ==== 按照周定时时间刷新 REF_WEEK_CLOCK:周几:24进制小时数
	REF_NOT_REFRESH, //6 ==== 不刷新
	REF_MONTH_CLOCK, //7 ==== 按照月定时时间刷新 REF_MONTH_CLOCK:几号:24进制小时数
	REF_MIN_CLOCK, //8 ==== 按照定时时间刷新 REF_MIN_CLOCK:24进制小时数:60进制分钟数
	;
public static final ENPTimeRefreshType[]  ENPTimeRefreshType_Values = ENPTimeRefreshType.values();
public static final int ENPTimeRefreshType_Length = ENPTimeRefreshType_Values.length;
public static ENPTimeRefreshType ENPTimeRefreshType_FromInt(int _ivalue) {
	if(_ivalue < 0 || _ivalue >= ENPTimeRefreshType_Length){ return null; }
	return ENPTimeRefreshType_Values[_ivalue];
}
}

