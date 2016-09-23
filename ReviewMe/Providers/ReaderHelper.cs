using System;
using System.Data;

namespace Scheduler.Providers
{
    public static class ReaderHelper
    {
        public
            static T GetColumnValue<T>(this IDataReader record, string columnName)
        {
            var value = record[columnName];
            if (value == null || value == DBNull.Value)
            {
                return default(T);
            }

            return (T)value;
        }
    }
}