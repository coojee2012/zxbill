using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Bill
{
    public class ListsConvertToJson
    {
        public string ConvertJson(List<object> lists)
        {
            return ConvertJson(lists, string.Empty);
        }
        public string ConvertJson(List<object> lists, string entityName)
        {
            try
            {
                string result = string.Empty;
                //当传进来的entityName=Emtpty的时候，随机获取lists[index]项的类型
                if (entityName.Equals(string.Empty))
                {
                    object obj = lists[0];
                    entityName = obj.GetType().ToString();
                }
                //result = "{\"" + entityName + "\":[";
                result = "[";
                bool flag = true;
                foreach (var item in lists)
                {
                    if (!flag)
                    {
                        result = result + "," + ListIndexConvertToJson(item);
                    }
                    else
                    {
                        result = result + ListIndexConvertToJson(item) + "";
                        flag = false;
                    }
                }
                result = result + "]";
                return result;

            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }

        private string ListIndexConvertToJson(object item)
        {
            try
            {
                string result = "{";
                List<string> str_lists = new List<string>();
                str_lists = GetAllListValue(item);
                foreach (var str in str_lists)
                {
                    if (result.Equals("{"))
                    {
                        result = result + str;
                    }
                    else
                    {
                        result = result + "," + str;
                    }
                }
                return result + "}";
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }
        //反射object的所有属性值
        private List<string> GetAllListValue(object item)
        {
            try
            {
                List<string> lists = new List<string>();
                PropertyInfo[] parms = item.GetType().GetProperties();
                foreach (var p in parms)
                {
                    lists.Add("\"" + p.Name.ToString() + "\":\"" + p.GetValue(item, null) + "\"");
                }
                return lists;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }

    }
}
