var typesOfQuestions = [{ Id: 1, Name: "Short Answer" }, { Id: 2, Name: "Boolean Answer" },
{ Id: 3, Name: "Multiple Choice" },
{ Id: 4, Name: "Multi Select" }, { Id: 5, Name: "Rating" }
];

var typesOfRatingQuestions = [{ Id: 1, Name: "Worst - Excellent" }, { Id: 2, Name: "Disagree - Agree" },
    { Id: 3, Name: "Smileys" }, { Id: 4, Name: "Slider" }
];

var sliderRatingValues = [{ Id: 5, Name: "5" }, { Id: 10, Name: "10" }
];

var worstToExcellent = ["Worst", "Not Good", "Neutral", "Good", "Excellent"];

var disagreeToAgree = ["Strongly Disagree", "Disagree", "Neutral", "Agree", "Strongly Agree"];

var smileyContent = ["Very Dissatisfied", "Dissatisfied", "Neutral", "Satisfied", "Very Satisfied"];

var smileyClass = ["classVeryDissatisfied", "classDissatisfied", "classNeutral", "classSatisfied", "classVerySatisfied"];

var smileyImages = ["feedback_very_dissatisfied.png", "feedback_dissatisfied.png", "feedback_neutral.png",
    "feedback_satisfied.png", "feedback_very_satisfied.png"];

function escapeHtml(str) {
    if (str == undefined || str == null)
        return str;
    return str.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;").replace(/"/g, "&quot;").replace(/'/g, "&#039;");
}