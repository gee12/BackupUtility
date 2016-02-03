using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Threading;
using System.Windows.Threading;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Runtime.Serialization;
using System.Security.Cryptography;

namespace SBUConfigurator
{
    public partial class Utils
    {
        public const Visibility VIS_VISIBLE = Visibility.Visible;
        public const Visibility VIS_COLLAPSED = Visibility.Collapsed;
        public const Visibility VIS_HIDDEN = Visibility.Hidden;

        public static byte BoolToByte(bool b)
        {
            return (byte)((b) ? 1 : 0);
        }

        public static bool ByteToBool(byte b)
        {
            return (b != 0);
        }

        public static Image GetIcon(string resName)
        {
            return new Image
            {
                Source = new BitmapImage(new Uri("pack://application:,,,/Tickets;component/Resources/" + resName, UriKind.RelativeOrAbsolute))
            };
        }

        public static Visibility VisReverse(Visibility vis)
        {
            return (vis == Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;
        }

        public static Visibility ToVisibility(bool value)
        {
            return (value) ? Visibility.Visible : Visibility.Collapsed;
        }

        public static int? ToInt(string s)
        {
            int temp;
            int? res = null;
            if (Int32.TryParse(s, out temp))
                res = temp;
            return res;
        }

        public static float? ToFloat(string s, NumberStyles style = NumberStyles.Float, CultureInfo culture = null)
        {
            culture = (App.AppCulture ?? culture);
            float temp;
            float? res = null;
            if (float.TryParse(s, style, culture, out temp))
                res = temp;
            return res;
        }

        public static double? ToDouble(string s, NumberStyles style = NumberStyles.Float, CultureInfo culture = null)
        {
            culture = (App.AppCulture ?? culture);
            double temp;
            double? res = null;
            if (double.TryParse(s, style, culture, out temp))
                res = temp;
            return res;
        }

        public static bool IsMatch(string source, string pattern)
        {
            return Regex.IsMatch(source, pattern, RegexOptions.IgnoreCase);
        }

        public static IEnumerable<T> AddEmptyAtFirst<T>(IEnumerable<T> source)
            where T : new()
        {
            List<T> res = source.ToList();
            res.Insert(0, new T());
            return res;
        }

        public static int CompareDate(DateTime d1, DateTime d2)
        {
            return d1.Date.CompareTo(d2.Date);
        }

        public static bool OnUnsignedFloatTextInput(TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[0-9,]+");
            return !regex.IsMatch(e.Text);
        }

        public static void OnUnsignedFloatPreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = Utils.OnUnsignedFloatTextInput(e);
        }

        public static bool OnUnsignedIntTextInput(TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[0-9]+");
            return !regex.IsMatch(e.Text);
        }

        public static void OnUnsignedIntPreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = Utils.OnUnsignedIntTextInput(e);
        }

        public static short ZeroIfNegative(short number)
        {
            return (number < 0) ? (short)0 : number;
        }

        public static short ValueIfNegative(short number, short value)
        {
            return (number < 0) ? value : number;
        }

        public static short ValueIfLess(short number, short comp, short value)
        {
            return (number < comp) ? value : number;
        }

        public static int ValueIfLess(int number, int comp, int value)
        {
            return (number < comp) ? value : number;
        }

        public static string ValueIfEmpty(string s, string value)
        {
            return (string.IsNullOrEmpty(s)) ? value : s;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public static List<T> GetFirstRange<T>(List<T> entities, int count)
        {
            if (entities == null) return null;
            var size = entities.Count;
            return (count <= 0 || count > size) ? entities
                : entities.GetRange(0, count);
        }

        public static IEnumerable<T> GetRange<T>(IEnumerable<T> entities, int firstIndex, int count)
        {
            if (entities == null) return null;
            var size = entities.Count();
            count = (firstIndex + count > size) ? size - firstIndex : count;
            return (firstIndex < 0 || firstIndex >= size || count <= 0) ? entities
                : entities.ToList().GetRange(firstIndex, count);
        }

        public static List<T> GetRange<T>(ObservableCollection<T> collection, int firstIndex, int count)
        {
            if (collection == null) return null;
            var size = collection.Count;
            count = (firstIndex + count > size) ? size - firstIndex : count;
            return (firstIndex < 0 || firstIndex >= size || count <= 0) ? collection.ToList()
                : collection.ToList().GetRange(firstIndex, count);
        }

        public static DateTime GetMondayFromCurrentWeek(DateTime date)
        {
            if (date == null) return date;
            int curDayOfWeek = (int)date.DayOfWeek;
            int daysToAdd = (curDayOfWeek != 0) ? (-curDayOfWeek + 1) : -6;
            return date.AddDays(daysToAdd);
        }

        /// <summary>
        /// 
        /// </summary>
        public static object DeepClone(object obj)
        {
            object objResult = null;
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, obj);

                ms.Position = 0;
                objResult = bf.Deserialize(ms);
            }
            return objResult;
        }

        /// <summary>
        /// 
        /// </summary>
        public class VisibilityToNullableBooleanConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value is Visibility)
                {
                    return (((Visibility)value) == Visibility.Visible);
                }
                else
                {
                    return Binding.DoNothing;
                }
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value is bool?)
                {
                    return (((bool?)value) == true ? Visibility.Visible : Visibility.Collapsed);
                }
                else if (value is bool)
                {
                    return (((bool)value) == true ? Visibility.Visible : Visibility.Collapsed);
                }
                else
                {
                    return Binding.DoNothing;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static ICollection<T> SortCollection<T>(ICollection<T> collection, string property, ListSortDirection direction)
        {
            string[] props = property.Split('.');
            ICollection<T> res = new List<T>(collection);

            switch (direction)
            {
                case ListSortDirection.Ascending:
                    res = ((from n in collection
                                   orderby
                                   n.GetType().GetProperty(props[0]).GetValue(n, null)
                                   select n).ToList<T>()) as ICollection<T>;
                    break;
                case ListSortDirection.Descending:
                    res = ((from n in collection
                                   orderby
                                   n.GetType().GetProperty(props[0]).GetValue(n, null)
                                   descending
                                   select n).ToList<T>()) as ICollection<T>;
                    break;
            }
            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool CopyFile(string updatePath, string destFileName)
        {
            try
            {
                File.Copy(updatePath, destFileName, true);
            }
            catch (Exception ex)
            {
                //Log.Add(ex);
                return false;
            }
            return File.Exists(destFileName);
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool DeleteFile(string path)
        {
            try
            {
                File.Delete(path);
            }
            catch (Exception ex)
            {
                //Log.Add(ex);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool CreateDir(string path)
        {
            try
            {
                Directory.CreateDirectory(path);
            }
            catch (Exception ex)
            {
                //Log.Add(ex);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        public static IEnumerable<C> getChildItems<C, P>(P parent)
            where P : ItemsControl
            where C : class
        {
            int cnt = parent.Items.Count;
            for (int idx = 0; idx < cnt; ++idx)
            {
                yield return parent.ItemContainerGenerator.ContainerFromIndex(idx) as C;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string ReadTextFile(string fileName)
        {
            if (!File.Exists(fileName)) return null;

            return File.ReadAllText(fileName);
        }

        /// <summary>
        /// 
        /// </summary>
        public static string GetMd5Hash(string input)
        {
            byte[] data = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sb.Append(data[i].ToString("x2"));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool VerifyMd5Hash(string input, string hash)
        {
            string hashOfInput = GetMd5Hash(input);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            return (comparer.Compare(hashOfInput, hash) == 0);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public class BindingRu : Binding
    {
        public BindingRu()
        {
            InitCulture();
        }

        public BindingRu(string path)
            : base(path)
        {
            InitCulture();
        }

        private void InitCulture()
        {
            ConverterCulture = App.AppCulture;
        }
    }

}
