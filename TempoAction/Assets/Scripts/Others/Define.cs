

public class Define
{

    public enum UIType
    {
        ALL, BUTTON, IMAGE, CANVASGROUP, TMPRO, TOGGLE, SLIDER
    }

    public enum KeyType
    {
        DOWN, HOLD, UP
    }

    public enum BuffType // ENTER : 범위에 들어가면 실행 , STAY : 범위에 들어가 있으면 실행, EXIT : 범위에서 나가면 실행 
    {
        ENTER, STAY, EXIT, NONE
    }

    public enum BuffInfo
    {
        NONE,
        SUPERJUMP,

        #region buff
        SPEEDUP,
        POWERUP,
        HEAL,
        #endregion

        #region debuff
        TICKDAMAGE

        #endregion
        
    }
    
    public enum WarpType
    {
        NONE,
        MIDDLEBOSS
    }
   


    #region 플레이어
    public enum TempoType
    {
        MAIN, POINT, NONE
    }

    public enum AttackState
    {
        ATTACK, CHECK, FINISH
    }

    public enum PlayerState
    {
        STUN, NONE
    }

    public enum PlayerSfxType
    {
        MAIN, POINT, DASH, RUN, JUMP, STUN, NONE
    }

    #region Skill
    public enum SkillColliderType
    {
        FORWARD,
        CENTER
    }

    public enum SkillType
    {
        ATK, // 공격형
        BUF, // 버프형
        MOV, // 기동형
    }

    public enum SkillTarget
    {
        NONE,       // 없음
        PC,            // Playable Character
        SELF,        // 자기자신
        ENEMY    // 적(몬스터)
    }

    public enum SkillEffectType
    {
        NONE,
        RUSH
    }
    #endregion
    #endregion

    #region 몬스터
    public enum PerceptionType
    {
        PATROL, BOUNDARY, DETECTIONM
    }

    #region 엘리트 몬스터
    public enum ElitePhaseState
    {
        START, PHASE1, PHASECHANGE, PHASE2, FINISH, NONE
    }
    public enum EliteMonsterState
    {
        IDLE, USESKILL, GROGGY, FAIL, DIE, NONE
    }
    public enum EliteMonsterSkill
    {
        FATTACK, SATTACK, LASER, LAUNCH, RUSH, SUPERPUNCH, BARRIER, THUNDERSTROKE, EXPLOSION, PLATFORM, NONE
    }
    #endregion

    #region 중간 보스
    public enum MiddlePhaseState
    {
        START, PHASE1, PHASECHANGE, PHASE2, FINISH, NONE
    }
    public enum MiddleMonsterState
    {
        IDLE, USESKILL, GROGGY, FAIL, DIE, NONE
    }
    public enum MiddleMonsterSkill
    {
        HOMERUN, SHELLING, TAKEDOWN, DROPBOMB, NONE
    }
    public enum MiddleMonsterPoint
    {
        BOMBDROPPOINT, CSPAWNPOINT, GSPAWNPOINT, SHELLINGPOINT, NONE
    }
    public enum MiddleMonsterName
    {
        CHEONG, GYEONGCHAE, NONE
    }
    #endregion

    #endregion

}
