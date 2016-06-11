using System.ComponentModel;

namespace Assets.Scripts.Helpers
{
    public enum AmmoType
    {
        Neutral, Fun, Sad
    }

    public enum BossBodyPart
    {
        Left, Central, Right
    }

    public enum Emotion
    {
        Netural, Fun, Sad
    }

    public enum AnalyticEventType
    {
        [Description("Shots fired")]
        ShotsFired,
        [Description("Emotion to express")]
        EmotionToExpress,
        [Description("Emotion registered")]
        EmotionRegistered,
        [Description("End score")]
        EndScore,
        [Description("Obstacle destroyed")]
        ObstacleDestoyed,
        [Description("Obstacle hit (not destroyed)")]
        ObstacleHit,
        [Description("Player lose")]
        PlayerLose,
        [Description("Player won")]
        PlayerWon,
        [Description("Boss health")]
        BossHealth,
        [Description("Boss fight started")]
        BossFightStarted,
        [Description("Boss killed to death")]
        BossKilled,
        [Description("Face lost")]
        FaceLost,
        [Description("Face found")]
        FaceFound,
        [Description("New game started")]
        NewGameStarted,
        [Description("Game closed")]
        GameClosed,
    }
}
