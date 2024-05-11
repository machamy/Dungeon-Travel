using Scripts.FSM;
using Scripts.Game.Dungeon.Unit;

public static class PlayerStates
{
    public static IState<PlayerUnit> Idle = new StateBuilder<PlayerUnit>()
        .SetEnterBehavior((e) => {})
        .SetExecuteBehavior(e => { })
        .SetExitBehavior(e => { }).Build() as IState<PlayerUnit>;
    public static IState<PlayerUnit> Run = new StateBuilder<PlayerUnit>()
        .SetEnterBehavior((e) => {})
        .SetExecuteBehavior(e => { })
        .SetExitBehavior(e => { }).Build() as IState<PlayerUnit>;
    public static IState<PlayerUnit> Walk = new StateBuilder<PlayerUnit>()
        .SetEnterBehavior((e) => {})
        .SetExecuteBehavior(e => { })
        .SetExitBehavior(e => { }).Build() as IState<PlayerUnit>;
}