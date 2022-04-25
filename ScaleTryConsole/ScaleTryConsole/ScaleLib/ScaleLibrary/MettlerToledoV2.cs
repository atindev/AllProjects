﻿using ScaleTryConsole;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace West.Kiosk.Core.ScaleLib.ScaleLibrary
{
    // SUPPORT FOR EU/AP Region Mettler Toledo MT-SICS Level 2

    // Notes for Mettler Toledo MT-SICS Level 1
    // TAC = Tare Clear (sets to 0)    Status A = Success and I = Failed
    // TA = Tare Inquiry               Status A = Success and I = Failed   (A will return Tare Value)
    // T = Tare (Store next stable)    Status S = Success (returns Tare) I = Failed , + = Upper Weight Limit Exceeded, - = Lower Limit Range Exceeded
    // @ = Reset (Resets to base condition like reboot - does not clear weight)
    // ZI = Zero immediately Status D = Success (not stable), S = Success (stable), I = Failed, + = Upper Limit Exceeded, - = Lower Limit Exceeded
    // Z = Zero    A = Success, I = Failed, + = Upper Limit Exceeded, - = Lower Limit Exceeded
    // SI = Get Net Weight Immediately    S = Success (Stable) D = Success (not stable), I = Failed, + = Upper Limit Exceeded, - = Lower Limit Exceeded
    // S = Get Net Weight    S = Success (Stable), I = Failed, + = Upper Limit Exceeded, - = Lower Limit Exceeded

    // To ensure consistency between our scale libraries, this library will perform the necessary checks against the scale to 
    // enable returning of all components (Net, Tare, Gross) when returning data to the calling app.

    /// <summary>
    /// MettlerToledoV2
    /// </summary>
    /// <seealso cref="West.Kiosk.Core.ScaleLib.AbsNetworkScale" />
    public class MettlerToledoV2 : AbsNetworkScale
    {
        #region "private variables"

        /// <summary>
        /// Gets or sets the scale.
        /// </summary>
        /// <value>
        /// The scale.
        /// </value>
        private TcpClient scale { get; set; }

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
        /// Initializes a new instance of the <see cref="MettlerToledoV2"/> class.
        /// </summary>
        /// <param name="ScaleObject">The scale object.</param>
        public MettlerToledoV2(ScaleObject ScaleObject)
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
                    scale = new TcpClient(scaleObject.ScalehostName, scaleObject.ReadPort);
                    connected = scale.Connected;
                    if (connected)
                    {
                        scale.Dispose();
                        scale.Close();
                    }
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
                return await MettlerToledoV2SendReadCommand(scaleObject.cmdZero);
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
                return await MettlerToledoV2SendReadCommand(scaleObject.cmdTare);
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
                return await MettlerToledoV2SendReadCommand(scaleObject.cmdResetClear);
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
                return await MettlerToledoV2SendReadCommand(scaleObject.cmdRead);
            }
            catch (Exception e)
            {
                return new ScaleResults()
                {
                    ScaleNetValue = 0.0m,
                    ScaleTareValue = 0.0m,
                    ScaleGrossValue = 0.0m,
                    success = false,
                    ErrorText = e.Message,
                };
            }
        }

        #endregion

        #region "Private methods"

        /// <summary>
        /// Connects the specified host name.
        /// </summary>
        /// <param name="hostName">Name of the host.</param>
        /// <param name="port">The port.</param>
        /// <returns></returns>
        private TcpClient Connect(string hostName, int port)
        {
            scale = new TcpClient(hostName, port);
            return scale;
        }

        /// <summary>
        /// Gets the specific weight command.
        /// </summary>
        /// <param name="scaleCmd">The scale command.</param>
        /// <returns></returns>
        private async Task<IntScaleResults> MettlerToledoV2GetSpecificWeightCommand(string scaleCmd)
        {
            IntScaleResults intScaleResults = new IntScaleResults();

            try
            {
                if (scale == null || !scale.Connected)
                {
                    scale = Connect(scaleObject.ScalehostName, scaleObject.ReadPort);
                }

                Stream stream = scale.GetStream();
                StreamWriter writer = new StreamWriter(stream);
                StreamReader reader;

                if (!scale.Connected)
                    return intScaleResults;

                //using (NetworkStream stream = scale.GetStream())
                //{
                //    byte[] bytesToSend = Encoding.ASCII.GetBytes($"{scaleCmd }" + '\n');
                //    stream.Flush();
                //    await stream.WriteAsync(bytesToSend, 0, bytesToSend.Length);
                //    stream.Flush();

                //    byte[] readBytes = new byte[scale.ReceiveBufferSize];
                //    int bytess = await stream.ReadAsync(readBytes, 0, scale.ReceiveBufferSize);
                //    string result = Encoding.ASCII.GetString(readBytes, 0, bytess);

                //    string splitString = " ";
                //    char[] splitChar = splitString.ToCharArray();

                //    string[] resultData = result.Split(splitChar, StringSplitOptions.RemoveEmptyEntries);

                //    if (resultData.Length < 2)
                //        return intScaleResults;

                //    intScaleResults = checkStateResponses(intScaleResults, scaleCmd, resultData);
                //    return intScaleResults;
                //}

                using (writer)
                {
                    // Sending requested command by user:
                    writer.Write($"{scaleCmd}" + '\n');
                    writer.Flush();

                    await Task.Delay(100);

                    // Get response from Scale based on request by user:
                    stream = scale.GetStream();
                    string readlinevalue;
                    reader = new StreamReader(stream, Encoding.ASCII);
                    readlinevalue = reader.ReadLine();

                    string splitString = " ";
                    char[] splitChar = splitString.ToCharArray();

                    string[] resultData = readlinevalue.Split(splitChar, StringSplitOptions.RemoveEmptyEntries);

                    if (resultData.Length < 2)
                        return intScaleResults;

                    intScaleResults = checkStateResponses(intScaleResults, scaleCmd, resultData);
                    return intScaleResults;
                }
            }
            catch (Exception ex)
            {
                return intScaleResults;
            }
        }

        /// <summary>
        /// Checks the state responses.
        /// </summary>
        /// <param name="intScaleResults">The int scale results.</param>
        /// <param name="scaleCmd">The scale command.</param>
        /// <param name="resultData">The result data.</param>
        /// <returns></returns>
        private IntScaleResults checkStateResponses(IntScaleResults intScaleResults, string scaleCmd, string[] resultData)
        {
            // Responses for MT-SCALE:
            // TAC = Tare Clear (sets to 0)    Status A = Success and I = Failed
            // TA = Tare Inquiry               Status A = Success and I = Failed   (A will return Tare Value)
            // T = Tare (Store next stable)    Status S = Success (returns Tare) I = Failed , + = Upper Weight Limit Exceeded, - = Lower Limit Range Exceeded
            // @ = Reset (Resets to base condition like reboot - does not clear weight)
            // ZI = Zero immediately Status D = Success (not stable), S = Success (stable), I = Failed, + = Upper Limit Exceeded, - = Lower Limit Exceeded
            // Z = Zero    A = Success, I = Failed, + = Upper Limit Exceeded, - = Lower Limit Exceeded
            // SI = Get Net Weight Immediately    S = Success (Stable) D = Success (not stable), I = Failed, + = Upper Limit Exceeded, - = Lower Limit Exceeded
            // S = Get Net Weight    S = Success (Stable), I = Failed, + = Upper Limit Exceeded, - = Lower Limit Exceeded

            string[] cmdsCheckScaleOutOfRange = new string[] { "S", "SI", "SIR", "Z", "ZI", "T", "TAC" };
            if (cmdsCheckScaleOutOfRange.Contains(scaleCmd))
            {
                intScaleResults.isScaleOutOfRange = (resultData[1] == "+" || resultData[1] == "-");
            }
            string[] cmdsCheckScaleStateA = new string[] { "Z", "TA", "TAC" };
            if (cmdsCheckScaleStateA.Contains(scaleCmd))
            {
                intScaleResults.success = (resultData[1] == "A");
            }
            string[] cmdsCheckScaleStateS = new string[] { "S", "SI", "SR", "ZI", "TI", "T" };
            if (cmdsCheckScaleStateS.Contains(scaleCmd))
            {
                intScaleResults.success = (resultData[1] == "S");
            }
            if (resultData.Length >= 4)
            {
                string[] cmdsCheckGetNetWeight = new string[] { "S", "SI", "SIR" };
                if (cmdsCheckGetNetWeight.Contains(scaleCmd) && decimal.TryParse(resultData[2], out decimal net))
                {
                    intScaleResults.Weight = net;
                    intScaleResults.UOM = resultData[3];
                }
                string[] cmdsCheckGetTareWeight = new string[] { "T", "TI", "TA" };
                if (cmdsCheckGetTareWeight.Contains(scaleCmd) && decimal.TryParse(resultData[2], out decimal tara))
                {
                    intScaleResults.Weight = tara;
                    intScaleResults.UOM = resultData[3];
                }
            }

            return intScaleResults;
        }

        /// <summary>
        /// Sends the read command.
        /// </summary>
        /// <param name="scaleCmd">The scale command.</param>
        /// <returns></returns>
        private async Task<ScaleResults> MettlerToledoV2SendReadCommand(string scaleCmd)
        {
            ScaleResults returnScaleResults = new ScaleResults();

            returnScaleResults.ErrorText = String.Empty;
            returnScaleResults.success = false;

            decimal tare = 0.0m;
            decimal netweight = 0.0m;

            if (scale == null || !scale.Connected)
            {
                scale = Connect(scaleObject.ScalehostName, scaleObject.ReadPort);
            }

            IntScaleResults resultBase = await MettlerToledoV2GetSpecificWeightCommand(scaleCmd);

            if (resultBase.success)
            {
                // get remaining weighment data needed to return complete data set:
                // request to get weight - need to get Tare:
                if (scaleCmd != scaleObject.cmdRead)
                {
                    // need to get both values:
                    // Net Weight Inquiry
                    resultBase = await MettlerToledoV2GetSpecificWeightCommand("SI");
                }

                if (resultBase.success)
                {
                    netweight = resultBase.Weight;
                    scaleObject.UOM = resultBase.UOM;
                    returnScaleResults.isScaleStable = resultBase.isScaleStable;
                    returnScaleResults.isScaleOutOfRange = resultBase.isScaleOutOfRange;
                }

                // Tare Inquiry
                resultBase = await MettlerToledoV2GetSpecificWeightCommand("TA");
                if (resultBase.success)
                {
                    tare = resultBase.Weight;
                    returnScaleResults.isScaleStable = resultBase.isScaleStable;
                    returnScaleResults.isScaleOutOfRange = resultBase.isScaleOutOfRange;
                }
            }

            //determine based on decimal positions the divisor value:
            string decimalFormat = "{0:N" + scaleObject.Precision + "}";

            returnScaleResults.isScaleStable = true;
            returnScaleResults.ScaleNetValue = netweight;
            returnScaleResults.ScaleTareValue = tare;
            returnScaleResults.ScaleGrossValue = netweight + tare;
            returnScaleResults.ScaleUOM = scaleObject.UOM;
            returnScaleResults.ScaleTareText = string.Format(CultureInfo.CurrentCulture, decimalFormat, returnScaleResults.ScaleTareValue) + " " + scaleObject.UOM;
            returnScaleResults.ScaleNetText = string.Format(CultureInfo.CurrentCulture, decimalFormat, returnScaleResults.ScaleNetValue) + " " + scaleObject.UOM;
            returnScaleResults.ScaleGrossText = string.Format(CultureInfo.CurrentCulture, decimalFormat, returnScaleResults.ScaleGrossValue) + " " + scaleObject.UOM;
            returnScaleResults.success = resultBase.success;
            return returnScaleResults;
        }
        #endregion
    }
}
