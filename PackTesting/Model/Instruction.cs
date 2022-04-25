using Newtonsoft.Json;
using System.Drawing;

namespace PackTesting.Model
{
    public class Instruction
    {
        /// <summary>
        /// Gets or sets the step.
        /// </summary>
        /// <value>
        /// The step.
        /// </value>
        [JsonProperty("InstructionSequence")]
        public string Step { get; set; }

        /// <summary>
        /// Gets or sets the name of the step.
        /// </summary>
        /// <value>
        /// The name of the step.
        /// </value>
        [JsonProperty("InstructionText")]
        public string StepName { get; set; }

        /// <summary>
        /// Gets or sets the current instruction.
        /// </summary>
        /// <value>
        /// The current instruction.
        /// </value>
        [JsonIgnore]
        public Color CurrentInstructionColor { get; set; } = Color.LightGray;

        /// <summary>
        /// Gets the step number.
        /// </summary>
        /// <value>
        /// The step number.
        /// </value>
        public string StepNumber
        {
            get
            {
                return string.Format("Step {0}: ", Step);
            }
        }
    }
}