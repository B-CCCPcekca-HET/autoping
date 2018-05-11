using System;
using System.Collections.Generic;

using System.Net.NetworkInformation;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Forms;
using Hardcodet.Wpf.TaskbarNotification;
using System.IO;
using System.Management;
using Microsoft.Win32;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace AutoPing
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {



        List<string> serversList2 = new List<string>();// servers list
        List<string> nopinghosts = new List<string>();// servers list

        bool exitmethod;
        string line;

        List<System.Windows.Controls.Label> labels = new List<System.Windows.Controls.Label>();
       

        /// <summary>
        /// main
        /// </summary>
        public MainWindow()
        {

            InitializeComponent();


            mytray.Icon = Resource1.ballgreen;
            //open file with hosts
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Multiselect = true;
            dlg.Title = "Открыть файл со списком  хостов.";
            // dlg.InitialDirectory = ".\\"; 
            dlg.DefaultExt = ".txt";
            dlg.Filter = "TXT (.txt)|*.txt|Excel_2003-2007(*.xls)|*.xls|csv files (*.csv)|*.csv|All files (*.*)|*.*";
            string filename = dlg.FileName;
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {

                var filepath = dlg.FileName;
            }

            //read  file
            // string line;

            try
            {
                System.IO.StreamReader file = new System.IO.StreamReader(dlg.FileName);
                while ((line = file.ReadLine()) != null)
                {
                    serversList2.Add(line);
                    // Console.WriteLine(line); // all hosts
                    // counter++;

                }
            }
            catch (Exception) { Console.WriteLine("empty file  name"); Environment.Exit(0); }



            foreach (string hostnew in serversList2)
            {

                //автогенерация  label
                System.Windows.Controls.Label lbl = new System.Windows.Controls.Label

                {
                   // Name = hostnew, // нельзя  имя с  тире -
                    Background = Brushes.DimGray,
                    Content = hostnew,
                    Height = window1.Height / 100 * 10,//
                    Width = window1.Width / 100 * 15,
                    Margin = new Thickness(2, 2, 2, 2), // отступ
                    Padding = new Thickness(1),
                    HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Center
                };
                wrap1.Children.Add(lbl);
            }

            //run 
           //foreach(string  h in serversList2)
                PingerTest2();//ping list
            
          
            


        }
        
        public void WMIQuery(string machine)
        {

            try
            {
                //string machine;
                ManagementScope scope = new ManagementScope("\\\\" + machine + "\\root\\cimv2");
                scope.Connect();


                ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
                SelectQuery query1 = new SelectQuery("SELECT * FROM Win32_LogicalDisk");
                // SelectQuery query2 = new SelectQuery("SELECT FreeSpace from Win32_LogicalDisk where name ='C:'");//free space on C
                SelectQuery query2 = new SelectQuery("SELECT FreeSpace from Win32_LogicalDisk");//free space on C
                // SelectQuery query3= new SelectQuery("SELECT * From Win32_Service");//просмотр  служб
                SelectQuery query3 = new SelectQuery("SELECT * From Win32_Service where name = 'AVP'");//просмотр  служб

                ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);
                ManagementObjectCollection queryCollection = searcher.Get();

                ManagementObjectSearcher searcher1 = new ManagementObjectSearcher(scope, query1);
                ManagementObjectCollection queryCollection1 = searcher1.Get();

                ManagementObjectSearcher searcher2 = new ManagementObjectSearcher(scope, query2);
                ManagementObjectCollection queryCollection2 = searcher2.Get();

                ManagementObjectSearcher searcher3 = new ManagementObjectSearcher(scope, query3);
                ManagementObjectCollection queryCollection3 = searcher3.Get();

                //host name
                foreach (ManagementObject m in queryCollection)
                {
                    //display remote comp inf

                    Console.WriteLine("host: " + m["csname"]);

                }
                //size  disk
                foreach (ManagementObject m in queryCollection1)
                {
                    //display remote comp inf
                    var hard = m["Name"];
                    var test = m.GetPropertyValue("DeviceID").ToString();

                    var FreeSpaceOnDisk = m.GetPropertyValue("FreeSpace");
                    var q = Convert.ToDouble(FreeSpaceOnDisk) / 1024 / 1024 / 1024;

                    Console.WriteLine(" Disk: " + m["Name"] + " " + (Convert.ToInt64(m["Size"]) / 1024 / 1024 / 1024) + " free: " + Math.Round(q, 1));
                    //test message
                    System.Windows.Forms.MessageBox.Show(" Disk: " + m["Name"] + " " + (Convert.ToInt64(m["Size"]) / 1024 / 1024 / 1024) + " free: " + Math.Round(q, 1));
                }


                //services name 
                foreach (ManagementObject m in queryCollection3)
                {

                    // Console.WriteLine("  Service: "  + m["name"] + " => " + m["State"]);

                    if (m["State"].ToString() == "Stopped")
                    {
                        Console.WriteLine("STOP " + m["Name"]);
                    }
                    else
                    {
                        Console.WriteLine("OK " + m["Name"]);
                    }

                }
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show("Отказано в  доступе!!! \n" + e);
                System.Environment.Exit(0);


            }

        }

        //scan disk space and  kasper
        public void WMIQuery2()
        {
            int i = -1;
            foreach (FrameworkElement label in wrap1.Children)
            {
                ++i;
                string machine = serversList2[i];

                if (label is System.Windows.Controls.Label lbl)
                {

                    try
                    {

                        //string machine;
                        ManagementScope scope = new ManagementScope("\\\\" + machine + "\\root\\cimv2");
                        scope.Connect();

                       
                        SelectQuery query1 = new SelectQuery("SELECT * FROM Win32_LogicalDisk"); // 
                        ManagementObjectSearcher searcher1 = new ManagementObjectSearcher(scope, query1);
                        ManagementObjectCollection queryCollection1 = searcher1.Get();

                        SelectQuery query2 = new SelectQuery("SELECT FreeSpace from Win32_LogicalDisk");//free space on C
                        ManagementObjectSearcher searcher2 = new ManagementObjectSearcher(scope, query2);
                        ManagementObjectCollection queryCollection2 = searcher2.Get();

                        SelectQuery query3 = new SelectQuery("SELECT * From Win32_Service where name ='KAVFS' OR name = 'AVP'");//просмотр  служб 'AVP' or  'kavfs'
                        ManagementObjectSearcher searcher3 = new ManagementObjectSearcher(scope, query3);
                        ManagementObjectCollection queryCollection3 = searcher3.Get();

                        //вызовы методов

                        lbl.Content = machine;
                        lbl.Height = window1.Height / 100 * 10; //вернуть надпись в  исходное состояние
                        //size  disk
                        foreach (ManagementObject disk in queryCollection1)
                        {
                            //display remote comp inf
                            var hard = disk["Name"];
                            var test = disk.GetPropertyValue("DeviceID").ToString();

                            var FreeSpaceOnDisk = disk.GetPropertyValue("FreeSpace");
                            var q = Convert.ToDouble(FreeSpaceOnDisk) / 1024 / 1024 / 1024;

                            Console.WriteLine("host: "+ machine+" Disk: " + disk["Name"] + " " + (Convert.ToInt64(disk["Size"]) / 1024 / 1024 / 1024) + " free: " + Math.Round(q, 1));
                           
                            //create label
                            lbl.Content = lbl.Content +"\n  "+disk["Name"] + " " + (Convert.ToInt64(disk["Size"]) / 1024 / 1024 / 1024) + "Gb free: " + Math.Round(q, 1)+" Gb";
                            lbl.Height =lbl.Height +20;
                        }
                        

                        //services name 
                        foreach (ManagementObject service in queryCollection3)
                        {

                            // Console.WriteLine("  Service: "  + m["name"] + " => " + m["State"]);

                            if (service["State"].ToString() == "Stopped")
                            {
                                Console.WriteLine("STOP " + service["Name"]);
                                lbl.Content = lbl.Content + "\n "+" "+ service["Name"] +" STOP";
                            }
                            else
                            {
                                Console.WriteLine("OK " + service["Name"]);
                                lbl.Content = lbl.Content + "\n " +" "+ service["Name"] + " OK";
                            }

                        }







                    }
                    catch (Exception )
                    {
                       // System.Windows.MessageBox.Show("Отказано в  доступе!!! \n" + e);
                        lbl.Content = lbl.Content + "\n" + "Нет доступа";




                    }
                    
                }
            }
        }

        public  void  WMIQuery3() //async
        {
            int i = -1;
            

            foreach (FrameworkElement label in wrap1.Children)
            {   
                ++i;
                string machine = serversList2[i];

                if (label is System.Windows.Controls.Label lbl)
                {

                    try
                    {

                        //string machine;
                        ManagementScope scope = new ManagementScope("\\\\" + machine + "\\root\\cimv2");
                        scope.Connect();
                   




                    SelectQuery query1 = new SelectQuery("SELECT * FROM Win32_LogicalDisk"); // 
                    ManagementObjectSearcher searcher1 = new ManagementObjectSearcher(scope, query1);
                    ManagementObjectCollection queryCollection1 = searcher1.Get();

                    SelectQuery query2 = new SelectQuery("SELECT FreeSpace from Win32_LogicalDisk");//free space on C
                    ManagementObjectSearcher searcher2 = new ManagementObjectSearcher(scope, query2);
                    ManagementObjectCollection queryCollection2 = searcher2.Get();

                    SelectQuery query3 = new SelectQuery("SELECT * From Win32_Service where name ='KAVFS' OR name = 'AVP'");//просмотр  служб 'AVP' or  'kavfs'
                    ManagementObjectSearcher searcher3 = new ManagementObjectSearcher(scope, query3);
                    ManagementObjectCollection queryCollection3 = searcher3.Get();
                   
                    //вызовы методов

                    lbl.Content = machine;
                        lbl.Height = window1.Height / 100 * 10; //вернуть надпись в  исходное состояние
                        //size  disk
                        foreach (ManagementObject disk in queryCollection1)
                        {
                            //display remote comp inf
                            var hard = disk["Name"];
                            var test = disk.GetPropertyValue("DeviceID").ToString();

                            var FreeSpaceOnDisk = disk.GetPropertyValue("FreeSpace");
                            var q = Convert.ToDouble(FreeSpaceOnDisk) / 1024 / 1024 / 1024;

                            Console.WriteLine("host: " + machine + " Disk: " + disk["Name"] + " " + (Convert.ToInt64(disk["Size"]) / 1024 / 1024 / 1024) + " free: " + Math.Round(q, 1));

                            //create label
                            lbl.Content = lbl.Content + "\n  " + disk["Name"] + " " + (Convert.ToInt64(disk["Size"]) / 1024 / 1024 / 1024) + "Gb free: " + Math.Round(q, 1) + " Gb";
                            lbl.Height = lbl.Height + 20;
                        }


                        //services name 
                        foreach (ManagementObject service in queryCollection3)
                        {

                            // Console.WriteLine("  Service: "  + m["name"] + " => " + m["State"]);

                            if (service["State"].ToString() == "Stopped")
                            {
                                Console.WriteLine("STOP " + service["Name"]);
                                lbl.Content = lbl.Content + "\n " + " " + service["Name"] + " STOP";
                            }
                            else
                            {
                                Console.WriteLine("OK " + service["Name"]);
                                lbl.Content = lbl.Content + "\n " + " " + service["Name"] + " OK";
                            }

                        }

                    }
                    catch (UnauthorizedAccessException)
                    {
                        // System.Windows.MessageBox.Show("Отказано в  доступе!!! \n" + e);
                        lbl.Content = lbl.Content + "\n" + "Нет доступа";


                    }








                }
            }
        }



        public async void PingerTest()
        {

            Ping ping = new Ping();
            PingOptions pingOptions = new PingOptions
            {
                DontFragment = true // фрагментация  данных
            };
            Host host = new Host();
            string dataping = "teststring";
            byte[] buffer = Encoding.ASCII.GetBytes(dataping);
            host.errorhost = 0;///////

            exitmethod = false; // выход из бесконечного цикла



             while (!exitmethod)  {

           // foreach (string hh in serversList2)
                foreach (FrameworkElement label in wrap1.Children)
                {

                if (label is System.Windows.Controls.Label lbl)
                {


                    
                    string hostnew = host.namehost = lbl.Content.ToString();
                   

                            try
                            {

                                PingReply pingReply = await ping.SendPingAsync(hostnew, 3000, buffer, pingOptions);//2000 = 2 sec
                            

                                if (pingReply.Status != IPStatus.Success)
                                {

                                    Console.WriteLine(hostnew + " host error  ");// добавить счетчик  ошибок при плохом  пинге

                                    lbl.Background = Brushes.Red;//цвет  ячейки при ошибке
                                    host.errorhost = 1;
                                    nopinghosts.Add(hostnew); // add list

                                }
                                else
                                {
                                    //Console.WriteLine(hostnew + "OK");
                                    lbl.Background = Brushes.ForestGreen;//цвет ячейки при корректном пинге
                                    var qqq = nopinghosts.Find(x => x.Contains(hostnew));
                                    nopinghosts.Remove(qqq); // remove  list

                                }

                            }
                            catch (PingException)
                            {
                                Console.WriteLine(hostnew + " unknow host ");
                                lbl.Background = Brushes.LightGray;

                            }


                        
                    }
                    else
                        continue;



                }


                //color  tray  проверка менять цвет согластно статусу
                if (host.errorhost == 0)
                {
                    mytray.Icon = Resource1.ballgreen;
                }
                else
                {
                    mytray.Icon = Resource1.ballred;
                }
                
                host.errorhost = 0;
                int visota = nopinghosts.Count;

                // Console.WriteLine(string.Join("\n", nopinghosts));
                // bordertooltip.Width = 150;//ширина
                // bordertooltip.Height = 50*visota; // высота
                tbtray.FontSize = 16;//размер текста
                tbtray.Width = 150;
                tbtray.Height = 40 * visota;


                tbtray.Text = string.Join("\n", nopinghosts);//текст в всплывающем  окне в  трее
                nopinghosts.Clear();//clear  list of  errors

              }
        }


        public async void PingerTest2()
        {

            Ping ping = new Ping();
            PingOptions pingOptions = new PingOptions
            {
                DontFragment = true // фрагментация  данных
            };
            Host host = new Host();
            string dataping = "teststringstring";
            byte[] buffer = Encoding.ASCII.GetBytes(dataping);
            host.errorhost = 0;///////

            exitmethod = false; // выход из бесконечного цикла

            while (!exitmethod)
            {
                int i = -1;
                foreach (FrameworkElement label in wrap1.Children)
                {
                    ++i;
                    string test = serversList2[i];

                    if (label is System.Windows.Controls.Label lbl)
                    {




                        //ping hosts
                        try
                        {

                            PingReply pingReply = await ping.SendPingAsync(test, 1000, buffer, pingOptions);//2000 = 2 sec


                            if (pingReply.Status != IPStatus.Success)
                            {

                                Console.WriteLine(test + " host error  ");// добавить счетчик  ошибок при плохом  пинге

                                lbl.Background = Brushes.Red;//цвет  ячейки при ошибке
                                host.errorhost = 1;
                                nopinghosts.Add(test); // add list

                            }
                            else
                            {
                                Console.WriteLine(test + "OK");
                                lbl.Background = Brushes.ForestGreen;//цвет ячейки при корректном пинге

                                var qqq = nopinghosts.Find(x => x.Contains(test));
                                nopinghosts.Remove(qqq); // remove  list

                            }

                        }
                        catch (PingException)
                        {
                            Console.WriteLine(test + " unknow host ");
                            lbl.Background = Brushes.LightGray;

                        }


                    }
                    else
                        continue;
                }
                //create  tray
                //color  tray  проверка менять цвет согластно статусу
                if (host.errorhost == 0)
                {
                    mytray.Icon = Resource1.ballgreen;
                }
                else
                {
                    mytray.Icon = Resource1.ballred;
                }

                host.errorhost = 0;
                int visota = nopinghosts.Count;

                // Console.WriteLine(string.Join("\n", nopinghosts));
                // bordertooltip.Width = 150;//ширина
                // bordertooltip.Height = 50*visota; // высота
                tbtray.FontSize = 16;//размер текста
                tbtray.Width = 150;
                tbtray.Height = 40 * visota;
                tbtray.TextAlignment = TextAlignment.Center;
                
                tbtray.Text = string.Join("\n",nopinghosts);//текст в всплывающем  окне в  трее
                nopinghosts.Clear();//clear  list of  errors

            }
        }





        


      
        protected override void OnStateChanged( EventArgs e)
        {
           // Console.WriteLine("test starte changed");
           
            if (WindowState == WindowState.Minimized)
                this.Hide();
            base.OnStateChanged(e);


        }




        private void ShowHideMainWindow_Click(object sender, EventArgs e)
        {

            if (WindowState == WindowState.Minimized)


             base.OnStateChanged(e);
            this.WindowState = WindowState.Maximized;
            this.Show();
            this.WindowState = WindowState.Normal;


        }
        private void MenuExitTray(object sender, EventArgs e)
        {
            window1.Close();
            try
            {
                exitmethod = true;
                mytray.Dispose();
                //this.Visibility = Visibility.Hidden; //  должно корректно закрывать окно без зависаний
                //
                base.OnClosed(e);
                App.Current.Shutdown();
                System.Environment.Exit(0);
                System.Windows.Forms.Application.ExitThread();
                System.Windows.Forms.Application.Exit();
                // return true;
            }
            catch (Exception )

            {
                exitmethod = true;
                mytray.Dispose();
               
                base.OnClosed(e);
                App.Current.Shutdown();
                System.Environment.Exit(0);
                System.Windows.Forms.Application.ExitThread();
                System.Windows.Forms.Application.Exit();

            }
            
        }


        
        

        
        
        /// tray notify
        
        private void createTrayIcon(  )
        {
           // string textballoon = "test";
           // tbtray.Text = textballoon; // всплывающий  текст
        }
        
        private void window1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mytray.Dispose();  
        }


        //test  wmic
        private  void Button_Click(object sender, RoutedEventArgs e)//проверка на  диски и службу каспера
        {
         WMIQuery3();            
            // WMIQuery2();
            
        }

 
    }
    }

