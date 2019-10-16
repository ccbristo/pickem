namespace ExampleCSharpBot.PickemApi.Models
{
    public enum GameStates
    {
        SpreadNotSet,
        SpreadLocked,
        InGame,
        Final,
        Cancelled
    }

    public enum GameLeaderTypes
    {
        Away,
        Home,
        None
    }

    public enum PickemScoringTypes
    {
        AllWinsOnePoint,
        VariablePoints,
    }

    public enum PickTypes
    {
        Away,
        Home,
        None,
        Hidden // THIS SHOULD NEVER be in the database (only for UI hides)
    }

    public enum PickStates
    {
        Cancelled,
        Losing,
        Lost,
        None,
        Pushing,
        Pushed,
        Winning,
        Won
    }

    public enum SpreadDirections
    {
        None,
        ToAway,
        ToHome
    }
}
