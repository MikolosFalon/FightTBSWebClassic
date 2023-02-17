using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    //1 step start game
    startGame,
    //2 step player 
    turnPlayer,
    //3 step enemy
    turnEnemy,
    //4 fight 
    turnFight
    //5 step 1 or 2 

}
public class Battle : MonoBehaviour
{
    public GameState gameState;

    public Stats Player;
    public Stats Enemy;

    //animators
    [SerializeField]private Animator playerAnimator;
    [SerializeField]private Animator enemyAnimator;
    private UIManager uIManager;

    private void Start() {
        gameState = GameState.startGame;
        //tz for player = 8 ,enemy = 6
        Player.Hp = 8;
        Enemy.Hp = 6;
        //set lists
        //attack
        Player.PowerZoneAttacks = new List<int>();
        Enemy.PowerZoneAttacks = new List<int>();
        //defense
        Player.PowerZoneDefense = new List<bool>();
        Enemy.PowerZoneDefense = new List<bool>();

        uIManager = FindObjectOfType<UIManager>();


    }

    //match logic
    public void Turn(GameState state){
        gameState = state;
        if(gameState==GameState.turnEnemy){
            TurnEnemy();
        }
    }
    //set start power zones value
    public void SetStartPowerZones(){
        //set base value for turn
        //8 size zones  
        for (int i = 0; i < 8; i++)
        {
            Player.PowerZoneAttacks.Add(0);
            Enemy.PowerZoneAttacks.Add(0);
            Player.PowerZoneDefense.Add(false);
            Enemy.PowerZoneDefense.Add(false);
        }
        playerAnimator.Play("Idle");
        enemyAnimator.Play("Idle");
        
    }
    //turn enemy
    void TurnEnemy(){
        //enemy logic = random (only pve can tz)
        Enemy.AttackPoints = 4;
        Enemy.DefensePoints = 4;
        //points for attack
        while(Enemy.AttackPoints>0)
        {
            int indexAttack = Random.Range(0, Enemy.PowerZoneAttacks.Count);
            Enemy.PowerZoneAttacks[indexAttack]++;
            Enemy.AttackPoints--;
        }
        //points for defense
        while (Enemy.DefensePoints>0)
        {
            int indexDefense = Random.Range(0, Enemy.PowerZoneDefense.Count);
            if (!Enemy.PowerZoneDefense[indexDefense])
            {
                Enemy.PowerZoneDefense[indexDefense] = true;
                Enemy.DefensePoints--;
            }
        }
        //turn next
        gameState = GameState.turnFight;
        StartCoroutine(Fight());
    }
    IEnumerator Fight(){
        //4 shoot player and enemy
        playerAnimator.Play("Shoot");
        //delay 3 seconds animation work 2 seconds 
        yield return new WaitForSeconds(3);
        enemyAnimator.Play("Shoot");
        yield return new WaitForSeconds(3);

        //cheat result player
        for (int i = 0; i < Player.PowerZoneAttacks.Count; i++)
        {
            if(Player.PowerZoneAttacks[i] > 0 && !Enemy.PowerZoneDefense[i]){
                Enemy.Hp -= Player.PowerZoneAttacks[i];
                //Debug.Log("Enemy HP" + Enemy.Hp);
            }
        }
        if(Enemy.Hp>0){
            for (int i = 0; i < Player.PowerZoneAttacks.Count; i++)
            {
                if(Enemy.PowerZoneAttacks[i] > 0 && !Player.PowerZoneDefense[i]){
                    Player.Hp -= Enemy.PowerZoneAttacks[i];
                //Debug.Log("Player Hp " + Player.Hp);

                }
            }
        }

        //next turn
        if(Player.Hp > 0 && Enemy.Hp > 0 ){
            //next round
            gameState = GameState.turnPlayer;
            Player.PowerZoneAttacks.Clear();
            Enemy.PowerZoneAttacks.Clear();
            Player.PowerZoneDefense.Clear();
            Enemy.PowerZoneDefense.Clear();
            uIManager.StartTactic();
        }else{
            //win or fail
            gameState = GameState.startGame;
            string win;
            if (Player.Hp > Enemy.Hp)
            {
                win = "Player";
                enemyAnimator.Play("Dead");
            }else{
                win = "Enemy";
                playerAnimator.Play("Dead");
            }
            uIManager.Victory(win);
        }
        yield return null;
    }



    
}
