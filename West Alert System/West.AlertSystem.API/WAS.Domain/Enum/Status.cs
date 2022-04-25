namespace WAS.Domain.Enum
{
    public enum Status
    {
        Submitted = 1,
        FirstLevelApproved = 2,
        FirstLevelRejected = 3,
        SecondLevelApproved = 4,
        SecondLevelRejected = 5,
        Failed = 6,
        Sent = 7
    }

    public enum SurveyStatus
    {
        Submitted = 1,
        SendNow = 2,
        Failed = 3,
        Sent = 4
    }

    public enum SurveyQuestionTypes
    {
        Short_Answer = 1,
        Boolean_Answer = 2,
        Multiple_Choice = 3,
        Multi_Select = 4,
        Rating = 5
    }

    public enum SurveyRatingTypes
    {
        Worst_Excellent = 1,
        Disagree_Agree = 2,
        Smileys = 3,
        Slider = 4
    }
}
