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
        /// Gets or sets the next action.
        /// </summary>
        /// <value>
        /// The next action.
        /// </value>
        public ActionOnResolve NextAction { get; set; }

    }

    public enum ActionOnResolve
    {
        MoveToNextQuestion,
        ShowNotification

    }
}