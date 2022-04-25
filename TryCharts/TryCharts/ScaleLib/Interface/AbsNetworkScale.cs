namespace West.Kiosk.Core.ScaleLib
{
    using ScaleTryConsole;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Abs Network Scale
    /// </summary>
    public abstract class AbsNetworkScale
    {
        /// <summary>
        /// Connections the established.
        /// </summary>
        /// <returns></returns>
        public abstract bool ConnectionAvailable();

        /// <summary>
        /// Zeroes the scale.
        /// </summary>
        /// <returns></returns>
        public abstract Task<ScaleResults> ZeroScale();

        /// <summary>
        /// Tares the scale.
        /// </summary>
        /// <returns></returns>
        public abstract Task<ScaleResults> TareScale();

        /// <summary>
        /// Resets the scale.
        /// </summary>
        /// <returns></returns>
        public abstract Task<ScaleResults> ResetScale();

        /// <summary>
        /// Gets the weight.
        /// </summary>
        /// <returns></returns>
        public abstract Task<ScaleResults> GetWeight();

        /// <summary>
        /// Gets the tare.
        /// </summary>
        /// <returns></returns>
        public abstract Task<ScaleResults> GetTare();

        /// <summary>
        /// Checks the command.
        /// </summary>
        /// <param name="command">if set to <c>true</c> [command].</param>
        /// <exception cref="System.ArgumentException">Scale does not allow command.</exception>
        protected void CheckCommand(bool command)
        {
            if (!command)
            {
                throw new ArgumentException("Scale does not allow command.");
            }
        }
    }
}
