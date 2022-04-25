namespace West.Kiosk.Core.ScaleLib.Implementations
{
    using ScaleTryConsole;
    using System;
    using System.Threading.Tasks;
    using West.Kiosk.Core.ScaleLib.ScaleLibrary;

    /// <summary>
    /// NetworkScale
    /// </summary>
    /// <seealso cref="West.Kiosk.Core.ScaleLib.AbsNetworkScale" />
    public class NetworkScale : AbsNetworkScale
    {
        #region "private variables"

        /// <summary>
        /// Gets or sets the scale object.
        /// </summary>
        /// <value>
        /// The scale object.
        /// </value>
        private ScaleObject scaleObject { get; set; }

        /// <summary>
        /// The scale results
        /// </summary>
        private ScaleResults scaleResults = null;

        /// <summary>
        /// The scale
        /// </summary>
        private readonly AbsNetworkScale scale = null;

        #endregion

        #region "public methods / interface overrides"

        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkScale"/> class.
        /// </summary>
        /// <param name="ScaleObject">The scale object.</param>
        /// <exception cref="System.ArgumentException">Unsupported Scale Library</exception>
        public NetworkScale(ScaleObject ScaleObject)
        {
            scaleObject = ScaleObject;

            switch (scaleObject.ScaleLibrary)
            {
                case "Mettler-Toledo":
                case nameof(MettlerToledoV1):
                    //NA Mettler-Toledo Scale
                    scale = new MettlerToledoV1(scaleObject);
                    break;

                case nameof(ToledoScale):
                    //Brazil Scale
                    scale = new ToledoScale(scaleObject);
                    break;

                case "Mettler-Toledo v2":
                case nameof(MettlerToledoV2):
                    //EU/AP Mettler-Toledo Scale
                    scale = new MettlerToledoV2(scaleObject);
                    break;

                case nameof(NodeSimulator):
                    //NodeJS Scale Simulator
                    scale = new NodeSimulator(scaleObject);
                    break;

                default:
                    throw new ArgumentException("Unsupported Scale Library");
            }
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
                if (scale != null)
                    connected = scale.ConnectionAvailable();
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
        public override async Task<ScaleResults> ZeroScale()
        {
            try
            {
                if (scale != null)
                    scaleResults = await scale.ZeroScale();
            }
            catch (Exception e)
            {
                scaleResults.success = false;
                scaleResults.ErrorText = e.Message;
            }
            return (scaleResults);
        }

        /// <summary>
        /// Tares the scale.
        /// </summary>
        /// <returns></returns>
        public override async Task<ScaleResults> TareScale()
        {
            try
            {
                if (scale != null)
                    scaleResults = await scale.TareScale();
            }
            catch (Exception e)
            {
                scaleResults.success = false;
                scaleResults.ErrorText = e.Message;
                return (scaleResults);
            }
            return (scaleResults);
        }

        /// <summary>
        /// Resets the scale.
        /// </summary>
        /// <returns></returns>
        public override async Task<ScaleResults> ResetScale()
        {
            try
            {
                if (scale != null)
                    scaleResults = await scale.ResetScale();
            }
            catch (Exception e)
            {
                scaleResults.success = false;
                scaleResults.ErrorText = e.Message;

                return (scaleResults);
            }
            return scaleResults;
        }

        /// <summary>
        /// Gets the weight.
        /// </summary>
        /// <returns></returns>
        public override async Task<ScaleResults> GetWeight()
        {
            try
            {
                if (scale != null)
                    scaleResults = await scale.GetWeight();
            }
            catch (Exception e)
            {
                scaleResults.ScaleNetValue = 0.0m;
                scaleResults.ScaleTareValue = 0.0m;
                scaleResults.ScaleGrossValue = 0.0m;
                scaleResults.success = false;
                scaleResults.ErrorText = e.Message;
            }
            return (scaleResults);
        }
        #endregion
    }
}
