using System.Reflection;
using System.Text;

namespace ConsoleApp
{
    internal static class Serializer
    {
        public static string SerializeToCsv<T>(T item) where T : class
        {
            var fields = typeof(T).GetFields();
            var properties = typeof(T).GetProperties();


            //Headers
            StringBuilder sbHeader = new();
            bool fieldsExist = false;

            var headerField = string.Join(",", fields.Select(p => p.Name));
            if (!string.IsNullOrEmpty(headerField))
            {              
                sbHeader.Append(headerField);
                fieldsExist = true;
            }

            var headerProps = string.Join(",", properties.Select(p => p.Name));
            if (!string.IsNullOrEmpty(headerProps))
            {
                if(fieldsExist)
                {
                    sbHeader.Append(",");
                }
                sbHeader.Append(headerProps);
            }

            //Values
            StringBuilder sbValue = new();
            fieldsExist = false;

            var valueField = string.Join(",", fields.Select(p => p.GetValue(item)?.ToString() ?? ""));
            if (!string.IsNullOrEmpty(valueField))
            {
                sbValue.Append(valueField);
                fieldsExist = true;
            }

            var valueProps = string.Join(",", properties.Select(p => p.GetValue(item)?.ToString() ?? ""));
            if (!string.IsNullOrEmpty(valueProps))
            {
                if (fieldsExist)
                {
                    sbValue.Append(",");
                }
                sbValue.Append(valueProps);
            }

            return $"{sbHeader}\n{sbValue}";
        }

        public static T DeserializeCSV<T>(List<string> lines) where T : new()
        {
            var memberInfos = typeof(T).GetMembers(BindingFlags.Public | BindingFlags.Instance)
                .Where(m => m is FieldInfo || m is PropertyInfo)
                .OrderBy(x => x.MemberType)
                .ToList();

            foreach (var line in lines)
            {
                var cells = line.Split(',');
                if (cells.Length != memberInfos.Count) continue;

                var item = new T();
                for (int i = 0; i < cells.Length; i++)
                {
                    var memberInfo = memberInfos[i];
                    var memberType = memberInfo switch
                    {
                        FieldInfo fi => fi.FieldType,
                        PropertyInfo pi => pi.PropertyType,
                        _ => throw new InvalidOperationException("Unsupported member type")
                    };
                    var memberValue = Convert.ChangeType(cells[i], memberType);
                    switch (memberInfo)
                    {
                        case FieldInfo fi:
                            fi.SetValue(item, memberValue);
                            break;
                        case PropertyInfo pi:
                            pi.SetValue(item, memberValue);
                            break;

                    }
                }

                return item;
            }

            return default(T);
        }


        public static List<string> ReadFromTheFileByLines(string path)
        {
            List<string> result = new();
            using (var reader = new StreamReader(path))
            {
                string? line;
                bool firstLine = true;
                while ((line = reader.ReadLine()) != null)
                {
                    if (firstLine) // Skip header row
                    {
                        firstLine = false;
                        continue;
                    }

                    result.Add(line);
                }
            }
            return result;

        }
    }
}
