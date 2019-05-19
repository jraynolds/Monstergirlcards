using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TriggeredEffectsManager : MonoBehaviour
{
    // PROPERTIES
    // Singleton
    public static TriggeredEffectsManager Instance;

    // Dictionaries containing a list of effects for each game event.
    // An example would be -- every time a TopPlayer monster is killed, a bottomplayer monster gets +1 attack.
    public enum TEffectDictionaries { Game, TopPlayer, BottomPlayer };
    public enum EffectTriggers
    {
        GameStart,
        StartTurn,
        EndTurn,
        CardDrawn,
        CardDiscarded,
        SpellPlayed,
        CreaturePlayed,
        CreatureKilled,
        DamageDealt,
        GameEnd
    }
    private Dictionary<EffectTriggers, List<Effect>> GameEffects = new Dictionary<EffectTriggers, List<Effect>>()
    {
        { EffectTriggers.GameStart, new List<Effect>() },
        { EffectTriggers.StartTurn, new List<Effect>() },
        { EffectTriggers.EndTurn, new List<Effect>() },
        { EffectTriggers.CardDrawn, new List<Effect>() },
        { EffectTriggers.CardDiscarded, new List<Effect>() },
        { EffectTriggers.SpellPlayed, new List<Effect>() },
        { EffectTriggers.CreaturePlayed, new List<Effect>() },
        { EffectTriggers.CreatureKilled, new List<Effect>() },
        { EffectTriggers.DamageDealt, new List<Effect>() },
        { EffectTriggers.GameEnd, new List<Effect>() }
    };
    private Dictionary<EffectTriggers, List<Effect>> TopPlayerEffects = new Dictionary<EffectTriggers, List<Effect>>()
    {
        { EffectTriggers.StartTurn, new List<Effect>() },
        { EffectTriggers.EndTurn, new List<Effect>() },
        { EffectTriggers.CardDrawn, new List<Effect>() },
        { EffectTriggers.CardDiscarded, new List<Effect>() },
        { EffectTriggers.SpellPlayed, new List<Effect>() },
        { EffectTriggers.CreaturePlayed, new List<Effect>() },
        { EffectTriggers.CreatureKilled, new List<Effect>() },
        { EffectTriggers.DamageDealt, new List<Effect>() },
    };
    private Dictionary<EffectTriggers, List<Effect>> BottomPlayerEffects = new Dictionary<EffectTriggers, List<Effect>>()
    {
        { EffectTriggers.StartTurn, new List<Effect>() },
        { EffectTriggers.EndTurn, new List<Effect>() },
        { EffectTriggers.CardDrawn, new List<Effect>() },
        { EffectTriggers.CardDiscarded, new List<Effect>() },
        { EffectTriggers.SpellPlayed, new List<Effect>() },
        { EffectTriggers.CreaturePlayed, new List<Effect>() },
        { EffectTriggers.CreatureKilled, new List<Effect>() },
        { EffectTriggers.DamageDealt, new List<Effect>() },
    };

    // DEFAULT METHODS
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // METHODS
    private Dictionary<EffectTriggers, List<Effect>> GetDict(TEffectDictionaries dict)
    {
        if (dict == TEffectDictionaries.Game)
            return GameEffects;
        else if (dict == TEffectDictionaries.TopPlayer)
            return TopPlayerEffects;
        else
            return BottomPlayerEffects;
    }

    public void AddEffects(TEffectDictionaries dictEnum, EffectTriggers trigger, List<Effect> effects)
    {
        Dictionary<EffectTriggers, List<Effect>> dict = GetDict(dictEnum);

        if (dict.ContainsKey(trigger))
            dict[trigger].AddRange(effects);
        else
            dict[trigger] = effects;
    }

    public void RemoveEffects(TEffectDictionaries dictEnum, EffectTriggers trigger, List<Effect> effects)
    {
        Dictionary<EffectTriggers, List<Effect>> dict = GetDict(dictEnum);

        if (dict.ContainsKey(trigger))
            dict[trigger] = dict[trigger].Except(effects).ToList();
    }

    public void RemoveEffects(TEffectDictionaries dictEnum, List<Effect> effects)
    {
        Dictionary<EffectTriggers, List<Effect>> dict = GetDict(dictEnum);

        foreach (EffectTriggers key in dict.Keys)
        {
            dict[key] = dict[key].Except(effects).ToList();
        }
    }

    public void RemoveEffects(List<Effect> effects)
    {
        Dictionary<EffectTriggers, List<Effect>>[] dicts = new Dictionary<EffectTriggers, List<Effect>>[] { GameEffects, TopPlayerEffects, BottomPlayerEffects };

        foreach (Dictionary < EffectTriggers, List<Effect>> dict in dicts)
        {
            foreach (EffectTriggers key in dict.Keys)
            {
                dict[key] = dict[key].Except(effects).ToList();
            }
        }
    }

    public void InvokeEffect(TEffectDictionaries dictEnum, EffectTriggers trigger, Player p1 = null, Player p2 = null, CardLogic card1 = null, CardLogic card2 = null)
    {
        Dictionary<EffectTriggers, List<Effect>> dict = GetDict(dictEnum);

        foreach (Effect e in dict[trigger]) e.InvokeEffect(p1, p2, card1, card2);
    }

    public void InvokeEffect(TEffectDictionaries dictEnum, EffectTriggers trigger, Player p1 = null, Player p2 = null, CreatureLogic card1 = null, CardLogic card2 = null)
    {
        Dictionary<EffectTriggers, List<Effect>> dict = GetDict(dictEnum);

        foreach (Effect e in dict[trigger]) e.InvokeEffect(p1, p2, card1, card2);
    }
}
