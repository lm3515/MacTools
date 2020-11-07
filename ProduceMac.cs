/*************************************************************************************
 * CLR版本：       4.0.30319.42000
 * 类 名 称：      Class1
 * 机器名称：      9GX1UOWROPIAEJ4
 * 命名空间：      MacTool
 * 文 件 名：      Class1
 * 创建时间：      2020/10/27 17:33:23
 * 作    者：      Richard Liu
 * 说   明：。。。。。
 * 修改时间：      2020/10/27 17:33:23
 * 修 改 人：      Richard Liu
*************************************************************************************/


using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows;

namespace MacTool
{
    class ProduceMac
    {
        public static void Produce(int num)
        {
            // 获取时间
            DateTime dt = DateTime.Now;
            string year = dt.ToString("yy", DateTimeFormatInfo.InvariantInfo);
            string month = dt.ToString("MM", DateTimeFormatInfo.InvariantInfo);
            string day = dt.ToString("dd", DateTimeFormatInfo.InvariantInfo);
            string hour = dt.ToString("hh", DateTimeFormatInfo.InvariantInfo);
            string min = dt.ToString("mm", DateTimeFormatInfo.InvariantInfo);
            string second = dt.ToString("ss", DateTimeFormatInfo.InvariantInfo);

            string all = TenChangeHex(year, month, day, hour, min, second);
            ProductRandom(all, num);
        }

        //十进制转十六进制     
        private static string TenChangeHex(string y, string m, string d, string h, string mm, string s)
        {
            int year = Convert.ToInt32(y);                   
            int month = Convert.ToInt32(m);
            int day = Convert.ToInt32(d);
            int hour = Convert.ToInt32(h);
            int min = Convert.ToInt32(mm);
            int second = Convert.ToInt32(s);

            //转换为二进制要求7位
            string yearbyte = Convert.ToString(year, 2).PadLeft(7, '0');
            //转换为二进制要求4位
            string monthbyte = Convert.ToString(month, 2).PadLeft(4, '0');
            string daybyte = Convert.ToString(day, 2).PadLeft(5, '0');
            string hourbyte = Convert.ToString(hour, 2).PadLeft(5, '0');
            string minbyte = Convert.ToString(min, 2).PadLeft(5, '0');
            string secondbyte = Convert.ToString(second, 2).PadLeft(5, '0');

            string concat = yearbyte + monthbyte + daybyte + hourbyte + minbyte + secondbyte;

            // 转换为十六进制
            string hex = string.Format("{0:x}", Convert.ToInt64(concat, 2));
            string endStr = hex.Substring(2, 6);
            // 印记厂商标识(注意第二位一定要为偶数)
            hex = "68" + endStr;

            //每两个字母中间加":"并且转化为大写字母
            hex = hex.ToUpper().Insert(2, ":");
            hex = hex.ToUpper().Insert(5, ":");
            hex = hex.ToUpper().Insert(8, ":");
            
            return hex;
        }

        // 
        private static void ProductRandom(string hex, int num)
        {
            int k = 0;
            string st = "";
            List<string> list = new List<string>();
            
            for (int i = 1; i < 256; i++)
            {
                for (int j = 1; j < 256; j++)
                {
                    st = i.ToString("x8").Substring(6, 2).ToUpper() + ":" + j.ToString("x8").Substring(6, 2).ToUpper();
                    st = hex + ":" + st;
                    if (k == num)
                    {
                        // 停止循环
                        MessageBox.Show("成功");
                        //写入数据
                        WriterData(list);


                        // 页面销毁
                        return;
                    }

                    list.Add(st);
                    Console.WriteLine("第" + Convert.ToString(k + 1) + "个MAC地址：" + st);

                    k++;

                }
            }
        }

        private static void WriterData(List<string> data)
        {
            bool exists = false;
            string path = @"d:\mac.txt";
            exists = CreateFile(path);

            if (exists)
            {
                StreamWriter sw = new StreamWriter(path, true);
                foreach (string s in data)
                {
                    sw.WriteLine(s);
                }
                sw.Close();
            }
        }

        // 创建文件
        private static bool CreateFile(string path)
        {
            if (File.Exists(path))
            {
                return true;
            }
            else
            {
                try
                {
                    FileStream fs = new FileStream(path, FileMode.CreateNew);
                    fs.Close();
                    return true;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    return false;
                }
            }
        }
    }
}
