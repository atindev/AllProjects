using ScaleTryConsole;
using System;
using System.Collections;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace West.Kiosk.Core.ScaleLib.ScaleLibrary
{
    // SUPPORT FOR TOLEDO SCALES MADE IN BRAZIL:

    /// <summary>
    /// ToledoScale
    /// </summary>
    /// <seealso cref="West.Kiosk.Core.ScaleLib.AbsNetworkScale" />
    public class ToledoScale : AbsNetworkScale
    {
        #region "private variables"

        /// <summary>
        /// Gets or sets the scale object.
        /// </summary>
        /// <value>
        /// The scale object.
        /// </value>
        private ScaleObject scaleObject { get; set; }

        #endregion

        #region "public methods / interface overrides"

        /// <summary>
        /// Initializes a new instance of the <see cref="ToledoScale"/> class.
        /// </summary>
        /// <param name="ScaleObject">The scale object.</param>
        public ToledoScale(ScaleObject ScaleObject)
        {
            scaleObject = ScaleObject;
        }

        /// <summary>
        /// Connections the established.
        /// </summary>
        /// <returns></returns>
        public override bool ConnectionAvailable()
        {
            bool connected = false;

            try
            {
                if (scaleObject != null)
                {

                    IPHostEntry iphostInfo = Dns.GetHostEntry(scaleObject.ScalehostName);
                    IPAddress ipAdress = iphostInfo.AddressList[0];
                    IPEndPoint ipEndpoint = new IPEndPoint(ipAdress, scaleObject.ReadPort);

                    Socket client = new Socket(ipAdress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                    client.Connect(ipEndpoint);
                    connected = client.Connected;
                    client.Close();
                }
            }
            catch
            {
                connected = false;
            }
            return connected;
        }

        /// <summary>
        /// Zeroes the scale.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">Scale does not allow command.</exception>
        public override async Task<ScaleResults> ZeroScale()
        {
            try
            {
                CheckCommand(scaleObject.enableZero);
                ToledoScaleSendWriteCommand(scaleObject.cmdZero);
                return await GetWeight();
            }
            catch (Exception e)
            {
                return new ScaleResults()
                {
                    success = false,
                    ErrorText = e.Message
                };
            }
        }

        /// <summary>
        /// Tares the scale.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.Exception">Scale does not allow command.</exception>
        public override async Task<ScaleResults> TareScale()
        {
            try
            {
                CheckCommand(scaleObject.enableTare);
                ToledoScaleSendWriteCommand(scaleObject.cmdTare);
                return await GetWeight();
            }
            catch (Exception e)
            {
                return new ScaleResults()
                {
                    success = false,
                    ErrorText = e.Message
                };
            }
        }

        /// <summary>
        /// Resets the scale.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.Exception">Scale does not allow command.</exception>
        public override async Task<ScaleResults> ResetScale()
        {
            try
            {
                CheckCommand(scaleObject.enableResetClear);
                ToledoScaleSendWriteCommand(scaleObject.cmdResetClear);
                return await GetWeight();
            }
            catch (Exception e)
            {
                return new ScaleResults()
                {
                    success = false,
                    ErrorText = e.Message
                };
            }
        }

        /// <summary>
        /// Gets the weight.
        /// </summary>
        /// <returns></returns>
        public override async Task<ScaleResults> GetWeight()
        {
            try
            {
                return await ToledoScaleSendReadCommand(scaleObject.cmdRead);
            }
            catch (Exception e)
            {
                return new ScaleResults()
                {
                    ScaleNetValue = 0.0m,
                    ScaleTareValue = 0.0m,
                    ScaleGrossValue = 0.0m,
                    success = false,
                    ErrorText = e.Message
                };
            }
        }

        /// <summary>
        /// Gets the tare.
        /// </summary>
        /// <returns></returns>
        public override async Task<ScaleResults> GetTare()
        {
            try
            {
                return await ToledoScaleSendReadCommand(scaleObject.cmdRead);
            }
            catch (Exception e)
            {
                return new ScaleResults()
                {
                    ScaleNetValue = 0.0m,
                    ScaleTareValue = 0.0m,
                    ScaleGrossValue = 0.0m,
                    success = false,
                    ErrorText = e.Message
                };
            }
        }

        #endregion

        #region "Private methods"

        /// <summary>
        /// Sends the write command.
        /// </summary>
        /// <param name="scaleCmd">The scale command.</param>
        /// <returns></returns>
        private void ToledoScaleSendWriteCommand(string scaleCmd)
        {
            IPHostEntry iphostInfo = Dns.GetHostEntry(scaleObject.ScalehostName);
            IPAddress ipAdress = iphostInfo.AddressList[0];
            IPEndPoint ipEndpoint = new IPEndPoint(ipAdress, scaleObject.WritePort);

            Socket client = new Socket(ipAdress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            client.Connect(ipEndpoint);

            char[] charValues = scaleCmd.ToCharArray();
            StringBuilder hexOutput = new StringBuilder();
            foreach (char _eachChar in charValues)
            {
                int value = Convert.ToInt32(_eachChar);
                hexOutput.Append(String.Format("{0:X}", value));
            }

            string hexForCmd = String.Format("02{0}0d", hexOutput);

            byte[] bytes = HexToByte(hexForCmd);

            client.NoDelay = true;
            client.LingerState = new LingerOption(true, 1);

            _ = client.Send(bytes);

            client.Close();
        }

        /// <summary>
        /// Sends the read command.
        /// </summary>
        /// <param name="scaleCmd">The scale command.</param>
        /// <returns></returns>
        private async Task<ScaleResults> ToledoScaleSendReadCommand(string scaleCmd)
        {

            ScaleResults retToledoScaleResults = new ScaleResults();

            retToledoScaleResults.ErrorText = String.Empty;
            retToledoScaleResults.isScaleStable = false;
            retToledoScaleResults.ScaleNetValue = 0.0m;
            retToledoScaleResults.ScaleTareValue = 0.0m;
            retToledoScaleResults.ScaleGrossValue = 0.0m;
            retToledoScaleResults.success = false;

            byte[] data = new byte[50];

            IPHostEntry iphostInfoToledo = Dns.GetHostEntry(scaleObject.ScalehostName);
            IPAddress ipAdressToledo = iphostInfoToledo.AddressList[0];
            IPEndPoint ipEndpointToledo = new IPEndPoint(ipAdressToledo, scaleObject.ReadPort);

            Socket clientToledo = new Socket(ipAdressToledo.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            clientToledo.Connect(ipEndpointToledo);

            if (scaleCmd != string.Empty)
            {
                char[] charValues = scaleCmd.ToCharArray();
                StringBuilder hexOutput = new StringBuilder();
                foreach (char _eachChar in charValues)
                {
                    int value = Convert.ToInt32(_eachChar);
                    hexOutput.Append(String.Format("{0:X}", value));
                }

                string hexForCmd = String.Format("02{0}0d", hexOutput);

                byte[] bytes = HexToByte(hexForCmd);

                clientToledo.NoDelay = true;
                clientToledo.LingerState = new LingerOption(true, 1);

                _ = clientToledo.Send(bytes);
            }

            await Task.Delay(100);
            _ = clientToledo.Receive(data);

            string ToledoResponse = Encoding.ASCII.GetString(data);

            decimal weight, tare, netweight;

            if (ToledoResponse.Trim().Length >= 15)
            {
                char swb;
                char.TryParse(ToledoResponse.Substring(2, 1), out swb);

                byte[] byteSWB = new byte[1] { 0 };

                byteSWB[0] = Convert.ToByte(swb);

                var bits = new BitArray(byteSWB);

                // SWB - STATUS WORD “B”
                // BIT 0----------->NET WEIGHT = 1
                // BIT 1----------->NEGATIVE WEIGHT = 1
                // BIT 2----------->OVERLOAD = 1
                // BIT 3----------->MOVING = 1
                // BIT 4----------->ALWAYS = 1
                // BIT 5----------->ALWAYS = 1
                // BIT 6----------->IF SELF - FREE = 1
                // BIT 7----------->PAR PARITY

                retToledoScaleResults.isScaleStable = bits[3];
                retToledoScaleResults.isScaleOutOfRange = bits[2];

                ToledoResponse = ToledoResponse.Substring(4);
                decimal divisor;

                decimal.TryParse(ToledoResponse.Substring(0, 6), out weight);
                decimal.TryParse(ToledoResponse.Substring(6, 6), out tare);

                // weight value returned is negative
                if (bits[1])
                {
                    weight = weight * -1;
                }

                if (bits[0])
                {
                    // weight value returned is net weight
                    netweight = weight;
                    weight = netweight + tare;
                }
                else
                {
                    // weight value returned is gross weight
                    netweight = weight - tare;
                }

                //determine based on decimal positions the divisor value:
                divisor = decimal.Parse("100000".Substring(0, scaleObject.Precision + 1));

                string decimalFormat = "{0:N" + scaleObject.Precision + "}";

                retToledoScaleResults.ScaleNetValue = netweight / divisor;
                retToledoScaleResults.ScaleTareValue = tare / divisor;
                retToledoScaleResults.ScaleGrossValue = weight / divisor;
                retToledoScaleResults.ScaleUOM = scaleObject.UOM;
                retToledoScaleResults.ScaleTareText = string.Format(CultureInfo.CurrentCulture, decimalFormat, retToledoScaleResults.ScaleTareValue) + " " + scaleObject.UOM;
                retToledoScaleResults.ScaleNetText = string.Format(CultureInfo.CurrentCulture, decimalFormat, retToledoScaleResults.ScaleNetValue) + " " + scaleObject.UOM;
                retToledoScaleResults.ScaleGrossText = string.Format(CultureInfo.CurrentCulture, decimalFormat, retToledoScaleResults.ScaleGrossValue) + " " + scaleObject.UOM;
                retToledoScaleResults.success = true;
            }

            clientToledo.Close();

            return retToledoScaleResults;
        }

        /// <summary>
        /// Hexadecimals to byte.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <returns></returns>
        private byte[] HexToByte(string msg)
        {

            //remove any spaces from the string

            msg = msg.Replace(" ", "");

            //create a byte array the length of the

            //divided by 2 (Hex is 2 characters in length)

            byte[] comBuffer = new byte[msg.Length / 2];

            //loop through the length of the provided string

            for (int i = 0; i < msg.Length; i += 2)

                //convert each set of 2 characters to a byte

                //and add to the array

                comBuffer[i / 2] = Convert.ToByte(msg.Substring(i, 2), 16);

            //return the array

            return comBuffer;

        }

        #endregion
    }
}
