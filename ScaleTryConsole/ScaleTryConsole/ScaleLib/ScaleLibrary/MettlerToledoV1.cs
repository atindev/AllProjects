using ScaleTryConsole;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace West.Kiosk.Core.ScaleLib.ScaleLibrary
{
    /// <summary>
    /// MettlerToledoV1
    /// </summary>
    /// <seealso cref="West.Kiosk.Core.ScaleLib.AbsNetworkScale" />
    public class MettlerToledoV1 : AbsNetworkScale
    {
        #region "private variables"

        /// <summary>
        /// Gets or sets the scale object.
        /// </summary>
        /// <value>
        /// The scale object.
        /// </value>
        private ScaleObject MettlerToledoV1ScaleObject { get; set; }

        #endregion

        #region "public methods / interface overrides"

        /// <summary>
        /// Initializes a new instance of the <see cref="MettlerToledoV1"/> class.
        /// </summary>
        /// <param name="ScaleObject">The scale object.</param>
        public MettlerToledoV1(ScaleObject ScaleObject)
        {
            MettlerToledoV1ScaleObject = ScaleObject;
        }

        /// <summary>
        /// Connections the established.
        /// </summary>
        /// <returns></returns>
        public override bool ConnectionAvailable()
        {
            bool connectedMettlerToledoV1 = false;

            try
            {
                if (MettlerToledoV1ScaleObject != null)
                {

                    IPHostEntry iphostInfoMettlerToledoV1 = Dns.GetHostEntry(MettlerToledoV1ScaleObject.ScalehostName);
                    IPAddress ipAdressMettlerToledoV1 = iphostInfoMettlerToledoV1.AddressList[0];
                    IPEndPoint ipEndpointMettlerToledoV1 = new IPEndPoint(ipAdressMettlerToledoV1, MettlerToledoV1ScaleObject.ReadPort);

                    Socket clientMettlerToledoV1 = new Socket(ipAdressMettlerToledoV1.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                    clientMettlerToledoV1.Connect(ipEndpointMettlerToledoV1);

                    connectedMettlerToledoV1 = clientMettlerToledoV1.Connected;

                    clientMettlerToledoV1.Close();
                }
            }
            catch
            {
                connectedMettlerToledoV1 = false;
            }

            return connectedMettlerToledoV1;
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
                CheckCommand(MettlerToledoV1ScaleObject.enableZero);
                MettlerToledoV1SendWriteCommand(MettlerToledoV1ScaleObject.cmdZero);
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
        /// <exception cref="System.ArgumentException">Scale does not allow command.</exception>
        public override async Task<ScaleResults> TareScale()
        {
            try
            {
                CheckCommand(MettlerToledoV1ScaleObject.enableTare);
                MettlerToledoV1SendWriteCommand(MettlerToledoV1ScaleObject.cmdTare);
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
        /// <exception cref="System.ArgumentException">Scale does not allow command.</exception>
        public override async Task<ScaleResults> ResetScale()
        {
            try
            {
                CheckCommand(MettlerToledoV1ScaleObject.enableResetClear);
                MettlerToledoV1SendWriteCommand(MettlerToledoV1ScaleObject.cmdResetClear);
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
            ScaleResults scaleResults;
            try
            {
                scaleResults = await MettlerToledoV1SendReadCommand(MettlerToledoV1ScaleObject.cmdRead);
                scaleResults.success = true;
            }
            catch (Exception e)
            {
                scaleResults = new ScaleResults()
                {
                    ScaleNetValue = 0.0m,
                    ScaleTareValue = 0.0m,
                    ScaleGrossValue = 0.0m,
                    success = false,
                    ErrorText = e.Message
                };
            }
            return scaleResults;
        }
        #endregion

        #region "Private methods"
        /// <summary>
        /// Sends the write command.
        /// </summary>
        /// <param name="scaleCmd">The scale command.</param>
        private void MettlerToledoV1SendWriteCommand(string scaleCmd)
        {

            IPHostEntry iphostInfoMettlerToledoV1 = Dns.GetHostEntry(MettlerToledoV1ScaleObject.ScalehostName);
            IPAddress ipAdressMettlerToledoV1 = iphostInfoMettlerToledoV1.AddressList[0];
            IPEndPoint ipEndpointMettlerToledoV1 = new IPEndPoint(ipAdressMettlerToledoV1, MettlerToledoV1ScaleObject.WritePort);

            Socket clientMettlerToledoV1 = new Socket(ipAdressMettlerToledoV1.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            clientMettlerToledoV1.Connect(ipEndpointMettlerToledoV1);

            char[] charValues = scaleCmd.ToCharArray();
            StringBuilder hexOutput = new StringBuilder();
            foreach (char _eachChar in charValues)
            {
                int value = Convert.ToInt32(_eachChar);
                hexOutput.Append(String.Format("{0:X}", value));
            }

            string hexForCmd = String.Format("02{0}0d", hexOutput);

            byte[] bytes = HexToByte(hexForCmd);

            clientMettlerToledoV1.NoDelay = true;
            clientMettlerToledoV1.LingerState = new LingerOption(true, 1);

            _ = clientMettlerToledoV1.Send(bytes);

            clientMettlerToledoV1.Close();
        }

        /// <summary>
        /// Sends the read command.
        /// </summary>
        /// <param name="scaleCmd">The scale command.</param>
        /// <returns></returns>
        private async Task<ScaleResults> MettlerToledoV1SendReadCommand(string scaleCmd)
        {
            ScaleResults returnScaleResults = new ScaleResults()
            {
                ErrorText = String.Empty,
                isScaleStable = false,
                ScaleNetValue = 0.0m,
                ScaleTareValue = 0.0m,
                ScaleGrossValue = 0.0m,
                success = false
            };
            IPHostEntry iphostInfo = Dns.GetHostEntry(MettlerToledoV1ScaleObject.ScalehostName);
            IPAddress ipAdress = iphostInfo.AddressList[0];
            IPEndPoint ipEndpoint = new IPEndPoint(ipAdress, MettlerToledoV1ScaleObject.ReadPort);

            Socket client = new Socket(ipAdress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            client.Connect(ipEndpoint);

            if (!client.Connected)
                return returnScaleResults;

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

                client.NoDelay = true;
                client.LingerState = new LingerOption(true, 1);

                _ = client.Send(bytes);
            }

            client.LingerState = new LingerOption(true, 1);
            var buffer = new List<byte>();
            await Task.Delay(200);

            while (client.Available > 0 && buffer.Count < 50)
            {
                var currByte = new Byte[1];
                var byteCounter = client.Receive(currByte, currByte.Length, SocketFlags.None);

                if (byteCounter.Equals(1))
                {
                    buffer.Add(currByte[0]);
                }
            }

            client.Disconnect(true);
            client.Close();

            string response = Encoding.ASCII.GetString(buffer.ToArray());

            int startIndex = response.IndexOf("\u0002");

            if (startIndex < 0)
            {
                return returnScaleResults;
            }

            response = response.Substring(startIndex + 1);

            return GetValuesfromResponse(returnScaleResults, response);
        }

        /// <summary>
        /// Gets the valuesfrom response.
        /// </summary>
        /// <param name="retScaleResults">The return scale results.</param>
        /// <param name="response">The response.</param>
        /// <returns></returns>
        private ScaleResults GetValuesfromResponse(ScaleResults retScaleResults, string response)
        {
            decimal weight, tare, netweight;

            if (response.Trim().Length >= 21)
            {
                char swa;
                char.TryParse(response.Substring(0, 1), out swa);
                byte[] byteSWA = new byte[1] { 0 };
                byteSWA[0] = Convert.ToByte(swa);

                char swb;
                char.TryParse(response.Substring(1, 1), out swb);
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

                response = response.Substring(4);
                decimal divisor;

                decimal.TryParse(response.Substring(1, 5), out weight);
                decimal.TryParse(response.Substring(6, 5), out tare);

                // weight value returned is negative
                if (bits[1])
                {
                    weight = weight * -1;
                }

                // weight value returned is net weight
                netweight = weight;
                weight = netweight + tare;

                //determine based on decimal positions the divisor value:
                divisor = decimal.Parse("100000".Substring(0, MettlerToledoV1ScaleObject.Precision + 1));

                string decimalFormat = "{0:N" + MettlerToledoV1ScaleObject.Precision + "}";

                //// bits[3];
                retScaleResults.isScaleStable = true;
                retScaleResults.ScaleNetValue = netweight / divisor;
                retScaleResults.ScaleTareValue = tare / divisor;
                retScaleResults.ScaleGrossValue = weight / divisor;
                retScaleResults.ScaleUOM = MettlerToledoV1ScaleObject.UOM;
                retScaleResults.ScaleTareText = string.Format(CultureInfo.CurrentCulture, decimalFormat, retScaleResults.ScaleTareValue) + " " + MettlerToledoV1ScaleObject.UOM;
                retScaleResults.ScaleNetText = string.Format(CultureInfo.CurrentCulture, decimalFormat, retScaleResults.ScaleNetValue) + " " + MettlerToledoV1ScaleObject.UOM;
                retScaleResults.ScaleGrossText = string.Format(CultureInfo.CurrentCulture, decimalFormat, retScaleResults.ScaleGrossValue) + " " + MettlerToledoV1ScaleObject.UOM;
                retScaleResults.success = true;
            }

            return retScaleResults;
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
