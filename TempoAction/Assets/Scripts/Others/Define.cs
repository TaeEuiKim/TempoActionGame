

public class Define
{
    public enum BuffType // ENTER : 범위에 들어가면 실행 , STAY : 범위에 들어가 있으면 실행, EXIT : 범위에서 나가면 실행 
    {
        ENTER, STAY, EXIT
    }

    public enum BuffInfo
    {
        SUPERJUMP,

        #region buff
        SPEEDUP,
        POWERUP,
        HEAL,
        #endregion

        #region debuff
        TICKDAMAGE,

        #endregion
        NONE
    }

    public enum UIType
    {
        ALL, BUTTON, IMAGE, CANVASGROUP, TMPRO, TOGGLE, SLIDER
    }
    public enum TempoType
    {
        MAIN, POINT, NONE
    }

    public enum AtkState
    {
        ATTACK, CHECK, FINISH
    }

    public enum PlayerState
    {
        OVERLOAD, STUN, NONE
    }

    public enum CircleState
    {
        MISS, BAD, GOOD, PERFECT, NONE
    }

    public enum PlayerSfxType
    {
        MAIN, POINT, DASH, RUN, JUMP, STUN, NONE
    }
}
