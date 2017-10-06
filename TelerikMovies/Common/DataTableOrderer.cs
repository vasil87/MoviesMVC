using System;
using System.Collections.Generic;
using System.Reflection;

namespace Common
{
    public static class DataTableOrderer
    {

        public static List<T> FilterData<T>(ref int recordFiltered, List<T> allData, int start, int length, string search, int sortColumn, string sortDirection)
        {
            List<PropertyInfo> propertiesOfT = new List<PropertyInfo>();
            List<T> list = new List<T>();
            if (search == null || string.IsNullOrWhiteSpace(search))
            {
                list = allData;
                foreach (var prop in typeof(T).GetProperties())
                {
                    propertiesOfT.Add(prop);
                }
            }
            else
            {
                // simulate search
                foreach (T dataItem in allData)
                {
                    foreach (var prop in typeof(T).GetProperties())
                    {
                        propertiesOfT.Add(prop);
                        if (prop.PropertyType == typeof(System.String))
                        {
                            if ((prop.GetValue(dataItem) as string).ToLower().Contains(search.ToLower()))
                            {
                                list.Add(dataItem);
                            }
                        }
                    }

                }
            }

            var propertyToUse = propertiesOfT[sortColumn];
            var propertyType = propertyToUse.PropertyType;
           
            // simulate sort
            if (propertyType == typeof(String))
            {
                list.Sort((x, y) => SortString((propertyToUse.GetValue(x) as string),(propertyToUse.GetValue(y) as string), sortDirection));
            }
            else if (propertyType == typeof(Int32))
            {// sort Age
                list.Sort((x, y) => SortInteger(Convert.ToInt32(propertyToUse.GetValue(x)), Convert.ToInt32(propertyToUse.GetValue(y)), sortDirection));
            }
            else if (propertyType == typeof(Boolean))
            {   // sort DoB
                list.Sort((x, y) => SortBool(Convert.ToBoolean(propertyToUse.GetValue(x)), Convert.ToBoolean(propertyToUse.GetValue(y)), sortDirection));
            }
            else if (propertyType == typeof(DateTime))
            {   // sort DoB
                list.Sort((x, y) => SortDateTime(Convert.ToDateTime(propertyToUse.GetValue(x)), Convert.ToDateTime(propertyToUse.GetValue(y)), sortDirection));
            }

            recordFiltered = list.Count;

            // get just one page of data
            list = list.GetRange(start, Math.Min(length, list.Count - start));

            return list;
        }

        private static int SortString(string s1, string s2, string sortDirection)
        {
            return sortDirection == "asc" ? s1.CompareTo(s2) : s2.CompareTo(s1);
        }

        private static int SortInteger(int i1, int i2, string sortDirection)
        {
            return sortDirection == "asc" ? i1.CompareTo(i2) : i2.CompareTo(i1);
        }
        private static int SortBool(bool b1, bool b2, string sortDirection)
        {
            return sortDirection == "asc" ? b1.CompareTo(b2) : b2.CompareTo(b1);
        }

        private static int SortDateTime(DateTime d1, DateTime d2, string sortDirection)
        {
           
            return sortDirection == "asc" ? d1.CompareTo(d2) : d2.CompareTo(d1);
        }
    }
}
