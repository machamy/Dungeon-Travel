using System.Data;

namespace Database
{
    public interface IDataParser<T>
    {
        /// <summary>
        /// 엑셀 시트를 읽어서 데이터를 파싱
        /// </summary>
        /// <param name="sheet">시트</param>
        /// <param name="header">속성명</param>
        /// <param name="colNum">총 열의 개수</param>
        /// <returns></returns>
        T Parse(DataTable sheet, string[] header, int colNum);
    }
}