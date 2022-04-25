using ScaleTryConsole;
using System;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace West.Kiosk.Core.ScaleLib.ScaleLibrary
{
    // SUPPORT FOR NodeJS Scale Simulator for testing purposes.
    // This will work with the NodeJS Scale Simulator v5.0

    /// <summary>
    /// NodeSimulator
    /// </summary>
    /// <seealso cref="West.Kiosk.Core.ScaleLib.AbsNetworkScale" />
    public class NodeSimulator : AbsNetworkScale
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
        /// Initializes a new instance of the <see cref="NodeSimulator"/> class.
        /// </summary>
        /// <param name="ScaleObject">The scale object.</param>
        public NodeSimulator(ScaleObject ScaleObject)
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
                return await NodeSimulatorSendReadCommand(scaleObject.cmdZero);
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
        /// <exception cref="System.ArgumentException">Scale does not allow command.</exception>
        public override async Task<ScaleResults> TareScale()
        {
            try
            {
                CheckCommand(scaleObject.enableTare);
                return await NodeSimulatorSendReadCommand(scaleObject.cmdTare);
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
        /// <exception cref="System.ArgumentException">Scale does not allow command.</exception>
        public override async Task<ScaleResults> ResetScale()
        {
            try
            {
                CheckCommand(scaleObject.enableResetClear);
                return await NodeSimulatorSendReadCommand(scaleObject.cmdResetClear);
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
                return await NodeSimulatorSendReadCommand(scaleObject.cmdRead);
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
        /// Sends the read command.
        /// </summary>
        /// <param name="scaleCmd">The scale command.</param>
        /// <returns></returns>
        private async Task<ScaleResults> NodeSimulatorSendReadCommand(string scaleCmd)
        {

            ScaleResults retScaleResult = new ScaleResults();

            retScaleResult.ErrorText = String.Empty;
            retScaleResult.isScaleStable = false;
            retScaleResult.ScaleNetValue = 0.0m;
            retScaleResult.ScaleTareValue = 0.0m;
            retScaleResult.ScaleGrossValue = 0.0m;
            retScaleResult.success = false;

            byte[] data = new byte[35];

            IPHostEntry iphostInfo = Dns.GetHostEntry(scaleObject.ScalehostName);
            IPAddress ipAdress = iphostInfo.AddressList[0];
            IPEndPoint ipEndpoint = new IPEndPoint(ipAdress, scaleObject.ReadPort);

            Socket client = new Socket(ipAdress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            client.Connect(ipEndpoint);

            if (client.Connected)
            {
                if (scaleCmd != string.Empty)
                {
                    char[] charValues = scaleCmd.ToCharArray();
                    StringBuilder hexOutput = new StringBuilder();
                    foreach (char _eachChar in charValues)
                    {
                        int value = Convert.ToInt32(_eachChar);
                        hexOutput.Append(String.Format("{0:X}", value));
                    }

                    ////string hexForCmd = String.Format("02{0}0d", hexOutput);
                    string hexForCmd = String.Format("{0}", hexOutput);

                    byte[] bytes = HexToByte(hexForCmd);

                    client.NoDelay = true;
                    client.LingerState = new LingerOption(true, 1);

                    _ = client.Send(bytes);
                }

                await Task.Delay(100);
                _ = client.Receive(data);

                string ScaleResponse = Encoding.ASCII.GetString(data);

                decimal weight, tare, netweight;

                if (ScaleResponse.Trim().Length >= 4)
                {
                    ScaleResponse = ScaleResponse.Substring(3);
                    decimal divisor;

                    decimal.TryParse(ScaleResponse.Substring(0, 11), out weight);
                    decimal.TryParse(ScaleResponse.Substring(12, 11), out tare);
                    netweight = weight;
                    weight = netweight + tare;

                    //determine based on decimal positions the divisor value:
                    divisor = decimal.Parse("100000".Substring(0, scaleObject.Precision + 1));

                    string decimalFormat = "{0:N" + scaleObject.Precision + "}";

                    retScaleResult.isScaleStable = true;
                    retScaleResult.ScaleNetValue = netweight / divisor;
                    retScaleResult.ScaleTareValue = tare / divisor;
                    retScaleResult.ScaleGrossValue = weight / divisor;
                    retScaleResult.ScaleUOM = scaleObject.UOM;
                    retScaleResult.ScaleTareText = string.Format(CultureInfo.CurrentCulture, decimalFormat, retScaleResult.ScaleTareValue) + " " + scaleObject.UOM;
                    retScaleResult.ScaleNetText = string.Format(CultureInfo.CurrentCulture, decimalFormat, retScaleResult.ScaleNetValue) + " " + scaleObject.UOM;
                    retScaleResult.ScaleGrossText = string.Format(CultureInfo.CurrentCulture, decimalFormat, retScaleResult.ScaleGrossValue) + " " + scaleObject.UOM;
                    retScaleResult.success = true;
                }

                client.Close();
            }

            return retScaleResult;
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
