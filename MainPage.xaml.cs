using System;
using System.Diagnostics;
using System.Threading;
using Windows.Devices.Enumeration;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Media;
using Windows.Graphics.Imaging;
using Windows.UI;
using Windows.Storage.Streams;
using Windows.Devices.SerialCommunication;
using System.Threading.Tasks;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Sumi2c
{

    public sealed partial class MainPage : Page
    {
        private SerialDevice SerialPort;
        private DataWriter dataWriter;
        private DataReader dataReader;
        string rxBuffer;
        String serialData;
        String serialDataСheck;
        CancellationTokenSource ReadCancellationTokenSource = new CancellationTokenSource();
        int vibor = 0;
        Boolean flagOff = true;

        Boolean Button0Clk = true, Button1Clk = true, Button2Clk = true, Button3Clk = true, Button4Clk = true, Button7Clk = true, Button8Clk = true, Button9Clk = true, 
                Button10Clk = true, Button11Clk = true, Button12Clk = true, Button13Clk = true, Button14Clk = true, Button15Clk = true, Button16Clk = true,
                Button17Clk = true, Button18Clk = true, Button19Clk = true, Button20Clk = true;

        public MainPage()
        {
            //button8.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/timer.png", UriKind.RelativeOrAbsolute)) };
            this.InitializeComponent();
            InitSerial();
        }

        private async void InitSerial()
        {
            string aqs = SerialDevice.GetDeviceSelector("UART0");
            var dis = await DeviceInformation.FindAllAsync(aqs);
            SerialPort = await SerialDevice.FromIdAsync(dis[0].Id);
            SerialPort.WriteTimeout = TimeSpan.FromMilliseconds(200);
            SerialPort.ReadTimeout = TimeSpan.FromMilliseconds(200);
            SerialPort.BaudRate = 9600;
            SerialPort.Parity = SerialParity.None;
            SerialPort.StopBits = SerialStopBitCount.One;
            SerialPort.DataBits = 8;

            dataWriter = new DataWriter();
            //dataReader = new DataReader(SerialPort.InputStream);
            Listen();
        }

        public async void SerialReceived()
        {
            /* Read data in from the serial port*/
            const uint maxReadLength = 1024;
            dataReader = new DataReader(SerialPort.InputStream);
            uint bytesToRead = await dataReader.LoadAsync(maxReadLength);
            rxBuffer = dataReader.ReadString(bytesToRead);
            //receivedData.Text = rxBuffer;
        }

        public async void SerialSend(string txBuffer)
        {
            /* Write a string out over serial */
            //string txBuffer = txBuffer2;
            dataWriter.WriteString(txBuffer);
            uint bytesWritten = await SerialPort.OutputStream.WriteAsync(dataWriter.DetachBuffer());
        }


        private async void Listen()
        {
            try
            {
                if (SerialPort != null)
                {
                    dataReader = new DataReader(SerialPort.InputStream);

                    // keep reading the serial input
                    while (true)
                    {
                        await ReadAsync(ReadCancellationTokenSource.Token);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Uart Error", ex);
            }
        }

        private async Task ReadAsync(CancellationToken cancellationToken)
        {
            Task<UInt32> loadAsyncTask;
            uint ReadBufferLength = 100;
            // If task cancellation was requested, comply
            cancellationToken.ThrowIfCancellationRequested();
            // Set InputStreamOptions to complete the asynchronous read operation when one or more bytes is available
            dataReader.InputStreamOptions = InputStreamOptions.Partial;
            // Create a task object to wait for data on the serialPort.InputStream
            loadAsyncTask = dataReader.LoadAsync(ReadBufferLength).AsTask(cancellationToken);
            // Launch the task and wait
            UInt32 bytesRead = await loadAsyncTask;
            if (bytesRead > 0)
            {
                serialData = dataReader.ReadString(bytesRead);
                string[]  serialDataArrayStr = serialData.Split(',');

                //string[] separators = { "," };
                // string[] serialDataArrayStr = serialData.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                //receivedData.Text = String.Concat(serialDataArrayStr);
                textBlock3.Text = serialDataArrayStr[5]; //установка Vishi
                txtTemperature2.Text = serialDataArrayStr[6]; //температура
                textBlock4.Text = serialDataArrayStr[10]; //установка вибро
                textBlock.Text = serialDataArrayStr[11]; //установка таймера
                textBlock6.Text = serialDataArrayStr[12]; //секундный таймер
                txtTemperature.Text = serialDataArrayStr[13]; //температура
                textBlock1.Text = serialDataArrayStr[14]; //установка пара
                textBlock2.Text = serialDataArrayStr[15]; //установка IR
           

                if (serialData != serialDataСheck) {
                    //общий
                    if (serialDataArrayStr[0] == "a")
                    {
                        button0.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/onoff.png", UriKind.RelativeOrAbsolute)) };
                        button1.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/vent.png", UriKind.RelativeOrAbsolute)) };
                        button2.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/svet.png", UriKind.RelativeOrAbsolute)) };
                        button3.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/ir.png", UriKind.RelativeOrAbsolute)) };
                        button4.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/par.png", UriKind.RelativeOrAbsolute)) };
                        button7.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/voda.png", UriKind.RelativeOrAbsolute)) };
                        button8.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/vibro.png", UriKind.RelativeOrAbsolute)) };
                        button9.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/time.png", UriKind.RelativeOrAbsolute)) };
                        button17.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/svetvanna.png", UriKind.RelativeOrAbsolute)) };
                        button18.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/massage.png", UriKind.RelativeOrAbsolute)) };
                        button19.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/gidro.png", UriKind.RelativeOrAbsolute)) };
                        button20.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/func.png", UriKind.RelativeOrAbsolute)) };
                        Button0Clk = true;
                        Button10Clk = true; Button11Clk = true; Button12Clk = true; Button13Clk = true; Button14Clk = true; Button15Clk = true; Button16Clk = true;
                        Button17Clk = true; Button18Clk = true; Button19Clk = true; Button20Clk = true;
                        textBlock5.Text = "OFF";
                        flagOff = true;
                    }
                    if (serialDataArrayStr[0] == "A")
                    {
                        button0.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/onoffg.png", UriKind.RelativeOrAbsolute)) };
                        Button0Clk = false;
                        textBlock5.Text = "On";
                        flagOff = false;
                    }
                    //вентилятор
                    if (serialDataArrayStr[1] == "b")
                    {
                        button1.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/vent.png", UriKind.RelativeOrAbsolute)) };
                        Button1Clk = true;
                    }
                    if (serialDataArrayStr[1] == "B")
                    {
                        button1.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/ventg.png", UriKind.RelativeOrAbsolute)) };
                        Button1Clk = false;
                    }
                    //свет
                    if (serialDataArrayStr[2] == "c")
                    {
                        button2.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/svet.png", UriKind.RelativeOrAbsolute)) };
                        Button2Clk = true;
                    }
                    if (serialDataArrayStr[2] == "C")
                    {
                        button2.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/svetg.png", UriKind.RelativeOrAbsolute)) };
                        Button2Clk = false;
                    }
                    //IR
                    if (serialDataArrayStr[3] == "d")
                    {
                        button3.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/ir.png", UriKind.RelativeOrAbsolute)) };
                        Button3Clk = true;
                    }
                    if (serialDataArrayStr[3] == "D")
                    {
                        button3.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/irg.png", UriKind.RelativeOrAbsolute)) };
                        Button3Clk = false;
                    }
                    //пар
                    if (serialDataArrayStr[4] == "e")
                    {
                        button4.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/par.png", UriKind.RelativeOrAbsolute)) };
                        Button4Clk = true;
                    }
                    if (serialDataArrayStr[4] == "E")
                    {
                        button4.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/parg.png", UriKind.RelativeOrAbsolute)) };
                        Button4Clk = false;
                    }
                    //душ виши
                    if (serialDataArrayStr[7] == "g")
                    {
                        button7.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/voda.png", UriKind.RelativeOrAbsolute)) };
                        Button7Clk = true;
                    }
                    if (serialDataArrayStr[7] == "G")
                    {
                        button7.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/vodag.png", UriKind.RelativeOrAbsolute)) };
                        Button7Clk = false;
                    }
                    //вибро
                    if (serialDataArrayStr[8] == "h")
                    {
                        button8.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/vibro.png", UriKind.RelativeOrAbsolute)) };
                        Button8Clk = true;
                    }
                    if (serialDataArrayStr[8] == "H")
                    {
                        button8.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/vibrog.png", UriKind.RelativeOrAbsolute)) };
                        Button8Clk = false;
                    }
                    //таймер
                    if (serialDataArrayStr[9] == "i")
                    {
                        button9.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/time.png", UriKind.RelativeOrAbsolute)) };
                        Button9Clk = true;
                    }
                    if (serialDataArrayStr[9] == "I")
                    {
                        button9.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/timeg.png", UriKind.RelativeOrAbsolute)) };
                        Button9Clk = false;
                    }

                    //ванна хромотерапия
                    if (serialDataArrayStr[16] == "j")
                    {
                        button17.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/svetvanna.png", UriKind.RelativeOrAbsolute)) };
                        Button17Clk = true;
                    }
                    if (serialDataArrayStr[16] == "J")
                    {
                        button17.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/svetvannag.png", UriKind.RelativeOrAbsolute)) };
                        Button17Clk = false;
                    }
                    //ванна массаж
                    if (serialDataArrayStr[17] == "k")
                    {
                        button18.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/massage.png", UriKind.RelativeOrAbsolute)) };
                        Button18Clk = true;
                    }
                    if (serialDataArrayStr[17] == "K")
                    {
                        button18.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/massageg.png", UriKind.RelativeOrAbsolute)) };
                        Button18Clk = false;
                    }
                    //ванна гидромассаж
                    if (serialDataArrayStr[18] == "l")
                    {
                        button19.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/gidro.png", UriKind.RelativeOrAbsolute)) };
                        Button19Clk = true;
                    }
                    if (serialDataArrayStr[18] == "L")
                    {
                        button19.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/gidrog.png", UriKind.RelativeOrAbsolute)) };
                        Button19Clk = false;
                    }
                    //ванна пустая функция
                    if (serialDataArrayStr[19] == "m")
                    {
                        button20.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/func.png", UriKind.RelativeOrAbsolute)) };
                        Button20Clk = true;
                    }
                    if (serialDataArrayStr[19] == "M")
                    {
                        button20.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/funcg.png", UriKind.RelativeOrAbsolute)) };
                        Button20Clk = false;
                    }
                    //установка душа виши 1
/*                    if (serialDataArrayStr[20] == "n")
                    {

                    }
                    if (serialDataArrayStr[20] == "N")
                    {

                    }
                    //установка душа виши 2
                    if (serialDataArrayStr[21] == "o")
                    {

                    }
                    if (serialDataArrayStr[21] == "O")
                    {

                    }
                    //установка душа виши 3
                    if (serialDataArrayStr[22] == "q")
                    {

                    }
                    if (serialDataArrayStr[22] == "Q")
                    {

                    }
                    //установка душа виши 4
                    if (serialDataArrayStr[23] == "r")
                    {

                    }
                    if (serialDataArrayStr[23] == "R")
                    {

                    }
*/


                }


                serialDataСheck = serialData;

            }
        }

        //OnOff
        private void Button0_Click(object sender, RoutedEventArgs e)
        {
            if (Button0Clk == true)
            {
                SerialSend("A");
                button0.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/onoffg.png", UriKind.RelativeOrAbsolute)) };
                textBlock5.Text = "On";
                Button0Clk = false;
                flagOff = false;
            }
            else
            {
                SerialSend("a");
                button0.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/onoff.png", UriKind.RelativeOrAbsolute)) };
                button1.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/vent.png", UriKind.RelativeOrAbsolute)) };
                button2.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/svet.png", UriKind.RelativeOrAbsolute)) };
                button3.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/ir.png", UriKind.RelativeOrAbsolute)) };
                button4.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/par.png", UriKind.RelativeOrAbsolute)) };
                button7.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/voda.png", UriKind.RelativeOrAbsolute)) };
                button8.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/vibro.png", UriKind.RelativeOrAbsolute)) };
                button9.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/time.png", UriKind.RelativeOrAbsolute)) };
                Button0Clk = true;
                Button10Clk = true; Button11Clk = true; Button12Clk = true; Button13Clk = true; Button14Clk = true; Button15Clk = true; Button16Clk = true;
                textBlock5.Text = "OFF";
                flagOff = true;
            }
        }

        //вентилятор
        private void Button1_Click(object sender, RoutedEventArgs e) 
        {
            if (Button1Clk)
            {
                if (flagOff == false) {
                    SerialSend("B");
                    button1.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/ventg.png", UriKind.RelativeOrAbsolute)) };
                    Button1Clk = false;}
            }
            else
            {
                SerialSend("b");
                button1.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/vent.png", UriKind.RelativeOrAbsolute)) };
                Button1Clk = true;
            }
            
        }
        //свет
        private void Button2_Click_1(object sender, RoutedEventArgs e)
        {
            if (Button2Clk)
            {
                if (flagOff == false)
                {
                    SerialSend("C");
                    button2.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/svetg.png", UriKind.RelativeOrAbsolute)) };
                    Button2Clk = false;
                }
            }
            else
            {
                SerialSend("c");
                button2.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/svet.png", UriKind.RelativeOrAbsolute)) };
                Button2Clk = true;
            }
            
        }

        //IR
        private void Button3_Click(object sender, RoutedEventArgs e)
        {
            if (Button3Clk)
            {
                if (flagOff == false)
                {
                    SerialSend("D");
                    button3.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/irg.png", UriKind.RelativeOrAbsolute)) };
                    Button3Clk = false;
                }
            }
            else
            {
                SerialSend("d");
                button3.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/ir.png", UriKind.RelativeOrAbsolute)) };
                Button3Clk = true;
            }   
        }
        //пар
        private void Button4_Click(object sender, RoutedEventArgs e)
        {
            if (Button4Clk)
            {
                if (flagOff == false)
                {
                    SerialSend("E");
                    button4.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/parg.png", UriKind.RelativeOrAbsolute)) };
                    Button4Clk = false;
                }
            }
            else
            {
                SerialSend("e");
                button4.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/par.png", UriKind.RelativeOrAbsolute)) };
                Button4Clk = true;
            }    
        }
        //плюс
        private void Button5_Click(object sender, RoutedEventArgs e)
        {
            //SerialSend("f");
            //button5.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/plus.png", UriKind.RelativeOrAbsolute)) };

            switch (vibor)
            {
                case 1: //таймер
                    SerialSend("T");
                    break;

                case 2: //установка пара
                    SerialSend("P");
                    break;

                case 4: //установка IR
                    SerialSend("X");
                    break;

                case 5: //установка света
                    SerialSend("s");
                    break;

                case 6: //уставка виши
                    SerialSend("N");
                    break;

                case 7: //установка вибро
                    SerialSend("v");
                    break;

                default:
                    break;
            }
        }
        //минус
        private void Button6_Click(object sender, RoutedEventArgs e)
        {
            switch (vibor)
            {
                case 1: //таймер
                    SerialSend("t");
                    break;

                case 2: //установка пара
                    SerialSend("p");
                    break;

                case 4: //установка IR
                    SerialSend("x");
                    break;

                case 5: //установка света
                    SerialSend("s");
                    break;

                case 6: //уставка виши
                    SerialSend("n");
                    break;

                case 7: //установка вибро
                    SerialSend("V");
                    break;

                default:
                    break;
            }
            //SerialSend("F");
            //button6.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/minus.png", UriKind.RelativeOrAbsolute)) };            
        }
        //душ виши
        private void Button7_Click(object sender, RoutedEventArgs e)
        {
            if (Button7Clk)
            {
                if (flagOff == false)
                {
                    SerialSend("G");
                    button7.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/vodag.png", UriKind.RelativeOrAbsolute)) };
                    Button7Clk = false;
                }
            }
            else
            {
                SerialSend("g");
                button7.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/voda.png", UriKind.RelativeOrAbsolute)) };
                Button7Clk = true;
            }            
        }
        //вибро
        private void Button8_Click(object sender, RoutedEventArgs e)
        {
            if (Button8Clk)
            {
                if (flagOff == false)
                {
                    SerialSend("H");
                    button8.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/vibrog.png", UriKind.RelativeOrAbsolute)) };
                    Button8Clk = false;
                }
            }
            else
            {
                SerialSend("h");   
                button8.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/vibro.png", UriKind.RelativeOrAbsolute)) };
                Button8Clk = true;
            }       
        }

        //запуск таймера
        private void Button9_Click(object sender, RoutedEventArgs e)
        {
            if (Button9Clk)
            {
                if (flagOff == false)
                {
                    SerialSend("I");
                    button9.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/timeg.png", UriKind.RelativeOrAbsolute)) };
                    Button9Clk = false;
                }
            }
            else
            {
                SerialSend("i");
                button9.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/time.png", UriKind.RelativeOrAbsolute)) };
                Button9Clk = true;
            }
        }

        //ванна хромотерапия
        private void Button17_Click_1(object sender, RoutedEventArgs e)
        {
            if (Button17Clk)
            {
                if (flagOff == false)
                {
                    SerialSend("J");
                    button17.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/svetvannag.png", UriKind.RelativeOrAbsolute)) };
                    Button17Clk = false;
                }
            }
            else
            {
                SerialSend("j");
                button17.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/svetvanna.png", UriKind.RelativeOrAbsolute)) };
                Button17Clk = true;
            }
        }
        //ванна массаж
        private void Button18_Click_1(object sender, RoutedEventArgs e)
        {
            if (Button18Clk)
            {
                if (flagOff == false)
                {
                    SerialSend("K");
                    button18.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/massageg.png", UriKind.RelativeOrAbsolute)) };
                    Button18Clk = false;
                }
            }
            else
            {
                SerialSend("k");
                button18.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/massage.png", UriKind.RelativeOrAbsolute)) };
                Button18Clk = true;
            }
        }
        //ванна гидромассаж
        private void Button19_Click_1(object sender, RoutedEventArgs e)
        {
            if (Button19Clk)
            {
                if (flagOff == false)
                {
                    SerialSend("L");
                    button19.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/gidrog.png", UriKind.RelativeOrAbsolute)) };
                    Button19Clk = false;
                }
            }
            else
            {
                SerialSend("l");
                button19.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/gidro.png", UriKind.RelativeOrAbsolute)) };
                Button19Clk = true;
            }
        }

        //ванна пустая функция
        private void Button20_Click_1(object sender, RoutedEventArgs e)
        {
            if (Button20Clk)
            {
                if (flagOff == false)
                {
                    SerialSend("M");
                    button20.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/funcg.png", UriKind.RelativeOrAbsolute)) };
                    Button20Clk = false;
                }
            }
            else
            {
                SerialSend("m");
                button20.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/func.png", UriKind.RelativeOrAbsolute)) };
                Button20Clk = true;
            }
        }


        //установка таймера
        private void Button10_Click(object sender, RoutedEventArgs e)
        {
            if (Button10Clk == true)
            {
                vibor = 1;
                button10.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/timerg.png", UriKind.RelativeOrAbsolute)) };
                button11.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/steam.png", UriKind.RelativeOrAbsolute)) };
                button12.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/infared.png", UriKind.RelativeOrAbsolute)) };
                button13.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/vichy.png", UriKind.RelativeOrAbsolute)) };
                button14.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/facefan.png", UriKind.RelativeOrAbsolute)) };
                button15.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/chromo.png", UriKind.RelativeOrAbsolute)) };
                button16.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/vibromassaje.png", UriKind.RelativeOrAbsolute)) };
                Button10Clk = false; Button11Clk = true; Button12Clk = true; Button13Clk = true; Button14Clk = true; Button15Clk = true; Button16Clk = true;
            }
            else
            {
                vibor = 0;
                button10.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/timer.png", UriKind.RelativeOrAbsolute)) };
                Button10Clk = true;
            }            
        }
        //установка пара
        private void Button11_Click(object sender, RoutedEventArgs e)
        {
            if (Button11Clk == true)
            {
                vibor = 2;
                button10.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/timer.png", UriKind.RelativeOrAbsolute)) };
                button11.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/steamg.png", UriKind.RelativeOrAbsolute)) };
                button12.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/infared.png", UriKind.RelativeOrAbsolute)) };
                button13.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/vichy.png", UriKind.RelativeOrAbsolute)) };
                button14.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/facefan.png", UriKind.RelativeOrAbsolute)) };
                button15.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/chromo.png", UriKind.RelativeOrAbsolute)) };
                button16.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/vibromassaje.png", UriKind.RelativeOrAbsolute)) };
                Button11Clk = false; Button10Clk = true; Button12Clk = true; Button13Clk = true; Button14Clk = true; Button15Clk = true; Button16Clk = true;
            }
            else
            {
                vibor = 0;
                button11.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/steam.png", UriKind.RelativeOrAbsolute)) };
                Button11Clk = true;
            }           
        }
        //установка IR
        private void Button12_Click(object sender, RoutedEventArgs e)
        {
            if (Button12Clk == true)
            {
                vibor = 4;
                button10.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/timer.png", UriKind.RelativeOrAbsolute)) };
                button11.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/steam.png", UriKind.RelativeOrAbsolute)) };
                button12.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/infaredg.png", UriKind.RelativeOrAbsolute)) };
                button13.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/vichy.png", UriKind.RelativeOrAbsolute)) };
                button14.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/facefan.png", UriKind.RelativeOrAbsolute)) };
                button15.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/chromo.png", UriKind.RelativeOrAbsolute)) };
                button16.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/vibromassaje.png", UriKind.RelativeOrAbsolute)) };
                Button12Clk = false; Button10Clk = true; Button11Clk = true; Button13Clk = true; Button14Clk = true; Button15Clk = true; Button16Clk = true;
            }
            else
            {
                vibor = 0;
                button12.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/infared.png", UriKind.RelativeOrAbsolute)) };
                Button12Clk = true;
            }         
        }
        //установка душа виши
        private void Button13_Click(object sender, RoutedEventArgs e)
        {
            if (Button13Clk == true)
            {
                vibor = 6;
                button10.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/timer.png", UriKind.RelativeOrAbsolute)) };
                button11.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/steam.png", UriKind.RelativeOrAbsolute)) };
                button12.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/infared.png", UriKind.RelativeOrAbsolute)) };
                button13.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/vichyg.png", UriKind.RelativeOrAbsolute)) };
                button14.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/facefan.png", UriKind.RelativeOrAbsolute)) };
                button15.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/chromo.png", UriKind.RelativeOrAbsolute)) };
                button16.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/vibromassaje.png", UriKind.RelativeOrAbsolute)) };
                Button13Clk = false; Button10Clk = true; Button11Clk = true; Button12Clk = true; Button14Clk = true; Button15Clk = true; Button16Clk = true;
            }
            else
            {
                vibor = 0;
                button13.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/vichy.png", UriKind.RelativeOrAbsolute)) };
                Button13Clk = true;
            }           
        }
        //установка вентилятора
        private void Button14_Click(object sender, RoutedEventArgs e)
        {
            if (Button14Clk == true)
            {
                
                button10.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/timer.png", UriKind.RelativeOrAbsolute)) };
                button11.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/steam.png", UriKind.RelativeOrAbsolute)) };
                button12.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/infared.png", UriKind.RelativeOrAbsolute)) };
                button13.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/vichy.png", UriKind.RelativeOrAbsolute)) };
                button14.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/facefang.png", UriKind.RelativeOrAbsolute)) };
                button15.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/chromo.png", UriKind.RelativeOrAbsolute)) };
                button16.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/vibromassaje.png", UriKind.RelativeOrAbsolute)) };
                Button14Clk = false; Button10Clk = true; Button11Clk = true; Button12Clk = true; Button13Clk = true; Button15Clk = true; Button16Clk = true;
            }
            else
            {
                
                button14.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/facefan.png", UriKind.RelativeOrAbsolute)) };
                Button14Clk = true;
            }            
        }
        //установка света
        private void Button15_Click(object sender, RoutedEventArgs e)
        {
            if (Button15Clk == true)
            {
                vibor = 5;
                button10.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/timer.png", UriKind.RelativeOrAbsolute)) };
                button11.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/steam.png", UriKind.RelativeOrAbsolute)) };
                button12.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/infared.png", UriKind.RelativeOrAbsolute)) };
                button13.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/vichy.png", UriKind.RelativeOrAbsolute)) };
                button14.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/facefan.png", UriKind.RelativeOrAbsolute)) };
                button15.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/chromog.png", UriKind.RelativeOrAbsolute)) };
                button16.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/vibromassaje.png", UriKind.RelativeOrAbsolute)) };
                Button15Clk = false; Button10Clk = true; Button11Clk = true; Button12Clk = true; Button13Clk = true; Button14Clk = true; Button16Clk = true;
            }
            else
            {
                vibor = 0;
                button15.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/chromo.png", UriKind.RelativeOrAbsolute)) };
                Button15Clk = true;
            }          
        }
        //установка вибро
        private void Button16_Click(object sender, RoutedEventArgs e)
        {
            if (Button16Clk == true)
            {
                vibor = 7;
                button10.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/timer.png", UriKind.RelativeOrAbsolute)) };
                button11.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/steam.png", UriKind.RelativeOrAbsolute)) };
                button12.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/infared.png", UriKind.RelativeOrAbsolute)) };
                button13.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/vichy.png", UriKind.RelativeOrAbsolute)) };
                button14.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/facefan.png", UriKind.RelativeOrAbsolute)) };
                button15.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/chromo.png", UriKind.RelativeOrAbsolute)) };
                button16.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/vibromassajeg.png", UriKind.RelativeOrAbsolute)) };
                Button16Clk = false; Button10Clk = true; Button11Clk = true; Button12Clk = true; Button13Clk = true; Button14Clk = true; Button15Clk = true;
            }
            else
            {
                vibor = 0;
                button16.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri("ms-appx:/Images/vibromassaje.png", UriKind.RelativeOrAbsolute)) };
                Button16Clk = true;
            }        
        }
    }
}
