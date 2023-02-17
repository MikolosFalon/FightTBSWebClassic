using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    //text points attack and defense
    [SerializeField] private TextMeshProUGUI attackPointsText;
    [SerializeField] private TextMeshProUGUI defensePointsText;

    //lists value text 
    [SerializeField] private List<TextMeshProUGUI> attackValueText;
    [SerializeField] private List<TextMeshProUGUI> defenseValueText;

    //hp text
    [SerializeField] private TextMeshProUGUI playerHPText;
    [SerializeField] private TextMeshProUGUI enemyHPText;

    //timer
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private float timer;
    private float ActiveTimer;

    [SerializeField] private TextMeshProUGUI VictoryText;
    private GameObject startWindow;
    private Battle battle;

    private void Start() {
        //set timer from tz 15 seconds
        timer = 15;
        ActiveTimer = timer;
        battle = FindObjectOfType<Battle>();

        attackPointsText.text = "Доступно очков атаки: " + 
        battle.Player.AttackPoints.ToString();
        defensePointsText.text = "Доступно очков защиты: " + 
        battle.Player.DefensePoints.ToString();
    }

    //start battle button
    public void StartBattle(GameObject window){
        //tz for player = 8 ,enemy = 6
        battle.Player.Hp = 8;
        battle.Enemy.Hp = 6;
        StartTactic();
        window.SetActive(false);
        startWindow = window;
    }

    //voids for buttons
    public void AttackButtons(int index){
        //check points and battle wave
        if(battle.gameState ==GameState.turnPlayer && battle.Player.AttackPoints>0){
            //add to value 
            battle.Player.AttackPoints--;
            battle.Player.PowerZoneAttacks[index]++;
            //update text
            attackValueText[index].text = battle.Player.PowerZoneAttacks[index].ToString();
            attackPointsText.text = "Доступно очков атаки: " + 
            battle.Player.AttackPoints.ToString();
        }
    }

    public void DefenseButtons(int index){
        //check points and battle wave
        if (battle.gameState == GameState.turnPlayer && battle.Player.DefensePoints > 0)
        {
            //check stats
            if (!battle.Player.PowerZoneDefense[index])
            {
                //add to value
                battle.Player.DefensePoints--;
                battle.Player.PowerZoneDefense[index]=true;
                //update text
                defenseValueText[index].text = "X";
                defensePointsText.text = "Доступно очков защиты: " + 
                battle.Player.DefensePoints.ToString();
            }
        }
    }

    //timer
    IEnumerator timerSetUp(){
        while (ActiveTimer >0)
        {
            yield return new WaitForSeconds(1.0f);
            if(battle.gameState ==GameState.turnPlayer){
                ActiveTimer--;
                timerText.text ="Таймер: "+ ActiveTimer.ToString()+"сек"; 
            }else{
                yield return null;
            }
        }
        EndTurn();
    }
    //start player turn
    public void StartTactic(){
        enemyHPText.text = battle.Enemy.Hp.ToString();
        playerHPText.text = battle.Player.Hp.ToString();
        battle.Turn(GameState.turnPlayer);
        battle.SetStartPowerZones();
        ActiveTimer = timer;
        StartCoroutine(timerSetUp());
        //clear all value text 
        foreach (var item in attackValueText)
        {
            item.text = "0";
        }
        foreach (var item in defenseValueText)
        {
            item.text = "0";
        }
        //set active points
        //tz 4
        battle.Player.AttackPoints = 4;
        battle.Player.DefensePoints = 4;        
    }
    //end player turn
    public void EndTurn(){
        battle.Turn(GameState.turnEnemy);
    }

    //match end
    public void Victory(string WinnerName){
        VictoryText.text = "Winner: " + WinnerName;
        startWindow.SetActive(true);
    }
}
