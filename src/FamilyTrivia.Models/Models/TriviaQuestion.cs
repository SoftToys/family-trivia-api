namespace FamilyTrivia.Contracts.Models
{
    public class TriviaQuestion
    {
        /// <summary>
        /// Gets or sets the Question text.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show the next question when this question anwered
        /// </summary>
        /// <value>
        /// <c>true</c> if to show the next question when this question anwered; otherwise, <c>false</c>.
        /// </value>
        public bool AutoPlayNextQuestion { get; set; }
    }
}