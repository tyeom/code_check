namespace AppPipe
{
    using System;
    using System.Data;

    public class DateTimeModeConverter
    {
        public static DataSet ConvertTo(DataSet dataSet, DataSetDateTime mode = DataSetDateTime.Utc)
        {
            var newDataSet = new DataSet();
            foreach (DataTable dataTable in dataSet.Tables)
            {
                newDataSet.Tables.Add(GetConvertedDataTable(dataTable, mode));
            }

            return newDataSet;
        }

        /// <summary>
        /// DataTable을 검사해 DateTimeMode를 바꾸고
        /// 전체 데이터를 재할당한 복사본 DataTable을 반환한다.
        /// 실제 데이터 변경 여부와 관계 없이 DataTable 전체를 새로 생성하므로 성능에 영향이 있을 수 있음.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static DataTable ConvertTo(DataTable dataTable, DataSetDateTime mode = DataSetDateTime.Utc)
        {
            var newTable = GetConvertedDataTable(dataTable, mode);

            if (dataTable.DataSet != null)
            {
                var tempParentDataSet = new DataSet();
                tempParentDataSet.DataSetName = dataTable.DataSet.DataSetName;

                tempParentDataSet.Tables.Add(newTable);
            }

            return newTable;
        }

        /// <summary>
        /// DateTimeMode를 바꾼 DataTable만 복제한다. ParentDataSet은 할당하지 않는다.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        private static DataTable GetConvertedDataTable(DataTable dataTable, DataSetDateTime mode = DataSetDateTime.Utc)
        {
            var newTable = new DataTable(dataTable.TableName);
            foreach (DataColumn column in dataTable.Columns)
            {
                var newColumn = new DataColumn(column.ColumnName, column.DataType);
                if (column.DataType == typeof(DateTime))
                {
                    newColumn.DateTimeMode = mode;
                }

                newTable.Columns.Add(newColumn);
            }

            foreach (DataRow row in dataTable.Rows)
            {
                var newRow = newTable.NewRow();
                for (var i = 0; i < row.ItemArray.Length; i++)
                {
                    newRow[i] = row[i];
                }

                newTable.Rows.Add(newRow);
            }

            return newTable;
        }

        /// <summary>
        /// 해당 DataTable의 컬럼 중 타입이 DateTime인 컬럼의 Mode를 Utc로 바꾼다.
        /// DataTable은 Row 할당이 없는 컬럼만 Mode 변경이 가능하다. (한 번이라도 값을 할당 하면 예외 발생)
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static void ConvertColumns(DataTable dataTable)
        {
            foreach (DataColumn column in dataTable.Columns)
            {
                if (column.DataType == typeof(DateTime))
                {
                    column.DateTimeMode = DataSetDateTime.Utc;
                }
            }
        }
    }
}
