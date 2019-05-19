using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

// this class will take care of switching turns and counting down time until the turn expires
public class TurnManager : MonoBehaviour {

    // PUBLIC FIELDS
    //public CardAsset CoinCard;

    // for Singleton Pattern
    public static TurnManager Instance;

    // PRIVATE FIELDS
    // reference to a timer to measure 
    //private RopeTimer timer;


    // PROPERTIES
    private Player _whoseTurn;
    public Player WhoseTurn
    {
        get
        {
            return _whoseTurn;
        }

        set
        {
            _whoseTurn = value;

            CardActionManager.Instance.UnHighlightAll();

            TurnMaker tm = WhoseTurn.GetComponent<TurnMaker>();
            // player`s method OnTurnStart() will be called in tm.OnTurnStart();
            tm.OnTurnStart();
            if (tm is PlayerTurnMaker)
            {
                DebugManager.Instance.DebugMessage("A human is playing.", DebugManager.MessageType.Game);
            } else
            {
                // Do AI
                DebugManager.Instance.DebugMessage("An AI is playing.", DebugManager.MessageType.Game);
                GlobalSettings.Instance.EndTurnButton.interactable = true;
            }
        }
    }


    // METHODS
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        OnGameStart();
    }

    public void OnGameStart()
    {
        DebugManager.Instance.DebugMessage("Game is starting.", DebugManager.MessageType.System);

        CardLogic.CardsCreatedThisGame.Clear();
        CreatureLogic.CreaturesCreatedThisGame.Clear();

        foreach (Player p in Player.Players)
        {
            p.ManaThisTurn = 0;
            p.ManaLeft = 0;
            p.LoadCharacterInfoFromAsset();
            p.TransmitInfoAboutPlayerToVisual();
            p.PArea.PDeck.CardsInDeck = p.deck.cards.Count;
        }

        DebugManager.Instance.DebugMessage("Players instantiated.", DebugManager.MessageType.Game);

        Sequence s = DOTween.Sequence();
        s.PrependInterval(1f);
        s.OnComplete(() =>
            {
                // determine who starts the game.
                int rnd = Random.Range(0,2);
                Player whoGoesFirst = Player.Players[rnd];
                DebugManager.Instance.DebugMessage("First player: " + whoGoesFirst, DebugManager.MessageType.Game);
                Player whoGoesSecond = whoGoesFirst.OtherPlayer;

                // draw 4 cards for first player and 5 for second player
                DebugManager.Instance.DebugMessage("Drawing cards for players.", DebugManager.MessageType.Game);
                int initDraw = 4;
                for (int i = 0; i < initDraw; i++)
                {            
                    // second player draws a card
                    whoGoesSecond.DrawACard(true);
                    // first player draws a card
                    whoGoesFirst.DrawACard(true);
                }
                // add one more card to second player`s hand
                whoGoesSecond.DrawACard(true);
                new StartATurnCommand(whoGoesFirst).AddToQueue();
            });
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GlobalSettings.Instance.EndTurnButton.interactable = false;
            new EndATurnCommand().AddToQueue();
        }
    }

    public void EndTurn()
    {
        // send all commands in the end of current player`s turn
        WhoseTurn.OnTurnEnd();

        new StartATurnCommand(WhoseTurn.OtherPlayer).AddToQueue();
    }
}

